/// <reference path="../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../miniui/miniui.js" />
/// <reference path="../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
	window.onLogined = onLogined;
	window.topShowTab = showTab;
	window.onIframeLoaded = onIframeLoaded;

	$().ready(function () {
		if (screen.width > 1366) {
			layout1.expandRegion("east ");
		}
		else {
			layout1.collapseRegion("east");
		}
		// 登录
		onLogined();
		$("#btnAbout").click(function () {
			var node = {};
			node.url = "/Home/About";
			node.text = "关于";
			topShowTab(node);
			return false;
		});
		$("#btnLogout").click(logoutClick);
		$("#btnLogon").click(logonClick);
		$("#btnRefresh").click(refreshTab);
		$("#btnRemoveOthers").click(removeOthers);
		$("#btnRemoveAll").click(removeAll);
		$("#btnRemoveCurrent").click(removeCurrent);
		$("#userContext a[rel]").each(function () {
			// We make use of the .each() loop to gain access to each element via the "this" keyword...
			$(this).qtip(
			{
				content: {
					text: '<img class="throbber" src="/Content/img/throbber.gif" alt="Loading..." />',
					ajax: {
						url: $(this).attr('rel') // Use the rel attribute of each element for the url to load
					},
					title: {
						text: '详细 - ' + $(this).attr('title'),
						button: '关闭'
					}
				},
				position: {
					at: 'bottom center', // Position the tooltip above the link
					my: 'top center',
					viewport: $(window), // Keep the tooltip on-screen at all times
					effect: false // Disable positioning animation
				},
				show: {
					event: 'click',
					solo: true // Only show one tooltip at a time
				},
				hide: 'unfocus',
				style: {
					classes: 'qtip-cellTooltip qtip-tipped qtip-shadow'
				}
			});
		}).click(function (event) { event.preventDefault(); });

		// 顶部菜单按钮点击事件响应方法
		$(".topMenu").click(function () {
			var e = mini.get(this);
			var node = {};
			node.text = e.getTooltip();
			node.url = e.href;
			node.iconCls = e.iconCls + "_16x16";
			showTab(node);
			return false;
		});
		$("#btnAbout").click();
	});

	Date.prototype.toHyphenDateString = function () {
		var year = this.getFullYear();
		var month = this.getMonth() + 1;
		var date = this.getDate();

		if (month < 10) {
			month = "0" + month;
		}
		if (date < 10) {
			date = "0" + date;
		}
		return year + "-" + month + "-" + date;
	};

	mini.parse();

	var btnBlank = mini.get("btnBlank");
	var passwordEditWin = mini.get("Home_Index_Password_passwordEditWin");
	var passwordEditForm = new mini.Form("Home_Index_Password_passwordEditForm");
	var btnChangePassword = mini.get("Home_Index_Password_btnChangePassword");
	$().ready(function () {
		$("#" + passwordEditWin.id + " .Home_Index_Password_btnOk").click(changePassword);
		$("#" + passwordEditWin.id + " .Home_Index_Password_btnCancel").click(function () {
			passwordEditWin.hide()
		});
	});
	btnChangePassword.on("click", function (e) {
		passwordEditWin.show();
	});
	var favoriteMenu = mini.get("favoriteMenu");
	favoriteMenu.on("itemclick", function (e) {
	    var item = e.item;
	    if (e.isLeaf) {
	        var node = {};
	        node.url = item.url;
	        node.text = item.text;
	        if (item.url.substring(0, 7) == "http://") {
	            node.url = item.url;
	        }
	        else {
	            node.url = item.url;
	        }
	        node.img = item.img;
	        topShowTab(node);
	        return true;
	    }
	});

	function changePassword() {
		var data = $("#Home_Index_Password_passwordEditForm").serialize();
		var url = bootPATH + "../Ac/Account/ChangeSelfPassword";
		passwordEditForm.validate();
		if (passwordEditForm.isValid() == false) return;
		if ($("#Home_Index_Password_passwordEditForm input[name='Password']").val()
			!= $("#Home_Index_Password_passwordEditForm input[name='PasswordAgain']").val()) {
			mini.alert("两次输入的密码不一致");
			return;
		}

		$.post(url, data, function (result) {
			helper.response(result, function () {
				passwordEditWin.hide();
				mini.alert("修改成功");
			});
		}, "json");
	}

	var layout1 = mini.get("layout1");
	var leftTree = mini.get("leftTree");
	leftTree.on("nodeclick", nodeclick);
	var mainTabs = mini.get("mainTabs");
	mainTabs.on("activechanged", function (e) {
	    var href;
	    if (e.tab) {
	        href = e.tab._href;
	    }
	    else {
	        href = mainTabs.getActiveTab()._href;
	    }
	    if (href) {
	        btnBlank.setHref(href);
	    }
	});

	function showTab(node) {
		var tabId = "tab$" + node.url;
		var tab = mainTabs.getTab(tabId);
		if (!tab) {
			helper.requesting();
			// 打开的页面太多的情况下IE执行javascript的速度太慢
			if ($.browser.msie) {
				var tabsCount = mainTabs.getTabs().length;
				// TODO:提取配置
				if (tabsCount > 1) {
					removeAll();
				}
			}
			tab = { _href: node.url };
			tab.name = tabId;
			tab.title = node.text;
			tab.showCloseButton = true;
			if (node.img) {
				tab.iconStyle = 'background:url(/content/icons/16x16/' + node.img + ')';
			}
			mainTabs.addTab(tab);
			mainTabs.activeTab(tab);
			var tabBody = $(mainTabs.getTabBodyEl(tab));
			var isInner = false;
			if (isInner) {
				tab.isInner = true;
				tab.refreshPage = function () {
					tabBody.load(node.url, { isInner: true });
				};
				tab.refreshPage();
			}
			else {
				var $iframe = $('<iframe frameborder="0" onload="onIframeLoaded()" style="width:100%;height:100%;" src="' + node.url + '"></iframe>');
				$iframe.appendTo(tabBody);
				tab.$iframe = $iframe;
			}
		}
		else {
			mainTabs.activeTab(tab);
		}
		return true;
	}
	function onIframeLoaded() {
		helper.responsed();
	}
	// 退出
	function logoutClick() {
		mini.confirm("确定要退出登录吗？", "确定退出？", function (action) {
			if (action == "ok") {
				$.post(bootPATH + "../Ac/Account/SignOut", null, function (result) {
					helper.response(result, function () {
						window.location.href = bootPATH + "../Home/LogOn";
					});
				}, "json");
			}
		});
	}
	// 登录
	function logonClick() {
		var loginWindow = mini.get("loginWindow");
		if (loginWindow) {
			loginWindow.show();
		}
	}
	// 关闭其他
	function removeOthers() {
		var currentTab = mainTabs.getActiveTab();
		var firstTab = mainTabs.getTab("first");
		var butTab = [currentTab, firstTab];
		mainTabs.removeAll(butTab);
	}
	// 关闭全部
	function removeAll() {
		var firstTab = mainTabs.getTab("first");
		mainTabs.removeAll(firstTab);
		mainTabs.activeTab(firstTab);
	}
	// 关闭当前
	function removeCurrent() {
		var firstTab = mainTabs.getTab("first");
		var currentTab = mainTabs.getActiveTab();
		if (currentTab != firstTab) {
			mainTabs.removeTab(currentTab);
		}
	}
	// 刷新
	function refreshTab() {
		var tab = mainTabs.getTab(mainTabs.activeIndex);
		if (tab.isInner) {
			helper.requesting();
			tab.refreshPage();
		}
		else {
			if (tab.$iframe) {
				helper.requesting();
				tab.$iframe.attr("src", tab.$iframe.attr("src"));
			}
		}
	}
	function onLogined() {
		if (window.isLogined) {
			var oldAccount = $("#logout #currentAccount").text();
			$.get(bootPATH + "../Ac/Account/GetAccountInfo", null, function (result) {
				if (result.isLogined) {
					// 如果切换了账户则需要刷新菜单和状态，否则不需要
					if (oldAccount != result.loginName) {
						if (result.backColor) {
							$("html").css("background-color", result.backColor);
						}
						if (result.wallpaper) {
							var path = bootPATH + "../Content/img/wallpapers/";
							var url = result.wallpaper;
							if (url && url.substring(0, 7) != "http://") {
								url = path + url;
							}
							wallpaper("#bgimg", url);
						}
						$("#logout #currentAccount").text(result.loginName);
						$("#logout").show();
						$("#logon").hide();
						if (result.menus) {
						    leftTree.loadList(result.menus, "id", "pid");
						    favoriteMenu.loadList(result.menus, "id", "pid");
						}
						$("#roleTemplate").tmpl(result.roles).appendTo("#roleList");
						$("#groupTemplate").tmpl(result.groups).appendTo("#groupList");
					}
				}
				else {
					$("#logout").hide();
					$("#logon").show();
					$("#btnLogon").click();
				}
			}, "json");
		}
	}

	function nodeclick(e) {
		var node = e.node;
		if (!node.url) {
			return false;
		}
		var tab = {};
		tab.text = node.text;
		if (node.url.substring(0, 7) == "http://") {
			tab.url = node.url;
		}
		else {
			tab.url = node.url;
		}
		tab.img = node.img;
		showTab(tab);
		return true;
	}
})(window);
