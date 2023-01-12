using SocialApp.Options;

namespace SocialApp.Registrars.WebApplicationBuilder;

public class SwaggerRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(Microsoft.AspNetCore.Builder.WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
    }
}