namespace SocialApp.Registrars.WebApplication;
using Microsoft.AspNetCore.Builder;

public interface IWebApplicationRegistrar : IRegistrar
{
    public void RegisterPipelineComponents(WebApplication app);
}