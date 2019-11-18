using Guestbook.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Guestbook
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		readonly string MyCorsPolicy = "MyCorsPolicy";

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(MyCorsPolicy,
				builder =>
				{
					builder
						.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod(); ;
				});
			});

			services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
				.AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));
			services.AddControllers();

			services.AddDbContext<GuestBookContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("GuestbookDatabase")));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors(MyCorsPolicy);

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetRequiredService<GuestBookContext>();
				context.Database.EnsureCreated();
			}
		}
	}
}
