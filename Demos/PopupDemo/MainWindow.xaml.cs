using PaddleCheckoutSDK;
using System.Linq;
using System.Windows;



namespace PopupDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string vendoridTxt = vendorID.Text;
            string productidTxt = productID.Text;
            string appnameTxt = appname.Text;


            if (Alt_style.IsChecked == true)
            {
                // This shows how underlying WinForms Form can be accessed and customized
                PaddleCheckout paddleCheckoutForm = new PaddleCheckout(vendoridTxt, productidTxt, appnameTxt);
                paddleCheckoutForm.TransactionCompleteEvent += PaddleCheckoutForm_TransactionCompleteEvent;
                paddleCheckoutForm.TransactionErrorEvent += PaddleCheckoutForm_TransctionErrorEvent;

                paddleCheckoutForm.Form.Text = "My Form Title";
                paddleCheckoutForm.Form.ForeColor = System.Drawing.Color.Green;
                paddleCheckoutForm.Form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                paddleCheckoutForm.Form.BackColor = System.Drawing.Color.Red;

                paddleCheckoutForm.ShowWindow();

            }
            else
            {
                // This demo shows how to pass extra parameters to the window before launch
                //realistically you would not pass all of them at one time!
                PaddleCheckout paddleCheckoutForm = new PaddleCheckout(vendoridTxt, productidTxt, appnameTxt)
                {
                    //   PreFilledCoupon = "C5BBBF89",
                    //    PassThroughData = "23456",
                    //    PreFilledEmail = "clive.collie@paddle.com",
                    //   PreFilledCountryCode = "GB",
                    //  PreFilledPostCode = "SE1 3UN",
                    //  If changing price authorization hash is required
                    //  Authorization = "f004faf66cb1e1165d3bac90fce0e0d0" eab6b5eded402a379cb2f7f20a243b38
                  //  Prices = new PriceOverride[2] { new PriceOverride { Currency = "GBP", Price = "30" , Authorization = "c6f1706aed13e05a4007624a48ef39cf" }, new PriceOverride { Currency = "USD", Price = "30" , Authorization = "c6f1706aed13e05a4007624a48ef39cf" } }
                };

                //this event required
                paddleCheckoutForm.TransactionCompleteEvent += PaddleCheckoutForm_TransactionCompleteEvent;
                //this event recommended
                paddleCheckoutForm.TransactionErrorEvent += PaddleCheckoutForm_TransctionErrorEvent;
                //this is optional
                paddleCheckoutForm.TransactionBeginEvent += PaddleCheckoutForm_TransactionBeginEvent;
                // Use to fetch email and location in when entered
                paddleCheckoutForm.PageSubmitted += PaddleCheckoutForm_PageSubmitted;
                paddleCheckoutForm.ShowWindow();

                var ps = paddleCheckoutForm.ProcessStatus;
            }

        }

        private void PaddleCheckoutForm_PageSubmitted(object sender, PageSubmittedEventArgs e)
        {
            returnText.Text += e.ToString();
        }

        private void PaddleCheckoutForm_TransactionBeginEvent(object sender, TransactionBeginEventArgs e)
        {
            returnText.Text += "Transaction Begin\n";
        }


        private void PaddleCheckoutForm_TransctionErrorEvent(object sender, TransactionErrorEventArgs e)
        {
            //for demo just display error
            returnText.Text = e.Error;
        }

        private void PaddleCheckoutForm_TransactionCompleteEvent(object sender, TransactionCompleteEventArgs e)
        {
            //ProcessStatus contains purchase information from server. 
            //for demo show all
            returnText.Text += e.ProcessStatus.ToString();
        }


    }
}
