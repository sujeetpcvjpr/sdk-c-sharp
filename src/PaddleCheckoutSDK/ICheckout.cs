using System.Collections.Generic;

namespace PaddleCheckoutSDK
{
    interface ICheckout
    {
        /// <summary>
        /// Fills email into web form and skips display of email form
        /// </summary>
         string PreFilledEmail {  set; }
        /// <summary>
        /// Fills Post/Zip into web form and skips display of email form
        /// </summary>
         string PreFilledPostCode {  set; }
        /// <summary>
        /// Fills country code into web form and skips display of country select form
        /// </summary>
         string PreFilledCountryCode {  set; }
        /// <summary>
        /// Fills Pass Through data
        /// </summary>
         string PassThroughData {  set; }
        /// <summary>
        /// Fills coupon data into web form
        /// </summary>
         string PreFilledCoupon {  set; }
        /// <summary>
        /// Sets array of PriceOverride structures containing multiple prices
        /// </summary>
        PriceOverride[] Prices { set;  }
       
        /// <summary>
        /// Email as submitted into web form 
        /// </summary>
        string UserSubmittedEmail { set; }

        /// <summary>
        /// TransactionCompletedEvent is fired when payment accepted and processing is finished. Control users must provide a handler implementation  for this event.
        /// EventArgs includes a block of text containing information about the transaction  to be used by vendor
        /// </summary>
        event TranactionCompleteEventHandler TransactionCompleteEvent;

        /// <summary>
        /// TransactionBeginEvent is fired after the checkout process has started and opening web page has loaded. Control users can optimally provide a handler.
        /// </summary>
         event TranactionBeginEventHandler TransactionBeginEvent;

        /// <summary>
        /// Fires if an error is thrown during checkout processing. Not required but recommended to identify unexpected conditions.
        /// </summary>
         event TranactionErrorEventHandler TransactionErrorEvent;

        /// <summary>
        /// Fires if the small X icon on checkout web pages is pressed. Implement handler to close user interface. 
        /// </summary>
         event CheckoutClosedEventHandler CheckoutClosed;
        /// <summary>
        /// Fires when user presses Continue button on email and location pages of the Checkout web site
        /// </summary>
        event PageSumbittedEventHandler PageSubmitted;
    }
}
