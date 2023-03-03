using Social.Application.Services;

namespace SocialApp.Registrars.WebApplicationBuilder;

public class ApplicationServicesRegistrar : IWebApplicationBuilderRegistrar
{
    
    public void RegisterServices(Microsoft.AspNetCore.Builder.WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IdentityService>();
    }
}