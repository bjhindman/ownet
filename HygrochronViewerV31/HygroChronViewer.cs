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
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Management; // must add reference to System.Management.dll
using com.dalsemi.onewire;
using com.dalsemi.onewire.adapter;
using com.dalsemi.onewire.container;
using com.dalsemi.onewire.utils;
using Microsoft.Win32;
using System.Reflection;
using System.Diagnostics;
using NPlot;
using NPlot.Windows;

namespace HygrochronViewer
{
   public class MainForm : System.Windows.Forms.Form
   {
      struct MissionCache
      {
         public double[] temperatureValues;
         public double[] humidityValues;
         public double[] time;
      }
      // Instantiate other forms used in the program
      //private FormProgressIndicator progressForm = new FormProgressIndicator(); // adapter selection "progress bar" form.
      public FormProgressIndicator progressForm = new FormProgressIndicator();

      private const string VersionString = "@AUTOVERSION@";

      private TemperatureCompensationOptions tcOptions;

      private PrintDocument print_document;
      private PageSettings pgSettings;
      private DSPortAdapter adapter = null;
      private MissionCache missionCache;
      private Hashtable DeviceHashtable = new Hashtable();
      private bool adapterIsSet = false;
      private RegistryKey key;
      private byte[] globalReadOnlyPassword = null;
      private byte[] globalReadWritePassword = null;

      protected Font HeaderFont = new Font("Courier New", 10, FontStyle.Bold);
      private System.Windows.Forms.MenuItem useHumdHysteresisMenuItem;
      private System.Windows.Forms.MenuItem menuItem5;
      private System.Windows.Forms.MenuItem menuItem35;
      private System.Windows.Forms.MenuItem menuItem33;
      private System.Windows.Forms.MenuItem menuItem36;
      private System.Windows.Forms.MenuItem menuItem34;
      private System.Windows.Forms.MenuItem menuItem37;
      private IContainer components;
      protected Font NormalFont = new Font("Courier New", 10, FontStyle.Regular);
      private MenuItem menuItem10;
      private BackgroundWorker backgroundWorkerAdapterDetection;
      private String previousDeviceSelection = "";

      public MainForm()
      {
         InitializeComponent();

         searchOptions.SelectedIndex = 0;

         tcOptions = new TemperatureCompensationOptions();

         setToolTips();


         // set up printer
         pgSettings = new PageSettings();
         print_document = new PrintDocument();
         print_document.DefaultPageSettings = pgSettings; // not 100% necessary.
         print_document.PrintPage += new PrintPageEventHandler(pd_PrintPage);
      }


      [STAThread]
      static void Main() 
      {
         Application.Run(new MainForm());
      }


      private void CloseMenuItem_Click(object sender, System.EventArgs e)
      {
         this.Dispose();
      }

      
      protected override void Dispose(bool disposing)
      {
         if(key==null)
         {
            key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Maxim Integrated Products\\HygrochronViewer");
         }

         if(adapter!=null)
         {
            if(adapterIsSet)
            {
               key.SetValue("adapterName", adapter.AdapterName);
               key.SetValue("adapterPort", adapter.PortName);
            }
            adapter.endExclusive();
            adapter.freePort();
            adapter = null;
         }

         key.SetValue("overdrive", overdriveMenuItem.Checked);

         key.SetValue("Temperature Calibration: Enabled", useTemperatureCalibrationRegisters.Checked);
         key.SetValue("Humidity Calibration: Enabled", useHumidityCalibrationRegisters.Checked);
         key.SetValue("Temperature Compensation: Enabled", useTempCompensationMenuItem.Checked);
         key.SetValue("Temperature Compensation: Default", tcOptions.defaultTemperatureValue.Value);
         key.SetValue("Temperature Compensation: Ignore Log", tcOptions.overrideTemperatureLog.Checked);

         if(rememberGlobalPasswordMenuItem.Checked)
         {
            if(globalReadOnlyPassword!=null)
               key.SetValue("Global Read-Only Password", 
                  com.dalsemi.onewire.utils.Convert.toHexString(globalReadOnlyPassword, 0, 8, " "));
            if(globalReadWritePassword!=null)
               key.SetValue("Global Read-Write Password", 
                  com.dalsemi.onewire.utils.Convert.toHexString(globalReadWritePassword, 0, 8, " "));
         }
         else
         {
            key.DeleteValue("Global Read-Only Password", false);
            key.DeleteValue("Global Read-Write Password", false);
         }

         key.Close();

         tcOptions.Dispose();

         print_document.Dispose();

         HeaderFont.Dispose();
         NormalFont.Dispose();

         base.Dispose( disposing );
      }

      
      public void setToolTips()
      {
         ToolTip tooltip = new ToolTip();
         // Set up the delays for the ToolTip.
         tooltip.AutoPopDelay = 5000;
         tooltip.InitialDelay = 1000;
         tooltip.ReshowDelay = 500;
         // Force the ToolTip text to be displayed whether or not the form is active.
         tooltip.ShowAlways = true;

         tooltip.SetToolTip(DeviceListbox, 
            "Select a DS1922/DS2422 device from this list" + Environment.NewLine +
            "to load it's current mission results or " + Environment.NewLine +
            "start a new mission.");
         tooltip.SetToolTip(startNewMissionButton, 
            "Starts a new mission on the selected device only.");
         tooltip.SetToolTip(stopMissionButton, 
            "Stops the currently running mission on the selected device only.");
         tooltip.SetToolTip(LoadResultsButton, 
            "Loads the mission results from the selected device only.");
      }

      
      protected void updateDescriptionTab()
      {
         DescriptionTextBox.Clear();

         if(owc41!=null)
         {
            DescriptionTextBox.Font = NormalFont;

            DescriptionTextBox.AppendText(Environment.NewLine);
            addDescriptionText("Device: ", owc41.ToString());
            //addDescriptionText("Description: ", owc41.getDescription());

            try
            {
               adapter.beginExclusive(true);
               if(enableGlobalPasswordsMenuItem.Checked)
               {
                  if(globalReadWritePassword!=null)
                     owc41.setContainerReadWritePassword(globalReadWritePassword, 0);
                  if(globalReadOnlyPassword!=null)
                     owc41.setContainerReadOnlyPassword(globalReadOnlyPassword, 0);
               }
               if(!owc41.isMissionLoaded())
               {
                  Status.Text = "Loading...";
                  if(overdriveMenuItem.Checked)
                     owc41.setSpeed(DSPortAdapter.SPEED_OVERDRIVE, true);
                  else
                     owc41.setSpeed(DSPortAdapter.SPEED_REGULAR, false);
                  this.Cursor = Cursors.WaitCursor;
                  owc41.loadMissionResults(); 
                  this.Cursor = Cursors.Default;
                  updateContainerProperties(owc41);
               }
               
               addDescriptionText("Is Mission Running: ", owc41.MissionRunning.ToString());

               if(owc41.MissionSUTA)
                  addDescriptionText("Start Upon Temperature Alarm: ", 
                     (owc41.MissionWFTA?"Waiting for Temperature Alarm":"Got Temperature Alarm, Mission Started"));

               addDescriptionText("Sample Rate: ", owc41.getMissionSampleRate(0) + " (secs)");

               addDescriptionText("Mission Start Time: ",
                  (new System.DateTime(owc41.getMissionTimeStamp(0))).ToString());

               addDescriptionText("Mission Sample Count: ", owc41.getMissionSampleCount(0).ToString());

               addDescriptionText("Rollover Enabled: ", owc41.MissionRolloverEnabled.ToString());

               if(owc41.MissionRolloverEnabled)
               {
                  addDescriptionText("Rollover Occurred: ", owc41.hasMissionRolloverOccurred().ToString());

                  if(owc41.hasMissionRolloverOccurred())
                  {
                     addDescriptionText("First Sample Timestamp: ",
                        (new System.DateTime(
                        owc41.getMissionSampleTimeStamp(OneWireContainer41.TEMPERATURE_CHANNEL,0))).ToString());
                     addDescriptionText("Total Mission Samples: ", owc41.getMissionSampleCountTotal(0).ToString());
                  }
               }

               addDescriptionText("Temperature Logging: ",
                  !owc41.getMissionChannelEnable(OneWireContainer41.TEMPERATURE_CHANNEL)?
                  "Disabled":
                  owc41.getMissionResolution(OneWireContainer41.TEMPERATURE_CHANNEL) + " bit");
               addDescriptionText("Temperature Low Alarm: ",
                  !owc41.getMissionAlarmEnable(OneWireContainer41.TEMPERATURE_CHANNEL, 0)?
                  "Disabled":
                  owc41.getMissionAlarm(OneWireContainer41.TEMPERATURE_CHANNEL, 0) + "C ("
                  + (owc41.hasMissionAlarmed(OneWireContainer41.TEMPERATURE_CHANNEL, 0)?
                  "ALARM)":"no alarm)"));
               addDescriptionText("Temperature High Alarm: ",
                  !owc41.getMissionAlarmEnable(OneWireContainer41.TEMPERATURE_CHANNEL, 1)?
                  "Disabled":
                  owc41.getMissionAlarm(OneWireContainer41.TEMPERATURE_CHANNEL, 1) + "C ("
                  + (owc41.hasMissionAlarmed(OneWireContainer41.TEMPERATURE_CHANNEL, 1)?
                  "ALARM)":"no alarm)"));

               addDescriptionText(owc41.getMissionLabel(OneWireContainer41.DATA_CHANNEL) + " Logging: ",
                  !owc41.getMissionChannelEnable(OneWireContainer41.DATA_CHANNEL)?
                  "Disabled":
                  owc41.getMissionResolution(OneWireContainer41.DATA_CHANNEL) + " bit");
               addDescriptionText(owc41.getMissionLabel(OneWireContainer41.DATA_CHANNEL) + " Low Alarm: ",
                  !owc41.getMissionAlarmEnable(OneWireContainer41.DATA_CHANNEL, 0)?
                  "Disabled":
                  owc41.getMissionAlarm(OneWireContainer41.DATA_CHANNEL, 0) + " ("
                  + (owc41.hasMissionAlarmed(OneWireContainer41.DATA_CHANNEL, 0)?
                  "ALARM)":"no alarm)"));
               addDescriptionText(owc41.getMissionLabel(OneWireContainer41.DATA_CHANNEL) + " High Alarm: ",
                  !owc41.getMissionAlarmEnable(OneWireContainer41.DATA_CHANNEL, 1)?
                  "Disabled":
                  owc41.getMissionAlarm(OneWireContainer41.DATA_CHANNEL, 1) + " ("
                  + (owc41.hasMissionAlarmed(OneWireContainer41.DATA_CHANNEL, 1)?
                  "ALARM)":"no alarm)"));

               addDescriptionText("Total Device Samples: ", owc41.getDeviceSampleCount().ToString());
            }
            catch(Exception e)
            {
               addDescriptionText("Fatal Error: ", e.Message + Environment.NewLine + e.StackTrace);
               Status.Text = e.Message;
            }
            finally
            {
               if (adapter != null)
               {
                  adapter.endExclusive();
               }
            }
         }
      }

      
      protected void addDescriptionText(string header, string text)
      {
         DescriptionTextBox.SelectionFont = HeaderFont;
         DescriptionTextBox.AppendText(header);
         DescriptionTextBox.SelectionFont = NormalFont;
         DescriptionTextBox.AppendText(text);
         DescriptionTextBox.AppendText(Environment.NewLine);
         DescriptionTextBox.AppendText(Environment.NewLine);
      }


      private void StartMissionMenuItem_Click(object sender, System.EventArgs e)
      {
         StartMissionForm form = new StartMissionForm(adapter, null);
         if(form.ShowDialog()==DialogResult.Cancel)
            Status.Text = "Gang -> Start Mission canceled!";
         else
            Status.Text = "Gang -> Start Mission complete!";
         form.Dispose();
         updateDescriptionTab();
      }

      
      private void startNewMissionButton_Click(object sender, System.EventArgs e)
      {
         if(owc41!=null)
         {
            StartMissionForm form = new StartMissionForm(adapter, owc41);
            if(form.ShowDialog()==DialogResult.Cancel)
               Status.Text = "Start Mission canceled!";
            else
               Status.Text = "Start Mission complete!";
            updateDescriptionTab();
         }
      }

      
      private void StopMissionMenuItem_Click(object sender, System.EventArgs e)
      {
         byte[] buffer = new byte [11];
         buffer[0] = (byte)0x0CC; // 1-Wire Skip ROM
         buffer[1] = OneWireContainer41.STOP_MISSION_PW_COMMAND;
         // send it a few times to make sure none of the devices miss it.
         for(int i=0; i<3; i++)
         {
            try
            {
               adapter.beginExclusive(true);
               if(overdriveMenuItem.Checked)
                  owc41.setSpeed(DSPortAdapter.SPEED_OVERDRIVE, true);
               else
                  owc41.setSpeed(DSPortAdapter.SPEED_REGULAR, false);
               buffer[10] = (byte)0xFF;
               adapter.reset();
               adapter.dataBlock(buffer, 0,  10);
            }
            catch(Exception exc)
            {
               Status.Text = exc.Message;
            }
            finally
            {
               adapter.endExclusive();
            }
         }
         updateDescriptionTab();
         Status.Text = "Gang -> Stop Mission complete!";
      }

      
      private void stopMissionButton_Click(object sender, System.EventArgs e)
      {
         if(owc41!=null)
         {
            try
            {
               adapter.beginExclusive(true);
               if(enableGlobalPasswordsMenuItem.Checked)
               {
                  if(globalReadWritePassword!=null)
                     owc41.setContainerReadWritePassword(globalReadWritePassword, 0);
                  if(globalReadOnlyPassword!=null)
                     owc41.setContainerReadOnlyPassword(globalReadOnlyPassword, 0);
               }
               if(overdriveMenuItem.Checked)
                  owc41.setSpeed(DSPortAdapter.SPEED_OVERDRIVE, true);
               else
                  owc41.setSpeed(DSPortAdapter.SPEED_REGULAR, false);
               owc41.stopMission();
               Status.Text = "Stop Mission Succeeded";
               updateDescriptionTab();
            }
            catch(Exception ex)
            {
               Status.Text = "Stop Mission failed: " + ex.Message;
            }
            finally
            {
               adapter.endExclusive();
            }
         }
      }


      private void LoadMissionMenuItem_Click(object sender, System.EventArgs e)
      {
         try
         {
            adapter.beginExclusive(true);
            this.Cursor = Cursors.WaitCursor;
            foreach(OneWireContainer41 l_owc41 in DeviceListbox.Items)
            {
               try
               {
                  if(enableGlobalPasswordsMenuItem.Checked)
                  {
                     if(globalReadWritePassword!=null)
                        l_owc41.setContainerReadWritePassword(globalReadWritePassword, 0);
                     if(globalReadOnlyPassword!=null)
                        l_owc41.setContainerReadOnlyPassword(globalReadOnlyPassword, 0);
                  }

                  if(overdriveMenuItem.Checked)
                     l_owc41.setSpeed(DSPortAdapter.SPEED_OVERDRIVE, true);
                  else
                     l_owc41.setSpeed(DSPortAdapter.SPEED_REGULAR, false);

                  l_owc41.loadMissionResults();
                  updateContainerProperties(l_owc41);
               }
               catch(Exception ex1)
               {
                  MessageBox.Show("Error w/ button: " + l_owc41 + ", error: " + ex1.Message + Environment.NewLine + ex1.StackTrace);
               }
            }
            this.Cursor = Cursors.Default;
            if(owc41!=null)
            {
               updateDescriptionTab();
               graphHumidity();
               graphTemperature();
            }
            MessageBox.Show("Done Loading Mission Results");
         }
         catch(Exception ex2)
         {
            MessageBox.Show("Fatal Error: " + ex2.Message + Environment.NewLine + ex2.StackTrace);
         }
         finally
         {
            adapter.endExclusive();
         }
      }


      private void LoadResultsButton_Click(object sender, System.EventArgs e)
      {
         if(owc41!=null)
         {
            Status.Text = "Loading Mission Results...";
            try
            {
               adapter.beginExclusive(true);

               if(enableGlobalPasswordsMenuItem.Checked)
               {
                  if(globalReadWritePassword!=null)
                     owc41.setContainerReadWritePassword(globalReadWritePassword, 0);
                  if(globalReadOnlyPassword!=null)
                     owc41.setContainerReadOnlyPassword(globalReadOnlyPassword, 0);
               }

               if(overdriveMenuItem.Checked)
                  owc41.setSpeed(DSPortAdapter.SPEED_OVERDRIVE, true);
               else
                  owc41.setSpeed(DSPortAdapter.SPEED_REGULAR, false);

               this.Cursor = Cursors.WaitCursor;
               owc41.loadMissionResults();
               this.Cursor = Cursors.Default;

               updateContainerProperties(owc41);

               graphHumidity();
               graphTemperature();

               Status.Text += " Done.";
               updateDescriptionTab();
            }
            catch(Exception exc) 
            {
               Status.Text = exc.Message; 
               addDescriptionText("message:", exc.Message); 
               addDescriptionText("stacktrace: ", exc.StackTrace); 
            }
            finally
            {
               if (adapter != null)
               {
                  adapter.endExclusive();
               }
            }
         }
      }


      private void updateContainerProperties(OneWireContainer41 owc)
      {
         owc.TemperatureCalibrationRegisterUsage = useTemperatureCalibrationRegisters.Checked;
         owc.HumidityCalibrationRegisterUsage = useHumidityCalibrationRegisters.Checked;
         owc.TemperatureCompensationUsage = useTempCompensationMenuItem.Checked;
         owc.setDefaultTemperatureCompensationValue(
            (double)tcOptions.defaultTemperatureValue.Value,
            tcOptions.overrideTemperatureLog.Checked);
         if(enableGlobalPasswordsMenuItem.Checked)
         {
            if(globalReadWritePassword!=null)
               owc.setContainerReadWritePassword(globalReadWritePassword, 0);
            if(globalReadOnlyPassword!=null)
               owc.setContainerReadOnlyPassword(globalReadOnlyPassword, 0);
         }
      }


      #region Windows Form Designer generated code

      private System.Windows.Forms.TabControl DeviceOptions;
      private System.Windows.Forms.MenuItem FileMenuItem;
      private System.Windows.Forms.MenuItem CloseMenuItem;
      private System.Windows.Forms.ListBox DeviceListbox;
      private System.Windows.Forms.TabPage DescriptionTab;
      private System.Windows.Forms.MainMenu MainMenu;
      private System.Windows.Forms.MenuItem SeparatorMenuItem;
      private System.Timers.Timer timer1;
      private System.Windows.Forms.StatusBar StatusBar;
      private System.Windows.Forms.StatusBarPanel AdapterStatus;
      private System.Windows.Forms.StatusBarPanel Status;
      private System.Windows.Forms.RichTextBox DescriptionTextBox;
      private System.Windows.Forms.MenuItem GangMenuItem;
      private System.Windows.Forms.MenuItem StopMissionMenuItem;
      private System.Windows.Forms.MenuItem StartMissionMenuItem;
      private OneWireContainer41 owc41 = null;
      private System.Windows.Forms.TabPage TemperatureTab;
      private NPlot.Windows.PlotSurface2D TemperaturePlot;
      private System.Windows.Forms.TabPage HumidityTab;
      private System.Windows.Forms.Button startNewMissionButton;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.MenuItem menuItem2;
      private System.Windows.Forms.MenuItem menuItem3;
      private System.Windows.Forms.MenuItem useHysteresisCompensation;
      private System.Windows.Forms.MenuItem menuItem4;
      private System.Windows.Forms.MenuItem saveDescriptionMenuItem;
      private System.Windows.Forms.MenuItem saveTemperatureResultsMenuItem;
      private System.Windows.Forms.MenuItem saveHumidityResultsMenuItem;
      private System.Windows.Forms.MenuItem saveAllTemperatureResultsMenuItem;
      private System.Windows.Forms.MenuItem saveAllHumidityResultsMenuItem;
      private System.Windows.Forms.MenuItem printDescriptionMenuItem;
      private System.Windows.Forms.MenuItem printTemperatureResultsMenuItem;
      private System.Windows.Forms.MenuItem printHumidityResultsMenuItem;
      private System.Windows.Forms.MenuItem saveMenuItem;
      private System.Windows.Forms.MenuItem printMenuItem;
      private System.Windows.Forms.MenuItem LoadMissionMenuItem;
      private System.Windows.Forms.Button stopMissionButton;
      private System.Windows.Forms.MenuItem tempCompensationMenuItem;
      private System.Windows.Forms.MenuItem useTempCompensationMenuItem;
      private System.Windows.Forms.MenuItem tempCompensationOptionsMenuItem;
      private System.Windows.Forms.ComboBox searchOptions;
      private System.Windows.Forms.MenuItem menuItem1;
      private System.Windows.Forms.MenuItem menuItem6;
      private System.Windows.Forms.MenuItem setDeviceReadWritePasswordMenuItem;
      private System.Windows.Forms.MenuItem setContainerReadWritePasswordMenuItem;
      private System.Windows.Forms.MenuItem setDeviceReadOnlyPasswordMenuItem;
      private System.Windows.Forms.MenuItem setContainerReadOnlyPasswordMenuItem;
      private System.Windows.Forms.MenuItem hideAllPasswordTextMenuItem;
      private System.Windows.Forms.MenuItem enableDevicePasswordsMenuItem;
      private System.Windows.Forms.MenuItem disableDevicePasswordsMenuItem;
      private System.Windows.Forms.MenuItem menuItem8;
      private System.Windows.Forms.MenuItem menuItem9;
      private System.Windows.Forms.MenuItem menuItem19;
      private System.Windows.Forms.MenuItem menuItem20;
      private System.Windows.Forms.MenuItem menuItem21;
      private System.Windows.Forms.MenuItem menuItem22;
      private System.Windows.Forms.MenuItem menuItem23;
      private System.Windows.Forms.MenuItem menuItem24;
      private System.Windows.Forms.MenuItem menuItem29;
      private System.Windows.Forms.MenuItem overdriveMenuItem;
      private System.Windows.Forms.MenuItem menuItem30;
      private System.Windows.Forms.MenuItem setGlobalContainerReadWritePasswordMenuItem;
      private System.Windows.Forms.MenuItem setGlobalContainerReadOnlyPasswordMenuItem;
      private System.Windows.Forms.MenuItem rememberGlobalPasswordMenuItem;
      private System.Windows.Forms.MenuItem menuItem31;
      private System.Windows.Forms.MenuItem enableGlobalPasswordsMenuItem;
      private System.Windows.Forms.MenuItem menuItem7;
      private System.Windows.Forms.MenuItem menuItem32;
      private System.Windows.Forms.Button LoadResultsButton;
      private System.Windows.Forms.MenuItem useHumidityCalibrationRegisters;
      private System.Windows.Forms.MenuItem useTemperatureCalibrationRegisters;
      private NPlot.Windows.PlotSurface2D HumidityPlot;

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
         this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
         this.FileMenuItem = new System.Windows.Forms.MenuItem();
         this.saveMenuItem = new System.Windows.Forms.MenuItem();
         this.saveDescriptionMenuItem = new System.Windows.Forms.MenuItem();
         this.saveTemperatureResultsMenuItem = new System.Windows.Forms.MenuItem();
         this.saveHumidityResultsMenuItem = new System.Windows.Forms.MenuItem();
         this.saveAllTemperatureResultsMenuItem = new System.Windows.Forms.MenuItem();
         this.saveAllHumidityResultsMenuItem = new System.Windows.Forms.MenuItem();
         this.printMenuItem = new System.Windows.Forms.MenuItem();
         this.printDescriptionMenuItem = new System.Windows.Forms.MenuItem();
         this.printTemperatureResultsMenuItem = new System.Windows.Forms.MenuItem();
         this.printHumidityResultsMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem30 = new System.Windows.Forms.MenuItem();
         this.menuItem8 = new System.Windows.Forms.MenuItem();
         this.overdriveMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem29 = new System.Windows.Forms.MenuItem();
         this.menuItem9 = new System.Windows.Forms.MenuItem();
         this.menuItem19 = new System.Windows.Forms.MenuItem();
         this.menuItem20 = new System.Windows.Forms.MenuItem();
         this.menuItem21 = new System.Windows.Forms.MenuItem();
         this.menuItem22 = new System.Windows.Forms.MenuItem();
         this.menuItem23 = new System.Windows.Forms.MenuItem();
         this.menuItem24 = new System.Windows.Forms.MenuItem();
         this.SeparatorMenuItem = new System.Windows.Forms.MenuItem();
         this.CloseMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem2 = new System.Windows.Forms.MenuItem();
         this.menuItem3 = new System.Windows.Forms.MenuItem();
         this.useHumidityCalibrationRegisters = new System.Windows.Forms.MenuItem();
         this.tempCompensationMenuItem = new System.Windows.Forms.MenuItem();
         this.useTempCompensationMenuItem = new System.Windows.Forms.MenuItem();
         this.tempCompensationOptionsMenuItem = new System.Windows.Forms.MenuItem();
         this.useHysteresisCompensation = new System.Windows.Forms.MenuItem();
         this.useHumdHysteresisMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem4 = new System.Windows.Forms.MenuItem();
         this.useTemperatureCalibrationRegisters = new System.Windows.Forms.MenuItem();
         this.menuItem1 = new System.Windows.Forms.MenuItem();
         this.hideAllPasswordTextMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem31 = new System.Windows.Forms.MenuItem();
         this.enableGlobalPasswordsMenuItem = new System.Windows.Forms.MenuItem();
         this.rememberGlobalPasswordMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem33 = new System.Windows.Forms.MenuItem();
         this.setGlobalContainerReadWritePasswordMenuItem = new System.Windows.Forms.MenuItem();
         this.setGlobalContainerReadOnlyPasswordMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem36 = new System.Windows.Forms.MenuItem();
         this.menuItem5 = new System.Windows.Forms.MenuItem();
         this.menuItem35 = new System.Windows.Forms.MenuItem();
         this.menuItem6 = new System.Windows.Forms.MenuItem();
         this.setContainerReadWritePasswordMenuItem = new System.Windows.Forms.MenuItem();
         this.setContainerReadOnlyPasswordMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem7 = new System.Windows.Forms.MenuItem();
         this.setDeviceReadWritePasswordMenuItem = new System.Windows.Forms.MenuItem();
         this.setDeviceReadOnlyPasswordMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem32 = new System.Windows.Forms.MenuItem();
         this.enableDevicePasswordsMenuItem = new System.Windows.Forms.MenuItem();
         this.disableDevicePasswordsMenuItem = new System.Windows.Forms.MenuItem();
         this.GangMenuItem = new System.Windows.Forms.MenuItem();
         this.StopMissionMenuItem = new System.Windows.Forms.MenuItem();
         this.StartMissionMenuItem = new System.Windows.Forms.MenuItem();
         this.LoadMissionMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem34 = new System.Windows.Forms.MenuItem();
         this.menuItem37 = new System.Windows.Forms.MenuItem();
         this.DeviceListbox = new System.Windows.Forms.ListBox();
         this.DeviceOptions = new System.Windows.Forms.TabControl();
         this.DescriptionTab = new System.Windows.Forms.TabPage();
         this.DescriptionTextBox = new System.Windows.Forms.RichTextBox();
         this.TemperatureTab = new System.Windows.Forms.TabPage();
         this.TemperaturePlot = new NPlot.Windows.PlotSurface2D();
         this.HumidityTab = new System.Windows.Forms.TabPage();
         this.HumidityPlot = new NPlot.Windows.PlotSurface2D();
         this.startNewMissionButton = new System.Windows.Forms.Button();
         this.LoadResultsButton = new System.Windows.Forms.Button();
         this.timer1 = new System.Timers.Timer();
         this.StatusBar = new System.Windows.Forms.StatusBar();
         this.AdapterStatus = new System.Windows.Forms.StatusBarPanel();
         this.Status = new System.Windows.Forms.StatusBarPanel();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.stopMissionButton = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.searchOptions = new System.Windows.Forms.ComboBox();
         this.menuItem10 = new System.Windows.Forms.MenuItem();
         this.backgroundWorkerAdapterDetection = new System.ComponentModel.BackgroundWorker();
         this.DeviceOptions.SuspendLayout();
         this.DescriptionTab.SuspendLayout();
         this.TemperatureTab.SuspendLayout();
         this.HumidityTab.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.AdapterStatus)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Status)).BeginInit();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // MainMenu
         // 
         this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileMenuItem,
            this.menuItem2,
            this.menuItem1,
            this.GangMenuItem,
            this.menuItem34});
         // 
         // FileMenuItem
         // 
         this.FileMenuItem.Index = 0;
         this.FileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.saveMenuItem,
            this.printMenuItem,
            this.menuItem30,
            this.menuItem8,
            this.SeparatorMenuItem,
            this.CloseMenuItem});
         this.FileMenuItem.Text = "File";
         // 
         // saveMenuItem
         // 
         this.saveMenuItem.Index = 0;
         this.saveMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.saveDescriptionMenuItem,
            this.saveTemperatureResultsMenuItem,
            this.saveHumidityResultsMenuItem,
            this.saveAllTemperatureResultsMenuItem,
            this.saveAllHumidityResultsMenuItem});
         this.saveMenuItem.Text = "Save";
         // 
         // saveDescriptionMenuItem
         // 
         this.saveDescriptionMenuItem.Index = 0;
         this.saveDescriptionMenuItem.Text = "Description";
         this.saveDescriptionMenuItem.Click += new System.EventHandler(this.saveDescriptionMenuItem_Click);
         // 
         // saveTemperatureResultsMenuItem
         // 
         this.saveTemperatureResultsMenuItem.Index = 1;
         this.saveTemperatureResultsMenuItem.Text = "Temperature Results";
         this.saveTemperatureResultsMenuItem.Click += new System.EventHandler(this.saveTemperatureResultsMenuItem_Click);
         // 
         // saveHumidityResultsMenuItem
         // 
         this.saveHumidityResultsMenuItem.Index = 2;
         this.saveHumidityResultsMenuItem.Text = "Humidity Results";
         this.saveHumidityResultsMenuItem.Click += new System.EventHandler(this.saveHumidityResultsMenuItem_Click);
         // 
         // saveAllTemperatureResultsMenuItem
         // 
         this.saveAllTemperatureResultsMenuItem.Index = 3;
         this.saveAllTemperatureResultsMenuItem.Text = "All Temperature Results";
         this.saveAllTemperatureResultsMenuItem.Click += new System.EventHandler(this.saveAllTemperatureResultsMenuItem_Click);
         // 
         // saveAllHumidityResultsMenuItem
         // 
         this.saveAllHumidityResultsMenuItem.Index = 4;
         this.saveAllHumidityResultsMenuItem.Text = "All Humidity Results";
         this.saveAllHumidityResultsMenuItem.Click += new System.EventHandler(this.saveAllHumidityResultsMenuItem_Click);
         // 
         // printMenuItem
         // 
         this.printMenuItem.Index = 1;
         this.printMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.printDescriptionMenuItem,
            this.printTemperatureResultsMenuItem,
            this.printHumidityResultsMenuItem});
         this.printMenuItem.Text = "Print";
         // 
         // printDescriptionMenuItem
         // 
         this.printDescriptionMenuItem.Index = 0;
         this.printDescriptionMenuItem.Text = "Description";
         this.printDescriptionMenuItem.Click += new System.EventHandler(this.printDescriptionMenuItem_Click);
         // 
         // printTemperatureResultsMenuItem
         // 
         this.printTemperatureResultsMenuItem.Index = 1;
         this.printTemperatureResultsMenuItem.Text = "Temperature Results";
         this.printTemperatureResultsMenuItem.Click += new System.EventHandler(this.printTemperatureResultsMenuItem_Click);
         // 
         // printHumidityResultsMenuItem
         // 
         this.printHumidityResultsMenuItem.Index = 2;
         this.printHumidityResultsMenuItem.Text = "Humidity Results";
         this.printHumidityResultsMenuItem.Click += new System.EventHandler(this.printHumidityResultsMenuItem_Click);
         // 
         // menuItem30
         // 
         this.menuItem30.Index = 2;
         this.menuItem30.Text = "-";
         // 
         // menuItem8
         // 
         this.menuItem8.Index = 3;
         this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.overdriveMenuItem,
            this.menuItem10,
            this.menuItem29,
            this.menuItem9,
            this.menuItem19,
            this.menuItem24});
         this.menuItem8.Text = "1-Wire Adapter";
         // 
         // overdriveMenuItem
         // 
         this.overdriveMenuItem.Index = 0;
         this.overdriveMenuItem.Text = "Use Overdrive Speed";
         this.overdriveMenuItem.Click += new System.EventHandler(this.overdriveMenuItem_Click);
         // 
         // menuItem29
         // 
         this.menuItem29.Index = 2;
         this.menuItem29.Text = "-";
         // 
         // menuItem9
         // 
         this.menuItem9.Index = 3;
         this.menuItem9.Text = "DS9097U or DS9481";
         // 
         // menuItem19
         // 
         this.menuItem19.Index = 4;
         this.menuItem19.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem20,
            this.menuItem21,
            this.menuItem22,
            this.menuItem23});
         this.menuItem19.Text = "DS9490";
         // 
         // menuItem20
         // 
         this.menuItem20.Index = 0;
         this.menuItem20.Text = "USB1";
         this.menuItem20.Click += new System.EventHandler(this.DS9490_Click);
         // 
         // menuItem21
         // 
         this.menuItem21.Index = 1;
         this.menuItem21.Text = "USB2";
         this.menuItem21.Click += new System.EventHandler(this.DS9490_Click);
         // 
         // menuItem22
         // 
         this.menuItem22.Index = 2;
         this.menuItem22.Text = "USB3";
         this.menuItem22.Click += new System.EventHandler(this.DS9490_Click);
         // 
         // menuItem23
         // 
         this.menuItem23.Index = 3;
         this.menuItem23.Text = "USB4";
         this.menuItem23.Click += new System.EventHandler(this.DS9490_Click);
         // 
         // menuItem24
         // 
         this.menuItem24.Index = 5;
         this.menuItem24.Text = "DS9097E";
         // 
         // SeparatorMenuItem
         // 
         this.SeparatorMenuItem.Index = 4;
         this.SeparatorMenuItem.Text = "-";
         // 
         // CloseMenuItem
         // 
         this.CloseMenuItem.Index = 5;
         this.CloseMenuItem.Text = "Exit";
         this.CloseMenuItem.Click += new System.EventHandler(this.CloseMenuItem_Click);
         // 
         // menuItem2
         // 
         this.menuItem2.Index = 1;
         this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3,
            this.menuItem4});
         this.menuItem2.Text = "Calibration";
         // 
         // menuItem3
         // 
         this.menuItem3.Index = 0;
         this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.useHumidityCalibrationRegisters,
            this.tempCompensationMenuItem,
            this.useHysteresisCompensation});
         this.menuItem3.Text = "Humidity";
         // 
         // useHumidityCalibrationRegisters
         // 
         this.useHumidityCalibrationRegisters.Checked = true;
         this.useHumidityCalibrationRegisters.Index = 0;
         this.useHumidityCalibrationRegisters.Text = "Use Software Calibration";
         this.useHumidityCalibrationRegisters.Click += new System.EventHandler(this.useHumidityCalibrationRegisters_Click);
         // 
         // tempCompensationMenuItem
         // 
         this.tempCompensationMenuItem.Index = 1;
         this.tempCompensationMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.useTempCompensationMenuItem,
            this.tempCompensationOptionsMenuItem});
         this.tempCompensationMenuItem.Text = "Temperature Compensation";
         this.tempCompensationMenuItem.Click += new System.EventHandler(this.useTempCompensationMenuItem_Click);
         // 
         // useTempCompensationMenuItem
         // 
         this.useTempCompensationMenuItem.Index = 0;
         this.useTempCompensationMenuItem.Text = "Enabled";
         this.useTempCompensationMenuItem.Click += new System.EventHandler(this.useTempCompensationMenuItem_Click);
         // 
         // tempCompensationOptionsMenuItem
         // 
         this.tempCompensationOptionsMenuItem.Index = 1;
         this.tempCompensationOptionsMenuItem.Text = "Options";
         this.tempCompensationOptionsMenuItem.Click += new System.EventHandler(this.tempCompensationOptionsMenuItem_Click);
         // 
         // useHysteresisCompensation
         // 
         this.useHysteresisCompensation.Index = 2;
         this.useHysteresisCompensation.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.useHumdHysteresisMenuItem});
         this.useHysteresisCompensation.Text = "Saturation Drift Compensation";
         // 
         // useHumdHysteresisMenuItem
         // 
         this.useHumdHysteresisMenuItem.Index = 0;
         this.useHumdHysteresisMenuItem.Text = "Enabled";
         this.useHumdHysteresisMenuItem.Click += new System.EventHandler(this.useHumdHysteresisMenuItem_Click);
         // 
         // menuItem4
         // 
         this.menuItem4.Index = 1;
         this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.useTemperatureCalibrationRegisters});
         this.menuItem4.Text = "Temperature";
         // 
         // useTemperatureCalibrationRegisters
         // 
         this.useTemperatureCalibrationRegisters.Index = 0;
         this.useTemperatureCalibrationRegisters.Text = "Use Software Calibration";
         this.useTemperatureCalibrationRegisters.Click += new System.EventHandler(this.useTemperatureCalibrationRegisters_Click);
         // 
         // menuItem1
         // 
         this.menuItem1.Index = 2;
         this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.hideAllPasswordTextMenuItem,
            this.menuItem31,
            this.menuItem6});
         this.menuItem1.Text = "Password";
         // 
         // hideAllPasswordTextMenuItem
         // 
         this.hideAllPasswordTextMenuItem.Checked = true;
         this.hideAllPasswordTextMenuItem.Index = 0;
         this.hideAllPasswordTextMenuItem.Text = "Hide All Password Input Text";
         this.hideAllPasswordTextMenuItem.Click += new System.EventHandler(this.hideAllPasswordTextMenuItem_Click);
         // 
         // menuItem31
         // 
         this.menuItem31.Index = 1;
         this.menuItem31.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.enableGlobalPasswordsMenuItem,
            this.rememberGlobalPasswordMenuItem,
            this.menuItem33,
            this.setGlobalContainerReadWritePasswordMenuItem,
            this.setGlobalContainerReadOnlyPasswordMenuItem,
            this.menuItem36,
            this.menuItem5,
            this.menuItem35});
         this.menuItem31.Text = "All Devices";
         // 
         // enableGlobalPasswordsMenuItem
         // 
         this.enableGlobalPasswordsMenuItem.Checked = true;
         this.enableGlobalPasswordsMenuItem.Index = 0;
         this.enableGlobalPasswordsMenuItem.Text = "Enable Global Passwords";
         this.enableGlobalPasswordsMenuItem.Click += new System.EventHandler(this.enableGlobalPasswordsMenuItem_Click);
         // 
         // rememberGlobalPasswordMenuItem
         // 
         this.rememberGlobalPasswordMenuItem.Checked = true;
         this.rememberGlobalPasswordMenuItem.Index = 1;
         this.rememberGlobalPasswordMenuItem.Text = "Remember Global Passwords";
         this.rememberGlobalPasswordMenuItem.Click += new System.EventHandler(this.rememberGlobalPasswordMenuItem_Click);
         // 
         // menuItem33
         // 
         this.menuItem33.Index = 2;
         this.menuItem33.Text = "-";
         // 
         // setGlobalContainerReadWritePasswordMenuItem
         // 
         this.setGlobalContainerReadWritePasswordMenuItem.Index = 3;
         this.setGlobalContainerReadWritePasswordMenuItem.Text = "Set Global Software Read/Write Password";
         this.setGlobalContainerReadWritePasswordMenuItem.Click += new System.EventHandler(this.setGlobalContainerReadWritePasswordMenuItem_Click);
         // 
         // setGlobalContainerReadOnlyPasswordMenuItem
         // 
         this.setGlobalContainerReadOnlyPasswordMenuItem.Index = 4;
         this.setGlobalContainerReadOnlyPasswordMenuItem.Text = "Set Global Software Read-Only Password";
         this.setGlobalContainerReadOnlyPasswordMenuItem.Click += new System.EventHandler(this.setGlobalContainerReadOnlyPasswordMenuItem_Click);
         // 
         // menuItem36
         // 
         this.menuItem36.Index = 5;
         this.menuItem36.Text = "-";
         // 
         // menuItem5
         // 
         this.menuItem5.Index = 6;
         this.menuItem5.Text = "Set Read/Write Password on All Devices";
         // 
         // menuItem35
         // 
         this.menuItem35.Index = 7;
         this.menuItem35.Text = "Set Read-Only Password on All Devices";
         // 
         // menuItem6
         // 
         this.menuItem6.Index = 2;
         this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.setContainerReadWritePasswordMenuItem,
            this.setContainerReadOnlyPasswordMenuItem,
            this.menuItem7,
            this.setDeviceReadWritePasswordMenuItem,
            this.setDeviceReadOnlyPasswordMenuItem,
            this.menuItem32,
            this.enableDevicePasswordsMenuItem,
            this.disableDevicePasswordsMenuItem});
         this.menuItem6.Text = "Current Device";
         // 
         // setContainerReadWritePasswordMenuItem
         // 
         this.setContainerReadWritePasswordMenuItem.Index = 0;
         this.setContainerReadWritePasswordMenuItem.Text = "Set Software Read/Write Password";
         this.setContainerReadWritePasswordMenuItem.Click += new System.EventHandler(this.setContainerReadWritePasswordMenuItem_Click);
         // 
         // setContainerReadOnlyPasswordMenuItem
         // 
         this.setContainerReadOnlyPasswordMenuItem.Index = 1;
         this.setContainerReadOnlyPasswordMenuItem.Text = "Set Software Read-Only Password";
         this.setContainerReadOnlyPasswordMenuItem.Click += new System.EventHandler(this.setContainerReadOnlyPasswordMenuItem_Click);
         // 
         // menuItem7
         // 
         this.menuItem7.Index = 2;
         this.menuItem7.Text = "-";
         // 
         // setDeviceReadWritePasswordMenuItem
         // 
         this.setDeviceReadWritePasswordMenuItem.Index = 3;
         this.setDeviceReadWritePasswordMenuItem.Text = "Set Read/Write Password on Device";
         this.setDeviceReadWritePasswordMenuItem.Click += new System.EventHandler(this.setDeviceReadWritePasswordMenuItem_Click);
         // 
         // setDeviceReadOnlyPasswordMenuItem
         // 
         this.setDeviceReadOnlyPasswordMenuItem.Index = 4;
         this.setDeviceReadOnlyPasswordMenuItem.Text = "Set Read-Only Password on Device";
         this.setDeviceReadOnlyPasswordMenuItem.Click += new System.EventHandler(this.setDeviceReadOnlyPasswordMenuItem_Click);
         // 
         // menuItem32
         // 
         this.menuItem32.Index = 5;
         this.menuItem32.Text = "-";
         // 
         // enableDevicePasswordsMenuItem
         // 
         this.enableDevicePasswordsMenuItem.Index = 6;
         this.enableDevicePasswordsMenuItem.Text = "Enable Passwords on Device";
         this.enableDevicePasswordsMenuItem.Click += new System.EventHandler(this.enableDevicePasswordsMenuItem_Click);
         // 
         // disableDevicePasswordsMenuItem
         // 
         this.disableDevicePasswordsMenuItem.Index = 7;
         this.disableDevicePasswordsMenuItem.Text = "Disable Passwords on Device";
         // 
         // GangMenuItem
         // 
         this.GangMenuItem.Index = 3;
         this.GangMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.StopMissionMenuItem,
            this.StartMissionMenuItem,
            this.LoadMissionMenuItem});
         this.GangMenuItem.Text = "All Devices";
         // 
         // StopMissionMenuItem
         // 
         this.StopMissionMenuItem.Index = 0;
         this.StopMissionMenuItem.Text = "Stop Mission";
         this.StopMissionMenuItem.Click += new System.EventHandler(this.StopMissionMenuItem_Click);
         // 
         // StartMissionMenuItem
         // 
         this.StartMissionMenuItem.Index = 1;
         this.StartMissionMenuItem.Text = "Start Mission";
         this.StartMissionMenuItem.Click += new System.EventHandler(this.StartMissionMenuItem_Click);
         // 
         // LoadMissionMenuItem
         // 
         this.LoadMissionMenuItem.Index = 2;
         this.LoadMissionMenuItem.Text = "Load Mission";
         this.LoadMissionMenuItem.Click += new System.EventHandler(this.LoadMissionMenuItem_Click);
         // 
         // menuItem34
         // 
         this.menuItem34.Index = 4;
         this.menuItem34.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem37});
         this.menuItem34.Text = "Help";
         // 
         // menuItem37
         // 
         this.menuItem37.Index = 0;
         this.menuItem37.Text = "About";
         this.menuItem37.Click += new System.EventHandler(this.menuItem37_Click);
         // 
         // DeviceListbox
         // 
         this.DeviceListbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)));
         this.DeviceListbox.Font = new System.Drawing.Font("Courier New", 9F);
         this.DeviceListbox.ItemHeight = 15;
         this.DeviceListbox.Location = new System.Drawing.Point(0, 24);
         this.DeviceListbox.Name = "DeviceListbox";
         this.DeviceListbox.Size = new System.Drawing.Size(216, 484);
         this.DeviceListbox.TabIndex = 0;
         this.DeviceListbox.SelectedIndexChanged += new System.EventHandler(this.DeviceListbox_SelectedIndexChanged);
         // 
         // DeviceOptions
         // 
         this.DeviceOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.DeviceOptions.Controls.Add(this.DescriptionTab);
         this.DeviceOptions.Controls.Add(this.TemperatureTab);
         this.DeviceOptions.Controls.Add(this.HumidityTab);
         this.DeviceOptions.Location = new System.Drawing.Point(216, 48);
         this.DeviceOptions.Name = "DeviceOptions";
         this.DeviceOptions.SelectedIndex = 0;
         this.DeviceOptions.Size = new System.Drawing.Size(576, 496);
         this.DeviceOptions.TabIndex = 1;
         // 
         // DescriptionTab
         // 
         this.DescriptionTab.BackColor = System.Drawing.SystemColors.Control;
         this.DescriptionTab.Controls.Add(this.DescriptionTextBox);
         this.DescriptionTab.Location = new System.Drawing.Point(4, 22);
         this.DescriptionTab.Name = "DescriptionTab";
         this.DescriptionTab.Size = new System.Drawing.Size(568, 470);
         this.DescriptionTab.TabIndex = 0;
         this.DescriptionTab.Text = "Description";
         // 
         // DescriptionTextBox
         // 
         this.DescriptionTextBox.BackColor = System.Drawing.SystemColors.Window;
         this.DescriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.DescriptionTextBox.Location = new System.Drawing.Point(0, 0);
         this.DescriptionTextBox.Name = "DescriptionTextBox";
         this.DescriptionTextBox.Size = new System.Drawing.Size(568, 470);
         this.DescriptionTextBox.TabIndex = 1;
         this.DescriptionTextBox.Text = "";
         this.DescriptionTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DescriptionTextBox_KeyDown);
         this.DescriptionTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DescriptionTextBox_KeyPress);
         // 
         // TemperatureTab
         // 
         this.TemperatureTab.AutoScroll = true;
         this.TemperatureTab.Controls.Add(this.TemperaturePlot);
         this.TemperatureTab.Location = new System.Drawing.Point(4, 22);
         this.TemperatureTab.Name = "TemperatureTab";
         this.TemperatureTab.Size = new System.Drawing.Size(568, 470);
         this.TemperatureTab.TabIndex = 2;
         this.TemperatureTab.Text = "Temperature Results";
         // 
         // TemperaturePlot
         // 
         this.TemperaturePlot.AutoScaleAutoGeneratedAxes = true;
         this.TemperaturePlot.AutoScaleTitle = false;
         this.TemperaturePlot.BackColor = System.Drawing.SystemColors.Control;
         this.TemperaturePlot.DateTimeToolTip = false;
         this.TemperaturePlot.Dock = System.Windows.Forms.DockStyle.Fill;
         this.TemperaturePlot.Legend = null;
         this.TemperaturePlot.LegendZOrder = -1;
         this.TemperaturePlot.Location = new System.Drawing.Point(0, 0);
         this.TemperaturePlot.Name = "TemperaturePlot";
         this.TemperaturePlot.RightMenu = null;
         this.TemperaturePlot.ShowCoordinates = true;
         this.TemperaturePlot.Size = new System.Drawing.Size(568, 470);
         this.TemperaturePlot.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
         this.TemperaturePlot.TabIndex = 0;
         this.TemperaturePlot.Title = "";
         this.TemperaturePlot.TitleFont = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
         this.TemperaturePlot.XAxis1 = null;
         this.TemperaturePlot.XAxis2 = null;
         this.TemperaturePlot.YAxis1 = null;
         this.TemperaturePlot.YAxis2 = null;
         // 
         // HumidityTab
         // 
         this.HumidityTab.Controls.Add(this.HumidityPlot);
         this.HumidityTab.Location = new System.Drawing.Point(4, 22);
         this.HumidityTab.Name = "HumidityTab";
         this.HumidityTab.Size = new System.Drawing.Size(568, 470);
         this.HumidityTab.TabIndex = 3;
         this.HumidityTab.Text = "Humidity Results";
         // 
         // HumidityPlot
         // 
         this.HumidityPlot.AutoScaleAutoGeneratedAxes = true;
         this.HumidityPlot.AutoScaleTitle = true;
         this.HumidityPlot.BackColor = System.Drawing.SystemColors.Control;
         this.HumidityPlot.DateTimeToolTip = false;
         this.HumidityPlot.Dock = System.Windows.Forms.DockStyle.Fill;
         this.HumidityPlot.Legend = null;
         this.HumidityPlot.LegendZOrder = -1;
         this.HumidityPlot.Location = new System.Drawing.Point(0, 0);
         this.HumidityPlot.Name = "HumidityPlot";
         this.HumidityPlot.RightMenu = null;
         this.HumidityPlot.ShowCoordinates = true;
         this.HumidityPlot.Size = new System.Drawing.Size(568, 470);
         this.HumidityPlot.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
         this.HumidityPlot.TabIndex = 0;
         this.HumidityPlot.Title = "";
         this.HumidityPlot.TitleFont = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
         this.HumidityPlot.XAxis1 = null;
         this.HumidityPlot.XAxis2 = null;
         this.HumidityPlot.YAxis1 = null;
         this.HumidityPlot.YAxis2 = null;
         // 
         // startNewMissionButton
         // 
         this.startNewMissionButton.Location = new System.Drawing.Point(56, 16);
         this.startNewMissionButton.Name = "startNewMissionButton";
         this.startNewMissionButton.Size = new System.Drawing.Size(136, 24);
         this.startNewMissionButton.TabIndex = 2;
         this.startNewMissionButton.Text = "Start Mission";
         this.startNewMissionButton.Click += new System.EventHandler(this.startNewMissionButton_Click);
         // 
         // LoadResultsButton
         // 
         this.LoadResultsButton.Location = new System.Drawing.Point(376, 16);
         this.LoadResultsButton.Name = "LoadResultsButton";
         this.LoadResultsButton.Size = new System.Drawing.Size(136, 24);
         this.LoadResultsButton.TabIndex = 1;
         this.LoadResultsButton.Text = "Load Mission";
         this.LoadResultsButton.Click += new System.EventHandler(this.LoadResultsButton_Click);
         // 
         // timer1
         // 
         this.timer1.Interval = 5000;
         this.timer1.SynchronizingObject = this;
         this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
         // 
         // StatusBar
         // 
         this.StatusBar.Location = new System.Drawing.Point(0, 544);
         this.StatusBar.Name = "StatusBar";
         this.StatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.AdapterStatus,
            this.Status});
         this.StatusBar.ShowPanels = true;
         this.StatusBar.Size = new System.Drawing.Size(792, 22);
         this.StatusBar.TabIndex = 5;
         this.StatusBar.Text = "Status";
         // 
         // AdapterStatus
         // 
         this.AdapterStatus.Name = "AdapterStatus";
         this.AdapterStatus.Text = "Adapter Not Loaded";
         this.AdapterStatus.Width = 200;
         // 
         // Status
         // 
         this.Status.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
         this.Status.Name = "Status";
         this.Status.Text = "Status";
         this.Status.Width = 575;
         // 
         // groupBox1
         // 
         this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox1.Controls.Add(this.stopMissionButton);
         this.groupBox1.Controls.Add(this.LoadResultsButton);
         this.groupBox1.Controls.Add(this.startNewMissionButton);
         this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.groupBox1.Location = new System.Drawing.Point(216, 0);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(576, 48);
         this.groupBox1.TabIndex = 6;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Currently Selected Device";
         // 
         // stopMissionButton
         // 
         this.stopMissionButton.Location = new System.Drawing.Point(216, 16);
         this.stopMissionButton.Name = "stopMissionButton";
         this.stopMissionButton.Size = new System.Drawing.Size(136, 24);
         this.stopMissionButton.TabIndex = 3;
         this.stopMissionButton.Text = "Stop Mission";
         this.stopMissionButton.Click += new System.EventHandler(this.stopMissionButton_Click);
         // 
         // label1
         // 
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(0, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(200, 24);
         this.label1.TabIndex = 7;
         this.label1.Text = "Device Address List";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // searchOptions
         // 
         this.searchOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.searchOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.searchOptions.Items.AddRange(new object[] {
            "Search All Devices",
            "Search Alarming Devices",
            "Pause Searching"});
         this.searchOptions.Location = new System.Drawing.Point(24, 520);
         this.searchOptions.MaxDropDownItems = 3;
         this.searchOptions.Name = "searchOptions";
         this.searchOptions.Size = new System.Drawing.Size(168, 21);
         this.searchOptions.TabIndex = 8;
         // 
         // menuItem10
         // 
         this.menuItem10.Index = 1;
         this.menuItem10.Text = "Detect 1-Wire Adapters Connected";
         this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
         // 
         // backgroundWorkerAdapterDetection
         // 
         this.backgroundWorkerAdapterDetection.WorkerReportsProgress = true;
         this.backgroundWorkerAdapterDetection.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerAdapterDetection_DoWork);
         this.backgroundWorkerAdapterDetection.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerAdapterDetection_RunWorkerCompleted);
         this.backgroundWorkerAdapterDetection.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerAdapterDetection_ProgressChanged);
         // 
         // MainForm
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.BackColor = System.Drawing.SystemColors.Control;
         this.ClientSize = new System.Drawing.Size(792, 566);
         this.Controls.Add(this.searchOptions);
         this.Controls.Add(this.StatusBar);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.DeviceOptions);
         this.Controls.Add(this.DeviceListbox);
         this.Controls.Add(this.groupBox1);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Menu = this.MainMenu;
         this.MinimumSize = new System.Drawing.Size(800, 600);
         this.Name = "MainForm";
         this.Text = "HygroChron Viewer";
         this.Load += new System.EventHandler(this.MainForm_Load);
         this.DeviceOptions.ResumeLayout(false);
         this.DescriptionTab.ResumeLayout(false);
         this.TemperatureTab.ResumeLayout(false);
         this.HumidityTab.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.AdapterStatus)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Status)).EndInit();
         this.groupBox1.ResumeLayout(false);
         this.ResumeLayout(false);

      }
      #endregion

      #region multi-threaded access to DeviceListBox and Menus for adapter selections

      // Updates the DeviceListBox's selected index.
      private void UpdateDeviceListBoxSelectedIndex(int index)
      {
         // set the device list box's index.
         DeviceListbox.SelectedIndex = index;
      }

      #endregion

      #region Hysterisis and Calibration Checkbox updates
      private void useTemperatureCalibrationRegisters_Click(object sender, System.EventArgs e)
      {
         useTemperatureCalibrationRegisters.Checked = !useTemperatureCalibrationRegisters.Checked;

         if(owc41!=null)
         {
            updateContainerProperties(owc41);
            graphTemperature();
            graphHumidity();
         }
      }

      private void useHumidityCalibrationRegisters_Click(object sender, System.EventArgs e)
      {
         useHumidityCalibrationRegisters.Checked = !useHumidityCalibrationRegisters.Checked;
         if(owc41!=null)
         {
            updateContainerProperties(owc41);
            graphHumidity();
         }
      }

      private void useTempCompensationMenuItem_Click(object sender, System.EventArgs e)
      {
         useTempCompensationMenuItem.Checked = !useTempCompensationMenuItem.Checked;
         if(owc41!=null)
         {
            updateContainerProperties(owc41);
            graphHumidity();
         }
      }

      private void tempCompensationOptionsMenuItem_Click(object sender, System.EventArgs e)
      {
         if(tcOptions.ShowDialog()==DialogResult.OK)
         {
            if(owc41!=null)
            {
               updateContainerProperties(owc41);
               graphHumidity();
            }
         }
      }


      private void useHumdHysteresisMenuItem_Click(object sender, System.EventArgs e)
      {
         bool update = false;
         if(!useHumdHysteresisMenuItem.Checked)
         {
            if(useTempCompensationMenuItem.Checked)
            {
               update = true;
               useHumdHysteresisMenuItem.Checked = true;
            }
            else
            {
               MessageBox.Show(this, 
                  "Temperature Compensation for Humidity readings must be enabled " + Environment.NewLine + "before Saturation Drift Compensation.",
                  "Error");
            }
         }
         else
         {
            update = true;
            useHumdHysteresisMenuItem.Checked = false;
         }
         if(update && owc41!=null)
         {
            updateContainerProperties(owc41);
            graphHumidity();
         }
      }

      #endregion

      #region Refresh Device List
      volatile bool isRefreshing; 
      private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {
         isRefreshing = true;
         RefreshDeviceList();
         isRefreshing = false;
      }

      protected void RefreshDeviceList()
      {
         byte configByte = (byte)0;
         if(adapter!=null && searchOptions.Text.IndexOf("Pause")<0)
         {
            Hashtable isPresent = new Hashtable(DeviceHashtable.Count);
            try
            {
               adapter.beginExclusive(true);

               if(searchOptions.Text.IndexOf("Alarm")>=0)
                  adapter.setSearchOnlyAlarmingDevices();
               else
                  adapter.setSearchAllDevices();
               adapter.targetFamily(0x41);

               OneWireContainer owc = adapter.FirstDeviceContainer;
               while(owc!=null)
               {
                  /*
                  if (searchOptions.Text.IndexOf("Alarm") >= 0) && (owc.Alarming)
                  {
                     owc.Alarming
                  }
                   */
                  isPresent.Add(owc, null);
                  if(!DeviceHashtable.ContainsKey(owc))
                  {
                     MissionCache mc = new MissionCache();
                     try
                     {
                        OneWireContainer41 local_owc41 = (OneWireContainer41)owc;
                        configByte = local_owc41.DeviceConfigByte;
                        
                     }
                     catch(Exception){}
                     DeviceHashtable.Add(owc, mc);
                  }
                  owc = adapter.NextDeviceContainer;
               }
               
               // take care of alarming parts
               if (searchOptions.Text.IndexOf("Alarm") >= 0)
               {
                  // Now, go through each device found.  If they are not alarming, remove from list
                  OneWireContainer[] owc_no_alarm_list = new OneWireContainer[DeviceHashtable.Keys.Count];
                  DeviceHashtable.Keys.CopyTo(owc_no_alarm_list, 0);
                  for (int j = 0; j < owc_no_alarm_list.Length; j++)
                  {
                     if (!owc_no_alarm_list[j].Alarming)
                     {
                        DeviceHashtable.Remove(owc_no_alarm_list[j]);
                        isPresent.Remove(owc_no_alarm_list[j]);
                        DeviceListbox.Items.Remove(owc_no_alarm_list[j]);
                     }
                  }
               }   
            }
            catch(Exception e)
            {
               Status.Text = "Failure: " + e.Message;
            }
            finally
            {
               adapter.endExclusive();
            }
            
            if (DeviceHashtable.Keys.Count == 0) DeviceListbox.Items.Clear(); // take care of empty case
            OneWireContainer[] owcs = new OneWireContainer[DeviceHashtable.Keys.Count];
            DeviceHashtable.Keys.CopyTo(owcs, 0);
            for (int i = 0; i < owcs.Length; i++)
            {
               if (!isPresent.Contains(owcs[i]))
               {
                  DeviceHashtable.Remove(owcs[i]);
                  DeviceListbox.Items.Remove(owcs[i]);
               }
               else if (!DeviceListbox.Items.Contains(owcs[i]))
               {
                  DeviceListbox.Items.Add(owcs[i]);
               }
            }
         }
      }
      #endregion

      #region Printing Methods
      int lastChar = 0;
      object controlToPrint;

      // The PrintPage event is raised for each page to be printed.
      private void pd_PrintPage(object sender, PrintPageEventArgs ev) 
      {
         if(controlToPrint==TemperaturePlot)
         {
            // ok - the windows.forms MultiPlot control can also be 
            // rendered to other Graphics surfaces. Here we output to a
            // printer. There are a 3 versions of Render. The first 
            // takes a single argument - the Drawing.Graphics drawing
            // surface. ScPl determines where to place the plot based
            // on the Graphics.ClipBounds property. This isn't set in
            // the graphics context here (ev.Graphics). Instead, another
            // version of Render is used that takes a Rectangle that 
            // specifies the clip boundary. This is conveniently provided
            // by ev.MarginBounds. A third version of Render takes a 
            // region as the second argument and works out the clip
            // bounds from the bounding box of this. 
            TemperaturePlot.Draw(ev.Graphics, ev.MarginBounds);
            ev.HasMorePages = false;
         }
         else if(controlToPrint==HumidityPlot)
         {
            HumidityPlot.Draw(ev.Graphics, ev.MarginBounds);
            ev.HasMorePages = false;
         }
         else
         {
            // print the description box
            // draw the control by selecting each character and deterimining its
            // color 

            int xPos = 10;
            int yPos = 40;
            int kMargin = 50; 

            // start from the last position of the previous page
            for (int c = lastChar; c < DescriptionTextBox.Text.Length; c++)
            {
               //  Select a single character and retrieve the color and font
               DescriptionTextBox.Select(c,1);
               char nextChar = DescriptionTextBox.Text[c];
               Color theColor = DescriptionTextBox.SelectionColor;
               Font theFont = DescriptionTextBox.SelectionFont;

               //  Determine the character height from the font
               int height = theFont.Height;  

               //  if the next character is a return character, increment the Y
               //  Position
               if (nextChar == '\n')
               {
                  // add to height on return characters
                  yPos += (height + 3);  
                  xPos = 10; 

                  //  Get the height of the default print page
                  int paperHeight = print_document.PrinterSettings.
                     DefaultPageSettings.PaperSize.Height; 

                  //  Test to see if we went past the bottom margin of the page
                  if (yPos  > paperHeight - kMargin)
                  {
                     lastChar = c;
                     ev.HasMorePages = true;
                     return;
                  }
               }
                  //  if the next character is a space or tab, increment the horizontal
                  //  position by half the height
               else if ((nextChar == ' ') || (nextChar == '\t'))
               {
                  xPos += theFont.Height/2;
               }
               else
               {
                  Regex r = new Regex(@"\w",
                     RegexOptions.IgnoreCase|RegexOptions.Compiled);
                  Match m;
                  string nextWord = "";
                  bool reduceAtEnd = false;
                  m = r.Match(nextChar.ToString());

                  // Determine if next character is alpha numeric
                  if (m.Success)
                     reduceAtEnd = true;
                  else
                     nextWord = nextChar.ToString(); 

                  // use a regular expression matching alphanumerics
                  // until a whole word is formed
                  // by printing the whole word, rather than individual
                  // characters, this way the characters will be spaced
                  // better in the printout
                  while (m.Success)
                  {
                     nextWord += nextChar;
                     c++;
                     nextChar = DescriptionTextBox.Text[c];
                     m = r.Match(nextChar.ToString());
                  } 

                  if (reduceAtEnd)
                  {
                     c--;
                  } 

                  //  Draw the string at the current x position with the current font
                  //  and current selection color 

                  ev.Graphics.DrawString(nextWord, theFont, new SolidBrush(theColor),
                     xPos, yPos); 

                  //  Measure the length of the string to see where to advance the next
                  //  horizontal position 

                  SizeF thesize = ev.Graphics.MeasureString(nextWord, theFont); 

                  //  Increment the x position by the size of the word
                  xPos += (int)thesize.Width - 4;
               } 

            } 

            //  All characters in the RichTextBox have been visited, return -1
            lastChar = 0;
            ev.HasMorePages = false;

         }
      }

      private void printDescriptionMenuItem_Click(object sender, System.EventArgs e)
      {
         if(DescriptionTextBox.Text.Length>0)
         {
            controlToPrint = DescriptionTextBox;
            PrintDialog dlg = new PrintDialog();
            dlg.Document = print_document;
            dlg.AllowSelection = false;
            if (dlg.ShowDialog() == DialogResult.OK) 
            {
               print_document.Print();
            }	      
         }
         else
         {
            MessageBox.Show("No device description has been loaded");
         }
      }
      private void printTemperatureResultsMenuItem_Click(object sender, System.EventArgs e)
      {
         if(missionCache.temperatureValues!=null && missionCache.temperatureValues.Length>0)
         {
            controlToPrint = TemperaturePlot;
            PrintDialog dlg = new PrintDialog();
            dlg.Document = print_document;
            dlg.AllowSelection = false;
            if (dlg.ShowDialog() == DialogResult.OK) 
            {
               print_document.Print();
            }	      
         }
         else
         {
            MessageBox.Show("No temperature values have been loaded");
         }
      }
      private void printHumidityResultsMenuItem_Click(object sender, System.EventArgs e)
      {
         if(missionCache.humidityValues!=null && missionCache.humidityValues.Length>0)
         {
            controlToPrint = HumidityPlot;
            PrintDialog dlg = new PrintDialog();
            dlg.Document = print_document;
            dlg.AllowSelection = false;
            if (dlg.ShowDialog() == DialogResult.OK) 
            {
               print_document.Print();
            }
         }
         else
         {
            MessageBox.Show("No humidity values have been loaded");
         }
      }
      #endregion

      #region Graphing Methods
      private void graphTemperature()
      {
         TemperaturePlot.Clear();
         if(owc41.isMissionLoaded())
         {
            this.Cursor = Cursors.WaitCursor;

            missionCache.temperatureValues = null;
            if(owc41.getMissionChannelEnable(OneWireContainer41.TEMPERATURE_CHANNEL))
               missionCache.temperatureValues = 
                  new double[owc41.getMissionSampleCount(OneWireContainer41.TEMPERATURE_CHANNEL)];
            else
               missionCache.temperatureValues = new double[0];

            if(missionCache.temperatureValues.Length>1)
            {
               TemperaturePlot.Title = "Temperature Results";
               int sampleRate = owc41.getMissionSampleRate(OneWireContainer41.TEMPERATURE_CHANNEL);
               missionCache.time = new double[missionCache.temperatureValues.Length];
               float timeDiv;
               string timeUnits;
               if(sampleRate>=12*60*60) // 12 hours
               {
                  timeDiv = 24f*60f*60f;
                  timeUnits = "Days";
               }
               else if(sampleRate>=60*30) // 30 mins
               {
                  timeDiv = 60f*60f;
                  timeUnits = "Hours";
               }
               else if(sampleRate>=30) // 30 seconds
               {
                  timeDiv = 60f;
                  timeUnits = "Minutes";
               }
               else
               {
                  timeDiv = 1f;
                  timeUnits = "Seconds";
               }

               for( int i=0; i<missionCache.temperatureValues.Length; ++i )  
               {
                  missionCache.temperatureValues[i] = 
                     owc41.getMissionSample(OneWireContainer41.TEMPERATURE_CHANNEL, i);
                  missionCache.time[i] = sampleRate*i/timeDiv;
               }

               LinePlot pp = new LinePlot();
               pp.OrdinateData = missionCache.temperatureValues;
               pp.AbscissaData = missionCache.time;
               pp.Color = Color.Red; // set the color using standard Drawing.Color class.
               TemperaturePlot.Add(pp); // add the point plot to those to be plotted.
               TemperaturePlot.XAxis1.Label = "Time (" + timeUnits + ")";
               TemperaturePlot.YAxis1.Label = "Temperature (C)";
               TemperaturePlot.Refresh();
               if(DeviceOptions.TabPages.IndexOf(TemperatureTab)<0)
                  DeviceOptions.TabPages.Add(TemperatureTab);
            }
            else
            {
               if(DeviceOptions.TabPages.IndexOf(TemperatureTab)>=0)
                  DeviceOptions.TabPages.Remove(TemperatureTab);
               Status.Text += " No Temp Results...";
            }
            this.Cursor = Cursors.Default;
         }
      }

      private void graphHumidity()
      {
         HumidityPlot.Clear();
         if(owc41.isMissionLoaded())
         {
            this.Cursor = Cursors.WaitCursor;

            missionCache.humidityValues = null;
            if(owc41.getMissionChannelEnable(OneWireContainer41.DATA_CHANNEL))
               missionCache.humidityValues = 
                  new double[owc41.getMissionSampleCount(OneWireContainer41.DATA_CHANNEL)];
            else
               missionCache.humidityValues = new double[0];

            if(missionCache.humidityValues.Length>1)
            {
               string dataLbl = owc41.getMissionLabel(OneWireContainer41.DATA_CHANNEL);
               HumidityPlot.Title =  dataLbl + " Results";
               int sampleRate = owc41.getMissionSampleRate(OneWireContainer41.DATA_CHANNEL);
               missionCache.time = new double[missionCache.humidityValues.Length];
               float timeDiv;
               string timeUnits;
               if(sampleRate>12*60*60) // 12 hours
               {
                  timeDiv = 24f*60f*60f;
                  timeUnits = "Days";
               }
               else if(sampleRate>60*30) // 30 mins
               {
                  timeDiv = 60f*60f;
                  timeUnits = "Hours";
               }
               else if(sampleRate>30) // 30 seconds
               {
                  timeDiv = 60f;
                  timeUnits = "Minutes";
               }
               else
               {
                  timeDiv = 1f;
                  timeUnits = "Seconds";
               }

               for( int i=0; i<missionCache.humidityValues.Length; ++i )  
               {
                  missionCache.humidityValues[i] = 
                     owc41.getMissionSample(OneWireContainer41.DATA_CHANNEL, i);
                  missionCache.time[i] = sampleRate*i/timeDiv;
               }

               if(useHumdHysteresisMenuItem.Checked)
               {
                  findHysteresisAreas(owc41, missionCache.humidityValues);
               }

               LinePlot pp = new LinePlot();
               pp.OrdinateData = missionCache.humidityValues;
               pp.AbscissaData = missionCache.time;
               pp.Color = Color.Blue; // set the color using standard Drawing.Color class.
               HumidityPlot.Add(pp); // add the point plot to those to be plotted.
               HumidityPlot.XAxis1.Label = "Time (" + timeUnits + ")";
               HumidityPlot.YAxis1.Label = dataLbl;
               if(dataLbl.Equals("Humidity"))
                  HumidityPlot.YAxis1.Label += " (%RH)";
               HumidityPlot.Refresh();
               if(DeviceOptions.TabPages.IndexOf(HumidityTab)<0)
                  DeviceOptions.TabPages.Add(HumidityTab);
            }
            else
            {
               if(DeviceOptions.TabPages.IndexOf(HumidityTab)>=0)
                  DeviceOptions.TabPages.Remove(HumidityTab);
               Status.Text += " Temperature Results only. No Humidity/Data Results...";
            }         
            this.Cursor = Cursors.Default;
         }
      }

      public double getTemperature(OneWireContainer41 l_owc41, int index)
      {
         if(!l_owc41.getMissionChannelEnable(OneWireContainer41.TEMPERATURE_CHANNEL) 
            || tcOptions.overrideTemperatureLog.Checked)
         {
            return (double)tcOptions.defaultTemperatureValue.Value;
         }
         else
         {
            return l_owc41.getMissionSample(OneWireContainer41.TEMPERATURE_CHANNEL, index);
         }
      }
      
      public double getAverageHumidity(double[] humidityValues, int start, int cnt)
      {
         double sum = 0;
         for(int i=0; i<cnt; i++)
            sum += humidityValues[i+start];
         return sum / (double)cnt;
      }

      public double getAverageTemperature(OneWireContainer41 l_owc41, int start, int cnt)
      {
         double sum = 0;
         for(int i=0; i<cnt; i++)
            sum += getTemperature(l_owc41, i+start);
         return sum / (double)cnt;
      }


      public void findHysteresisAreas(OneWireContainer41 l_owc41, double[] humidityValues)
      {
         int start=-1, stop=-1;

         for(int i=0; i<humidityValues.Length; i++)
         {
            if(humidityValues[i]>=70.0 && start==-1)
            {
               start = i;
            }
            else if(humidityValues[i]<70.0 && start!=-1)
            {
               stop = i;
               if(stop>(start+1))
                  applyHysteresis(l_owc41, humidityValues, start, stop);
               start = -1;
               stop = -1;
            }
         }
      }

     /*public void applyHysteresis1(int start, int stop)
      {
         //sw.WriteLine(" * * * * * * * * * * * * ");
         //sw.WriteLine("applyHysteris1, start=" + start + ", stop=" + stop);
         int sampleRate = owc41.getMissionSampleRate(OneWireContainer41.DATA_CHANNEL);
         //sw.WriteLine("sampleRate=" + sampleRate);
         //sw.WriteLine("totaltime = (1+stop-start)*sampleRate=" + (stop-start)*sampleRate);
         int numSamples = (stop-start+1);
         //sw.WriteLine("numSamples=" + numSamples);
         for(int N=1; N<numSamples; N++)
         {
            //sw.WriteLine("N=" + N);
            double sum = 0;
            for(int k=0; k<N; k++)
            {
               //sw.WriteLine("k=" + k);
               double ARHk = 0, Tk = 0, cnt = 0;
               int currStop = k + start;
               //sw.WriteLine("currStop=" + currStop);
               for(int i=start; i<=(currStop) && i<=stop; i++)
               {
                  ARHk += missionCache.humidityValues[i];
                  Tk += getTemperature(i);
                  cnt++;
               }
               //sw.WriteLine("cnt=" + cnt);
               ARHk = (ARHk)/cnt;
               //sw.WriteLine("ARHk=" + ARHk);
               Tk = (Tk)/cnt;
               //sw.WriteLine("Tk=" + Tk);
               sum += (0.0156*ARHk*Math.Pow(2.54, -0.3502d*k))/(1d+(Tk-25)/100);
               //sw.WriteLine("sum=" + sum);
            }
            //sw.WriteLine("(before) missionCache.humidityValues[" + (N+start) + "]=" + missionCache.humidityValues[start+N]);
            missionCache.humidityValues[start+N] -= sum;
            //sw.WriteLine("(after)  missionCache.humidityValues[" + (N+start) + "]=" + missionCache.humidityValues[start+N]);
         }
         //sw.WriteLine(" - - - - - - - - - - - - ");
         //sw.Flush();
      }

      public void applyHysteresis2(int start, int stop)
      {
         //sw.WriteLine(" * * * * * * * * * * * * ");
         //sw.WriteLine("applyHysteris2, start=" + start + ", stop=" + stop);
         int sampleRate = owc41.getMissionSampleRate(OneWireContainer41.DATA_CHANNEL);
         //sw.WriteLine("sampleRate=" + sampleRate);
         //sw.WriteLine("(stop-start)*sampleRate=" + (stop-start)*sampleRate);
         int N = ((stop-start)*sampleRate)/3600; // num hours of exposure
         //sw.WriteLine("N=" + N);
         if(N < 2)
         {
            //sw.WriteLine("Not enough samples to apply saturation drift, aborting...");
            return;
         }
         if(sampleRate>1800)
            throw new Exception("Sample rate greater than 30 minutes, saturation drift cannot be measured");
         int numSamplesPerHour = 3600/sampleRate;
         //sw.WriteLine("numSamplesPerHour=" + numSamplesPerHour);
         int sum = 0;
         for(int k=0; k<N; k++)
         {
            //sw.WriteLine("k=" + k);
            double ARHk = 0, Tk = 0, cnt = 0;
            int currStart = k*numSamplesPerHour + start;
            //sw.WriteLine("currStart=" + currStart);
            for(int i=currStart; i<(currStart+numSamplesPerHour) && i<=stop; i++)
            {
               ARHk += missionCache.humidityValues[i];
               Tk += getTemperature(i);
               cnt++;
            }
            //sw.WriteLine("cnt=" + cnt);
            ARHk = (ARHk)/cnt;
            //sw.WriteLine("ARHk=" + ARHk);
            Tk = (Tk)/cnt;
            //sw.WriteLine("Tk=" + Tk);
            
         }
         //sw.WriteLine(" - - - - - - - - - - - - ");
         //sw.Flush();
      }*/

      public void applyHysteresis(OneWireContainer41 l_owc41, double[] humidityValues, int start, int stop)
      {
         int sampleRate = l_owc41.getMissionSampleRate(OneWireContainer41.DATA_CHANNEL);
         int N = ((stop-start)*sampleRate)/3600; // num hours of exposure
         if(((stop-start)*sampleRate)%3600!=0)
            N = N + 1;
         if(N < 2)
         {
            return;
         }
         if(sampleRate>1800)
            throw new Exception("Sample rate greater than 30 minutes, saturation drift cannot be measured");
         int numSamplesPerHour = 3600/sampleRate;
         double sum = 0;
         double[] ARHk = new double[N-1];
         double[] Tk = new double[N-1];
         for(int i=0; i<(N-1); i++)
         {
            ARHk[i] = getAverageHumidity(humidityValues, start + i*numSamplesPerHour, numSamplesPerHour);
            Tk[i] = getAverageTemperature(l_owc41, start + i*numSamplesPerHour, numSamplesPerHour);
         }
         for(int k=1; k<N; k++)
         {
            int currStart = (k)*numSamplesPerHour + start;
            int currStop = Math.Min(currStart + numSamplesPerHour, stop+1);
            double correction = (0.0156*ARHk[k-1]*Math.Pow(2.54, -0.3502d*k))/(1d+(Tk[k-1]-25)/100);
            sum += correction;
            for(int i=currStart; i<currStop; i++)
            {
               humidityValues[i] -= sum;
            }
         }
      }

      #endregion

      #region Saving File Methods
      private void saveDescriptionMenuItem_Click(object sender, System.EventArgs e)
      {
         if(DescriptionTextBox.Text.Length>0)
         {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format Files (*.rtf)|*.rtf";
            dlg.DefaultExt = ".rtf";
            dlg.AddExtension = true;
            dlg.CheckPathExists = true;
            if(dlg.ShowDialog() == DialogResult.OK)
            {
               this.Cursor = Cursors.WaitCursor;
               DescriptionTextBox.SaveFile(dlg.FileName);
               this.Cursor = Cursors.Default;
            }
         }
         else
         {
            MessageBox.Show("No device description has been loaded");
         }
      }
      private void saveTemperatureResultsMenuItem_Click(object sender, System.EventArgs e)
      {
         if(missionCache.temperatureValues!=null && missionCache.temperatureValues.Length>0)
         {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Comma-separated Values Files (*.csv)|*.csv";
            dlg.DefaultExt = ".csv";
            dlg.AddExtension = true;
            dlg.CheckPathExists = true;
            System.DateTime missionSampleTimeStamp;
            System.DateTime missionTStamp;
            long missionSampleTimeStamp_long;

            if(dlg.ShowDialog() == DialogResult.OK)
            {
               StreamWriter sw = new StreamWriter(dlg.FileName, false);
               this.Cursor = Cursors.WaitCursor;
               sw.Write(owc41.AddressAsString);
               sw.Write(",");
               missionTStamp = new System.DateTime(owc41.getMissionTimeStamp(
                  OneWireContainer41.TEMPERATURE_CHANNEL));
               //sw.WriteLine(new System.DateTime(owc41.getMissionTimeStamp(
               //   OneWireContainer41.TEMPERATURE_CHANNEL)));
               sw.WriteLine(missionTStamp);

               for(int i=0; i<missionCache.temperatureValues.Length; i++)
               {
                  sw.Write(missionCache.temperatureValues[i]);
                  sw.Write(",");
                  missionSampleTimeStamp_long = owc41.getMissionSampleTimeStamp(OneWireContainer41.TEMPERATURE_CHANNEL, i);
                  missionSampleTimeStamp = new System.DateTime(missionSampleTimeStamp_long);
                  sw.WriteLine(missionSampleTimeStamp);
               }
               this.Cursor = Cursors.Default;
               sw.Flush();
               sw.Close();
            }
         }
         else
         {
            MessageBox.Show("No temperature values have been loaded");
         }
      }

      private void saveHumidityResultsMenuItem_Click(object sender, System.EventArgs e)
      {
         if(missionCache.humidityValues!=null && missionCache.humidityValues.Length>0)
         {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Comma-separated Values Files (*.csv)|*.csv";
            dlg.DefaultExt = ".csv";
            dlg.AddExtension = true;
            dlg.CheckPathExists = true;
            if(dlg.ShowDialog() == DialogResult.OK)
            {
               StreamWriter sw = new StreamWriter(dlg.FileName, false);
               this.Cursor = Cursors.WaitCursor;
               sw.Write(owc41.AddressAsString);
               sw.Write(",");
               sw.WriteLine(new System.DateTime(owc41.getMissionTimeStamp(
                  OneWireContainer41.DATA_CHANNEL)));
               for(int i=0; i<missionCache.humidityValues.Length; i++)
               {
                  sw.Write(missionCache.humidityValues[i]);
                  sw.Write(",");
                  sw.WriteLine(new System.DateTime(owc41.getMissionSampleTimeStamp(
                     OneWireContainer41.DATA_CHANNEL, i)));
               }
               this.Cursor = Cursors.Default;
               sw.Flush();
               sw.Close();
            }
         }
         else
         {
            MessageBox.Show("No humidity values have been loaded");
         }
      }

      private void saveAllTemperatureResultsMenuItem_Click(object sender, System.EventArgs e)
      {
         SaveFileDialog dlg = new SaveFileDialog();
         dlg.Filter = "Comma-separated Values Files (*.csv)|*.csv";
         dlg.DefaultExt = ".csv";
         dlg.AddExtension = true;
         dlg.CheckPathExists = true;
         if(dlg.ShowDialog() == DialogResult.OK)
         {
            Status.Text = "Saving all temperature results... ";
            this.Cursor = Cursors.WaitCursor;
            try
            {
               foreach(OneWireContainer41 l_owc41 in DeviceHashtable.Keys)
               {
                  if(!l_owc41.isMissionLoaded())
                     l_owc41.loadMissionResults();
                  updateContainerProperties(l_owc41);
               }
            }
            catch(Exception ex)
            {
               MessageBox.Show("Failed to load mission results from button: "
                  + owc41.AddressAsString + Environment.NewLine + ex.Message);
               return;
            }

            StreamWriter sw = new StreamWriter(dlg.FileName, false);

            int maxCount = 0;
            bool firstElement = true;
            foreach(OneWireContainer41 l_owc41 in DeviceHashtable.Keys)
            {
               if(l_owc41.getMissionChannelEnable(OneWireContainer41.TEMPERATURE_CHANNEL))
               {
                  maxCount = Math.Max(maxCount, 
                     l_owc41.getMissionSampleCount(OneWireContainer41.TEMPERATURE_CHANNEL));

                  if(!firstElement)
                     sw.Write(",");

                  sw.Write(l_owc41.AddressAsString);
                  firstElement = false;
               }
            }

            for(int i=0; i<maxCount; i++)
            {
               firstElement = true;
               foreach(OneWireContainer41 l_owc41 in DeviceHashtable.Keys)
               {
                  if(l_owc41.getMissionChannelEnable(OneWireContainer41.TEMPERATURE_CHANNEL))
                  {
                     if(!firstElement)
                        sw.Write(",");
                     if(i<l_owc41.getMissionSampleCount(OneWireContainer41.TEMPERATURE_CHANNEL))
                        sw.Write(l_owc41.getMissionSample(OneWireContainer41.TEMPERATURE_CHANNEL, i));
                     firstElement = false;
                  }
               }
               sw.WriteLine();
            }
            sw.Flush();
            sw.Close();

            this.Cursor = Cursors.Default;
            Status.Text += "Done.";
         }
      }

      private void saveAllHumidityResultsMenuItem_Click(object sender, System.EventArgs e)
      {
         SaveFileDialog dlg = new SaveFileDialog();
         dlg.Filter = "Comma-separated Values Files (*.csv)|*.csv";
         dlg.DefaultExt = ".csv";
         dlg.AddExtension = true;
         dlg.CheckPathExists = true;
         if(dlg.ShowDialog() == DialogResult.OK)
         {
            Status.Text = "Saving all Humidity results... ";

            this.Cursor = Cursors.WaitCursor;
            StreamWriter sw = new StreamWriter(dlg.FileName, false);

            int maxCount = 0, index = 0;
            bool firstElement = true;
            double[][] humidityValues = 
               new double[DeviceHashtable.Keys.Count][];
            foreach(OneWireContainer41 l_owc41 in DeviceHashtable.Keys)
            {
               try
               {
                  if(!l_owc41.isMissionLoaded())
                     l_owc41.loadMissionResults();
                  updateContainerProperties(l_owc41);
               }
               catch(Exception ex)
               {
                  this.Cursor = Cursors.Default;
                  MessageBox.Show("Failed to load mission results from button: "
                     + owc41.AddressAsString + Environment.NewLine + ex.Message);
                  return;
               }

               if(l_owc41.getMissionChannelEnable(OneWireContainer41.DATA_CHANNEL))
               {
                  maxCount = Math.Max(maxCount, 
                     l_owc41.getMissionSampleCount(OneWireContainer41.DATA_CHANNEL));

                  if(!firstElement)
                     sw.Write(",");
                  sw.Write(l_owc41.AddressAsString);
                  firstElement = false;

                  humidityValues[index] = new double[l_owc41.getMissionSampleCount(OneWireContainer41.DATA_CHANNEL)];
                  for(int k=0; k<humidityValues[index].Length; ++k )  
                  {
                     humidityValues[index][k] = 
                        l_owc41.getMissionSample(OneWireContainer41.DATA_CHANNEL, k);
                  }
                  if(useHumdHysteresisMenuItem.Checked)
                  {
                     findHysteresisAreas(l_owc41, humidityValues[index]);
                  }
               }
               else
               {
                  humidityValues[index] = new double[0];
               }
               index++;
            }
            sw.WriteLine();


            for(int i=0; i<maxCount; i++)
            {
               firstElement = true;
               for(int k=0; k<index; k++)
               {
                  if(humidityValues[k].Length>0)
                  {
                     if(!firstElement)
                        sw.Write(",");
                     if(i<humidityValues[k].Length)
                     {
                        sw.Write(humidityValues[k][i]);
                     }
                     firstElement = false;
                  }
               }
               sw.WriteLine();
            }

            sw.Flush();
            sw.Close();

            this.Cursor = Cursors.Default;
            Status.Text += "Done.";
         }
      }
      #endregion

      #region Password Methods
      private void hideAllPasswordTextMenuItem_Click(object sender, System.EventArgs e)
      {
         hideAllPasswordTextMenuItem.Checked = !hideAllPasswordTextMenuItem.Checked;
      }

      private void setDeviceReadWritePasswordMenuItem_Click(object sender, System.EventArgs e)
      {
         if(owc41!=null)
         {
            DevicePasswordForm dpf = new DevicePasswordForm("Device Read/Write",
               "Please enter the 8-byte Read/Write password that will be written"
               + " to the device's Read/Write password register.",
               hideAllPasswordTextMenuItem.Checked);

            if(enableGlobalPasswordsMenuItem.Checked && globalReadWritePassword!=null)
               dpf.password = globalReadWritePassword;
            else if(owc41.ContainerReadWritePasswordSet)
            {
               byte[] pass = new byte[8];
               owc41.getContainerReadWritePassword(pass, 0);
               dpf.password = pass;
            }

            if(dpf.ShowDialog() == DialogResult.OK)
            {
               try
               {
                  adapter.beginExclusive(true);
                  if(enableGlobalPasswordsMenuItem.Checked)
                  {
                     if(globalReadWritePassword!=null)
                        owc41.setContainerReadWritePassword(globalReadWritePassword, 0);
                     if(globalReadOnlyPassword!=null)
                        owc41.setContainerReadOnlyPassword(globalReadOnlyPassword, 0);
                  }
                  if(overdriveMenuItem.Checked)
                     owc41.setSpeed(DSPortAdapter.SPEED_OVERDRIVE, true);
                  else
                     owc41.setSpeed(DSPortAdapter.SPEED_REGULAR, false);

                  byte[] pass = dpf.password;
                  owc41.setDeviceReadWritePassword(pass, 0);

                  for(int i =0; enableGlobalPasswordsMenuItem.Checked && i<8; i++)
                     enableGlobalPasswordsMenuItem.Checked
                        = (pass[i] == globalReadWritePassword[i]);
               }
               catch(Exception ex)
               {
                  MessageBox.Show(ex.Message);
               }
               finally
               {
                  adapter.endExclusive();
               }
            }
         }
         else
         {
            MessageBox.Show("You must select a device from the device list first");
         }
      }

      private void setContainerReadWritePasswordMenuItem_Click(object sender, System.EventArgs e)
      {
         if(owc41!=null)
         {
            DevicePasswordForm dpf = new DevicePasswordForm("Container Read/Write",
               "Please enter the 8-byte Read-Write password that will be used by"
               + " the software when writing to the device's memory.",
               hideAllPasswordTextMenuItem.Checked);

            if(enableGlobalPasswordsMenuItem.Checked && globalReadWritePassword!=null)
               dpf.password = globalReadWritePassword;
            else if(owc41.ContainerReadWritePasswordSet)
            {
               byte[] pass = new byte[8];
               owc41.getContainerReadWritePassword(pass, 0);
               dpf.password = pass;
            }

            if(dpf.ShowDialog() == DialogResult.OK)
            {
               byte[] pass = dpf.password;
               owc41.setContainerReadWritePassword(pass, 0);

               for(int i =0; enableGlobalPasswordsMenuItem.Checked && i<8; i++)
                  enableGlobalPasswordsMenuItem.Checked
                     = (pass[i] == globalReadWritePassword[i]);
            }
         }
         else
         {
            MessageBox.Show("You must select a device from the device list first");
         }
      }

      private void setDeviceReadOnlyPasswordMenuItem_Click(object sender, System.EventArgs e)
      {
         if(owc41!=null)
         {
            DevicePasswordForm dpf = new DevicePasswordForm("Device Read-Only",
               "Please enter the 8-byte Read-Only password that will be written"
               + " to the device's Read-Only password register.",
               hideAllPasswordTextMenuItem.Checked);

            if(enableGlobalPasswordsMenuItem.Checked && globalReadOnlyPassword!=null)
               dpf.password = globalReadOnlyPassword;
            else if(owc41.ContainerReadOnlyPasswordSet)
            {
               byte[] pass = new byte[8];
               owc41.getContainerReadOnlyPassword(pass, 0);
               dpf.password = pass;
            }

            if(dpf.ShowDialog() == DialogResult.OK)
            {
               try
               {
                  adapter.beginExclusive(true);
                  if(enableGlobalPasswordsMenuItem.Checked)
                  {
                     if(globalReadWritePassword!=null)
                        owc41.setContainerReadWritePassword(globalReadWritePassword, 0);
                     if(globalReadOnlyPassword!=null)
                        owc41.setContainerReadOnlyPassword(globalReadOnlyPassword, 0);
                  }
                  if(overdriveMenuItem.Checked)
                     owc41.setSpeed(DSPortAdapter.SPEED_OVERDRIVE, true);
                  else
                     owc41.setSpeed(DSPortAdapter.SPEED_REGULAR, false);

                  byte[] pass = dpf.password;
                  owc41.setDeviceReadOnlyPassword(pass, 0);

                  for(int i =0; enableGlobalPasswordsMenuItem.Checked && i<8; i++)
                     enableGlobalPasswordsMenuItem.Checked
                        = (pass[i] == globalReadOnlyPassword[i]);
               }
               catch(Exception ex)
               {
                  MessageBox.Show(ex.Message);
               }
               finally
               {
                  adapter.endExclusive();
               }
            }
         }
         else
         {
            MessageBox.Show("You must select a device from the device list first");
         }
      }

      private void setContainerReadOnlyPasswordMenuItem_Click(object sender, System.EventArgs e)
      {
         if(owc41!=null)
         {
            DevicePasswordForm dpf = new DevicePasswordForm("Container Read-Only",
               "Please enter the 8-byte Read-Only password that will be used by"
               + " the software when reading from the device's memory.",
               hideAllPasswordTextMenuItem.Checked);

            if(enableGlobalPasswordsMenuItem.Checked && globalReadOnlyPassword!=null)
               dpf.password = globalReadOnlyPassword;
            else if(owc41.ContainerReadOnlyPasswordSet)
            {
               byte[] pass = new byte[8];
               owc41.getContainerReadOnlyPassword(pass, 0);
               dpf.password = pass;
            }

            if(dpf.ShowDialog() == DialogResult.OK)
            {
               byte[] pass = dpf.password;
               owc41.setContainerReadOnlyPassword(pass, 0);

               for(int i =0; enableGlobalPasswordsMenuItem.Checked && i<8; i++)
                  enableGlobalPasswordsMenuItem.Checked
                     = (pass[i] == globalReadOnlyPassword[i]);
            }
         }
         else
         {
            MessageBox.Show("You must select a device from the device list first");
         }
      }

      private void enableDevicePasswordsMenuItem_Click(object sender, System.EventArgs e)
      {
         if(owc41!=null)
         {
            if(owc41.ContainerReadWritePasswordSet && owc41.ContainerReadOnlyPasswordSet)
            {
               try
               {
                  adapter.beginExclusive(true);
                  if(enableGlobalPasswordsMenuItem.Checked)
                  {
                     if(globalReadWritePassword!=null)
                        owc41.setContainerReadWritePassword(globalReadWritePassword, 0);
                     if(globalReadOnlyPassword!=null)
                        owc41.setContainerReadOnlyPassword(globalReadOnlyPassword, 0);
                  }
                  if(overdriveMenuItem.Checked)
                     owc41.setSpeed(DSPortAdapter.SPEED_OVERDRIVE, true);
                  else
                     owc41.setSpeed(DSPortAdapter.SPEED_REGULAR, false);
                  owc41.DevicePasswordEnableAll = true;
               }
               catch(Exception ex)
               {
                  MessageBox.Show(ex.Message);
               }
               finally
               {
                  adapter.endExclusive();
               }
            }
            else
            {
               MessageBox.Show("Must set Container Read-Only and Read/Write Passwords first");
            }
         }
         else
         {
            MessageBox.Show("You must select a device from the device list first");
         }
      }

      private void setGlobalContainerReadWritePasswordMenuItem_Click(object sender, System.EventArgs e)
      {
         DevicePasswordForm dpf = new DevicePasswordForm("Container Read/Write",
            "Please enter the 8-byte Read/Write password that will be used by"
            + " the software when writing to the memory of all devices.",
            hideAllPasswordTextMenuItem.Checked);

         if(globalReadWritePassword!=null)
            dpf.password = globalReadWritePassword;

         if(dpf.ShowDialog() == DialogResult.OK)
            this.globalReadWritePassword = dpf.password;
      }

      private void setGlobalContainerReadOnlyPasswordMenuItem_Click(object sender, System.EventArgs e)
      {
         DevicePasswordForm dpf = new DevicePasswordForm("Container Read-Only",
            "Please enter the 8-byte Read-Only password that will be used by"
            + " the software when reading from the memory of all devices.",
            hideAllPasswordTextMenuItem.Checked);

         if(globalReadOnlyPassword!=null)
            dpf.password = globalReadOnlyPassword;

         if(dpf.ShowDialog() == DialogResult.OK)
            this.globalReadOnlyPassword = dpf.password;
      }

      private void enableGlobalPasswordsMenuItem_Click(object sender, System.EventArgs e)
      {
         enableGlobalPasswordsMenuItem.Checked = !enableGlobalPasswordsMenuItem.Checked;
      }

      private void rememberGlobalPasswordMenuItem_Click(object sender, System.EventArgs e)
      {
         rememberGlobalPasswordMenuItem.Checked = !rememberGlobalPasswordMenuItem.Checked;
      }
      #endregion

      #region 1-Wire Adapter
      public void DS9097U_Click(object sender, System.EventArgs e)
      {
         setAdapter("{DS9097U_DS948X}", ((MenuItem)sender).Text);
      }
      private void DS1410E_Click(object sender, System.EventArgs e)
      {
         setAdapter("{DS1410E}", ((MenuItem)sender).Text);
      }
      private void DS9490_Click(object sender, System.EventArgs e)
      {
         setAdapter("{DS9490}", ((MenuItem)sender).Text);
      }
      private void DS9097E_Click(object sender, System.EventArgs e)
      {
         setAdapter("{DS9097E}", ((MenuItem)sender).Text);
      }

      private void setAdapter(string adapterName, string portName)
      {
         try
         {
            if(adapter!=null)
            {
               this.timer1.Stop();
               bool stopped = false;
               while(!stopped)
               {
                  adapter.beginExclusive(true);
                  stopped = !isRefreshing;
                  adapter.endExclusive();
               }
               adapter.freePort();
               adapter = null;
            }
            adapter = OneWireAccessProvider.getAdapter(adapterName, portName);
            AdapterStatus.Text = adapter.ToString();

            this.timer1.Start();
            adapterIsSet = true;
            Status.Text = "";
            previousDeviceSelection = "";

            DescriptionTextBox.Clear();
            if (DeviceOptions.TabPages.IndexOf(TemperatureTab) >= 0)
               DeviceOptions.TabPages.Remove(TemperatureTab);
            if (DeviceOptions.TabPages.IndexOf(HumidityTab) >= 0)
               DeviceOptions.TabPages.Remove(HumidityTab);
            // clear devicelist hash table
            DeviceHashtable.Clear();
            // clear device list textbox
            DeviceListbox.Items.Clear();
            // refresh the device list
            RefreshDeviceList();
         }

         catch(Exception ex)
         {
            //AdapterStatus.Text = "Not Loaded";
            DeviceListbox.Items.Clear();

            if (adapter != null)
            {
               adapter.freePort();
               adapter = null;
            }

            DescriptionTextBox.Clear();
            if (DeviceOptions.TabPages.IndexOf(TemperatureTab) >= 0)
               DeviceOptions.TabPages.Remove(TemperatureTab);
            if (DeviceOptions.TabPages.IndexOf(HumidityTab) >= 0)
               DeviceOptions.TabPages.Remove(HumidityTab);

            MessageBox.Show("Couldn't load " + adapterName + " " + portName + ". Reason: " + ex.Message + "." + Environment.NewLine
               + Environment.NewLine + "Please select another adapter and port from the 'File->1-Wire Adapter' menu.");
            Status.Text = "Couldn't load " + adapterName + " " + portName + " " + ". Reason: " + ex.Message + ". Select another adapter.";
            // clear devicelist hash table
            DeviceHashtable.Clear();
         }		
      }

      private void overdriveMenuItem_Click(object sender, System.EventArgs e)
      {
         overdriveMenuItem.Checked = !overdriveMenuItem.Checked;
      }
      #endregion

      private void menuItem37_Click(object sender, System.EventArgs e)
      {
         // get the version string -- need to use namespace System.Reflection
         String version = Assembly.GetExecutingAssembly().GetName().Version.ToString();     
         
         MessageBox.Show(this, 
            "HygroChron Viewer " + version + Environment.NewLine + Environment.NewLine,
            "HygroChron Viewer");
      }

      private ArrayList getComPortList()
      {
         ManagementObjectCollection ManObjList;
         ManagementObjectSearcher ManObjSearch;
         System.Collections.ArrayList comports = new System.Collections.ArrayList();
         int i = 0;

         try
         {
            // get all the "friendly" names of Plug and Play devices connected to computer
            ManObjSearch = new ManagementObjectSearcher("Select * from Win32_PnPEntity");
            ManObjList = ManObjSearch.Get();
            // get the com port strings
            foreach (ManagementObject ManObj in ManObjList)
            {
               if (ManObj["Name"].ToString().Contains("(COM"))
               {
                  comports.Add(ManObj["Name"].ToString());
               }
            }
            // massage the strings to actual com ports such as COM1, COM2, etc.
            comports.Sort();
            for (i = 0; i < comports.Count; i++)
            {
               int indexOfCOMString = 0;
               String stringTemp = "";
               indexOfCOMString = comports[i].ToString().IndexOf("(COM");
               indexOfCOMString++;
               stringTemp = comports[i].ToString().Substring(indexOfCOMString);
               stringTemp = stringTemp.Trim(')');
               comports[i] = stringTemp;
            }
         }
         catch (Exception exc)
         {
            if (exc.Equals(null)) { } // drain exception
         }
         // now we have a list of COM ports
         return comports;
      }

      private void refreshAdapterSelections(bool performAdapterDetection)
      {
         ManagementObjectCollection ManObjList;
         ManagementObjectSearcher ManObjSearch;
         string portName = "";
         string adapterName = "";
         System.Collections.ArrayList comports = new System.Collections.ArrayList();
         
         int i = 0;

         if (adapter != null)
         {
            // save off original adapter
            portName = adapter.PortName;
            adapterName = adapter.AdapterName;
         }
         
         deleteAdapterSelections(); // delete all adapter selections
         // set the device list box's index to -1
         DeviceListbox.SelectedIndex = -1;
         try
         {
            // get all the "friendly" names of Plug and Play devices connected to computer
            ManObjSearch = new ManagementObjectSearcher("Select * from Win32_PnPEntity");
           

            ManObjList = ManObjSearch.Get();
            // get the com port strings
            foreach (ManagementObject ManObj in ManObjList)
            {
               if (ManObj["Name"].ToString().Contains("(COM"))
               {
                  comports.Add(ManObj["Name"].ToString());
               }
            }
            // massage the strings to actual com ports such as COM1, COM2, etc.
            comports.Sort();
            for (i = 0; i < comports.Count; i++)
            {
               int indexOfCOMString = 0;
               String stringTemp = "";
               indexOfCOMString = comports[i].ToString().IndexOf("(COM");
               indexOfCOMString++;
               stringTemp = comports[i].ToString().Substring(indexOfCOMString);
               stringTemp = stringTemp.Trim(')');
               comports[i] = stringTemp;
            }
         }
         catch (Exception exc)
         {
            if (exc.Equals(null)) { } // drain exception
         }
         // now we have a list of COM ports

         // if performing an exhaustive search with adapter detection, cycle through 
         // each possible adapter and if present, then add it to the menu
         if (performAdapterDetection)
         {
            // now that we have a com port list, let's figure out if an adapter is actually present
            for (i = 0; i < comports.Count; i++) // for DS9097U or DS9481
            {
               try
               {
                  adapter = OneWireAccessProvider.getAdapter("{DS9097U_DS948X}", comports[i].ToString());
               }
               catch (Exception except)
               {
                  if (except.Equals(null)) { } // drain exception
                  continue;
               }
               menuItem9.MenuItems.Add(comports[i].ToString(), DS9097U_Click);
               adapter.freePort(); // necessary
               adapter = null; // necessary
            }

            for (i = 0; i < comports.Count; i++) // for DS9097E
            {
               try
               {
                  adapter = OneWireAccessProvider.getAdapter("{DS9097E}", comports[i].ToString());
               }
               catch (Exception except)
               {
                  if (except.Equals(null)) { } // drain exception
                  continue;
               }
               menuItem9.MenuItems.Add(comports[i].ToString(), DS9097U_Click);
               adapter.freePort(); // necessary
               adapter = null; // necessary
            }

            for (i = 0; i < 16; i++) // for DS9490
            {
               try
               {
                  adapter = OneWireAccessProvider.getAdapter("{DS9490}", "USB" + i.ToString());
               }
               catch (Exception except)
               {
                  if (except.Equals(null)) { } // drain exception
                  continue;
               }
               menuItem19.MenuItems.Add(("USB" + i.ToString()), DS9490_Click);
               adapter.freePort();
               adapter = null;
            }
            // get original adapter back for the timer 
            if (!(adapterName.Equals("") || portName.Equals("")))
            {
               setAdapter(adapterName, portName);
            }
            else
            {
               // make sure adapter is blank
               if (adapter != null)
               {
                  adapter.freePort();
                  adapter = null;
               }
            }
         }
         else
         {
            // add menu items
            for (i = 0; i < comports.Count; i++)
            {
               menuItem24.MenuItems.Add(comports[i].ToString(), DS9097E_Click);
               menuItem9.MenuItems.Add(comports[i].ToString(), DS9097U_Click);
            }
            for (i = 0; i < 16; i++)
            {
               menuItem19.MenuItems.Add(("USB" + i.ToString()), DS9490_Click);
            }
         }
      }

      private void MainForm_Load(object sender, System.EventArgs e)
      {
         progressForm.Hide();
         refreshAdapterSelections(false); // for the sake of GUI responsiveness, do not perform adapter detection on form_load
         string adapterName = null, adapterPort = null;
         key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Maxim Integrated Products\\HygrochronViewer", true);
         if (key != null)
         {
            adapterName = (string)key.GetValue("adapterName", null);
            adapterPort = (string)key.GetValue("adapterPort", null);

            overdriveMenuItem.Checked
               = bool.Parse(key.GetValue("overdrive", "true").ToString());

            useTemperatureCalibrationRegisters.Checked
               = bool.Parse(key.GetValue("Temperature Calibration: Enabled", "true").ToString());
            useHumidityCalibrationRegisters.Checked
               = bool.Parse(key.GetValue("Humidity Calibration: Enabled", "true").ToString());
            useTempCompensationMenuItem.Checked
               = bool.Parse(key.GetValue("Temperature Compensation: Enabled", "false").ToString());
            tcOptions.defaultTemperatureValue.Value
               = decimal.Parse(key.GetValue("Temperature Compensation: Default", "25").ToString());
            tcOptions.overrideTemperatureLog.Checked
               = bool.Parse(key.GetValue("Temperature Compensation: Ignore Log", "false").ToString());

            string gROpass = (string)key.GetValue("Global Read-Only Password", null);
            string gRWpass = (string)key.GetValue("Global Read-Write Password", null);
            if (gROpass != null && gRWpass != null)
            {
               this.rememberGlobalPasswordMenuItem.Checked = true;
               globalReadOnlyPassword = com.dalsemi.onewire.utils.Convert.toByteArray(gROpass);
               globalReadWritePassword = com.dalsemi.onewire.utils.Convert.toByteArray(gRWpass);
            }
            else
               this.rememberGlobalPasswordMenuItem.Checked = false;
         }

         if (adapterName != null && adapterPort != null)
         {
            this.setAdapter(adapterName, adapterPort);
         }
         else
         {
            try
            {
               adapter = OneWireAccessProvider.DefaultAdapter;
               AdapterStatus.Text = adapter.ToString();
            }
            catch (Exception exc)
            {
               AdapterStatus.Text = exc.Message;
               if (adapter != null)
                  adapter.endExclusive();
               adapter = null;
            }
         }

         if (DeviceOptions.TabPages.IndexOf(TemperatureTab) >= 0)
            DeviceOptions.TabPages.Remove(TemperatureTab);
         if (DeviceOptions.TabPages.IndexOf(HumidityTab) >= 0)
            DeviceOptions.TabPages.Remove(HumidityTab);

         timer1.Enabled = true;
         //RefreshDeviceList();
      }

      private void deleteAdapterSelections()
      {
         // clear DS9097U adapter selections
         menuItem9.MenuItems.Clear();
         // clear DS9097E adapter selections
         menuItem24.MenuItems.Clear();
         // clear DS9490 adapter selections
         menuItem19.MenuItems.Clear();
      }

      private void DeviceListbox_SelectedIndexChanged(object sender, EventArgs e)
      {
         DeviceOptions.SelectedIndex = 0;
         Status.Text = "Status";
         if (DeviceListbox.SelectedItem == null) return;
         String thisDeviceSelection = DeviceListbox.SelectedItem.ToString();

         if (thisDeviceSelection.Equals(previousDeviceSelection))
         {
            return;
         }

         DescriptionTextBox.Clear();

         if (DeviceListbox.SelectedItem != null && owc41 != DeviceListbox.SelectedItem)
         {
            owc41 = (OneWireContainer41)DeviceListbox.SelectedItem;
            updateContainerProperties(owc41);

            missionCache = (MissionCache)DeviceHashtable[owc41];

            updateDescriptionTab();
            graphHumidity();
            graphTemperature();
         }
         else
         {
            if (DeviceOptions.TabPages.IndexOf(TemperatureTab) >= 0)
               DeviceOptions.TabPages.Remove(TemperatureTab);
            if (DeviceOptions.TabPages.IndexOf(HumidityTab) >= 0)
               DeviceOptions.TabPages.Remove(HumidityTab);
         }
         previousDeviceSelection = thisDeviceSelection;
      }
      // The code in the KeyPress and KeyDown events suppress any modifications to the data 
      // in the DescriptionTextBox -- kinda like setting the TextBox to "ReadOnly"
      private void DescriptionTextBox_KeyPress(object sender, KeyPressEventArgs e)
      {
         // catch letters and numbers here
         e.Handled = true;
         return;
      }

      private void DescriptionTextBox_KeyDown(object sender, KeyEventArgs e)
      {
         // catch special function keys (delete, backspace, mouse clicks) in this event
         if (e.KeyCode != Keys.RButton)
         {
            e.Handled = true;
         }
         return;
      }

      private void menuItem10_Click(object sender, EventArgs e)
      {
         // show the progress form
         progressForm.Show();
         // disable some of the GUI components
         enableGUI(false);
         // start thread
         if (backgroundWorkerAdapterDetection.IsBusy != true)
         {
            backgroundWorkerAdapterDetection.RunWorkerAsync();
         }
      }

      private void backgroundWorkerAdapterDetection_DoWork(object sender, DoWorkEventArgs e)
      {
         string portName = "";
         string adapterName = "";
         System.Collections.ArrayList comports;
         bool performAdapterDetection = true;
         int progressTick = 0;
         DSPortAdapter threadAdapter;
         int i = 0;

         timer1.Stop();
         if (adapter != null)
         {
            // save off original adapter
            portName = adapter.PortName;
            adapterName = adapter.AdapterName;
         }
 
         deleteAdapterSelections(); // delete all adapter selections
         progressTick += 5;
         // set the device list box's index to -1.  Do this through progress changed
         backgroundWorkerAdapterDetection.ReportProgress(progressTick,(int)-1);         

         comports = getComPortList();
         progressTick += 5;
         backgroundWorkerAdapterDetection.ReportProgress(progressTick);

         // if performing an exhaustive search with adapter detection, cycle through 
         // each possible adapter and if present, then add it to the menu
         if (performAdapterDetection)
         {
 
            // now that we have a com port list, let's figure out if an adapter is actually present
            for (i = 0; i < comports.Count; i++) // for DS9097U or DS9481
            {
               try
               {
                  threadAdapter = OneWireAccessProvider.getAdapter("{DS9097U_DS948X}", comports[i].ToString());
               }
               catch (Exception except)
               {
                  if (except.Equals(null)) { } // drain exception
                  continue;
               }
               menuItem9.MenuItems.Add(comports[i].ToString(), DS9097U_Click);
               threadAdapter.freePort(); // necessary
               threadAdapter = null; // necessary
            }

            progressTick += 30;
            backgroundWorkerAdapterDetection.ReportProgress(progressTick);

            for (i = 0; i < comports.Count; i++) // for DS9097E
            {
               try
               {
                  threadAdapter = OneWireAccessProvider.getAdapter("{DS9097E}", comports[i].ToString());
               }
               catch (Exception except)
               {
                  if (except.Equals(null)) { } // drain exception
                  continue;
               }
               menuItem9.MenuItems.Add(comports[i].ToString(), DS9097U_Click);
               threadAdapter.freePort(); // necessary
               threadAdapter = null; // necessary
            }

            progressTick += 30;
            backgroundWorkerAdapterDetection.ReportProgress(progressTick);

            for (i = 0; i < 16; i++) // for DS9490
            {
               try
               {
                  threadAdapter = OneWireAccessProvider.getAdapter("{DS9490}", "USB" + i.ToString());
               }
               catch (Exception except)
               {
                  if (except.Equals(null)) { } // drain exception
                  continue;
               }
               menuItem19.MenuItems.Add(("USB" + i.ToString()), DS9490_Click);
               threadAdapter.freePort();
               threadAdapter = null;
            }

            if (adapter != null)
            {
               // set to null
               adapter.freePort();
               adapter = null;
            }

            // get original adapter back for the timer 
            if (!(adapterName.Equals("") || portName.Equals("")))
            {
               //setAdapter(adapterName, portName);
               try
               {
                  adapter = OneWireAccessProvider.getAdapter(adapterName, portName);
                  DeviceHashtable.Clear();
                  previousDeviceSelection = "";
                  // clear devicelistbox.  Do this through progress changed
                  backgroundWorkerAdapterDetection.ReportProgress(progressTick, (int)0);         
               }
               catch (Exception except)
               {
                  if (except.Equals(null)) { } // drain exception
               }
            }
         }
         else
         {
            // add menu items
            for (i = 0; i < comports.Count; i++)
            {
               menuItem24.MenuItems.Add(comports[i].ToString(), DS9097E_Click);
               menuItem9.MenuItems.Add(comports[i].ToString(), DS9097U_Click);
            }
            for (i = 0; i < 16; i++)
            {
               menuItem19.MenuItems.Add(("USB" + i.ToString()), DS9490_Click);
            }
         }
         progressTick += 30;
         backgroundWorkerAdapterDetection.ReportProgress(progressTick);         
      }

      private void backgroundWorkerAdapterDetection_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         enableGUI(true); // enable the form
         progressForm.Hide(); // hide the progress form
         RefreshDeviceList();
         timer1.Start(); // re-start timer
      }

      private void backgroundWorkerAdapterDetection_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         int state = 0;
         if (e.UserState != null)
         {
            state = (int)e.UserState;
            switch (state)
            {
               case -1:
                  DeviceListbox.SelectedIndex = -1;
                  break;
               case 0:
                  DeviceListbox.Items.Clear();
                  break;
               case 1:
                  //this.Enabled = false;
                  //enableGUI(false);
                  break;
               default:
                  break;
            }
         }
         progressForm.setProgressBar(e.ProgressPercentage);
      }
      private void enableGUI(bool GUIEnable)
      {
         DeviceListbox.Enabled = GUIEnable;
         startNewMissionButton.Enabled = GUIEnable;
         stopMissionButton.Enabled = GUIEnable;
         LoadResultsButton.Enabled = GUIEnable;
         menuItem2.Enabled = GUIEnable; // Calibration menu item
         menuItem1.Enabled = GUIEnable; // Password menu item
         menuItem8.Enabled = GUIEnable; // File -> 1-Wire Adapter menu item
         GangMenuItem.Enabled = GUIEnable; // All Devices main menu item
         searchOptions.Enabled = GUIEnable; 
      }
   }
}
