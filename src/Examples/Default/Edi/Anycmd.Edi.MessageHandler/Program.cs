
namespace Anycmd.Edi.MessageHandler
{
    using Application;
	using Ef;
	using Engine.Edi;
	using Engine.Host;
	using Engine.Host.Edi.Handlers;
	using Engine.Host.Edi.Handlers.Execute;
	using Engine.Host.Impl;
	using Exceptions;
	using Logging;
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Reflection;
	using System.Security.Principal;
	using System.Threading;

	/// <summary>
	/// <remarks>
	/// 设计不能在本体库服务器之外运行本程序
	/// </remarks>
	/// </summary>
	class Program
	{
		private static AcDomain acDomain;
		static void Main(string[] args)
		{
			//程序没有运行
			try
			{
				// 当前用户是管理员的时候，直接启动应用程序，如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
				WindowsIdentity identity = WindowsIdentity.GetCurrent();
				var principal = new WindowsPrincipal(identity);
				if (principal.IsInRole(WindowsBuiltInRole.Administrator))
				{
					Run();
				}
				else
				{
					//创建启动对象
					var startInfo = new System.Diagnostics.ProcessStartInfo();
					//设置运行文件
					startInfo.FileName = Assembly.GetExecutingAssembly().Location;
					//设置启动参数
					startInfo.Arguments = String.Join(" ", args);
					//设置启动动作,确保以管理员身份运行
					startInfo.Verb = "runas";
					//如果不是管理员，则启动UAC
					System.Diagnostics.Process.Start(startInfo);
				}
			}
			catch (Exception ex)
			{
				acDomain.LoggingService.Error(ex);
				Console.WriteLine(ex.Message);
				Console.WriteLine("按任意键退出");
				Console.ReadKey();
			}
		}

		#region Run
		private static void Run()
		{
			Console.WriteLine("正在启动服务...");
			#region boot
			EfContext.InitStorage(new SimpleEfContextStorage());
			// 环境初始化
			acDomain = new DefaultAcDomain();
			acDomain.AddService(typeof(ILoggingService), new Log4NetLoggingService(acDomain));
			acDomain.AddService(typeof(IAcSessionStorage), new SimpleAcSessionStorage());
			acDomain.Init();
			acDomain.RegisterRepository(new List<string>
			{
				"EdiEntities",
				"AcEntities",
				"InfraEntities",
				"IdentityEntities"
			}, typeof(AcDomain).Assembly);
			acDomain.RegisterEdiCore();

			string processId = ConfigurationManager.AppSettings["ProcessId"];
			ProcessDescriptor process;
			if (!acDomain.NodeHost.Processs.TryGetProcess(new Guid(processId), out process))
			{
				throw new AnycmdException("非法的分发器标识" + processId);
			}
			bool createdNew;
			// 使用本体标识从而限制一个本体只能有一个执行器进程
			string globalGuid = string.Format("Global\\{0}_Executor", process.Process.OntologyId);
			var mutex = new Mutex(true, globalGuid, out createdNew);
			if (!createdNew)
			{
				//程序正在运行
				Console.WriteLine("绑定到本本体的程序已在运行……");
				Console.WriteLine("无需重复启动。按任意键退出本窗口");
				Console.ReadKey();
				return;
			}
			#endregion

			IExecutor executor = acDomain.GetRequiredService<IExecutorFactory>().CreateExecutor(process);
			Console.Title = process.Title;
			if (process.IsRuning())
			{
				//程序正在运行
				Console.WriteLine("绑定到本本体的程序已在运行……");
				Console.WriteLine("无需重复启动。按任意键退出本窗口");
				Console.ReadKey();
				return;
			}

			var serviceHost = new ServiceSelfHost(acDomain, process);
			serviceHost.Init();
			string words = string.Format("监听地址：{0}\n", process.WebApiBaseAddress)
				+ "命令提示：开始:start 停止:stop 退出:exit 帮助:help";
			Console.WriteLine(words);

			AtachEventMethod(executor);
			executor.Start();

			#region 控制
			bool isRuning = false;
			while (true)
			{
				string arg = "";
				arg = Console.ReadLine().Trim().ToLower();

				switch (arg)
				{
					case "start":
						executor.Start();
						isRuning = true;
						break;
					case "stop":
						executor.Stop();
						isRuning = false;
						break;
					case "exit":
						executor.Stop();
						isRuning = false;
						Environment.Exit(0);
						break;
					case "?":
					case "/?":
					case "help":
						Console.WriteLine(words);
						if (isRuning)
						{
							Console.WriteLine("运行中...");
						}
						break;
					case "":
						break;
					default:
						Console.WriteLine("不支持的命令");
						break;
				}
			}
			#endregion
		}
		#endregion

		private static void AtachEventMethod(IExecutor executor)
		{
			executor.Starting += executor_Starting;
			executor.Stopping += executor_Stopping;
			executor.Stoped += executor_Stoped;
			executor.Executing += executor_Executing;
			executor.Executed += executor_Executed;
			executor.Sleepping += executor_Sleepping;
			executor.Waked += executor_Waked;
			executor.Error += executor_Error;
		}

		#region event methods
		static void executor_Waked(object sender, EventArgs e)
		{
			Console.WriteLine("醒来" + DateTime.Now.ToString());
		}

		private static void executor_Starting(object sender, EventArgs e)
		{
			Console.WriteLine("开始工作");
		}

		private static void executor_Stopping(object sender, StoppingEventArgs e)
		{
			Console.WriteLine("正在停止...");
		}

		private static void executor_Stoped(object sender, EventArgs e)
		{
			Console.WriteLine("停止工作");
		}

		private static void executor_Executing(object sender, ExecutingEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine();
			Console.Write("----------------------------------");
			Console.Write("执行开始");
			Console.Write("----------------------------------");
			Console.WriteLine();
		}

		private static void executor_Executed(object sender, ExecutedEventArgs e)
		{
			var executor = sender as IExecutor;
			if (executor == null)
			{
				throw new AnycmdException();
			}
			Console.ForegroundColor = ConsoleColor.Green;
			if (!e.Command.Result.IsSuccess)
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}
			Console.Write("结果:");
			Console.WriteLine(e.Command.Result.Description);
			if (!e.Command.Result.IsSuccess)
			{
				Console.ForegroundColor = ConsoleColor.Green;
			}
			Console.WriteLine(string.Format("开始执行时间:{0}", e.ExecutingOn.ToString()));
			Console.WriteLine(string.Format(
				"完成执行时间:{0}\n本次执行耗时长:{1}",
				e.ExecutedOn.ToString(), e.TimeSpan.ToString()));
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(string.Format(
				"累计执行{0}条，成功{1}条，失败{2}条",
				executor.SucessCount + executor.FailCount, executor.SucessCount, executor.FailCount));
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("----------------------------------");
			Console.Write("执行结束");
			Console.Write("----------------------------------");
		}

		private static void executor_Sleepping(object sender, SleepingEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine();
			Console.WriteLine(string.Format("没有待执行命令！休眠{0}秒...", (e.SleepTimeSpan / 1000).ToString()));
		}

		private static void executor_Error(object sender, ExceptionEventArgs e)
		{
			if (e.Exception != null)
			{
				Console.WriteLine(e.Exception.Message);
				if (!e.ExceptionHandled)
				{
					acDomain.LoggingService.Error(e.Exception);
				}
			}
		}
		#endregion
	}
}

