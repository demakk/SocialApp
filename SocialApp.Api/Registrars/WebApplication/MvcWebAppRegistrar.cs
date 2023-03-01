using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace SocialApp.Registrars.WebApplication;

public class MvcWebAppRegistrar : IWebApplicationRegistrar
{
    public void RegisterPipelineComponents(Microsoft.AspNetCore.Builder.WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.ApiVersion.ToString());
            }
        });
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}