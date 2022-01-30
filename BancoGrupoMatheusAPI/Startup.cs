using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BancoGrupoMatheusAPI.DAL;
using BancoGrupoMatheusAPI.Services.Implementations;
using BancoGrupoMatheusAPI.Services.Interfaces;
using BancoGrupoMatheusAPI.Utils;

namespace BancoGrupoMatheusAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<DBContextBancoGrupoMatheus>(options => options.UseSqlServer(Configuration.GetConnectionString("NetCoreOpenBankngConnection")));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IServiceTransacao, TransacaoService>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Banco Grupo Matheus",
                    Version = "v1",
                    Description = "Teste grupo Matheus",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Felipe Marchini",
                        Email = "felipemarchini@icloud.com",
                        Url = new System.Uri("https://github.com/Feemarchini?tab=repositories")
                    }
                }); ;
            });
            //add automappeerr
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var prefix = string.IsNullOrEmpty(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{prefix}/swagger/v1/swagger.json", "Banco Grupo Matheus");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
