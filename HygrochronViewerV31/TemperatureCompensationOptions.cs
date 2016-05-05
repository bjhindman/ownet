/*---------------------------------------------------------------------------
 * Copyright (C) 2005 Maxim Integrated Products, All Rights Reserved.
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
	/// Summary description for TemperatureCompensationOptions.
	/// </summary>
	public class TemperatureCompensationOptions : System.Windows.Forms.Form
	{
      private System.Windows.Forms.GroupBox groupBox1;
      public System.Windows.Forms.CheckBox overrideTemperatureLog;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      public System.Windows.Forms.NumericUpDown defaultTemperatureValue;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Button okbutton;
      private System.Windows.Forms.Button cancelbutton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TemperatureCompensationOptions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.defaultTemperatureValue = new System.Windows.Forms.NumericUpDown();
         this.label1 = new System.Windows.Forms.Label();
         this.overrideTemperatureLog = new System.Windows.Forms.CheckBox();
         this.okbutton = new System.Windows.Forms.Button();
         this.cancelbutton = new System.Windows.Forms.Button();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.defaultTemperatureValue)).BeginInit();
         this.SuspendLayout();
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.label3);
         this.groupBox1.Controls.Add(this.label2);
         this.groupBox1.Controls.Add(this.defaultTemperatureValue);
         this.groupBox1.Controls.Add(this.label1);
         this.groupBox1.Controls.Add(this.overrideTemperatureLog);
         this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
         this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.groupBox1.Location = new System.Drawing.Point(0, 0);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(292, 200);
         this.groupBox1.TabIndex = 0;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Options";
         // 
         // label3
         // 
         this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.label3.Location = new System.Drawing.Point(8, 136);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(280, 56);
         this.label3.TabIndex = 4;
         this.label3.Text = "The application will use the above temperature value for temperature compensation" +
            " if the device has no temperature log data or if the \"Override Temperature Log\" " +
            "checkbox has been selected.";
         // 
         // label2
         // 
         this.label2.Location = new System.Drawing.Point(88, 104);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(184, 24);
         this.label2.TabIndex = 3;
         this.label2.Text = "(C)    Default Temperature Value";
         // 
         // defaultTemperatureValue
         // 
         this.defaultTemperatureValue.DecimalPlaces = 2;
         this.defaultTemperatureValue.Location = new System.Drawing.Point(16, 104);
         this.defaultTemperatureValue.Name = "defaultTemperatureValue";
         this.defaultTemperatureValue.Size = new System.Drawing.Size(72, 20);
         this.defaultTemperatureValue.TabIndex = 2;
         this.defaultTemperatureValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         this.defaultTemperatureValue.Value = new System.Decimal(new int[] {
                                                                              250,
                                                                              0,
                                                                              0,
                                                                              65536});
         // 
         // label1
         // 
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.label1.Location = new System.Drawing.Point(8, 48);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(280, 48);
         this.label1.TabIndex = 1;
         this.label1.Text = "Check the \"Override Temperature Log\" box if you want to ignore the device\'s tempe" +
            "rature log and always use the user-specified temperature value.";
         // 
         // overrideTemperatureLog
         // 
         this.overrideTemperatureLog.Location = new System.Drawing.Point(16, 24);
         this.overrideTemperatureLog.Name = "overrideTemperatureLog";
         this.overrideTemperatureLog.Size = new System.Drawing.Size(264, 16);
         this.overrideTemperatureLog.TabIndex = 0;
         this.overrideTemperatureLog.Text = "Override Temperature Log";
         // 
         // okbutton
         // 
         this.okbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.okbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.okbutton.Location = new System.Drawing.Point(40, 208);
         this.okbutton.Name = "okbutton";
         this.okbutton.Size = new System.Drawing.Size(88, 24);
         this.okbutton.TabIndex = 1;
         this.okbutton.Text = "OK";
         // 
         // cancelbutton
         // 
         this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cancelbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.cancelbutton.Location = new System.Drawing.Point(168, 208);
         this.cancelbutton.Name = "cancelbutton";
         this.cancelbutton.Size = new System.Drawing.Size(88, 24);
         this.cancelbutton.TabIndex = 2;
         this.cancelbutton.Text = "Cancel";
         // 
         // TemperatureCompensationOptions
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(292, 248);
         this.ControlBox = false;
         this.Controls.Add(this.cancelbutton);
         this.Controls.Add(this.okbutton);
         this.Controls.Add(this.groupBox1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "TemperatureCompensationOptions";
         this.Text = "Temperature Compensation Options";
         this.groupBox1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.defaultTemperatureValue)).EndInit();
         this.ResumeLayout(false);

      }
		#endregion
	}
}
