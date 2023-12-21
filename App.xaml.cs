using MblexApp.Models;
using MblexApp.Services;
using Plugin.InAppBilling;

namespace MblexApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        protected override void OnStart()
        {
            // Handle when your app starts
            // Initialize in-app billing
           
            // Attempt automatic login
            AttemptAutoLogin();
        }

        private void AttemptAutoLogin()
        {
            // Retrieve user settings
            var userSettings = AuthenticationService.GetUserSettings();

            if (userSettings != null && IsLoggedInRecently(userSettings))
            {
                // Auto-login the user
                // Your login logic here
                // For example, set the MainPage of the application to the authenticated view
                MainPage = new AppShell();
            }
            
        }

        private bool IsLoggedInRecently(UserSettings userSettings)
        {
            // Check if the user has logged in within the last 2 days
            var twoDaysAgo = DateTime.Now.AddDays(-2);
            return userSettings.LastLoginTime > twoDaysAgo;
        }
    }
}