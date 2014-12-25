
namespace Anycmd.Web.Mvc
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public sealed class FormatJsonResult : ActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        public Object Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FormatJsonResult()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";

            var sw = new StringWriter();
            var settings = new JsonSerializerSettings
            {
                Converters = new JsonConverter[] { 
                    new IsoDateTimeConverter() },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Include
            };
            JsonSerializer serializer = JsonSerializer.Create(settings);

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                serializer.Serialize(jsonWriter, Data);
            }
            response.Write(sw.ToString());
        }
    }
}
