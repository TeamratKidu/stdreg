public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<UserContext>(options =>
            options.UseInMemoryDatabase("UserDb"));
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
