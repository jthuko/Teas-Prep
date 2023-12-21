using MblexApp.Services;
using System.Runtime.InteropServices;

namespace MblexApp
{
    public partial class AppShell : Shell
    {
        
        public AppShell()
        {
            InitializeComponent();

            var existingUserSettings = AuthenticationService.GetUserSettings();
            if(existingUserSettings != null && existingUserSettings.IsPremium && existingUserSettings.IsLoggedIn)
            {
                Upgrade.Text = "Unsubscribe";
            }
            else
            {
                Upgrade.Text = "Subscribe";
            } 
            
            if(existingUserSettings != null && existingUserSettings.IsLoggedIn && existingUserSettings.LastLoginTime >= DateTime.Now.AddDays(-3))
            {
                SigninSignout.Text = "Logout";
            }
            else if(existingUserSettings != null && existingUserSettings.IsLoggedIn && existingUserSettings.LastLoginTime < DateTime.Now.AddDays(-3)) 
            {
                //Log user out
                SigninSignout.Text = "Login";
                existingUserSettings.IsLoggedIn = false;
                DisplayAlert("Login Expired", "You have not logged in for more than three days. Please login to your app again", "OK");
                //Navigate to Login Page
                Navigation.PushAsync(new MainPage());
            }
            else
            {
                SigninSignout.Text = "Login";
            }
        }

        private async void Upgrade_Clicked(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if(label != null &&  label.Text == "Unsubscribe")
            {
                //1. Unscubsribe inAppBilling
                //2. set settings to isPremium = false
                //3. Set user.ispremium = false

            }
            else
            {
                //navigate to Subscription page
                await Navigation.PushAsync(new SubscriptionPlansPage());
            }
        }

        private async void SigninSignout_Clicked(object sender, EventArgs e)
        {
            var existingUserSettings = AuthenticationService.GetUserSettings();
            ToolbarItem signinSignout = sender as ToolbarItem;
            if(signinSignout != null && signinSignout.Text == "Login")
            {
                //Navigate to Login page
                await Navigation.PushAsync(new MainPage());
            }
            else if(signinSignout != null && signinSignout.Text == "Logout")
            {
                if (existingUserSettings != null)
                {
                    existingUserSettings.IsLoggedIn = false;
                    existingUserSettings.LastLoginTime = DateTime.Now;
                    AuthenticationService.SaveUserSettings(existingUserSettings);
                }
                bool action = await DisplayAlert("Close App", "Do you want to close the App?", "Yes", "No");
                if(action) 
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        // For iOS
                        System.Environment.Exit(0);
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        // For Android and Windows
                        Application.Current.Quit();
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        // For Windows desktop
                        Application.Current?.CloseWindow(Application.Current.MainPage.Window);
                    }
                }
                else if(!action)
                {
                    // Navigate to the Shell page named "YourShellPage"
                    await Shell.Current.GoToAsync("//AnatomyPage");
                }
            }
        }

        private void Profile_Clicked(object sender, EventArgs e)
        {
            var existingUserSettings = AuthenticationService.GetUserSettings();
            ToolbarItem signinSignout = sender as ToolbarItem;
        }
    }
}