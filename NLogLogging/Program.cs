using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Fluent;

namespace NLogLogging
{
	class Program
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();
		
		static void Main(string[] args)
		{
			var logger = LogManager.GetCurrentClassLogger();
			try
			{
				var servicesProvider = BuildDi();

				using (servicesProvider as IDisposable)
				{
					// var address = ConfigurationManager.AppSettings["Addr"];
					// var glSourceInput = ConfigurationManager.AppSettings["GlSourceInput"];
					NLog.LogManager.Configuration.Variables["address"] = "http://:12213/gelf";
					NLog.LogManager.Configuration.Variables["gl_source_input"] = "";
					
					Logger.Warn("Person {name} spoke {text}", "Name", "text");
					Logger.Error("Person {name} spoke {text}", "Name", "text");
					Logger.Info("Person {name} spoke {text}", "Name", "text");
					Console.ReadKey();
				}
			}
			catch (Exception ex)
			{
				// NLog: catch any exception and log it.
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
				LogManager.Shutdown();
			}
		}
		private static IServiceProvider BuildDi()
		{
			return new ServiceCollection()
				   //Add DI Classes here
				   .AddLogging(loggingBuilder =>
				   {
					   // configure Logging with NLog
					   loggingBuilder.ClearProviders();
					   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
					   loggingBuilder.AddNLog();
				   })
				   .BuildServiceProvider();
		}
	}
}
