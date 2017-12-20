using PaddleCheckoutSDK;
using System;
using System.Windows;

namespace WpfControlDemoApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //To show control:
            // Create form instance
            //set required properties
            //load into Control Host

            CheckoutControl checkoutControl = new CheckoutControl();

            //Get credentials from vendor dashboard
            string vendor_id = "12345";
            string app_name = "AppName1";
            string product_id = "123456";

            checkoutControl.VendorID = vendor_id;
            checkoutControl.ProductID = product_id;
            checkoutControl.AppName = app_name;

            //Required
            checkoutControl.TransactionBeginEvent += CheckoutControl_TransactionBeginEvent;
            //optional
            checkoutControl.TransactionErrorEvent += CheckoutControl_TranactionErrorEvent;
            checkoutControl.TransactionCompleteEvent += CheckoutControl_TransactionCompleteEvent;
            checkoutControl.CheckoutClosed += CheckoutControl_CheckoutClosed;

            WBControlHost.Child = checkoutControl;

            // once form set into Control Host call the StartPurchace method

            checkoutControl.StartPurchase();

            // Transaction Information can be retrieved before control disposed
            // The same information is passed in the transaction done event
            ProcessStatus ps = checkoutControl.ProcessStatus;
            string userCountry = checkoutControl.UserCountry;
        }

        private void CheckoutControl_CheckoutClosed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckoutControl_TranactionErrorEvent(object sender, TransactionErrorEventArgs e)
        {
            string err = e.Error;
        }

        private void CheckoutControl_TransactionCompleteEvent(object sender, TransactionCompleteEventArgs e)
        {
            var processed = e.ProcessStatus;
        }

        private void CheckoutControl_TransactionBeginEvent(object sender, TransactionBeginEventArgs e)
        {
        }

    }
}
