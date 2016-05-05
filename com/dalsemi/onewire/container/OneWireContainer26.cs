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
using com.dalsemi.onewire.utils;
using com.dalsemi.onewire;
using com.dalsemi.onewire.adapter;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary>  <P>1-Wire&reg; container that encapsulates the functionality of the 1-Wire
	/// family type <B>26</B> (hex), Maxim Integrated Products part number: <B>DS2438,
	/// Smart Battery Monitor</B>.</P>
	/// 
	/// <H2>Features</H2>
	/// <UL>
	/// <LI>direct-to-digital temperature sensor
	/// <LI>A/D converters which measures the battery voltage and current
	/// <LI>integrated current accumulator which keeps a running
	/// total of all current going into and out of the battery
	/// <LI>elapsed time meter
	/// <LI>40 bytes of nonvolatile EEPROM memory for storage of important parameters
	/// <LI>Operating temperature range from -40@htmlonly &#176C @endhtmlonly to
	/// +85@htmlonly &#176C @endhtmlonlyi
	/// </UL>
	/// 
	/// <H2>Note</H2>
	/// <P>
	/// Sometimes the VAD input will report 10.23 V even if nothing is attached.
	/// This value is also the maximum voltage that part can report.
	/// </P>
	/// 
	/// <H3> DataSheet </H3>
	/// <DL>
	/// <DD>http://pdfserv.maxim-ic.com/arpdf/DS2438.pdf (not active yet, Sep-06-2001)
	/// <DD><A HREF="http://www.ibutton.com/weather/humidity.html">http://www.ibutton.com/weather/humidity.html</A>
	/// </DL>
	/// 
	/// </summary>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      COlmstea
	/// 
	/// </author>
	public class OneWireContainer26:OneWireContainer, ADContainer, TemperatureContainer, ClockContainer, HumidityContainer
	{
		/// <summary> Gets an enumeration of memory bank instances that implement one or more
		/// of the following interfaces:
		/// {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
		/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank},
		/// and {@link com.dalsemi.onewire.container.OTPMemoryBank OTPMemoryBank}.
		/// </summary>
		/// <returns> <CODE>Enumeration</CODE> of memory banks
		/// </returns>
		override public System.Collections.IEnumerator MemoryBanks
		{
			get
			{
				System.Collections.ArrayList bank_vector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(8));
				
				// Status
				bank_vector.Add(new MemoryBankSBM(this));
				
				// Temp/Volt/Current
				MemoryBankSBM temp = new MemoryBankSBM(this);
				temp.bankDescription = "Temperature/Voltage/Current";
				temp.generalPurposeMemory = false;
				temp.startPhysicalAddress = 1;
				temp.size = 6;
				temp.readWrite = false;
				temp.readOnly = true;
				temp.nonVolatile = false;
				temp.powerDelivery = false;
				bank_vector.Add(temp);
				
				// Threshold
				temp = new MemoryBankSBM(this);
				temp.bankDescription = "Threshold";
				temp.generalPurposeMemory = false;
				temp.startPhysicalAddress = 7;
				temp.size = 1;
				temp.readWrite = true;
				temp.readOnly = false;
				temp.nonVolatile = true;
				temp.powerDelivery = true;
				bank_vector.Add(temp);
				
				// Elapsed Timer Meter
				temp = new MemoryBankSBM(this);
				temp.bankDescription = "Elapsed Timer Meter";
				temp.generalPurposeMemory = false;
				temp.startPhysicalAddress = 8;
				temp.size = 5;
				temp.readWrite = true;
				temp.readOnly = false;
				temp.nonVolatile = false;
				temp.powerDelivery = true;
				bank_vector.Add(temp);
				
				// Current Offset
				temp = new MemoryBankSBM(this);
				temp.bankDescription = "Current Offset";
				temp.generalPurposeMemory = false;
				temp.startPhysicalAddress = 13;
				temp.size = 2;
				temp.readWrite = true;
				temp.readOnly = false;
				temp.nonVolatile = true;
				temp.powerDelivery = true;
				bank_vector.Add(temp);
				
				// Disconnect / End of Charge
				temp = new MemoryBankSBM(this);
				temp.bankDescription = "Disconnect / End of Charge";
				temp.generalPurposeMemory = false;
				temp.startPhysicalAddress = 16;
				temp.size = 8;
				temp.readWrite = true;
				temp.readOnly = false;
				temp.nonVolatile = false;
				temp.powerDelivery = true;
				bank_vector.Add(temp);
				
				// User Main Memory
				temp = new MemoryBankSBM(this);
				temp.bankDescription = "User Main Memory";
				temp.generalPurposeMemory = true;
				temp.startPhysicalAddress = 24;
				temp.size = 32;
				temp.readWrite = true;
				temp.readOnly = false;
				temp.nonVolatile = true;
				temp.powerDelivery = true;
				bank_vector.Add(temp);
				
				// User Memory / CCA / DCA
				temp = new MemoryBankSBM(this);
				temp.bankDescription = "User Memory / CCA / DCA";
				temp.generalPurposeMemory = false;
				temp.startPhysicalAddress = 56;
				temp.size = 8;
				temp.readWrite = true;
				temp.readOnly = false;
				temp.nonVolatile = true;
				temp.powerDelivery = true;
				bank_vector.Add(temp);
				
				return bank_vector.GetEnumerator();
			}
			
		}
		/// <summary>  Returns the Maxim Integrated Products part number of this 1-Wire device
		/// as a string.
		/// 
		/// </summary>
		/// <returns> representation of this 1-Wire device's name
		/// 
		/// </returns>
		override public System.String Name
		{
			get
			{
				return "DS2438";
			}
			
		}
		/// <summary>  Return the alternate Maxim Integrated Products part number or name.
		/// ie. Smart Battery Monitor
		/// 
		/// </summary>
		/// <returns> representation of the alternate name(s)
		/// </returns>
		override public System.String AlternateNames
		{
			get
			{
				return "Smart Battery Monitor";
			}
			
		}
		/// <summary>  Return a short description of the function of this 1-Wire device type.
		/// 
		/// </summary>
		/// <returns> representation of the functional description
		/// </returns>
		override public System.String Description
		{
			get
			{
				return "1-Wire device that integrates the total current charging or " + "discharging through a battery and stores it in a register. " + "It also returns the temperature (accurate to 2 degrees celcius)," + " as well as the instantaneous current and voltage and also " + "provides 40 bytes of EEPROM storage.";
			}
			
		}
		/// <summary> Directs the container to avoid the calls to doSpeed() in methods that communicate
		/// with the Thermocron. To ensure that all parts can talk to the 1-Wire bus
		/// at their desired speed, each method contains a call
		/// to <code>doSpeed()</code>.  However, this is an expensive operation.
		/// If a user manages the bus speed in an
		/// application,  call this method with <code>doSpeedCheck</code>
		/// as <code>false</code>.  The default behavior is
		/// to call <code>doSpeed()</code>.
		/// 
		/// </summary>
		/// <param name="doSpeedCheck"><code>true</code> for <code>doSpeed()</code> to be called before every
		/// 1-Wire bus access, <code>false</code> to skip this expensive call
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer.doSpeed()">
		/// </seealso>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'setSpeedCheck'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		virtual public bool SpeedCheck
		{
			set
			{
				lock (this)
				{
					doSpeedEnable = value;
				}
			}
			
		}
		/// <summary> Calculate the remaining capacity in mAH as outlined in the data sheet.
		/// 
		/// </summary>
		/// <returns> battery capacity remaining in mAH
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		virtual public double RemainingCapacity
		{
			get
			{
				int ica = ICA;
				
				return (1000 * ica / (2048 * Rsens));
			}
			
		}
		/// <summary> Set the minimum current measurement magnitude for which the ICA/CCA/DCA
		/// are incremented. This is important for applications where the current
		/// may get very small for long periods of time. Small currents can be
		/// inaccurate by a high percentage, which leads to very inaccurate
		/// accumulations.
		/// 
		/// </summary>
		/// <param name="threshold">minimum number of bits a current measurement must have to be accumulated,
		/// Only 0,2,4 and 8 are valid parameters
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error setting the threshold </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		virtual public byte Threshold
		{
			set
			{
				byte thresholdReg;
				byte[] data;
				
				switch (value)
				{
					
					case 0: 
						thresholdReg = 0;
						break;
					
					case 2: 
						thresholdReg = 64;
						break;
					
					case 4:
                       thresholdReg = (byte) SupportClass.Identity(128);
						break;
					
					case 8:
                       thresholdReg = (byte) SupportClass.Identity(192);
						break;
					
					default: 
						throw new System.ArgumentException("OneWireContainer26-Threshold value must be 0,2,4, or 8.");
					
				}
				
				// first save their original IAD settings so we dont change anything
				bool IADvalue = getFlag(IAD_FLAG);
				
				// current measurements must be off to write to the threshold register
				setFlag(IAD_FLAG, false);
				
				// write the threshold register
				data = readPage(0);
				data[7] = thresholdReg;
				
				writePage(0, data, 0);
				
				// set the IAD back to the way the user had it
				setFlag(IAD_FLAG, IADvalue);
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Retrieves the current ICA value in mVHr.
		/// 
		/// </summary>
		/// <returns> value in the ICA register
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		/// <summary> Set the value of the ICA.
		/// 
		/// </summary>
		/// <param name="icaValue"> new ICA value
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error writing data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		virtual public int ICA
		{
			get
			{
				byte[] data = readPage(1);
				
				return (int) (data[4] & 0x000000ff);
			}
			
			set
			{
				byte[] data = readPage(1);
				
				data[4] = (byte) (value & 0x000000ff);
				
				writePage(1, data, 0);
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Retrieves the current CCA value in mVHr. This value is accumulated over
		/// the lifetime of the part (until it is set to 0 or the CA flag is set
		/// to false) and includes only charging current (positive).
		/// 
		/// </summary>
		/// <returns> CCA value
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		/// <summary> Set the value of the CCA.
		/// 
		/// </summary>
		/// <param name="ccaValue">new CCA value
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error writing data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		virtual public int CCA
		{
			get
			{
				byte[] data = readPage(7);
				
				return ((data[5] << 8) & 0x0000ff00) | (data[4] & 0x000000ff);
			}
			
			set
			{
				byte[] data = readPage(7);
				
				data[4] = (byte) (value & 0x00ff);
				data[5] = (byte) (SupportClass.URShift((value & 0xff00), 8));
				
				writePage(7, data, 0);
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Retrieves the value of the DCA in mVHr. This value is accumulated over
		/// the lifetime of the part (until explicitly set to 0 or if the CA flag
		/// is set to false) and includes only discharging current (negative).
		/// 
		/// </summary>
		/// <returns> DCA value
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		/// <summary> Set the value of the DCA.
		/// 
		/// </summary>
		/// <param name="dcaValue">new DCA value
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error writing data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		virtual public int DCA
		{
			get
			{
				byte[] data = readPage(7);
				
				return ((data[7] << 8) & 0x0000ff00) | (data[6] & 0x000000ff);
			}
			
			set
			{
				byte[] data = readPage(7);
				
				data[6] = (byte) (value & 0x00ff);
				data[7] = (byte) (SupportClass.URShift((value & 0xff00), 8));
				
				writePage(7, data, 0);
			}
			
		}
		/// <summary> Query to get the number of channels supported by this A/D.
		/// Channel specific methods will use a channel number specified
		/// by an integer from [0 to (getNumberChannels() - 1)].
		/// 
		/// </summary>
		/// <returns> number of channels
		/// </returns>
		virtual public int NumberADChannels
		{
			get
			{
				return 3; //has VDD, VAD channel  (battery, gen purpose)
				// and it has a Vsense channel for current sensing
			}
			
		}
		/// <summary> Query to get an array of available resolutions in degrees C.
		/// 
		/// </summary>
		/// <returns> available resolutions in degrees C
		/// </returns>
		virtual public double[] TemperatureResolutions
		{
			get
			{
				double[] result = new double[1];
				
				result[0] = 0.03125;
				
				return result;
			}
			
		}
		/// <summary> Query to get the high/low resolution in degrees C.
		/// 
		/// </summary>
		/// <returns> high/low resolution resolution in degrees C
		/// 
		/// </returns>
		/// <throws>  OneWireException Device does not have temperature alarms </throws>
		virtual public double TemperatureAlarmResolution
		{
			get
			{
				throw new OneWireException("This device does not have temperature alarms");
			}
			
		}
		/// <summary> Query to get the maximum temperature in degrees C.
		/// 
		/// </summary>
		/// <returns> maximum temperature in degrees C
		/// </returns>
		virtual public double MaxTemperature
		{
			get
			{
				return 125.0;
			}
			
		}
		/// <summary> Query to get the minimum temperature in degrees C.
		/// 
		/// </summary>
		/// <returns> minimum temperature in degrees C
		/// </returns>
		virtual public double MinTemperature
		{
			get
			{
				return - 55.0;
			}
			
		}
		/// <summary> Query to get the clock resolution in milliseconds
		/// 
		/// </summary>
		/// <returns> clock resolution in milliseconds.
		/// </returns>
		virtual public long ClockResolution
		{
			get
			{
				return 1000;
			}
			
		}
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
		virtual public bool Relative
		{
			get
			{
				return true;
			}
			
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
		virtual public double[] HumidityResolutions
		{
			get
			{
				double[] result = new double[1];
				
				result[0] = 0.1;
				
				return result;
			}
			
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
		virtual public double HumidityAlarmResolution
		{
			get
			{
				throw new OneWireException("This device does not have a humidity alarm!");
			}
			
		}
		
		/// <summary> Memory commands.</summary>
		private static byte READ_SCRATCHPAD_COMMAND = (byte) SupportClass.Identity(0xBE);
		private static byte RECALL_MEMORY_COMMAND = (byte) SupportClass.Identity(0xB8);
		private static byte COPY_SCRATCHPAD_COMMAND = (byte) 0x48;
		private static byte WRITE_SCRATCHPAD_COMMAND = (byte) 0x4E;
		private static byte CONVERT_TEMP_COMMAND = (byte) 0x44;
		private static byte CONVERT_VOLTAGE_COMMAND = (byte) SupportClass.Identity(0xB4);
		
		/// <summary> Channel selector for the VDD input.  Meant to be used with
		/// a battery.
		/// </summary>
		public const int CHANNEL_VDD = 0x00;
		
		/// <summary> Channel selector for the VAD input.  This is the general purpose
		/// A-D input.
		/// </summary>
		public const int CHANNEL_VAD = 0x01;
		
		/// <summary> Channel selectro the the IAD input.  Measures voltage across
		/// a resistor, Rsense, for calculating current.
		/// </summary>
		public const int CHANNEL_VSENSE = 0x02;
		
		/// <summary> Flag to set/check the Current A/D Control bit with setFlag/getFlag. When
		/// this bit is true, the current A/D and the ICA are enabled and
		/// current measurements will be taken at the rate of 36.41 Hz.
		/// </summary>
		public const byte IAD_FLAG = (byte) (0x01);
		
		/// <summary> Flag to set/check the Current Accumulator bit with setFlag/getFlag. When
		/// this bit is true, both the total discharging and charging current are
		/// integrated into seperate registers and can be used for determining
		/// full/empty levels.  When this bit is zero the memory (page 7) can be used
		/// as user memory.
		/// </summary>
		public const byte CA_FLAG = (byte) (0x02);
		
		/// <summary> Flag to set/check the Current Accumulator Shadow Selector bit with
		/// setFlag/getFlag.  When this bit is true the CCA/DCA registers used to
		/// add up charging/discharging current are shadowed to EEPROM to protect
		/// against loss of data if the battery pack becomes discharged.
		/// </summary>
		public const byte EE_FLAG = (byte) (0x04);
		
		/// <summary> Flag to set/check the voltage A/D Input Select Bit with setFlag/getFlag
		/// When this bit is true the battery input is (VDD) is selected as input for
		/// the voltage A/D input. When false the general purpose A/D input (VAD) is
		/// selected as the voltage A/D input.
		/// </summary>
		public const byte AD_FLAG = (byte) (0x08);
		
		/// <summary> Flag to check whether or not a temperature conversion is in progress
		/// using getFlag().
		/// </summary>
		public const byte TB_FLAG = (byte) (0x10);
		
		/// <summary> Flag to check whether or not an operation is being performed on the
		/// nonvolatile memory using getFlag.
		/// </summary>
		public const byte NVB_FLAG = (byte) (0x20);
		
		/// <summary> Flag to check whether or not the A/D converter is busy using getFlag().</summary>
		public const byte ADB_FLAG = (byte) (0x40);
		
		/// <summary> Holds the value of the sensor resistance.</summary>
		private double Rsens = .05;
		
		/// <summary> Flag to indicate need to check speed</summary>
		private bool doSpeedEnable = true;
		
		//--------
		//-------- Constructors
		//--------
		
		/// <summary> Default constructor</summary>
		public OneWireContainer26():base()
		{
		}
		
		/// <summary> Create a container with a provided adapter object
		/// and the address of the 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public OneWireContainer26(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
		}
		
		/// <summary> Create a container with a provided adapter object
		/// and the address of the 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public OneWireContainer26(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
		}
		
		/// <summary> Create a container with a provided adapter object
		/// and the address of the 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public OneWireContainer26(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
		}
		
		/// <summary> Set the value of the sense resistor used to determine
		/// battery current.  This value is used in the <CODE>getCurrent()</CODE> calculation.
		/// See the DS2438 datasheet for more information on sensing battery
		/// current.
		/// 
		/// </summary>
		/// <param name="resistance">Value of the sense resistor in Ohms.
		/// </param>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'setSenseResistor'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual void  setSenseResistor(double resistance)
		{
			lock (this)
			{
				Rsens = resistance;
			}
		}
		
		/// <summary> Get the value used for the sense resistor in the <CODE>getCurrent()</CODE>
		/// calculations.
		/// 
		/// </summary>
		/// <returns> currently stored value of the sense resistor in Ohms
		/// </returns>
		public virtual double getSenseResistor()
		{
			return Rsens;
		}
		
		/// <summary> Reads the specified 8 byte page and returns the data in an array.
		/// 
		/// </summary>
		/// <param name="page">the page number to read
		/// 
		/// </param>
		/// <returns>  eight byte array that make up the page
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		public virtual byte[] readPage(int page)
		{
			byte[] buffer = new byte[11];
			byte[] result = new byte[8];
			int crc8; // this device uses a crc 8
			
			/* check validity of parameter */
			if ((page < 0) || (page > 7))
				throw new System.ArgumentException("OneWireContainer26-Page " + page + " is an invalid page.");
			
			/* perform the read/verification */
			if (doSpeedEnable)
				doSpeed();
			
			if (adapter.select(address))
			{
				
				/* recall memory to the scratchpad */
				buffer[0] = RECALL_MEMORY_COMMAND;
				buffer[1] = (byte) page;
				
				adapter.dataBlock(buffer, 0, 2);
				
				/* perform the read scratchpad */
				adapter.reset();
				adapter.select(address);
				
				buffer[0] = READ_SCRATCHPAD_COMMAND;
				buffer[1] = (byte) page;
				
				for (int i = 2; i < 11; i++)
					buffer[i] = (byte) SupportClass.Identity(0x0ff);
				
				adapter.dataBlock(buffer, 0, 11);
				
				/* do the crc check */
				crc8 = CRC8.compute(buffer, 2, 9);
				
				if (crc8 != 0x0)
					throw new OneWireIOException("OneWireContainer26-Bad CRC during read." + crc8);
				
				// copy the data into the result
				Array.Copy(buffer, 2, result, 0, 8);
			}
			else
				throw new OneWireException("OneWireContainer26-device not found.");
			
			return result;
		}
		
		/// <summary> Writes a page of memory to this device. Pages 3-6 are always
		/// available for user storage and page 7 is available if the CA bit is set
		/// to 0 (false) with <CODE>setFlag()</CODE>.
		/// 
		/// </summary>
		/// <param name="page">   the page number
		/// </param>
		/// <param name="source"> data to be written to page
		/// </param>
		/// <param name="offset"> offset with page to begin writting
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		public virtual void  writePage(int page, byte[] source, int offset)
		{
			byte[] buffer = new byte[10];
			
			/* check parameter validity */
			if ((page < 0) || (page > 7))
				throw new System.ArgumentException("OneWireContainer26-Page " + page + " is an invalid page.");
			
			if (source.Length < 8)
				throw new System.ArgumentException("OneWireContainer26-Invalid data page passed to writePage.");
			
			if (doSpeedEnable)
				doSpeed();
			
			if (adapter.select(address))
			{
				
				// write the page to the scratchpad first
				buffer[0] = WRITE_SCRATCHPAD_COMMAND;
				buffer[1] = (byte) page;
				
				Array.Copy(source, offset, buffer, 2, 8);
				adapter.dataBlock(buffer, 0, 10);
				
				// now copy that part of the scratchpad to memory
				adapter.reset();
				adapter.select(address);
				
				buffer[0] = COPY_SCRATCHPAD_COMMAND;
				buffer[1] = (byte) page;
				
				adapter.dataBlock(buffer, 0, 2);
			}
			else
				throw new OneWireException("OneWireContainer26-Device not found.");
		}
		
		/// <summary> Checks the specified flag in the status/configuration register
		/// and returns its status as a boolean.
		/// 
		/// </summary>
		/// <param name="flagToGet">flag bitmask.
		/// Acceptable parameters: IAD_FLAG, CA_FLAG, EE_FLAG, AD_FLAG, TB_FLAG,
		/// NVB_FLAG, ADB_FLAG
		/// (may be ORed with | to check the status of more than one).
		/// 
		/// </param>
		/// <returns> state of flag
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		public virtual bool getFlag(byte flagToGet)
		{
			byte[] data = readPage(0);
			
			if ((data[0] & flagToGet) != 0)
				return true;
			
			return false;
		}
		
		/// <summary> Set one of the flags in the STATUS/CONFIGURATION register.
		/// 
		/// </summary>
		/// <param name="bitmask">of the flag to set
		/// Acceptable parameters: IAD_FLAG, CA_FLAG, EE_FLAG, AD_FLAG, TB_FLAG,
		/// NVB_FLAG, ADB_FLAG.
		/// 
		/// </param>
		/// <param name="flagValue">value to set flag to
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error writting data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		public virtual void  setFlag(byte flagToSet, bool flagValue)
		{
			byte[] data = readPage(0);
			
			if (flagValue)
				data[0] = (byte) (data[0] | flagToSet);
			else
				data[0] = (byte) (data[0] & ~ (flagToSet));
			
			writePage(0, data, 0);
		}
		
		/// <summary> Get the instantaneous current. The IAD flag must be true!!
		/// Remember to set the Sense resistor value using
		/// <CODE>setSenseResitor(double)</CODE>.
		/// 
		/// 
		/// </summary>
		/// <param name="state">current state of device
		/// </param>
		/// <returns> current value in Amperes
		/// </returns>
		public virtual double getCurrent(byte[] state)
		{
			short rawCurrent = (short) ((state[6] << 8) | (state[5] & 0x0ff));
			
			return rawCurrent / (4096.0 * Rsens);
		}
		
		/// <summary> Determines if the battery is charging and returns a boolean.
		/// 
		/// 
		/// </summary>
		/// <param name="state">current state of device
		/// 
		/// </param>
		/// <returns> true if battery is changing, false if battery is idle or discharging
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		public virtual bool isCharging(byte[] state)
		{
			
			// positive current (if the thing is hooked up right) is charging
			if (getCurrent(state) > 0)
				return true;
			
			return false;
		}
		
		/// <summary> Calibrate the current ADC. Although the part is shipped calibrated,
		/// calibrations should be done whenever possible for best results.
		/// NOTE: You MUST force zero current through Rsens (the sensor resistor)
		/// while calibrating.
		/// 
		/// </summary>
		/// <throws>  OneWireIOException Error calibrating </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		public virtual void  calibrateCurrentADC()
		{
			byte[] data;
			byte currentLSB, currentMSB;
			
			// grab the current IAD settings so that we dont change anything
			bool IADvalue = getFlag(IAD_FLAG);
			
			// the IAD bit must be set to "0" to write to the Offset Register
			setFlag(IAD_FLAG, false);
			
			// write all zeroes to the offset register
			data = readPage(1);
			data[5] = data[6] = 0;
			
			writePage(1, data, 0);
			
			// enable current measurements once again
			setFlag(IAD_FLAG, true);
			
			// read the Current Register value
			data = readPage(0);
			currentLSB = data[5];
			currentMSB = data[6];
			
			// disable current measurements so that we can write to the offset reg
			setFlag(IAD_FLAG, false);
			
			// change the sign of the current register value and store it as the offset
			data = readPage(1);
			data[5] = (byte) (~ (currentLSB) + 1);
			data[6] = (byte) (~ (currentMSB));
			
			writePage(1, data, 0);
			
			// eset the IAD settings back to normal
			setFlag(IAD_FLAG, IADvalue);
		}
		
		/// <summary> This method extracts the Clock Value in milliseconds from the
		/// state data retrieved from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <returns> time in milliseconds that have
		/// occured since 1970
		/// </returns>
		public virtual long getDisconnectTime(byte[] state)
		{
			return getTime(state, 16) * 1000;
		}
		
		/// <summary> This method extracts the Clock Value in milliseconds from the
		/// state data retrieved from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <returns> time in milliseconds that have
		/// occured since 1970
		/// </returns>
		public virtual long getEndOfChargeTime(byte[] state)
		{
			return getTime(state, 20) * 1000;
		}
		
		//actually could be called byteArrayToLong, only used in time functions
		private long getTime(byte[] state, int start)
		{
			long time = (state[start] & 0x0ff) | ((state[start + 1] & 0x0ff) << 8) | ((state[start + 2] & 0x0ff) << 16) | ((state[start + 3] & 0x0ff) << 24);
			
			return time & unchecked((int) 0x0ffffffff);
		}
		
		//////////////////////////////////////////////////////////////////////////////
		//
		//      INTERFACE METHODS!!!!!!!!
		//
		//////////////////////////////////////////////////////////////////////////////
		
		/// <summary> Query to see if this A/D measuring device has high/low
		/// alarms.
		/// 
		/// </summary>
		/// <returns> true if has high/low trips
		/// </returns>
		public virtual bool hasADAlarms()
		{
			return false;
		}
		
		/// <summary> Query to get an array of available ranges for the specified
		/// A/D channel.
		/// 
		/// </summary>
		/// <param name="channel"> channel in the range
		/// [0 to (getNumberChannels() - 1)]
		/// 
		/// </param>
		/// <returns> available ranges
		/// </returns>
		public virtual double[] getADRanges(int channel)
		{
			double[] result = new double[1];
			
			if (channel == CHANNEL_VSENSE)
				result[0] = .250;
			else
				result[0] = 10.23;
			
			/* for VAD, not entirely true--this should be
			2 * VDD.  If you hook up VDD to the
			one-wire in series with a diode and then
			hang a .1 microF capacitor off the line to ground,
			you can get about 9.5 for the high end accurately
			----------------------------------
			|             *****************  |
			One-Wire------- DIODE-------*VDD     ONEWIRE*---
			|   *               *
			|   *        GROUND *---
			C   *               *  |
			|   *    2438       *  |
			gnd  *               *  |
			|   *****************  |
			|----------------------|
			
			*/
			return result;
		}
		
		/// <summary> Query to get an array of available resolutions based
		/// on the specified range on the specified A/D channel.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// [0 to (getNumberChannels() - 1)]
		/// </param>
		/// <param name="range">A/D range
		/// 
		/// </param>
		/// <returns> available resolutions
		/// </returns>
		public virtual double[] getADResolutions(int channel, double range)
		{
			double[] result = new double[1];
			
			if (channel == CHANNEL_VSENSE)
				result[0] = 0.2441;
			else
				result[0] = 0.01; //10 mV
			
			return result;
		}
		
		/// <summary> Query to see if this A/D supports doing multiple voltage
		/// conversions at the same time.
		/// 
		/// </summary>
		/// <returns> true if device can do multi-channel voltage reads
		/// </returns>
		public virtual bool canADMultiChannelRead()
		{
			return false;
		}
		
		//--------
		//-------- A/D IO Methods
		//--------
		
		/// <summary> This method is used to perform voltage conversion on all specified
		/// channels.  The method 'getVoltage()' can be used to read the result
		/// of the conversion.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE> [0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="state"> current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error writing data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual void  doADConvert(int channel, byte[] state)
		{
			if (channel == CHANNEL_VSENSE)
			{
				if ((state[0] & IAD_FLAG) == 0)
				{
					// enable the current sense channel
					setFlag(IAD_FLAG, true);
					state[0] |= IAD_FLAG;
					try
					{
						// updates once every 27.6 milliseconds
						//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 30));
					}
					catch (System.Threading.ThreadInterruptedException e)
					{
					}
				}
				
				byte[] data = readPage(0);
				// update the state
				Array.Copy(data, 5, state, 5, 2);
			}
			else
			{
				setFlag(AD_FLAG, channel == CHANNEL_VDD);
				
				// first perform the conversion
				if (doSpeedEnable)
					doSpeed();
				
				if (adapter.select(address))
				{
					adapter.putByte(CONVERT_VOLTAGE_COMMAND);
					
					try
					{
						//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 4));
					}
					catch (System.Threading.ThreadInterruptedException e)
					{
					}
					
					byte[] data = readPage(0);
					
					//let's update state with this info
					Array.Copy(data, 0, state, 0, 8);
					
					// save off the voltage in our state's holdindg area
					state[24 + channel * 2] = data[4];
					state[24 + channel * 2 + 1] = data[3];
				}
				else
					throw new OneWireException("OneWireContainer26-Device not found.");
			}
		}
		
		/// <summary> This method is used to perform voltage conversion on all specified
		/// channels.  The method <CODE>getVoltage()</CODE> can be used to read the result
		/// of the conversion. This A/D must support multi-channel read
		/// <CODE>canMultiChannelRead()</CODE> if there are more then 1 channel is specified.
		/// 
		/// </summary>
		/// <param name="doConvert"> channels
		/// to perform conversion on
		/// </param>
		/// <param name="state"> current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error writing data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual void  doADConvert(bool[] doConvert, byte[] state)
		{
			throw new OneWireException("This device cannot do multi-channel reads");
		}
		
		/// <summary> This method is used to read the voltage values.  Must
		/// be used after a <CODE>doADConvert()</CODE> method call.  Also must
		/// include the last valid state from the <CODE>readDevice()</CODE> method
		/// and this A/D must support multi-channel read <CODE>canMultiChannelRead()</CODE>
		/// if there are more then 1 channel.
		/// 
		/// </summary>
		/// <param name="state">current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> voltage values for all channels
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual double[] getADVoltage(byte[] state)
		{
			throw new OneWireException("This device cannot do multi-channel reads");
		}
		
		/// <summary> This method is used to read a channels voltage value.  Must
		/// be used after a <CODE>doADConvert()</CODE> method call.  Also must
		/// include the last valid state from the <CODE>readDevice()</CODE> method.
		/// Note, if more then one channel is to be read then it is more
		/// efficient to use the <CODE>getVoltage()</CODE> method that returns all
		/// channel values.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="state">current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns>   voltage value for the specified
		/// channel
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual double getADVoltage(int channel, byte[] state)
		{
			double result = 0;
			
			if (channel == CHANNEL_VSENSE)
				result = ((state[6] << 8) | (state[5] & 0x0ff)) / 4096d;
			else
				result = (((state[24 + channel * 2] << 8) & 0x00300) | (state[24 + channel * 2 + 1] & 0x0ff)) / 100.0d;
			
			return result;
		}
		
		//--------
		//-------- A/D 'get' Methods
		//--------
		
		/// <summary> This method is used to extract the alarm voltage value of the
		/// specified channel from the provided state buffer.  The
		/// state buffer is retrieved from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="alarmType">desired alarm, <CODE>ALARM_HIGH (1)
		/// or ALARM_LOW (0)</CODE>
		/// </param>
		/// <param name="state">current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> alarm_value in volts
		/// 
		/// </returns>
		/// <throws>  OneWireException Device does not support A/D alarms </throws>
		public virtual double getADAlarm(int channel, int alarmType, byte[] state)
		{
			throw new OneWireException("This device does not have A/D alarms");
		}
		
		/// <summary> This method is used to extract the alarm enable value of the
		/// specified channel from the provided state buffer.  The state
		/// buffer is retrieved from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="alarmType">desired alarm, <CODE>ALARM_HIGH (1)
		/// or ALARM_LOW (0)</CODE>
		/// </param>
		/// <param name="state">current state of the state
		/// returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> true if specified alarm is enabled
		/// 
		/// </returns>
		/// <throws>  OneWireException Device does not support A/D alarms </throws>
		public virtual bool getADAlarmEnable(int channel, int alarmType, byte[] state)
		{
			throw new OneWireException("This device does not have A/D alarms");
		}
		
		/// <summary> This method is used to check the alarm event value of the
		/// specified channel from the provided state buffer.  The
		/// state buffer is retrieved from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="alarmType">desired alarm, <CODE>ALARM_HIGH (1)
		/// or ALARM_LOW (0)</CODE>
		/// </param>
		/// <param name="state">current state of the state
		/// returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> true if specified alarm occurred
		/// 
		/// </returns>
		/// <throws>  OneWireException Device does not support A/D alarms </throws>
		public virtual bool hasADAlarmed(int channel, int alarmType, byte[] state)
		{
			throw new OneWireException("This device does not have A/D alarms");
		}
		
		/// <summary> This method is used to extract the conversion resolution of the
		/// specified channel from the provided state buffer expressed in
		/// volts.  The state is retrieved from the
		/// <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="state">current state of the state
		/// returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> resolution of channel in volts
		/// </returns>
		public virtual double getADResolution(int channel, byte[] state)
		{
			
			//this is easy, its always 0.01 V = 10 mV
			return 0.01;
		}
		
		/// <summary> This method is used to extract the input voltage range of the
		/// specified channel from the provided state buffer.  The state
		/// buffer is retrieved from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="state">current state of the state
		/// returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> input voltage range
		/// </returns>
		public virtual double getADRange(int channel, byte[] state)
		{
			if (channel == CHANNEL_VSENSE)
				return .250;
			else
				return 10.23;
		}
		
		//--------
		//-------- A/D 'set' Methods
		//--------
		
		/// <summary> This method is used to set the alarm voltage value of the
		/// specified channel in the provided state buffer.  The
		/// state buffer is retrieved from the <CODE>readDevice()</CODE> method.
		/// The method <CODE>writeDevice()</CODE> must be called to finalize these
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <CODE>writeDevice()</CODE>.
		/// 
		/// </summary>
		/// <param name="channel"> channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="alarmType">desired alarm, <CODE>ALARM_HIGH (1)
		/// or ALARM_LOW (0)</CODE>
		/// </param>
		/// <param name="alarm"> alarm value (will be reduced to 8 bit resolution)
		/// </param>
		/// <param name="state"> current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  OneWireException Device does not support A/D alarms </throws>
		public virtual void  setADAlarm(int channel, int alarmType, double alarm, byte[] state)
		{
			throw new OneWireException("This device does not have A/D alarms");
		}
		
		/// <summary> This method is used to set the alarm enable value of the
		/// specified channel in the provided state buffer.  The
		/// state buffer is retrieved from the <CODE>readDevice()</CODE> method.
		/// The method <CODE>writeDevice()</CODE> must be called to finalize these
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <CODE>writeDevice()</CODE>.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="alarmType">desired alarm, <CODE>ALARM_HIGH (1)
		/// or ALARM_LOW (0)</CODE>
		/// </param>
		/// <param name="alarmEnable">alarm enable value
		/// </param>
		/// <param name="state">current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  OneWireException Device does not support A/D alarms </throws>
		public virtual void  setADAlarmEnable(int channel, int alarmType, bool alarmEnable, byte[] state)
		{
			throw new OneWireException("This device does not have AD alarms");
		}
		
		/// <summary> This method is used to set the conversion resolution value for the
		/// specified channel in the provided state buffer.  The
		/// state buffer is retrieved from the <CODE>readDevice()</CODE> method.
		/// The method <CODE>writeDevice()</CODE> must be called to finalize these
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <CODE>writeDevice()</CODE>.
		/// 
		/// </summary>
		/// <param name="channel"> channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="resolution">resolution to use in volts
		/// </param>
		/// <param name="state">current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// </param>
		public virtual void  setADResolution(int channel, double resolution, byte[] state)
		{
			
			//but you can't select the resolution for this part!!!!
			//just make it an airball
		}
		
		/// <summary> This method is used to set the input range for the
		/// specified channel in the provided state buffer.  The
		/// state buffer is retrieved from the <CODE>readDevice()</CODE> method.
		/// The method <CODE>writeDevice()</CODE> must be called to finalize these
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <CODE>writeDevice()</CODE>.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="range">maximum volt range, use
		/// <CODE>getRanges()</CODE> method to get available ranges
		/// </param>
		/// <param name="state">current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// </param>
		public virtual void  setADRange(int channel, double range, byte[] state)
		{
			
			//you can't change the ranges here without changing VDD!!!
			//just make this function call an airball
		}
		
		/// <summary> This method retrieves the 1-Wire device sensor state.  This state is
		/// returned as a byte array.  Pass this byte array to the static query
		/// and set methods.  If the device state needs to be changed then call
		/// the <CODE>writeDevice()</CODE> to finalize the one or more change.
		/// 
		/// </summary>
		/// <returns> 1-Wire device's state
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual byte[] readDevice()
		{
			
			//should return the first three pages
			//and then 6 extra bytes, 2 for channel 1 voltage and
			//2 for channel 2 voltage
			byte[] state = new byte[28];
			
			for (int i = 0; i < 3; i++)
			{
				byte[] pg = readPage(i);
				
				Array.Copy(pg, 0, state, i * 8, 8);
			}
			
			//the last four bytes are used this way...
			//the current voltage reading is kept in page 0,
			//but if a new voltage reading is asked for we move it to the
			//end so it can be there in case it is asked for again,
			//so we kind of weasel around this whole ADcontainer thing
			
			/* here's a little map
			byte[24] VDD high byte
			byte[25] VDD low byte
			byte[26] VAD high byte
			byte[27] VAD low byte
			*/
			return state;
		}
		
		/// <summary> This method write the 1-Wire device sensor state that
		/// have been changed by the 'set' methods.  It knows which registers have
		/// changed by looking at the bitmap fields appended to the state
		/// data.
		/// 
		/// </summary>
		/// <param name="state">device's state
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error writting data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual void  writeDevice(byte[] state)
		{
			writePage(0, state, 0);
			writePage(1, state, 8);
		}
		
		//--------
		//-------- Temperature Feature methods
		//--------
		
		/// <summary> Query to see if this temperature measuring device has high/low
		/// trip alarms.
		/// 
		/// </summary>
		/// <returns> true if has high/low trip alarms
		/// </returns>
		public virtual bool hasTemperatureAlarms()
		{
			return false;
		}
		
		/// <summary> Query to see if this device has selectable resolution.
		/// 
		/// </summary>
		/// <returns> true if device has selectable resolution
		/// </returns>
		public virtual bool hasSelectableTemperatureResolution()
		{
			return false;
		}
		
		//--------
		//-------- Temperature I/O Methods
		//--------
		
		/// <summary> Perform an temperature conversion.  Use this state information
		/// to calculate the conversion time.
		/// 
		/// </summary>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error writting data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual void  doTemperatureConvert(byte[] state)
		{
			byte[] data; // hold page
			
			if (doSpeedEnable)
				doSpeed();
			
			if (adapter.select(address))
			{
				
				// perform the temperature conversion
				adapter.putByte(CONVERT_TEMP_COMMAND);
				
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 10));
				}
				catch (System.Threading.ThreadInterruptedException Ie)
				{
				}
				
				data = readPage(0);
				state[2] = data[2];
				state[1] = data[1];
			}
			else
				throw new OneWireException("OneWireContainer26-Device not found.");
		}
		
		//--------
		//-------- Temperature 'get' Methods
		//--------
		
		/// <summary> This method extracts the Temperature Value in degrees C from the
		/// state data retrieved from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <returns> temperature in degrees C from the last <CODE>doTemperature()</CODE>
		/// </returns>
		public virtual double getTemperature(byte[] state)
		{
			double temp = ((short) ((state[2] << 8) | (state[1] & 0x0ff)) >> 3) * 0.03125;
			
			return temp;
		}
		
		/// <summary> This method extracts the specified Alarm value in degrees C from the
		/// state data retrieved from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="alarmType">alarm trip type <CODE>ALARM_HIGH (1)
		/// or ALARM_LOW (0)</CODE>
		/// </param>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <returns> alarm trip temperature in degrees C
		/// 
		/// </returns>
		/// <throws>  OneWireException Device does not have temperature alarms </throws>
		public virtual double getTemperatureAlarm(int alarmType, byte[] state)
		{
			throw new OneWireException("This device does not have temperature alarms");
		}
		
		/// <summary> This method extracts the current resolution in degrees C from the
		/// state data retrieved from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <returns> temperature resolution in degrees C
		/// </returns>
		public virtual double getTemperatureResolution(byte[] state)
		{
			return 0.03125;
		}
		
		//--------
		//-------- Temperature 'set' Methods
		//--------
		
		/// <summary> This method sets the alarm value in degrees C in the
		/// provided state data.  Use the method <CODE>writeDevice()</CODE> with
		/// this data to finalize the change to the device.
		/// 
		/// </summary>
		/// <param name="alarmType">alarm type <CODE>ALARM_HIGH (1)
		/// or ALARM_LOW (0)</CODE>
		/// </param>
		/// <param name="alarmValue">trip value in degrees C
		/// </param>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <throws>  OneWireException Device does not have temperature alarms </throws>
		public virtual void  setTemperatureAlarm(int alarmType, double alarmValue, byte[] state)
		{
			throw new OneWireException("This device does not have temperature alarms");
		}
		
		/// <summary> This method sets the current resolution in degrees C in the
		/// provided state data.   Use the method <CODE>writeDevice()</CODE> with
		/// this data to finalize the change to the device.
		/// 
		/// </summary>
		/// <param name="resolution">temperature resolution in degrees C
		/// </param>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <throws>  OneWireIOException Error writting data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual void  setTemperatureResolution(double resolution, byte[] state)
		{
			
			//airball, only one resolution!!!
		}
		
		//--------
		//-------- Clock Feature methods
		//--------
		
		/// <summary> Query to see if the clock has an alarm feature.
		/// 
		/// </summary>
		/// <returns> true if real-time-clock has an alarm
		/// </returns>
		public virtual bool hasClockAlarm()
		{
			return false;
		}
		
		/// <summary> Query to see if the clock can be disabled.  See
		/// the methods <CODE>isClockRunning()</CODE> and <CODE>setClockRunEnable()</CODE>.
		/// 
		/// </summary>
		/// <returns> true if the clock can be enabled and
		/// disabled
		/// </returns>
		public virtual bool canDisableClock()
		{
			return false;
		}
		
		//--------
		//-------- Clock 'get' Methods
		//--------
		
		/// <summary> This method extracts the Clock Value in milliseconds from the
		/// state data retrieved from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="state"> device state
		/// 
		/// </param>
		/// <returns> time in milliseconds that have
		/// occured since 1970
		/// </returns>
		public virtual long getClock(byte[] state)
		{
			return getTime(state, 8) * 1000;
		}
		
		/// <summary> This method extracts the Clock Alarm Value from the provided
		/// state data retrieved from the <CODE>readDevice()</CODE>
		/// method.
		/// 
		/// </summary>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <returns> time in milliseconds that have
		/// the clock alarm is set to
		/// 
		/// </returns>
		/// <throws>  OneWireException Device does not have clock alarm </throws>
		//public virtual long getClockAlarm(byte[] state)
        public virtual DateTime getClockAlarm(byte[] state)

		{
			throw new OneWireException("This device does not have a clock alarm!");
		}
		
		/// <summary> This method checks if the Clock Alarm flag has been set
		/// from the state data retrieved from the
		/// <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <returns> true if clock is alarming
		/// </returns>
		public virtual bool isClockAlarming(byte[] state)
		{
			return false;
		}
		
		/// <summary> This method checks if the Clock Alarm is enabled
		/// from the provided state data retrieved from the
		/// <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <returns> true if clock alarm is enabled
		/// </returns>
		public virtual bool isClockAlarmEnabled(byte[] state)
		{
			return false;
		}
		
		/// <summary> This method checks if the device's oscilator is enabled.  The clock
		/// will not increment if the clock is not enabled.
		/// This value is read from the provided state data retrieved from the
		/// <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <returns> true if clock is running
		/// </returns>
		public virtual bool isClockRunning(byte[] state)
		{
			return true;
		}
		
		//--------
		//-------- Clock 'set' Methods
		//--------
		
		/// <summary> This method sets the Clock time in the provided state data
		/// Use the method <CODE>writeDevice()</CODE> with
		/// this data to finalize the change to the device.
		/// 
		/// </summary>
		/// <param name="time">new clock setting in milliseconds
		/// </param>
		/// <param name="state">device state
		/// </param>
		public virtual void  setClock(long time, byte[] state)
		{
			time = time / 1000; //convert to seconds
			state[8] = (byte) time;
			state[9] = (byte) (time >> 8);
			state[10] = (byte) (time >> 16);
			state[11] = (byte) (time >> 24);
		}
		
		/// <summary> This method sets the Clock Alarm in the provided state
		/// data.  Use the method <CODE>writeDevice()</CODE> with
		/// this data to finalize the change to the device.
		/// 
		/// </summary>
		/// <param name="time">new clock setting in mlliseconds
		/// </param>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <throws>  OneWireException Device does not support clock alarm </throws>
		public virtual void  setClockAlarm(long time, byte[] state)
		{
			throw new OneWireException("This device does not have a clock alarm!");
		}
		
		/// <summary> This method sets the oscillator enable to the specified
		/// value. Use the method <CODE>writeDevice()</CODE> with this
		/// data to finalize the change to the device.
		/// 
		/// </summary>
		/// <param name="runEnable">true to enable clock oscillator
		/// </param>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <throws>  OneWireException Device does not support disabled clock </throws>
		public virtual void  setClockRunEnable(bool runEnable, byte[] state)
		{
			if (!runEnable)
				throw new OneWireException("This device's clock cannot be disabled!");
		}
		
		/// <summary> This method sets the Clock Alarm enable. Use the method
		/// <CODE>writeDevice()</CODE> with this data to finalize the
		/// change to the device.
		/// 
		/// </summary>
		/// <param name="alarmEnable">- true to enable clock alarm
		/// </param>
		/// <param name="state">device state
		/// 
		/// </param>
		/// <throws>  OneWireException Device does not support clock alarm </throws>
		public virtual void  setClockAlarmEnable(bool alarmEnable, byte[] state)
		{
			throw new OneWireException("This device does not have a clock alarm!");
		}
		
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
		public virtual bool hasHumidityAlarms()
		{
			return false;
		}
		
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
		public virtual bool hasSelectableHumidityResolution()
		{
			return false;
		}
		
		//--------
		//-------- Humidity I/O Methods
		//--------
		
		/// <summary> Performs a Humidity conversion.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		public virtual void  doHumidityConvert(byte[] state)
		{
			// do temp convert
			doTemperatureConvert(state);
			
			// do VDD for supply voltage
			doADConvert(CHANNEL_VDD, state);
			
			// do VAD for sensor voltage
			doADConvert(CHANNEL_VAD, state);
		}
		
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
		public virtual double getHumidity(byte[] state)
		{
			double temp = 0, vdd = 0, vad = 0, rh = 0;
			
			try
			{
				// read the temperature
				temp = getTemperature(state);
				
				// read the supply voltage
				vdd = getADVoltage(CHANNEL_VDD, state);
				
				// read the sample voltage
				vad = getADVoltage(CHANNEL_VAD, state);
			}
			catch (OneWireException e)
			{
				// know from this implementatin that this will never happen
				return 0.0;
			}
			
			// do calculation and check for out of range values
			if (vdd != 0)
				rh = (((vad / vdd) - 0.16) / 0.0062) / (1.0546 - 0.00216 * temp);
			
			if (rh < 0.0)
				rh = 0.0;
			else if (rh > 100.0)
				rh = 100.0;
			
			return rh;
		}
		
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
		public virtual double getHumidityResolution(byte[] state)
		{
			return 0.1;
		}
		
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
		public virtual double getHumidityAlarm(int alarmType, byte[] state)
		{
			throw new OneWireException("This device does not have a humidity alarm!");
		}
		
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
		public virtual void  setHumidityAlarm(int alarmType, double alarmValue, byte[] state)
		{
			throw new OneWireException("This device does not have a humidity alarm!");
		}
		
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
		public virtual void  setHumidityResolution(double resolution, byte[] state)
		{
			throw new OneWireException("This device does not have selectable humidity resolution!");
		}
	}
}