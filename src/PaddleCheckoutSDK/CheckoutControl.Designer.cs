namespace PaddleCheckoutSDK
{
    partial class CheckoutControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.labelCompleting = new System.Windows.Forms.Label();
            this.loadingCircle = new PaddleCheckoutSDK.LoadingCircle();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(150, 150);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // labelCompleting
            // 
            this.labelCompleting.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelCompleting.BackColor = System.Drawing.SystemColors.Window;
            this.labelCompleting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelCompleting.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCompleting.ForeColor = System.Drawing.Color.DimGray;
            this.labelCompleting.Location = new System.Drawing.Point(0, 0);
            this.labelCompleting.Margin = new System.Windows.Forms.Padding(0);
            this.labelCompleting.Name = "labelCompleting";
            this.labelCompleting.Size = new System.Drawing.Size(150, 15);
            this.labelCompleting.TabIndex = 2;
            this.labelCompleting.Text = "Completing Purchase";
            this.labelCompleting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loadingCircle
            // 
            this.loadingCircle.Active = false;
            this.loadingCircle.BackColor = System.Drawing.SystemColors.Window;
            this.loadingCircle.Color = System.Drawing.Color.Black;
            this.loadingCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadingCircle.InnerCircleRadius = 5;
            this.loadingCircle.Location = new System.Drawing.Point(0, 0);
            this.loadingCircle.Name = "loadingCircle";
            this.loadingCircle.NumberSpoke = 12;
            this.loadingCircle.OuterCircleRadius = 11;
            this.loadingCircle.RotationSpeed = 100;
            this.loadingCircle.Size = new System.Drawing.Size(150, 150);
            this.loadingCircle.SpokeThickness = 2;
            this.loadingCircle.StylePreset = PaddleCheckoutSDK.LoadingCircle.StylePresets.MacOSX;
            this.loadingCircle.TabIndex = 1;
            this.loadingCircle.Text = "loadingCircle1";
            // 
            // CheckoutControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelCompleting);
            this.Controls.Add(this.loadingCircle);
            this.Controls.Add(this.webBrowser);
            this.Name = "CheckoutControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        internal LoadingCircle loadingCircle;
        internal System.Windows.Forms.Label labelCompleting;
    }
}
