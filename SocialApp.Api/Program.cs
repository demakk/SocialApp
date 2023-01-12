using SocialApp.Registrars.WebApplicationBuilder;
using SocialApp.Registrars.WebApplication;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices(typeof(Program));
var app = builder.Build();
app.RegisterPipelineComponents(typeof(Program));
app.Run();