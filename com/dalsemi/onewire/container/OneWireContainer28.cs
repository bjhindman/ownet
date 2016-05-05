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
using com.dalsemi.onewire;
using com.dalsemi.onewire.utils;
using com.dalsemi.onewire.adapter;
namespace com.dalsemi.onewire.container
{
	
	
	//----------------------------------------------------------------------------
	
	/// <summary> <P> 1-Wire container for temperature iButton which measures temperatures
	/// from -55@htmlonly &#176C @endhtmlonly to +125@htmlonly &#176C @endhtmlonly, DS18B20.  This container encapsulates the
	/// functionality of the iButton family type <B>28</B> (hex)</P>
	/// 
	/// <H3> Features </H3>
	/// <UL>
	/// <LI> Measures temperatures from -55@htmlonly &#176C @endhtmlonly to +125@htmlonly &#176C @endhtmlonly. Fahrenheit
	/// equivalent is -67@htmlonly &#176F @endhtmlonly to +257@htmlonly &#176F @endhtmlonly
	/// <LI> Power supply range is 3.0V to 5.5V
	/// <LI> Zero standby power
	/// <LI> +/- 0.5@htmlonly &#176C @endhtmlonly accuracy from -10@htmlonly &#176C @endhtmlonly to +85@htmlonly &#176C @endhtmlonly
	/// <LI> Thermometer resolution programmable from 9 to 12 bits
	/// <LI> Converts 12-bit temperature to digital word in 750 ms (max.)
	/// <LI> User-definable, nonvolatile temperature alarm settings
	/// <LI> Alarm search command identifies and addresses devices whose temperature is
	/// outside of programmed limits (temperature alarm condition)
	/// </UL>
	/// 
	/// <H3> Usage </H3>
	/// 
	/// <DL>
	/// <DD> See the usage example in
	/// {@link com.dalsemi.onewire.container.TemperatureContainer TemperatureContainer}
	/// for temperature specific operations.
	/// </DL>
	/// 
	/// <H3> DataSheet </H3>
	/// <DL>
	/// <DD><A HREF="http://pdfserv.maxim-ic.com/arpdf/DS18B20.pdf"> http://pdfserv.maxim-ic.com/arpdf/DS18B20.pdf</A>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.TemperatureContainer">
	/// 
	/// </seealso>
	/// <version>     1.00, 15 September 2000
	/// </version>
	/// <author>      BH
	/// </author>
	public class OneWireContainer28:OneWireContainer, TemperatureContainer
	{
		/// <summary> Retrieves the Maxim Integrated Products part number of this
		/// <code>OneWireContainer28</code> as a <code>String</code>.
		/// For example 'DS18B20'.
		/// 
		/// </summary>
		/// <returns> this <code>OneWireContainer28</code> name
		/// </returns>
		override public System.String Name
		{
			get
			{
				return "DS18B20";
			}
			
		}
		/// <summary> Retrieves the alternate Maxim Integrated Products part numbers or names.
		/// A 'family' of 1-Wire Network devices may have more than one part number
		/// depending on packaging.  There can also be nicknames such as
		/// 'Crypto iButton'.
		/// 
		/// </summary>
		/// <returns> this <code>OneWireContainer28</code> alternate names
		/// </returns>
		override public System.String AlternateNames
		{
			get
			{
				return "DS1820B, DS18B20X";
			}
			
		}
		/// <summary> Retrieves a short description of the function of this
		/// <code>OneWireContainer28</code> type.
		/// 
		/// </summary>
		/// <returns> <code>OneWireContainer28</code> functional description
		/// </returns>
		override public System.String Description
		{
			get
			{
				return "Digital thermometer measures temperatures from " + "-55C to 125C in 0.75 seconds (max).  +/- 0.5C " + "accuracy between -10C and 85C. Thermometer " + "resolution is programmable at 9, 10, 11, and 12 bits. ";
			}
			
		}
		/// <summary> Gets an array of available temperature resolutions in Celsius.
		/// 
		/// </summary>
		/// <returns> byte array of available temperature resolutions in Celsius for
		/// this <code>OneWireContainer28</code>. The minimum resolution is
		/// returned as the first element and maximum resolution as the last
		/// element.
		/// 
		/// </returns>
		/// <seealso cref="hasSelectableTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolution">
		/// </seealso>
		/// <seealso cref="setTemperatureResolution">
		/// </seealso>
		virtual public double[] TemperatureResolutions
		{
			get
			{
				double[] resolutions = new double[4];
				
				resolutions[0] = (double) 0.5; // 9-bit
				resolutions[1] = (double) 0.25; // 10-bit
				resolutions[2] = (double) 0.125; // 11-bit
				resolutions[3] = (double) 0.0625; // 12-bit
				
				return resolutions;
			}
			
		}
		/// <summary> Gets the temperature alarm resolution in Celsius.
		/// 
		/// </summary>
		/// <returns> temperature alarm resolution in Celsius for this
		/// <code>OneWireContainer28</code>
		/// 
		/// </returns>
		/// <seealso cref="hasTemperatureAlarms">
		/// </seealso>
		/// <seealso cref="getTemperatureAlarm">
		/// </seealso>
		/// <seealso cref="setTemperatureAlarm">
		/// 
		/// </seealso>
		virtual public double TemperatureAlarmResolution
		{
			get
			{
				return 1.0;
			}
			
		}
		/// <summary> Gets the maximum temperature in Celsius.
		/// 
		/// </summary>
		/// <returns> maximum temperature in Celsius for this
		/// <code>OneWireContainer28</code>
		/// 
		/// </returns>
		/// <seealso cref="getMinTemperature">
		/// </seealso>
		virtual public double MaxTemperature
		{
			get
			{
				return 125.0;
			}
			
		}
		/// <summary> Gets the minimum temperature in Celsius.
		/// 
		/// </summary>
		/// <returns> minimum temperature in Celsius for this
		/// <code>OneWireContainer28</code>
		/// 
		/// </returns>
		/// <seealso cref="getMaxTemperature">
		/// </seealso>
		virtual public double MinTemperature
		{
			get
			{
				return (-55.0);
			}
			
		}
		/// <summary> Reads the way power is supplied to the DS18B20.
		/// 
		/// </summary>
		/// <returns> <code>true</code> for external power, <BR>
		/// <code>false</code> for parasite power
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer28</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		virtual public bool ExternalPowerSupplied
		{
			get
			{
				int intresult = 0;
				bool result = false;
				
				// select the device
				if (adapter.select(address))
				{
					
					// send the "Read Power Supply" memory command
					adapter.putByte(READ_POWER_SUPPLY_COMMAND);
					
					// read results
					intresult = adapter.Byte;
				}
				else
				{
					
					// device must not have been present
					throw new OneWireIOException("OneWireContainer28-Device not found on 1-Wire Network");
				}
				
				if (intresult != 0x00)
					result = true; // reads 0xFF for true and 0x00 for false
				
				return result;
			}
			
		}
		
		//-------------------------------------------------------------------------
		//-------- Static Final Variables
		//-------------------------------------------------------------------------
		
		/// <summary>DS18B20 writes data to scratchpad command </summary>
		public static byte WRITE_SCRATCHPAD_COMMAND = (byte) 0x4E;
		
		/// <summary>DS18B20 reads data from scratchpad command </summary>
		public static byte READ_SCRATCHPAD_COMMAND = (byte) SupportClass.Identity(0xBE);
		
		/// <summary>DS18B20 copys data from scratchpad to E-squared memory command </summary>
		public static byte COPY_SCRATCHPAD_COMMAND = (byte) 0x48;
		
		/// <summary>DS18B20 converts temperature command </summary>
		public static byte CONVERT_TEMPERATURE_COMMAND = (byte) 0x44;
		
		/// <summary>DS18B20 recalls E-squared memory command </summary>
		public static byte RECALL_E2MEMORY_COMMAND = (byte) SupportClass.Identity(0xB8);
		
		/// <summary> DS18B20 reads power supply command.  This command is used to determine
		/// if external power is supplied.
		/// </summary>
		public static byte READ_POWER_SUPPLY_COMMAND = (byte) SupportClass.Identity(0xB4);
		
		/// <summary>DS18B20 12-bit resolution constant for CONFIG byte  </summary>
		public const byte RESOLUTION_12_BIT = (byte) 0x7F;
		
		/// <summary>DS18B20 11-bit resolution constant for CONFIG byte  </summary>
		public const byte RESOLUTION_11_BIT = (byte) 0x5F;
		
		/// <summary>DS18B20 10-bit resolution constant for CONFIG byte  </summary>
		public const byte RESOLUTION_10_BIT = (byte) 0x3F;
		
		/// <summary>DS18B20 9-bit resolution constant for CONFIG byte   </summary>
		public const byte RESOLUTION_9_BIT = (byte) 0x1F;
		
		/// <summary> Creates an empty <code>OneWireContainer28</code>.  Must call
		/// <code>setupContainer()</code> before using this new container.<p>
		/// 
		/// This is one of the methods to construct a <code>OneWireContainer28</code>.
		/// The others are through creating a <code>OneWireContainer28</code> with
		/// parameters.
		/// 
		/// </summary>
		/// <seealso cref="OneWireContainer28(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer28(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer28(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer28():base()
		{
		}
		
		/// <summary> Creates a <code>OneWireContainer28</code> with the provided adapter
		/// object and the address of this One-Wire device.
		/// 
		/// This is one of the methods to construct a <code>OneWireContainer28</code>.
		/// The others are through creating a <code>OneWireContainer28</code> with
		/// different parameters types.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this One-Wire device
		/// </param>
		/// <param name="newAddress">       address of this One-Wire device
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		/// <seealso cref="OneWireContainer28()">
		/// </seealso>
		/// <seealso cref="OneWireContainer28(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer28(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer28(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
		}
		
		/// <summary> Creates a <code>OneWireContainer28</code> with the provided adapter
		/// object and the address of this One-Wire device.
		/// 
		/// This is one of the methods to construct a <code>OneWireContainer28</code>.
		/// The others are through creating a <code>OneWireContainer28</code> with
		/// different parameters types.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this One-Wire device
		/// </param>
		/// <param name="newAddress">       address of this One-Wire device
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		/// <seealso cref="OneWireContainer28()">
		/// </seealso>
		/// <seealso cref="OneWireContainer28(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer28(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer28(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
		}
		
		/// <summary> Creates a <code>OneWireContainer28</code> with the provided adapter
		/// object and the address of this One-Wire device.
		/// 
		/// This is one of the methods to construct a <code>OneWireContainer28</code>.
		/// The others are through creating a <code>OneWireContainer28</code> with
		/// different parameters types.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this One-Wire device
		/// </param>
		/// <param name="newAddress">       address of this One-Wire device
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		/// <seealso cref="OneWireContainer28()">
		/// </seealso>
		/// <seealso cref="OneWireContainer28(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer28(DSPortAdapter,long)">
		/// </seealso>
		public OneWireContainer28(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
		}
		
		//--------
		//-------- Information methods
		//--------
		
		//--------
		//-------- Temperature Feature methods
		//--------
		
		/// <summary> Checks to see if this temperature measuring device has high/low
		/// trip alarms.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if this <code>OneWireContainer28</code>
		/// has high/low trip alarms
		/// 
		/// </returns>
		/// <seealso cref="getTemperatureAlarm">
		/// </seealso>
		/// <seealso cref="setTemperatureAlarm">
		/// </seealso>
		public virtual bool hasTemperatureAlarms()
		{
			return true;
		}
		
		/// <summary> Checks to see if this device has selectable temperature resolution.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if this <code>OneWireContainer28</code>
		/// has selectable temperature resolution
		/// 
		/// </returns>
		/// <seealso cref="getTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolutions">
		/// </seealso>
		/// <seealso cref="setTemperatureResolution">
		/// </seealso>
		public virtual bool hasSelectableTemperatureResolution()
		{
			return true;
		}
		
		//--------
		//-------- Temperature I/O Methods
		//--------
		
		/// <summary> Performs a temperature conversion on <code>state</code> information.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer28</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// 
		/// </summary>
		/// <seealso cref="getTemperature">
		/// </seealso>
		public virtual void  doTemperatureConvert(byte[] state)
		{
			int msDelay = 750; // in milliseconds
			
			// select the device
			if (adapter.select(address))
			{
				
				// Setup Power Delivery
				adapter.PowerDuration = DSPortAdapter.DELIVERY_INFINITE;
				adapter.startPowerDelivery(DSPortAdapter.CONDITION_AFTER_BYTE);
				// send the convert temperature command
				adapter.putByte(CONVERT_TEMPERATURE_COMMAND);
				
				// calculate duration of delay according to resolution desired
				switch (state[4])
				{
					
					
					case RESOLUTION_9_BIT: 
						msDelay = 94;
						break;
					
					case RESOLUTION_10_BIT: 
						msDelay = 188;
						break;
					
					case RESOLUTION_11_BIT: 
						msDelay = 375;
						break;
					
					case RESOLUTION_12_BIT: 
						msDelay = 750;
						break;
					
					default: 
						msDelay = 750;
						break;
					
				} // switch
				
				// delay for specified amount of time
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * msDelay));
				}
				catch (System.Threading.ThreadInterruptedException e)
				{
				}
				
				// Turn power back to normal.
				adapter.setPowerNormal();
				
				// check to see if the temperature conversion is over
				if (adapter.Byte != 0xFF)
					throw new OneWireIOException("OneWireContainer28-temperature conversion not complete");
				
				// BH added call to recallE2 to get new converted temperature into "state" variable
				adapter.select(address);
				state = recallE2();
			}
			else
			{
				
				// device must not have been present
				throw new OneWireIOException("OneWireContainer28-device not present");
			}
		}
		
		//--------
		//-------- Temperature 'get' Methods
		//--------
		
		/// <summary> Gets the temperature value in Celsius from the <code>state</code>
		/// data retrieved from the <code>readDevice()</code> method.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information for this
		/// <code>OneWireContainer28</code>
		/// 
		/// </param>
		/// <returns> temperature in Celsius from the last
		/// <code>doTemperatureConvert()</code>
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer28</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// 
		/// </summary>
		/// <seealso cref="doTemperatureConvert">
		/// </seealso>
		public virtual double getTemperature(byte[] state)
		{
			
			// Take these three steps:
			// 1)  Make an 11-bit integer number out of MSB and LSB of the first 2 bytes from scratchpad
			// 2)  Divide final number by 16 to retrieve the floating point number.
			// 3)  Afterwards, test for the following temperatures:
			//     0x07D0 = 125.0C
			//     0x0550 = 85.0C
			//     0x0191 = 25.0625C
			//     0x00A2 = 10.125C
			//     0x0008 = 0.5C
			//     0x0000 = 0.0C
			//     0xFFF8 = -0.5C
			//     0xFF5E = -10.125C
			//     0xFE6F = -25.0625C
			//     0xFC90 = -55.0C
			double theTemperature = (double) 0.0;
			int inttemperature = state[1]; // inttemperature is automatically sign extended here.
			
			inttemperature = (inttemperature << 8) | (state[0] & 0xFF); // this converts 2 bytes into integer
			theTemperature = (double) ((double) inttemperature / (double) 16); // converts integer to a double
			
			return (theTemperature);
		}
		
		/// <summary> Gets the specified temperature alarm value in Celsius from the
		/// <code>state</code> data retrieved from the  <code>readDevice()</code>
		/// method.
		/// 
		/// </summary>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <param name="state">    byte array with device state information
		/// 
		/// </param>
		/// <returns> temperature alarm trip values in Celsius for this
		/// <code>OneWireContainer28</code>
		/// 
		/// </returns>
		/// <seealso cref="hasTemperatureAlarms">
		/// </seealso>
		/// <seealso cref="setTemperatureAlarm">
		/// </seealso>
		public virtual double getTemperatureAlarm(int alarmType, byte[] state)
		{
			return (double) state[alarmType == com.dalsemi.onewire.container.TemperatureContainer_Fields.ALARM_LOW?3:2];
		}
		
		/// <summary> Gets the current temperature resolution in Celsius from the
		/// <code>state</code> data retrieved from the <code>readDevice()</code>
		/// method.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// 
		/// </param>
		/// <returns> temperature resolution in Celsius for this
		/// <code>OneWireContainer28</code>
		/// 
		/// </returns>
		/// <seealso cref="RESOLUTION_9_BIT">
		/// </seealso>
		/// <seealso cref="RESOLUTION_10_BIT">
		/// </seealso>
		/// <seealso cref="RESOLUTION_11_BIT">
		/// </seealso>
		/// <seealso cref="RESOLUTION_12_BIT">
		/// </seealso>
		/// <seealso cref="hasSelectableTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolutions">
		/// </seealso>
		/// <seealso cref="setTemperatureResolution">
		/// </seealso>
		public virtual double getTemperatureResolution(byte[] state)
		{
			double tempres = (double) 0.0;
			
			// calculate temperature resolution according to configuration byte
			switch (state[4])
			{
				
				
				case RESOLUTION_9_BIT: 
					tempres = (double) 0.5;
					break;
				
				case RESOLUTION_10_BIT: 
					tempres = (double) 0.25;
					break;
				
				case RESOLUTION_11_BIT: 
					tempres = (double) 0.125;
					break;
				
				case RESOLUTION_12_BIT: 
					tempres = (double) 0.0625;
					break;
				
				default: 
					tempres = (double) 0.0;
					break;
				
			} // switch
			
			return tempres;
		}
		
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
		/// <seealso cref="hasTemperatureAlarms">
		/// </seealso>
		/// <seealso cref="getTemperatureAlarm">
		/// </seealso>
		public virtual void  setTemperatureAlarm(int alarmType, double alarmValue, byte[] state)
		{
			if ((alarmType != com.dalsemi.onewire.container.TemperatureContainer_Fields.ALARM_LOW) && (alarmType != com.dalsemi.onewire.container.TemperatureContainer_Fields.ALARM_HIGH))
				throw new System.ArgumentException("Invalid alarm type.");
			
			if (alarmValue > 125.0 || alarmValue < - 55.0)
				throw new System.ArgumentException("Value for alarm not in accepted range.  Must be -55 C <-> +125 C.");
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			state[(alarmType == com.dalsemi.onewire.container.TemperatureContainer_Fields.ALARM_LOW)?3:2] = (byte) alarmValue;
		}
		
		/// <summary> Sets the current temperature resolution in Celsius in the provided
		/// <code>state</code> data.   Use the method <code>writeDevice()</code>
		/// with this data to finalize the change to the device.
		/// 
		/// </summary>
		/// <param name="resolution">temperature resolution in Celsius. Valid values are
		/// <code>RESOLUTION_9_BIT</code>,
		/// <code>RESOLUTION_10_BIT</code>,
		/// <code>RESOLUTION_11_BIT</code> and
		/// <code>RESOLUTION_12_BIT</code>.
		/// </param>
		/// <param name="state">     byte array with device state information
		/// 
		/// </param>
		/// <seealso cref="RESOLUTION_9_BIT">
		/// </seealso>
		/// <seealso cref="RESOLUTION_10_BIT">
		/// </seealso>
		/// <seealso cref="RESOLUTION_11_BIT">
		/// </seealso>
		/// <seealso cref="RESOLUTION_12_BIT">
		/// </seealso>
		/// <seealso cref="hasSelectableTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolutions">
		/// </seealso>
		public virtual void  setTemperatureResolution(double resolution, byte[] state)
		{
			byte configbyte = RESOLUTION_12_BIT;
			
			lock (this)
			{
				
				// calculate configbyte from given resolution
				if (resolution == 0.5)
					configbyte = RESOLUTION_9_BIT;
				
				if (resolution == 0.25)
					configbyte = RESOLUTION_10_BIT;
				
				if (resolution == 0.125)
					configbyte = RESOLUTION_11_BIT;
				
				if (resolution == 0.0625)
					configbyte = RESOLUTION_12_BIT;
				
				state[4] = configbyte;
			}
		}
		
		/// <summary> Retrieves this <code>OneWireContainer28</code> state information.
		/// The state information is returned as a byte array.  Pass this byte
		/// array to the '<code>get</code>' and '<code>set</code>' methods.
		/// If the device state needs to be changed, then call the
		/// <code>writeDevice()</code> to finalize the changes.
		/// 
		/// </summary>
		/// <returns> <code>OneWireContainer28</code> state information.
		/// Device state looks like this:
		/// <pre>
		/// 0 : temperature LSB
		/// 1 : temperature MSB
		/// 2 : trip high
		/// 3 : trip low
		/// 4 : configuration register (for resolution)
		/// 5 : reserved
		/// 6 : reserved
		/// 7 : reserved
		/// 8 : an 8 bit CRC of the previous 8 bytes
		/// </pre>
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer28</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// 
		/// </summary>
		/// <seealso cref="writeDevice">
		/// </seealso>
		public virtual byte[] readDevice()
		{
			
			byte[] data;
			
			data = recallE2();
			
			return data;
		}
		
		/// <summary> Writes to this <code>OneWireContainer28</code> <code>state</code>
		/// information that have been changed by '<code>set</code>' methods.
		/// Only the state registers that changed are updated.  This is done
		/// by referencing a field information appended to the state data.
		/// 
		/// </summary>
		/// <param name="state">     byte array with device state information
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer28</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// 
		/// </summary>
		/// <seealso cref="readDevice">
		/// </seealso>
		public virtual void  writeDevice(byte[] state)
		{
			byte[] temp = new byte[3];
			
			temp[0] = state[2];
			temp[1] = state[3];
			temp[2] = state[4];
			
			// Write it to the Scratchpad.
			writeScratchpad(temp);
			
			// Place in memory.
			copyScratchpad();
		}
		
		//--------
		//-------- Custom Methods for this iButton Type
		//--------
		//-------------------------------------------------------------------------
		
		/// <summary> Reads the Scratchpad of the DS18B20.
		/// 
		/// </summary>
		/// <returns> 9-byte buffer representing the scratchpad
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer28</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		public virtual byte[] readScratchpad()
		{
			byte[] result_block;
			
			// select the device
			if (adapter.select(address))
			{
				
				// create a block to send that reads the scratchpad
				byte[] send_block = new byte[10];
				
				// read scratchpad command
				send_block[0] = (byte) READ_SCRATCHPAD_COMMAND;
				
				// now add the read bytes for data bytes and crc8
				for (int i = 1; i < 10; i++)
					send_block[i] = (byte) SupportClass.Identity(0xFF);
				
				// send the block
				adapter.dataBlock(send_block, 0, send_block.Length);
				
				// now, send_block contains the 9-byte Scratchpad plus READ_SCRATCHPAD_COMMAND byte
				// convert the block to a 9-byte array representing Scratchpad (get rid of first byte)
				result_block = new byte[9];
				
				for (int i = 0; i < 9; i++)
				{
					result_block[i] = send_block[i + 1];
				}
				
				// see if CRC8 is correct
				if (CRC8.compute(send_block, 1, 9) == 0)
					return (result_block);
				else
					throw new OneWireIOException("OneWireContainer28-Error reading CRC8 from device.");
			}
			
			// device must not have been present
			throw new OneWireIOException("OneWireContainer28-Device not found on 1-Wire Network");
		}
		
		//-------------------------------------------------------------------------
		
		/// <summary> Writes to the Scratchpad of the DS18B20.
		/// 
		/// </summary>
		/// <param name="data">data to be written to the scratchpad.  First
		/// byte of data must be the temperature High Trip Point, the
		/// second byte must be the temperature Low Trip Point, and
		/// the third must be the Resolution (configuration register).
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer28</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		/// <throws>  IllegalArgumentException when data is of invalid length </throws>
		public virtual void  writeScratchpad(byte[] data)
		{
			
			// setup buffer to write to scratchpad
			byte[] writeBuffer = new byte[4];
			
			writeBuffer[0] = WRITE_SCRATCHPAD_COMMAND;
			writeBuffer[1] = data[0];
			writeBuffer[2] = data[1];
			writeBuffer[3] = data[2];
			
			// send command block to device
			if (adapter.select(address))
			{
				adapter.dataBlock(writeBuffer, 0, writeBuffer.Length);
			}
			else
			{
				
				// device must not have been present
				throw new OneWireIOException("OneWireContainer28-Device not found on 1-Wire Network");
			}
			
			// double check by reading scratchpad
			byte[] readBuffer;
			
			readBuffer = readScratchpad();
			
			if ((readBuffer[2] != data[0]) || (readBuffer[3] != data[1]) || (readBuffer[4] != data[2]))
			{
				
				// writing to scratchpad failed
				throw new OneWireIOException("OneWireContainer28-Error writing to scratchpad");
			}
			
			return ;
		}
		
		//-------------------------------------------------------------------------
		
		/// <summary> Copies the Scratchpad to the E-squared memory of the DS18B20.
		/// 
		/// </summary>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer28</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		public virtual void  copyScratchpad()
		{
			
			// first, let's read the scratchpad to compare later.
			byte[] readfirstbuffer;
			
			readfirstbuffer = readScratchpad();
			
			// second, let's copy the scratchpad.
			if (adapter.select(address))
			{
				
				// apply the power delivery
				adapter.PowerDuration = DSPortAdapter.DELIVERY_INFINITE;
				adapter.startPowerDelivery(DSPortAdapter.CONDITION_AFTER_BYTE);
				
				// send the convert temperature command
				adapter.putByte(COPY_SCRATCHPAD_COMMAND);
				
				// sleep for 10 milliseconds to allow copy to take place.
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 10));
				}
				catch (System.Threading.ThreadInterruptedException e)
				{
				}
				
				// Turn power back to normal.
				adapter.setPowerNormal();
			}
			else
			{
				
				// device must not have been present
				throw new OneWireIOException("OneWireContainer28-Device not found on 1-Wire Network");
			}
			
			// third, let's read the scratchpad again with the recallE2 command and compare.
			byte[] readlastbuffer;
			
			readlastbuffer = recallE2();
			
			if ((readfirstbuffer[2] != readlastbuffer[2]) || (readfirstbuffer[3] != readlastbuffer[3]) || (readfirstbuffer[4] != readlastbuffer[4]))
			{
				
				// copying to scratchpad failed
				throw new OneWireIOException("OneWireContainer28-Error copying scratchpad to E2 memory.");
			}
		}
		
		//-------------------------------------------------------------------------
		
		/// <summary> Recalls the DS18B20 temperature trigger values (<code>ALARM_HIGH</code>
		/// and <code>ALARM_LOW</code>) and the configuration register to the
		/// scratchpad and reads the scratchpad.
		/// 
		/// </summary>
		/// <returns> byte array representing data in the device's scratchpad.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer28</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		public virtual byte[] recallE2()
		{
			byte[] ScratchBuff;
			
			// select the device
			if (adapter.select(address))
			{
				
				// send the Recall E-squared memory command
				adapter.putByte(RECALL_E2MEMORY_COMMAND);
				
				// read scratchpad
				ScratchBuff = readScratchpad();
				
				return (ScratchBuff);
			}
			
			// device must not have been present
			throw new OneWireIOException("OneWireContainer28-Device not found on 1-Wire Network");
		}
		
		//-------------------------------------------------------------------------
		
		//-------------------------------------------------------------------------
		
		/// <summary> Converts a temperature reading from Celsius to Fahrenheit.
		/// 
		/// </summary>
		/// <param name="celsiusTemperature">temperature value in Celsius
		/// 
		/// </param>
		/// <returns>  the Fahrenheit conversion of the supplied temperature
		/// 
		/// </returns>
		/// <deprecated> Replace with call to com.dalsemi.onewire.utils.Convert.toFahrenheit()
		/// 
		/// </deprecated>
		/// <seealso cref="com.dalsemi.onewire.utils.Convert.toFahrenheit(double)">
		/// </seealso>
		public virtual float convertToFahrenheit(float celsiusTemperature)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) com.dalsemi.onewire.utils.Convert.toFahrenheit(celsiusTemperature);
		}
	}
}