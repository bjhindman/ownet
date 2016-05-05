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
	
	
	/// <summary> 1-Wire temperature interface class for basic temperature measuring
	/// operations. This class should be implemented for each temperature
	/// type 1-Wire device.
	/// 
	/// 
	/// <P>The TemperatureContainer methods can be organized into the following categories: </P>
	/// <UL>
	/// <LI> <B> Information </B>
	/// <UL>
	/// <LI> {@link #getMaxTemperature                  getMaxTemperature}
	/// <LI> {@link #getMinTemperature                  getMinTemperature}
	/// <LI> {@link #getTemperature                     getTemperature}
	/// <LI> {@link #getTemperatureAlarm                getTemperatureAlarm}
	/// <LI> {@link #getTemperatureAlarmResolution      getTemperatureAlarmResolution}
	/// <LI> {@link #getTemperatureResolution           getTemperatureResolution}
	/// <LI> {@link #getTemperatureResolutions          getTemperatureResolutions}
	/// <LI> {@link #hasSelectableTemperatureResolution hasSelectableTemperatureResolution}
	/// <LI> {@link #hasTemperatureAlarms               hasTemperatureAlarms}
	/// </UL>
	/// <LI> <B> Options </B>
	/// <UL>
	/// <LI> {@link #doTemperatureConvert     doTemperatureConvert}
	/// <LI> {@link #setTemperatureAlarm      setTemperatureAlarm}
	/// <LI> {@link #setTemperatureResolution setTemperatureResolution}
	/// </UL>
	/// <LI> <B> I/O </B>
	/// <UL>
	/// <LI> {@link #readDevice  readDevice}
	/// <LI> {@link #writeDevice writeDevice}
	/// </UL>
	/// </UL>
	/// 
	/// <H3> Usage </H3>
	/// 
	/// <DL>
	/// <DD> <H4> Example 1</H4>
	/// Display some features of TemperatureContainer instance '<code>tc</code>':
	/// <PRE> <CODE>
	/// // Read High and Low Alarms
	/// if (!tc.hasTemperatureAlarms())
	/// System.out.println("Temperature alarms not supported");
	/// else
	/// {
	/// byte[] state     = tc.readDevice();
	/// double alarmLow  = tc.getTemperatureAlarm(TemperatureContainer.ALARM_LOW, state);
	/// double alarmHigh = tc.getTemperatureAlarm(TemperatureContainer.ALARM_HIGH, state);
	/// System.out.println("Alarm: High = " + alarmHigh + ", Low = " + alarmLow);
	/// }             }
	/// </CODE> </PRE>
	/// 
	/// <DD> <H4> Example 2</H4>
	/// Gets temperature reading from a TemperatureContainer instance '<code>tc</code>':
	/// <PRE> <CODE>
	/// double lastTemperature;
	/// 
	/// // get the current resolution and other settings of the device (done only once)
	/// byte[] state = tc.readDevice();
	/// 
	/// do // loop to read the temp
	/// {
	/// // perform a temperature conversion
	/// tc.doTemperatureConvert(state);
	/// 
	/// // read the result of the conversion
	/// state = tc.readDevice();
	/// 
	/// // extract the result out of state
	/// lastTemperature = tc.getTemperature(state);
	/// ...
	/// 
	/// }while (!done);
	/// </CODE> </PRE>
	/// 
	/// The reason the conversion and the reading are separated
	/// is that one may want to do a conversion without reading
	/// the result.  One could take advantage of the alarm features
	/// of a device by setting a threshold and doing conversions
	/// until the device is alarming.  For example:
	/// <PRE> <CODE>
	/// // get the current resolution of the device
	/// byte [] state = tc.readDevice();
	/// 
	/// // set the trips
	/// tc.setTemperatureAlarm(TemperatureContainer.ALARM_HIGH, 50, state);
	/// tc.setTemperatureAlarm(TemperatureContainer.ALARM_LOW, 20, state);
	/// tc.writeDevice(state);
	/// 
	/// do // loop on conversions until an alarm occurs
	/// {
	/// tc.doTemperatureConvert(state);
	/// } while (!tc.isAlarming());
	/// </CODE> </PRE>
	/// 
	/// <DD> <H4> Example 3</H4>
	/// Sets the temperature resolution of a TemperatureContainer instance '<code>tc</code>':
	/// <PRE> <CODE>
	/// byte[] state = tc.readDevice();
	/// if (tc.hasSelectableTemperatureResolution())
	/// {
	/// double[] resolution = tc.getTemperatureResolutions();
	/// tc.setTemperatureResolution(resolution [resolution.length - 1], state);
	/// tc.writeDevice(state);
	/// }
	/// </CODE> </PRE>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer10">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer21">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer26">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer28">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer30">
	/// 
	/// </seealso>
	/// <version>     0.00, 27 August 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public struct TemperatureContainer_Fields{
		/// <summary>high temperature alarm </summary>
		public readonly static int ALARM_HIGH = 1;
		/// <summary>low temperature alarm </summary>
		public readonly static int ALARM_LOW = 0;
	}
	public interface TemperatureContainer:OneWireSensor
	{
		//UPGRADE_NOTE: Members of interface 'TemperatureContainer' were extracted into structure 'TemperatureContainer_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		/// <summary> Get an array of available temperature resolutions in Celsius.
		/// 
		/// </summary>
		/// <returns> byte array of available temperature resolutions in Celsius with
		/// minimum resolution as the first element and maximum resolution
		/// as the last element.
		/// 
		/// </returns>
		/// <seealso cref="hasSelectableTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolution">
		/// </seealso>
		/// <seealso cref="setTemperatureResolution">
		/// </seealso>
		double[] TemperatureResolutions
		{
			get;
			
		}
		/// <summary> Gets the temperature alarm resolution in Celsius.
		/// 
		/// </summary>
		/// <returns> temperature alarm resolution in Celsius for this 1-wire device
		/// 
		/// </returns>
		/// <throws>  OneWireException         Device does not support temperature </throws>
		/// <summary>                                  alarms
		/// 
		/// </summary>
		/// <seealso cref="hasTemperatureAlarms">
		/// </seealso>
		/// <seealso cref="getTemperatureAlarm">
		/// </seealso>
		/// <seealso cref="setTemperatureAlarm">
		/// 
		/// </seealso>
		double TemperatureAlarmResolution
		{
			get;
			
		}
		/// <summary> Gets the maximum temperature in Celsius.
		/// 
		/// </summary>
		/// <returns> maximum temperature in Celsius for this 1-wire device
		/// </returns>
		double MaxTemperature
		{
			get;
			
		}
		/// <summary> Gets the minimum temperature in Celsius.
		/// 
		/// </summary>
		/// <returns> minimum temperature in Celsius for this 1-wire device
		/// </returns>
		double MinTemperature
		{
			get;
			
		}
		
		//--------
		//-------- Static Final Variables
		//--------
		
		//--------
		//-------- Temperature Feature methods
		//--------
		
		/// <summary> Checks to see if this temperature measuring device has high/low
		/// trip alarms.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if this <code>TemperatureContainer</code>
		/// has high/low trip alarms
		/// 
		/// </returns>
		/// <seealso cref="getTemperatureAlarm">
		/// </seealso>
		/// <seealso cref="setTemperatureAlarm">
		/// </seealso>
		bool hasTemperatureAlarms();
		
		/// <summary> Checks to see if this device has selectable temperature resolution.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if this <code>TemperatureContainer</code>
		/// has selectable temperature resolution
		/// 
		/// </returns>
		/// <seealso cref="getTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolutions">
		/// </seealso>
		/// <seealso cref="setTemperatureResolution">
		/// </seealso>
		bool hasSelectableTemperatureResolution();
		
		//--------
		//-------- Temperature I/O Methods
		//--------
		
		/// <summary> Performs a temperature conversion.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// 
		/// </param>
		/// <throws>  OneWireException         Part could not be found [ fatal ] </throws>
		/// <throws>  OneWireIOException       Data wasn't transferred properly [ recoverable ] </throws>
		void  doTemperatureConvert(byte[] state);
		
		//--------
		//-------- Temperature 'get' Methods
		//--------
		
		/// <summary> Gets the temperature value in Celsius from the <code>state</code>
		/// data retrieved from the <code>readDevice()</code> method.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// 
		/// </param>
		/// <returns> temperature in Celsius from the last
		/// <code>doTemperatureConvert()</code>
		/// 
		/// </returns>
		/// <throws>  OneWireIOException In the case of invalid temperature data </throws>
		double getTemperature(byte[] state);
		
		/// <summary> Gets the specified temperature alarm value in Celsius from the
		/// <code>state</code> data retrieved from the
		/// <code>readDevice()</code> method.
		/// 
		/// </summary>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <param name="state">    byte array with device state information
		/// 
		/// </param>
		/// <returns> temperature alarm trip values in Celsius for this 1-wire device
		/// 
		/// </returns>
		/// <throws>  OneWireException         Device does not support temperature </throws>
		/// <summary>                                  alarms
		/// 
		/// </summary>
		/// <seealso cref="hasTemperatureAlarms">
		/// </seealso>
		/// <seealso cref="setTemperatureAlarm">
		/// </seealso>
		double getTemperatureAlarm(int alarmType, byte[] state);
		
		/// <summary> Gets the current temperature resolution in Celsius from the
		/// <code>state</code> data retrieved from the <code>readDevice()</code>
		/// method.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// 
		/// </param>
		/// <returns> temperature resolution in Celsius for this 1-wire device
		/// 
		/// </returns>
		/// <seealso cref="hasSelectableTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolutions">
		/// </seealso>
		/// <seealso cref="setTemperatureResolution">
		/// </seealso>
		double getTemperatureResolution(byte[] state);
		
		//--------
		//-------- Temperature 'set' Methods
		//--------
		
		/// <summary> Sets the temperature alarm value in Celsius in the provided
		/// <code>state</code> data.
		/// Use the method <code>writeDevice()</code> with
		/// this data to finalize the change to the device.
		/// 
		/// </summary>
		/// <param name="alarmType"> valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <param name="alarmValue">alarm trip value in Celsius
		/// </param>
		/// <param name="state">     byte array with device state information
		/// 
		/// </param>
		/// <throws>  OneWireException         Device does not support temperature </throws>
		/// <summary>                                  alarms
		/// 
		/// </summary>
		/// <seealso cref="hasTemperatureAlarms">
		/// </seealso>
		/// <seealso cref="getTemperatureAlarm">
		/// </seealso>
		void  setTemperatureAlarm(int alarmType, double alarmValue, byte[] state);
		
		/// <summary> Sets the current temperature resolution in Celsius in the provided
		/// <code>state</code> data.   Use the method <code>writeDevice()</code>
		/// with this data to finalize the change to the device.
		/// 
		/// </summary>
		/// <param name="resolution">temperature resolution in Celsius
		/// </param>
		/// <param name="state">     byte array with device state information
		/// 
		/// </param>
		/// <throws>  OneWireException         Device does not support selectable </throws>
		/// <summary>                                  temperature resolution
		/// 
		/// </summary>
		/// <seealso cref="hasSelectableTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolutions">
		/// </seealso>
		void  setTemperatureResolution(double resolution, byte[] state);
	}
}