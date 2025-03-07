using HotelManagementSystem.Core.Interfaces;
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

        private static ServiceProvider ConfigureServices(ServiceCollection services)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = configBuilder.Build();

            // Register configuration
            services.AddSingleton(configuration);

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