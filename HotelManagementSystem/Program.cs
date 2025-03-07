/*using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Services;
using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Context;
using HotelManagementSystem.Data.Repositories;
using HotelManagementSystem.UI.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelManagementSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            
            // Handle unhandled exceptions
            Application.ThreadException += (sender, e) => 
                ShowExceptionDialog("Thread Exception", e.Exception);
            
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => 
                ShowExceptionDialog("Unhandled Exception", e.ExceptionObject as Exception);

            try
            {
                var services = new ServiceCollection();
                var serviceProvider = ConfigureServices(services);

                // Initialize database
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
                    //Console.WriteLine(dbContext);
                    DatabaseInitializer.Initialize(dbContext);
                }

                // Run the main form
                using (var mainForm = serviceProvider.GetRequiredService<MainForm>())
                {
                    Application.Run(mainForm);
                }
                //Application.Run(new Test());
            }
            catch (Exception ex)
            {
                ShowExceptionDialog("Startup Exception", ex);
            }
        }

        /*private static ServiceProvider ConfigureServices(ServiceCollection services)
        {
            // Configuration
            IConfiguration configuration;
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            // Check if appsettings.json exists, create it if it doesn't
            if (!File.Exists("appsettings.json"))
            {
                // Create default appsettings.json
                var defaultConfig = @"{
                  ""ConnectionStrings"": {
                    ""DefaultConnection"": ""Host=localhost;Database=HotelManagementDB;Username=postgres;Password=Sangareba1@""
                  }
                }";
                Console.WriteLine(defaultConfig);
                File.WriteAllText("appsettings.json", defaultConfig);
            }

            // Add configuration file
            configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = configBuilder.Build();

            // Register configuration
            services.AddSingleton<IConfiguration>(configuration);

            // Database context
            services.AddDbContext<HotelDbContext>(options => 
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                // Enable sensitive data logging in development
                #if DEBUG
                options.EnableSensitiveDataLogging();
                #endif
            });

            // Repositories
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();

            // Services
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IGuestService, GuestService>();

            // Forms
            services.AddTransient<MainForm>();
            services.AddTransient<ReservationForm>();
            services.AddTransient<GuestForm>();

            return services.BuildServiceProvider();
        }
        
        private static ServiceProvider ConfigureServices(ServiceCollection services)
        {
            // Chaîne de connexion définie en dur (à adapter selon votre configuration)
            string connectionString = "Host=localhost;Database=HotelManagementDB;Username=postgres;Password=Sangareba1@";

            // Database context
            services.AddDbContext<HotelDbContext>(options => 
            {
                options.UseNpgsql(connectionString);
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            });

            // Repositories
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();

            // Services
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IGuestService, GuestService>();

            // Forms
            services.AddTransient<MainForm>();
            services.AddTransient<ReservationForm>();
            services.AddTransient<GuestForm>();

            return services.BuildServiceProvider();
        }


        private static void ShowExceptionDialog(string title, Exception exception)
        {
            if (exception == null)
                return;

            string message = $"{exception.Message}\n\nStack Trace:\n{exception.StackTrace}";
            if (exception.InnerException != null)
            {
                message += $"\n\nInner Exception:\n{exception.InnerException.Message}";
            }
            Console.WriteLine(message);
            MessageBox.Show(message, $"Error: {title}", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}*/

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Services;
using HotelManagementSystem.Data.Context;
using HotelManagementSystem.Data.Repositories;
using HotelManagementSystem.Data;
using HotelManagementSystem.UI.Forms;

namespace HotelManagementSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            
            // Handle unhandled exceptions
            Application.ThreadException += (sender, e) => 
                ShowExceptionDialog("Thread Exception", e.Exception);
            
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => 
                ShowExceptionDialog("Unhandled Exception", e.ExceptionObject as Exception);

            try
            {
                var services = new ServiceCollection();
                // Configure services but DO NOT build the provider yet
                ConfigureServices(services);
                
                // Create the service provider
                using (var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = false }))
                {
                    // Initialize database asynchronously
                    RunAsync(async () => 
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
                            await DatabaseInitializer.InitializeAsync(dbContext);
                        }
                    }).GetAwaiter().GetResult();

                    // Create a scope for the MainForm and run it
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mainForm = scope.ServiceProvider.GetRequiredService<MainForm>();
                        Application.Run(mainForm);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowExceptionDialog("Startup Exception", ex);
            }
        }

        // Helper method to run async code from a synchronous context
        private static async Task RunAsync(Func<Task> task)
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in async operation: {ex.Message}");
                throw;
            }
        }
        
        private static void ConfigureServices(ServiceCollection services)
        {
            // Chaîne de connexion définie en dur
            string connectionString = "Host=localhost;Database=HotelManagementDB;Username=postgres;Password=Sangareba1@";

            // Add DbContext with proper options
            services.AddDbContext<HotelDbContext>(options => 
            {
                options.UseNpgsql(connectionString);
                
                #if DEBUG
                options.EnableSensitiveDataLogging();
                #endif
            });

            // Repositories - register as scoped
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IGuestRepository, GuestRepository>();

            // Services - register as scoped
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IGuestService, GuestService>();

            // Forms - also register as scoped (not transient!)
            services.AddScoped<MainForm>();
            services.AddScoped<ReservationForm>();
            services.AddScoped<GuestForm>();
        }

        private static void ShowExceptionDialog(string title, Exception exception)
        {
            if (exception == null)
                return;

            string message = $"{exception.Message}\n\nStack Trace:\n{exception.StackTrace}";
            if (exception.InnerException != null)
            {
                message += $"\n\nInner Exception:\n{exception.InnerException.Message}";
            }
            Console.WriteLine(message);
            MessageBox.Show(message, $"Error: {title}", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}