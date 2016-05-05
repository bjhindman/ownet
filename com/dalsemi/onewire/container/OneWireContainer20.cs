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
using System;
using com.dalsemi.onewire;
using com.dalsemi.onewire.utils;
using com.dalsemi.onewire.adapter;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary><P>1-Wire&reg; container that encapsulates the functionality of the 1-Wire
	/// family type <b>20</b> (hex), Maxim Integrated Products part number: <B> DS2450,
	/// 1-Wire Quad A/D Converter</B>.</P>
	/// 
	/// 
	/// <H3>Features</H3>
	/// <UL>
	/// <LI>Four high-impedance inputs
	/// <LI>Programmable input range (2.56V, 5.12V),
	/// resolution (1 to 16 bits) and alarm thresholds
	/// <LI>5V, single supply operation
	/// <LI>Very low power, 2.5 mW active, 25 @htmlonly &#181W @endhtmlonly idle
	/// <LI>Unused analog inputs can serve as open
	/// drain digital outputs for closed-loop control
	/// <LI>Operating temperature range from -40@htmlonly &#176C @endhtmlonly to
	/// +85@htmlonly &#176C @endhtmlonly
	/// </UL>
	/// 
	/// <H3>Usage</H3>
	/// 
	/// <P>Example device setup</P>
	/// <PRE><CODE>
	/// byte[] state = owd.readDevice();
	/// owd.setResolution(OneWireContainer20.CHANNELA, 16, state);
	/// owd.setResolution(OneWireContainer20.CHANNELB, 8, state);
	/// owd.setRange(OneWireContainer20.CHANNELA, 5.12, state);
	/// owd.setRange(OneWireContainer20.CHANNELB, 2.56, state);
	/// owd.writeDevice();
	/// </CODE></PRE>
	/// 
	/// <P>Example device read</P>
	/// <PRE><CODE>
	/// owd.doADConvert(OneWireContainer20.CHANNELA, state);
	/// owd.doADConvert(OneWireContainer20.CHANNELB, state);
	/// double chAVolatge = owd.getADVoltage(OneWireContainer20.CHANNELA, state);
	/// double chBVoltage = owd.getADVoltage(OneWireContainer20.CHANNELB, state);
	/// </CODE></PRE>
	/// 
	/// <H3>Note</H3>
	/// 
	/// <P>When converting analog voltages to digital, the user of the device must
	/// gaurantee that the voltage seen by the channel of the quad A/D does not exceed
	/// the selected input range of the device.  If this happens, the device will default
	/// to reading 0 volts.  There is NO way to know if the device is reading a higher than
	/// specified voltage or NO voltage.</P>
	/// 
	/// <H3> DataSheet </H3>
	/// 
	/// <A HREF="http://pdfserv.maxim-ic.com/arpdf/DS2450.pdf"> http://pdfserv.maxim-ic.com/arpdf/DS2450.pdf</A>
	/// 
	/// </summary>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      JK,DSS
	/// </author>
	public class OneWireContainer20:OneWireContainer, ADContainer
	{
		/// <summary> Gets the name of this 1-Wire device.
		/// 
		/// </summary>
		/// <returns> representation of this 1-Wire device's name
		/// </returns>
		override public System.String Name
		{
			get
			{
				return "DS2450";
			}
			
		}
		/// <summary> Gets any other possible names for this 1-Wire device.
		/// 
		/// </summary>
		/// <returns> representation of this 1-Wire device's other names
		/// </returns>
		override public System.String AlternateNames
		{
			get
			{
				return "1-Wire Quad A/D Converter";
			}
			
		}
		/// <summary> Gets a brief description of the functionality
		/// of this 1-Wire device.
		/// 
		/// </summary>
		/// <returns> description of this 1-Wire device's functionality
		/// </returns>
		override public System.String Description
		{
			get
			{
				return "Four high-impedance inputs for measurement of analog " + "voltages.  User programable input range.  Very low " + "power.  Built-in multidrop controller.  Channels " + "not used as input can be configured as outputs " + "through the use of open drain digital outputs. " + "Capable of use of Overdrive for fast data transfer. " + "Uses on-chip 16-bit CRC-generator to guarantee good data.";
			}
			
		}
		/// <summary> Gets the maximum speed this 1-Wire device can communicate at.
		/// 
		/// </summary>
		/// <returns> maximum speed of this One-Wire device
		/// </returns>
		override public int MaxSpeed
		{
			get
			{
				return DSPortAdapter.SPEED_OVERDRIVE;
			}
			
		}
		/// <summary> Gets an enumeration of memory banks.
		/// 
		/// </summary>
		/// <returns> enumeration of memory banks
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.MemoryBank">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.container.PagedMemoryBank">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.container.OTPMemoryBank">
		/// </seealso>
		override public System.Collections.IEnumerator MemoryBanks
		{
			get
			{
				System.Collections.ArrayList bank_vector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(4));
				
				// readout
				bank_vector.Add(readout);
				
				// control/alarms/calibration
				for (int i = 0; i < 3; i++)
					bank_vector.Add(regs[i]);
				
				return bank_vector.GetEnumerator();
			}
			
		}
		/// <summary> Queries to get the number of channels supported by this A/D.
		/// Channel specific methods will use a channel number specified
		/// by an integer from <CODE>[0 to (getNumberChannels() - 1)]</CODE>.
		/// 
		/// </summary>
		/// <returns> the number of channels
		/// </returns>
		virtual public int NumberADChannels
		{
			get
			{
				return NUM_CHANNELS;
			}
			
		}
		
		//--------
		//-------- Static Final Variables
		//--------
		
		/// <summary>Offset of BITMAP in array returned from read state </summary>
		public const int BITMAP_OFFSET = 24;
		
		/// <summary>Offset of ALARMS in array returned from read state </summary>
		public const int ALARM_OFFSET = 8;
		
		/// <summary>Offset of external power offset in array returned from read state </summary>
		public const int EXPOWER_OFFSET = 20;
		
		/// <summary>Channel A number </summary>
		public const int CHANNELA = 0;
		
		/// <summary>Channel B number </summary>
		public const int CHANNELB = 1;
		
		/// <summary>Channel C number </summary>
		public const int CHANNELC = 2;
		
		/// <summary>Channel D number </summary>
		public const int CHANNELD = 3;
		
		/// <summary>No preset value </summary>
		public const int NO_PRESET = 0;
		
		/// <summary>Preset value to zeros </summary>
		public const int PRESET_TO_ZEROS = 1;
		
		/// <summary>Preset value to ones </summary>
		public const int PRESET_TO_ONES = 2;
		
		/// <summary>Number of channels </summary>
		public const int NUM_CHANNELS = 4;
		
		/// <summary>DS2450 Convert command </summary>
		private static byte CONVERT_COMMAND = (byte) 0x3C;
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Voltage readout memory bank</summary>
		private MemoryBankAD readout;
		
		/// <summary> Control/Alarms/calibration memory banks vector</summary>
		private System.Collections.ArrayList regs;
		
		//--------
		//-------- Constructors
		//--------
		
		/// <summary> Default constructor</summary>
		public OneWireContainer20():base()
		{
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Creates a container with a provided adapter object
		/// and the address of the 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter required to communicate with
		/// this device
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public OneWireContainer20(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Creates a container with a provided adapter object
		/// and the address of the 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter required to communicate with
		/// this device
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public OneWireContainer20(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Creates a container with a provided adapter object
		/// and the address of the 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter required to communicate with
		/// this device
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public OneWireContainer20(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
		}
		
		//--------
		//-------- Methods
		//--------
		
		//--------
		//-------- A/D Feature methods
		//--------
		
		/// <summary> Queries to see if this A/D measuring device has high/low
		/// alarms.
		/// 
		/// </summary>
		/// <returns> <CODE>true</CODE> if it has high/low trips
		/// </returns>
		public virtual bool hasADAlarms()
		{
			return true;
		}
		
		/// <summary> Queries to get an array of available ranges for the specified
		/// A/D channel.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// 
		/// </param>
		/// <returns> available ranges starting
		/// from the largest range to the smallest range
		/// </returns>
		public virtual double[] getADRanges(int channel)
		{
			double[] ranges = new double[2];
			
			ranges[0] = 5.12;
			ranges[1] = 2.56;
			
			return ranges;
		}
		
		/// <summary> Queries to get an array of available resolutions based
		/// on the specified range on the specified A/D channel.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="range">specified range
		/// 
		/// </param>
		/// <returns> available resolutions
		/// </returns>
		public virtual double[] getADResolutions(int channel, double range)
		{
			double[] res = new double[16];
			
			for (int i = 0; i < 16; i++)
				res[i] = range / (double) (1 << (i + 1));
			
			return res;
		}
		
		/// <summary> Queries to see if this A/D supports doing multiple voltage
		/// conversions at the same time.
		/// 
		/// </summary>
		/// <returns> <CODE>true</CODE> if can do multi-channel voltage reads
		/// </returns>
		public virtual bool canADMultiChannelRead()
		{
			return true;
		}
		
		//--------
		//-------- A/D IO Methods
		//--------
		
		/// <summary> Retrieves the entire A/D control/status and alarm pages.
		/// It reads this and verifies the data with the onboard CRC generator.
		/// Use the byte array returned from this method with static
		/// utility methods to extract the status, alarm and other register values.
		/// Appended to the data is 2 bytes that represent a bitmap
		/// of changed bytes.  These bytes are used in the <CODE>writeADRegisters()</CODE>
		/// in conjuction with the 'set' methods to only write back the changed
		/// register bytes.
		/// 
		/// </summary>
		/// <returns> register page contents verified
		/// with onboard CRC
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Data was not read correctly </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual byte[] readDevice()
		{
			byte[] read_buf = new byte[27];
			MemoryBankAD mb;
			
			// read the banks, control/alarm/calibration
			for (int i = 0; i < 3; i++)
			{
				mb = (MemoryBankAD) regs[i];
				
				mb.readPageCRC(0, (i != 0), read_buf, i * 8);
			}
			
			// zero out the bitmap
			read_buf[24] = 0;
			read_buf[25] = 0;
			read_buf[26] = 0;
			
			return read_buf;
		}
		
		/// <summary> Writes the bytes in the provided A/D register pages that
		/// have been changed by the 'set' methods.  It knows which state has
		/// changed by looking at the bitmap fields appended to the
		/// register data.  Any alarm flags will be automatically
		/// cleared.  Only VCC powered indicator byte in physical location 0x1C
		/// can be written in the calibration memory bank.
		/// 
		/// </summary>
		/// <param name="state">register pages
		/// 
		/// </param>
		/// <throws>  OneWireIOException Data was not written correctly </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual void  writeDevice(byte[] state)
		{
			int start_offset, len, i, bank, index;
			bool got_block;
			MemoryBankAD mb;
			
			// Force a clear of the alarm flags
			for (i = 0; i < 4; i++)
			{
				
				// check if POR or alarm high/low flag present
				index = i * 2 + 1;
				
				if ((state[index] & (byte) SupportClass.Identity(0xB0)) != 0)
				{
					
					// clear the bits
					state[index] &= (byte) 0x0F;
					
					// set to write in bitmap
					Bit.arrayWriteBit(1, index, BITMAP_OFFSET, state);
				}
			}
			
			// only allow physical address 0x1C to be written in calibration bank
			state[BITMAP_OFFSET + 2] = (byte) (state[BITMAP_OFFSET + 2] & 0x10);
			
			// loop through the three memory banks collecting changes
			for (bank = 0; bank < 3; bank++)
			{
				start_offset = 0;
				len = 0;
				got_block = false;
				mb = (MemoryBankAD) regs[bank];
				
				// loop through each byte in the memory bank
				for (i = 0; i < 8; i++)
				{
					
					// check to see if this byte needs writing (skip control register for now)
					if (Bit.arrayReadBit(bank * 8 + i, BITMAP_OFFSET, state) == 1)
					{
						
						// check if already in a block
						if (got_block)
							len++;
						// new block
						else
						{
							got_block = true;
							start_offset = i;
							len = 1;
						}
						
						// check for last byte exception, write current block
						if (i == 7)
							mb.write(start_offset, state, bank * 8 + start_offset, len);
					}
					else if (got_block)
					{
						
						// done with this block so write it
						mb.write(start_offset, state, bank * 8 + start_offset, len);
						
						got_block = false;
					}
				}
			}
			
			// clear out the bitmap
			state[24] = 0;
			state[25] = 0;
			state[26] = 0;
		}
		
		/// <summary> Reads the voltage values.  Must be used after a <CODE>doADConvert()</CODE>
		/// method call.  Also must include the last valid state from the
		/// <CODE>readDevice()</CODE> method and this A/D must support multi-channel
		/// read <CODE>canMultiChannelRead()</CODE> if there are more then 1 channel.
		/// 
		/// </summary>
		/// <param name="state">current state of this device returned from
		/// <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> voltage values for all channels
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Data was not read correctly </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual double[] getADVoltage(byte[] state)
		{
			byte[] read_buf = new byte[8];
			double[] ret_dbl = new double[4];
			
			// get readout page
			readout.readPageCRC(0, false, read_buf, 0);
			
			// convert to array of doubles
			for (int ch = 0; ch < 4; ch++)
			{
				ret_dbl[ch] = interpretVoltage(com.dalsemi.onewire.utils.Convert.toLong(read_buf, ch * 2, 2), getADRange(ch, state));
               
			}
			
			return ret_dbl;
		}
		
		/// <summary> Reads a channels voltage value.  Must be used after a
		/// <CODE>doADConvert()</CODE> method call.  Also must include
		/// the last valid state from the <CODE>readDevice()</CODE> method.
		/// Note, if more then one channel is to be read then it is more
		/// efficient to use the <CODE>getADVoltage(byte[])</CODE> method that returns
		/// all channel values.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> voltage value for the specified
		/// channel
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Data was not read correctly </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual double getADVoltage(int channel, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			// get readout page
			byte[] read_buf = new byte[8];
			
			readout.readPageCRC(0, false, read_buf, 0);
			
			return interpretVoltage(com.dalsemi.onewire.utils.Convert.toLong(read_buf, channel * 2, 2), getADRange(channel, state));
		}
		
		/// <summary> Performs voltage conversion on specified channel.  The method
		/// <CODE>getADVoltage()</CODE> can be used to read the result
		/// of the conversion.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  OneWireIOException Data was not written correctly </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual void  doADConvert(int channel, byte[] state)
		{
			
			// call with set presets to 0
			doADConvert(channel, PRESET_TO_ZEROS, state);
		}
		
		/// <summary> Performs voltage conversion on all specified channels.  The method
		/// <CODE>getADVoltage()</CODE> can be used to read the result of the
		/// conversion. This A/D must support multi-channel read
		/// <CODE>canMultiChannelRead()</CODE> if there are more then 1 channel
		/// is specified.
		/// 
		/// </summary>
		/// <param name="doConvert">which channels to perform conversion on.
		/// </param>
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  OneWireIOException Data was not written correctly </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual void  doADConvert(bool[] doConvert, byte[] state)
		{
			
			// call with set presets to 0
			int[] presets = new int[4];
			
			for (int i = 0; i < 4; i++)
				presets[i] = PRESET_TO_ZEROS;
			
			doADConvert(doConvert, presets, state);
		}
		
		/// <summary> Performs voltage conversion on specified channel.  The method
		/// <CODE>getADVoltage()</CODE> can be used to read the result
		/// of the conversion.
		/// 
		/// </summary>
		/// <param name="channel">0,1,2,3 representing the channels A,B,C,D
		/// </param>
		/// <param name="preset">preset value:
		/// <CODE>NO_PRESET (0), PRESET_TO_ZEROS (1), and PRESET_TO_ONES (2)</CODE>
		/// </param>
		/// <param name="state">state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  OneWireIOException Data could not be written correctly </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual void  doADConvert(int channel, int preset, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			// perform the conversion (do fixed max conversion time)
			doADConvert((byte) (0x01 << channel), (byte) (preset << channel), 1440, state);
		}
		
		/// <summary> Performs voltage conversion on all specified channels.
		/// The method <CODE>getADVoltage()</CODE> can be used to read the result
		/// of the conversion.
		/// 
		/// </summary>
		/// <param name="doConvert">which channels to perform conversion on
		/// </param>
		/// <param name="preset">preset values
		/// <CODE>NO_PRESET (0), PRESET_TO_ZEROS (1), and PRESET_TO_ONES (2)</CODE>
		/// </param>
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  OneWireIOException Data could not be written correctly </throws>
		/// <throws>  OneWireException Could not find part </throws>
		public virtual void  doADConvert(bool[] doConvert, int[] preset, byte[] state)
		{
			byte input_select_mask = 0;
			byte read_out_control = 0;
			int time = 160; // Time required in micro Seconds to covert.
			
			// calculate the input mask, readout control, and conversion time
			for (int ch = 3; ch >= 0; ch--)
			{
				
				// input select
				input_select_mask <<= 1;
				
				if (doConvert[ch])
					input_select_mask |= (byte) (0x01);
				
				// readout control
				read_out_control <<= 2;
				
				if (preset[ch] == PRESET_TO_ZEROS)
					read_out_control |= (byte) (0x01);
				else if (preset[ch] == PRESET_TO_ONES)
					read_out_control |= (byte) (0x02);
				
				// conversion time
				time = (int) (time + (80 * getADResolution(ch, state)));
			}
			
			// do the conversion
			doADConvert(input_select_mask, read_out_control, time, state);
		}
		
		//--------
		//-------- A/D 'get' Methods
		//--------
		
		/// <summary> Extracts the alarm voltage value of the specified channel from the
		/// provided state buffer.  The state buffer is retrieved from the
		/// <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="alarmType">desired alarm, <CODE>ALARM_HIGH (1) or ALARM_LOW (0)</CODE>
		/// </param>
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> alarm value in volts
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual double getADAlarm(int channel, int alarmType, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			// extract alarm value and convert to voltage
			long temp_long = (long) (state[ALARM_OFFSET + channel * 2 + alarmType] & 0x00FF) << 8;
			
			return interpretVoltage(temp_long, getADRange(channel, state));
		}
		
		/// <summary> Extracts the alarm enable value of the specified channel from
		/// the provided state buffer.  The state buffer is retrieved from
		/// the <CODE>readDevice()</CODE> method.
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
		/// <returns> <CODE>true</CODE> if specified alarm is enabled
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual bool getADAlarmEnable(int channel, int alarmType, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			return (Bit.arrayReadBit(2 + alarmType, channel * 2 + 1, state) == 1);
		}
		
		/// <summary> Checks the alarm event value of the specified channel from the provided
		/// state buffer.  The state buffer is retrieved from the
		/// <CODE>readDevice()</CODE> method.
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
		/// <returns> <CODE>true</CODE> if specified alarm occurred
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual bool hasADAlarmed(int channel, int alarmType, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			return (Bit.arrayReadBit(4 + alarmType, channel * 2 + 1, state) == 1);
		}
		
		/// <summary> Extracts the conversion resolution of the specified channel from the
		/// provided state buffer expressed in volts.  The state is retrieved from the
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
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual double getADResolution(int channel, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			int res = state[channel * 2] & 0x0F;
			
			// return resolution, if 0 then 16 bits
			if (res == 0)
				res = 16;
			
			return getADRange(channel, state) / (double) (1 << res);
		}
		
		/// <summary> Extracts the input voltage range of the specified channel from
		/// the provided state buffer.  The state buffer is retrieved from
		/// the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="state">current state of the state
		/// returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> A/D input voltage range
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual double getADRange(int channel, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			return (Bit.arrayReadBit(0, channel * 2 + 1, state) == 1)?5.12:2.56;
		}
		
		/// <summary> Detects if the output is enabled for the specified channel from
		/// the provided register buffer.  The register buffer is retrieved
		/// from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="state">current state of the device
		/// returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> <CODE>true</CODE> if output is enabled on specified channel
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual bool isOutputEnabled(int channel, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			return (Bit.arrayReadBit(7, channel * 2, state) == 1);
		}
		
		/// <summary> Detects if the output is enabled for the specified channel from
		/// the provided register buffer.  The register buffer is retrieved
		/// from the <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="state">current state of the device
		/// returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> <CODE>false</CODE> if output is conducting to ground and
		/// <CODE>true</CODE> if not conducting
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual bool getOutputState(int channel, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			return (Bit.arrayReadBit(6, channel * 2, state) == 1);
		}
		
		/// <summary> Detects if this device has seen a Power-On-Reset (POR).  If this has
		/// occured it may be necessary to set the state of the device to the
		/// desired values.   The register buffer is retrieved from the
		/// <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="state">current state of the device
		/// returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> <CODE>false</CODE> if output is conducting to ground and
		/// <CODE>true</CODE> if not conducting
		/// </returns>
		public virtual bool getDevicePOR(byte[] state)
		{
			return (Bit.arrayReadBit(7, 1, state) == 1);
		}
		
		/// <summary> Extracts the state of the external power indicator from the provided
		/// register buffer.  Use 'setPower' to set or clear the external power
		/// indicator flag. The register buffer is retrieved from the
		/// <CODE>readDevice()</CODE> method.
		/// 
		/// </summary>
		/// <param name="state">current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <returns> <CODE>true</CODE> if set to external power operation
		/// </returns>
		public virtual bool isPowerExternal(byte[] state)
		{
			return (state[EXPOWER_OFFSET] != 0);
		}
		
		//--------
		//-------- A/D 'set' Methods
		//--------
		
		/// <summary> Sets the alarm voltage value of the specified channel in the
		/// provided state buffer.  The state buffer is retrieved from the
		/// <CODE>readDevice()</CODE> method. The method <CODE>writeDevice()</CODE>
		/// must be called to finalize these changes to the device.  Note that
		/// multiple 'set' methods can be called before one call to
		/// <CODE>writeDevice()</CODE>.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="alarmType">desired alarm, <CODE>ALARM_HIGH (1)
		/// or ALARM_LOW (0)</CODE>
		/// </param>
		/// <param name="alarm">alarm value (will be reduced to 8 bit resolution)
		/// </param>
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual void  setADAlarm(int channel, int alarmType, double alarm, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			int offset = ALARM_OFFSET + channel * 2 + alarmType;
			
			state[offset] = (byte) ((SupportClass.URShift(voltageToInt(alarm, getADRange(channel, state)), 8)) & 0x00FF);
			
			// set bitmap field to indicate this register has changed
			Bit.arrayWriteBit(1, offset, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the alarm enable value of the specified channel in the
		/// provided state buffer.  The state buffer is retrieved from the
		/// <CODE>readDevice()</CODE> method. The method <CODE>writeDevice()</CODE>
		/// must be called to finalize these changes to the device.  Note that
		/// multiple 'set' methods can be called before one call to
		/// <CODE>writeDevice()</CODE>.
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
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual void  setADAlarmEnable(int channel, int alarmType, bool alarmEnable, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			// change alarm enable
			Bit.arrayWriteBit(((alarmEnable)?1:0), 2 + alarmType, channel * 2 + 1, state);
			
			// set bitmap field to indicate this register has changed
			Bit.arrayWriteBit(1, channel * 2 + 1, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the conversion resolution value for the specified channel in
		/// the provided state buffer.  The state buffer is retrieved from the
		/// <CODE>readDevice()</CODE> method. The method <CODE>writeDevice()</CODE>
		/// must be called to finalize these changes to the device.  Note that
		/// multiple 'set' methods can be called before one call to
		/// <CODE>writeDevice()</CODE>.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="resolution">resolution to use in volts
		/// </param>
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual void  setADResolution(int channel, double resolution, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			// convert voltage resolution into bit resolution
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int div = (int) (getADRange(channel, state) / resolution);
			int res_bits = 0;
			
			do 
			{
				div = SupportClass.URShift(div, 1);
				
				res_bits++;
			}
			while (div != 0);
			
			res_bits -= 1;
			
			if (res_bits == 16)
				res_bits = 0;
			
			// check for valid bit resolution
			if ((res_bits < 0) || (res_bits > 15))
				throw new System.ArgumentException("Invalid resolution");
			
			// clear out the resolution
			state[channel * 2] &= (byte) SupportClass.Identity(0xF0);
			
			// set the resolution
			state[channel * 2] |= (byte) ((res_bits == 16)?0:res_bits);
			
			// set bitmap field to indicate this register has changed
			Bit.arrayWriteBit(1, channel * 2, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the input range for the specified channel in the provided state
		/// buffer.  The state buffer is retrieved from the <CODE>readDevice()</CODE>
		/// method. The method <CODE>writeDevice()</CODE> must be called to finalize
		/// these changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <CODE>writeDevice()</CODE>.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="range">max volt range, use
		/// getRanges() method to get available ranges
		/// </param>
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  IllegalArgumentException Invalid channel number passed </throws>
		public virtual void  setADRange(int channel, double range, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			// convert range into bit value
			int range_bit;
			
			if ((range > 5.00) & (range < 5.30))
				range_bit = 1;
			else if ((range > 2.40) & (range < 2.70))
				range_bit = 0;
			else
				throw new System.ArgumentException("Invalid range");
			
			// change range bit
			Bit.arrayWriteBit(range_bit, 0, channel * 2 + 1, state);
			
			// set bitmap field to indicate this register has changed
			Bit.arrayWriteBit(1, channel * 2 + 1, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the output enable and state for the specified channel in the
		/// provided register buffer.  The register buffer is retrieved from
		/// the <CODE>readDevice()</CODE> method. The method <CODE>writeDevice()</CODE>
		/// must be called to finalize these changes to the device.  Note that
		/// multiple 'set' methods can be called before one call to
		/// <CODE>writeDevice()</CODE>.
		/// 
		/// </summary>
		/// <param name="channel">channel in the range
		/// <CODE>[0 to (getNumberChannels() - 1)]</CODE>
		/// </param>
		/// <param name="outputEnable"><CODE>true</CODE> if output is enabled
		/// </param>
		/// <param name="outputState"><CODE>false</CODE> if output is conducting to
		/// ground and <CODE>true</CODE> if not conducting.  This
		/// parameter is not used if <CODE>outputEnable</CODE> is
		/// <CODE>false</CODE>
		/// </param>
		/// <param name="state">current state of the
		/// device returned from <CODE>readDevice()</CODE>
		/// </param>
		public virtual void  setOutput(int channel, bool outputEnable, bool outputState, byte[] state)
		{
			
			// check for valid channel value
			if ((channel < 0) || (channel > 3))
				throw new System.ArgumentException("Invalid channel number");
			
			// output enable bit
			Bit.arrayWriteBit(((outputEnable)?1:0), 7, channel * 2, state);
			
			// optionally set state
			if (outputEnable)
				Bit.arrayWriteBit(((outputState)?1:0), 6, channel * 2, state);
			
			// set bitmap field to indicate this register has changed
			Bit.arrayWriteBit(1, channel * 2, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets or clears the external power flag in the provided register buffer.
		/// The register buffer is retrieved from the <CODE>readDevice()</CODE> method.
		/// The method <CODE>writeDevice()</CODE> must be called to finalize these
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <CODE>writeDevice()</CODE>.
		/// 
		/// </summary>
		/// <param name="external"><CODE>true</CODE> if setting external power is used
		/// </param>
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// </param>
		public virtual void  setPower(bool external, byte[] state)
		{
			
			// sed the flag
			state[EXPOWER_OFFSET] = (byte) (external?0x40:0);
			
			// set bitmap field to indicate this register has changed
			Bit.arrayWriteBit(1, EXPOWER_OFFSET, BITMAP_OFFSET, state);
		}
		
		//--------
		//-------- Utility methods
		//--------
		
		/// <summary> Converts a raw voltage long value for the DS2450 into a valid voltage.
		/// Requires the max voltage value.
		/// 
		/// </summary>
		/// <param name="rawVoltage">raw voltage
		/// </param>
		/// <param name="range">max voltage
		/// 
		/// </param>
		/// <returns> calculated voltage based on the range
		/// </returns>
		public static double interpretVoltage(long rawVoltage, double range)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (((double) rawVoltage / 65535.0) * range);
		}
		
		/// <summary> Converts a voltage double value to the DS2450 specific int value.
		/// Requires the max voltage value.
		/// 
		/// </summary>
		/// <param name="voltage">voltage
		/// </param>
		/// <param name="range">max voltage
		/// 
		/// </param>
		/// <returns> the DS2450 voltage
		/// </returns>
		public static int voltageToInt(double voltage, double range)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (int) ((voltage * 65535.0) / range);
		}
		
		//--------
		//-------- Private methods
		//--------
		
		/// <summary> Create the memory bank interface to read/write</summary>
		private void  initMem()
		{
			
			// readout
			readout = new MemoryBankAD(this);
			
			// control
			regs = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(3));
			
			MemoryBankAD temp_mb = new MemoryBankAD(this);
			
			temp_mb.bankDescription = "A/D Control and Status";
			temp_mb.generalPurposeMemory = false;
			temp_mb.startPhysicalAddress = 8;
			temp_mb.readWrite = true;
			temp_mb.readOnly = false;
			
			regs.Add(temp_mb);
			
			// Alarms
			temp_mb = new MemoryBankAD(this);
			temp_mb.bankDescription = "A/D Alarm Settings";
			temp_mb.generalPurposeMemory = false;
			temp_mb.startPhysicalAddress = 16;
			temp_mb.readWrite = true;
			temp_mb.readOnly = false;
			
			regs.Add(temp_mb);
			
			// calibration
			temp_mb = new MemoryBankAD(this);
			temp_mb.bankDescription = "A/D Calibration";
			temp_mb.generalPurposeMemory = false;
			temp_mb.startPhysicalAddress = 24;
			temp_mb.readWrite = true;
			temp_mb.readOnly = false;
			
			regs.Add(temp_mb);
		}
		
		/// <summary> Performs voltage conversion on all specified channels.  The method
		/// <CODE>getADVoltage()</CODE> can be used to read the result of the
		/// conversion.
		/// 
		/// </summary>
		/// <param name="inputSelectMask">input select mask
		/// </param>
		/// <param name="readOutControl">read out control
		/// </param>
		/// <param name="timeUs">time in microseconds for conversion
		/// </param>
		/// <param name="state">current state of this
		/// device returned from <CODE>readDevice()</CODE>
		/// 
		/// </param>
		/// <throws>  OneWireIOException Data was not written correctly </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IlleaglArgumentException Invalid channel number passed </throws>
		private void  doADConvert(byte inputSelectMask, byte readOutControl, int timeUs, byte[] state)
		{
			
			// check if no conversions
			if (inputSelectMask == 0)
			{
				throw new System.ArgumentException("No conversion will take place.  No channel selected.");
			}
			
			// Create the command block to be sent.
			byte[] raw_buf = new byte[5];
			
			raw_buf[0] = CONVERT_COMMAND;
			raw_buf[1] = inputSelectMask;
			raw_buf[2] = (byte) readOutControl;
			raw_buf[3] = (byte) SupportClass.Identity(0xFF);
			raw_buf[4] = (byte) SupportClass.Identity(0xFF);
			
			// calculate the CRC16 up to and including readOutControl
			int crc16 = CRC16.compute(raw_buf, 0, 3, 0);
			
			// Send command block.
			if (adapter.select(address))
			{
				if (isPowerExternal(state))
				{
					
					// good power so send the entire block (with both CRC)
					adapter.dataBlock(raw_buf, 0, 5);
					
					// Wait for complete of conversion
					try
					{
						//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * ((timeUs / 1000) + 10)));
					}
					catch (System.Threading.ThreadInterruptedException e)
					{
					}
					
					// calculate the rest of the CRC16
					crc16 = CRC16.compute(raw_buf, 3, 2, crc16);
				}
				else
				{
					
					// parasite power so send the all but last byte
					adapter.dataBlock(raw_buf, 0, 4);
					
					// setup power delivery
					adapter.PowerDuration = DSPortAdapter.DELIVERY_INFINITE;
					adapter.startPowerDelivery(DSPortAdapter.CONDITION_AFTER_BYTE);
					
					// get the final CRC byte and start strong power delivery
					raw_buf[4] = (byte) adapter.Byte;
					crc16 = CRC16.compute(raw_buf, 3, 2, crc16);
					
					// Wait for power delivery to complete the conversion
					try
					{
						//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * ((timeUs / 1000) + 1)));
					}
					catch (System.Threading.ThreadInterruptedException e)
					{
					}
					
					// Turn power off.
					adapter.setPowerNormal();
				}
			}
			else
				throw new OneWireException("OneWireContainer20 - Device not found.");
			
			// check the CRC result
			if (crc16 != 0x0000B001)
				throw new OneWireIOException("OneWireContainer20 - Failure during conversion - Bad CRC");
			
			// check if still busy
			if (adapter.Byte == 0x00)
				throw new OneWireIOException("Conversion failed to complete.");
		}
	}
}