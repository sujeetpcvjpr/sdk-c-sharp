using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace PaddleCheckoutSDK
{

    internal class CheckoutControlController
    {
        #region private members
        private CheckoutControl checkoutControl;
        private WebBrowser webBrowser;
        private LoadingCircle loadingCircle;
        private Label labelCompleting;

        private string UserID;

        private string webBrowserVersion = "";
        private bool LoadCompleted = false;
        private string transactionid = "";
        private ProcessStatus processStatus;
        private bool IsStaging = false;
        private bool IsDev = false;
        private bool AllErrors = false;
        private bool errorTimeout = false;
        private System.Timers.Timer errorWaitTimer;

        private const string CheckoutCompletePollURL = "https://checkout.paddle.com/api/1.0/order?checkout_id=";
        private const string PaddleSDKInitialPageURL = "https://cdn.paddle.com/sdk/checkout.html?vendor_id={0}&application_name={1}&bundle_identifier={2}&application_version={3}&sdk_version={4}&sdk_platform=windows&passthrough_base64=true";
        private const string Staging_CheckoutCompletePollURL = "https://staging-checkout.paddle.com/api/1.0/order?checkout_id=";
        private const string Staging_PaddleSDKInitialPageURL = "https://cdn.paddle.com/sdk/checkout-staging.html?vendor_id={0}&application_name={1}&bundle_identifier={2}&application_version={3}&sdk_version={4}&sdk_platform=windows&passthrough_base64=true";
        private const string Dev_CheckoutCompletePollURL = "https://dev-checkout.paddle.com/api/1.0/order?checkout_id=";
        private const string Dev_PaddleSDKInitialPageURL = "https://cdn.paddle.com/sdk/checkout-dev.html?vendor_id={0}&application_name={1}&bundle_identifier={2}&application_version={3}&sdk_version={4}&sdk_platform=windows&passthrough_base64=true";
        private const string SimpleCheckoutPageURL = "https://pay.paddle.com/checkout/{0}";
        private const string Staging_SimpleCheckoutPageURL = "https://staging-pay.paddle.com/checkout/{0}";
        private const string Dev_SimpleCheckoutPageURL = "https://staging-pay.paddle.com/checkout/{0}";

        public string WebBrowserVersion { get => webBrowserVersion; set => webBrowserVersion = value; }
        #endregion

        public CheckoutControlController(CheckoutControl checkoutControl)
        {
            this.checkoutControl = checkoutControl;
            this.webBrowser = checkoutControl.WebBrowser;
            this.loadingCircle = checkoutControl.loadingCircle;
            this.labelCompleting = checkoutControl.labelCompleting;
        }

        internal void SetupControl()
        {
            checkoutControl.TimeOut = 60;
            errorWaitTimer = new System.Timers.Timer();
            errorWaitTimer.Elapsed += ErrorWaitTimer_Elapsed;
            errorWaitTimer.Interval = (checkoutControl.TimeOut * 1000) / 2;

            SetOptionsFromConfig();

            webBrowser.IsWebBrowserContextMenuEnabled = true;
            webBrowser.WebBrowserShortcutsEnabled = true;

            webBrowserVersion = webBrowser.Version.ToString();

            loadingCircle.InnerCircleRadius = 9;
            loadingCircle.OuterCircleRadius = 17;
            loadingCircle.SpokeThickness = 3;

            labelCompleting.Top = checkoutControl.Height / 2 - 50;
            labelCompleting.Left = checkoutControl.Width / 2 - (labelCompleting.Width / 2);

            HideLoadingCircle();

            NativeMethods.SuppressCookiePersistence();
            NativeMethods.ChangeUserAgent("PaddleAgent");
            SuppressScriptErrorsOnly(webBrowser);

            webBrowser.Navigating += WebBrowser_Navigating;
            webBrowser.NewWindow += WebBrowser_NewWindow;
        
        }

        private void WebBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }


        private void SetOptionsFromConfig()
        {
            var staging = ConfigurationManager.AppSettings["Staging"] ?? "false";
            var dev = ConfigurationManager.AppSettings["Dev"] ?? "false";
            var allerrors = ConfigurationManager.AppSettings["AllErrors"] ?? "false";

            IsDev = bool.Parse(dev);
            IsStaging = bool.Parse(staging);
            AllErrors = bool.Parse(allerrors);

            var EnableMenus = ConfigurationManager.AppSettings["EnableMenus"] ?? "false";

            if (bool.Parse(EnableMenus) == true)
            {
                webBrowser.IsWebBrowserContextMenuEnabled = true;
                webBrowser.WebBrowserShortcutsEnabled = true;
            }
        }

        // Hides script errors without hiding other dialog boxes.
        private void SuppressScriptErrorsOnly(WebBrowser browser)
        {
            // Ensure that ScriptErrorsSuppressed is set to false.
            browser.ScriptErrorsSuppressed = false;

            // Handle DocumentCompleted to gain access to the Document object.
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Browser_DocumentCompleted);
        }

        /// <summary>
        /// This is where it all begins. StartPurchase loads checkout web page and begins the process of checkout.
        /// </summary>
        public void StartPurchase()
        {
            Debug.WriteLine($"Start Purchase {DateTime.Now.ToString()}");

            errorTimeout = false;

            string bundle_name = System.AppDomain.CurrentDomain.FriendlyName;

            string url;

            if (IsStaging == true)
                url = String.Format(Staging_PaddleSDKInitialPageURL, checkoutControl.VendorID, checkoutControl.AppName, bundle_name, checkoutControl.AppVersion, checkoutControl.SDKVersion);
            else if (IsDev == true)
                url = String.Format(Dev_PaddleSDKInitialPageURL, checkoutControl.VendorID, checkoutControl.AppName, bundle_name, checkoutControl.AppVersion, checkoutControl.SDKVersion);
            else
                url = String.Format(PaddleSDKInitialPageURL, checkoutControl.VendorID, checkoutControl.AppName, bundle_name, checkoutControl.AppVersion, checkoutControl.SDKVersion);

            if (webBrowserVersion.StartsWith("11.") || webBrowserVersion.StartsWith("10.") || webBrowserVersion.StartsWith("9."))
            {
                webBrowser.Navigate(url);
            }
            else
            {
                LaunchDefaultBrowser();
            }
        }

        internal void LaunchDefaultBrowser()
        {
            string url;
            if (IsStaging == true)
                url = String.Format(Staging_SimpleCheckoutPageURL, checkoutControl.ProductID);
            else if (IsDev == true)
                url = String.Format(Dev_SimpleCheckoutPageURL, checkoutControl.ProductID);
            else
                url = String.Format(SimpleCheckoutPageURL, checkoutControl.ProductID);

            Process.Start(url);
        }

        //Return and parameter are mainly here for unit testing
        internal string InvokeScript(bool invoke = true)
        {
            string priceArray = "";

            if (checkoutControl.Prices != null)
            {   
                priceArray = JsonConvert.SerializeObject(checkoutControl.Prices);
            }
            string js = $"Paddle.Checkout.open({{product: {checkoutControl.ProductID} , " +
                $"display_mode: 'sdk'" +
                $" { (string.IsNullOrEmpty(checkoutControl.PreFilledEmail) ? "" : ", email: '" + checkoutControl.PreFilledEmail + "'")}" +
                $" { (string.IsNullOrEmpty(checkoutControl.PreFilledCountryCode) ? "" : ", country: '" + checkoutControl.PreFilledCountryCode + "'")}" +
                $" { (string.IsNullOrEmpty(checkoutControl.PreFilledPostCode) ? "" : ", postcode: '" + checkoutControl.PreFilledPostCode + "'")}" +
                $" { (string.IsNullOrEmpty(checkoutControl.PassThroughData) ? "" : ", passthrough: '" + checkoutControl.PassThroughData + "'")}" +
                $" { (string.IsNullOrEmpty(checkoutControl.PreFilledCoupon) ? "" : ", coupon: '" + checkoutControl.PreFilledCoupon + "'")}" +
                $" { (string.IsNullOrEmpty(checkoutControl.Locale) ? "" : ", locale: '" + checkoutControl.Locale + "'")}" +
                $" { (string.IsNullOrEmpty(priceArray) ? "" : ", prices: " + priceArray + "")}" +
                $" }});";

            //// Make sure the HTML document has loaded before attempting to
            //// invoke script of the document page. 
            if (webBrowser.Document != null)
            {
                if(invoke==true)
                    SendJS(js);
            }

            return js;
        }


        private void SendJS(string JScript)
        {
            object[] args = { JScript };
            webBrowser.Document.InvokeScript("eval", args);
        }


        public void ShowLoadingCircle(bool showLable = false)
        {
            loadingCircle.Active = true;
            loadingCircle.Visible = true;
            labelCompleting.Visible = showLable;
        }


        public void HideLoadingCircle()
        {
            loadingCircle.Active = false;
            loadingCircle.Visible = false;
            labelCompleting.Visible = false;
        }


        private void ErrorWaitTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            checkoutControl.Invoke(new Action(() => { HideLoadingCircle(); }));
            checkoutControl.Invoke(new Action(() => { checkoutControl.SendErrorAsync("Checkout load operation timed out. Possibly an error has occurred opening the Checkout, e.g. Invalid Vendor or Product ID"); }));
            errorWaitTimer.Stop();
            errorWaitTimer.Enabled = false;
        }

     

        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            Debug.WriteLine(checkoutControl.Name + " Navigating " + e.Url.ToString());
            if (e.Url.ToString().Contains(@"paddle://event/?data="))
            {
                e.Cancel = true;

                string decode = HttpUtility.UrlDecode(e.Url.ToString());

                Debug.WriteLine("Query" + e.Url.Query);

                string json = HttpUtility.UrlDecode(e.Url.Query.ToString());

                if (json.Contains("\"id\":"))
                {
                    int id_start = json.IndexOf("\"id\":\"") + ("\"id\":\"").Length;
                    int id_end = json.IndexOf("\",", id_start);

                    transactionid = json.Substring(id_start, id_end - id_start);
                }

                if (json.Contains("Checkout.Login"))
                {
                    Debug.WriteLine("Checkout.Login");
                    errorWaitTimer.Stop();

                    GetFormInfo(e);

                    PageSubmittedEventArgs pageArgs = new PageSubmittedEventArgs()
                    {
                        PageName = "Checkout.Login",
                       UserEmail = checkoutControl.UserSubmittedEmail,
                        ID = UserID
                    };

                    checkoutControl.FirePageSubmittedEvent(pageArgs);
                }

                if (json.Contains("Checkout.Loaded"))
                {
                    Debug.WriteLine("Checkout.Loaded");

                    HideLoadingCircle();
                    errorWaitTimer.Stop();

                    GetFormInfo(e);

                    PageSubmittedEventArgs pageArgs = new PageSubmittedEventArgs()
                    {
                        PageName = "Checkout.Loaded",
                        UserEmail = checkoutControl.UserSubmittedEmail,
                        ID = UserID
                    };

                    checkoutControl.FirePageSubmittedEvent(pageArgs);
                }


                if (json.Contains("Checkout.Location.Submit"))
                {
                    Debug.WriteLine("Checkout.Location.Submit");

                    GetFormInfo(e);

                    PageSubmittedEventArgs pageArgs = new PageSubmittedEventArgs()
                    {
                        PageName = "Checkout.Location.Submit",
                        UserEmail = checkoutControl.UserSubmittedEmail,
                       ID = UserID,
                       UserContry = checkoutControl.UserCountry
                    };

                    checkoutControl.FirePageSubmittedEvent(pageArgs);

                    checkoutControl.FireTransactionBeginEvent();
                }


                if (json.Contains("Checkout.PaymentMethodSelected"))
                {
                    Debug.WriteLine("Checkout.PaymentMethodSelected");

                    GetFormInfo(e);

                    PageSubmittedEventArgs pageArgs = new PageSubmittedEventArgs()
                    {
                        PageName = "Checkout.PaymentMethodSelected",
                        UserEmail = checkoutControl.UserSubmittedEmail,
                        ID = UserID,
                        UserContry = checkoutControl.UserCountry
                    };

                    checkoutControl.FirePageSubmittedEvent(pageArgs);
                }

                //  CheckoutView Navigating https://staging-checkout.paddle.com/pay/11877442-chred3db6271715-9962130049/paypal-complete?display_mode=sdk&token=EC-01B58355K8729915R&PayerID=C4YEL3ASXWBW4


                if (json.Contains("Checkout.PaymentComplete"))
                {
                    Debug.WriteLine("Checkout.PaymentComplete");
                    ShowLoadingCircle(true);
                    PollForIDBackgroundWorker();
                }

                if (json.Contains("Checkout.Complete"))
                {
                    Debug.WriteLine("Checkout Complete");
                    PageSubmittedEventArgs pageArgs = new PageSubmittedEventArgs()
                    {
                        PageName = "Checkout.Complete",
                        UserEmail = checkoutControl.UserSubmittedEmail,
                        ID = UserID,
                        UserContry = checkoutControl.UserCountry
                    };

                    checkoutControl.FirePageSubmittedEvent(pageArgs);


                }

                if (json.Contains("Checkout.Close"))
                {
                    Debug.WriteLine("Checkout.Close");
                    checkoutControl.FireCheckoutCloseEvent();
                }

                if (json.Contains("Checkout.ReturnFromPaypal"))
                {
                    Debug.WriteLine(checkoutControl.Name + " Checkout.ReturnFromPaypal");
           

                }
               
            }

            else if (e.Url.ToString().ToLower().Contains("paypal.com"))
            {
                if (e.Url != null)
                {
                    string sPayPalUrl = e.Url.ToString().ToLower();//.Replace("sdk","direct");
                    if (openPaypalUrl(sPayPalUrl))
                    {
                        PageSubmittedEventArgs pageArgs = new PageSubmittedEventArgs()
                        {
                            PageName = "Checkout.PaypalLoaded.CloseSDK",
                            UserEmail = checkoutControl.UserSubmittedEmail,
                            ID = UserID,
                            UserContry = checkoutControl.UserCountry,
                            Url = sPayPalUrl
                        };

                        checkoutControl.FirePageSubmittedEvent(pageArgs);
                    }
                }

            }
            
        }

    

        private void GetFormInfo(WebBrowserNavigatingEventArgs e)
        {
            string decode = HttpUtility.UrlDecode(e.Url.ToString());
            int id_start = decode.IndexOf("data=") + ("data=").Length;
            int id_end = decode.Length;

            string json = decode.Substring(id_start, id_end - id_start);

            CheckoutLoaded coLoaded = ProcessStatus.GetCheckoutLoaded(json);
            checkoutControl.UserCountry = coLoaded.User.Country;
            checkoutControl.UserSubmittedEmail = coLoaded.User.Email;
            UserID = coLoaded.User.ID;
        }

        private void PollForIDBackgroundWorker()
        {
            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += new DoWorkEventHandler(this.PollForId);
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;

            bw.RunWorkerAsync();
        }

        private void PopulateResponse(string json)
        {
            ProcessStatus ps = new ProcessStatus();
            processStatus = ps.MapResponse(json);

            checkoutControl.ProcessStatus = processStatus;
            checkoutControl.CustomerEmail = ps.CustomerEmail;
            checkoutControl.LicenseCode = ps.LicenseCode;
            checkoutControl.LockerID = ps.LockerID;
            checkoutControl.OrderID = ps.OrderID;
            
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (errorTimeout == true)
            {
                checkoutControl.SendErrorAsync($"Processing Timed Out (TimeOut value =  {checkoutControl.TimeOut})");
                errorTimeout = false;
            }

            HideLoadingCircle();

            TransactionCompleteEventArgs evt = new TransactionCompleteEventArgs
            {
                ProductID = checkoutControl.ProductID,
                UserEmail = checkoutControl.UserSubmittedEmail,
                UserCountry = checkoutControl.UserCountry,
                LicenseCode = checkoutControl.LicenseCode,
                OrderID = checkoutControl.OrderID,
                ProcessStatus = checkoutControl.ProcessStatus
            };
            checkoutControl.TransactionComplete = true;
            checkoutControl.FireTranactionCompleteEvent(evt);
        }

        private void PollForId(object sender, DoWorkEventArgs e)
        {
            bool loop = true;
            int numLoops = 0;

            do
            {
                ++numLoops;
                Thread.Sleep(2000);

                WebClient myClient = new WebClient();

                string url;

                if (IsStaging == true)
                    url = Staging_CheckoutCompletePollURL;
                else if (IsDev == true)
                    url = Dev_CheckoutCompletePollURL;
                else
                    url = CheckoutCompletePollURL;


                string json = "";
                try
                {
                    using (Stream response = myClient.OpenRead(url + transactionid))
                    {
                        StreamReader reader = new StreamReader(response);
                        json = reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);

                    checkoutControl.SendErrorAsync(ex.Message);

                    loop = false;
                }

                if (json.Contains("\"state\":"))
                {
                    int id_start = json.IndexOf("\"state\":\"") + ("\"state\":\"").Length;
                    int id_end = json.IndexOf("\",", id_start);

                    string status = json.Substring(id_start, id_end - id_start);
                    Debug.WriteLine($"Status = {status}");

                    if (status == "processed")
                    {
                        Debug.WriteLine("STAUTS = processed. Loop ending");
                        PopulateResponse(json);
                        loop = false;

                        return;
                    }
                }
            } while (loop == true && numLoops < checkoutControl.TimeOut / 2);

            //if code gets here then check numloops to see if it timed out
            //if so send error
            if (numLoops >= checkoutControl.TimeOut / 2)
            {
                errorTimeout = true;
            }
        }

        private void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            //Check config setting
            if (AllErrors == true)
                //show errors
                e.Handled = false;
            else
            // Ignore the error and suppress the error dialog box. 
            e.Handled = true;

            MessageBox.Show(e.Description, "Un-handled browser Error");
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ((WebBrowser)sender).Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);

            if (LoadCompleted == false)
            {
                LoadCompleted = true;
                InvokeScript();
            }
       }
        private bool openPaypalUrl(string url)
        {
            bool _WebLoaded = false;
            if (!string.IsNullOrEmpty(url))
            {
                string sNewPayUrl=string.Format("{0}&display_mode=direct", url);
                System.Diagnostics.Process.Start(sNewPayUrl);
                //System.Diagnostics.Process.Start(url);
                _WebLoaded = true;
            }
            return _WebLoaded;
        }
    }
}