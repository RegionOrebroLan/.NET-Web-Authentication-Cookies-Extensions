using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Application.Models.Web.Authentication.Extensions
{
	public static class AuthenticationSchemeProviderExtension
	{
		#region Methods

		public static async Task<IEnumerable<AuthenticationScheme>> GetRemoteSchemesAsync(this IAuthenticationSchemeProvider authenticationSchemeProvider)
		{
			if(authenticationSchemeProvider == null)
				throw new ArgumentNullException(nameof(authenticationSchemeProvider));

			return (await authenticationSchemeProvider.GetAllSchemesAsync()).Where(authenticationScheme => authenticationScheme.HandlerType != typeof(CookieAuthenticationHandler));
		}

		#endregion
	}
}