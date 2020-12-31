using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using HangFire.Example.Jobs;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;

namespace HangFire.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Add Hangfire services.
            var hangFireConnectionString = Configuration.GetConnectionString("Hangfire");
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage()

            );
            //.UseSqlServerStorage(hangFireConnectionString, new SqlServerStorageOptions
            //{
            //    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //    QueuePollInterval = TimeSpan.Zero,
            //    UseRecommendedIsolationLevel = true,
            //    DisableGlobalLocks = true
            //})
            services.AddHangfireServer();
            
            
            
            services.AddSingleton<IRecurringJob>(new StartCalculatorJob());
            services.AddSingleton<IRecurringJob>(new StartFileExplorer());
            services.AddSingleton<IRecurringJob>(new AlwaysFailJob());
            services.AddSingleton<IRecurringJob>(new StartChromeJob());


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs, IRecurringJobManager recurringJobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });



            AddHangfireJobs(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }


        private void AddHangfireJobs(IApplicationBuilder app)
        {
            // ADD HangFire UI 
            app.UseHangfireDashboard("/jobs", new DashboardOptions()
            {
                DashboardTitle = "Hangfire Example Job Dashboard",
            });


            var allRecurringJobs = app.ApplicationServices.GetServices<IRecurringJob>();
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            foreach (var job in allRecurringJobs)
            {
                RecurringJob.AddOrUpdate(job.JobName, () => job.ExecuteJob(), job.CronTime,timezone);
            }
        }
    }
}
