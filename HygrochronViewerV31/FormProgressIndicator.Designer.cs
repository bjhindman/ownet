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
namespace HygrochronViewer
{
   partial class FormProgressIndicator
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
         this.progressBar = new System.Windows.Forms.ProgressBar();
         this.labelProgressTop = new System.Windows.Forms.Label();
         this.labelProgressBottom = new System.Windows.Forms.Label();
         this.labelProgressBottom2 = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // progressBar
         // 
         this.progressBar.Location = new System.Drawing.Point(84, 73);
         this.progressBar.Name = "progressBar";
         this.progressBar.Size = new System.Drawing.Size(173, 23);
         this.progressBar.TabIndex = 0;
         // 
         // labelProgressTop
         // 
         this.labelProgressTop.AutoSize = true;
         this.labelProgressTop.Location = new System.Drawing.Point(29, 39);
         this.labelProgressTop.Name = "labelProgressTop";
         this.labelProgressTop.Size = new System.Drawing.Size(279, 13);
         this.labelProgressTop.TabIndex = 1;
         this.labelProgressTop.Text = "Please wait while 1-Wire adapters are detected.";
         // 
         // labelProgressBottom
         // 
         this.labelProgressBottom.AutoSize = true;
         this.labelProgressBottom.Location = new System.Drawing.Point(29, 116);
         this.labelProgressBottom.Name = "labelProgressBottom";
         this.labelProgressBottom.Size = new System.Drawing.Size(288, 13);
         this.labelProgressBottom.TabIndex = 2;
         this.labelProgressBottom.Text = "Adapters detected will be listed in the menu here:";
         // 
         // labelProgressBottom2
         // 
         this.labelProgressBottom2.AutoSize = true;
         this.labelProgressBottom2.Location = new System.Drawing.Point(99, 149);
         this.labelProgressBottom2.Name = "labelProgressBottom2";
         this.labelProgressBottom2.Size = new System.Drawing.Size(131, 13);
         this.labelProgressBottom2.TabIndex = 3;
         this.labelProgressBottom2.Text = "File -> 1-Wire Adapter";
         // 
         // FormProgressIndicator
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(341, 200);
         this.ControlBox = false;
         this.Controls.Add(this.labelProgressBottom2);
         this.Controls.Add(this.labelProgressBottom);
         this.Controls.Add(this.labelProgressTop);
         this.Controls.Add(this.progressBar);
         this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "FormProgressIndicator";
         this.Load += new System.EventHandler(this.FormProgressIndicator_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ProgressBar progressBar;
      private System.Windows.Forms.Label labelProgressTop;
      private System.Windows.Forms.Label labelProgressBottom;
      private System.Windows.Forms.Label labelProgressBottom2;
   }
}