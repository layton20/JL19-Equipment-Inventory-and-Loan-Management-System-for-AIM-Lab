using Azure.Storage.Blobs;
using ELMS.WEB.Jobs;
using ELMS.WEB.Managers.Admin.Concrete;
using ELMS.WEB.Managers.Admin.Interfaces;
using ELMS.WEB.Managers.Email.Concrete;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Managers.Equipment.Concrete;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.General.Concrete;
using ELMS.WEB.Managers.General.Interface;
using ELMS.WEB.Managers.Loan.Concrete;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Repositories.Admin.Concrete;
using ELMS.WEB.Repositories.Admin.Interfaces;
using ELMS.WEB.Repositories.Email.Concrete;
using ELMS.WEB.Repositories.Email.Interface;
using ELMS.WEB.Repositories.Equipment.Concrete;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using ELMS.WEB.Repositories.General.Concrete;
using ELMS.WEB.Repositories.General.Interface;
using ELMS.WEB.Repositories.Identity.Concrete;
using ELMS.WEB.Repositories.Identity.Interface;
using ELMS.WEB.Repositories.Loan.Concrete;
using ELMS.WEB.Repositories.Loan.Interface;
using ELMS.WEB.Services;
using ELMS.WEB.Services.Concrete;
using ELMS.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace ELMS.WEB
{
    public class Startup
    {
        private IScheduler _QuartsScheduler;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _QuartsScheduler = ConfigureQuartz();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddRazorPages();

            // Manager
            services.AddScoped<IEquipmentManager, EquipmentManager>();
            services.AddScoped<INoteManager, NoteManager>();
            services.AddScoped<ILoanManager, LoanManager>();
            services.AddScoped<ILoanEquipmentManager, LoanEquipmentManager>();
            services.AddScoped<IEmailTemplateManager, EmailTemplateManager>();
            services.AddScoped<IEmailScheduleManager, EmailScheduleManager>();
            services.AddScoped<IEmailScheduleParameterManager, EmailScheduleParameterManager>();
            services.AddScoped<IBlacklistManager, BlacklistManager>();
            services.AddScoped<ILoanExtensionManager, LoanExtensionManager>();
            services.AddScoped<IConfigurationManager, ConfigurationManager>();
            services.AddScoped<IBlobManager, BlobManager>();
            services.AddScoped<IEquipmentBlobManager, EquipmentBlobManager>();

            // Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<ILoanEquipmentRepository, LoanEquipmentRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IEmailScheduleRepository, EmailScheduleRepository>();
            services.AddScoped<IEmailScheduleParameterRepository, EmailScheduleParameterRepository>();
            services.AddScoped<IBlacklistRepository, BlacklistRepository>();
            services.AddScoped<ILoanExtensionRepository, LoanExtensionRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IBlobRepository, BlobRepository>();
            services.AddScoped<IEquipmentBlobRepository, EquipmentBlobRepository>();

            services.AddSingleton(provider => _QuartsScheduler);
            services.AddTransient<EquipmentJob>();
            services.AddTransient<LoanJob>();
            services.AddTransient<EmailJob>();

            services.AddSingleton(x => new BlobServiceClient(Configuration.GetValue<string>("AzureBlobStorage:ConnectionString")));
            services.AddSingleton<IBlobService, BlobService>();

            services.AddTransient<IApplicationEmailSender, ApplicationEmailSender>();
            services.Configure<SendGridEmailSenderOptions>(options =>
            {
                options.SendGridKey = Configuration["SendGrid:ApiKey"];
                options.SenderEmail = Configuration["SendGrid:SenderEmail"];
                options.SenderName = Configuration["SendGrid:SenderName"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateLoanPolicy", policy => policy.RequireClaim("Create Loan", "true"));
                options.AddPolicy("EditLoanPolicy", policy => policy.RequireClaim("Edit Loan", "true"));
                options.AddPolicy("DeleteLoanPolicy", policy => policy.RequireClaim("Delete Loan", "true"));
                options.AddPolicy("ViewLoanPolicy", policy => policy.RequireClaim("View Loan", "true"));

                options.AddPolicy("CreateEquipmentPolicy", policy => policy.RequireClaim("Create Equipment", "true"));
                options.AddPolicy("EditEquipmentPolicy", policy => policy.RequireClaim("Edit Equipment", "true"));
                options.AddPolicy("DeleteEquipmentPolicy", policy => policy.RequireClaim("Delete Equipment", "true"));
                options.AddPolicy("ViewEquipmentPolicy", policy => policy.RequireClaim("View Equipment", "true"));

                options.AddPolicy("CreateNotePolicy", policy => policy.RequireClaim("Create Note", "true"));
                options.AddPolicy("EditNotePolicy", policy => policy.RequireClaim("Edit Note", "true"));
                options.AddPolicy("DeleteNotePolicy", policy => policy.RequireClaim("Delete Note", "true"));
                options.AddPolicy("ViewNotePolicy", policy => policy.RequireClaim("View Note", "true"));

                options.AddPolicy("CreateEmailSchedulePolicy", policy => policy.RequireClaim("Create EmailSchedule", "true"));
                options.AddPolicy("EditEmailSchedulePolicy", policy => policy.RequireClaim("Edit EmailSchedule", "true"));
                options.AddPolicy("DeleteEmailSchedulePolicy", policy => policy.RequireClaim("Delete EmailSchedule", "true"));
                options.AddPolicy("ViewEmailSchedulePolicy", policy => policy.RequireClaim("View EmailSchedule", "true"));

                options.AddPolicy("CreateUserPolicy", policy => policy.RequireClaim("Create User", "true"));
                options.AddPolicy("EditUserPolicy", policy => policy.RequireClaim("Edit User", "true"));
                options.AddPolicy("DeleteUserPolicy", policy => policy.RequireClaim("Delete User", "true"));
                options.AddPolicy("ViewUserPolicy", policy => policy.RequireClaim("View User", "true"));

                options.AddPolicy("CreateConfigurationPolicy", policy => policy.RequireClaim("Create Configuration", "true"));
                options.AddPolicy("EditConfigurationPolicy", policy => policy.RequireClaim("Edit Configuration", "true"));
                options.AddPolicy("DeleteConfigurationPolicy", policy => policy.RequireClaim("Delete Configuration", "true"));
                options.AddPolicy("ViewConfigurationPolicy", policy => policy.RequireClaim("View Configuration", "true"));

                options.AddPolicy("CreateLoanExtensionPolicy", policy => policy.RequireClaim("Create LoanExtension", "true"));

                options.AddPolicy("ViewReportPolicy", policy => policy.RequireClaim("View Report", "true"));
                options.AddPolicy("FilterReportPolicy", policy => policy.RequireClaim("Filter Report", "true"));

                options.AddPolicy("CreateEmailTemplatePolicy", policy => policy.RequireClaim("Create EmailTemplate", "true"));
                options.AddPolicy("EditEmailTemplatePolicy", policy => policy.RequireClaim("Edit EmailTemplate", "true"));
                options.AddPolicy("DeleteEmailTemplatePolicy", policy => policy.RequireClaim("Delete EmailTemplate", "true"));
                options.AddPolicy("ViewEmailTemplatePolicy", policy => policy.RequireClaim("View EmailTemplate", "true"));

                options.AddPolicy("CreateEmailSchedulePolicy", policy => policy.RequireClaim("Create EmailSchedule", "true"));
                options.AddPolicy("EditEmailSchedulePolicy", policy => policy.RequireClaim("Edit EmailSchedule", "true"));
                options.AddPolicy("DeleteEmailSchedulePolicy", policy => policy.RequireClaim("Delete EmailSchedule", "true"));
                options.AddPolicy("ViewEmailSchedulePolicy", policy => policy.RequireClaim("View EmailSchedule", "true"));
                options.AddPolicy("SendEmailSchedulePolicy", policy => policy.RequireClaim("Send EmailSchedule", "true"));

                options.AddPolicy("ViewEmailLogPolicy", policy => policy.RequireClaim("View EmailLog", "true"));

                options.AddPolicy("RunJobSchedulerPolicy", policy => policy.RequireClaim("Run JobScheduler", "true"));

                options.AddPolicy("ViewCalendarPolicy", policy => policy.RequireClaim("View Calendar", "true"));

                options.AddPolicy("CreateBlacklistPolicy", policy => policy.RequireClaim("Create Blacklist", "true"));
                options.AddPolicy("EditBlacklistPolicy", policy => policy.RequireClaim("Edit Blacklist", "true"));
                options.AddPolicy("DeleteBlacklistPolicy", policy => policy.RequireClaim("Delete Blacklist", "true"));
                options.AddPolicy("ViewBlacklistPolicy", policy => policy.RequireClaim("View Blacklist", "true"));

                options.AddPolicy("CreateUserPolicy", policy => policy.RequireClaim("Create User", "true"));
                options.AddPolicy("EditUserPolicy", policy => policy.RequireClaim("Edit User", "true"));
                options.AddPolicy("DeleteUserPolicy", policy => policy.RequireClaim("Delete User", "true"));
                options.AddPolicy("ViewUserPolicy", policy => policy.RequireClaim("View User", "true"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            _QuartsScheduler.JobFactory = new AspNetCoreJobFactory(app.ApplicationServices);
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void OnShutdown()
        {
            if (!_QuartsScheduler.IsShutdown)
            {
                _QuartsScheduler.Shutdown();
            }
        }

        private IScheduler ConfigureQuartz()
        {
            NameValueCollection _Properties = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };

            StdSchedulerFactory _Factory = new StdSchedulerFactory(_Properties);
            IScheduler _Scheduler = _Factory.GetScheduler().Result;

            _Scheduler.Start().Wait();

            return _Scheduler;
        }
    }
}