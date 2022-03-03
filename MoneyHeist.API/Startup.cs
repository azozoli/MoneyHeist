using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MoneyHeist.API.BackgroudTasks;
using MoneyHeist.API.JsonConverter;
using MoneyHeist.DAL.Repositories;
using MoneyHeist.Models;
using MoneyHeist.Models.Dtos;
using MoneyHeist.Models.Interfaces.IRepositories;
using MoneyHeist.Models.Interfaces.IServices;
using MoneyHeist.Models.Model;
using MoneyHeist.Service.BackgroundTasks;
using MoneyHeist.Service.Mail;
using MoneyHeist.Service.Services;
using MoneyHeist.Service.TaskScheduler;
using System;
using System.IO;
using System.Reflection;

namespace MoneyHeist.API
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
			var builder = new ConfigurationBuilder()
				.SetBasePath( Directory.GetCurrentDirectory() )
				.AddJsonFile( "appsettings.json" )
				.AddEnvironmentVariables();

			var configuration = builder.Build();
			var connectionString = configuration.GetConnectionString( "MoneyHeist" );

			services.Configure<ConnectionSettingsMoneyHeist>( options =>
			{
				options.MoneyHeistConnectionSettings = connectionString;
			} );

			//DBCONTEXT
			services.AddDbContext<MoneyHeistContext>( options => options.UseSqlServer( connectionString ) );

			// REPOS
			services.AddScoped<IMemberRepository, MemberRepository>();
			services.AddScoped<IHeistRepository, HeistRepository>();

			//Services
			services.AddScoped<IMemberService, MemberService>();
			services.AddScoped<IHeistService, HeistService>();

			services.AddScoped<ITaskCreatorService, TaskCreatorService>();
			services.AddSingleton<IBackgroundWorker, BackgroundWorker>();

			services.AddAutoMapper( p => p.AddProfile<MoneyHeistProfile>(), typeof( Startup ) );

			services.AddControllers( options => options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>() )
			.AddJsonOptions( options =>
			{
				//options.JsonSerializerOptions.Converters.Add( new DoubleNullableConverter() );
				//options.JsonSerializerOptions.Converters.Add( new DoubleConverter() );
				options.JsonSerializerOptions.Converters.Add( new DateTimeConverter() );
			} )
			.AddFluentValidation( cfg =>
			{
				cfg.RegisterValidatorsFromAssemblyContaining<MemberDto>();
			} );

			services.AddSwaggerGen( c =>
			{
				c.SwaggerDoc( "v1", new OpenApiInfo { Title = "Adnet.MoneyHeist.API", Version = "v1" } );
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine( AppContext.BaseDirectory, xmlFile );
				c.IncludeXmlComments( xmlPath );
			} );

			services.AddCors( o => o.AddPolicy( "AllRequestPolicy", builder =>
			{
				builder
					.SetIsOriginAllowed( (host) => true )
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials();
			} ) );

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if ( env.IsDevelopment() )
			{
				app.UseDeveloperExceptionPage();
			}

			//app.UseHttpsRedirection();

			app.UseRouting();

			//app.UseAuthorization();
			app.UseSwagger();

			app.UseSwaggerUI( c =>
			{
				c.SwaggerEndpoint( "/swagger/v1/swagger.json", "Adnet.MoneyHeist.API V1" );
				c.RoutePrefix = string.Empty;
			} );

			app.UseCors( "AllRequestPolicy" );

			app.UseEndpoints( endpoints =>
			 {
				 endpoints.MapControllers();
			 } );
		}
	}
}
