using FamilyTreeNet.Core.Services;
using FamilyTreeNet.Data;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FamilyTreeNet
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // set up auth db
            services.AddDbContext<AuthDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("AuthConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            // require login everywhere, except where explicitly marked with [AllowAnonymous]
            // for access only to specific roles, add [Authorize(Roles = "...")]. Roles to add: admin, editor
            services.AddMvc(config =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();
                    config.Filters.Add(new AuthorizeFilter(policy));
                    config.EnableEndpointRouting = false;
                });

            // https://stackoverflow.com/a/54813987/121309
            services.Configure<CookieTempDataProviderOptions>(options => {
                options.Cookie.IsEssential = true;
            });

            FamilyTree.Infra.MySql.StartupInfra.ConfigureServices(services, this.Configuration);

            services.AddTransient<TreeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (app is null)
            {
                throw new System.ArgumentNullException(nameof(app));
            }

            if (env is null)
            {
                throw new System.ArgumentNullException(nameof(env));
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            //// app.UseSession();

            app.UseRouting();
            app.UseMvc();

            app.UseEndpoints(endpoints => endpoints.MapRazorPages());
        }
    }
}
