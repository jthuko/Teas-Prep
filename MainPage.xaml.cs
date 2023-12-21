
using TeasPrep;
using TeasPrep.Services;
using TeasPrep.StaticMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using Plugin.InAppBilling;

namespace TeasPrep
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
       
        public MainPage()
        {
            InitializeComponent();
            // Initialize in-app billing
                     
        }       
       
        private async void OnSignupClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new SignupPage());
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string usernameOrEmail = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Add your authentication logic here
            if (LoginUser(usernameOrEmail, password))
            {
               await DisplayAlert("Login Successful", "Welcome, " + usernameOrEmail + "!", "OK");
                // Navigate to the Shell page named "YourShellPage"
                await Shell.Current.GoToAsync("//AnatomyPage");
            }
            else
            {
                await DisplayAlert("Login Failed", "Invalid username or password", "OK");
            }
        }
        private bool LoginUser(string usernameOrEmail, string password)
        {
            // Replace this with your actual authentication logic using the LoginUser method
            return UserManager.LoginUser(usernameOrEmail, password);
        }


    }
}