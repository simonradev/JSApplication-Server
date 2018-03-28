namespace ToDoList.WebAPI
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using ToDoList.DAL;
    using ToDoList.DAL.Concrete;
    using ToDoList.WebAPI.Constants;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            services.AddSingleton<INotesRepository, NotesInMemoryContext>();
        }

        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env, 
                              IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            string allowedOrigin = configuration[ConfigurationKey.AllowedOrigin];
            app.UseCors(
                options => options
                            .WithOrigins(allowedOrigin)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
            );

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(ExceptionMessage.RequestCouldntBeHandled);
            });
        }
    }
}
