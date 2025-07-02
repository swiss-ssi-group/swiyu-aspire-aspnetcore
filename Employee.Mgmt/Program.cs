using Employee.Mgmt.Services;

namespace Employee.Mgmt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<CreateVerificationPresentation>();
            builder.Services.AddScoped<CreateIssuer>();

            builder.Services.AddHttpClient();
            builder.Services.AddOptions();
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();
            app.MapControllers();

            app.Run();
        }
    }
}
