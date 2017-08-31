using System.Web.Http;
using WebActivatorEx;
using Voyage.Api;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using System.Collections.Generic;
using System;
using System.Xml.XPath;
using System.Runtime.InteropServices;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Voyage.Api
{
    public class SwaggerConfig
    {

        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr LoadLibrary(string lpFileName);

        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Voyage.Api");
                    c.OperationFilter<AddRequiredHeaderParameter>();
                    c.IncludeXmlComments(GetApiProjectXmlCommentsPath());
                    if (CheckLibrary("Voyage.Api.UserManager"))
                        c.IncludeXmlComments(GetUserManagerXmlCommentsPath());
                })
                .EnableSwaggerUi(c => {});
        }

        private static bool CheckLibrary(string fileName)
        {
            return LoadLibrary(fileName) == IntPtr.Zero;
        }

        private static string GetApiProjectXmlCommentsPath()
        {
            return string.Format(@"{0}\XmlComments.xml",
                System.AppDomain.CurrentDomain.BaseDirectory);
        }

        private static string GetUserManagerXmlCommentsPath()
        {
            return string.Format(@"{0}\..\..\Voyage.Api.UserManager\XmlComments.xml",
                System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }

    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();
            var headerParam = new List<Parameter>();

            headerParam.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                type = "string",
                description = "Bearer JWT",
                required = true
            });

            headerParam.AddRange(operation.parameters);

            operation.parameters = headerParam;
        }
    }
}