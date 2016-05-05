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
	
	
	/// <summary> <P> 1-Wire&reg; container for a Single Addressable Switch, DS2408.  This container
	/// encapsulates the functionality of the 1-Wire family type <B>29</B> (hex)</P>
	/// 
	/// <H3> Features </H3>
	/// <UL>
	/// <LI> Eight channels of programmable I/O with open-drain outputs
	/// <LI> Logic level sensing of the PIO pin can be sensed
	/// <LI> Multiple DS2408's can be identified on a common 1-Wire bus and operated
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
	public class OneWireContainer29:OneWireContainer, SwitchContainer
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
				return "DS2408";
			}
			
		}
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
				System.Collections.ArrayList bank_vector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(5));
				
				bank_vector.Add(map);
				bank_vector.Add(search);
				
				return bank_vector.GetEnumerator();
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
				return "8-Channel Addressable Switch";
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
				return "1-Wire 8-Channel Addressable Switch";
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
		
		/// <summary> Status memory bank of the DS2408 for memory map registers</summary>
		private MemoryBankEEPROMstatus map;
		
		/// <summary> Status memory bank of the DS2408 for the conditional search</summary>
		private MemoryBankEEPROMstatus search;
		
		/// <summary> Reset the activity latches</summary>
		public static byte RESET_ACTIVITY_LATCHES = (byte) SupportClass.Identity(0xC3);
		
		/// <summary> Used for 0xFF array</summary>
		private byte[] FF = new byte[8];
		
		
		//--------
		//-------- Constructors
		//--------
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a DS2408.
		/// Note that the method <code>setupContainer(com.dalsemi.onewire.adapter.DSPortAdapter,byte[])</code>
		/// must be called to set the correct <code>DSPortAdapter</code> device address.
		/// 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer.setupContainer(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) setupContainer(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer29(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) OneWireContainer29(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer29(com.dalsemi.onewire.adapter.DSPortAdapter,long) OneWireContainer29(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer29(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer29(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer29():base()
		{
			
			initmem();
			
			for (int i = 0; i < FF.Length; i++)
				FF[i] = (byte) SupportClass.Identity(0x0FF);
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a DS2408.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       address of this DS2408
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer29()">
		/// </seealso>
		/// <seealso cref="OneWireContainer29(com.dalsemi.onewire.adapter.DSPortAdapter,long) OneWireContainer29(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer29(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer29(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer29(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
			
			initmem();
			
			for (int i = 0; i < FF.Length; i++)
				FF[i] = (byte) SupportClass.Identity(0x0FF);
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a DS2408.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       address of this DS2408
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer29()">
		/// </seealso>
		/// <seealso cref="OneWireContainer29(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) OneWireContainer29(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer29(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer29(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer29(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
			
			initmem();
			
			for (int i = 0; i < FF.Length; i++)
				FF[i] = (byte) SupportClass.Identity(0x0FF);
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a DS2408.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       address of this DS2408
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer29()">
		/// </seealso>
		/// <seealso cref="OneWireContainer29(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) OneWireContainer29(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer29(com.dalsemi.onewire.adapter.DSPortAdapter,long) OneWireContainer29(DSPortAdapter,long)">
		/// </seealso>
		public OneWireContainer29(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
			
			initmem();
			
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
			// check the 88h byte bits 6 and 7
			// 00 - 4 channels
			// 01 - 5 channels
			// 10 - 8 channels
			// 11 - 16 channes, which hasn't been implemented yet
			return 8;
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
			return true;
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
			byte level = (byte) (0x01 << channel);
			return ((state[0] & level) == level);
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
			byte latch = (byte) (0x01 << channel);
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
			byte activity = (byte) (0x01 << channel);
			return ((state[2] & activity) == activity);
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
			adapter.select(address);
			byte[] buffer = new byte[9];
			
			buffer[0] = RESET_ACTIVITY_LATCHES;
			Array.Copy(FF, 0, buffer, 1, 8);
			
			adapter.dataBlock(buffer, 0, 9);
			
			if ((buffer[1] != (byte) SupportClass.Identity(0xAA)) && (buffer[1] != (byte) 0x55))
				throw new OneWireException("Sense Activity was not cleared.");
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
			
			if (latchState)
				state[1] = (byte) (state[1] | latch);
			else
				state[1] = (byte) (state[1] & ~ latch);
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
			state[1] = (byte) set_Renamed;
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
			byte[] state = new byte[3];
			
			Array.Copy(FF, 0, state, 0, 3);
			map.read(0, false, state, 0, 3);
			
			return state;
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
			
			search.read(0, false, register, 0, 3);
			
			return register;
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
			map.write(1, state, 1, 1);
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
			search.write(0, register, 0, 3);
		}
		
		/// <summary> Turns the Reset mode on/off.
		/// 
		/// </summary>
		/// <param name="set">if 'TRUE' the reset mode will be set or 'FALSE' to turn it off.
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
		public virtual void  setResetMode(byte[] register, bool set_Renamed)
		{
			if (set_Renamed && ((register[2] & 0x04) == 0x04))
			{
				register[2] = (byte) (register[2] & (byte) SupportClass.Identity(0xFB));
			}
			else if ((!set_Renamed) && ((register[2] & (byte) 0x04) == (byte) 0x00))
			{
				register[2] = (byte) (register[2] | (byte) 0x04);
			}
		}
		
		/// <summary> Retrieves the state of the VCC pin.  If the pin is powered 'TRUE' is
		/// returned else 'FALSE' is returned if the pin is grounded.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if VCC is powered and <code>false</code> if it is
		/// grounded.
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
		public virtual bool getVCC(byte[] register)
		{
			if ((register[2] & (byte) SupportClass.Identity(0x80)) == (byte) SupportClass.Identity(0x80))
				return true;
			
			return false;
		}
		
		/// <summary> Checks if the Power On Reset if on and if so clears it.
		/// 
		/// </summary>
		/// <param name="register">current register for conditional search, which
		/// if returned from <code>readRegister()</code>
		/// </param>
		public virtual void  clearPowerOnReset(byte[] register)
		{
			if ((register[2] & (byte) 0x08) == (byte) 0x08)
			{
				register[2] = (byte) ((byte) register[2] & (byte) SupportClass.Identity(0xF7));
			}
		}
		
		/// <summary> Checks if the 'or' Condition Search is set and if not sets it.
		/// 
		/// </summary>
		/// <param name="register">current register for conditional search, which
		/// if returned from <code>readRegister()</code>
		/// </param>
		public virtual void  orConditionalSearch(byte[] register)
		{
			if ((register[2] & (byte) 0x02) == (byte) 0x02)
			{
				register[2] = (byte) ((byte) register[2] & (byte) SupportClass.Identity(0xFD));
			}
		}
		
		/// <summary> Checks if the 'and' Conditional Search is set and if not sets it.
		/// 
		/// </summary>
		/// <param name="register">current register for conditional search, which
		/// if returned from <code>readRegister()</code>
		/// </param>
		public virtual void  andConditionalSearch(byte[] register)
		{
			if ((register[2] & (byte) 0x02) != (byte) 0x02)
			{
				register[2] = (byte) ((byte) register[2] | (byte) 0x02);
			}
		}
		
		/// <summary> Checks if the 'PIO' Conditional Search is set for input and if not sets it.
		/// 
		/// </summary>
		/// <param name="register">current register for conditional search, which
		/// if returned from <code>readRegister()</code>
		/// </param>
		public virtual void  pioConditionalSearch(byte[] register)
		{
			if ((register[2] & (byte) 0x01) == (byte) 0x01)
			{
				register[2] = (byte) ((byte) register[2] & (byte) SupportClass.Identity(0xFE));
			}
		}
		
		/// <summary> Checks if the activity latches are set for Conditional Search and if not sets it.
		/// 
		/// </summary>
		/// <param name="register">current register for conditional search, which
		/// if returned from <code>readRegister()</code>
		/// </param>
		public virtual void  activityConditionalSearch(byte[] register)
		{
			if ((register[2] & (byte) 0x01) != (byte) 0x01)
			{
				register[2] = (byte) ((byte) register[2] | (byte) 0x01);
			}
		}
		
		/// <summary> Sets the channel passed to the proper state depending on the set parameter for
		/// responding to the Conditional Search.
		/// 
		/// </summary>
		/// <param name="channel"> current channel to set
		/// </param>
		/// <param name="set">     whether to turn the channel on/off for Conditional Search
		/// </param>
		/// <param name="register">current register for conditional search, which
		/// if returned from <code>readRegister()</code>
		/// </param>
		public virtual void  setChannelMask(int channel, bool set_Renamed, byte[] register)
		{
			byte mask = (byte) (0x01 << channel);
			
			if (set_Renamed)
				register[0] = (byte) ((byte) register[0] | (byte) mask);
			else
				register[0] = (byte) ((byte) register[0] & (byte) ~ mask);
		}
		
		/// <summary> Sets the channel passed to the proper state depending on the set parameter for
		/// the correct polarity in the Conditional Search.
		/// 
		/// </summary>
		/// <param name="channel"> current channel to set
		/// </param>
		/// <param name="set">     whether to turn the channel on/off for polarity
		/// Conditional Search
		/// </param>
		/// <param name="register">current register for conditional search, which
		/// if returned from <code>readRegister()</code>
		/// </param>
		public virtual void  setChannelPolarity(int channel, bool set_Renamed, byte[] register)
		{
			byte polarity = (byte) (0x01 << channel);
			
			if (set_Renamed)
				register[1] = (byte) ((byte) register[1] | (byte) polarity);
			else
				register[1] = (byte) ((byte) register[1] & (byte) ~ polarity);
		}
		
		/// <summary> Retrieves the information if the channel is masked for the Conditional Search.
		/// 
		/// </summary>
		/// <param name="channel"> current channel to set
		/// </param>
		/// <param name="register">current register for conditional search, which
		/// if returned from <code>readRegister()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if the channel is masked and <code>false</code> other wise.
		/// </returns>
		public virtual bool getChannelMask(int channel, byte[] register)
		{
			byte mask = (byte) (0x01 << channel);
			
			return ((register[0] & mask) == mask);
		}
		
		/// <summary> Retrieves the polarity of the channel for the Conditional Search.
		/// 
		/// </summary>
		/// <param name="channel"> current channel to set
		/// </param>
		/// <param name="register">current register for conditional search, which
		/// if returned from <code>readRegister()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if the channel is masked and <code>false</code> other wise.
		/// </returns>
		public virtual bool getChannelPolarity(int channel, byte[] register)
		{
			byte polarity = (byte) (0x01 << channel);
			
			return ((register[1] & polarity) == polarity);
		}
		
		/// <summary> Initialize the memory banks and data associated with each.</summary>
		private void  initmem()
		{
			// Memory map registers
			map = new MemoryBankEEPROMstatus(this);
			map.bankDescription = "Memory mapped register of pin logic state, port output " + "latch logic state and activity latch logic state.";
			map.startPhysicalAddress = 136;
			map.size = 3;
			map.readOnly = true;
			
			// Conditional Search
			search = new MemoryBankEEPROMstatus(this);
			search.bankDescription = "Conditional search bit mask, polarity bit mask and " + "control register.";
			search.startPhysicalAddress = 139;
			search.size = 3;
			search.readWrite = true;
		}
	}
}