using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Models.Web.Authentication;
using Application.Models.Web.Authentication.Extensions;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Controllers
{
	public class AuthenticateController : SecurityController
	{
		#region Constructors

		public AuthenticateController(IAuthenticationSchemeProvider authenticationSchemeProvider, ILoggerFactory loggerFactory) : base(authenticationSchemeProvider, loggerFactory) { }

		#endregion

		#region Methods

		public virtual async Task<IActionResult> Callback()
		{
			// ReSharper disable All

			var authenticateResult = await this.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			if(!authenticateResult.Succeeded)
				throw new InvalidOperationException("Authentication error.", authenticateResult.Failure);

			var returnUrl = await this.ResolveAndValidateReturnUrlAsync(authenticateResult.Properties.Items[AuthenticationKeys.ReturnUrl]);

			var authenticationScheme = authenticateResult.Properties.Items[AuthenticationKeys.Scheme];

			var claims = new List<Claim>();

			foreach(var claim in authenticateResult.Principal.Claims)
			{
				if(string.Equals(claim.Type, JwtClaimTypes.IdentityProvider, StringComparison.OrdinalIgnoreCase))
				{
					claims.Add(new Claim(claim.Type, authenticationScheme, claim.ValueType, claim.Issuer, claim.OriginalIssuer));
					continue;
				}

				claims.Add(claim);
			}

			if(!claims.Any(claim => string.Equals(claim.Type, JwtClaimTypes.IdentityProvider, StringComparison.OrdinalIgnoreCase)))
				claims.Add(new Claim(JwtClaimTypes.IdentityProvider, authenticationScheme));

			var claimsIdentity = (ClaimsIdentity)authenticateResult.Principal.Identity;
			var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, claimsIdentity.AuthenticationType, claimsIdentity.NameClaimType, claimsIdentity.RoleClaimType));

			await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticateResult.Properties);

			return this.Redirect(returnUrl);

			// ReSharper restore All
		}

		protected internal virtual async Task<AuthenticationProperties> CreateAuthenticationPropertiesAsync(string authenticationScheme, string returnUrl)
		{
			var authenticationProperties = new AuthenticationProperties
			{
				RedirectUri = this.Url.Action(nameof(this.Callback))
			};

			authenticationProperties.SetString(AuthenticationKeys.ReturnUrl, returnUrl);
			authenticationProperties.SetString(AuthenticationKeys.Scheme, authenticationScheme);

			return await Task.FromResult(authenticationProperties);
		}

		public virtual async Task<IActionResult> Remote(string authenticationScheme, string returnUrl)
		{
			returnUrl = await this.ResolveAndValidateAsync(authenticationScheme, returnUrl);

			var authenticationProperties = await this.CreateAuthenticationPropertiesAsync(authenticationScheme, returnUrl);

			return this.Challenge(authenticationProperties, authenticationScheme);
		}

		protected internal virtual async Task<string> ResolveAndValidateAsync(string authenticationScheme, string returnUrl)
		{
			if(authenticationScheme == null)
				throw new ArgumentNullException(nameof(authenticationScheme));

			var remoteAuthenticationSchemes = (await this.AuthenticationSchemeProvider.GetRemoteSchemesAsync()).Select(item => item.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

			if(!remoteAuthenticationSchemes.Contains(authenticationScheme))
				throw new ArgumentException($"The authentication-scheme \"{authenticationScheme}\" is invalid.", nameof(authenticationScheme));

			returnUrl = await this.ResolveAndValidateReturnUrlAsync(returnUrl);

			return returnUrl;
		}

		#endregion
	}
}