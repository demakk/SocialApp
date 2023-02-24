using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using Social.Application.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SocialApp.Options;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{

    //gives info on the existing versions 
    private readonly IApiVersionDescriptionProvider _provider;
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }
    
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }

        var scheme = GetJwtSecurityScheme();
        options.AddSecurityDefinition(scheme.Reference.Id, scheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {scheme, new string[0]}
        });
    }

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "SocialApp",
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description = "This Api Version has been deprecated";
        }
        return info;
    }

    private OpenApiSecurityScheme GetJwtSecurityScheme()
    {
        return new OpenApiSecurityScheme
        {
            Name = "JWT Authentication",
            Description = "Provide a JWT Bearer",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
    }
}
