/*---------------------------------------------------------------------------
 * Copyright (C) 2005-2008 Maxim Integrated Products, All Rights Reserved.
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
using com.dalsemi.onewire;
using com.dalsemi.onewire.adapter;
using com.dalsemi.onewire.container;
using com.dalsemi.onewire.utils;

namespace HygrochronViewer
{
	/// <summary>
	/// Summary description for StartMissionForm.
	/// </summary>
	public class StartMissionForm : System.Windows.Forms.Form
	{
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.CheckBox syncClockCheckBox;
      private System.Windows.Forms.CheckBox enableRolloverCheckBox;
      private System.Windows.Forms.NumericUpDown sampleRate;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.NumericUpDown startDelay;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.ComboBox temperatureResolution;
      private System.Windows.Forms.ComboBox humidityResolution;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.CheckBox suta;
      private System.Windows.Forms.Button startNewMission;
      private System.Windows.Forms.Button Cancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

      private OneWireContainer41 owc41;
      private System.Windows.Forms.CheckBox enableTemperatureSampling;
      private System.Windows.Forms.CheckBox enableHumiditySampling;
      private System.Windows.Forms.CheckBox enableTempHighAlarm;
      private System.Windows.Forms.CheckBox enableTempLowAlarm;
      private System.Windows.Forms.CheckBox enableHumdLowAlarm;
      private System.Windows.Forms.CheckBox enableHumdHighAlarm;
      private DSPortAdapter adapter;
      private System.Windows.Forms.NumericUpDown temperatureHighAlarm;
      private System.Windows.Forms.NumericUpDown temperatureLowAlarm;
      private System.Windows.Forms.NumericUpDown humidityHighAlarm;
      private System.Windows.Forms.NumericUpDown humidityLowAlarm;
      private System.Windows.Forms.GroupBox humidityGroupBox;
      private int counter = 0;

		public StartMissionForm(DSPortAdapter adapter, OneWireContainer41 owc41)
		{
         this.adapter = adapter;
         this.owc41 = owc41;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
         temperatureResolution.SelectedIndex = 0;
         humidityResolution.SelectedIndex = 0;

         if(owc41 == null)
         {
            this.Text += " on all DS1922s";
            this.humidityGroupBox.Text = "Data";
         }
         else
         {
            this.humidityGroupBox.Text = owc41.getMissionLabel(OneWireContainer41.DATA_CHANNEL);
            this.Text += " on device: " + owc41.AddressAsString;
         }

         if(this.humidityGroupBox.Text.Equals("Humidity"))
         {
            this.enableHumiditySampling.Text = "Enable Humidity Sampling";
            this.enableHumdHighAlarm.Text = "Enable High Alarm (%RH)";
            this.enableHumdLowAlarm.Text = "Enable Low Alarm (%RH)";
         }
         else
         {
            this.enableHumiditySampling.Text = "Enable Sampling";
            this.enableHumdHighAlarm.Text = "Enable High Alarm";
            this.enableHumdLowAlarm.Text = "Enable Low Alarm";
         }
           
         if (owc41 != null && (owc41.Name.Equals("DS1922T") || owc41.Name.Equals("DS1922L") || owc41.Name.Equals("DS1922E")))
         {
            this.humidityGroupBox.Enabled = false;
            this.enableHumiditySampling.Checked = false;
         }
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
         this.label4 = new System.Windows.Forms.Label();
         this.startDelay = new System.Windows.Forms.NumericUpDown();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.sampleRate = new System.Windows.Forms.NumericUpDown();
         this.enableRolloverCheckBox = new System.Windows.Forms.CheckBox();
         this.syncClockCheckBox = new System.Windows.Forms.CheckBox();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.temperatureLowAlarm = new System.Windows.Forms.NumericUpDown();
         this.enableTempLowAlarm = new System.Windows.Forms.CheckBox();
         this.enableTempHighAlarm = new System.Windows.Forms.CheckBox();
         this.suta = new System.Windows.Forms.CheckBox();
         this.temperatureResolution = new System.Windows.Forms.ComboBox();
         this.label5 = new System.Windows.Forms.Label();
         this.enableTemperatureSampling = new System.Windows.Forms.CheckBox();
         this.temperatureHighAlarm = new System.Windows.Forms.NumericUpDown();
         this.humidityGroupBox = new System.Windows.Forms.GroupBox();
         this.humidityLowAlarm = new System.Windows.Forms.NumericUpDown();
         this.enableHumdHighAlarm = new System.Windows.Forms.CheckBox();
         this.enableHumdLowAlarm = new System.Windows.Forms.CheckBox();
         this.humidityResolution = new System.Windows.Forms.ComboBox();
         this.label10 = new System.Windows.Forms.Label();
         this.enableHumiditySampling = new System.Windows.Forms.CheckBox();
         this.humidityHighAlarm = new System.Windows.Forms.NumericUpDown();
         this.startNewMission = new System.Windows.Forms.Button();
         this.Cancel = new System.Windows.Forms.Button();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.startDelay)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sampleRate)).BeginInit();
         this.groupBox2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.temperatureLowAlarm)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.temperatureHighAlarm)).BeginInit();
         this.humidityGroupBox.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.humidityLowAlarm)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.humidityHighAlarm)).BeginInit();
         this.SuspendLayout();
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.label3);
         this.groupBox1.Controls.Add(this.label4);
         this.groupBox1.Controls.Add(this.startDelay);
         this.groupBox1.Controls.Add(this.label2);
         this.groupBox1.Controls.Add(this.label1);
         this.groupBox1.Controls.Add(this.sampleRate);
         this.groupBox1.Controls.Add(this.enableRolloverCheckBox);
         this.groupBox1.Controls.Add(this.syncClockCheckBox);
         this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
         this.groupBox1.Location = new System.Drawing.Point(0, 0);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(442, 80);
         this.groupBox1.TabIndex = 0;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Mission General";
         // 
         // label3
         // 
         this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.label3.Location = new System.Drawing.Point(312, 24);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(104, 16);
         this.label3.TabIndex = 7;
         this.label3.Text = "Start Delay";
         // 
         // label4
         // 
         this.label4.Location = new System.Drawing.Point(360, 40);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(48, 16);
         this.label4.TabIndex = 6;
         this.label4.Text = "Minutes";
         this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
         // 
         // startDelay
         // 
         this.startDelay.Location = new System.Drawing.Point(304, 40);
         this.startDelay.Maximum = new System.Decimal(new int[] {
                                                                   65535,
                                                                   0,
                                                                   0,
                                                                   0});
         this.startDelay.Name = "startDelay";
         this.startDelay.Size = new System.Drawing.Size(56, 20);
         this.startDelay.TabIndex = 5;
         this.startDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         // 
         // label2
         // 
         this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.label2.Location = new System.Drawing.Point(176, 24);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(104, 16);
         this.label2.TabIndex = 4;
         this.label2.Text = "Sample Rate";
         // 
         // label1
         // 
         this.label1.Location = new System.Drawing.Point(224, 40);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(48, 16);
         this.label1.TabIndex = 3;
         this.label1.Text = "Seconds";
         this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
         // 
         // sampleRate
         // 
         this.sampleRate.Location = new System.Drawing.Point(168, 40);
         this.sampleRate.Maximum = new System.Decimal(new int[] {
                                                                   65535,
                                                                   0,
                                                                   0,
                                                                   0});
         this.sampleRate.Name = "sampleRate";
         this.sampleRate.Size = new System.Drawing.Size(56, 20);
         this.sampleRate.TabIndex = 2;
         this.sampleRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         this.sampleRate.Value = new System.Decimal(new int[] {
                                                                 10,
                                                                 0,
                                                                 0,
                                                                 0});
         // 
         // enableRolloverCheckBox
         // 
         this.enableRolloverCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.enableRolloverCheckBox.Location = new System.Drawing.Point(16, 48);
         this.enableRolloverCheckBox.Name = "enableRolloverCheckBox";
         this.enableRolloverCheckBox.Size = new System.Drawing.Size(120, 16);
         this.enableRolloverCheckBox.TabIndex = 1;
         this.enableRolloverCheckBox.Text = "Enable Rollover";
         // 
         // syncClockCheckBox
         // 
         this.syncClockCheckBox.Checked = true;
         this.syncClockCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
         this.syncClockCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.syncClockCheckBox.Location = new System.Drawing.Point(16, 24);
         this.syncClockCheckBox.Name = "syncClockCheckBox";
         this.syncClockCheckBox.Size = new System.Drawing.Size(128, 16);
         this.syncClockCheckBox.TabIndex = 0;
         this.syncClockCheckBox.Text = "Synchronize Clock";
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.temperatureLowAlarm);
         this.groupBox2.Controls.Add(this.enableTempLowAlarm);
         this.groupBox2.Controls.Add(this.enableTempHighAlarm);
         this.groupBox2.Controls.Add(this.suta);
         this.groupBox2.Controls.Add(this.temperatureResolution);
         this.groupBox2.Controls.Add(this.label5);
         this.groupBox2.Controls.Add(this.enableTemperatureSampling);
         this.groupBox2.Controls.Add(this.temperatureHighAlarm);
         this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
         this.groupBox2.Location = new System.Drawing.Point(0, 80);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(442, 112);
         this.groupBox2.TabIndex = 1;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Temperature";
         // 
         // temperatureLowAlarm
         // 
         this.temperatureLowAlarm.DecimalPlaces = 1;
         this.temperatureLowAlarm.Increment = new System.Decimal(new int[] {
                                                                              5,
                                                                              0,
                                                                              0,
                                                                              65536});
         this.temperatureLowAlarm.Location = new System.Drawing.Point(384, 56);
         this.temperatureLowAlarm.Maximum = new System.Decimal(new int[] {
                                                                            128,
                                                                            0,
                                                                            0,
                                                                            0});
         this.temperatureLowAlarm.Minimum = new System.Decimal(new int[] {
                                                                            30,
                                                                            0,
                                                                            0,
                                                                            -2147483648});
         this.temperatureLowAlarm.Name = "temperatureLowAlarm";
         this.temperatureLowAlarm.Size = new System.Drawing.Size(48, 20);
         this.temperatureLowAlarm.TabIndex = 12;
         this.temperatureLowAlarm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         this.temperatureLowAlarm.Value = new System.Decimal(new int[] {
                                                                          10,
                                                                          0,
                                                                          0,
                                                                          0});
         // 
         // enableTempLowAlarm
         // 
         this.enableTempLowAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.enableTempLowAlarm.Location = new System.Drawing.Point(240, 56);
         this.enableTempLowAlarm.Name = "enableTempLowAlarm";
         this.enableTempLowAlarm.Size = new System.Drawing.Size(144, 16);
         this.enableTempLowAlarm.TabIndex = 11;
         this.enableTempLowAlarm.Text = "Enable Low Alarm (C)";
         // 
         // enableTempHighAlarm
         // 
         this.enableTempHighAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.enableTempHighAlarm.Location = new System.Drawing.Point(240, 24);
         this.enableTempHighAlarm.Name = "enableTempHighAlarm";
         this.enableTempHighAlarm.Size = new System.Drawing.Size(144, 16);
         this.enableTempHighAlarm.TabIndex = 10;
         this.enableTempHighAlarm.Text = "Enable High Alarm (C)";
         // 
         // suta
         // 
         this.suta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.suta.Location = new System.Drawing.Point(240, 88);
         this.suta.Name = "suta";
         this.suta.Size = new System.Drawing.Size(200, 16);
         this.suta.TabIndex = 9;
         this.suta.Text = "Start Mission Upon Temp. Alarm";
         this.suta.CheckedChanged += new System.EventHandler(this.suta_CheckedChanged);
         // 
         // temperatureResolution
         // 
         this.temperatureResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.temperatureResolution.Items.AddRange(new object[] {
                                                                   "High Resolution",
                                                                   "Low Resolution"});
         this.temperatureResolution.Location = new System.Drawing.Point(88, 56);
         this.temperatureResolution.MaxDropDownItems = 2;
         this.temperatureResolution.Name = "temperatureResolution";
         this.temperatureResolution.Size = new System.Drawing.Size(120, 21);
         this.temperatureResolution.TabIndex = 4;
         // 
         // label5
         // 
         this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.label5.Location = new System.Drawing.Point(16, 56);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(64, 24);
         this.label5.TabIndex = 2;
         this.label5.Text = "Resolution";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // enableTemperatureSampling
         // 
         this.enableTemperatureSampling.Checked = true;
         this.enableTemperatureSampling.CheckState = System.Windows.Forms.CheckState.Checked;
         this.enableTemperatureSampling.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.enableTemperatureSampling.Location = new System.Drawing.Point(16, 24);
         this.enableTemperatureSampling.Name = "enableTemperatureSampling";
         this.enableTemperatureSampling.Size = new System.Drawing.Size(192, 16);
         this.enableTemperatureSampling.TabIndex = 0;
         this.enableTemperatureSampling.Text = "Enable Temperature Sampling";
         // 
         // temperatureHighAlarm
         // 
         this.temperatureHighAlarm.DecimalPlaces = 1;
         this.temperatureHighAlarm.Increment = new System.Decimal(new int[] {
                                                                               5,
                                                                               0,
                                                                               0,
                                                                               65536});
         this.temperatureHighAlarm.Location = new System.Drawing.Point(384, 24);
         this.temperatureHighAlarm.Maximum = new System.Decimal(new int[] {
                                                                             128,
                                                                             0,
                                                                             0,
                                                                             0});
         this.temperatureHighAlarm.Minimum = new System.Decimal(new int[] {
                                                                             30,
                                                                             0,
                                                                             0,
                                                                             -2147483648});
         this.temperatureHighAlarm.Name = "temperatureHighAlarm";
         this.temperatureHighAlarm.Size = new System.Drawing.Size(48, 20);
         this.temperatureHighAlarm.TabIndex = 8;
         this.temperatureHighAlarm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         this.temperatureHighAlarm.Value = new System.Decimal(new int[] {
                                                                           50,
                                                                           0,
                                                                           0,
                                                                           0});
         // 
         // humidityGroupBox
         // 
         this.humidityGroupBox.Controls.Add(this.humidityLowAlarm);
         this.humidityGroupBox.Controls.Add(this.enableHumdHighAlarm);
         this.humidityGroupBox.Controls.Add(this.enableHumdLowAlarm);
         this.humidityGroupBox.Controls.Add(this.humidityResolution);
         this.humidityGroupBox.Controls.Add(this.label10);
         this.humidityGroupBox.Controls.Add(this.enableHumiditySampling);
         this.humidityGroupBox.Controls.Add(this.humidityHighAlarm);
         this.humidityGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.humidityGroupBox.Location = new System.Drawing.Point(0, 192);
         this.humidityGroupBox.Name = "humidityGroupBox";
         this.humidityGroupBox.Size = new System.Drawing.Size(442, 88);
         this.humidityGroupBox.TabIndex = 9;
         this.humidityGroupBox.TabStop = false;
         this.humidityGroupBox.Text = "Humidity";
         // 
         // humidityLowAlarm
         // 
         this.humidityLowAlarm.DecimalPlaces = 1;
         this.humidityLowAlarm.Increment = new System.Decimal(new int[] {
                                                                           5,
                                                                           0,
                                                                           0,
                                                                           65536});
         this.humidityLowAlarm.Location = new System.Drawing.Point(384, 56);
         this.humidityLowAlarm.Name = "humidityLowAlarm";
         this.humidityLowAlarm.Size = new System.Drawing.Size(48, 20);
         this.humidityLowAlarm.TabIndex = 14;
         this.humidityLowAlarm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         this.humidityLowAlarm.Value = new System.Decimal(new int[] {
                                                                       20,
                                                                       0,
                                                                       0,
                                                                       0});
         // 
         // enableHumdHighAlarm
         // 
         this.enableHumdHighAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.enableHumdHighAlarm.Location = new System.Drawing.Point(224, 24);
         this.enableHumdHighAlarm.Name = "enableHumdHighAlarm";
         this.enableHumdHighAlarm.Size = new System.Drawing.Size(160, 16);
         this.enableHumdHighAlarm.TabIndex = 13;
         this.enableHumdHighAlarm.Text = "Enable High Alarm (%RH)";
         // 
         // enableHumdLowAlarm
         // 
         this.enableHumdLowAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.enableHumdLowAlarm.Location = new System.Drawing.Point(224, 56);
         this.enableHumdLowAlarm.Name = "enableHumdLowAlarm";
         this.enableHumdLowAlarm.Size = new System.Drawing.Size(160, 16);
         this.enableHumdLowAlarm.TabIndex = 12;
         this.enableHumdLowAlarm.Text = "Enable Low Alarm (%RH)";
         // 
         // humidityResolution
         // 
         this.humidityResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.humidityResolution.Items.AddRange(new object[] {
                                                                "High Resolution",
                                                                "Low Resolution"});
         this.humidityResolution.Location = new System.Drawing.Point(88, 56);
         this.humidityResolution.MaxDropDownItems = 2;
         this.humidityResolution.Name = "humidityResolution";
         this.humidityResolution.Size = new System.Drawing.Size(120, 21);
         this.humidityResolution.TabIndex = 4;
         // 
         // label10
         // 
         this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.label10.Location = new System.Drawing.Point(16, 56);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(64, 24);
         this.label10.TabIndex = 2;
         this.label10.Text = "Resolution";
         this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // enableHumiditySampling
         // 
         this.enableHumiditySampling.Checked = true;
         this.enableHumiditySampling.CheckState = System.Windows.Forms.CheckState.Checked;
         this.enableHumiditySampling.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.enableHumiditySampling.Location = new System.Drawing.Point(16, 24);
         this.enableHumiditySampling.Name = "enableHumiditySampling";
         this.enableHumiditySampling.Size = new System.Drawing.Size(192, 16);
         this.enableHumiditySampling.TabIndex = 0;
         this.enableHumiditySampling.Text = "Enable Humidity Sampling";
         // 
         // humidityHighAlarm
         // 
         this.humidityHighAlarm.DecimalPlaces = 1;
         this.humidityHighAlarm.Increment = new System.Decimal(new int[] {
                                                                            5,
                                                                            0,
                                                                            0,
                                                                            65536});
         this.humidityHighAlarm.Location = new System.Drawing.Point(384, 24);
         this.humidityHighAlarm.Name = "humidityHighAlarm";
         this.humidityHighAlarm.Size = new System.Drawing.Size(48, 20);
         this.humidityHighAlarm.TabIndex = 13;
         this.humidityHighAlarm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         this.humidityHighAlarm.Value = new System.Decimal(new int[] {
                                                                        80,
                                                                        0,
                                                                        0,
                                                                        0});
         // 
         // startNewMission
         // 
         this.startNewMission.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.startNewMission.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.startNewMission.Location = new System.Drawing.Point(80, 296);
         this.startNewMission.Name = "startNewMission";
         this.startNewMission.Size = new System.Drawing.Size(136, 24);
         this.startNewMission.TabIndex = 11;
         this.startNewMission.Text = "Start New Mission";
         this.startNewMission.Click += new System.EventHandler(this.startNewMission_Click);
         // 
         // Cancel
         // 
         this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.Cancel.Location = new System.Drawing.Point(240, 296);
         this.Cancel.Name = "Cancel";
         this.Cancel.Size = new System.Drawing.Size(88, 24);
         this.Cancel.TabIndex = 12;
         this.Cancel.Text = "Cancel";
         // 
         // StartMissionForm
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(442, 362);
         this.ControlBox = false;
         this.Controls.Add(this.Cancel);
         this.Controls.Add(this.startNewMission);
         this.Controls.Add(this.humidityGroupBox);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.groupBox1);
         this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MaximumSize = new System.Drawing.Size(448, 368);
         this.MinimizeBox = false;
         this.MinimumSize = new System.Drawing.Size(448, 368);
         this.Name = "StartMissionForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Start Mission";
         this.groupBox1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.startDelay)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sampleRate)).EndInit();
         this.groupBox2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.temperatureLowAlarm)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.temperatureHighAlarm)).EndInit();
         this.humidityGroupBox.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.humidityLowAlarm)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.humidityHighAlarm)).EndInit();
         this.ResumeLayout(false);

      }
		#endregion

      private void suta_CheckedChanged(object sender, System.EventArgs e)
      {
         if(suta.Checked)
         {
            enableTempHighAlarm.Checked = true;
            enableTempLowAlarm.Checked = true;
         }
      }

      private void startNewMission_Click(object sender, System.EventArgs e)
      {
         counter = 0;
         try
         {
            adapter.beginExclusive(true);

            byte[] buffer = new byte [10];
            // make sure no mission is running on device
            buffer[0] = OneWireContainer41.STOP_MISSION_PW_COMMAND;
            buffer[9] = (byte)0xFF;

            if(owc41!=null)
            {
               // stop mission on single device
               adapter.reset();
               adapter.assertSelect(owc41.Address);
               adapter.dataBlock(buffer, 0, buffer.Length);

               StartNewMission();
            }
            else
            {
               // stop mission on all device
               adapter.reset();
               adapter.putByte(0xCC); // Skip rom
               adapter.dataBlock(buffer, 0, buffer.Length);

               adapter.targetFamily(0x41);
               owc41 = (OneWireContainer41)adapter.FirstDeviceContainer;
               for(; owc41!=null; owc41 = (OneWireContainer41)adapter.NextDeviceContainer)
                  StartNewMission();
            }
            MessageBox.Show("Succesfully missioned " + counter + " device(s)");
         }
         catch(Exception ex)
         {
            MessageBox.Show("Exception: " + ex.Message + "\n" + ex.StackTrace);
         }
         finally
         {
            adapter.endExclusive();
         }
      }

      private void StartNewMission()
      {
         int tempBytes = 0;
         int humdBytes = 0;

         if(enableTemperatureSampling.Checked)
         {
            tempBytes++;
            if(temperatureResolution.Text.IndexOf("High")>=0)
               tempBytes++;
         }

         if(enableHumiditySampling.Checked)
         {
            humdBytes++;
            if(humidityResolution.Text.IndexOf("High")>=0)
               humdBytes++;
         }

         if(tempBytes==1)
            owc41.setMissionResolution(0, owc41.getMissionResolutions(0)[0]);
         else
            owc41.setMissionResolution(0, owc41.getMissionResolutions(0)[1]);
         if(humdBytes==1)
            owc41.setMissionResolution(1, owc41.getMissionResolutions(1)[0]);
         else
            owc41.setMissionResolution(1, owc41.getMissionResolutions(1)[1]);


         if(enableTempHighAlarm.Checked)
         {
            owc41.setMissionAlarm(
               OneWireContainer41.TEMPERATURE_CHANNEL,
               1, //TemperatureContainer.ALARM_HIGH,
               (double)temperatureHighAlarm.Value);
            owc41.setMissionAlarmEnable(
               OneWireContainer41.TEMPERATURE_CHANNEL,
               1, //TemperatureContainer.ALARM_HIGH,
               true);
         }
         else
         {
            owc41.setMissionAlarmEnable(
               OneWireContainer41.TEMPERATURE_CHANNEL,
               1, //TemperatureContainer.ALARM_HIGH,
               false);
         }

         if(enableTempLowAlarm.Checked)
         {
            owc41.setMissionAlarm(
               OneWireContainer41.TEMPERATURE_CHANNEL,
               0, //TemperatureContainer.ALARM_LOW,
               (double)temperatureLowAlarm.Value);
            owc41.setMissionAlarmEnable(
               OneWireContainer41.TEMPERATURE_CHANNEL,
               0, //TemperatureContainer.ALARM_LOW,
               true);
         }
         else
         {
            owc41.setMissionAlarmEnable(
               OneWireContainer41.TEMPERATURE_CHANNEL,
               0, //TemperatureContainer.ALARM_LOW,
               false);
         }

         if(enableHumdHighAlarm.Checked)
         {
            owc41.setMissionAlarm(
               OneWireContainer41.DATA_CHANNEL,
               1, //ADContainer.ALARM_HIGH,
               (double)humidityHighAlarm.Value);
            owc41.setMissionAlarmEnable(
               OneWireContainer41.DATA_CHANNEL,
               1, //ADContainer.ALARM_HIGH,
               true);
         }
         else
         {
            owc41.setMissionAlarmEnable(
               OneWireContainer41.DATA_CHANNEL,
               1, //ADContainer.ALARM_HIGH,
               false);
         }

         if(enableHumdLowAlarm.Checked)
         {
            owc41.setMissionAlarm(
               OneWireContainer41.DATA_CHANNEL,
               0, //ADContainer.ALARM_LOW,
               (double)humidityLowAlarm.Value);
            owc41.setMissionAlarmEnable(
               OneWireContainer41.DATA_CHANNEL,
               0, //ADContainer.ALARM_LOW,
               true);
         }
         else
         {
            owc41.setMissionAlarmEnable(
               OneWireContainer41.DATA_CHANNEL,
               0, //ADContainer.ALARM_LOW,
               false);
         }

         owc41.setStartUponTemperatureAlarmEnable(suta.Checked);

         owc41.startNewMission((int)sampleRate.Value, (int)startDelay.Value, 
            enableRolloverCheckBox.Checked, syncClockCheckBox.Checked, 
            new bool[]{tempBytes>0, humdBytes>0});

         counter++;
      }

	}
}
