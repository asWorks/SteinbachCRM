namespace TestOutlookAddin.Resources
{
    partial class SelectContact
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
            this.components = new System.ComponentModel.Container();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cboFirma = new System.Windows.Forms.ComboBox();
            this.steinbachEntitiesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.steinbachEntitiesBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.cboKontakt = new System.Windows.Forms.ComboBox();
            this.cmdProcess = new System.Windows.Forms.Button();
            this.txtSender = new System.Windows.Forms.TextBox();
            this.txtRecipient = new System.Windows.Forms.TextBox();
            this.txtSenderEmail = new System.Windows.Forms.TextBox();
            this.txtRecipientEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.steinbachEntitiesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.steinbachEntitiesBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(98, 53);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(290, 20);
            this.txtSubject.TabIndex = 1;
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(98, 27);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(290, 20);
            this.txtDate.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(98, 195);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(660, 432);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // cboFirma
            // 
            this.cboFirma.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.steinbachEntitiesBindingSource, "firmas", true));
            this.cboFirma.FormattingEnabled = true;
            this.cboFirma.Location = new System.Drawing.Point(494, 26);
            this.cboFirma.Name = "cboFirma";
            this.cboFirma.Size = new System.Drawing.Size(247, 21);
            this.cboFirma.TabIndex = 4;
            this.cboFirma.SelectedValueChanged += new System.EventHandler(this.cboFirma_SelectedValueChanged);
            this.cboFirma.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboFirma_KeyDown);
            // 
            // steinbachEntitiesBindingSource
            // 
            this.steinbachEntitiesBindingSource.DataSource = typeof(TestOutlookAddin.SteinbachEntities);
            // 
            // steinbachEntitiesBindingSource1
            // 
            this.steinbachEntitiesBindingSource1.DataSource = typeof(TestOutlookAddin.SteinbachEntities);
            // 
            // cboKontakt
            // 
            this.cboKontakt.FormattingEnabled = true;
            this.cboKontakt.Location = new System.Drawing.Point(494, 53);
            this.cboKontakt.Name = "cboKontakt";
            this.cboKontakt.Size = new System.Drawing.Size(247, 21);
            this.cboKontakt.TabIndex = 5;
            // 
            // cmdProcess
            // 
            this.cmdProcess.Location = new System.Drawing.Point(494, 90);
            this.cmdProcess.Name = "cmdProcess";
            this.cmdProcess.Size = new System.Drawing.Size(148, 23);
            this.cmdProcess.TabIndex = 6;
            this.cmdProcess.Text = "Zuordnung speichern";
            this.cmdProcess.UseVisualStyleBackColor = true;
            this.cmdProcess.Click += new System.EventHandler(this.cmdProcess_Click);
            // 
            // txtSender
            // 
            this.txtSender.Location = new System.Drawing.Point(98, 79);
            this.txtSender.Name = "txtSender";
            this.txtSender.Size = new System.Drawing.Size(290, 20);
            this.txtSender.TabIndex = 7;
            // 
            // txtRecipient
            // 
            this.txtRecipient.Location = new System.Drawing.Point(98, 105);
            this.txtRecipient.Name = "txtRecipient";
            this.txtRecipient.Size = new System.Drawing.Size(290, 20);
            this.txtRecipient.TabIndex = 8;
            // 
            // txtSenderEmail
            // 
            this.txtSenderEmail.Location = new System.Drawing.Point(98, 131);
            this.txtSenderEmail.Name = "txtSenderEmail";
            this.txtSenderEmail.Size = new System.Drawing.Size(290, 20);
            this.txtSenderEmail.TabIndex = 9;
            // 
            // txtRecipientEmail
            // 
            this.txtRecipientEmail.Location = new System.Drawing.Point(98, 157);
            this.txtRecipientEmail.Name = "txtRecipientEmail";
            this.txtRecipientEmail.Size = new System.Drawing.Size(290, 20);
            this.txtRecipientEmail.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(450, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Firma :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(438, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Kontakt :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(494, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Mail zuordnen zu :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Datum :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Betreff :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Absender :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 105);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Empfänger :";
            // 
            // SelectContact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 693);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRecipientEmail);
            this.Controls.Add(this.txtSenderEmail);
            this.Controls.Add(this.txtRecipient);
            this.Controls.Add(this.txtSender);
            this.Controls.Add(this.cmdProcess);
            this.Controls.Add(this.cboKontakt);
            this.Controls.Add(this.cboFirma);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.txtSubject);
            this.Name = "SelectContact";
            this.Text = "Email zuordnen";
            this.Load += new System.EventHandler(this.SelectContact_Load);
            ((System.ComponentModel.ISupportInitialize)(this.steinbachEntitiesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.steinbachEntitiesBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox cboFirma;
        private System.Windows.Forms.BindingSource steinbachEntitiesBindingSource;
        private System.Windows.Forms.BindingSource steinbachEntitiesBindingSource1;
        private System.Windows.Forms.ComboBox cboKontakt;
        private System.Windows.Forms.Button cmdProcess;
        private System.Windows.Forms.TextBox txtSender;
        private System.Windows.Forms.TextBox txtRecipient;
        private System.Windows.Forms.TextBox txtSenderEmail;
        private System.Windows.Forms.TextBox txtRecipientEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}