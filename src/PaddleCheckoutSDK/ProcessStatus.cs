using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaddleCheckoutSDK
{
    /// <summary>
    /// Member of ProcessStatus structure 
    /// </summary>
    public struct CheckOut
    {
        [JsonProperty("checkout_id")]
        public string CheckoutID { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }

    /// <summary>
    /// Member of ProcessStatus structure 
    /// </summary>
    public struct Completed
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("timezone_type")]
        public int TimezoneType { get; set; }
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }

    /// <summary>
    /// Member of ProcessStatus structure 
    /// </summary>
    public struct Order
    {
        [JsonProperty("order_id")]
        public int OrderID { get; set; }
        [JsonProperty("total")]
        public double Total { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("formatted_total")]
        public string FormattedTotal { get; set; }
        [JsonProperty("completed")]
        public Completed Completed { get; set; }
        [JsonProperty("receipt_url")]
        public string ReceiptUrl { get; set; }
        [JsonProperty("has_locker")]
        public bool HasLocker { get; set; }
        [JsonProperty("customer")]
        public Customer Customer { get; set; }
        [JsonProperty("is_subscription")]
        public bool IsSubscription { get; set; }

    }

    internal struct CheckoutLoadedCheckout
    {
        [JsonProperty("id")]
        public string ID;
        [JsonProperty("passthrough")]
        public string PassThrough;
    }
    internal struct CheckoutLoadedProduct
    {
        [JsonProperty("id")]
        public string ID;
        [JsonProperty("name")]
        public string Name;

    }
    internal struct CheckoutLoadedUser
    {
        [JsonProperty("country")]
        public string Country;
        [JsonProperty("email")]
        public string Email;
        [JsonProperty("id")]
        public string ID;
    }

    internal struct CheckoutLoaded
    {
        [JsonProperty("event")]
        public string EventType;
        [JsonProperty("checkout")]
        public CheckoutLoadedCheckout Checkout;
        [JsonProperty("product")]
        public CheckoutLoadedProduct Product;
        [JsonProperty("user")]
        public CheckoutLoadedUser User;
    }



    /// <summary>
    /// Member of ProcessStatus structure 
    /// </summary>
    public struct Customer
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    /// <summary>
    /// Member of ProcessStatus structure 
    /// </summary>
    public struct Locker
    {
        [JsonProperty("locker_id")]
        public int LockerID { get; set; }
        [JsonProperty("product_id")]
        public int ProductID { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        [JsonProperty("license_code")]
        public string LicenseCode { get; set; }
        [JsonProperty("download")]
        public string Download { get; set; }
        [JsonProperty("instructions")]
        public string Instructions { get; set; }
    }

    /// <summary>
    /// Price Override structure. Passed to Prices property when doing multiple over rides
    /// </summary>
    public struct PriceOverride
    {
        [JsonProperty("currency")]
        public string Currency;
        [JsonProperty("price")]
        public string Price;
        /// <summary>
        /// Authorization hash required to change prices
        /// </summary>
        [JsonProperty("auth")]
        public string Authorization;
    }

    /// <summary>
    /// Information returned when transaction fully processed
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public struct ProcessStatus
    {
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("checkout")]
        public CheckOut Checkout { get; set; }
        [JsonProperty("order")]
        public Order Order { get; set; }
        [JsonProperty("lockers")]
        public List<Locker> Lockers { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Order Id = {0}{1}", Order.OrderID, Environment.NewLine);
            sb.AppendFormat("Order Total = {0}{1}", Order.Total, Environment.NewLine);
            sb.AppendFormat("Order Currency = {0}{1}", Order.Currency, Environment.NewLine);
            sb.AppendFormat("Order Customer Email = {0}{1}", Order.Customer.Email, Environment.NewLine);
            sb.AppendFormat("Order Receipt Url = {0}{1}", Order.ReceiptUrl, Environment.NewLine);
            sb.AppendFormat("Order Completed Date = {0}{1}", Order.Completed.Date.ToString(), Environment.NewLine);
            if (Order.HasLocker == true)
            {
                sb.AppendFormat("Locker ID = {0}{1}", Lockers[0].LockerID, Environment.NewLine);
                sb.AppendFormat("License Code = {0}{1}", Lockers[0].LicenseCode, Environment.NewLine);
                sb.AppendFormat("Product ID = {0}{1}", Lockers[0].ProductID, Environment.NewLine);
                sb.AppendFormat("Product Name = {0}{1}", Lockers[0].ProductName, Environment.NewLine);
                sb.AppendFormat("Instructions = {0}{1}", Lockers[0].Instructions, Environment.NewLine);
                sb.AppendFormat("Download = {0}{1}", Lockers[0].Download, Environment.NewLine);
            }

            return sb.ToString();
        }

        public string ProductID { get { return (Order.HasLocker) ? Lockers[0].ProductID.ToString() : null; } }
        public string CustomerEmail { get { return Order.Customer.Email; } }
        public string LicenseCode { get { return (Order.HasLocker) ? Lockers[0].LicenseCode:null; } }
        public string LockerID {  get { return (Order.HasLocker) ? Lockers[0].LockerID.ToString(): null; } }
        public string OrderID { get { return Order.OrderID.ToString(); } }

        internal ProcessStatus MapResponse(string json)
        {
            this = JsonConvert.DeserializeObject<ProcessStatus>(json);

            return this;
        }

        internal static CheckoutLoaded GetCheckoutLoaded(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            CheckoutLoaded coutLoaded = JsonConvert.DeserializeObject<CheckoutLoaded>(json, settings);
            return coutLoaded;
        }

    }
}
