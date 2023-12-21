
using MblexApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace MblexApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
                    
        }
        

        private void OnCounterClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new HomePage());
        }
    }
}