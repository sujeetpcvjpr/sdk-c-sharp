using PaddleCheckoutSDK;
using System;
using System.Windows.Forms;

namespace PopupCheckoutWinforms
{
    public partial class FormsDemo : Form
    {
        public FormsDemo()
        {
            InitializeComponent();
        }

        private void ButtonLaunch_Click(object sender, EventArgs e)
        {
            //Create instance of PaddleCheckout passing in credentials from vendor dashboard
            PaddleCheckout paddleCheckoutForm = new PaddleCheckout(textBoxVendorID.Text, textBoxProductID.Text, textBoxAppName.Text);

            //requires  these two event handlers
            //optimal TransactionBegin event can be wired up
            paddleCheckoutForm.TransactionCompleteEvent += PaddleCheckoutForm_TransactionCompleteEvent;
            paddleCheckoutForm.TransactionErrorEvent += PaddleCheckoutForm_TransactionErrorEvent;

            //This event handler fires when the email and location pages are submitted 
            //allowing access to the email and location information entered by the use
            paddleCheckoutForm.PageSubmitted += PaddleCheckoutForm_PageSubmitted;

            paddleCheckoutForm.ShowWindow();

            //Get transaction details. Also passed in TransactionCompleted Event
            ProcessStatus ps = paddleCheckoutForm.ProcessStatus;
        }

        private void PaddleCheckoutForm_PageSubmitted(object sender, PageSubmittedEventArgs e)
        {
            textBoxErrors.Text +=  e.ToString();
        }

        private void PaddleCheckoutForm_TransactionErrorEvent(object sender, TransactionErrorEventArgs e)
        {
            textBoxErrors.Text += e.Error;
        }

        private void PaddleCheckoutForm_TransactionCompleteEvent(object sender, TransactionCompleteEventArgs e)
        {
            textBoxErrors.Text += e.ToString();
        }
    }
}
