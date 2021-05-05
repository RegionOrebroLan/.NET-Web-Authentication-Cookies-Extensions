using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Controllers
{
	public abstract class SecurityController : Controller
	{
		#region Constructors

		protected SecurityController(IAuthenticationSchemeProvider authenticationSchemeProvider, ILoggerFactory loggerFactory)
		{
			this.AuthenticationSchemeProvider = authenticationSchemeProvider ?? throw new ArgumentNullException(nameof(authenticationSchemeProvider));
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		protected internal virtual IAuthenticationSchemeProvider AuthenticationSchemeProvider { get; }
		protected internal virtual ILogger Logger { get; }

		#endregion

		#region Methods

		protected internal virtual async Task<string> ResolveAndValidateReturnUrlAsync(string returnUrl)
		{
			returnUrl = await this.ResolveReturnUrlAsync(returnUrl);

			if(this.Url.IsLocalUrl(returnUrl))
				return returnUrl;

			var message = $"The return-url \"{returnUrl}\" is invalid.";

			if(this.Logger.IsEnabled(LogLevel.Error))
				this.Logger.LogError(message);

			throw new InvalidOperationException(message);
		}

		protected internal virtual async Task<string> ResolveReturnUrlAsync(string returnUrl)
		{
			return await Task.FromResult(string.IsNullOrEmpty(returnUrl) ? "~/" : returnUrl);
		}

		#endregion
	}
}