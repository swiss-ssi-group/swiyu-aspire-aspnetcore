using Microsoft.AspNetCore.Mvc.ApplicationParts;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapDefaultEndpoints();

//app.UseHttpsRedirection();

//app.UseAuthorization();
//app.UseAuthentication();

app.MapReverseProxy();

app.UseStaticFiles();

app.Run();

// https://localhost:7009/.well-known/openid-configuration