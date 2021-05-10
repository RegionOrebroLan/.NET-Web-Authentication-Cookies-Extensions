using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RegionOrebroLan;
using RegionOrebroLan.Caching.Distributed.Builder.Extensions;
using RegionOrebroLan.Caching.Distributed.DependencyInjection.Extensions;
using RegionOrebroLan.DataProtection.Builder.Extensions;
using RegionOrebroLan.DataProtection.DependencyInjection.Extensions;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.Extensions;
using RegionOrebroLan.Security.Cryptography;
using RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection.Extensions;

namespace Application
{
	public class Startup
	{
		#region Constructors

		public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
		{
			this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.HostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
		}

		#endregion

		#region Properties

		protected internal virtual IConfiguration Configuration { get; }
		protected internal virtual IHostEnvironment HostEnvironment { get; }

		#endregion

		#region Methods

		public virtual void Configure(IApplicationBuilder applicationBuilder)
		{
			if(applicationBuilder == null)
				throw new ArgumentNullException(nameof(applicationBuilder));

			applicationBuilder
				.UseDeveloperExceptionPage()
				.UseDataProtection()
				.UseDistributedCache()
				.UseStaticFiles()
				.UseRouting()
				.UseAuthentication()
				.UseAuthorization()
				.UseEndpoints(endpoints =>
				{
					endpoints.MapDefaultControllerRoute();
				});
		}

		public virtual void ConfigureServices(IServiceCollection services)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			AppDomain.CurrentDomain.SetDataDirectory(Path.Combine(this.HostEnvironment.ContentRootPath, "Data"));
			JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

			var instanceFactory = new InstanceFactory();
			services.AddDataProtection(this.CreateCertificateResolver(), this.Configuration, this.HostEnvironment, instanceFactory);
			services.AddDistributedCache(this.Configuration, this.HostEnvironment, instanceFactory);
			services.AddTicketStore(this.Configuration, this.HostEnvironment, instanceFactory);

			services
				.AddAuthentication(options =>
					{
						options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
						options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
						options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
					}
				)
				.AddCookie(options =>
				{
					options.LoginPath = "/Account/SignIn";
					options.LogoutPath = "/Account/SignOut";
				})
				.AddOpenIdConnect("Google", options =>
				{
					options.Authority = "https://accounts.google.com";
					options.CallbackPath = "/signin-google";
					options.ClientId = "260174815090-v4u9lb79btv3pbss9tk9qupvqq0voo7s.apps.googleusercontent.com";
					options.Scope.Add("email");
					options.TokenValidationParameters = new TokenValidationParameters
					{
						NameClaimType = "name",
						RoleClaimType = "role"
					};
				})
				.AddOpenIdConnect("Identity-Server", options =>
				{
					options.Authority = "https://demo.duendesoftware.com";
					options.CallbackPath = "/signin-identity-server";
					options.ClientId = "login";
					options.RemoteSignOutPath = "/signout-identity-server";
					options.ResponseType = "id_token";
					options.SaveTokens = true;
					options.Scope.Add("email");
					options.SignedOutCallbackPath = "/signout-callback-identity-server";
					options.TokenValidationParameters = new TokenValidationParameters
					{
						NameClaimType = "name",
						RoleClaimType = "role"
					};
				});

			services.AddControllersWithViews();
		}

		protected internal virtual ICertificateResolver CreateCertificateResolver()
		{
			var applicationDomain = new ApplicationHost(AppDomain.CurrentDomain, this.HostEnvironment);
			var fileCertificateResolver = new FileCertificateResolver(applicationDomain);
			var storeCertificateResolver = new StoreCertificateResolver();

			return new CertificateResolver(fileCertificateResolver, storeCertificateResolver);

			//var services = new ServiceCollection();

			//services.AddSingleton(AppDomain.CurrentDomain);
			//services.AddSingleton<FileCertificateResolver>();
			//services.AddSingleton(this.HostEnvironment);
			//services.AddSingleton<IApplicationDomain, ApplicationHost>();
			//services.AddSingleton<ICertificateResolver, CertificateResolver>();
			//services.AddSingleton<StoreCertificateResolver>();

			//return services.BuildServiceProvider().GetRequiredService<ICertificateResolver>();
		}

		#endregion
	}
}