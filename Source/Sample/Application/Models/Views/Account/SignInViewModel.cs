using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace Application.Models.Views.Account
{
	public class SignInViewModel
	{
		#region Properties

		public virtual IList<AuthenticationScheme> AuthenticationSchemes { get; } = new List<AuthenticationScheme>();
		public virtual SignInForm Form { get; set; } = new SignInForm();

		#endregion
	}
}