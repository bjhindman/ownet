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
using DSPortAdapter = com.dalsemi.onewire.adapter.DSPortAdapter;
using com.dalsemi.onewire.adapter;
using OneWireException = com.dalsemi.onewire.OneWireException;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> <P> 1-Wire&reg; container for a Single Addressable Switch, DS2413.  This container
	/// encapsulates the functionality of the 1-Wire family type <B>3A</B> (hex)</P>
	/// 
	/// <H3> Features </H3>
	/// <UL>
	/// <LI> Eight channels of programmable I/O with open-drain outputs
	/// <LI> Logic level sensing of the PIO pin can be sensed
	/// <LI> Multiple DS2413's can be identified on a common 1-Wire bus and operated
	/// independently.
	/// <LI> Supports 1-Wire Conditional Search command with response controlled by
	/// programmable PIO conditions
	/// <LI> Supports Overdrive mode which boosts communication speed up to 142k bits
	/// per second.
	/// </UL>
	/// 
	/// <H3> Usage </H3>
	/// 
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.SwitchContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer">
	/// 
	/// </seealso>
	/// <version>     1.00, 01 Jun 2002
	/// </version>
	/// <author>      JPE
	/// </author>
	public class OneWireContainer3A:OneWireContainer, SwitchContainer
	{
		/// <summary> Gets the Maxim Integrated Products part number of the iButton
		/// or 1-Wire Device as a <code>java.lang.String</code>.
		/// For example "DS1992".
		/// 
		/// </summary>
		/// <returns> iButton or 1-Wire device name
		/// </returns>
		override public System.String Name
		{
			get
			{
				return "DS2413";
			}
			
		}
		/// <summary> Retrieves the alternate Maxim Integrated Products part numbers or names.
		/// A 'family' of MicroLAN devices may have more than one part number
		/// depending on packaging.  There can also be nicknames such as
		/// "Crypto iButton".
		/// 
		/// </summary>
		/// <returns>  the alternate names for this iButton or 1-Wire device
		/// </returns>
		override public System.String AlternateNames
		{
			get
			{
				return "Dual Channel Switch";
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
				return "Dual Channel Addressable Switch";
			}
			
		}
		/// <summary> Returns the maximum speed this iButton or 1-Wire device can
		/// communicate at.
		/// 
		/// </summary>
		/// <returns> maximum speed
		/// </returns>
		/// <seealso cref="DSPortAdapter.setSpeed">
		/// </seealso>
		override public int MaxSpeed
		{
			get
			{
				return DSPortAdapter.SPEED_OVERDRIVE;
			}
			
		}
		/// <summary> Checks if the channels of this switch are 'high side'
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
				return false;
			}
			
		}
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Status memory bank of the DS2413 for memory map registers</summary>
		//private MemoryBankEEPROMstatus map; // never used
		
		/// <summary> Status memory bank of the DS2413 for the conditional search</summary>
		//private MemoryBankEEPROMstatus search; // never used
		
		/// <summary> PIO Access read command</summary>
		public static byte PIO_ACCESS_READ = (byte) SupportClass.Identity(0xF5);
		
		/// <summary> PIO Access read command</summary>
		public static byte PIO_ACCESS_WRITE = (byte) 0x5A;
		
		/// <summary> Used for 0xFF array</summary>
		private byte[] FF = new byte[8];
		
		
		//--------
		//-------- Constructors
		//--------
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a DS2413.
		/// Note that the method <code>setupContainer(com.dalsemi.onewire.adapter.DSPortAdapter,byte[])</code>
		/// must be called to set the correct <code>DSPortAdapter</code> device address.
		/// 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer.setupContainer(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) setupContainer(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer3A(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) OneWireContainer3A(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer3A(com.dalsemi.onewire.adapter.DSPortAdapter,long) OneWireContainer3A(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer3A(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer3A(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer3A():base()
		{
			
			for (int i = 0; i < FF.Length; i++)
				FF[i] = (byte) SupportClass.Identity(0x0FF);
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a DS2413.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       address of this DS2413
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer3A()">
		/// </seealso>
		/// <seealso cref="OneWireContainer3A(com.dalsemi.onewire.adapter.DSPortAdapter,long) OneWireContainer3A(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer3A(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer3A(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer3A(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
			
			for (int i = 0; i < FF.Length; i++)
				FF[i] = (byte) SupportClass.Identity(0x0FF);
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a DS2413.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       address of this DS2413
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer3A()">
		/// </seealso>
		/// <seealso cref="OneWireContainer3A(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) OneWireContainer3A(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer3A(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer3A(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer3A(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
			
			for (int i = 0; i < FF.Length; i++)
				FF[i] = (byte) SupportClass.Identity(0x0FF);
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a DS2413.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       address of this DS2413
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer3A()">
		/// </seealso>
		/// <seealso cref="OneWireContainer3A(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) OneWireContainer3A(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer3A(com.dalsemi.onewire.adapter.DSPortAdapter,long) OneWireContainer3A(DSPortAdapter,long)">
		/// </seealso>
		public OneWireContainer3A(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
			
			for (int i = 0; i < FF.Length; i++)
				FF[i] = (byte) SupportClass.Identity(0x0FF);
		}
		
		//--------
		//-------- Methods
		//--------
		
		//--------
		//-------- Switch Feature methods
		//--------
		
		/// <summary> Gets the number of channels supported by this switch.
		/// Channel specific methods will use a channel number specified
		/// by an integer from [0 to (<code>getNumberChannels(byte[])</code> - 1)].  Note that
		/// all devices of the same family will not necessarily have the
		/// same number of channels.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> the number of channels for this device
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		public virtual int getNumberChannels(byte[] state)
		{
			return 2;
		}
		
		/// <summary> Checks if the channels of this switch support
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
			return false;
		}
		
		/// <summary> Checks if the channels of this switch support
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
		
		/// <summary> Checks if the channels of this switch support
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
			return false;
		}
		
		/// <summary> Checks if the channels of this switch require that only one
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
			return false;
		}
		
		//--------
		//-------- Switch 'get' Methods
		//--------
		
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
			byte level = (byte) (0x01 << (channel * 2));
			return ((state[1] & level) == level);
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
			byte latch = (byte) (0x01 << ((channel * 2) + 1));
			return ((state[1] & latch) == latch);
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
			return false;
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
		}
		
		//--------
		//-------- Switch 'set' Methods
		//--------
		
		/// <summary> Sets the latch state of the indicated channel.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.
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
			byte latch = (byte) (0x01 << channel);
			byte temp;
			
			state[0] = (byte) SupportClass.Identity(0x00FC);
			
			if (getLatchState(0, state))
			{
				temp = (byte) 0x01;
				state[0] = (byte) (((byte) state[0]) | temp);
			}
			
			if (getLatchState(1, state))
			{
				temp = (byte) 0x02;
				state[0] = (byte) (((byte) state[0]) | temp);
			}
			
			if (latchState)
				state[0] = (byte) (state[0] | latch);
			else
				state[0] = (byte) (state[0] & ~ latch);
		}
		
		/// <summary> Sets the latch state for all of the channels.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.
		/// 
		/// </summary>
		/// <param name="set">the state to set all of the channels, in the range [0 to (<code>getNumberChannels(byte[])</code> - 1)]
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="getLatchState(int,byte[])">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		public virtual void  setLatchState(byte set_Renamed, byte[] state)
		{
			state[0] = (byte) set_Renamed;
		}
		
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
			byte[] buff = new byte[2];
			
			buff[0] = (byte) SupportClass.Identity(0xF5); // PIO Access Read Command
			buff[1] = (byte) SupportClass.Identity(0xFF); // Used to read the PIO Status Bit Assignment
			
			// select the device
			if (adapter.select(address))
			{
				adapter.dataBlock(buff, 0, 2);
			}
			else
				throw new OneWireIOException("Device select failed");
			
			return buff;
		}
		
		/// <summary> Retrieves the 1-Wire device register mask.  This register is
		/// returned as a byte array.  Pass this byte array to the 'get'
		/// and 'set' methods.  If the device register mask needs to be changed then call
		/// the 'writeRegister' to finalize the changes.
		/// 
		/// </summary>
		/// <returns> 1-Wire device register mask
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
		public virtual byte[] readRegister()
		{
			byte[] register = new byte[3];
			
			return register;
		}
		
		/// <summary> Writes the 1-Wire device sensor state that
		/// have been changed by 'set' methods.  Only the state registers that
		/// changed are updated.  This is done by referencing a field information
		/// appended to the state data.
		/// 
		/// </summary>
		/// <param name="state">1-Wire device PIO access write (x x x x x x PIOB PIOA)
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
			byte[] buff = new byte[5];
			
			buff[0] = (byte) 0x5A; // PIO Access Write Command
			buff[1] = (byte) state[0]; // Channel write information
			buff[2] = (byte) ~ state[0]; // Inverted write byte
			buff[3] = (byte) SupportClass.Identity(0xFF); // Confirmation Byte
			buff[4] = (byte) SupportClass.Identity(0xFF); // PIO Pin Status
			
			// select the device
			if (adapter.select(address))
			{
				adapter.dataBlock(buff, 0, 5);
			}
			else
				throw new OneWireIOException("Device select failed");
			
			if (buff[3] != (byte) SupportClass.Identity(0x00AA))
			{
				throw new OneWireIOException("Failure to change latch state.");
			}
		}
		
		/// <summary> Writes the 1-Wire device register mask that
		/// have been changed by 'set' methods.
		/// 
		/// </summary>
		/// <param name="register">1-Wire device sensor state
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
		public virtual void  writeRegister(byte[] register)
		{
		}
	}
}