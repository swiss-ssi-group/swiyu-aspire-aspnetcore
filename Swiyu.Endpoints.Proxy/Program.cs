using Swiyu.Endpoints.Proxy;
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddReverseProxy()
    .LoadFromMemory(YarpConfigurations.GetRoutes(), 
        YarpConfigurations.GetClusters(builder.Configuration["SwiyuIssuerMgmtUrl"]!, 
            builder.Configuration["SwiyuVerifierMgmtUrl"]!));

//builder.Services.AddReverseProxy()
//    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapDefaultEndpoints();

//app.UseAuthorization();
//app.UseAuthentication();

app.MapReverseProxy();

app.UseStaticFiles();

app.Run();

// https://localhost:7009/.well-known/openid-configuration