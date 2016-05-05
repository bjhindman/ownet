/*---------------------------------------------------------------------------
 * Copyright (C) 2005-2012 Maxim Integrated Products, All Rights Reserved.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY,  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL MAXIM INTEGRATED PRODUCTS BE LIABLE FOR ANY CLAIM, DAMAGES
 * OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * Except as contained in this notice, the name of Maxim Integrated Products
 * shall not be used except as stated in the Maxim Integrated Products
 * Branding Policy.
 *---------------------------------------------------------------------------
 */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace HygrochronViewer
{
	/// <summary>
	/// Summary description for DevicePasswordForm.
	/// </summary>
	public class DevicePasswordForm : System.Windows.Forms.Form
	{
      private System.Windows.Forms.RadioButton asciiRadioButton;
      private System.Windows.Forms.RadioButton hexRadioButton;
      private System.Windows.Forms.Label instructionLabel;
      private System.Windows.Forms.TextBox passwordTextBox;
      private System.Windows.Forms.Button okButton;
      private System.Windows.Forms.Button cancelButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

      public byte[] password
      {
         get
         {
            byte[] pass = new byte[8];
            if(hexRadioButton.Checked)
            {
               com.dalsemi.onewire.utils.Convert.toByteArray(
                  passwordTextBox.Text, pass, 0, 8);
            }
            else
            {
               for(int i=0; i<8; i++)
               {
                  if(i<passwordTextBox.Text.Length)
                     pass[i] = (byte)passwordTextBox.Text[i];
                  else
                     pass[i] = (byte)' ';
               }
            }
            return pass;
         }
         set
         {
            passwordTextBox.Text = "";
            hexRadioButton.Checked = true;
            bool isString = true;
            for(int i=0; isString && i<8; i++)
            {
               if(Char.IsLetterOrDigit((char)value[i]))
                  passwordTextBox.Text += (char)value[i];
               else
                  isString = false;
            }
            if(!isString)
               passwordTextBox.Text = com.dalsemi.onewire.utils.Convert.toHexString(value);
            else
               asciiRadioButton.Checked = true;
         }
      }

      public DevicePasswordForm(string title, string instructions, bool hideText)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

         this.Text = title;
         this.instructionLabel.Text = instructions;

         if(hideText)
            this.passwordTextBox.PasswordChar = '*'; 

         passwordTextBox.Focus();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
           this.passwordTextBox = new System.Windows.Forms.TextBox();
           this.asciiRadioButton = new System.Windows.Forms.RadioButton();
           this.hexRadioButton = new System.Windows.Forms.RadioButton();
           this.okButton = new System.Windows.Forms.Button();
           this.cancelButton = new System.Windows.Forms.Button();
           this.instructionLabel = new System.Windows.Forms.Label();
           this.SuspendLayout();
           // 
           // passwordTextBox
           // 
           this.passwordTextBox.Location = new System.Drawing.Point(24, 128);
           this.passwordTextBox.Name = "passwordTextBox";
           this.passwordTextBox.Size = new System.Drawing.Size(232, 20);
           this.passwordTextBox.TabIndex = 0;
           // 
           // asciiRadioButton
           // 
           this.asciiRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.asciiRadioButton.Location = new System.Drawing.Point(152, 72);
           this.asciiRadioButton.Name = "asciiRadioButton";
           this.asciiRadioButton.Size = new System.Drawing.Size(112, 40);
           this.asciiRadioButton.TabIndex = 1;
           this.asciiRadioButton.Text = "Ascii Characters (\'TestPass\')";
           this.asciiRadioButton.CheckedChanged += new System.EventHandler(this.asciiRadioButton_CheckedChanged);
           // 
           // hexRadioButton
           // 
           this.hexRadioButton.Checked = true;
           this.hexRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.hexRadioButton.Location = new System.Drawing.Point(16, 72);
           this.hexRadioButton.Name = "hexRadioButton";
           this.hexRadioButton.Size = new System.Drawing.Size(128, 40);
           this.hexRadioButton.TabIndex = 2;
           this.hexRadioButton.TabStop = true;
           this.hexRadioButton.Text = "Ascii-Encoded Hex (\'1A 2B 3C  4D ... \')";
           this.hexRadioButton.CheckedChanged += new System.EventHandler(this.hexRadioButton_CheckedChanged);
           // 
           // okButton
           // 
           this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
           this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.okButton.Location = new System.Drawing.Point(24, 168);
           this.okButton.Name = "okButton";
           this.okButton.Size = new System.Drawing.Size(96, 24);
           this.okButton.TabIndex = 3;
           this.okButton.Text = "OK";
           this.okButton.Click += new System.EventHandler(this.okButton_Click);
           // 
           // cancelButton
           // 
           this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
           this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.cancelButton.Location = new System.Drawing.Point(160, 168);
           this.cancelButton.Name = "cancelButton";
           this.cancelButton.Size = new System.Drawing.Size(96, 24);
           this.cancelButton.TabIndex = 4;
           this.cancelButton.Text = "Cancel";
           this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
           // 
           // instructionLabel
           // 
           this.instructionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.instructionLabel.Location = new System.Drawing.Point(8, 8);
           this.instructionLabel.Name = "instructionLabel";
           this.instructionLabel.Size = new System.Drawing.Size(264, 56);
           this.instructionLabel.TabIndex = 5;
           this.instructionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
           // 
           // DevicePasswordForm
           // 
           this.AcceptButton = this.okButton;
           this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
           this.CancelButton = this.cancelButton;
           this.ClientSize = new System.Drawing.Size(278, 204);
           this.Controls.Add(this.instructionLabel);
           this.Controls.Add(this.cancelButton);
           this.Controls.Add(this.okButton);
           this.Controls.Add(this.hexRadioButton);
           this.Controls.Add(this.asciiRadioButton);
           this.Controls.Add(this.passwordTextBox);
           this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
           this.MaximizeBox = false;
           this.MinimizeBox = false;
           this.Name = "DevicePasswordForm";
           this.ShowInTaskbar = false;
           this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
           this.TopMost = true;
           this.ResumeLayout(false);
           this.PerformLayout();

      }
		#endregion

      private void hexRadioButton_CheckedChanged(object sender, System.EventArgs e)
      {
         asciiRadioButton.Checked = !hexRadioButton.Checked;
         passwordTextBox.Focus();
      }

      private void asciiRadioButton_CheckedChanged(object sender, System.EventArgs e)
      {
         hexRadioButton.Checked = !asciiRadioButton.Checked;
         passwordTextBox.Focus();
      }

       private void okButton_Click(object sender, EventArgs e)
       {

       }

       private void cancelButton_Click(object sender, EventArgs e)
       {

       }
	}
}
