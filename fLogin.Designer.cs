namespace CoffeeStore
{
    partial class fLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fLogin));
            this.panel1 = new System.Windows.Forms.Panel();
            this.csTBoxPass = new CoffeeStore.csControls.csTextBox();
            this.pBoxPass = new System.Windows.Forms.PictureBox();
            this.csTBoxUserName = new CoffeeStore.csControls.csTextBox();
            this.pBoxUser = new System.Windows.Forms.PictureBox();
            this.lblSlogan = new System.Windows.Forms.Label();
            this.pBoxBrandName = new System.Windows.Forms.PictureBox();
            this.pBoxBigPic = new System.Windows.Forms.PictureBox();
            this.csBtnLogin = new CoffeeStore.csControls.csButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxBrandName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxBigPic)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.csTBoxPass);
            this.panel1.Controls.Add(this.pBoxPass);
            this.panel1.Controls.Add(this.csTBoxUserName);
            this.panel1.Controls.Add(this.pBoxUser);
            this.panel1.Location = new System.Drawing.Point(0, 328);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(582, 99);
            this.panel1.TabIndex = 0;
            // 
            // csTBoxPass
            // 
            this.csTBoxPass.BackColor = System.Drawing.SystemColors.Window;
            this.csTBoxPass.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.csTBoxPass.BorderFocusColor = System.Drawing.Color.HotPink;
            this.csTBoxPass.BorderRadius = 0;
            this.csTBoxPass.BorderSize = 2;
            this.csTBoxPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.csTBoxPass.ForeColor = System.Drawing.Color.DimGray;
            this.csTBoxPass.Location = new System.Drawing.Point(173, 57);
            this.csTBoxPass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.csTBoxPass.Multiline = false;
            this.csTBoxPass.Name = "csTBoxPass";
            this.csTBoxPass.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.csTBoxPass.PasswordChar = true;
            this.csTBoxPass.PlaceHolderColor = System.Drawing.Color.DarkGray;
            this.csTBoxPass.PlaceHolderText = "Mật khẩu";
            this.csTBoxPass.ReadOnly = false;
            this.csTBoxPass.Size = new System.Drawing.Size(300, 31);
            this.csTBoxPass.TabIndex = 2;
            this.csTBoxPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.csTBoxPass.Texts = "";
            this.csTBoxPass.UnderlineStyle = true;
            // 
            // pBoxPass
            // 
            this.pBoxPass.Image = global::CoffeeStore.Properties.Resources.passwordImg;
            this.pBoxPass.Location = new System.Drawing.Point(127, 54);
            this.pBoxPass.Name = "pBoxPass";
            this.pBoxPass.Size = new System.Drawing.Size(35, 35);
            this.pBoxPass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBoxPass.TabIndex = 2;
            this.pBoxPass.TabStop = false;
            // 
            // csTBoxUserName
            // 
            this.csTBoxUserName.BackColor = System.Drawing.SystemColors.Window;
            this.csTBoxUserName.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.csTBoxUserName.BorderFocusColor = System.Drawing.Color.HotPink;
            this.csTBoxUserName.BorderRadius = 0;
            this.csTBoxUserName.BorderSize = 2;
            this.csTBoxUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.csTBoxUserName.ForeColor = System.Drawing.Color.DimGray;
            this.csTBoxUserName.Location = new System.Drawing.Point(173, 8);
            this.csTBoxUserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.csTBoxUserName.Multiline = false;
            this.csTBoxUserName.Name = "csTBoxUserName";
            this.csTBoxUserName.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.csTBoxUserName.PasswordChar = false;
            this.csTBoxUserName.PlaceHolderColor = System.Drawing.Color.DarkGray;
            this.csTBoxUserName.PlaceHolderText = "Tên đăng nhập";
            this.csTBoxUserName.ReadOnly = false;
            this.csTBoxUserName.Size = new System.Drawing.Size(300, 31);
            this.csTBoxUserName.TabIndex = 1;
            this.csTBoxUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.csTBoxUserName.Texts = "";
            this.csTBoxUserName.UnderlineStyle = true;
            // 
            // pBoxUser
            // 
            this.pBoxUser.Image = global::CoffeeStore.Properties.Resources.userImg;
            this.pBoxUser.Location = new System.Drawing.Point(122, 0);
            this.pBoxUser.Name = "pBoxUser";
            this.pBoxUser.Size = new System.Drawing.Size(45, 45);
            this.pBoxUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBoxUser.TabIndex = 0;
            this.pBoxUser.TabStop = false;
            // 
            // lblSlogan
            // 
            this.lblSlogan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(177)))), ((int)(((byte)(65)))));
            this.lblSlogan.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSlogan.Enabled = false;
            this.lblSlogan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSlogan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblSlogan.Location = new System.Drawing.Point(0, 509);
            this.lblSlogan.Name = "lblSlogan";
            this.lblSlogan.Size = new System.Drawing.Size(582, 44);
            this.lblSlogan.TabIndex = 0;
            this.lblSlogan.Text = "DRINK GOOD COFFEE – THE BETTER THE HEALTH";
            this.lblSlogan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pBoxBrandName
            // 
            this.pBoxBrandName.Image = global::CoffeeStore.Properties.Resources.brand;
            this.pBoxBrandName.Location = new System.Drawing.Point(125, 260);
            this.pBoxBrandName.Name = "pBoxBrandName";
            this.pBoxBrandName.Size = new System.Drawing.Size(350, 50);
            this.pBoxBrandName.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBoxBrandName.TabIndex = 1;
            this.pBoxBrandName.TabStop = false;
            // 
            // pBoxBigPic
            // 
            this.pBoxBigPic.Dock = System.Windows.Forms.DockStyle.Top;
            this.pBoxBigPic.Image = global::CoffeeStore.Properties.Resources.login_img;
            this.pBoxBigPic.Location = new System.Drawing.Point(0, 0);
            this.pBoxBigPic.Name = "pBoxBigPic";
            this.pBoxBigPic.Size = new System.Drawing.Size(582, 250);
            this.pBoxBigPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBoxBigPic.TabIndex = 0;
            this.pBoxBigPic.TabStop = false;
            // 
            // csBtnLogin
            // 
            this.csBtnLogin.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.csBtnLogin.BackgroundColor = System.Drawing.Color.MediumSlateBlue;
            this.csBtnLogin.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.csBtnLogin.BorderRadius = 0;
            this.csBtnLogin.BorderSize = 0;
            this.csBtnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.csBtnLogin.FlatAppearance.BorderSize = 0;
            this.csBtnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.csBtnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.csBtnLogin.ForeColor = System.Drawing.Color.Ivory;
            this.csBtnLogin.Location = new System.Drawing.Point(212, 440);
            this.csBtnLogin.Name = "csBtnLogin";
            this.csBtnLogin.Size = new System.Drawing.Size(176, 44);
            this.csBtnLogin.TabIndex = 3;
            this.csBtnLogin.TabStop = false;
            this.csBtnLogin.Text = "Đăng nhập";
            this.csBtnLogin.TextColor = System.Drawing.Color.Ivory;
            this.csBtnLogin.UseVisualStyleBackColor = false;
            this.csBtnLogin.Click += new System.EventHandler(this.csBtnLogin_Click);
            // 
            // fLogin
            // 
            this.AcceptButton = this.csBtnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(582, 553);
            this.Controls.Add(this.csBtnLogin);
            this.Controls.Add(this.lblSlogan);
            this.Controls.Add(this.pBoxBrandName);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pBoxBigPic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "fLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pBoxPass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxBrandName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxBigPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBoxBigPic;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pBoxBrandName;
        private System.Windows.Forms.Label lblSlogan;
        private System.Windows.Forms.PictureBox pBoxUser;
        private csControls.csTextBox csTBoxUserName;
        private csControls.csTextBox csTBoxPass;
        private System.Windows.Forms.PictureBox pBoxPass;
        private csControls.csButton csBtnLogin;
    }
}