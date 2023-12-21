
using MblexApp.Models;
using MblexApp.ViewModel;
namespace MblexApp;

public partial class SignupPage : ContentPage
{
   
    public SignupPage()
    {
        InitializeComponent();      
    }


    private void OnSignupClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        // Check if the fields are empty before attempting signup
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            DisplayAlert("Invalid Input", "Username, email, and password cannot be empty", "OK");
            return;
        }

        // Call the CreateUser method
        bool signupSuccess = UserManager.CreateUser(username, email, password);

        if (signupSuccess)
        {
            DisplayAlert("Signup Successful", "User created successfully!", "OK");
            // Optionally navigate to the login page or perform any other action
            App.Current.MainPage = new NavigationPage(new MainPage());
        }
        else
        {
            DisplayAlert("Signup Failed", "Username or email is already in use", "OK");
        }
    }
   

}

