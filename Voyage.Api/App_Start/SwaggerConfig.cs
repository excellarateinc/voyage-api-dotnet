using System.Web.Http;
using WebActivatorEx;
using Voyage.Api;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using System.Collections.Generic;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Voyage.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Voyage.Api");
                    c.OperationFilter<AddRequiredHeaderParameter>();
                })
                .EnableSwaggerUi(c => {});
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