using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegionOrebroLan.DependencyInjection;

namespace RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection
{
	public interface ITicketStoreBuilder
	{
		#region Properties

		IConfiguration Configuration { get; }
		string ConfigurationKey { get; set; }
		IHostEnvironment HostEnvironment { get; }
		IInstanceFactory InstanceFactory { get; }
		IServiceCollection Services { get; }

		#endregion

		#region Methods

		void Configure();

		#endregion
	}
}