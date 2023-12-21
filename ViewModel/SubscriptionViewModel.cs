
using TeasPrep.Models;
using TeasPrep.Services;
using TeasPrep.StaticMethods;
using Plugin.InAppBilling;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace TeasPrep.ViewModel
{

    public class SubscriptionViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Models.InAppBillingProduct> subscriptionPlans;
        public ObservableCollection<Models.InAppBillingProduct> SubscriptionPlans
        {
            get { return subscriptionPlans; }
            set
            {
                if (subscriptionPlans != value)
                {
                    subscriptionPlans = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }
                  
        public SubscriptionViewModel()
        {          
            SubscriptionPlans = new ObservableCollection<Models.InAppBillingProduct>();
            LoadSubscriptionPlans();           
        }
       
        private async void LoadSubscriptionPlans()
        {
            var productIds = new string[] { "mblexpremium" };
                     
            // Retrieve subscription Plans  from InAppBilling
            subscriptionPlans.Clear();
            IsBusy = true;
            SubscriptionPlans = await InAppBilling.GetProductInfoAsync(productIds);
           
            IsBusy = false;
        }


        // Implement INotifyPropertyChanged interface for property change notification
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }   
}
