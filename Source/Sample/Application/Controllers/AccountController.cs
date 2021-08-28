using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Models.Views.Account;
using Application.Models.Web.Authentication;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Controllers
{
	[Authorize]
	public class AccountController : SecurityController
	{
		#region Constructors

		public AccountController(IAuthenticationSchemeProvider authenticationSchemeProvider, ILoggerFactory loggerFactory) : base(authenticationSchemeProvider, loggerFactory) { }

		#endregion

		#region Properties

		protected internal virtual IDictionary<string, string> ValidCredentials { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "Alice", "Alice" },
			{ "Bob", "Bob" }
		};

		#endregion

		#region Methods

		[AllowAnonymous]
		public virtual async Task<IActionResult> AccessDenied()
		{
			return await Task.FromResult(this.View());
		}

		protected internal virtual async Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(string userName)
		{
			if(string.IsNullOrWhiteSpace(userName))
				throw new ArgumentException("The user-name can not be null, empty or only whitespaces.", nameof(userName));

			// We want to have a large ticket.
			var claims = new List<Claim>
			{
				new Claim(JwtClaimTypes.Address, $"{userName}-Address"),
				new Claim(JwtClaimTypes.Email, $"{userName}-Email"),
				new Claim(JwtClaimTypes.FamilyName, $"{userName}-FamilyName"),
				new Claim(JwtClaimTypes.Gender, $"{userName}-Gender"),
				new Claim(JwtClaimTypes.GivenName, $"{userName}-GivenName"),
				new Claim(JwtClaimTypes.Locale, $"{userName}-Locale"),
				new Claim(JwtClaimTypes.MiddleName, $"{userName}-MiddleName"),
				new Claim(JwtClaimTypes.Name, $"{userName}-Name"),
				new Claim(JwtClaimTypes.NickName, $"{userName}-NickName"),
				new Claim(JwtClaimTypes.PhoneNumber, $"{userName}-PhoneNumber"),
				new Claim(JwtClaimTypes.PhoneNumberVerified, $"{userName}-PhoneNumberVerified"),
				new Claim(JwtClaimTypes.Picture, $"{userName}-Picture XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim(JwtClaimTypes.Subject, $"{userName}-Subject"),
				new Claim(JwtClaimTypes.WebSite, $"{userName}-Website"),
				new Claim("Z-Dummy-01", $"{userName}-Z-Dummy-01 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-02", $"{userName}-Z-Dummy-02 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-03", $"{userName}-Z-Dummy-03 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-04", $"{userName}-Z-Dummy-04 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-05", $"{userName}-Z-Dummy-05 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-06", $"{userName}-Z-Dummy-06 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-07", $"{userName}-Z-Dummy-07 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-08", $"{userName}-Z-Dummy-08 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-09", $"{userName}-Z-Dummy-09 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-10", $"{userName}-Z-Dummy-10 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-11", $"{userName}-Z-Dummy-11 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-12", $"{userName}-Z-Dummy-12 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-13", $"{userName}-Z-Dummy-13 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-14", $"{userName}-Z-Dummy-14 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-15", $"{userName}-Z-Dummy-15 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-16", $"{userName}-Z-Dummy-16 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-17", $"{userName}-Z-Dummy-17 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-18", $"{userName}-Z-Dummy-18 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-19", $"{userName}-Z-Dummy-19 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-20", $"{userName}-Z-Dummy-20 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-21", $"{userName}-Z-Dummy-21 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-22", $"{userName}-Z-Dummy-22 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-23", $"{userName}-Z-Dummy-23 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-24", $"{userName}-Z-Dummy-24 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-25", $"{userName}-Z-Dummy-25 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-26", $"{userName}-Z-Dummy-26 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-27", $"{userName}-Z-Dummy-27 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-28", $"{userName}-Z-Dummy-28 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-29", $"{userName}-Z-Dummy-29 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX"),
				new Claim("Z-Dummy-30", $"{userName}-Z-Dummy-30 XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX XXXX-XXXXXXXX-XXXXXX-XXXXXXXX-XXXX")
			};

			return await Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity(claims, "Forms", JwtClaimTypes.Name, JwtClaimTypes.Role)));
		}

		protected internal virtual async Task<SignInViewModel> CreateSignInViewModelAsync(string returnUrl)
		{
			returnUrl = await this.ResolveAndValidateReturnUrlAsync(returnUrl);

			var model = new SignInViewModel
			{
				Form =
				{
					ReturnUrl = returnUrl
				}
			};

			foreach(var authenticationScheme in (await this.AuthenticationSchemeProvider.GetAllSchemesAsync()).Where(authenticationScheme => authenticationScheme.HandlerType != typeof(CookieAuthenticationHandler)))
			{
				model.AuthenticationSchemes.Add(authenticationScheme);
			}

			return model;
		}

		protected internal virtual async Task<SignInViewModel> CreateSignInViewModelAsync(SignInForm form)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

			var model = await this.CreateSignInViewModelAsync(form.ReturnUrl);

			model.Form = form;

			return model;
		}

		public virtual async Task<IActionResult> Index()
		{
			return await Task.FromResult(this.View());
		}

		protected internal virtual async Task ResolveAndValidateSignInFormAsync(SignInForm form)
		{
			if(form == null)
				throw new ArgumentNullException(nameof(form));

			form.ReturnUrl = await this.ResolveAndValidateReturnUrlAsync(form.ReturnUrl);
		}

		[AllowAnonymous]
		public virtual async Task<IActionResult> SignedOut()
		{
			return await Task.FromResult(this.View());
		}

		[AllowAnonymous]
		public virtual async Task<IActionResult> SignIn(string returnUrl)
		{
			var model = await this.CreateSignInViewModelAsync(returnUrl);

			return this.View(model);
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Is validated in another method.")]
		public virtual async Task<IActionResult> SignIn(SignInForm form)
		{
			await this.ResolveAndValidateSignInFormAsync(form);

			if(form.Cancel)
				return this.Redirect(form.ReturnUrl);

			if(this.ModelState.IsValid)
			{
				if(this.ValidateCredentials(form, out var userName))
				{
					await this.HttpContext.SignInAsync(AuthenticationDefaults.DefaultScheme, await this.CreateClaimsPrincipalAsync(userName), new AuthenticationProperties { IsPersistent = form.Persistent });

					return this.Redirect(form.ReturnUrl);
				}

				this.ModelState.AddModelError("InvalidCredentials", "Invalid user-name or password.");
			}

			var model = await this.CreateSignInViewModelAsync(form);

			return await Task.FromResult(this.View(model));
		}

		[AllowAnonymous]
		public virtual async Task<IActionResult> SignOut(object _)
		{
			var authenticationProperties = new AuthenticationProperties { RedirectUri = this.Url.Action("SignedOut") };
			var authenticationShemes = new List<string>
			{
				AuthenticationDefaults.DefaultScheme
			};

			// ReSharper disable All
			if(this.User.Identity.IsAuthenticated)
			{
				var identityProviderClaim = this.User.FindFirst(JwtClaimTypes.IdentityProvider);

				if(identityProviderClaim != null && !string.Equals(identityProviderClaim.Value, "Google", StringComparison.OrdinalIgnoreCase))
					authenticationShemes.Add(identityProviderClaim.Value);
			}
			// ReSharper restore All

			return await Task.FromResult(this.SignOut(authenticationProperties, authenticationShemes.ToArray()));
		}

		[SuppressMessage("Design", "CA1021:Avoid out parameters")]
		protected internal virtual bool ValidateCredentials(SignInForm form, out string userName)
		{
			userName = null;

			foreach(var (key, value) in this.ValidCredentials)
			{
				if(!string.Equals(key, form?.UserName, StringComparison.OrdinalIgnoreCase))
					continue;

				if(!string.Equals(value, form?.Password, StringComparison.Ordinal))
					continue;

				userName = key;
				return true;
			}

			return false;
		}

		#endregion
	}
}