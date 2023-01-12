using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace SocialApp.Registrars.WebApplicationBuilder;

public class MvcRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(Microsoft.AspNetCore.Builder.WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            //adds report about versions in the header
            config.ReportApiVersions = true;
            //read version from the url
            config.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        builder.Services.AddVersionedApiExplorer(config =>
        {
            config.GroupNameFormat = "'v'VVV";
            config.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddEndpointsApiExplorer();
    }
}