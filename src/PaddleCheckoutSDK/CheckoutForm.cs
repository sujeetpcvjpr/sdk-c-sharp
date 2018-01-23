using System;
using System.Windows.Forms;
using PaddleCheckoutSDK;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;

namespace PaddleCheckoutSDK
{
    /// <summary>
    /// CheckoutForm is a standard WinForms form showing the Paddle Checkout website in the Checkout Control.
    /// The control handles the checkout process and provides feedback via events.
    /// </summary>
    public partial class CheckoutForm : Form, ICheckout
    {
        #region Public Properties and Event
        /// <summary>
        /// Set label for completing purchase loading circle
        /// </summary>
        [DefaultValue("Completing Purchase")]
        public string CompletingPurchaseText { set { CheckoutView.CompletingPurchaseText = value; } }

        /// <summary>
        /// Approximate time to wait for transaction processing to complete. Default 60secs.
        /// </summary>
        [DefaultValue(60)]
        public int TimeOut { set { CheckoutView.TimeOut = value; } private get { return CheckoutView.TimeOut; } }

        /// <summary>
        /// Set Vendor ID obtained from Vendor Dashboard. Required.
        /// </summary>
        public string VendorID { set { CheckoutView.VendorID = value; } }
        /// <summary>
        /// Application Name as setup on Vendor Dashboard. Required.
        /// </summary>
        public string AppName { set { CheckoutView.AppName = value; } }
        /// <summary>
        /// Set Product ID obtained from Vendor Dashboard. Required.
        /// </summary>
        public string ProductID { set { CheckoutView.ProductID = value; } }

        public string AppVersion { set { CheckoutView.AppVersion = value; } }
        public string SDKVersion { set { CheckoutView.SDKVersion = value; } }
        /// <summary>
        /// Gets License Code 
        /// </summary>
        public string LicenseCode { get { return CheckoutView.LicenseCode; } }
        public string LockerID { get { return CheckoutView.LockerID; } }
        /// <summary>
        /// Gets Order ID assigned by paddle
        /// </summary>
        public string OrderID { get { return CheckoutView.OrderID; } }

        /// <summary>
        /// Email as submitted into web form 
        /// </summary>
        public string UserSubmittedEmail { set { CheckoutView.UserSubmittedEmail = value; } }

        /// <summary>
        /// ProcessStatus contains all information returned about transaction. Null until TransactionComplete==true
        /// </summary>
        public ProcessStatus ProcessStatus { get { return CheckoutView.ProcessStatus; } }

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
        /// Fires if an error is thrown during checkout processing. Not required but recommended to identify unexpected conditions.
        /// </summary>
        public event TranactionErrorEventHandler TransactionErrorEvent;
        /// <summary>
        /// Fires if the small X icon on checkout web pages is pressed. Implement handler to close user interface. 
        /// </summary>
        public event CheckoutClosedEventHandler CheckoutClosed;
        /// <summary>
        /// Fires when user presses Continue button email and location pages of the Checkout web site
        /// </summary>
        public event PageSumbittedEventHandler PageSubmitted;


        /// <summary>
        /// CheckoutControl exposes the browse control as a WinForms user control instance
        /// </summary>
        public CheckoutControl CheckoutControl { get { return CheckoutView; } }

         /// <summary>
        /// Fills email into web form and skips display of email form
        /// </summary>  
        public string PreFilledEmail {  set => ((ICheckout)CheckoutControl).PreFilledEmail = value; }
        /// <summary>
        /// Fills Post/Zip into web form and skips display of email form
        /// </summary>
        public string PreFilledPostCode {  set => ((ICheckout)CheckoutControl).PreFilledPostCode = value; }
        /// <summary>
        /// Fills country code into web form and skips display of country select form
        /// </summary>
        public string PreFilledCountryCode {  set => ((ICheckout)CheckoutControl).PreFilledCountryCode = value; }
        /// <summary>
        /// Fills Pass Through data
        /// </summary>
        public string PassThroughData {  set => ((ICheckout)CheckoutControl).PassThroughData = value; }
        /// <summary>
        /// Fills coupon data into web form
        /// </summary>
        public string PreFilledCoupon {  set => ((ICheckout)CheckoutControl).PreFilledCoupon = value; }
        /// <summary>
        /// Sets array of PriceOverride structures containing multiple prices
        /// </summary>
        public PriceOverride[] Prices { set => ((ICheckout)CheckoutControl).Prices = value;  }
      
        #endregion

        public CheckoutForm()
        {
            InitializeComponent();

            CheckoutView.TransactionCompleteEvent += WebBrowser_TransactionCompleteEvent;
            CheckoutView.TransactionBeginEvent += WebBrowser_TransactionBeginEvent;
            CheckoutView.CheckoutClosed += WebBrowser_CheckoutClosed;
            CheckoutView.TransactionErrorEvent += WebBrowser_TransctionErrorEvent;
            CheckoutView.PageSubmitted += CheckoutView_PageSubmitted;

            CheckoutView.parent = this;
            
        }

        private void CheckoutView_PageSubmitted(object sender,PageSubmittedEventArgs e)
        {
            PageSubmitted?.Invoke(this, e);
        }

        private void WebBrowser_TransctionErrorEvent(object sender, TransactionErrorEventArgs e)
        {
            TransactionErrorEvent?.Invoke(this, e);
        }

        private void WebBrowser_CheckoutClosed(object sender, EventArgs e)
        {
            CheckoutClosed?.Invoke(sender, e);            
            this.Close();
        }

        private void WebBrowser_TransactionBeginEvent(object sender, TransactionBeginEventArgs e)
        {
            TransactionBeginEvent?.Invoke(sender, e);
            ProgressBegin();
        }

        private void WebBrowser_TransactionCompleteEvent(object sender, TransactionCompleteEventArgs e)
        {
            ProgressStop();

            TransactionCompleteEvent?.Invoke(sender, e);

            Close_Form.Enabled = true;
        }

        private void ProgressBegin()
        {
        }

        private void ProgressStop()
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ProgressBegin();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            ProgressStop();
        }

        private void Close_Form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WebBrowser_Load(object sender, EventArgs e)
        {

        }

        private void Form_Load(object sender, EventArgs e)
        {
            if (DesignMode == false)
            {
                CheckoutView.ShowLoadingCircle();
            }
            else
                return;

             if (CheckoutView.WebBrowserVersion.StartsWith("11.") ||
                CheckoutView.WebBrowserVersion.StartsWith("10.") )
            {
                try
                {
                    CheckoutView.StartPurchase();
                }
                catch(Exception )
                {
                    CheckoutView.HideLoadingCircle();
                    throw ;
                }
                finally
                {
                    CheckoutView.HideLoadingCircle();
                }
            }
            else
            {
                CheckoutView.LaunchDefaultBrowser();
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void CheckoutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckoutView.Dispose();
        }

    }



}



