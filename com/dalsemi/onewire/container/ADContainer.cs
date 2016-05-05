/*---------------------------------------------------------------------------
* Copyright (C) 1999,2000 Maxim Integrated Products, All Rights Reserved.
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
// imports
using System;
using OneWireException = com.dalsemi.onewire.OneWireException;
using OneWireIOException = com.dalsemi.onewire.adapter.OneWireIOException;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> <p>Interface class for 1-Wire&reg; devices that perform analog measuring
	/// operations. This class should be implemented for each A/D
	/// type 1-Wire device.</p>
	/// 
	/// <h3> Features </h3>
	/// 
	/// <ul>
	/// <li>
	/// Allows multi-channel voltage readings
	/// </li>
	/// <li>
	/// Supports A/D Alarm enabling on devices with A/D Alarms
	/// </li>
	/// <li>
	/// Supports selectable A/D ranges on devices with selectable ranges
	/// </li>
	/// <li>
	/// Supports selectable A/D resolutions on devices with selectable resolutions
	/// </li>
	/// </ul>
	/// 
	/// <h3> Usage </h3>
	/// 
	/// <p><code>ADContainer</code> extends <code>OneWireSensor</code>, so the general usage
	/// model applies to any <code>ADContainer</code>:
	/// <ol>
	/// <li> readDevice()  </li>
	/// <li> perform operations on the <code>ADContainer</code>  </li>
	/// <li> writeDevice(byte[]) </li>
	/// </ol>
	/// 
	/// <p>Consider this interaction with an <code>ADContainer</code> that reads from all of its
	/// A/D channels, then tries to set its high alarm on its first channel (channel 0):
	/// 
	/// <pre><code>
	/// //adcontainer is a com.dalsemi.onewire.container.ADContainer
	/// byte[] state = adcontainer.readDevice();
	/// double[] voltages = new double[adcontainer.getNumberADChannels()];
	/// for (int i=0; i &lt; adcontainer.getNumberADChannels(); i++)
	/// {
	/// adcontainer.doADConvert(i, state);         
	/// voltages[i] = adc.getADVoltage(i, state);
	/// }
	/// if (adcontainer.hasADAlarms())
	/// {
	/// double highalarm = adcontainer.getADAlarm(0, ADContainer.ALARM_HIGH, state);
	/// adcontainer.setADAlarm(0, ADContainer.ALARM_HIGH, highalarm + 1.0, state);
	/// adcontainer.writeDevice(state);
	/// }
	/// 
	/// </code></pre>
	/// 
	/// </summary>
	/// <seealso cref="OneWireSensor">
	/// </seealso>
	/// <seealso cref="ClockContainer">
	/// </seealso>
	/// <seealso cref="TemperatureContainer">
	/// </seealso>
	/// <seealso cref="PotentiometerContainer">
	/// </seealso>
	/// <seealso cref="SwitchContainer">
	/// 
	/// </seealso>
	/// <version>     0.00, 27 August 2000
	/// </version>
	/// <author>      DS, KLA
	/// </author>
	public struct ADContainer_Fields{
		/// <summary> Indicates the high AD alarm.</summary>
		public readonly static int ALARM_HIGH = 1;
		/// <summary> Indicates the low AD alarm.</summary>
		public readonly static int ALARM_LOW = 0;
	}
	public interface ADContainer:OneWireSensor
	{
		//UPGRADE_NOTE: Members of interface 'ADContainer' were extracted into structure 'ADContainer_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		/// <summary> Gets the number of channels supported by this A/D.
		/// Channel specific methods will use a channel number specified
		/// by an integer from [0 to (<code>getNumberADChannels()</code> - 1)].
		/// 
		/// </summary>
		/// <returns> the number of channels
		/// </returns>
		int NumberADChannels
		{
			get;
			
		}
		
		//--------
		//-------- Static Final Variables
		//--------
		
		//--------
		//-------- A/D Feature methods
		//--------
		
		/// <summary> Checks to see if this A/D measuring device has high/low
		/// alarms.
		/// 
		/// </summary>
		/// <returns> true if this device has high/low trip alarms
		/// </returns>
		bool hasADAlarms();
		
		/// <summary> Gets an array of available ranges for the specified
		/// A/D channel.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// 
		/// </param>
		/// <returns> array indicating the available ranges starting
		/// from the largest range to the smallest range
		/// 
		/// </returns>
		/// <seealso cref="getNumberADChannels()">
		/// </seealso>
		double[] getADRanges(int channel);
		
		/// <summary> Gets an array of available resolutions based
		/// on the specified range on the specified A/D channel.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="range">A/D range setting from the <code>getADRanges(int)</code> method
		/// 
		/// </param>
		/// <returns> array indicating the available resolutions on this 
		/// <code>channel</code> for this <code>range</code>
		/// 
		/// </returns>
		/// <seealso cref="getNumberADChannels()">
		/// </seealso>
		/// <seealso cref="getADRanges(int)">
		/// </seealso>
		double[] getADResolutions(int channel, double range);
		
		/// <summary> Checks to see if this A/D supports doing multiple voltage
		/// conversions at the same time.
		/// 
		/// </summary>
		/// <returns> true if the device can do multi-channel voltage reads
		/// 
		/// </returns>
		/// <seealso cref="doADConvert(boolean[],byte[])">
		/// </seealso>
		bool canADMultiChannelRead();
		
		//--------
		//-------- A/D IO Methods
		//--------
		
		/// <summary> Performs a voltage conversion on one specified channel.  
		/// Use the method <code>getADVoltage(int,byte[])</code> to read 
		/// the result of this conversion, using the same <code>channel</code>
		/// argument as this method uses.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         no 1-Wire device present.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// This is usually a recoverable error.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the </throws>
		/// <summary>         1-Wire adapter.  This is usually a non-recoverable error.
		/// 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="getADVoltage(int,byte[])">
		/// </seealso>
		void  doADConvert(int channel, byte[] state);
		
		/// <summary> Performs voltage conversion on one or more specified
		/// channels.  The method <code>getADVoltage(byte[])</code> can be used to read the result
		/// of the conversion(s). This A/D must support multi-channel read,
		/// reported by <code>canADMultiChannelRead()</code>, if more then 1 channel is specified.
		/// 
		/// </summary>
		/// <param name="doConvert">array of size <code>getNumberADChannels()</code> representing 
		/// which channels should perform conversions
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         no 1-Wire device present.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// This is usually a recoverable error.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the </throws>
		/// <summary>         1-Wire adapter.  This is usually a non-recoverable error.
		/// 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="getADVoltage(byte[])">
		/// </seealso>
		/// <seealso cref="canADMultiChannelRead()">
		/// </seealso>
		void  doADConvert(bool[] doConvert, byte[] state);
		
		/// <summary> Reads the value of the voltages after a <code>doADConvert(boolean[],byte[])</code>
		/// method call.  This A/D device must support multi-channel reading, reported by
		/// <code>canADMultiChannelRead()</code>, if more than 1 channel conversion was attempted
		/// by <code>doADConvert()</code>.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> array with the voltage values for all channels
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         no 1-Wire device present.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// This is usually a recoverable error.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the </throws>
		/// <summary>         1-Wire adapter.  This is usually a non-recoverable error.
		/// 
		/// </summary>
		/// <seealso cref="doADConvert(boolean[],byte[])">
		/// </seealso>
		double[] getADVoltage(byte[] state);
		
		/// <summary> Reads the value of the voltages after a <code>doADConvert(int,byte[])</code>
		/// method call.  If more than one channel has been read it is more
		/// efficient to use the <code>getADVoltage(byte[])</code> method that 
		/// returns all channel voltage values.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> the voltage value for the specified channel
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         no 1-Wire device present.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// This is usually a recoverable error.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the </throws>
		/// <summary>         1-Wire adapter.  This is usually a non-recoverable error.
		/// 
		/// </summary>
		/// <seealso cref="doADConvert(int,byte[])">
		/// </seealso>
		/// <seealso cref="getADVoltage(byte[])">
		/// </seealso>
		double getADVoltage(int channel, byte[] state);
		
		//--------
		//-------- A/D 'get' Methods
		//--------
		
		/// <summary> Reads the value of the specified A/D alarm on the specified channel.
		/// Not all A/D devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasADAlarms()</code> method.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="alarmType">the desired alarm, <code>ALARM_HIGH</code> or <code>ALARM_LOW</code>
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> the alarm value in volts
		/// 
		/// </returns>
		/// <throws>  OneWireException if this device does not have A/D alarms </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="hasADAlarms()">
		/// </seealso>
		double getADAlarm(int channel, int alarmType, byte[] state);
		
		/// <summary> Checks to see if the specified alarm on the specified channel is enabled.
		/// Not all A/D devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasADAlarms()</code> method.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="alarmType">the desired alarm, <code>ALARM_HIGH</code> or <code>ALARM_LOW</code>
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> true if specified alarm is enabled
		/// 
		/// </returns>
		/// <throws>  OneWireException if this device does not have A/D alarms </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="hasADAlarms()">
		/// </seealso>
		bool getADAlarmEnable(int channel, int alarmType, byte[] state);
		
		/// <summary> Checks the state of the specified alarm on the specified channel.
		/// Not all A/D devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasADAlarms()</code> method.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="alarmType">the desired alarm, <code>ALARM_HIGH</code> or <code>ALARM_LOW</code>
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> true if specified alarm occurred
		/// 
		/// </returns>
		/// <throws>  OneWireException if this device does not have A/D alarms </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="hasADAlarms()">
		/// </seealso>
		/// <seealso cref="getADAlarmEnable(int,int,byte[])">
		/// </seealso>
		/// <seealso cref="setADAlarmEnable(int,int,boolean,byte[])">
		/// </seealso>
		bool hasADAlarmed(int channel, int alarmType, byte[] state);
		
		/// <summary> Returns the currently selected resolution for the specified
		/// channel.  This device may not have selectable resolutions,
		/// though this method will return a valid value.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> the current resolution of <code>channel</code> in volts
		/// 
		/// </returns>
		/// <seealso cref="getADResolutions(int,double)">
		/// </seealso>
		/// <seealso cref="setADResolution(int,double,byte[])">
		/// </seealso>
		double getADResolution(int channel, byte[] state);
		
		/// <summary> Returns the currently selected range for the specified
		/// channel.  This device may not have selectable ranges,
		/// though this method will return a valid value.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> the input voltage range
		/// 
		/// </returns>
		/// <seealso cref="getADRanges(int)">
		/// </seealso>
		/// <seealso cref="setADRange(int,double,byte[])">
		/// </seealso>
		double getADRange(int channel, byte[] state);
		
		//--------
		//-------- A/D 'set' Methods
		//--------
		
		/// <summary> Sets the voltage value of the specified alarm on the specified channel.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.  Also note that
		/// not all A/D devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasADAlarms()</code> method.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="alarmType">the desired alarm, <code>ALARM_HIGH</code> or <code>ALARM_LOW</code>
		/// </param>
		/// <param name="alarm">new alarm value
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <throws>  OneWireException if this device does not have A/D alarms </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="hasADAlarms()">
		/// </seealso>
		/// <seealso cref="getADAlarm(int,int,byte[])">
		/// </seealso>
		/// <seealso cref="getADAlarmEnable(int,int,byte[])">
		/// </seealso>
		/// <seealso cref="setADAlarmEnable(int,int,boolean,byte[])">
		/// </seealso>
		/// <seealso cref="hasADAlarmed(int,int,byte[])">
		/// </seealso>
		void  setADAlarm(int channel, int alarmType, double alarm, byte[] state);
		
		/// <summary> Enables or disables the specified alarm on the specified channel.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.  Also note that
		/// not all A/D devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasADAlarms()</code> method.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="alarmType">the desired alarm, <code>ALARM_HIGH</code> or <code>ALARM_LOW</code>
		/// </param>
		/// <param name="alarmEnable">true to enable the alarm, false to disable
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <throws>  OneWireException if this device does not have A/D alarms </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="hasADAlarms()">
		/// </seealso>
		/// <seealso cref="getADAlarm(int,int,byte[])">
		/// </seealso>
		/// <seealso cref="setADAlarm(int,int,double,byte[])">
		/// </seealso>
		/// <seealso cref="getADAlarmEnable(int,int,byte[])">
		/// </seealso>
		/// <seealso cref="hasADAlarmed(int,int,byte[])">
		/// </seealso>
		void  setADAlarmEnable(int channel, int alarmType, bool alarmEnable, byte[] state);
		
		/// <summary> Sets the conversion resolution value for the specified channel.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.  Also note that
		/// not all A/D devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasADAlarms()</code> method.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="resolution">one of the resolutions returned by <code>getADResolutions(int,double)</code>
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="getADResolutions(int,double)">
		/// </seealso>
		/// <seealso cref="getADResolution(int,byte[])">
		/// 
		/// </seealso>
		void  setADResolution(int channel, double resolution, byte[] state);
		
		/// <summary> Sets the input range for the specified channel.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.  Also note that
		/// not all A/D devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasADAlarms()</code> method.
		/// 
		/// </summary>
		/// <param name="channel">channel number in the range [0 to (<code>getNumberADChannels()</code> - 1)]
		/// </param>
		/// <param name="range">one of the ranges returned by <code>getADRanges(int)</code>
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="getADRanges(int)">
		/// </seealso>
		/// <seealso cref="getADRange(int,byte[])">
		/// </seealso>
		void  setADRange(int channel, double range, byte[] state);
	}
}