
using TeasPrep.Models;
using TeasPrep.Services;
using TeasPrep.StaticMethods;
using TeasPrep.ViewModel;
namespace TeasPrep;

public partial class SubscriptionPlansPage : ContentPage
{
    
    private readonly SubscriptionViewModel viewModel;
    public SubscriptionPlansPage()
    {
        InitializeComponent();
       
        viewModel = new SubscriptionViewModel();
        BindingContext = viewModel;      
    }

    private async void SubscriptionButton_Clicked(object sender, EventArgs e)
    {
        
        //chack if user is logged in, if not take them to login 
        var getUserSettings = AuthenticationService.GetUserSettings();
        //if logged in get user info then do the purchase
        if (getUserSettings != null && getUserSettings.IsLoggedIn)
        {
            bool purchaseSuccessful = await InAppBilling.PurchaseItem("mblexpremium");
            if (purchaseSuccessful)
            {
                await DisplayAlert("Success", "Your subscription has started. Enjoy", "Ok");
                //Save to user on db
                UserManager.UpdatePremiumForUser(getUserSettings.Username, true);
                //update settings
                getUserSettings.IsPremium = true;
                AuthenticationService.SaveUserSettings(getUserSettings);
            }
            else
            {
                await DisplayAlert("Failed", "Transaction failed. Please try again", "Ok");
            }
        }
        else
        {
           var result = await DisplayAlert("Log in", "You must be logged in to subscribe. Would you like to log in first?", "Yes", "No");
            if(result)
            {
                //Navigate to Login page
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await Navigation.PushAsync(new AnatomyPage());
            }
        }
    }
}

