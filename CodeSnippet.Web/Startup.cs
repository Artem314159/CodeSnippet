using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using CodeSnippet.Services.Contacts;
using CodeSnippet.Services.Groups;
using CodeSnippet.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeSnippet.Web
{
    public class Startup
    {
        public IContainer Container { get; private set; }

        protected virtual void Setup(ContainerBuilder builder)
        {
            builder.RegisterType<GroupService>().As<IGroupService>();
            builder.RegisterType<GroupCreateService>().As<IGroupCreateService>();
            builder.RegisterType<ContactService>().As<IContactService>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Create the Autofac container builder.
            var builder = new ContainerBuilder();

            builder.Populate(services);

            Setup(builder);

            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
