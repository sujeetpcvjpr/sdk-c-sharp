using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PaddleCheckoutSDK
{
    public class PaddleCheckout : ICheckout
    {
        #region Events
        /// <summary>
        /// TransactionCompletedEvent is fired when payment accepted and processing is finished. Control users must provide a handler implementation  for this event.
        /// EventArgs includes a block of text containing information about the transaction  to be used by vendor
        /// </summary>
        public event TranactionCompleteEventHandler TransactionCompleteEvent;

        /// <summary>
        /// TransactionBeginEvent is fired after the checkout process has started and opening web page has loaded. Control users can optimally provide a handler.
        /// </summary>
        public event TranactionBeginEventHandler TransactionBeginEvent;

        /// <summary>
        /// Fires if an error is thrown during checkout processing
        /// </summary>
        public event TranactionErrorEventHandler TransactionErrorEvent;

        /// <summary>
        /// Fires if the small X icon on checkout web pages is pressed. Implement handler to close user interface. 
        /// </summary>
        public event CheckoutClosedEventHandler CheckoutClosed;

        /// <summary>
        /// Fires when user presses Continue button on email and location pages of the Checkout web site
        /// </summary>
        public event PageSumbittedEventHandler PageSubmitted;
        #endregion

        #region Properties
        /// <summary>
        /// ProcessStatus contains all information returned about transaction. Null until Transaction Complete.
        /// </summary>
        public ProcessStatus ProcessStatus { get { return checkoutFrom.CheckoutControl.ProcessStatus; } }

        /// <summary>  
        /// Fills email into web form and skips display of email form
        /// </summary>
        public string PreFilledEmail {  set => ((ICheckout)checkoutFrom).PreFilledEmail = value; }
        /// <summary>
        /// Fills Post/Zip into web form and skips display of email form
        /// </summary>
        public string PreFilledPostCode {  set => ((ICheckout)checkoutFrom).PreFilledPostCode = value; }
        /// <summary>
        /// Fills country code into web form and skips display of country select form
        /// </summary>
        public string PreFilledCountryCode {  set => ((ICheckout)checkoutFrom).PreFilledCountryCode = value; }
        /// <summary>
        /// Fills Pass Through data
        /// </summary>
        public string PassThroughData {  set => ((ICheckout)checkoutFrom).PassThroughData = value; }
        /// <summary>
        /// Fills coupon data into web form
        /// </summary>
        public string PreFilledCoupon {  set => ((ICheckout)checkoutFrom).PreFilledCoupon = value; }
        /// <summary>
        /// Sets price over rides
        /// </summary>
        public PriceOverride[] Prices { set => ((ICheckout)checkoutFrom).Prices = value;   }
     
        /// <summary>
        /// Email as submitted into web form 
        /// </summary>
        public string UserSubmittedEmail { set => ((ICheckout)checkoutFrom).UserSubmittedEmail = value; }
       
        #endregion

        /// <summary>
        /// WinForms Form instance. 
        /// </summary>
        public Form Form;

        private CheckoutForm checkoutFrom;
        /// <summary>
        /// Construct PaddleCheckout class
        /// </summary>
        /// <param name="VendorID">Vendor ID obtained from Vendor Dashboard.</param>
        /// <param name="ProductID">Product ID obtained from Vendor Dashboard</param>
        /// <param name="AppName">Application Name as setup on Vendor Dashboard</param>
        public PaddleCheckout(string VendorID, string ProductID, string AppName)
        {
            checkoutFrom = new CheckoutForm
            {
                VendorID = VendorID,
                ProductID = ProductID,
                AppName = AppName
            };

            checkoutFrom.CheckoutControl.TransactionCompleteEvent += WebBrowser_TransactionCompleteEvent;
            checkoutFrom.CheckoutControl.TransactionBeginEvent += WebBrowser_TransactionBeginEvent;
            checkoutFrom.CheckoutControl.CheckoutClosed += WebBrowser_CheckoutClosed;
            checkoutFrom.CheckoutControl.TransactionErrorEvent += WebBrowser_TransctionErrorEvent;
            checkoutFrom.CheckoutControl.PageSubmitted += CheckoutControl_PageSubmitted;

            Form = checkoutFrom as Form;
        }

        private void CheckoutControl_PageSubmitted(object sender, PageSubmittedEventArgs e)
        {
            PageSubmitted?.Invoke(this, e);
        }

        /// <summary>
        /// Show the Windows in modal or non-modal mode
        /// </summary>
        /// <param name="IsDialog">Set True to show window as a dialog box</param>
        public void ShowWindow(bool IsDialog = true)
        {
            if (IsDialog)
            {
                Form.ShowDialog();
            }
            else
                Form.Show();
        }

        private void WebBrowser_TransctionErrorEvent(object sender, TransactionErrorEventArgs e)
        {
            TransactionErrorEvent?.Invoke(this, e);
        }

        private void WebBrowser_CheckoutClosed(object sender, EventArgs e)
        {
            CheckoutClosed?.Invoke(this, e);
        }

        private void WebBrowser_TransactionBeginEvent(object sender, TransactionBeginEventArgs e)
        {
             TransactionBeginEvent?.Invoke(sender, e);
       }

        private void WebBrowser_TransactionCompleteEvent(object sender, TransactionCompleteEventArgs e)
        {
            TransactionCompleteEvent?.Invoke(sender, e);
        }

 


    }
}

