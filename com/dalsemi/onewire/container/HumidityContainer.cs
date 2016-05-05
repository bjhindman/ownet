/*---------------------------------------------------------------------------
* Copyright (C) 2001 Maxim Integrated Products, All Rights Reserved.
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
	
	
	/// <summary> 1-Wire Humidity interface class for basic Humidity measuring
	/// operations. This class should be implemented for each Humidity
	/// type 1-Wire device.
	/// 
	/// 
	/// <P>The HumidityContainer methods can be organized into the following categories: </P>
	/// <UL>
	/// <LI> <B> Information </B>
	/// <UL>
	/// <LI> {@link #getHumidity                        getHumidity}
	/// <LI> {@link #getHumidityResolution              getHumidityResolution}
	/// <LI> {@link #getHumidityAlarm                   getHumidityAlarm}
	/// <LI> {@link #getHumidityAlarmResolution         getHumidityAlarmResolution}
	/// <LI> {@link #getHumidityResolution              getHumidityResolution}
	/// <LI> {@link #getHumidityResolutions             getHumidityResolutions}
	/// <LI> {@link #hasSelectableHumidityResolution    hasSelectableHumidityResolution}
	/// <LI> {@link #hasHumidityAlarms                  hasHumidityAlarms}
	/// <LI> {@link #isRelative                         isRelative}
	/// </UL>
	/// <LI> <B> Options </B>
	/// <UL>
	/// <LI> {@link #doHumidityConvert     doHumidityConvert}
	/// <LI> {@link #setHumidityAlarm      setHumidityAlarm}
	/// <LI> {@link #setHumidityResolution setHumidityResolution}
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
	/// <DD> <H4> Example</H4>
	/// Gets humidity reading from a HumidityContainer instance '<code>hc</code>':
	/// <PRE> <CODE>
	/// double lastHumidity;
	/// 
	/// // get the current resolution and other settings of the device (done only once)
	/// byte[] state = hc.readDevice();
	/// 
	/// // loop to read the humidity
	/// do 
	/// {
	/// // perform a humidity conversion
	/// hc.doHumidityConvert(state);
	/// 
	/// // read the result of the conversion
	/// state = hc.readDevice();
	/// 
	/// // extract the result out of state
	/// lastHumidity = hc.getHumidity(state);
	/// ...
	/// 
	/// }
	/// while (!done);
	/// </CODE> </PRE>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer28">
	/// 
	/// </seealso>
	/// <version>     0.00, 27 August 2001
	/// </version>
	/// <author>      DS
	/// </author>
	public struct HumidityContainer_Fields{
		/// <summary>high temperature alarm </summary>
		public readonly static int ALARM_HIGH = 1;
		/// <summary>low temperature alarm </summary>
		public readonly static int ALARM_LOW = 0;
	}
	public interface HumidityContainer:OneWireSensor
	{
		//UPGRADE_NOTE: Members of interface 'HumidityContainer' were extracted into structure 'HumidityContainer_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		/// <summary> Checks to see if humidity value given is a 'relative' humidity value.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if this <code>HumidityContainer</code>
		/// provides a relative humidity reading
		/// 
		/// </returns>
		/// <seealso cref="getHumidityResolution">
		/// </seealso>
		/// <seealso cref="getHumidityResolutions">
		/// </seealso>
		/// <seealso cref="setHumidityResolution">
		/// </seealso>
		bool Relative
		{
			get;
			
		}
		/// <summary> Get an array of available Humidity resolutions in percent humidity (0 to 100).
		/// 
		/// </summary>
		/// <returns> byte array of available Humidity resolutions in percent with
		/// minimum resolution as the first element and maximum resolution
		/// as the last element.
		/// 
		/// </returns>
		/// <seealso cref="hasSelectableHumidityResolution">
		/// </seealso>
		/// <seealso cref="getHumidityResolution">
		/// </seealso>
		/// <seealso cref="setHumidityResolution">
		/// </seealso>
		double[] HumidityResolutions
		{
			get;
			
		}
		/// <summary> Gets the Humidity alarm resolution in percent.
		/// 
		/// </summary>
		/// <returns> Humidity alarm resolution in percent for this 1-wire device
		/// 
		/// </returns>
		/// <throws>  OneWireException         Device does not support Humidity </throws>
		/// <summary>                                  alarms
		/// 
		/// </summary>
		/// <seealso cref="hasHumidityAlarms">
		/// </seealso>
		/// <seealso cref="getHumidityAlarm">
		/// </seealso>
		/// <seealso cref="setHumidityAlarm">
		/// 
		/// </seealso>
		double HumidityAlarmResolution
		{
			get;
			
		}
		//--------
		//-------- Static Final Variables
		//--------
		
		//--------
		//-------- Humidity Feature methods
		//--------
		
		/// <summary> Checks to see if this Humidity measuring device has high/low
		/// trip alarms.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if this <code>HumidityContainer</code>
		/// has high/low trip alarms
		/// 
		/// </returns>
		/// <seealso cref="getHumidityAlarm">
		/// </seealso>
		/// <seealso cref="setHumidityAlarm">
		/// </seealso>
		bool hasHumidityAlarms();
		
		/// <summary> Checks to see if this device has selectable Humidity resolution.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if this <code>HumidityContainer</code>
		/// has selectable Humidity resolution
		/// 
		/// </returns>
		/// <seealso cref="getHumidityResolution">
		/// </seealso>
		/// <seealso cref="getHumidityResolutions">
		/// </seealso>
		/// <seealso cref="setHumidityResolution">
		/// </seealso>
		bool hasSelectableHumidityResolution();
		
		//--------
		//-------- Humidity I/O Methods
		//--------
		
		/// <summary> Performs a Humidity conversion.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as  </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to 
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		void  doHumidityConvert(byte[] state);
		
		//--------
		//-------- Humidity 'get' Methods
		//--------
		
		/// <summary> Gets the humidity expressed as a percent value (0.0 to 100.0) of humidity.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// </param>
		/// <returns> humidity expressed as a percent
		/// 
		/// </returns>
		/// <seealso cref="hasSelectableHumidityResolution">
		/// </seealso>
		/// <seealso cref="getHumidityResolution">
		/// </seealso>
		/// <seealso cref="setHumidityResolution">
		/// </seealso>
		double getHumidity(byte[] state);
		
		/// <summary> Gets the current Humidity resolution in percent from the
		/// <code>state</code> data retrieved from the <code>readDevice()</code>
		/// method.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// 
		/// </param>
		/// <returns> Humidity resolution in percent for this 1-wire device
		/// 
		/// </returns>
		/// <seealso cref="hasSelectableHumidityResolution">
		/// </seealso>
		/// <seealso cref="getHumidityResolutions">
		/// </seealso>
		/// <seealso cref="setHumidityResolution">
		/// </seealso>
		double getHumidityResolution(byte[] state);
		
		/// <summary> Gets the specified Humidity alarm value in percent from the
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
		/// <returns> Humidity alarm trip values in percent for this 1-wire device
		/// 
		/// </returns>
		/// <throws>  OneWireException         Device does not support Humidity </throws>
		/// <summary>                                  alarms
		/// 
		/// </summary>
		/// <seealso cref="hasHumidityAlarms">
		/// </seealso>
		/// <seealso cref="setHumidityAlarm">
		/// </seealso>
		double getHumidityAlarm(int alarmType, byte[] state);
		
		//--------
		//-------- Humidity 'set' Methods
		//--------
		
		/// <summary> Sets the Humidity alarm value in percent in the provided
		/// <code>state</code> data.
		/// Use the method <code>writeDevice()</code> with
		/// this data to finalize the change to the device.
		/// 
		/// </summary>
		/// <param name="alarmType"> valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <param name="alarmValue">alarm trip value in percent
		/// </param>
		/// <param name="state">     byte array with device state information
		/// 
		/// </param>
		/// <throws>  OneWireException         Device does not support Humidity </throws>
		/// <summary>                                  alarms
		/// 
		/// </summary>
		/// <seealso cref="hasHumidityAlarms">
		/// </seealso>
		/// <seealso cref="getHumidityAlarm">
		/// </seealso>
		void  setHumidityAlarm(int alarmType, double alarmValue, byte[] state);
		
		/// <summary> Sets the current Humidity resolution in percent in the provided
		/// <code>state</code> data.   Use the method <code>writeDevice()</code>
		/// with this data to finalize the change to the device.
		/// 
		/// </summary>
		/// <param name="resolution">Humidity resolution in percent
		/// </param>
		/// <param name="state">     byte array with device state information
		/// 
		/// </param>
		/// <throws>  OneWireException         Device does not support selectable </throws>
		/// <summary>                                  Humidity resolution
		/// 
		/// </summary>
		/// <seealso cref="hasSelectableHumidityResolution">
		/// </seealso>
		/// <seealso cref="getHumidityResolution">
		/// </seealso>
		/// <seealso cref="getHumidityResolutions">
		/// </seealso>
		void  setHumidityResolution(double resolution, byte[] state);
	}
}