using System;
using System.Text;

namespace PaddleCheckoutSDK
{
    /// <summary>
    /// EventArgs sent in TransactionComplete Event after transaction is complete.  
    /// </summary>
    public class TransactionCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// ProductID used in checkout
        /// </summary>
        public string ProductID { get; set; }
        /// <summary>
        /// Email entered by user into login page
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// Country code entered by user in location page
        /// </summary>
        public string UserCountry { get; set; }
        /// <summary>
        /// Paddle generated license code
        /// </summary>
        public string LicenseCode { get; set; }
        /// <summary>
        /// Paddle generate Order ID
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// Structure containing transaction details
        /// </summary>
        public ProcessStatus ProcessStatus { get; set; }
    }

    /// <summary>
    /// EventArgs sent in TransactionBegin Event.
    /// </summary>
    public class TransactionBeginEventArgs : EventArgs
    {
    }

    /// <summary>
    /// EventArgs sent in PageSubmitted Event.
    /// </summary>
    public class PageSubmittedEventArgs : EventArgs
    {
        /// <summary>
        /// Identifies checkout page
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// Email entered in login page
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        ///Country entered into location page
        /// </summary>
        public string UserContry { get; set; }
        /// <summary>
        /// Transaction id
        /// </summary>
        public string ID { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("PageName: {0}", PageName + Environment.NewLine);
            sb.AppendFormat("ID: {0}" , ID + Environment.NewLine); 
            sb.AppendFormat("UserEmail: {0}", UserEmail + Environment.NewLine);
            sb.AppendFormat("UserCountry: {0}", UserContry + Environment.NewLine);
            sb.AppendFormat("URL: {0}", Url + Environment.NewLine);
            return sb.ToString();
        }
    }

    /// <summary>
    /// EventArgs sent in TransactionError Event. Contains error message string.
    /// </summary>
    public class TransactionErrorEventArgs : EventArgs
    {
        public string Error { get; set; }
    }

    public delegate void TranactionCompleteEventHandler(object sender, TransactionCompleteEventArgs e);
    public delegate void TranactionBeginEventHandler(object sender, TransactionBeginEventArgs e);
    public delegate void TranactionErrorEventHandler(object sender, TransactionErrorEventArgs e);
    public delegate void CheckoutClosedEventHandler(object sender, EventArgs e);
    public delegate void PageSumbittedEventHandler(object sender, PageSubmittedEventArgs e);


}
