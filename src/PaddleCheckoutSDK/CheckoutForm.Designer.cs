namespace PaddleCheckoutSDK
{
   
    partial class CheckoutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckoutForm));
            this.button1 = new System.Windows.Forms.Button();
            this.Close_Form = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.CheckoutView = new PaddleCheckoutSDK.CheckoutControl();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(226, 782);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Close_Form
            // 
            this.Close_Form.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_Form.Enabled = false;
            this.Close_Form.Location = new System.Drawing.Point(562, 708);
            this.Close_Form.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Close_Form.Name = "Close_Form";
            this.Close_Form.Size = new System.Drawing.Size(112, 35);
            this.Close_Form.TabIndex = 2;
            this.Close_Form.Text = "Close";
            this.Close_Form.UseVisualStyleBackColor = true;
            this.Close_Form.Visible = false;
            this.Close_Form.Click += new System.EventHandler(this.Close_Form_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(348, 782);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 35);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // CheckoutView
            // 
            this.CheckoutView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckoutView.BackColor = System.Drawing.SystemColors.Control;
            this.CheckoutView.Location = new System.Drawing.Point(6, 5);
            this.CheckoutView.Margin = new System.Windows.Forms.Padding(0);
            this.CheckoutView.Name = "CheckoutView";
            this.CheckoutView.PassThroughData = null;
            this.CheckoutView.PreFilledCountryCode = null;
            this.CheckoutView.PreFilledCoupon = null;
            this.CheckoutView.PreFilledEmail = null;
            this.CheckoutView.PreFilledPostCode = null;
            this.CheckoutView.Prices = null;
            this.CheckoutView.Size = new System.Drawing.Size(674, 744);
            this.CheckoutView.TabIndex = 1;
            this.CheckoutView.UserCountry = null;
            this.CheckoutView.UserSubmittedEmail = null;
            // 
            // CheckoutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(684, 754);
            this.Controls.Add(this.CheckoutView);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Close_Form);
            this.Controls.Add(this.button1);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckoutForm";
            this.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Text = "Paddle Checkout";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CheckoutForm_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button Close_Form;
        private System.Windows.Forms.Button button2;
        //Exposed as public to allow manipulation via Forms Visual Inheritance
        public CheckoutControl CheckoutView;
    }
}