using Microsoft.AspNetCore.Authentication.Cookies;

namespace Application.Models.Web.Authentication
{
	public static class AuthenticationDefaults
	{
		#region Fields

		public const string DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		public const string DefaultSignInScheme = "Remote";

		#endregion
	}
}