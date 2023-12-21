﻿using Plugin.InAppBilling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeasPrep.Models
{
    public class InAppBillingProduct
    {
        private object title;

        public InAppBillingProduct(string productId, string description, string name, string localizedPrice)
        {
            ProductId = productId;
            Description = description;
            Name = name;
            LocalizedPrice = localizedPrice;
        }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Product ID or sku
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Localized Price (not including tax)
        /// </summary>
        public string LocalizedPrice { get; set; }

        /// <summary>
        /// ISO 4217 currency code for price. For example, if price is specified in British pounds sterling is "GBP".
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Price in micro-units, where 1,000,000 micro-units equal one unit of the 
        /// currency. For example, if price is "€7.99", price_amount_micros is "7990000". 
        /// This value represents the localized, rounded price for a particular currency.
        /// </summary>
        public Int64 MicrosPrice { get; set; }

        /// <summary>
        /// Extra information for apple platforms
        /// </summary>
        public InAppBillingProductAppleExtras AppleExtras { get; set; } = null;
        /// <summary>
        /// Extra information for Android platforms
        /// </summary>
        public InAppBillingProductAndroidExtras AndroidExtras { get; set; } = null;
        /// <summary>
        /// Extra information for Windows platforms
        /// </summary>
        public InAppBillingProductWindowsExtras WindowsExtras { get; set; } = null;

    }
}
