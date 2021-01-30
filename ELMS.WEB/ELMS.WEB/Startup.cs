using ELMS.WEB.Managers.Email.Concrete;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Managers.Equipment.Concrete;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.Loan.Concrete;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Repositories.Email.Concrete;
using ELMS.WEB.Repositories.Email.Interface;
using ELMS.WEB.Repositories.Equipment.Concrete;
using ELMS.WEB.Repositories.Equipment.Interfaces;
using ELMS.WEB.Repositories.Identity.Concrete;
using ELMS.WEB.Repositories.Identity.Interface;
using ELMS.WEB.Repositories.Loan.Concrete;
using ELMS.WEB.Repositories.Loan.Interface;
using ELMS.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ELMS.WEB
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
            services.AddControllersWithViews();
            services.AddRazorPages();

            // Manager
            services.AddScoped<IEquipmentManager, EquipmentManager>();
            services.AddScoped<INoteManager, NoteManager>();
            services.AddScoped<ILoanManager, LoanManager>();
            services.AddScoped<ILoanEquipmentManager, LoanEquipmentManager>();
            services.AddScoped<IEmailTemplateManager, EmailTemplateManager>();
            services.AddScoped<IEmailScheduleManager, EmailScheduleManager>();

            // Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<ILoanEquipmentRepository, LoanEquipmentRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IEmailScheduleRepository, EmailScheduleRepository>();

            services.AddTransient<IApplicationEmailSender, ApplicationEmailSender>();
            services.Configure<SendGridEmailSenderOptions>(options =>
            {
                options.SendGridKey = Configuration["SendGrid:ApiKey"];
                options.SenderEmail = Configuration["SendGrid:SenderEmail"];
                options.SenderName = Configuration["SendGrid:SenderName"];
            });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
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
    }
}