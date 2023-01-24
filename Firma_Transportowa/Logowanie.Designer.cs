
namespace Firma_Transportowa
{
    partial class Logowanie
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Logowanie));
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.btnZaloguj = new System.Windows.Forms.Button();
            this.lblWynikLogowania = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panelLogin = new System.Windows.Forms.Panel();
            this.panelHaslo = new System.Windows.Forms.Panel();
            this.Zarejestruj = new System.Windows.Forms.Button();
            this.txtHaslo = new System.Windows.Forms.TextBox();
            this.exit = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLogin
            // 
            this.txtLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.txtLogin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtLogin.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtLogin.HideSelection = false;
            this.txtLogin.Location = new System.Drawing.Point(97, 120);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(168, 20);
            this.txtLogin.TabIndex = 0;
            this.txtLogin.TabStop = false;
            this.txtLogin.Text = "Login";
            this.txtLogin.Click += new System.EventHandler(this.txtLogin_Click);
            // 
            // btnZaloguj
            // 
            this.btnZaloguj.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(166)))), ((int)(((byte)(13)))));
            this.btnZaloguj.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnZaloguj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZaloguj.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnZaloguj.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.btnZaloguj.Location = new System.Drawing.Point(65, 240);
            this.btnZaloguj.Name = "btnZaloguj";
            this.btnZaloguj.Size = new System.Drawing.Size(200, 38);
            this.btnZaloguj.TabIndex = 2;
            this.btnZaloguj.Text = "Zaloguj";
            this.btnZaloguj.UseVisualStyleBackColor = false;
            this.btnZaloguj.Click += new System.EventHandler(this.btnZaloguj_Click);
            // 
            // lblWynikLogowania
            // 
            this.lblWynikLogowania.AutoSize = true;
            this.lblWynikLogowania.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblWynikLogowania.Location = new System.Drawing.Point(81, 209);
            this.lblWynikLogowania.Name = "lblWynikLogowania";
            this.lblWynikLogowania.Size = new System.Drawing.Size(28, 13);
            this.lblWynikLogowania.TabIndex = 3;
            this.lblWynikLogowania.Text = "       ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Ubuntu", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(166)))), ((int)(((byte)(13)))));
            this.label1.Location = new System.Drawing.Point(78, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 35);
            this.label1.TabIndex = 4;
            this.label1.Text = "Logowanie";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Firma_Transportowa.Properties.Resources.userW;
            this.pictureBox1.Location = new System.Drawing.Point(65, 114);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(26, 26);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.Location = new System.Drawing.Point(65, 166);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(26, 26);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // panelLogin
            // 
            this.panelLogin.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelLogin.Location = new System.Drawing.Point(65, 146);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(200, 1);
            this.panelLogin.TabIndex = 7;
            // 
            // panelHaslo
            // 
            this.panelHaslo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelHaslo.Location = new System.Drawing.Point(65, 196);
            this.panelHaslo.Name = "panelHaslo";
            this.panelHaslo.Size = new System.Drawing.Size(200, 1);
            this.panelHaslo.TabIndex = 8;
            // 
            // Zarejestruj
            // 
            this.Zarejestruj.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.Zarejestruj.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Zarejestruj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Zarejestruj.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Zarejestruj.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Zarejestruj.Location = new System.Drawing.Point(65, 293);
            this.Zarejestruj.Name = "Zarejestruj";
            this.Zarejestruj.Size = new System.Drawing.Size(200, 38);
            this.Zarejestruj.TabIndex = 9;
            this.Zarejestruj.Text = "Zarejestruj";
            this.Zarejestruj.UseVisualStyleBackColor = false;
            this.Zarejestruj.Click += new System.EventHandler(this.Zarejestruj_Click);
            // 
            // txtHaslo
            // 
            this.txtHaslo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(40)))));
            this.txtHaslo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHaslo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtHaslo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtHaslo.Location = new System.Drawing.Point(97, 172);
            this.txtHaslo.Name = "txtHaslo";
            this.txtHaslo.Size = new System.Drawing.Size(168, 20);
            this.txtHaslo.TabIndex = 10;
            this.txtHaslo.Text = "Hasło";
            this.txtHaslo.Click += new System.EventHandler(this.txtHaslo_Click_1);
            this.txtHaslo.TextChanged += new System.EventHandler(this.txtHaslo_TextChanged);
            // 
            // exit
            // 
            this.exit.AutoSize = true;
            this.exit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exit.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.exit.Location = new System.Drawing.Point(304, 9);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(27, 19);
            this.exit.TabIndex = 11;
            this.exit.Text = " X ";
            this.exit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // Logowanie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(335, 358);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.txtHaslo);
            this.Controls.Add(this.Zarejestruj);
            this.Controls.Add(this.panelHaslo);
            this.Controls.Add(this.panelLogin);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblWynikLogowania);
            this.Controls.Add(this.btnZaloguj);
            this.Controls.Add(this.txtLogin);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Logowanie";
            this.ShowIcon = false;
            this.Text = "Logowanie";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Logowanie_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Logowanie_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Logowanie_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Logowanie_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Button btnZaloguj;
        private System.Windows.Forms.Label lblWynikLogowania;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panelLogin;
        private System.Windows.Forms.Panel panelHaslo;
        private System.Windows.Forms.Button Zarejestruj;
        private System.Windows.Forms.TextBox txtHaslo;
        private System.Windows.Forms.Label exit;
    }
}