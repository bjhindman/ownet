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
using OneWireIOException = com.dalsemi.onewire.adapter.OneWireIOException;
namespace com.dalsemi.onewire.container
{
	
	/// <summary> <P> 1-Wire&reg; container for 1-Wire(MicroLAN) Coupler, DS2409.
	/// This container encapsulates the functionality of the 1-Wire family type <B>1F</B> (hex).
	/// </P>
	/// 
	/// <H3> Features </H3>
	/// <UL>
	/// <li> Low impedance coupler to create large
	/// common-ground, multi-level MicroLAN
	/// networks
	/// <li> Keeps inactive branches pulled high to 5V
	/// <li> Simplifies network topology analysis by
	/// logically decoupling devices on active
	/// network segments
	/// <li> Conditional search for fast event signaling
	/// <li> Auxiliary 1-Wire TM line to connect a memory
	/// chip or to be used as digital input
	/// <li> Programmable, general purpose open drain
	/// control output
	/// <li> Operating temperature range from -40@htmlonly &#176C @endhtmlonly to
	/// +85@htmlonly &#176C @endhtmlonly
	/// <li> Compact, low cost 6-pin TSOC surface mount
	/// package
	/// </UL>
	/// 
	/// <P> Setting the latch on the DS2409 to 'on'
	/// (see {@link #setLatchState(int,boolean,boolean,byte[]) seLatchState})
	/// connects the channel [Main(0) or Auxillary(1)] to the 1-Wire data line.  Note
	/// that this is the opposite of the
	/// {@link com.dalsemi.onewire.container.OneWireContainer12 DS2406} and
	/// {@link com.dalsemi.onewire.container.OneWireContainer05 DS2405}
	/// which connect thier I/O lines to ground.
	/// <H3> Usage </H3>
	/// 
	/// <DL>
	/// <DD> See the usage example in
	/// {@link com.dalsemi.onewire.container.SwitchContainer SwitchContainer}
	/// for basic switch operations.
	/// </DL>
	/// 
	/// <H3> DataSheet </H3>
	/// <DL>
	/// <DD><A HREF="http://pdfserv.maxim-ic.com/arpdf/DS2409.pdf"> http://pdfserv.maxim-ic.com/arpdf/DS2409.pdf</A>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.SwitchContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer05">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer12">
	/// 
	/// </seealso>
	/// <version>     0.00, 13 Sept 2000
	/// </version>
	/// <author>      DSS
	/// </author>
	public class OneWireContainer1F:OneWireContainer, SwitchContainer
	{
		/// <summary> Gets the Maxim Integrated Products part number of the iButton
		/// or 1-Wire Device as a string.  For example 'DS1992'.
		/// 
		/// </summary>
		/// <returns> iButton or 1-Wire device name
		/// </returns>
		override public System.String Name
		{
			get
			{
				return "DS2409";
			}
			
		}
		/// <summary> Gets the alternate Maxim Integrated Products part numbers or names.
		/// A 'family' of 1-Wire Network devices may have more than one part number
		/// depending on packaging.  There can also be nicknames such as
		/// 'Crypto iButton'.
		/// 
		/// </summary>
		/// <returns> 1-Wire device alternate names
		/// </returns>
		override public System.String AlternateNames
		{
			get
			{
				return "Coupler";
			}
			
		}
		/// <summary> Gets a short description of the function of this iButton
		/// or 1-Wire Device type.
		/// 
		/// </summary>
		/// <returns> device description
		/// </returns>
		override public System.String Description
		{
			get
			{
				return "1-Wire Network Coupler with dual addressable " + "switches and a general purpose open drain control " + "output.  Provides a common ground for all connected" + "multi-level MicroLan networks.  Keeps inactive branches" + "Pulled to 5V.";
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
		/// <summary> Checks to see if the channels of this switch are 'high side'
		/// switches.  This indicates that when 'on' or <code>true</code>, the switch output is
		/// connect to the 1-Wire data.  If this method returns  <code>false</code>
		/// then when the switch is 'on' or <code>true</code>, the switch is connected
		/// to ground.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the switch is a 'high side' switch,
		/// <code>false</code> if the switch is a 'low side' switch
		/// 
		/// </returns>
		/// <seealso cref="getLatchState(int,byte[])">
		/// </seealso>
		virtual public bool HighSideSwitch
		{
			get
			{
				return true;
			}
			
		}
		/// <summary> Gets flag that indicates if a device was present when doing the
		/// last smart on.  Note that this flag is only valid if the DS2409
		/// flag was cleared with an ALL_LINES_OFF command and the last writeDevice
		/// performed a 'smart-on' on one of the channels.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if device detected on branch
		/// </returns>
		virtual public bool LastSmartOnDeviceDetect
		{
			get
			{
				return devicesOnBranch;
			}
			
		}
		
		//--------
		//-------- Static Final Variables
		//--------
		
		/// <summary>Offset of BITMAP in array returned from read state. </summary>
		protected internal const int BITMAP_OFFSET = 3;
		
		/// <summary>Offset of Status in array returned from read state. </summary>
		protected internal const int STATUS_OFFSET = 0;
		
		/// <summary>Offset of Main channel flag in array returned from read state. </summary>
		protected internal const int MAIN_OFFSET = 1;
		
		/// <summary>Offset of Main channel flag in array returned from read state. </summary>
		protected internal const int AUX_OFFSET = 2;
		
		/// <summary>Channel flag to indicate turn off. </summary>
		protected internal const int SWITCH_OFF = 0;
		
		/// <summary>Channel flag to indicate turn on. </summary>
		protected internal const int SWITCH_ON = 1;
		
		/// <summary>Channel flag to indicate smart on.  </summary>
		protected internal const int SWITCH_SMART = 2;
		
		/// <summary>Read Write Status register commmand. </summary>
		protected internal static byte READ_WRITE_STATUS_COMMAND = (byte) 0x5A;
		
		/// <summary>All lines off command. </summary>
		protected internal static byte ALL_LINES_OFF_COMMAND = (byte) 0x66;
		
		/// <summary>Discharge command. </summary>
		protected internal static byte DISCHARGE_COMMAND = (byte) SupportClass.Identity(0x99);
		
		/// <summary>Direct on main command. </summary>
		protected internal static byte DIRECT_ON_MAIN_COMMAND = (byte) SupportClass.Identity(0xA5);
		
		/// <summary>Smart on main command. </summary>
		protected internal static byte SMART_ON_MAIN_COMMAND = (byte) SupportClass.Identity(0xCC);
		
		/// <summary>Smart on aux command. </summary>
		protected internal static byte SMART_ON_AUX_COMMAND = (byte) 0x33;
		
		/// <summary>Main Channel number. </summary>
		public const int CHANNEL_MAIN = 0;
		
		/// <summary>Aux Channel number. </summary>
		public const int CHANNEL_AUX = 1;
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary>Flag to clear the activity on a write operation </summary>
		private bool clearActivityOnWrite;
		/// <summary>Flag to do speed checking </summary>
		private bool doSpeedEnable = true;
		/// <summary>Flag to indicated devices detected on branch during smart-on </summary>
		private bool devicesOnBranch = false;
		
		//--------
		//-------- Constructors
		//--------
		
		/// <summary> Create an empty container that is not complete until after a call
		/// to <code>setupContainer</code>. <p>
		/// 
		/// This is one of the methods to construct a container.  The others are
		/// through creating a OneWireContainer with parameters.
		/// 
		/// </summary>
		/// <seealso cref="setupContainer(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) super.setupContainer()">
		/// </seealso>
		public OneWireContainer1F():base()
		{
			
			clearActivityOnWrite = false;
		}
		
		/// <summary> Create a container with the provided adapter instance
		/// and the address of the iButton or 1-Wire device.<p>
		/// 
		/// This is one of the methods to construct a container.  The other is
		/// through creating a OneWireContainer with NO parameters.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter instance used to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       {@link com.dalsemi.onewire.utils.Address Address}
		/// of this 1-Wire device
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer1F() OneWireContainer1F">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer1F(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
			
			clearActivityOnWrite = false;
		}
		
		/// <summary> Create a container with the provided adapter instance
		/// and the address of the iButton or 1-Wire device.<p>
		/// 
		/// This is one of the methods to construct a container.  The other is
		/// through creating a OneWireContainer with NO parameters.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter instance used to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       {@link com.dalsemi.onewire.utils.Address Address}
		/// of this 1-Wire device
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer1F() OneWireContainer1F">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer1F(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
			
			clearActivityOnWrite = false;
		}
		
		/// <summary> Create a container with the provided adapter instance
		/// and the address of the iButton or 1-Wire device.<p>
		/// 
		/// This is one of the methods to construct a container.  The other is
		/// through creating a OneWireContainer with NO parameters.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter instance used to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       {@link com.dalsemi.onewire.utils.Address Address}
		/// of this 1-Wire device
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer1F() OneWireContainer1F">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer1F(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
			
			clearActivityOnWrite = false;
		}
		
		//--------
		//-------- Methods
		//--------
		
		//--------
		//-------- Sensor I/O methods
		//--------
		
		/// <summary> Retrieves the 1-Wire device sensor state.  This state is
		/// returned as a byte array.  Pass this byte array to the 'get'
		/// and 'set' methods.  If the device state needs to be changed then call
		/// the 'writeDevice' to finalize the changes.
		/// 
		/// </summary>
		/// <returns> 1-Wire device sensor state
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		public virtual byte[] readDevice()
		{
			byte[] ret_buf = new byte[4];
			
			if (doSpeedEnable)
				doSpeed();
			
			// read the status byte
			byte[] tmp_buf = deviceOperation(READ_WRITE_STATUS_COMMAND, (byte) SupportClass.Identity(0x00FF), 2);
			
			// extract the status byte
			ret_buf[0] = tmp_buf[2];
			
			return ret_buf;
		}
		
		/// <summary> Writes the 1-Wire device sensor state that
		/// have been changed by 'set' methods.  Only the state registers that
		/// changed are updated.  This is done by referencing a field information
		/// appended to the state data.
		/// 
		/// </summary>
		/// <param name="state">1-Wire device sensor state
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
		public virtual void  writeDevice(byte[] state)
		{
			int extra = 0;
			byte command, first_byte;
			byte[] tmp_buf = null;
			
			if (doSpeedEnable)
				doSpeed();
			
			// check for both switches set to on
			if ((Bit.arrayReadBit(MAIN_OFFSET, BITMAP_OFFSET, state) == 1) && (Bit.arrayReadBit(AUX_OFFSET, BITMAP_OFFSET, state) == 1))
			{
				if ((state[MAIN_OFFSET] != SWITCH_OFF) && (state[AUX_OFFSET] != SWITCH_OFF))
					throw new OneWireException("Attempting to set both channels on, only single channel on at a time");
			}
			
			// check if need to set control
			if (Bit.arrayReadBit(STATUS_OFFSET, BITMAP_OFFSET, state) == 1)
			{
				
				// create a command based on bit 6/7 of status
				first_byte = 0;
				
				// mode bit
				if (Bit.arrayReadBit(7, STATUS_OFFSET, state) == 1)
					first_byte |= (byte) 0x20;
				
				// Control output
				if (Bit.arrayReadBit(6, STATUS_OFFSET, state) == 1)
					first_byte |= (byte) SupportClass.Identity(0xC0);
				
				tmp_buf = deviceOperation(READ_WRITE_STATUS_COMMAND, first_byte, 2);
				state[0] = (byte) tmp_buf[2];
			}
			
			// check for AUX state change
			command = 0;
			
			if (Bit.arrayReadBit(AUX_OFFSET, BITMAP_OFFSET, state) == 1)
			{
				if ((state[AUX_OFFSET] == SWITCH_ON) || (state[AUX_OFFSET] == SWITCH_SMART))
				{
					command = SMART_ON_AUX_COMMAND;
					extra = 2;
				}
				else
				{
					command = ALL_LINES_OFF_COMMAND;
					extra = 0;
				}
			}
			
			// check for MAIN state change
			if (Bit.arrayReadBit(MAIN_OFFSET, BITMAP_OFFSET, state) == 1)
			{
				if (state[MAIN_OFFSET] == SWITCH_ON)
				{
					command = DIRECT_ON_MAIN_COMMAND;
					extra = 0;
				}
				else if (state[MAIN_OFFSET] == SWITCH_SMART)
				{
					command = SMART_ON_MAIN_COMMAND;
					extra = 2;
				}
				else
				{
					command = ALL_LINES_OFF_COMMAND;
					extra = 0;
				}
			}
			
			// check if there are events to clear and not about to do clear anyway
			if ((clearActivityOnWrite) && (command != ALL_LINES_OFF_COMMAND))
			{
				if ((Bit.arrayReadBit(4, STATUS_OFFSET, state) == 1) || (Bit.arrayReadBit(5, STATUS_OFFSET, state) == 1))
				{
					
					// clear the events
					deviceOperation(ALL_LINES_OFF_COMMAND, (byte) SupportClass.Identity(0xFF), 0);
					
					// set the channels back to the correct state
					if (command == 0)
					{
						if (Bit.arrayReadBit(0, STATUS_OFFSET, state) == 0)
							command = SMART_ON_MAIN_COMMAND;
						else if (Bit.arrayReadBit(2, STATUS_OFFSET, state) == 0)
							command = SMART_ON_AUX_COMMAND;
						
						extra = 2;
					}
				}
			}
			
			// check if there is a command to send
			if (command != 0)
				tmp_buf = deviceOperation(command, (byte) SupportClass.Identity(0xFF), extra);
			
			// if doing a SMART_ON, then look at result data for presence
			if ((command == SMART_ON_MAIN_COMMAND) || (command == SMART_ON_AUX_COMMAND))
			{
				// devices on branch indicated if 3rd byte is 0
				devicesOnBranch = (tmp_buf[2] == 0);
			}
			else
				devicesOnBranch = false;
			
			// clear clear activity on write
			clearActivityOnWrite = false;
			
			// clear the bitmap
			state[BITMAP_OFFSET] = 0;
		}
		
		/// <summary> <P>Force a power-on reset for parasitically powered 1-Wire
		/// devices connected to the main or auziliary output of the DS2409. </P>
		/// 
		/// <P>IMPORTANT: the duration of the discharge time should be 100ms minimum.</P> <BR>
		/// 
		/// </summary>
		/// <param name="time">number of milliseconds the lines are
		/// to be discharged for (minimum 100)
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
		public virtual void  dischargeLines(int time)
		{
			
			// Error checking
			if (time < 100)
				time = 100;
			
			if (doSpeedEnable)
				doSpeed();
			
			// discharge the lines
			deviceOperation(DISCHARGE_COMMAND, (byte) SupportClass.Identity(0xFF), 0);
			
			// wait for desired time and return.
			try
			{
				//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
				System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * time));
			}
			catch (System.Threading.ThreadInterruptedException e)
			{
				
				// DRAIN
			}
			
			// clear the discharge
			deviceOperation(READ_WRITE_STATUS_COMMAND, (byte) SupportClass.Identity(0x00FF), 2);
		}
		
		//--------
		//-------- Switch Feature methods
		//--------
		
		/// <summary> Checks to see if the channels of this switch support
		/// activity sensing.  If this method returns <code>true</code> then the
		/// method <code>getSensedActivity(int,byte[])</code> can be used.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if channels support activity sensing
		/// 
		/// </returns>
		/// <seealso cref="getSensedActivity(int,byte[])">
		/// </seealso>
		/// <seealso cref="clearActivity()">
		/// </seealso>
		public virtual bool hasActivitySensing()
		{
			return true;
		}
		
		/// <summary> Checks to see if the channels of this switch support
		/// level sensing.  If this method returns <code>true</code> then the
		/// method <code>getLevel(int,byte[])</code> can be used.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if channels support level sensing
		/// 
		/// </returns>
		/// <seealso cref="getLevel(int,byte[])">
		/// </seealso>
		public virtual bool hasLevelSensing()
		{
			return true;
		}
		
		/// <summary> Checks to see if the channels of this switch support
		/// 'smart on'. Smart on is the ability to turn on a channel
		/// such that only 1-Wire device on this channel are awake
		/// and ready to do an operation.  This greatly reduces
		/// the time to discover the device down a branch.
		/// If this method returns <code>true</code> then the
		/// method <code>setLatchState(int,boolean,boolean,byte[])</code>
		/// can be used with the <code>doSmart</code> parameter <code>true</code>.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if channels support 'smart on'
		/// 
		/// </returns>
		/// <seealso cref="setLatchState(int,boolean,boolean,byte[])">
		/// </seealso>
		public virtual bool hasSmartOn()
		{
			return true;
		}
		
		/// <summary> Checks to see if the channels of this switch require that only one
		/// channel is on at any one time.  If this method returns <code>true</code> then the
		/// method <code>setLatchState(int,boolean,boolean,byte[])</code>
		/// will not only affect the state of the given
		/// channel but may affect the state of the other channels as well
		/// to insure that only one channel is on at a time.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if only one channel can be on at a time.
		/// 
		/// </returns>
		/// <seealso cref="setLatchState(int,boolean,boolean,byte[])">
		/// </seealso>
		public virtual bool onlySingleChannelOn()
		{
			return true;
		}
		
		//--------
		//-------- Switch 'get' Methods
		//--------
		
		/// <summary> Query to get the number of channels supported by this switch.
		/// Channel specific methods will use a channel number specified
		/// by an integer from [0 to (<code>getNumberChannels(byte[])</code> - 1)].  Note that
		/// all devices of the same family will not necessarily have the
		/// same number of channels.  The DS2406 comes in two packages--one that
		/// has a single channel, and one that has two channels.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> the number of channels for this device
		/// </returns>
		public virtual int getNumberChannels(byte[] state)
		{
			return 2;
		}
		
		/// <summary> Checks the sensed level on the indicated channel.
		/// To avoid an exception, verify that this switch
		/// has level sensing with the  <code>hasLevelSensing()</code>.
		/// Level sensing means that the device can sense the logic
		/// level on its PIO pin.
		/// 
		/// </summary>
		/// <param name="channel">channel to execute this operation, in the range [0 to (<code>getNumberChannels(byte[])</code> - 1)]
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if level sensed is 'high' and <code>false</code> if level sensed is 'low'
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="hasLevelSensing()">
		/// </seealso>
		public virtual bool getLevel(int channel, byte[] state)
		{
			return (Bit.arrayReadBit(1 + channel * 2, STATUS_OFFSET, state) == 1);
		}
		
		/// <summary> Checks the latch state of the indicated channel.
		/// 
		/// </summary>
		/// <param name="channel">channel to execute this operation, in the range [0 to (<code>getNumberChannels(byte[])</code> - 1)]
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if channel latch is 'on'
		/// or conducting and <code>false</code> if channel latch is 'off' and not
		/// conducting.  Note that the actual output when the latch is 'on'
		/// is returned from the <code>isHighSideSwitch()</code> method.
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="isHighSideSwitch()">
		/// </seealso>
		/// <seealso cref="setLatchState(int,boolean,boolean,byte[])">
		/// </seealso>
		public virtual bool getLatchState(int channel, byte[] state)
		{
			return (Bit.arrayReadBit(channel * 2, STATUS_OFFSET, state) == 0);
		}
		
		/// <summary> Checks if the indicated channel has experienced activity.
		/// This occurs when the level on the PIO pins changes.  To clear
		/// the activity that is reported, call <code>clearActivity()</code>.
		/// To avoid an exception, verify that this device supports activity
		/// sensing by calling the method <code>hasActivitySensing()</code>.
		/// 
		/// </summary>
		/// <param name="channel">channel to execute this operation, in the range [0 to (<code>getNumberChannels(byte[])</code> - 1)]
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if activity was detected and <code>false</code> if no activity was detected
		/// 
		/// </returns>
		/// <throws>  OneWireException if this device does not have activity sensing </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="hasActivitySensing()">
		/// </seealso>
		/// <seealso cref="clearActivity()">
		/// </seealso>
		public virtual bool getSensedActivity(int channel, byte[] state)
		{
			return (Bit.arrayReadBit(4 + channel, STATUS_OFFSET, state) == 1);
		}
		
		//--------
		//-------- DS2409 Specific Switch 'get' Methods
		//--------
		
		/// <summary> Checks if the control I/O pin mode is automatic (see DS2409 data sheet).
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if control mode is automatic
		/// </returns>
		public virtual bool isModeAuto(byte[] state)
		{
			return (Bit.arrayReadBit(7, STATUS_OFFSET, state) == 0);
		}
		
		/// <summary> Checks the channel association of the control pin.
		/// This value only makes sense if
		/// the control mode is automatic (see <CODE>isModeAuto</CODE>).
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>int</code> the channel number that is associated
		/// with the control pin
		/// </returns>
		public virtual int getControlChannelAssociation(byte[] state)
		{
			return Bit.arrayReadBit(6, STATUS_OFFSET, state);
		}
		
		/// <summary> Checks the control data value.
		/// This value only makes sense if
		/// the control mode is manual (see <CODE>isModeAuto</CODE>).
		/// 0 = output transistor off, 1 = output transistor on
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>int</code> the control output transistor state
		/// </returns>
		public virtual int getControlData(byte[] state)
		{
			return Bit.arrayReadBit(6, STATUS_OFFSET, state);
		}
		
		//--------
		//-------- Switch 'set' Methods
		//--------
		
		/// <summary> Sets the latch state of the indicated channel.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="channel">channel to execute this operation, in the range [0 to (<code>getNumberChannels(byte[])</code> - 1)]
		/// </param>
		/// <param name="latchState"><code>true</code> to set the channel latch 'on'
		/// (conducting) and <code>false</code> to set the channel latch 'off' (not
		/// conducting).  Note that the actual output when the latch is 'on'
		/// is returned from the <code>isHighSideSwitch()</code> method.
		/// </param>
		/// <param name="doSmart">If latchState is 'on'/<code>true</code> then doSmart indicates
		/// if a 'smart on' is to be done.  To avoid an exception
		/// check the capabilities of this device using the
		/// <code>hasSmartOn()</code> method.
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="hasSmartOn()">
		/// </seealso>
		/// <seealso cref="getLatchState(int,byte[])">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		public virtual void  setLatchState(int channel, bool latchState, bool doSmart, byte[] state)
		{
			
			// set the state flag
			if (latchState)
				state[channel + 1] = (byte) ((doSmart)?SWITCH_SMART:SWITCH_ON);
			else
				state[channel + 1] = (byte) SupportClass.Identity(SWITCH_OFF);
			
			// indicate in bitmap the the state has changed
			Bit.arrayWriteBit(1, channel + 1, BITMAP_OFFSET, state);
		}
		
		/// <summary> Clears the activity latches the next time possible.  For
		/// example, on a DS2406/07, this happens the next time the
		/// status is read with <code>readDevice()</code>.
		/// 
		/// </summary>
		/// <throws>  OneWireException if this device does not support activity sensing </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="getSensedActivity(int,byte[])">
		/// </seealso>
		public virtual void  clearActivity()
		{
			clearActivityOnWrite = true;
		}
		
		//--------
		//-------- DS2409 Specific Switch 'set' Methods
		//--------
		
		/// <summary> Sets the control pin mode.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="makeAuto"><CODE>true</CODE> to set to auto mode, false for manual mode
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// </param>
		public virtual void  setModeAuto(bool makeAuto, byte[] state)
		{
			// set the bit
			Bit.arrayWriteBit((makeAuto?0:1), 7, STATUS_OFFSET, state);
			
			// indicate in bitmap the the state has changed
			Bit.arrayWriteBit(1, STATUS_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the control pin channel association.  This only makes sense
		/// if the contol pin is in automatic mode.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="channel">channel to associate with control pin
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <throws>  OneWireException when trying to set channel association in manual mode </throws>
		public virtual void  setControlChannelAssociation(int channel, byte[] state)
		{
			
			// check for invalid mode
			if (!isModeAuto(state))
				throw new OneWireException("Trying to set channel association in manual mode");
			
			// set the bit
			Bit.arrayWriteBit(channel, 6, STATUS_OFFSET, state);
			
			// indicate in bitmap the the state has changed
			Bit.arrayWriteBit(1, STATUS_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the control pin data to a value. Note this
		/// method only works if the control pin is in manual mode.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="data"><CODE>true</CODE> for on and <CODE>false</CODE> for off
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <throws>  OneWireException when trying to set control data in automatic mode </throws>
		public virtual void  setControlData(bool data, byte[] state)
		{
			// check for invalid mode
			if (isModeAuto(state))
				throw new OneWireException("Trying to set control data when control is in automatic mode");
			
			// set the bit
			Bit.arrayWriteBit((data?1:0), 6, STATUS_OFFSET, state);
			
			// indicate in bitmap the the state has changed
			Bit.arrayWriteBit(1, STATUS_OFFSET, BITMAP_OFFSET, state);
		}
		
		//--------
		//-------- Private methods
		//--------
		
		/// <summary> Do a DS2409 specidific operation.
		/// 
		/// </summary>
		/// <param name="command">code to send
		/// </param>
		/// <param name="sendByte">data byte to send
		/// </param>
		/// <param name="extra">number of extra bytes to send
		/// 
		/// </param>
		/// <returns> block of the complete resulting transaction
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		private byte[] deviceOperation(byte command, byte sendByte, int extra)
		{
			OneWireIOException exc = null;
			for (int attemptCounter = 2; attemptCounter > 0; attemptCounter--)
			{
				// Variables.
				byte[] raw_buf = new byte[extra + 2];
				
				// build block.
				raw_buf[0] = (byte) command;
				raw_buf[1] = (byte) sendByte;
				
				for (int i = 2; i < raw_buf.Length; i++)
					raw_buf[i] = (byte) SupportClass.Identity(0xFF);
				
				// Select the device.
				if (adapter.select(address))
				{
					
					// send the block
					adapter.dataBlock(raw_buf, 0, raw_buf.Length);
					
					// verify
					if (command == READ_WRITE_STATUS_COMMAND)
					{
						if ((byte) raw_buf[raw_buf.Length - 1] != (byte) raw_buf[raw_buf.Length - 2])
						{
							if (exc == null)
								exc = new OneWireIOException("OneWireContainer1F verify on command incorrect");
							continue;
						}
					}
					else
					{
						if ((byte) raw_buf[raw_buf.Length - 1] != (byte) command)
						{
							if (exc == null)
								exc = new OneWireIOException("OneWireContainer1F verify on command incorrect");
							continue;
						}
					}
					
					return raw_buf;
				}
				else
					throw new OneWireIOException("OneWireContainer1F failure - Device not found.");
			}
			// get here after a few attempts
			throw exc;
		}
	}
}