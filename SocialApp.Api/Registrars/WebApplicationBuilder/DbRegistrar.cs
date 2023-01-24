﻿using Microsoft.EntityFrameworkCore;
using Social.Dal;

namespace SocialApp.Registrars.WebApplicationBuilder;

public class DbRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(Microsoft.AspNetCore.Builder.WebApplicationBuilder builder)
    {
        var cs = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<DataContext>(options => { options.UseSqlServer(cs);});
    }
}