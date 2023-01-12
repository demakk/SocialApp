using MediatR;
using Social.Application.UserProfiles.Queries;

namespace SocialApp.Registrars.WebApplicationBuilder;

public class BogardRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(Microsoft.AspNetCore.Builder.WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfiles));
        builder.Services.AddMediatR(typeof(GetAllUserProfiles));
    }
}