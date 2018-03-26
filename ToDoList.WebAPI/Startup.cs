namespace ToDoList.WebAPI
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                options => options.WithOrigins("http://localhost:3000/").AllowAnyMethod()
            );

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(ExceptionMessage.RequestCouldntBeHandled);
            });
        }
    }
}
