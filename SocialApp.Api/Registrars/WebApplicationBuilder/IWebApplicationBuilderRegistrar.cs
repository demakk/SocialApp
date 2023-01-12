namespace SocialApp.Registrars.WebApplicationBuilder;
using Microsoft.AspNetCore.Builder;
public interface IWebApplicationBuilderRegistrar : IRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder);
}