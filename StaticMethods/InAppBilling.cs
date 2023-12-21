using MblexApp.Models;
using Plugin.InAppBilling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MblexApp.StaticMethods
{
    public static class InAppBilling
    {
        public static async Task<ObservableCollection<Models.InAppBillingProduct>> GetProductInfoAsync(string[] productIds)
        {
            var productsCollection = new ObservableCollection<Models.InAppBillingProduct>();
            var connected = await CrossInAppBilling.Current.ConnectAsync();
            if (connected)
            {
                // Check if there are pending orders, if so then subscribe
                var purchasesInfo = await CrossInAppBilling.Current.GetProductInfoAsync(ItemType.Subscription, productIds);
                foreach (var item in purchasesInfo)
                {
                    //item info here.
                    // Assuming InAppBillingProduct has a constructor that takes relevant parameters
                    var product = new Models.InAppBillingProduct(item.ProductId, item.Description, item.Name, item.LocalizedPrice /* other parameters */);

                    // Add the product to the list
                    productsCollection.Add(product);
                }
                CrossInAppBilling.Current.DisconnectAsync();
                return productsCollection;
            }
            else
            {
                return productsCollection;
            }
        }
        public static async Task<bool> PurchaseItem(string productId)
        {
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync();
                if (!connected)
                {
                    // we are offline or can't connect, don't try to purchase
                    return false;
                }

                // check purchases
                var purchase = await billing.PurchaseAsync(productId, ItemType.Subscription);

                // possibility that a null came through.
                if (purchase == null)
                {
                    // did not purchase
                    return false;
                }
                else if (purchase.State == PurchaseState.Purchased)
                {
                    // only needed on android unless you turn off auto finalize
                    var ack = await CrossInAppBilling.Current.FinalizePurchaseAsync(purchase.TransactionIdentifier);

                    // Handle if acknowledge was successful or not

                }

                // Add a return statement here
                return true;
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                // Billing Exception handle this based on the type
                Debug.WriteLine("Error: " + purchaseEx);
                //DisplayAlert("Login Failed", "Invalid username or password", "OK");
                return false;
            }
            catch (Exception ex)
            {
                // Something else has gone wrong, log it
                Debug.WriteLine("Issue connecting: " + ex);
                return false;
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            // Add a return statement here
           // return false;
        }
        public static async Task<bool> WasItemPurchased(string productId)
        {
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync();

                if (!connected)
                {
                    //Couldn't connect
                    return false;
                }

                //check purchases
                var idsToNotFinish = new List<string>(new[] { "myconsumable" });

                var purchases = await billing.GetPurchasesAsync(ItemType.InAppPurchase);

                //check for null just in case
                if (purchases?.Any(p => p.ProductId == productId) ?? false)
                {
                    //Purchase restored
                    // if on Android may be good to check if these purchases need to be acknowledge
                    return true;
                }
                else
                {
                    //no purchases found
                    return false;
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Billing Exception handle this based on the type
                Debug.WriteLine("Error: " + purchaseEx);
            }
            catch (Exception ex)
            {
                //Something has gone wrong
                return false;
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return false;
        }
        public static async Task<bool> CancelSubscription(string subscriptionProductId)
        {
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync();
                if (!connected)
                {
                    // Unable to connect, handle accordingly
                    return false;
                }

                // Retrieve existing subscriptions
                var subscriptions = await billing.GetPurchasesAsync(ItemType.Subscription);

                // Find the subscription to cancel
                var subscriptionToCancel = subscriptions.FirstOrDefault(sub => sub.ProductId == subscriptionProductId);

                if (subscriptionToCancel != null)
                {
                    // Attempt to cancel the subscription
                    // Subscription found, handle cancellation using platform-specific mechanisms
                    // For Android, you would typically direct users to the Google Play Store for cancellation
                    // On iOS, users manage subscriptions through the App Store

                    // You might also want to provide guidance to users on how to cancel through your app's UI

                    // Return true to indicate that the process was initiated
                    return true;
                }
                else
                {
                    // Subscription not found, handle accordingly
                    Debug.WriteLine("Subscription not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine("Error: " + ex);
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return false;
        }


    }
}
