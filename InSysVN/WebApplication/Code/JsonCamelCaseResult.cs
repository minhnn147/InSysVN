﻿using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Mvc;
using System;
using System.Web;

namespace WebApplication.Code
{
    public class JsonCamelCaseResult: JsonResult
    {
        public JsonCamelCaseResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        public JsonCamelCaseResult(object data, JsonRequestBehavior behavior)
        {
            Data = data;
            JsonRequestBehavior = behavior;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                response.Write(JsonConvert.SerializeObject(Data, Formatting.Indented, jsonSerializerSettings));
            }
        }
    }
}