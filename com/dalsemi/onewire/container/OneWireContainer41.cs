/*---------------------------------------------------------------------------
* Copyright (C) 2002-2012 Maxim Integrated Products, All Rights Reserved.
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
using com.dalsemi.onewire.debug;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> <P> 1-Wire&reg; container for a Temperature and Humidity/A-D Logging iButton, DS1922.
	/// This container encapsulates the functionality of the 1-Wire family type <B>22</B> (hex).
	/// </P>
	/// 
	/// <H3> Features </H3>
	/// <UL>
	/// <LI> Logs up to 8192 consecutive temperature/humidity/A-D measurements in
	/// nonvolatile, read-only memory
	/// <LI> Real-Time clock
	/// <LI> Programmable high and low temperature alarms
	/// <LI> Programmable high and low humidity/A-D alarms
	/// <LI> Automatically 'wakes up' and logs temperature at user-programmable intervals
	/// <LI> 4096 bits of general-purpose read/write nonvolatile memory
	/// <LI> 256-bit scratchpad ensures integrity of data transfer
	/// <LI> On-chip 16-bit CRC generator to verify read operations
	/// </UL>
	/// 
	/// <H3> Memory </H3>
	/// 
	/// <P> The memory can be accessed through the objects that are returned
	/// from the {@link #getMemoryBanks() getMemoryBanks} method. </P>
	/// 
	/// The following is a list of the MemoryBank instances that are returned:
	/// 
	/// <UL>
	/// <LI> <B> Scratchpad with CRC and Password support </B>
	/// <UL>
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 32 starting at physical address 0
	/// <LI> <I> Features</I> Read/Write not-general-purpose volatile
	/// <LI> <I> Pages</I> 1 page of length 32 bytes
	/// <LI> <I> Page Features </I> page-device-CRC
	/// <li> <i> Extra information for each page</i>  Target address, offset, length 3
	/// <LI> <i> Supports Copy Scratchpad With Password command </I>
	/// </UL>
	/// <LI> <B> Main Memory </B>
	/// <UL>
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 512 starting at physical address 0
	/// <LI> <I> Features</I> Read/Write general-purpose non-volatile
	/// <LI> <I> Pages</I> 16 pages of length 32 bytes giving 29 bytes Packet data payload
	/// <LI> <I> Page Features </I> page-device-CRC
	/// <LI> <I> Read-Only and Read/Write password </I> if enabled, passwords are required for
	/// reading from and writing to the device.
	/// </UL>
	/// <LI> <B> Register control </B>
	/// <UL>
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 64 starting at physical address 512
	/// <LI> <I> Features</I> Read/Write not-general-purpose non-volatile
	/// <LI> <I> Pages</I> 2 pages of length 32 bytes
	/// <LI> <I> Page Features </I> page-device-CRC
	/// <LI> <I> Read-Only and Read/Write password </I> if enabled, passwords are required for
	/// reading from and writing to the device.
	/// </UL>
	/// <LI> <B> Temperature/Humidity/A-D log </B>
	/// <UL>
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 8192 starting at physical address 4096
	/// <LI> <I> Features</I> Read-only not-general-purpose non-volatile
	/// <LI> <I> Pages</I> 256 pages of length 32 bytes
	/// <LI> <I> Page Features </I> page-device-CRC
	/// <LI> <I> Read-Only and Read/Write password </I> if enabled, passwords are required for
	/// reading from and writing to the device.
	/// </UL>
	/// </UL>
	/// 
	/// <H3> Usage </H3>
	/// 
	/// <p>The code below starts a mission with the following characteristics:
	/// <ul>
	/// <li>Rollover flag enabled.</li>
	/// <li>Sets both channels (temperature and humidity) to low resolution</li>
	/// <li>High temperature alarm of 28.0@htmlonly &#176C @endhtmlonly and a low temperature alarm of 23.0@htmlonly &#176C @endhtmlonly.</li>
	/// <li>High humidity alarm of 70%RH and a low temperature alarm of 20%RH.</li>
	/// <li>Sets the Real-Time Clock to the host system's clock.</li>
	/// <li>The mission will start in 2 minutes.</li>
	/// <li>A sample rate of 1.5 minutes.</li>
	/// </ul></p>
	/// <pre><code>
	/// // "ID" is a byte array of size 8 with an address of a part we
	/// // have already found with family code 22 hex
	/// // "access" is a DSPortAdapter
	/// OneWireContainer41 ds1922 = (OneWireContainer41)access.getDeviceContainer(ID);
	/// ds1922.setupContainer(access,ID);
	/// //  stop the currently running mission, if there is one
	/// ds1922.stopMission();
	/// //  clear the previous mission results
	/// ds1922.clearMemory();
	/// //  set the high temperature alarm to 28 C
	/// ds1922.setMissionAlarm(ds1922.TEMPERATURE_CHANNEL, ds1922.ALARM_HIGH, 28);
	/// ds1922.setMissionAlarmEnable(ds1922.TEMPERATURE_CHANNEL,
	/// ds1922.ALARM_HIGH, true);
	/// //  set the low temperature alarm to 23 C
	/// ds1922.setMissionAlarm(ds1922.TEMPERATURE_CHANNEL, ds1922.ALARM_LOW, 23);
	/// ds1922.setMissionAlarmEnable(ds1922.TEMPERATURE_CHANNEL,
	/// ds1922.ALARM_LOW, true);
	/// //  set the high humidity alarm to 70%RH
	/// ds1922.setMissionAlarm(ds1922.DATA_CHANNEL, ds1922.ALARM_HIGH, 70);
	/// ds1922.setMissionAlarmEnable(ds1922.DATA_CHANNEL,
	/// ds1922.ALARM_HIGH, true);
	/// //  set the low humidity alarm to 20%RH
	/// ds1922.setMissionAlarm(ds1922.DATA_CHANNEL, ds1922.ALARM_LOW, 20);
	/// ds1922.setMissionAlarmEnable(ds1922.DATA_CHANNEL,
	/// ds1922.ALARM_LOW, true);
	/// // set both channels to low resolution.
	/// ds1922.setMissionResolution(ds1922.TEMPERATURE_CHANNEL,
	/// ds1922.getMissionResolutions()[0]);
	/// ds1922.setMissionResolution(ds1922.DATA_CHANNEL,
	/// ds1922.getMissionResolutions()[0]);
	/// // enable both channels
	/// boolean[] enableChannel = new boolean[ds1922.getNumberMissionChannels()];
	/// enableChannel[ds1922.TEMPERATURE_CHANNEL] = true;
	/// enableChannel[ds1922.DATA_CHANNEL] = true;
	/// //  now start the mission with a sample rate of 1 minute
	/// ds1922.startNewMission(90, 2, true, true, enableChannel);
	/// </code></pre>
	/// <p>The following code processes the mission log:</p>
	/// <code><pre>
	/// System.out.println("Temperature Readings");
	/// if(ds1922.getMissionChannelEnable(owc.TEMPERATURE_CHANNEL))
	/// {
	/// int dataCount =
	/// ds1922.getMissionSampleCount(ds1922.TEMPERATURE_CHANNEL);
	/// System.out.println("SampleCount = " + dataCount);
	/// for(int i=0; i&lt;dataCount; i++)
	/// {
	/// System.out.println(
	/// ds1922.getMissionSample(ds1922.TEMPERATURE_CHANNEL, i));
	/// }
	/// }
	/// System.out.println("Humidity Readings");
	/// if(ds1922.getMissionChannelEnable(owc.DATA_CHANNEL))
	/// {
	/// int dataCount =
	/// ds1922.getMissionSampleCount(ds1922.DATA_CHANNEL);
	/// System.out.println("SampleCount = " + dataCount);
	/// for(int i=0; i&lt;dataCount; i++)
	/// {
	/// System.out.println(
	/// ds1922.getMissionSample(ds1922.DATA_CHANNEL, i));
	/// }
	/// }
	/// </pre></code>
	/// 
	/// <p>Also see the usage examples in the {@link com.dalsemi.onewire.container.TemperatureContainer TemperatureContainer}
	/// and {@link com.dalsemi.onewire.container.ClockContainer ClockContainer}
	/// and {@link com.dalsemi.onewire.container.ADContainer ADContainer}
	/// interfaces.</p>
	/// 
	/// For examples regarding memory operations,
	/// <uL>
	/// <li> See the usage example in
	/// {@link com.dalsemi.onewire.container.OneWireContainer OneWireContainer}
	/// to enumerate the MemoryBanks.
	/// <li> See the usage examples in
	/// {@link com.dalsemi.onewire.container.MemoryBank MemoryBank} and
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// for bank specific operations.
	/// </uL>
	/// 
	/// <H3> DataSheet </H3>
	/// <P>DataSheet link is unavailable at time of publication.  Please visit the website
	/// and search for DS1922 or DS2422 to find the current datasheet.
	/// <DL>
	/// <DD><A HREF="http://www.maxim-ic.com/">Maxim Website</A>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.SwitchContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.TemperatureContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.ADContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.MissionContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.PasswordContainer">
	/// 
	/// </seealso>
	/// <version>     1.02, 30 November 2010
	/// </version>
	/// <author>      Maxim Integrated Products
	/// 
	/// </author>
	public class OneWireContainer41:OneWireContainer, PasswordContainer, MissionContainer, ClockContainer, TemperatureContainer, ADContainer, HumidityContainer
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
				System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(4));
				
				v.Add(scratch);
				v.Add(userDataMemory);
				v.Add(register);
				v.Add(log);
				
				return v.GetEnumerator();
			}
			
		}
		/// <summary> Returns instance of the memory bank representing this device's
		/// scratchpad.
		/// 
		/// </summary>
		/// <returns> scratchpad memory bank
		/// </returns>
		virtual public MemoryBankScratchCRCPW ScratchpadMemoryBank
		{
			get
			{
				return this.scratch;
			}
			
		}
		/// <summary> Returns instance of the memory bank representing this device's
		/// general-purpose user data memory.
		/// 
		/// </summary>
		/// <returns> user data memory bank
		/// </returns>
		virtual public MemoryBankNVCRCPW UserDataMemoryBank
		{
			get
			{
				return this.userDataMemory;
			}
			
		}
		/// <summary> Returns instance of the memory bank representing this device's
		/// data log.
		/// 
		/// </summary>
		/// <returns> data log memory bank
		/// </returns>
		virtual public MemoryBankNVCRCPW DataLogMemoryBank
		{
			get
			{
				return this.log;
			}
			
		}
		/// <summary> Returns instance of the memory bank representing this device's
		/// special function registers.
		/// 
		/// </summary>
		/// <returns> register memory bank
		/// </returns>
		virtual public MemoryBankNVCRCPW RegisterMemoryBank
		{
			get
			{
				return this.register;
			}
			
		}
		/// <summary> Returns the maximum speed this iButton device can
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
				return partNumber;
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
				if (partNumber.Equals("DS1923"))
					return "Hygrochron";
				return "Thermochron8k";
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
				return descriptionString;
			}
			
		}
		/// <summary> Returns the Device Configuration Byte, which specifies whether or
		/// not this device is a DS1922, DS1923, or DS2422.
		/// 
		/// </summary>
		/// <returns> the Device Configuration Byte
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		virtual public byte DeviceConfigByte
		{
			get
			{
				if (deviceConfigByte == (byte) SupportClass.Identity(0xFF))
				{
					byte[] state = readDevice();
					if (deviceConfigByte == (byte) SupportClass.Identity(0xFF))
						deviceConfigByte = state[DEVICE_CONFIGURATION_BYTE & 0x3F];
				}
				return deviceConfigByte;
			}
			
		}
		/// <summary> Directs the container to avoid the calls to doSpeed() in methods that communicate
		/// with the DS1922/DS2422. To ensure that all parts can talk to the 1-Wire bus
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
		/// <summary> Returns the length in bytes of the Read-Only password.
		/// 
		/// </summary>
		/// <returns> the length in bytes of the Read-Only password.
		/// </returns>
		virtual public int ReadOnlyPasswordLength
		{
			get
			{
				return PASSWORD_LENGTH;
			}
			
		}
		/// <summary> Returns the length in bytes of the Read/Write password.
		/// 
		/// </summary>
		/// <returns> the length in bytes of the Read/Write password.
		/// </returns>
		virtual public int ReadWritePasswordLength
		{
			get
			{
				return PASSWORD_LENGTH;
			}
			
		}
		/// <summary> Returns the length in bytes of the Write-Only password.
		/// 
		/// </summary>
		/// <returns> the length in bytes of the Write-Only password.
		/// </returns>
		virtual public int WriteOnlyPasswordLength
		{
			get
			{
				throw new OneWireException("The DS1922 does not have a write only password.");
			}
			
		}
		/// <summary> Returns the absolute address of the memory location where
		/// the Read-Only password is written.
		/// 
		/// </summary>
		/// <returns> the absolute address of the memory location where
		/// the Read-Only password is written.
		/// </returns>
		virtual public int ReadOnlyPasswordAddress
		{
			get
			{
				return READ_ACCESS_PASSWORD;
			}
			
		}
		/// <summary> Returns the absolute address of the memory location where
		/// the Read/Write password is written.
		/// 
		/// </summary>
		/// <returns> the absolute address of the memory location where
		/// the Read/Write password is written.
		/// </returns>
		virtual public int ReadWritePasswordAddress
		{
			get
			{
				return READ_WRITE_ACCESS_PASSWORD;
			}
			
		}
		/// <summary> Returns the absolute address of the memory location where
		/// the Write-Only password is written.
		/// 
		/// </summary>
		/// <returns> the absolute address of the memory location where
		/// the Write-Only password is written.
		/// </returns>
		virtual public int WriteOnlyPasswordAddress
		{
			get
			{
				throw new OneWireException("The DS1922 does not have a write password.");
			}
			
		}
		/// <summary> Returns true if the device's Read-Only password has been enabled.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the device's Read-Only password has been enabled.
		/// </returns>
		virtual public bool DeviceReadOnlyPasswordEnable
		{
			get
			{
				return readOnlyPasswordEnabled;
			}
			
		}
		/// <summary> Returns true if the device's Read/Write password has been enabled.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the device's Read/Write password has been enabled.
		/// </returns>
		virtual public bool DeviceReadWritePasswordEnable
		{
			get
			{
				return readWritePasswordEnabled;
			}
			
		}
		/// <summary> Returns true if the device's Write-Only password has been enabled.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the device's Write-Only password has been enabled.
		/// </returns>
		virtual public bool DeviceWriteOnlyPasswordEnable
		{
			get
			{
				throw new OneWireException("The DS1922 does not have a Write Only Password.");
			}
			
		}
		/// <summary> <p>Enables/Disables passwords for this device.  If the part has more than one
		/// type of password (Read-Only, Write-Only, or Read/Write), all passwords
		/// will be enabled.  This function is equivalent to the following:
		/// <code> owc41.setDevicePasswordEnable(
		/// owc41.hasReadOnlyPassword(),
		/// owc41.hasReadWritePassword(),
		/// owc41.hasWriteOnlyPassword() ); </code></p>
		/// 
		/// <p>For this to be successful, either write-protect passwords must be disabled,
		/// or the write-protect password(s) for this container must be set and must match
		/// the value of the write-protect password(s) in the device's register.</P>
		/// 
		/// <P><B>
		/// WARNING: Enabling passwords requires that both the read password and the
		/// read/write password be re-written to the part.  Before calling this method,
		/// you should set the container read password and read/write password values.
		/// This will ensure that the correct value is written into the part.
		/// </B></P>
		/// 
		/// </summary>
		/// <param name="enableAll">if <code>true</code>, all passwords are enabled.  Otherwise,
		/// all passwords are disabled.
		/// </param>
		virtual public bool DevicePasswordEnableAll
		{
			set
			{
				setDevicePasswordEnable(value, value, false);
			}
			
		}
		/// <summary> Returns true if the password used by the API for reading from the
		/// device's memory has been set.  The return value is not affected by
		/// whether or not the read password of the container actually matches
		/// the value in the device's password register
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the password used by the API for
		/// reading from the device's memory has been set.
		/// </returns>
		virtual public bool ContainerReadOnlyPasswordSet
		{
			get
			{
				return readPasswordSet;
			}
			
		}
		/// <summary> Returns true if the password used by the API for reading from or
		/// writing to the device's memory has been set.  The return value is
		/// not affected by whether or not the read/write password of the
		/// container actually matches the value in the device's password
		/// register.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the password used by the API for
		/// reading from or writing to the device's memory has been set.
		/// </returns>
		virtual public bool ContainerReadWritePasswordSet
		{
			get
			{
				return readWritePasswordSet;
			}
			
		}
		/// <summary> Returns true if the password used by the API for writing to the
		/// device's memory has been set.  The return value is not affected by
		/// whether or not the write password of the container actually matches
		/// the value in the device's password register.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the password used by the API for
		/// writing to the device's memory has been set.
		/// </returns>
		virtual public bool ContainerWriteOnlyPasswordSet
		{
			get
			{
				throw new OneWireException("The DS1922 does not have a write only password");
			}
			
		}
		/// <summary> Returns true if the currently loaded mission results indicate
		/// that this mission has the SUTA bit enabled.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the currently loaded mission
		/// results indicate that this mission has the SUTA bit
		/// enabled.
		/// </returns>
		virtual public bool MissionSUTA
		{
			get
			{
				if (isMissionUploaded)
					return getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_START_MISSION_ON_TEMPERATURE_ALARM, missionRegister);
				else
					return getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_START_MISSION_ON_TEMPERATURE_ALARM);
			}
			
		}
		/// <summary> Returns true if the currently loaded mission results indicate
		/// that this mission has the SUTA bit enabled and is still
		/// Waiting For Temperature Alarm (WFTA).
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the currently loaded mission
		/// results indicate that this mission has the SUTA bit
		/// enabled and is still Waiting For Temperature Alarm (WFTA).
		/// </returns>
		virtual public bool MissionWFTA
		{
			get
			{
				// check for MIP=1 and SUTA=1 before returning value of WFTA.
				// if MIP=0 or SUTA=0, WFTA could be in invalid state if previous
				// mission did not get a temperature alarm.  Clear Memory should
				// clear this bit, so this is the workaround.
				if (MissionRunning && MissionSUTA)
				{
					if (isMissionUploaded)
						return getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_WAITING_FOR_TEMPERATURE_ALARM, missionRegister);
					else
						return getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_WAITING_FOR_TEMPERATURE_ALARM);
				}
				return false;
			}
			
		}

        /// <summary> Returns true if the mission results have been loaded from the device.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if the mission results have been loaded.
        /// </returns>
        virtual public bool isMissionLoaded()
        {
           return isMissionUploaded;

        }


		/// <summary> Gets the number of channels supported by this Missioning device.
		/// Channel specific methods will use a channel number specified
		/// by an integer from [0 to (<code>getNumberOfMissionChannels()</code> - 1)].
		/// 
		/// </summary>
		/// <returns> the number of channels
		/// </returns>
		virtual public int NumberMissionChannels
		{
			get
			{
				if (deviceConfigByte == DCB_DS1922L || deviceConfigByte == DCB_DS1922T || deviceConfigByte == DCB_DS1922E || deviceConfigByte == DCB_DS1922S)
					return 1;
				// temperature only
				else
					return 2; // temperature and data/voltage/humidity
			}
			
		}
		/// <summary> Returns <code>true</code> if a mission is currently running.</summary>
		/// <returns> <code>true</code> if a mission is currently running.
		/// </returns>
		virtual public bool MissionRunning
		{
			get
			{
				return getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MISSION_IN_PROGRESS);
			}
			
		}
		/// <summary> Returns <code>true</code> if a rollover is enabled.</summary>
		/// <returns> <code>true</code> if a rollover is enabled.
		/// </returns>
		virtual public bool MissionRolloverEnabled
		{
			get
			{
				if (isMissionUploaded)
					return getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_ROLLOVER, missionRegister);
				else
					return getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_ROLLOVER);
			}
			
		}
		/// <summary> Enables/Disables the usage of calibration registers.  Only
		/// applies to the DS1923 configuration.  The calibration depends
		/// on an average error at 3 known reference points.  This average
		/// error is written to 3 registers on the DS1922.  The container
		/// use these values to calibrate the recorded humidity values
		/// and improve the accuracy of the device.  This method allows you
		/// to turn off calibration so that you may download the actual
		/// data recorded to the device's memory and perform a manual
		/// calibration.
		/// 
		/// </summary>
		/// <param name="use">if <code>true</code>, all humidity values read from
		/// device will be calibrated.
		/// 
		/// </param>
		virtual public bool TemperatureCalibrationRegisterUsage
		{
			set
			{
				this.useTempCalibrationRegisters = value;
			}
			
		}
		/// <summary> Enables/Disables the usage of the humidity calibration registers.
		/// Only applies to the DS1923 configuration.  The calibration depends
		/// on an average error at 3 known reference points.  This average
		/// error is written to 3 registers on the DS1922.  The container
		/// use these values to calibrate the recorded humidity values
		/// and improve the accuracy of the device.  This method allows you
		/// to turn off calibration so that you may download the actual
		/// data recorded to the device's memory and perform a manual
		/// calibration.
		/// 
		/// </summary>
		/// <param name="use">if <code>true</code>, all humidity values read from
		/// device will be calibrated.
		/// </param>
		virtual public bool HumidityCalibrationRegisterUsage
		{
			set
			{
				this.useHumdCalibrationRegisters = value;
			}
			
		}
		/// <summary> Enables/Disables the usage of temperature compensation.  Only
		/// applies to the DS1923 configuration.  The temperature
		/// compensation adjusts the humidity values based on the known
		/// effects of temperature on the humidity sensor.  If this is
		/// a joint humidity and temperature mission, the temperature
		/// values used could (should?) come from the temperature log
		/// itself.  If, however, there is no temperature log the
		/// default temperature value can be set for the mission using
		/// the <code>setDefaultTemperatureCompensationValue</code> method.
		/// 
		/// </summary>
		/// <param name="use">if <code>true</code>, all humidity values read from
		/// device will be compensated for temperature.
		/// 
		/// </param>
		/// <seealso cref="setDefaultTemperatureCompensationValue">
		/// </seealso>
		virtual public bool TemperatureCompensationUsage
		{
			set
			{
				this.useTemperatureCompensation = value;
			}
			
		}
		/// <summary> Get an array of available temperature resolutions in Celsius.
		/// 
		/// </summary>
		/// <returns> byte array of available temperature resolutions in Celsius with
		/// minimum resolution as the first element and maximum resolution
		/// as the last element
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
				double[] d = new double[1];
				
				d[0] = temperatureResolutions[1];
				
				return d;
			}
			
		}
		/// <summary> Gets the temperature alarm resolution in Celsius.
		/// 
		/// </summary>
		/// <returns> temperature alarm resolution in Celsius for this 1-wire device
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
				return temperatureResolutions[0];
			}
			
		}
		/// <summary> Gets the maximum temperature in Celsius.
		/// 
		/// </summary>
		/// <returns> maximum temperature in Celsius for this 1-wire device
		/// 
		/// </returns>
		/// <seealso cref="getMinTemperature()">
		/// </seealso>
		virtual public double MaxTemperature
		{
			get
			{
				return temperatureRangeLow + temperatureRangeWidth;
			}
			
		}
		/// <summary> Gets the minimum temperature in Celsius.
		/// 
		/// </summary>
		/// <returns> minimum temperature in Celsius for this 1-wire device
		/// 
		/// </returns>
		/// <seealso cref="getMaxTemperature()">
		/// </seealso>
		virtual public double MinTemperature
		{
			get
			{
				return temperatureRangeLow;
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
				double[] d = new double[1];
				
				d[0] = humidityResolutions[1];
				
				return d;
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
				return humidityResolutions[0];
			}
			
		}
		/// <summary> Gets the number of channels supported by this A/D.
		/// Channel specific methods will use a channel number specified
		/// by an integer from [0 to (<code>getNumberADChannels()</code> - 1)].
		/// 
		/// </summary>
		/// <returns> the number of channels
		/// </returns>
		virtual public int NumberADChannels
		{
			// *****************************************************************************
			//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
			// A-to-D Interface Functions
			//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
			// *****************************************************************************
			
			get
			{
				return 1;
			}
			
		}
		virtual public double ADReferenceVoltage
		{
			get
			{
				return adReferenceVoltage;
			}
			
			set
			{
				adReferenceVoltage = value;
			}
			
		}
		virtual public int ADDeviceBitCount
		{
			get
			{
				return adDeviceBits;
			}
			
			set
			{
				if (value > 16)
					value = 16;
				if (value < 8)
					value = 8;
				adDeviceBits = value;
			}
			
		}
		virtual public bool ForceADResults
		{
			get
			{
				return adForceResults;
			}
			
			set
			{
				adForceResults = value;
			}
			
		}
		/// <summary> Gets the clock resolution in milliseconds
		/// 
		/// </summary>
		/// <returns> the clock resolution in milliseconds
		/// </returns>
		virtual public long ClockResolution
		{
			get
			{
				return 1000;
			}
			
		}
		/// <summary> Sets the following, calculated from the 12-bit code of the 1-Wire Net Address:
		/// 1)  The part numbers:
		/// DS1923 - Temperature/Humidity iButton
		/// DS1922L - Temperature iButton
		/// DS1922T - Extended Temperature iButton
		/// DS1i22S - Temperature/A-D iButton
		/// </summary>
		private byte[] ContainerVariables
		{
			set
			{
				//double Tref1 = 60;
                Tref1 = 60;
				bool autoLoadCalibration = true;
				
				// clear this flag..  Gets set later if registerPages!=null
				isContainerVariablesSet = false;
				
				// reset mission parameters
				hasHumiditySensor = false;
				isMissionUploaded = false;
				missionRegister = null;
				dataLog = null;
				temperatureLog = null;
				adReferenceVoltage = 5.02d;
				adDeviceBits = 10;
				adForceResults = false;
				
				
				deviceConfigByte = (byte) SupportClass.Identity(0xFF);
				if (value != null)
				{
					deviceConfigByte = value[DEVICE_CONFIGURATION_BYTE & 0x03F];
				}
				
				switch ((byte)deviceConfigByte)
				{
					
					case DCB_DS2422: 
						partNumber = PART_NUMBER_DS2422;
						temperatureRangeLow = - 40;
						temperatureRangeWidth = 125;
						Tref1 = 60;
						descriptionString = DESCRIPTION_DS2422;
						autoLoadCalibration = false;
						break;
					
					case DCB_DS1923: 
						partNumber = PART_NUMBER_DS1923;
						temperatureRangeLow = - 40;
						temperatureRangeWidth = 125;
						Tref1 = 60;
						hasHumiditySensor = true;
						descriptionString = DESCRIPTION_DS1923;
						break;
					
					case DCB_DS1922L: 
						partNumber = PART_NUMBER_DS1922L;
						temperatureRangeLow = - 40;
						temperatureRangeWidth = 125;
						Tref1 = 60;
						descriptionString = DESCRIPTION_DS1922;
						break;
					
					case DCB_DS1922T: 
						partNumber = PART_NUMBER_DS1922T;
						temperatureRangeLow = 0;
						temperatureRangeWidth = 125;
						Tref1 = 90;
						descriptionString = DESCRIPTION_DS1922;
						break;
					
					case DCB_DS1922E: 
						partNumber = PART_NUMBER_DS1922E;
						temperatureRangeLow = 15;
						temperatureRangeWidth = 125;
						descriptionString = DESCRIPTION_DS1922;
						break;

                    case DCB_DS1922F:                          
                        partNumber = PART_NUMBER_DS1922F;
                        temperatureRangeLow = 15; 
                        temperatureRangeWidth = 125;
                        Tref1 = 130; 
                        descriptionString = DESCRIPTION_DS1922; 
                        break;				
					
					case DCB_DS1922S: 
						partNumber = PART_NUMBER_DS1922S;
						temperatureRangeLow = - 40;
						temperatureRangeWidth = 125;
						Tref1 = 60;
						descriptionString = DESCRIPTION_DS1922 + "\r\n\r\n* Please note that the DS1922S can be missioned only once.";
						break;
					
					default: 
						partNumber = PART_NUMBER_UNKNOWN;
						temperatureRangeLow = - 40;
						temperatureRangeWidth = 125;
						Tref1 = 60;
						descriptionString = DESCRIPTION_UNKNOWN;
						autoLoadCalibration = false;
						break;
					
				}
				
				
				if (value != null)
				{
					isContainerVariablesSet = true;
					
					if (autoLoadCalibration)
					{
						// if humidity device, calculate the calibration coefficients
						if (hasHumiditySensor)
						{
							useHumdCalibrationRegisters = true;
							
							// DEBUG: Product samples were sent out uncalibrated.  This flag
							// allows the customer to not use the temperature calibration
                           /*
							System.String useHumdCal = OneWireAccessProvider.getProperty("DS1923.useHumidityCalibrationRegisters");
							if (useHumdCal != null && useHumdCal.ToLower().Equals("false"))
							{
								useHumdCalibrationRegisters = false;
								if (DEBUG)
								{
									Debug.debug("DEBUG: Disabling Humidity Calibration Usage in Container");
								}
							}
							*/
							Href1 = decodeHumidity(value, 0x48, 2, true);
							Hread1 = decodeHumidity(value, 0x4A, 2, true);
							Herror1 = Hread1 - Href1;
							Href2 = decodeHumidity(value, 0x4C, 2, true);
							Hread2 = decodeHumidity(value, 0x4E, 2, true);
							Herror2 = Hread2 - Href2;
							Href3 = decodeHumidity(value, 0x50, 2, true);
							Hread3 = decodeHumidity(value, 0x52, 2, true);
							Herror3 = Hread3 - Href3;
							
							double Href1sq = Href1 * Href1;
							double Href2sq = Href2 * Href2;
							double Href3sq = Href3 * Href3;
							humdCoeffB = ((Href2sq - Href1sq) * (Herror3 - Herror1) + Href3sq * (Herror1 - Herror2) + Href1sq * (Herror2 - Herror1)) / ((Href2sq - Href1sq) * (Href3 - Href1) + (Href3sq - Href1sq) * (Href1 - Href2));
							humdCoeffA = (Herror2 - Herror1 + humdCoeffB * (Href1 - Href2)) / (Href2sq - Href1sq);
							humdCoeffC = Herror1 - humdCoeffA * Href1sq - humdCoeffB * Href1;
						}
						
						useTempCalibrationRegisters = true;
						
						// DEBUG: Product samples were sent out uncalibrated.  This flag
						// allows the customer to not use the temperature calibration
                       /*
						System.String useTempCal = OneWireAccessProvider.getProperty("DS1923.useTemperatureCalibrationRegisters");
						if (useTempCal != null && useTempCal.ToLower().Equals("false"))
						{
							useTempCalibrationRegisters = false;
							if (DEBUG)
							{
								Debug.debug("DEBUG: Disabling Temperature Calibration Usage in Container");
							}
						}
						*/
						//if (partNumber == PART_NUMBER_DS1922E)
						//	useTempCalibrationRegisters = false; // !!!
						
						Tref2 = decodeTemperature(value, 0x40, 2, true);
						Tread2 = decodeTemperature(value, 0x42, 2, true);
						Terror2 = Tread2 - Tref2;
						Tref3 = decodeTemperature(value, 0x44, 2, true);
						Tread3 = decodeTemperature(value, 0x46, 2, true);
						Terror3 = Tread3 - Tref3;
						Terror1 = Terror2;
						Tread1 = Tref1 + Terror1;
						
						if (DEBUG)
						{
							Debug.debug("Tref1=" + Tref1);
							Debug.debug("Tread1=" + Tread1);
							Debug.debug("Terror1=" + Terror1);
							Debug.debug("Tref2=" + Tref2);
							Debug.debug("Tread2=" + Tread2);
							Debug.debug("Terror2=" + Terror2);
							Debug.debug("Tref3=" + Tref3);
							Debug.debug("Tread3=" + Tread3);
							Debug.debug("Terror3=" + Terror3);
						}
						
						double Tref1sq = Tref1 * Tref1;
						double Tref2sq = Tref2 * Tref2;
						double Tref3sq = Tref3 * Tref3;
						tempCoeffB = ((Tref2sq - Tref1sq) * (Terror3 - Terror1) + Tref3sq * (Terror1 - Terror2) + Tref1sq * (Terror2 - Terror1)) / ((Tref2sq - Tref1sq) * (Tref3 - Tref1) + (Tref3sq - Tref1sq) * (Tref1 - Tref2));
						tempCoeffA = (Terror2 - Terror1 + tempCoeffB * (Tref1 - Tref2)) / (Tref2sq - Tref1sq);
						tempCoeffC = Terror1 - tempCoeffA * Tref1sq - tempCoeffB * Tref1;
					}
				}
			}
			
		}
		// enables/disables debugging
		private const bool DEBUG = false;
		
		// when reading a page, the memory bank may throw a crc exception if the device
		// is sampling or starts sampling during the read.  This value sets how many
		// times the device retries before passing the exception on to the application.
		private const int MAX_READ_RETRY_CNT = 10;
		
		// the length of the Read-Only and Read/Write password registers
		private const int PASSWORD_LENGTH = 8;
		
		// indicates whether or not the device configuration has been read
		// and all the ranges for the part have been set.
		private bool isContainerVariablesSet = false;
		
		// memory bank for scratchpad
		private MemoryBankScratchCRCPW scratch = null;
		// memory bank for general-purpose user data
		private MemoryBankNVCRCPW userDataMemory = null;
		// memory bank for control register
		private MemoryBankNVCRCPW register = null;
		// memory bank for mission log
		private MemoryBankNVCRCPW log = null;
		
		// Maxim/Maxim Integrated Products Part number
		private System.String partNumber = null;
		
		// Device Configuration Byte
		private byte deviceConfigByte = (byte) SupportClass.Identity(0xFF);
		
		// Temperature range low temperaturein degrees Celsius
		private double temperatureRangeLow = - 40.0;
		
		// Temperature range width in degrees Celsius
		private double temperatureRangeWidth = 125.0;
		
		// Temperature resolution in degrees Celsius
		// private double temperatureResolution = 0.5; // value is never used.
		
		// A-D Reference voltage
		private double adReferenceVoltage = 5.02d;
		// Number of valid bits in A-D Result
		private int adDeviceBits = 10;
		// Force mission results to return value as A-D, not humidity
		private bool adForceResults = false;
		
		// should we update the Real time clock?
		private bool updatertc = false;
		
		// should we check the speed
		private bool doSpeedEnable = true;
		
		/// <summary>The current password for readingfrom this device.</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'readPassword '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private byte[] readPassword = new byte[8];
		private bool readPasswordSet = false;
		private bool readOnlyPasswordEnabled = false;
		
		/// <summary>The current password for reading/writing from/to this device. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'readWritePassword '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private byte[] readWritePassword = new byte[8];
		private bool readWritePasswordSet = false;
		private bool readWritePasswordEnabled = false;
		
		/// <summary>indicates whether or not the results of a mission are successfully loaded </summary>
		private bool isMissionUploaded = false;
		/// <summary>holds the missionRegister, which details the status of the current mission </summary>
		private byte[] missionRegister = null;
		/// <summary>The mission logs </summary>
		private byte[] dataLog = null, temperatureLog = null;
		/// <summary>Number of bytes used to store temperature values (0, 1, or 2) </summary>
		private int temperatureBytes = 0;
		/// <summary>Number of bytes used to stroe data valuas (0, 1, or 2) </summary>
		private int dataBytes = 0;
		/// <summary>indicates whether or not the log has rolled over </summary>
		private bool rolledOver = false;
		/// <summary>start time offset for the first sample, if rollover occurred </summary>
		private int timeOffset = 0;
		/// <summary>the time (unix time) when mission started </summary>
		private long missionTimeStamp = - 1;
		/// <summary>The rate at which samples are taken, and the number of samples </summary>
		private int sampleRate = - 1, sampleCount = - 1;
		/// <summary>total number of samples, including rollover </summary>
		private int sampleCountTotal;
		
		// indicates whether or not to use calibration for the humidity values
		private bool useHumdCalibrationRegisters = false;
		// reference humidities that the calibration was calculated over
		private double Href1 = 20, Href2 = 60, Href3 = 90;
		// the average value for each reference point
		private double Hread1 = 0, Hread2 = 0, Hread3 = 0;
		// the average error for each reference point
		private double Herror1 = 0, Herror2 = 0, Herror3 = 0;
		// the coefficients for calibration
		private double humdCoeffA, humdCoeffB, humdCoeffC;
		
		// indicates whether or not to use calibration for the temperature values
		private bool useTempCalibrationRegisters = false;
		// reference temperatures that the calibration was calculated over
        private double Tref1 = 0;
		private double Tref2 = 0, Tref3 = 0;
		// the average value for each reference point
		private double Tread1 = 0, Tread2 = 0, Tread3 = 0;
		// the average error for each reference point
		private double Terror1 = 0, Terror2 = 0, Terror3 = 0;
		// the coefficients for calibration of temperature
		private double tempCoeffA, tempCoeffB, tempCoeffC;
		
		// indicates whether or not to temperature compensate the humidity values
		private bool useTemperatureCompensation = false;
		// indicates whether or not to use the temperature log for compensation
		private bool overrideTemperatureLog = false;
		// default temperature in case of no log or override log
		private double defaultTempCompensationValue = 25;
		
		// indicates whether or not this is a DS1923
		private bool hasHumiditySensor = false;
		
		
		// temperature is 8-bit or 11-bit
		//UPGRADE_NOTE: Final was removed from the declaration of 'temperatureResolutions '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly double[] temperatureResolutions = new double[]{.5d, .0625d};
		// data is 10-bit or 16-bit
		//UPGRADE_NOTE: Final was removed from the declaration of 'dataResolutions '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly double[] dataResolutions = new double[]{.5d, 0.001953125};
		//UPGRADE_NOTE: Final was removed from the declaration of 'humidityResolutions '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly double[] humidityResolutions = new double[]{.6d, .04d};
		
		private System.String descriptionString = DESCRIPTION_UNKNOWN;
		
		// first year that calendar starts counting years from
		private const int FIRST_YEAR_EVER = 2000;
		
		// used to 'enable' passwords
		private static byte ENABLE_BYTE = (byte) SupportClass.Identity(0xAA);
		// used to 'disable' passwords
		private const byte DISABLE_BYTE = (byte) (0x00);
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// 1-Wire Commands
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		/// <summary>Refers to the Temperature Channel for this device </summary>
		public const int TEMPERATURE_CHANNEL = 0;
		/// <summary>Refers to the Humidity/A-D Channel for this device </summary>
		public const int DATA_CHANNEL = 1;
		
		/// <summary>1-Wire command for Write Scratchpad </summary>
		public static byte WRITE_SCRATCHPAD_COMMAND = (byte) 0x0F;
		/// <summary>1-Wire command for Read Scratchpad </summary>
		public static byte READ_SCRATCHPAD_COMMAND = (byte) SupportClass.Identity(0xAA);
		/// <summary>1-Wire command for Copy Scratchpad With Password </summary>
		public static byte COPY_SCRATCHPAD_PW_COMMAND = (byte) SupportClass.Identity(0x99);
		/// <summary>1-Wire command for Read Memory CRC With Password </summary>
		public static byte READ_MEMORY_CRC_PW_COMMAND = (byte) 0x69;
		/// <summary>1-Wire command for Clear Memory With Password </summary>
		public static byte CLEAR_MEMORY_PW_COMMAND = (byte) SupportClass.Identity(0x96);
		/// <summary>1-Wire command for Start Mission With Password </summary>
		public static byte START_MISSION_PW_COMMAND = (byte) SupportClass.Identity(0xCC);
		/// <summary>1-Wire command for Stop Mission With Password </summary>
		public static byte STOP_MISSION_PW_COMMAND = (byte) 0x33;
		/// <summary>1-Wire command for Forced Conversion </summary>
		public static byte FORCED_CONVERSION = (byte) 0x55;
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Register addresses and control bits
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		/// <summary>Address of the Real-time Clock Time value</summary>
		public const int RTC_TIME = 0x200;
		/// <summary>Address of the Real-time Clock Date value</summary>
		public const int RTC_DATE = 0x203;
		
		/// <summary>Address of the Sample Rate Register </summary>
		public const int SAMPLE_RATE = 0x206; // 2 bytes, LSB first, MSB no greater than 0x3F
		
		/// <summary>Address of the Temperature Low Alarm Register </summary>
		public const int TEMPERATURE_LOW_ALARM_THRESHOLD = 0x208;
		/// <summary>Address of the Temperature High Alarm Register </summary>
		public const int TEMPERATURE_HIGH_ALARM_THRESHOLD = 0x209;
		
		/// <summary>Address of the Data Low Alarm Register </summary>
		public const int DATA_LOW_ALARM_THRESHOLD = 0x20A;
		/// <summary>Address of the Data High Alarm Register </summary>
		public const int DATA_HIGH_ALARM_THRESHOLD = 0x20B;
		
		/// <summary>Address of the last temperature conversion's LSB </summary>
		public const int LAST_TEMPERATURE_CONVERSION_LSB = 0x20C;
		/// <summary>Address of the last temperature conversion's MSB </summary>
		public const int LAST_TEMPERATURE_CONVERSION_MSB = 0x20D;
		
		/// <summary>Address of the last data conversion's LSB </summary>
		public const int LAST_DATA_CONVERSION_LSB = 0x20E;
		/// <summary>Address of the last data conversion's MSB </summary>
		public const int LAST_DATA_CONVERSION_MSB = 0x20F;
		
		/// <summary>Address of Temperature Control Register </summary>
		public const int TEMPERATURE_CONTROL_REGISTER = 0x210;
		/// <summary>Temperature Control Register Bit: Enable Data Low Alarm </summary>
		public static byte TCR_BIT_ENABLE_TEMPERATURE_LOW_ALARM = (byte) 0x01;
		/// <summary>Temperature Control Register Bit: Enable Data Low Alarm </summary>
		public static byte TCR_BIT_ENABLE_TEMPERATURE_HIGH_ALARM = (byte) 0x02;
		
		/// <summary>Address of Data Control Register </summary>
		public const int DATA_CONTROL_REGISTER = 0x211;
		/// <summary>Data Control Register Bit: Enable Data Low Alarm </summary>
		public static byte DCR_BIT_ENABLE_DATA_LOW_ALARM = (byte) 0x01;
		/// <summary>Data Control Register Bit: Enable Data High Alarm </summary>
		public static byte DCR_BIT_ENABLE_DATA_HIGH_ALARM = (byte) 0x02;
		
		/// <summary>Address of Real-Time Clock Control Register </summary>
		public const int RTC_CONTROL_REGISTER = 0x212;
		/// <summary>Real-Time Clock Control Register Bit: Enable Oscillator </summary>
		public static byte RCR_BIT_ENABLE_OSCILLATOR = (byte) 0x01;
		/// <summary>Real-Time Clock Control Register Bit: Enable High Speed Sample </summary>
		public static byte RCR_BIT_ENABLE_HIGH_SPEED_SAMPLE = (byte) 0x02;
		
		/// <summary>Address of Mission Control Register </summary>
		public static int MISSION_CONTROL_REGISTER = (byte) SupportClass.Identity(0x213);
		/// <summary>Mission Control Register Bit: Enable Temperature Logging </summary>
		public static byte MCR_BIT_ENABLE_TEMPERATURE_LOGGING = (byte) 0x01;
		/// <summary>Mission Control Register Bit: Enable Data Logging </summary>
		public static byte MCR_BIT_ENABLE_DATA_LOGGING = (byte) 0x02;
		/// <summary>Mission Control Register Bit: Set Temperature Resolution </summary>
		public static byte MCR_BIT_TEMPERATURE_RESOLUTION = (byte) 0x04;
		/// <summary>Mission Control Register Bit: Set Data Resolution </summary>
		public static byte MCR_BIT_DATA_RESOLUTION = (byte) 0x08;
		/// <summary>Mission Control Register Bit: Enable Rollover </summary>
		public static byte MCR_BIT_ENABLE_ROLLOVER = (byte) 0x10;
		/// <summary>Mission Control Register Bit: Start Mission on Temperature Alarm </summary>
		public static byte MCR_BIT_START_MISSION_ON_TEMPERATURE_ALARM = (byte) 0x20;
		
		/// <summary>Address of Alarm Status Register </summary>
		public const int ALARM_STATUS_REGISTER = 0x214;
		/// <summary>Alarm Status Register Bit: Temperature Low Alarm </summary>
		public static byte ASR_BIT_TEMPERATURE_LOW_ALARM = (byte) 0x01;
		/// <summary>Alarm Status Register Bit: Temperature High Alarm </summary>
		public static byte ASR_BIT_TEMPERATURE_HIGH_ALARM = (byte) 0x02;
		/// <summary>Alarm Status Register Bit: Data Low Alarm </summary>
		public static byte ASR_BIT_DATA_LOW_ALARM = (byte) 0x04;
		/// <summary>Alarm Status Register Bit: Data High Alarm </summary>
		public static byte ASR_BIT_DATA_HIGH_ALARM = (byte) 0x08;
		/// <summary>Alarm Status Register Bit: Battery On Reset </summary>
		public static byte ASR_BIT_BATTERY_ON_RESET = (byte) SupportClass.Identity(0x80);
		
		/// <summary>Address of General Status Register </summary>
		public const int GENERAL_STATUS_REGISTER = 0x215;
		/// <summary>General Status Register Bit: Sample In Progress </summary>
		public static byte GSR_BIT_SAMPLE_IN_PROGRESS = (byte) 0x01;
		/// <summary>General Status Register Bit: Mission In Progress </summary>
		public static byte GSR_BIT_MISSION_IN_PROGRESS = (byte) 0x02;
		/// <summary>General Status Register Bit: Conversion In Progress </summary>
		public static byte GSR_BIT_CONVERSION_IN_PROGRESS = (byte) 0x04;
		/// <summary>General Status Register Bit: Memory Cleared </summary>
		public static byte GSR_BIT_MEMORY_CLEARED = (byte) 0x08;
		/// <summary>General Status Register Bit: Waiting for Temperature Alarm </summary>
		public static byte GSR_BIT_WAITING_FOR_TEMPERATURE_ALARM = (byte) 0x10;
		/// <summary>General Status Register Bit: Forced Conversion In Progress </summary>
		public static byte GSR_BIT_FORCED_CONVERSION_IN_PROGRESS = (byte) 0x20;
		
		/// <summary>Address of the Mission Start Delay </summary>
		public const int MISSION_START_DELAY = 0x216; // 3 bytes, LSB first
		
		/// <summary>Address of the Mission Timestamp Time value</summary>
		public const int MISSION_TIMESTAMP_TIME = 0x219;
		/// <summary>Address of the Mission Timestamp Date value</summary>
		public const int MISSION_TIMESTAMP_DATE = 0x21C;
		
		/// <summary>Address of Device Configuration Register </summary>
		public const int DEVICE_CONFIGURATION_BYTE = 0x226;
		/// <summary>Value of Device Configuration Register for DS1922S </summary>
		public const int DCB_DS2422 = 0x00;
		/// <summary>Value of Device Configuration Register for DS1923 </summary>
		public const int DCB_DS1923 = 0x20;
		/// <summary>Value of Device Configuration Register for DS1922L </summary>
		public const int DCB_DS1922L = 0x40;
		/// <summary>Value of Device Configuration Register for DS1922T </summary>
		public const int DCB_DS1922T = 0x60;
		/// <summary>Value of Device Configuration Register for DS1922E </summary>
		public const int DCB_DS1922E = 0x80;
        /// <summary>Value of Device Configuration Register for DS1922F </summary>
        public const int DCB_DS1922F = 0xC0;	
		/// <summary>Value of Device Configuration Register for DS1922S </summary>
		public const int DCB_DS1922S = 0xA0;
		
		
		// 1 byte, alternating ones and zeroes indicates passwords are enabled
		/// <summary>Address of the Password Control Register. </summary>
		public const int PASSWORD_CONTROL_REGISTER = 0x227;
		
		// 8 bytes, write only, for setting the Read Access Password
		/// <summary>Address of Read Access Password. </summary>
		public const int READ_ACCESS_PASSWORD = 0x228;
		
		// 8 bytes, write only, for setting the Read Access Password
		/// <summary>Address of the Read Write Access Password. </summary>
		public const int READ_WRITE_ACCESS_PASSWORD = 0x230;
		
		// 3 bytes, LSB first
		/// <summary>Address of the Mission Sample Count </summary>
		public const int MISSION_SAMPLE_COUNT = 0x220;
		
		// 3 bytes, LSB first
		/// <summary>Address of the Device Sample Count </summary>
		public const int DEVICE_SAMPLE_COUNT = 0x223;
		
		/// <summary>maximum size of the mission log </summary>
		public const int MISSION_LOG_SIZE = 8192;
		
		/// <summary> mission log size for odd combination of resolutions (i.e. 8-bit temperature
		/// & 16-bit data or 16-bit temperature & 8-bit data
		/// </summary>
		public const int ODD_MISSION_LOG_SIZE = 7680;
		
		private const System.String PART_NUMBER_DS1923 = "DS1923";
		private const System.String PART_NUMBER_DS2422 = "DS2422";
		private const System.String PART_NUMBER_DS1922L = "DS1922L";
		private const System.String PART_NUMBER_DS1922T = "DS1922T";
		private const System.String PART_NUMBER_DS1922E = "DS1922E";
        private const System.String PART_NUMBER_DS1922F = "DS1922F";
		private const System.String PART_NUMBER_DS1922S = "DS1922S";
		private const System.String PART_NUMBER_UNKNOWN = "DS1922/DS1923/DS2422";
		
		private const System.String DESCRIPTION_DS1923 = "The DS1923 Temperature/Humidity Logger iButton is a rugged, " + "self-sufficient system that measures temperature and/or humidity " + "and records the result in a protected memory section. The recording " + "is done at a user-defined rate. A total of 8192 8-bit readings or " + "4096 16-bit readings taken at equidistant intervals ranging from 1 " + "second to 273 hours can be stored. In addition to this, there are 512 " + "bytes of SRAM for storing application specific information and 64 " + "bytes for calibration data. A mission to collect data can be " + "programmed to begin immediately, or after a user-defined delay or " + "after a temperature alarm. Access to the memory and control functions " + "can be password-protected.";
		
		private const System.String DESCRIPTION_DS1922 = "The DS1922L/T/E/F/S Temperature Logger iButtons are rugged, " + "self-sufficient systems that measure temperature and record the " + "result in a protected memory section. The recording is done at a " + "user-defined rate. A total of 8192 8-bit readings or 4096 16-bit " + "readings taken at equidistant intervals ranging from 1s to 273hrs " + "can be stored. In addition to this, there are 512 bytes of SRAM for " + "storing application-specific information and 64 bytes for calibration " + "data. A mission to collect data can be programmed to begin " + "immediately, or after a user-defined delay or after a temperature " + "alarm. Access to the memory and control functions can be password " + "protected.";
		
		private const System.String DESCRIPTION_DS2422 = "The DS2422 temperature/datalogger combines the core functions of a " + "fully featured datalogger in a single chip. It includes a temperature " + "sensor, realtime clock (RTC), memory, 1-Wire® interface, and serial " + "interface for an analog-to-digital converter (ADC) as well as control " + "circuitry for a charge pump. The ADC and the charge pump are " + "peripherals that can be added to build application-specific " + "dataloggers. The MAX1086 is an example of a compatible serial ADC. " + "Without external ADC, the DS2422 functions as a temperature logger " + "only. The DS2422 measures the temperature and/or reads the ADC at a " + "user-defined rate. A total of 8192 8-bit readings or 4096 16-bit " + "readings taken at equidistant intervals ranging from 1s to 273hrs can " + "be stored.";
		
		private const System.String DESCRIPTION_UNKNOWN = "Rugged, self-sufficient 1-Wire device that, once setup for " + "a mission, will measure temperature and A-to-D/Humidity, with the " + "result recorded in a protected memory section. It stores up " + "to 8192 1-byte measurements, which can be filled with 1- or " + "2-byte temperature readings and 1- or 2-byte A-to-D/Humidity readings " + "taken at a user-specified rate.";
		
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Constructors and Initializers
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a
		/// DS1922.
		/// Note that the method <code>setupContainer(DSPortAdapter,byte[])</code>
		/// must be called to set the correct <code>DSPortAdapter</code> device address.
		/// 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer.setupContainer(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) setupContainer(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer41(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) OneWireContainer41(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer41(com.dalsemi.onewire.adapter.DSPortAdapter,long)   OneWireContainer41(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer41(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer41(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer41():base()
		{
			// initialize the memory banks
			initMem();
			ContainerVariables = null;
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a
		/// DS1922.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       address of this DS1922
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer41()">
		/// </seealso>
		/// <seealso cref="OneWireContainer41(com.dalsemi.onewire.adapter.DSPortAdapter,long)   OneWireContainer41(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer41(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer41(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer41(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
			ContainerVariables = null;
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a
		/// DS1922.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       address of this DS1922
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer41()">
		/// </seealso>
		/// <seealso cref="OneWireContainer41(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) OneWireContainer41(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer41(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer41(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer41(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
			ContainerVariables = null;
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a
		/// DS1922.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       address of this DS1922
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer41()">
		/// </seealso>
		/// <seealso cref="OneWireContainer41(com.dalsemi.onewire.adapter.DSPortAdapter,long) OneWireContainer41(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer41(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer41(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer41(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
			ContainerVariables = null;
		}
		
		/// <summary> Provides this container with the adapter object used to access this device and
		/// the address of the iButton or 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override void  setupContainer(DSPortAdapter sourceAdapter, byte[] newAddress)
		{
			base.setupContainer(sourceAdapter, newAddress);
			
			// initialize the memory banks
			initMem();
			ContainerVariables = null;
		}
		
		/// <summary> Provides this container with the adapter object used to access this device and
		/// the address of the iButton or 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override void  setupContainer(DSPortAdapter sourceAdapter, long newAddress)
		{
			base.setupContainer(sourceAdapter, newAddress);
			
			// initialize the memory banks
			initMem();
			ContainerVariables = null;
		}
		
		/// <summary> Provides this container with the adapter object used to access this device and
		/// the address of the iButton or 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override void  setupContainer(DSPortAdapter sourceAdapter, System.String newAddress)
		{
			base.setupContainer(sourceAdapter, newAddress);
			
			// initialize the memory banks
			initMem();
			ContainerVariables = null;
		}
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Sensor read/write
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
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
			byte[] buffer = new byte[96];
			
			int retryCnt = MAX_READ_RETRY_CNT;
			int page = 0;
			do 
			{
				try
				{
					switch (page)
					{
						
						default: 
							break;
						
						
						case 0: 
							register.readPageCRC(0, false, buffer, 0);
							page++;
							goto case 1;
						
						case 1: 
							register.readPageCRC(1, retryCnt == MAX_READ_RETRY_CNT, buffer, 32);
							page++;
							goto case 2;
						
						case 2: 
							register.readPageCRC(2, retryCnt == MAX_READ_RETRY_CNT, buffer, 64);
							page++;
							break;
						}
					retryCnt = MAX_READ_RETRY_CNT;
				}
				catch (OneWireIOException owioe)
				{
					//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
					if (DEBUG)
						Debug.debug("readDevice exc, retryCnt=" + retryCnt, owioe);
					//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
					/*
					// this "workaround" is broken.  Idea was based on suggestion
					// that scratchpad and data memory had different buses.  So,
					// Should be possible to read scratchpad while data memory is
					// written by logger. Experiments show this isn't true.
					try
					{
					scratch.readPageCRC(0, false, buffer, 0);
					}
					catch(Exception e)
					{
					throw new OneWireIOException("Invalid CRC16 read from device, battery may be dead.");
					}*/
					if (--retryCnt == 0)
						throw owioe;
				}
				catch (OneWireException owe)
				{
					//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
					if (DEBUG)
						Debug.debug("readDevice exc, retryCnt=" + retryCnt, owe);
					//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
					if (--retryCnt == 0)
						throw owe;
				}
			}
			while (page < 3);
			
			if (!isContainerVariablesSet)
				ContainerVariables = buffer;
			
			return buffer;
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
			int start = updatertc?0:6;
			
			register.write(start, state, start, 32 - start);
			
			lock (this)
			{
				updatertc = false;
			}
		}
		
		/// <summary> Reads a single byte from the DS1922.  Note that the preferred manner
		/// of reading from the DS1922 Thermocron is through the <code>readDevice()</code>
		/// method or through the <code>MemoryBank</code> objects returned in the
		/// <code>getMemoryBanks()</code> method.
		/// 
		/// </summary>
		/// <param name="memAddr">the address to read from  (in the range of 0x200-0x21F)
		/// 
		/// </param>
		/// <returns> the data byte read
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// 
		/// </summary>
		/// <seealso cref="readDevice()">
		/// </seealso>
		/// <seealso cref="getMemoryBanks()">
		/// </seealso>
		public virtual byte readByte(int memAddr)
		{
			// break the address up into bytes
			byte msbAddress = (byte) ((memAddr >> 8) & 0x0ff);
			byte lsbAddress = (byte) (memAddr & 0x0ff);
			
			/* check the validity of the address */
			if ((msbAddress > 0x2F) || (msbAddress < 0))
				throw new System.ArgumentException("OneWireContainer41-Address for read out of range.");
			
			int numBytesToEndOfPage = 32 - (lsbAddress & 0x1F);
			byte[] buffer = new byte[11 + numBytesToEndOfPage + 2];
			
			if (doSpeedEnable)
				doSpeed();
			
			if (adapter.select(address))
			{
				buffer[0] = READ_MEMORY_CRC_PW_COMMAND;
				buffer[1] = lsbAddress;
				buffer[2] = msbAddress;
				
				if (ContainerReadWritePasswordSet)
					getContainerReadWritePassword(buffer, 3);
				else
					getContainerReadOnlyPassword(buffer, 3);
				
				for (int i = 11; i < buffer.Length; i++)
					buffer[i] = (byte) SupportClass.Identity(0x0ff);
				
				//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
				if (DEBUG)
					Debug.debug("Send-> ", buffer);
				//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
				adapter.dataBlock(buffer, 0, buffer.Length);
				//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
				if (DEBUG)
					Debug.debug("Recv<- ", buffer);
				//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
				
				// exclude password from CRC 16
				if (CRC16.compute(buffer, 11, buffer.Length - 11, CRC16.compute(buffer, 0, 3, 0)) != 0x0000B001)
					throw new OneWireIOException("Invalid CRC16 read from device.  Password may be incorrect or a sample may be in progress.");
				
				return buffer[11];
			}
			else
				throw new OneWireException("OneWireContainer41-Device not present.");
		}
		
		/// <summary> <p>Gets the status of the specified flag from the specified register.
		/// This method actually communicates with the DS1922.  To improve
		/// performance if you intend to make multiple calls to this method,
		/// first call <code>readDevice()</code> and use the
		/// <code>getFlag(int, byte, byte[])</code> method instead.</p>
		/// 
		/// <p>The DS1922 has several sets of flags.</p>
		/// <ul>
		/// <LI>Register: <CODE> TEMPERATURE_CONTROL_REGISTER </CODE><BR>
		/// Flags:
		/// <UL>
		/// <li><code> TCR_BIT_ENABLE_TEMPERATURE_LOW_ALARM  </code></li>
		/// <li><code> TCR_BIT_ENABLE_TEMPERATURE_HIGH_ALARM </code></li>
		/// </UL>
		/// </LI>
		/// <LI>Register: <CODE> DATA_CONTROL_REGISTER </CODE><BR>
		/// Flags:
		/// <UL>
		/// <li><code> DCR_BIT_ENABLE_DATA_LOW_ALARM  </code></li>
		/// <li><code> DCR_BIT_ENABLE_DATA_HIGH_ALARM </code></li>
		/// </UL>
		/// </LI>
		/// <LI>Register: <CODE> RTC_CONTROL_REGISTER </CODE><BR>
		/// Flags:
		/// <UL>
		/// <li><code> RCR_BIT_ENABLE_OSCILLATOR        </code></li>
		/// <li><code> RCR_BIT_ENABLE_HIGH_SPEED_SAMPLE </code></li>
		/// </UL>
		/// </LI>
		/// <LI>Register: <CODE> MISSION_CONTROL_REGISTER </CODE><BR>
		/// Flags:
		/// <UL>
		/// <li><code> MCR_BIT_ENABLE_TEMPERATURE_LOGGING           </code></li>
		/// <li><code> MCR_BIT_ENABLE_DATA_LOGGING                  </code></li>
		/// <li><code> MCR_BIT_TEMPERATURE_RESOLUTION               </code></li>
		/// <li><code> MCR_BIT_DATA_RESOLUTION                      </code></li>
		/// <li><code> MCR_BIT_ENABLE_ROLLOVER                      </code></li>
		/// <li><code> MCR_BIT_START_MISSION_UPON_TEMPERATURE_ALARM </code></li>
		/// </UL>
		/// </LI>
		/// <LI>Register: <CODE> ALARM_STATUS_REGISTER </CODE><BR>
		/// Flags:
		/// <UL>
		/// <li><code> ASR_BIT_TEMPERATURE_LOW_ALARM  </code></li>
		/// <li><code> ASR_BIT_TEMPERATURE_HIGH_ALARM </code></li>
		/// <li><code> ASR_BIT_DATA_LOW_ALARM         </code></li>
		/// <li><code> ASR_BIT_DATA_HIGH_ALARM        </code></li>
		/// <li><code> ASR_BIT_BATTERY_ON_RESET       </code></li>
		/// </UL>
		/// </LI>
		/// <LI>Register: <CODE> GENERAL_STATUS_REGISTER </CODE><BR>
		/// Flags:
		/// <UL>
		/// <li><code> GSR_BIT_SAMPLE_IN_PROGRESS            </code></li>
		/// <li><code> GSR_BIT_MISSION_IN_PROGRESS           </code></li>
		/// <li><code> GSR_BIT_MEMORY_CLEARED                </code></li>
		/// <li><code> GSR_BIT_WAITING_FOR_TEMPERATURE_ALARM </code></li>
		/// </UL>
		/// </LI>
		/// </ul>
		/// 
		/// </summary>
		/// <param name="register">address of register containing the flag (see above for available options)
		/// </param>
		/// <param name="bitMask">the flag to read (see above for available options)
		/// 
		/// </param>
		/// <returns> the status of the flag, where <code>true</code>
		/// signifies a "1" and <code>false</code> signifies a "0"
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// 
		/// </summary>
		/// <seealso cref="getFlag(int,byte,byte[])">
		/// </seealso>
		/// <seealso cref="readDevice()">
		/// </seealso>
		/// <seealso cref="setFlag(int,byte,boolean)">
		/// </seealso>
		public virtual bool getFlag(int register, byte bitMask)
		{
			int retryCnt = MAX_READ_RETRY_CNT;
			while (true)
			{
				try
				{
					return ((readByte(register) & bitMask) != 0);
				}
				catch (OneWireException owe)
				{
					if (--retryCnt == 0)
						throw owe;
				}
			}
		}
		
		/// <summary> <p>Gets the status of the specified flag from the specified register.
		/// This method is the preferred manner of reading the control and
		/// status flags.</p>
		/// 
		/// <p>For more information on valid values for the <code>bitMask</code>
		/// parameter, see the {@link #getFlag(int,byte) getFlag(int,byte)} method.</p>
		/// 
		/// </summary>
		/// <param name="register">address of register containing the flag (see
		/// {@link #getFlag(int,byte) getFlag(int,byte)} for available options)
		/// </param>
		/// <param name="bitMask">the flag to read (see {@link #getFlag(int,byte) getFlag(int,byte)}
		/// for available options)
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> the status of the flag, where <code>true</code>
		/// signifies a "1" and <code>false</code> signifies a "0"
		/// 
		/// </returns>
		/// <seealso cref="getFlag(int,byte)">
		/// </seealso>
		/// <seealso cref="readDevice()">
		/// </seealso>
		/// <seealso cref="setFlag(int,byte,boolean,byte[])">
		/// </seealso>
		public virtual bool getFlag(int register, byte bitMask, byte[] state)
		{
			return ((state[register & 0x3F] & bitMask) != 0);
		}
		
		/// <summary> <p>Sets the status of the specified flag in the specified register.
		/// If a mission is in progress a <code>OneWireIOException</code> will be thrown
		/// (one cannot write to the registers while a mission is commencing).  This method
		/// actually communicates with the DS1922.  To improve
		/// performance if you intend to make multiple calls to this method,
		/// first call <code>readDevice()</code> and use the
		/// <code>setFlag(int,byte,boolean,byte[])</code> method instead.</p>
		/// 
		/// <p>For more information on valid values for the <code>bitMask</code>
		/// parameter, see the {@link #getFlag(int,byte) getFlag(int,byte)} method.</p>
		/// 
		/// </summary>
		/// <param name="register">address of register containing the flag (see
		/// {@link #getFlag(int,byte) getFlag(int,byte)} for available options)
		/// </param>
		/// <param name="bitMask">the flag to read (see {@link #getFlag(int,byte) getFlag(int,byte)}
		/// for available options)
		/// </param>
		/// <param name="flagValue">new value for the flag (<code>true</code> is logic "1")
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// In the case of the DS1922, this could also be due to a
		/// currently running mission.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// 
		/// </summary>
		/// <seealso cref="getFlag(int,byte)">
		/// </seealso>
		/// <seealso cref="getFlag(int,byte,byte[])">
		/// </seealso>
		/// <seealso cref="setFlag(int,byte,boolean,byte[])">
		/// </seealso>
		/// <seealso cref="readDevice()">
		/// </seealso>
		public virtual void  setFlag(int register, byte bitMask, bool flagValue)
		{
			byte[] state = readDevice();
			
			setFlag(register, bitMask, flagValue, state);
			
			writeDevice(state);
		}
		
		
		/// <summary> <p>Sets the status of the specified flag in the specified register.
		/// If a mission is in progress a <code>OneWireIOException</code> will be thrown
		/// (one cannot write to the registers while a mission is commencing).  This method
		/// is the preferred manner of setting the DS1922 status and control flags.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.</p>
		/// 
		/// <p>For more information on valid values for the <code>bitMask</code>
		/// parameter, see the {@link #getFlag(int,byte) getFlag(int,byte)} method.</p>
		/// 
		/// </summary>
		/// <param name="register">address of register containing the flag (see
		/// {@link #getFlag(int,byte) getFlag(int,byte)} for available options)
		/// </param>
		/// <param name="bitMask">the flag to read (see {@link #getFlag(int,byte) getFlag(int,byte)}
		/// for available options)
		/// </param>
		/// <param name="flagValue">new value for the flag (<code>true</code> is logic "1")
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="getFlag(int,byte)">
		/// </seealso>
		/// <seealso cref="getFlag(int,byte,byte[])">
		/// </seealso>
		/// <seealso cref="setFlag(int,byte,boolean)">
		/// </seealso>
		/// <seealso cref="readDevice()">
		/// </seealso>
		/// <seealso cref="writeDevice(byte[])">
		/// </seealso>
		public virtual void  setFlag(int register, byte bitMask, bool flagValue, byte[] state)
		{
			register = register & 0x3F;
			
			byte flags = state[register];
			
			if (flagValue)
				flags = (byte) (flags | bitMask);
			else
				flags = (byte) (flags & ~ (bitMask));
			
			// write the regs back
			state[register] = flags;
		}
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Container Functions
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// DS1922 Device Specific Functions
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************




		/// <summary> Stops the currently running mission.
		/// 
		/// </summary>
		public virtual void  stopMission()
		{
			/* read a user specified amount of memory and verify its validity */
			if (doSpeedEnable)
				doSpeed();
			
			if (!adapter.select(address))
				throw new OneWireException("OneWireContainer41-Device not present.");
			
			byte[] buffer = new byte[10];
			buffer[0] = STOP_MISSION_PW_COMMAND;
			getContainerReadWritePassword(buffer, 1);
			buffer[9] = (byte) SupportClass.Identity(0xFF);
			
			adapter.dataBlock(buffer, 0, 10);
			
			if (getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MISSION_IN_PROGRESS))
				throw new OneWireException("OneWireContainer41-Stop mission failed.  Check read/write password.");
		}
		
		/// <summary> Starts a new mission.  Assumes all parameters have been set by either
		/// writing directly to the device registers, or by calling other setup
		/// methods.
		/// </summary>
		public virtual void  startMission()
		{
			if (getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MISSION_IN_PROGRESS))
				throw new OneWireException("OneWireContainer41-Cannot start a mission while a mission is in progress.");
			
			if (!getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MEMORY_CLEARED))
				throw new OneWireException("OneWireContainer41-Must clear memory before calling start mission.");
			
			if (doSpeedEnable)
				doSpeed();
			
			if (!adapter.select(address))
				throw new OneWireException("OneWireContainer41-Device not present.");
			
			byte[] buffer = new byte[10];
			buffer[0] = START_MISSION_PW_COMMAND;
			getContainerReadWritePassword(buffer, 1);
			buffer[9] = (byte) SupportClass.Identity(0xFF);
			
			adapter.dataBlock(buffer, 0, 10);
		}
		
		/// <summary> Erases the log memory from this missioning device.</summary>
		public virtual void  clearMemory()
		{
			if (getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MISSION_IN_PROGRESS))
				throw new OneWireException("OneWireContainer41-Cannot clear memory while mission is in progress.");
			
			if (doSpeedEnable)
				doSpeed();
			
			if (!adapter.select(address))
				throw new OneWireException("OneWireContainer41-Device not present.");
			
			byte[] buffer = new byte[10];
			buffer[0] = CLEAR_MEMORY_PW_COMMAND;
			getContainerReadWritePassword(buffer, 1);
			buffer[9] = (byte) SupportClass.Identity(0xFF);
			
			adapter.dataBlock(buffer, 0, 10);
			
			// wait 2 ms for Clear Memory to complete
			msWait(2);
			
			if (!getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MEMORY_CLEARED))
				throw new OneWireException("OneWireContainer41-Clear Memory failed.  Check read/write password.");
		}
		
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Read/Write Password Functions
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		/// <summary> Returns true if this device has a Read-Only password.
		/// If false, all other functions dealing with the Read-Only
		/// password will throw an exception if called.
		/// 
		/// </summary>
		/// <returns> <code>true</code> always, since DS1922 has Read-Only password.
		/// </returns>
		public virtual bool hasReadOnlyPassword()
		{
			return true;
		}
		
		/// <summary> Returns true if this device has a Read/Write password.
		/// If false, all other functions dealing with the Read/Write
		/// password will throw an exception if called.
		/// 
		/// </summary>
		/// <returns> <code>true</code> always, since DS1922 has Read/Write password.
		/// </returns>
		public virtual bool hasReadWritePassword()
		{
			return true;
		}
		
		/// <summary> Returns true if this device has a Write-Only password.
		/// If false, all other functions dealing with the Write-Only
		/// password will throw an exception if called.
		/// 
		/// </summary>
		/// <returns> <code>false</code> always, since DS1922 has no Write-Only password.
		/// </returns>
		public virtual bool hasWriteOnlyPassword()
		{
			return false;
		}
		
		/// <summary> Returns true if this device has the capability to enable one type of password
		/// while leaving another type disabled.  i.e. if the device has Read-Only password
		/// protection and Write-Only password protection, this method indicates whether or
		/// not you can enable Read-Only protection while leaving the Write-Only protection
		/// disabled.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the device has the capability to enable one type
		/// of password while leaving another type disabled.
		/// </returns>
		public virtual bool hasSinglePasswordEnable()
		{
			return false;
		}
		
		/// <summary> <p>Enables/Disables passwords for this Device.  This method allows you to
		/// individually enable the different types of passwords for a particular
		/// device.  If <code>hasSinglePasswordEnable()</code> returns true,
		/// you can selectively enable particular types of passwords.  Otherwise,
		/// this method will throw an exception if all supported types are not
		/// enabled.</p>
		/// 
		/// <p>For this to be successful, either write-protect passwords must be disabled,
		/// or the write-protect password(s) for this container must be set and must match
		/// the value of the write-protect password(s) in the device's register.</p>
		/// 
		/// <P><B>
		/// WARNING: Enabling passwords requires that both the read password and the
		/// read/write password be re-written to the part.  Before calling this method,
		/// you should set the container read password and read/write password values.
		/// This will ensure that the correct value is written into the part.
		/// </B></P>
		/// 
		/// </summary>
		/// <param name="enableReadOnly">if <code>true</code> Read-Only passwords will be enabled.
		/// </param>
		/// <param name="enableReadWrite">if <code>true</code> Read/Write passwords will be enabled.
		/// </param>
		/// <param name="enableWriteOnly">if <code>true</code> Write-Only passwords will be enabled.
		/// </param>
		public virtual void  setDevicePasswordEnable(bool enableReadOnly, bool enableReadWrite, bool enableWriteOnly)
		{
			if (enableWriteOnly)
				throw new OneWireException("The DS1922 does not have a write only password.");
			if (enableReadOnly != enableReadWrite)
				throw new OneWireException("Both read-only and read/write will be set with enable.");
			if (!ContainerReadOnlyPasswordSet)
				throw new OneWireException("Container Read Password is not set");
			if (!ContainerReadWritePasswordSet)
				throw new OneWireException("Container Read/Write Password is not set");
			
			// must write both passwords for this to work
			byte[] bothPasswordsEnable = new byte[17];
			bothPasswordsEnable[0] = (enableReadOnly?ENABLE_BYTE:DISABLE_BYTE);
			getContainerReadOnlyPassword(bothPasswordsEnable, 1);
			getContainerReadWritePassword(bothPasswordsEnable, 9);
			
			register.write(PASSWORD_CONTROL_REGISTER & 0x3F, bothPasswordsEnable, 0, 17);
			
			if (enableReadOnly)
			{
				readOnlyPasswordEnabled = true;
				readWritePasswordEnabled = true;
			}
			else
			{
				readOnlyPasswordEnabled = false;
				readWritePasswordEnabled = false;
			}
		}
		
		/// <summary> <p>Writes the given password to the device's Read-Only password register.  Note
		/// that this function does not enable the password, just writes the value to
		/// the appropriate memory location.</p>
		/// 
		/// <p>For this to be successful, either write-protect passwords must be disabled,
		/// or the write-protect password(s) for this container must be set and must match
		/// the value of the write-protect password(s) in the device's register.</p>
		/// 
		/// <P><B>
		/// WARNING: Setting the read password requires that both the read password
		/// and the read/write password be written to the part.  Before calling this
		/// method, you should set the container read/write password value.
		/// This will ensure that the correct value is written into the part.
		/// </B></P>
		/// 
		/// </summary>
		/// <param name="password">the new password to be written to the device's Read-Only
		/// password register.  Length must be
		/// <code>(offset + getReadOnlyPasswordLength)</code>
		/// </param>
		/// <param name="offset">the starting point for copying from the given password array
		/// </param>
		public virtual void  setDeviceReadOnlyPassword(byte[] password, int offset)
		{
			if (getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MISSION_IN_PROGRESS))
				throw new OneWireIOException("OneWireContainer41-Cannot change password while mission is in progress.");
			
			if (!ContainerReadWritePasswordSet)
				throw new OneWireException("Container Read/Write Password is not set");
			
			// must write both passwords for this to work
			byte[] bothPasswords = new byte[16];
			Array.Copy(password, offset, bothPasswords, 0, 8);
			getContainerReadWritePassword(bothPasswords, 8);
			
			register.write(READ_ACCESS_PASSWORD & 0x3F, bothPasswords, 0, 16);
			setContainerReadOnlyPassword(password, offset);
		}
		
		/// <summary> <p>Writes the given password to the device's Read/Write password register.  Note
		/// that this function does not enable the password, just writes the value to
		/// the appropriate memory location.</p>
		/// 
		/// <p>For this to be successful, either write-protect passwords must be disabled,
		/// or the write-protect password(s) for this container must be set and must match
		/// the value of the write-protect password(s) in the device's register.</p>
		/// 
		/// </summary>
		/// <param name="password">the new password to be written to the device's Read-Write
		/// password register.  Length must be
		/// <code>(offset + getReadWritePasswordLength)</code>
		/// </param>
		/// <param name="offset">the starting point for copying from the given password array
		/// </param>
		public virtual void  setDeviceReadWritePassword(byte[] password, int offset)
		{
			if (getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MISSION_IN_PROGRESS))
				throw new OneWireIOException("OneWireContainer41-Cannot change password while mission is in progress.");
			
			register.write(READ_WRITE_ACCESS_PASSWORD & 0x3F, password, offset, 8);
			setContainerReadWritePassword(password, offset);
		}
		
		/// <summary> <p>Writes the given password to the device's Write-Only password register.  Note
		/// that this function does not enable the password, just writes the value to
		/// the appropriate memory location.</p>
		/// 
		/// <p>For this to be successful, either write-protect passwords must be disabled,
		/// or the write-protect password(s) for this container must be set and must match
		/// the value of the write-protect password(s) in the device's register.</p>
		/// 
		/// </summary>
		/// <param name="password">the new password to be written to the device's Write-Only
		/// password register.  Length must be
		/// <code>(offset + getWriteOnlyPasswordLength)</code>
		/// </param>
		/// <param name="offset">the starting point for copying from the given password array
		/// </param>
		public virtual void  setDeviceWriteOnlyPassword(byte[] password, int offset)
		{
			throw new OneWireException("The DS1922 does not have a write only password.");
		}
		
		/// <summary> Sets the Read-Only password used by the API when reading from the
		/// device's memory.  This password is not written to the device's
		/// Read-Only password register.  It is the password used by the
		/// software for interacting with the device only.
		/// 
		/// </summary>
		/// <param name="password">the new password to be used by the API when
		/// reading from the device's memory.  Length must be
		/// <code>(offset + getReadOnlyPasswordLength)</code>
		/// </param>
		/// <param name="offset">the starting point for copying from the given password array
		/// </param>
		public virtual void  setContainerReadOnlyPassword(byte[] password, int offset)
		{
			Array.Copy(password, offset, readPassword, 0, PASSWORD_LENGTH);
			readPasswordSet = true;
		}
		
		/// <summary> Sets the Read/Write password used by the API when reading from  or
		/// writing to the device's memory.  This password is not written to
		/// the device's Read/Write password register.  It is the password used
		/// by the software for interacting with the device only.
		/// 
		/// </summary>
		/// <param name="password">the new password to be used by the API when
		/// reading from or writing to the device's memory.  Length must be
		/// <code>(offset + getReadWritePasswordLength)</code>
		/// </param>
		/// <param name="offset">the starting point for copying from the given password array
		/// </param>
		public virtual void  setContainerReadWritePassword(byte[] password, int offset)
		{
			Array.Copy(password, offset, readWritePassword, 0, 8);
			readWritePasswordSet = true;
		}
		
		/// <summary> Sets the Write-Only password used by the API when writing to the
		/// device's memory.  This password is not written to the device's
		/// Write-Only password register.  It is the password used by the
		/// software for interacting with the device only.
		/// 
		/// </summary>
		/// <param name="password">the new password to be used by the API when
		/// writing to the device's memory.  Length must be
		/// <code>(offset + getWriteOnlyPasswordLength)</code>
		/// </param>
		/// <param name="offset">the starting point for copying from the given password array
		/// </param>
		public virtual void  setContainerWriteOnlyPassword(byte[] password, int offset)
		{
			throw new OneWireException("The DS1922 does not have a write only password.");
		}
		
		/// <summary> Gets the Read-Only password used by the API when reading from the
		/// device's memory.  This password is not read from the device's
		/// Read-Only password register.  It is the password used by the
		/// software for interacting with the device only and must have been
		/// set using the <code>setContainerReadOnlyPassword</code> method.
		/// 
		/// </summary>
		/// <param name="password">array for holding the password that is used by the
		/// API when reading from the device's memory.  Length must be
		/// <code>(offset + getWriteOnlyPasswordLength)</code>
		/// </param>
		/// <param name="offset">the starting point for copying into the given password array
		/// </param>
		public virtual void  getContainerReadOnlyPassword(byte[] password, int offset)
		{
			Array.Copy(readPassword, 0, password, offset, PASSWORD_LENGTH);
		}
		
		/// <summary> Gets the Read/Write password used by the API when reading from or
		/// writing to the device's memory.  This password is not read from
		/// the device's Read/Write password register.  It is the password used
		/// by the software for interacting with the device only and must have
		/// been set using the <code>setContainerReadWritePassword</code> method.
		/// 
		/// </summary>
		/// <param name="password">array for holding the password that is used by the
		/// API when reading from or writing to the device's memory.  Length must be
		/// <code>(offset + getReadWritePasswordLength)</code>
		/// </param>
		/// <param name="offset">the starting point for copying into the given password array
		/// </param>
		public virtual void  getContainerReadWritePassword(byte[] password, int offset)
		{
			Array.Copy(readWritePassword, 0, password, offset, PASSWORD_LENGTH);
		}
		
		/// <summary> Gets the Write-Only password used by the API when writing to the
		/// device's memory.  This password is not read from the device's
		/// Write-Only password register.  It is the password used by the
		/// software for interacting with the device only and must have been
		/// set using the <code>setContainerWriteOnlyPassword</code> method.
		/// 
		/// </summary>
		/// <param name="password">array for holding the password that is used by the
		/// API when writing to the device's memory.  Length must be
		/// <code>(offset + getWriteOnlyPasswordLength)</code>
		/// </param>
		/// <param name="offset">the starting point for copying into the given password array
		/// </param>
		public virtual void  getContainerWriteOnlyPassword(byte[] password, int offset)
		{
			throw new OneWireException("The DS1922 does not have a write only password");
		}
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Mission Interface Functions
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		/// <summary> Returns a default friendly label for each channel supported by this
		/// Missioning device.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> friendly label for the specified channel
		/// </returns>
		public virtual System.String getMissionLabel(int channel)
		{
			if (channel == TEMPERATURE_CHANNEL)
			{
				return "Temperature";
			}
			else if (channel == DATA_CHANNEL)
			{
				if (hasHumiditySensor && !adForceResults)
					return "Humidity";
				else
					return "Data";
			}
			else
				throw new OneWireException("Invalid Channel");
		}
		
		/// <summary> Sets the SUTA (Start Upon Temperature Alarm) bit in the Mission Control
		/// register.  This method will communicate with the device directly.
		/// 
		/// </summary>
		/// <param name="enable">sets/clears the SUTA bit in the Mission Control register.
		/// </param>
		public virtual void  setStartUponTemperatureAlarmEnable(bool enable)
		{
			setFlag(MISSION_CONTROL_REGISTER, MCR_BIT_START_MISSION_ON_TEMPERATURE_ALARM, enable);
		}
		
		/// <summary> Sets the SUTA (Start Upon Temperature Alarm) bit in the Mission Control
		/// register.  This method will set the bit in the provided 'state' array,
		/// which should be acquired through a call to <code>readDevice()</code>.
		/// After updating the 'state', the method <code>writeDevice(byte[])</code>
		/// should be called to commit your changes.
		/// 
		/// </summary>
		/// <param name="enable">sets/clears the SUTA bit in the Mission Control register.
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// </param>
		public virtual void  setStartUponTemperatureAlarmEnable(bool enable, byte[] state)
		{
			setFlag(MISSION_CONTROL_REGISTER, MCR_BIT_START_MISSION_ON_TEMPERATURE_ALARM, enable, state);
		}
		
		/// <summary> Returns true if the SUTA (Start Upon Temperature Alarm) bit in the
		/// Mission Control register is set.  This method will communicate with
		/// the device to read the status of the SUTA bit.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the SUTA bit in the Mission Control register is set.
		/// </returns>
		public virtual bool isStartUponTemperatureAlarmEnabled()
		{
			return getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_START_MISSION_ON_TEMPERATURE_ALARM);
		}
		
		/// <summary> Returns true if the SUTA (Start Upon Temperature Alarm) bit in the
		/// Mission Control register is set.  This method will check for  the bit
		/// in the provided 'state' array, which should be acquired through a call
		/// to <code>readDevice()</code>.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// </param>
		/// <returns> <code>true</code> if the SUTA bit in the Mission Control register is set.
		/// </returns>
		public virtual bool isStartUponTemperatureAlarmEnabled(byte[] state)
		{
			return getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_START_MISSION_ON_TEMPERATURE_ALARM, state);
		}
		
		/// <summary> Begins a new mission on this missioning device.
		/// 
		/// </summary>
		/// <param name="sampleRate">indicates the sampling rate, in seconds, that
		/// this missioning device should log samples.
		/// </param>
		/// <param name="missionStartDelay">indicates the amount of time, in minutes,
		/// that should pass before the mission begins.
		/// </param>
		/// <param name="rolloverEnabled">if <code>false</code>, this device will stop
		/// recording new samples after the data log is full.  Otherwise,
		/// it will replace samples starting at the beginning.
		/// </param>
		/// <param name="syncClock">if <code>true</code>, the real-time clock of this
		/// missioning device will be synchronized with the current time
		/// according to this <code>java.util.Date</code>.
		/// </param>
		public virtual void  startNewMission(int sampleRate, int missionStartDelay, bool rolloverEnabled, bool syncClock, bool[] channelEnabled)
		{
			byte[] state = readDevice();
			//if(isMissionUploaded)
			//   state = missionRegister;
			//else
			//   state = readDevice();
			
			//      System.out.println("startNewMission: before state=" + Convert.toHexString(state));
			
			for (int i = 0; i < NumberMissionChannels; i++)
				setMissionChannelEnable(i, channelEnabled[i], state);
			
			if (sampleRate % 60 == 0 || sampleRate > 0x03FFF)
			{
				//convert to minutes
				sampleRate = (sampleRate / 60) & 0x03FFF;
				setFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_HIGH_SPEED_SAMPLE, false, state);
			}
			else
			{
				setFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_HIGH_SPEED_SAMPLE, true, state);
			}
			
			com.dalsemi.onewire.utils.Convert.toByteArray(sampleRate, state, SAMPLE_RATE & 0x3F, 2);
			
			com.dalsemi.onewire.utils.Convert.toByteArray(missionStartDelay, state, MISSION_START_DELAY & 0x3F, 3);
			
			setFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_ROLLOVER, rolloverEnabled, state);
			
			if (syncClock)
			{
				//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
				setClock(System.DateTime.Now.Ticks, state);
			}
			else if (!getFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_OSCILLATOR, state))
			{
				setFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_OSCILLATOR, true, state);
			}
			//      System.out.println("startNewMission: after  state=" + Convert.toHexString(state));
			
			clearMemory();
			writeDevice(state);
			startMission();
		}
		
		/// <summary> Loads the results of the currently running mission.  Must be called
		/// before all mission result/status methods.
		/// </summary>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'loadMissionResults'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual void  loadMissionResults()
		{
			lock (this)
			{
				// read the register contents
				missionRegister = readDevice();
				
				// get the number of samples
				sampleCount = com.dalsemi.onewire.utils.Convert.toInt(missionRegister, MISSION_SAMPLE_COUNT & 0x3F, 3);
				sampleCountTotal = sampleCount;
				
				// sample rate, in seconds
				sampleRate = com.dalsemi.onewire.utils.Convert.toInt(missionRegister, SAMPLE_RATE & 0x3F, 2);
				if (!getFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_HIGH_SPEED_SAMPLE, missionRegister))
				// if sample rate is in minutes, convert to seconds
					sampleRate *= 60;
				
				//grab the time
				int[] time = getTime(MISSION_TIMESTAMP_TIME & 0x3F, missionRegister);
				//grab the date
				int[] date = getDate(MISSION_TIMESTAMP_DATE & 0x3F, missionRegister);
				
				//date[1] - 1 because Java months are 0 offset
				System.Globalization.GregorianCalendar temp_calendar;
				temp_calendar = new System.Globalization.GregorianCalendar();
				SupportClass.CalendarManager.manager.Set(temp_calendar, date[0], date[1] - 1, date[2], time[2], time[1], time[0]);
				System.Globalization.Calendar d = temp_calendar;
				
				//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.util.Calendar.getTime' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				missionTimeStamp = SupportClass.CalendarManager.manager.GetDateTime(d).Ticks;
				
				// figure out how many bytes for each temperature sample
				temperatureBytes = 0;
				// if it's being logged, add 1 to the size
				if (getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_TEMPERATURE_LOGGING, missionRegister))
				{
					temperatureBytes += 1;
					// if it's 16-bit resolution, add another 1 to the size
					if (getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_TEMPERATURE_RESOLUTION, missionRegister))
						temperatureBytes += 1;
				}
				
				
				// figure out how many bytes for each data sample
				dataBytes = 0;
				// if it's being logged, add 1 to the size
				if (getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_DATA_LOGGING, missionRegister))
				{
					dataBytes += 1;
					// if it's 16-bit resolution, add another 1 to the size
					if (getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_DATA_RESOLUTION, missionRegister))
						dataBytes += 1;
				}
				
				// default size of the log, could be different if using an odd
				// sample size combination.
				//int logSize = MISSION_LOG_SIZE;
				
				// figure max number of samples
				int maxSamples = 0;
				switch (temperatureBytes + dataBytes)
				{
					
					case 1: 
						maxSamples = 8192;
						break;
					
					case 2: 
						maxSamples = 4096;
						break;
					
					case 3: 
						maxSamples = 2560;
						//logSize = ODD_MISSION_LOG_SIZE;
						break;
					
					case 4: 
						maxSamples = 2048;
						break;
					
					case 0: 
					default: 
						// assert! should never, ever get here
						break;
					}
				
				// check for rollover
				int wrapCount = 0, offsetDepth = 0;
				if (getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_ROLLOVER, missionRegister) && (rolledOver = (sampleCount > maxSamples)))
				// intentional assignment
				{
					wrapCount = (sampleCount / maxSamples) - 1;
					offsetDepth = sampleCount % maxSamples;
					sampleCount = maxSamples;
				}
				
				//DEBUG: For bad SOICS
				if (!getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_ROLLOVER, missionRegister) && rolledOver)
				{
					throw new OneWireException("Device Error: rollover was not enabled, but it did occur.");
				}
				
				// figure out where the temperature bytes end, that's where
				// the data bytes begin
				int temperatureLogSize = temperatureBytes * maxSamples;
				
				// calculate first log entry time offset, in samples
				timeOffset = ((wrapCount * maxSamples) + offsetDepth);
				
				// temperature log
				temperatureLog = new byte[sampleCount * temperatureBytes];
				// data log
				dataLog = new byte[sampleCount * dataBytes];
				// cache for entire log
				byte[] missionLogBuffer = new byte[System.Math.Max(temperatureLog.Length, dataLog.Length)];
				byte[] pagebuffer = new byte[32];
				
				if (temperatureLog.Length > 0)
				{
					// read the data log for temperature
					int numPages = (temperatureLog.Length / 32) + ((temperatureLog.Length % 32) > 0?1:0);
					int retryCnt = MAX_READ_RETRY_CNT;
					for (int i = 0; i < numPages; )
					{
						try
						{
							log.readPageCRC(i, i > 0 && retryCnt == MAX_READ_RETRY_CNT, pagebuffer, 0);
							Array.Copy(pagebuffer, 0, missionLogBuffer, i * 32, System.Math.Min(32, temperatureLog.Length - (i * 32)));
							retryCnt = MAX_READ_RETRY_CNT;
							i++;
						}
						catch (OneWireIOException owioe)
						{
							if (--retryCnt == 0)
								throw owioe;
						}
						catch (OneWireException owe)
						{
							if (--retryCnt == 0)
								throw owe;
						}
					}
					
					// get the temperature bytes in order
					int offsetIndex = offsetDepth * temperatureBytes;
					Array.Copy(missionLogBuffer, offsetIndex, temperatureLog, 0, temperatureLog.Length - offsetIndex);
					Array.Copy(missionLogBuffer, 0, temperatureLog, temperatureLog.Length - offsetIndex, offsetIndex);
				}
				
				if (dataLog.Length > 0)
				{
					// read the data log for humidity
					int numPages = (dataLog.Length / 32) + ((dataLog.Length % 32) > 0?1:0);
					int retryCnt = MAX_READ_RETRY_CNT;
					for (int i = 0; i < numPages; )
					{
						try
						{
							log.readPageCRC((temperatureLogSize / 32) + i, i > 0 && retryCnt == MAX_READ_RETRY_CNT, pagebuffer, 0);
							Array.Copy(pagebuffer, 0, missionLogBuffer, i * 32, System.Math.Min(32, dataLog.Length - (i * 32)));
							retryCnt = MAX_READ_RETRY_CNT;
							i++;
						}
						catch (OneWireIOException owioe)
						{
							if (--retryCnt == 0)
								throw owioe;
						}
						catch (OneWireException owe)
						{
							if (--retryCnt == 0)
								throw owe;
						}
					}
					
					// get the data bytes in order
					int offsetIndex = offsetDepth * dataBytes;
					Array.Copy(missionLogBuffer, offsetIndex, dataLog, 0, dataLog.Length - offsetIndex);
					Array.Copy(missionLogBuffer, 0, dataLog, dataLog.Length - offsetIndex, offsetIndex);
				}
				
				isMissionUploaded = true;
			}
		}
		
		/// <summary> Enables/disables the specified mission channel, indicating whether or
		/// not the channel's readings will be recorded in the mission log.
		/// 
		/// </summary>
		/// <param name="channel">the channel to enable/disable
		/// </param>
		/// <param name="enable">if true, the channel is enabled
		/// </param>
		public virtual void  setMissionChannelEnable(int channel, bool enable)
		{
			if (!isMissionUploaded)
				missionRegister = readDevice();
			setMissionChannelEnable(channel, enable, missionRegister);
			writeDevice(missionRegister);
		}
		
		/// <summary> Enables/disables the specified mission channel, indicating whether or
		/// not the channel's readings will be recorded in the mission log.
		/// 
		/// </summary>
		/// <param name="channel">the channel to enable/disable
		/// </param>
		/// <param name="enable">if true, the channel is enabled
		/// </param>
		/// <param name="state">the state as returned from readDevice, for cached writes
		/// </param>
		public virtual void  setMissionChannelEnable(int channel, bool enable, byte[] state)
		{
			if (channel == TEMPERATURE_CHANNEL)
			{
				setFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_TEMPERATURE_LOGGING, enable, state);
			}
			else if (channel == DATA_CHANNEL)
			{
				setFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_DATA_LOGGING, enable, state);
			}
			else
			{
				throw new System.ArgumentException("Invalid Channel");
			}
		}
		
		/// <summary> Returns true if the specified mission channel is enabled, indicating
		/// that the channel's readings will be recorded in the mission log.
		/// 
		/// </summary>
		/// <param name="channel">the channel to enable/disable
		/// </param>
		/// <param name="enable">if true, the channel is enabled
		/// </param>
		public virtual bool getMissionChannelEnable(int channel)
		{
			if (!isMissionUploaded)
				missionRegister = readDevice();
			
			return getMissionChannelEnable(channel, missionRegister);
		}
		
		/// <summary> Returns true if the specified mission channel is enabled, indicating
		/// that the channel's readings will be recorded in the mission log.
		/// 
		/// </summary>
		/// <param name="channel">the channel to enable/disable
		/// </param>
		/// <param name="enable">if true, the channel is enabled
		/// </param>
		public virtual bool getMissionChannelEnable(int channel, byte[] state)
		{
			if (channel == TEMPERATURE_CHANNEL)
			{
				return getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_TEMPERATURE_LOGGING, state);
			}
			else if (channel == DATA_CHANNEL)
			{
				return getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_ENABLE_DATA_LOGGING, state);
			}
			else
			{
				throw new System.ArgumentException("Invalid Channel");
			}
		}
		
		
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// - Mission Results
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		
		/// <summary> Returns the amount of time, in seconds, between samples taken
		/// by this missioning device.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> time, in seconds, between sampling
		/// </returns>
		public virtual int getMissionSampleRate(int channel)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			return sampleRate;
		}
		
		/// <summary> Returns the number of samples available for the specified channel
		/// during the current mission.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> number of samples available for the specified channel
		/// </returns>
		public virtual int getMissionSampleCount(int channel)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			return sampleCount;
		}
		
		/// <summary> Reads the device and returns the total number of samples logged
		/// since the first power-on of this device.
		/// 
		/// </summary>
		/// <returns> the total number of samples logged since the first power-on
		/// of this device.
		/// </returns>
		public virtual int getDeviceSampleCount()
		{
			return getDeviceSampleCount(readDevice());
		}
		
		/// <summary> Returns the total number of samples logged since the first power-on
		/// of this device.
		/// 
		/// </summary>
		/// <param name="state">The current state of the device as return from <code>readDevice()</code>
		/// </param>
		/// <returns> the total number of samples logged since the first power-on
		/// of this device.
		/// </returns>
		public virtual int getDeviceSampleCount(byte[] state)
		{
			return com.dalsemi.onewire.utils.Convert.toInt(state, DEVICE_SAMPLE_COUNT & 0x3F, 3);
		}
		
		
		/// <summary> Returns the total number of samples taken for the specified channel
		/// during the current mission.  This number can be more than the actual
		/// sample count if rollover is enabled and the log has been filled.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> number of samples taken for the specified channel
		/// </returns>
		public virtual int getMissionSampleCountTotal(int channel)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			return sampleCountTotal;
		}
		
		/// <summary> Returns the sample as degrees celsius if temperature channel is specified
		/// or as percent relative humidity if data channel is specified.  If the
		/// device is a DS2422 configuration (or A-D results are forced on the DS1923),
		/// the data channel will return samples as the voltage measured.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="sampleNum">the sample number to return, between <code>0</code> and
		/// <code>(getMissionSampleCount(channel)-1)</code>
		/// </param>
		/// <returns> the sample's value in degrees Celsius or percent RH.
		/// </returns>
		public virtual double getMissionSample(int channel, int sampleNum)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			if (sampleNum >= sampleCount || sampleNum < 0)
				throw new System.ArgumentException("Invalid sample number");
			
			double val = 0;
			if (channel == TEMPERATURE_CHANNEL)
			{
				val = decodeTemperature(temperatureLog, sampleNum * temperatureBytes, temperatureBytes, true);
				if (useTempCalibrationRegisters && ((System.Object) partNumber != (System.Object) PART_NUMBER_DS1922E)) // DS1922E does not use calibration registers
				{
			        if ((partNumber == PART_NUMBER_DS1922F) && (val < 130.0))
			        { 
			          // do nothing -- the DS1922F only gets software corrected above or equal to 130 degrees Celsius
			        } 
			        else 
			        { 			
				        double valsq = val*val;
				        double error
					        = tempCoeffA*valsq + tempCoeffB*val + tempCoeffC;
				        val = val - error;
			        } 				
                }
			}
			else if (channel == DATA_CHANNEL)
			{
				if (hasHumiditySensor && !adForceResults)
				{
					val = decodeHumidity(dataLog, sampleNum * dataBytes, dataBytes, true);
					
					if (useTemperatureCompensation)
					{
						double T;
						if (!overrideTemperatureLog && getMissionSampleCount(TEMPERATURE_CHANNEL) > 0)
							T = getMissionSample(TEMPERATURE_CHANNEL, sampleNum);
						else
							T = (double) defaultTempCompensationValue;
						double gamma = (T > 15)?0.00001:- 0.00005;
						T -= 25;
						val = (val * 0.0307 + .0035 * T - 0.000043 * T * T) / (0.0307 + gamma * T - 0.000002 * T * T);
					}
				}
				else
				{
					val = getADVoltage(dataLog, sampleNum * dataBytes, dataBytes, true);
				}
			}
			else
				throw new System.ArgumentException("Invalid Channel");
			
			return val;
		}
		
		
		/// <summary> Returns the sample as an integer value.  This value is not converted to
		/// degrees Celsius for temperature or to percent RH for Humidity.  It is
		/// simply the 8 or 16 bits of digital data written in the mission log for
		/// this sample entry.  It is up to the user to mask off the unused bits
		/// and convert this value to it's proper units.  This method is primarily
		/// for users of the DS2422 who are using an input device which is not an
		/// A-D or have an A-D wholly dissimilar to the one specified in the
		/// datasheet.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="sampleNum">the sample number to return, between <code>0</code> and
		/// <code>(getMissionSampleCount(channel)-1)</code>
		/// </param>
		/// <returns> the sample as a whole integer
		/// </returns>
		public virtual int getMissionSampleAsInteger(int channel, int sampleNum)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			if (sampleNum >= sampleCount || sampleNum < 0)
				throw new System.ArgumentException("Invalid sample number");
			
			int i = 0;
			if (channel == TEMPERATURE_CHANNEL)
			{
				if (temperatureBytes == 2)
				{
					i = ((0x0FF & temperatureLog[sampleNum * temperatureBytes]) << 8) | ((0x0FF & temperatureLog[sampleNum * temperatureBytes + 1]));
				}
				else
				{
					i = (0x0FF & temperatureLog[sampleNum * temperatureBytes]);
				}
			}
			else if (channel == DATA_CHANNEL)
			{
				if (dataBytes == 2)
				{
					i = ((0x0FF & dataLog[sampleNum * dataBytes]) << 8) | ((0x0FF & dataLog[sampleNum * dataBytes + 1]));
				}
				else
				{
					i = (0x0FF & dataLog[sampleNum * dataBytes]);
				}
			}
			else
				throw new System.ArgumentException("Invalid Channel");
			
			return i;
		}
		
		
		/// <summary> Returns the time, in milliseconds, that each sample was taken by the
		/// current mission.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="sampleNum">the sample number to return, between <code>0</code> and
		/// <code>(getMissionSampleCount(channel)-1)</code>
		/// </param>
		/// <returns> the sample's timestamp, in milliseconds
		/// </returns>
		public virtual long getMissionSampleTimeStamp(int channel, int sampleNum)
		{
            long missionSampleTStamp = 0;
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			long delta = ((long) ((long) timeOffset + (long) sampleNum)) * (long) sampleRate;
            missionSampleTStamp = delta * (long) 10000000 + missionTimeStamp;
			//return delta * 1000L + missionTimeStamp; // old java way
            return missionSampleTStamp;
		}
		
		/// <summary> Returns <code>true</code> if a mission has rolled over.</summary>
		/// <returns> <code>true</code> if a mission has rolled over.
		/// </returns>
		public virtual bool hasMissionRolloverOccurred()
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			return this.rolledOver;
		}
		
		/// <summary> Clears the mission results and erases the log memory from this
		/// missioning device.
		/// </summary>
		public virtual void  clearMissionResults()
		{
			clearMemory();
			isMissionUploaded = false;
		}
		
		/// <summary> Returns the time, in milliseconds, that the mission began.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> time, in milliseconds, that the mission began
		/// </returns>
		public virtual long getMissionTimeStamp(int channel)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			return this.missionTimeStamp;
		}
		
		/// <summary> Returns the amount of time, in milliseconds, before the first sample
		/// occurred.  If rollover disabled, or datalog didn't fill up, this
		/// will be 0.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> time, in milliseconds, before first sample occurred
		/// </returns>
		public virtual long getFirstSampleOffset(int channel)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			return timeOffset * sampleRate * 1000L;
		}
		
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// - Mission Resolutions
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		
		/// <summary> Returns all available resolutions for the specified mission channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> all available resolutions for the specified mission channel.
		/// </returns>
		public virtual double[] getMissionResolutions(int channel)
		{
			if (channel == TEMPERATURE_CHANNEL)
			{
				return new double[]{temperatureResolutions[0], temperatureResolutions[1]};
			}
			else if (channel == DATA_CHANNEL)
			{
				if (hasHumiditySensor && !adForceResults)
					return new double[]{humidityResolutions[0], humidityResolutions[1]};
				else if (adForceResults)
					return new double[]{dataResolutions[0], dataResolutions[1]};
				else
					return new double[]{8, 16};
			}
			else
				throw new System.ArgumentException("Invalid Channel");
		}
		
		/// <summary> Returns the currently selected resolution for the specified
		/// channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> the currently selected resolution for the specified channel.
		/// </returns>
		public virtual double getMissionResolution(int channel)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			double resolution = 0;
			if (channel == TEMPERATURE_CHANNEL)
			{
				if (getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_TEMPERATURE_RESOLUTION, missionRegister))
					resolution = temperatureResolutions[1];
				else
					resolution = temperatureResolutions[0];
			}
			else if (channel == DATA_CHANNEL)
			{
				if (getFlag(MISSION_CONTROL_REGISTER, MCR_BIT_DATA_RESOLUTION, missionRegister))
				{
					if (hasHumiditySensor && !adForceResults)
						resolution = humidityResolutions[1];
					else if (adForceResults)
						resolution = dataResolutions[1];
					else
						resolution = 16;
				}
				else
				{
					if (hasHumiditySensor && !adForceResults)
						resolution = humidityResolutions[0];
					else if (adForceResults)
						resolution = dataResolutions[0];
					else
						resolution = 8;
				}
			}
			else
			{
				throw new System.ArgumentException("Invalid Channel");
			}
			return resolution;
		}
		
		/// <summary> Sets the selected resolution for the specified channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="resolution">the new resolution for the specified channel.
		/// </param>
		public virtual void  setMissionResolution(int channel, double resolution)
		{
			if (!isMissionUploaded)
				missionRegister = readDevice();
			
			if (channel == TEMPERATURE_CHANNEL)
			{
				setFlag(MISSION_CONTROL_REGISTER, MCR_BIT_TEMPERATURE_RESOLUTION, resolution == temperatureResolutions[1], missionRegister);
			}
			else if (channel == DATA_CHANNEL)
			{
				if (hasHumiditySensor && !adForceResults)
					setFlag(MISSION_CONTROL_REGISTER, MCR_BIT_DATA_RESOLUTION, resolution == humidityResolutions[1], missionRegister);
				else if (adForceResults)
					setFlag(MISSION_CONTROL_REGISTER, MCR_BIT_DATA_RESOLUTION, resolution == dataResolutions[1], missionRegister);
				else
					setFlag(MISSION_CONTROL_REGISTER, MCR_BIT_DATA_RESOLUTION, resolution == 16, missionRegister);
			}
			else
			{
				throw new System.ArgumentException("Invalid Channel");
			}
			
			writeDevice(missionRegister);
		}
		
		/// <summary> Sets the default temperature value for temperature compensation.  This
		/// value will be used if there is no temperature log data or if the
		/// <code>override</code> parameter is true.
		/// 
		/// </summary>
		/// <param name="temperatureValue">the default temperature value for temperature
		/// compensation.
		/// </param>
		/// <param name="override">if <code>true</code>, the default temperature value
		/// will always be used (instead of the temperature log data).
		/// 
		/// </param>
		/// <seealso cref="setDefaultTemperatureCompensationValue">
		/// </seealso>
		public virtual void  setDefaultTemperatureCompensationValue(double temperatureValue, bool override_Renamed)
		{
			this.defaultTempCompensationValue = temperatureValue;
			this.overrideTemperatureLog = override_Renamed;
		}
		
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// - Mission Alarms
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		
		/// <summary> Indicates whether or not the specified channel of this missioning device
		/// has mission alarm capabilities.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> true if the device has mission alarms for the specified channel.
		/// </returns>
		public virtual bool hasMissionAlarms(int channel)
		{
			return true;
		}
		
		/// <summary> Returns true if the specified channel's alarm value of the specified
		/// type has been triggered during the mission.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <returns> true if the alarm was triggered.
		/// </returns>
		public virtual bool hasMissionAlarmed(int channel, int alarmType)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			if (channel == TEMPERATURE_CHANNEL)
			{
				if (alarmType == com.dalsemi.onewire.container.MissionContainer_Fields.ALARM_HIGH)
				{
					return getFlag(ALARM_STATUS_REGISTER, ASR_BIT_TEMPERATURE_HIGH_ALARM, missionRegister);
				}
				else
				{
					return getFlag(ALARM_STATUS_REGISTER, ASR_BIT_TEMPERATURE_LOW_ALARM, missionRegister);
				}
			}
			else if (channel == DATA_CHANNEL)
			{
				if (alarmType == com.dalsemi.onewire.container.MissionContainer_Fields.ALARM_HIGH)
				{
					return getFlag(ALARM_STATUS_REGISTER, ASR_BIT_DATA_HIGH_ALARM, missionRegister);
				}
				else
				{
					return getFlag(ALARM_STATUS_REGISTER, ASR_BIT_DATA_LOW_ALARM, missionRegister);
				}
			}
			else
			{
				throw new System.ArgumentException("Invalid Channel");
			}
		}
		
		/// <summary> Returns true if the alarm of the specified type has been enabled for
		/// the specified channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <returns> true if the alarm of the specified type has been enabled for
		/// the specified channel.
		/// </returns>
		public virtual bool getMissionAlarmEnable(int channel, int alarmType)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			if (channel == TEMPERATURE_CHANNEL)
			{
				if (alarmType == com.dalsemi.onewire.container.MissionContainer_Fields.ALARM_HIGH)
				{
					return getFlag(TEMPERATURE_CONTROL_REGISTER, TCR_BIT_ENABLE_TEMPERATURE_HIGH_ALARM, missionRegister);
				}
				else
				{
					return getFlag(TEMPERATURE_CONTROL_REGISTER, TCR_BIT_ENABLE_TEMPERATURE_LOW_ALARM, missionRegister);
				}
			}
			else if (channel == DATA_CHANNEL)
			{
				if (alarmType == com.dalsemi.onewire.container.MissionContainer_Fields.ALARM_HIGH)
				{
					return getFlag(DATA_CONTROL_REGISTER, DCR_BIT_ENABLE_DATA_HIGH_ALARM, missionRegister);
				}
				else
				{
					return getFlag(DATA_CONTROL_REGISTER, DCR_BIT_ENABLE_DATA_LOW_ALARM, missionRegister);
				}
			}
			else
			{
				throw new System.ArgumentException("Invalid Channel");
			}
		}
		
		/// <summary> Enables/disables the alarm of the specified type for the specified channel
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <param name="enable">if true, alarm is enabled.
		/// </param>
		public virtual void  setMissionAlarmEnable(int channel, int alarmType, bool enable)
		{
			if (!isMissionUploaded)
				missionRegister = readDevice();
			
			//      System.out.println("setMissionAlarmEnable: channel=" + channel +", alarmType=" + alarmType + ", enable=" + enable);
			//      System.out.println("setMissionAlarmEnable: before state=" + Convert.toHexString(missionRegister));
			if (channel == TEMPERATURE_CHANNEL)
			{
				if (alarmType == com.dalsemi.onewire.container.MissionContainer_Fields.ALARM_HIGH)
				{
					setFlag(TEMPERATURE_CONTROL_REGISTER, TCR_BIT_ENABLE_TEMPERATURE_HIGH_ALARM, enable, missionRegister);
				}
				else
				{
					setFlag(TEMPERATURE_CONTROL_REGISTER, TCR_BIT_ENABLE_TEMPERATURE_LOW_ALARM, enable, missionRegister);
				}
			}
			else if (channel == DATA_CHANNEL)
			{
				if (alarmType == com.dalsemi.onewire.container.MissionContainer_Fields.ALARM_HIGH)
				{
					setFlag(DATA_CONTROL_REGISTER, DCR_BIT_ENABLE_DATA_HIGH_ALARM, enable, missionRegister);
				}
				else
				{
					setFlag(DATA_CONTROL_REGISTER, DCR_BIT_ENABLE_DATA_LOW_ALARM, enable, missionRegister);
				}
			}
			else
			{
				throw new System.ArgumentException("Invalid Channel");
			}
			//      System.out.println("setMissionAlarmEnable: after  state=" + Convert.toHexString(missionRegister));
			writeDevice(missionRegister);
		}
		
		/// <summary> Returns the threshold value which will trigger the alarm of the
		/// specified type on the specified channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <returns> the threshold value which will trigger the alarm
		/// </returns>
		public virtual double getMissionAlarm(int channel, int alarmType)
		{
			if (!isMissionUploaded)
				throw new OneWireException("Must load mission results first.");
			
			double th = 0;
			if (channel == TEMPERATURE_CHANNEL)
			{
				if (alarmType == com.dalsemi.onewire.container.MissionContainer_Fields.ALARM_HIGH)
				{
					th = decodeTemperature(missionRegister, TEMPERATURE_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
				}
				else
				{
					th = decodeTemperature(missionRegister, TEMPERATURE_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
				}
			}
			else if (channel == DATA_CHANNEL)
			{
				if (alarmType == com.dalsemi.onewire.container.MissionContainer_Fields.ALARM_HIGH)
				{
					if (hasHumiditySensor && !adForceResults)
						th = decodeHumidity(missionRegister, DATA_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
					else if (adForceResults)
						th = getADVoltage(missionRegister, DATA_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
					else
						th = (0x0FF & missionRegister[DATA_HIGH_ALARM_THRESHOLD & 0x3F]);
				}
				else
				{
					if (hasHumiditySensor && !adForceResults)
						th = decodeHumidity(missionRegister, DATA_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
					else if (adForceResults)
						th = getADVoltage(missionRegister, DATA_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
					else
						th = (0x0FF & missionRegister[DATA_LOW_ALARM_THRESHOLD & 0x3F]);
				}
			}
			else
			{
				throw new System.ArgumentException("Invalid Channel");
			}
			return th;
		}
		
		/// <summary> Sets the threshold value which will trigger the alarm of the
		/// specified type on the specified channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <param name="threshold">the threshold value which will trigger the alarm
		/// </param>
		public virtual void  setMissionAlarm(int channel, int alarmType, double threshold)
		{
			if (!isMissionUploaded)
				missionRegister = readDevice();
			
			//      System.out.println("setMissionAlarm: channel=" + channel +", alarmType=" + alarmType + ", threshold=" + threshold);
			//      System.out.println("setMissionAlarm: before state=" + Convert.toHexString(missionRegister));
			
			if (channel == TEMPERATURE_CHANNEL)
			{
				if (alarmType == com.dalsemi.onewire.container.MissionContainer_Fields.ALARM_HIGH)
				{
					encodeTemperature(threshold, missionRegister, TEMPERATURE_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
				}
				else
				{
					encodeTemperature(threshold, missionRegister, TEMPERATURE_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
				}
			}
			else if (channel == DATA_CHANNEL)
			{
				if (alarmType == com.dalsemi.onewire.container.MissionContainer_Fields.ALARM_HIGH)
				{
					if (hasHumiditySensor && !adForceResults)
						encodeHumidity(threshold, missionRegister, DATA_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
					else if (adForceResults)
						setADVoltage(threshold, missionRegister, DATA_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
					else
					{
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						missionRegister[DATA_HIGH_ALARM_THRESHOLD & 0x3F] = (byte) threshold;
					}
				}
				else
				{
					if (hasHumiditySensor && !adForceResults)
						encodeHumidity(threshold, missionRegister, DATA_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
					else if (adForceResults)
						setADVoltage(threshold, missionRegister, DATA_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
					else
					{
						//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
						missionRegister[DATA_LOW_ALARM_THRESHOLD & 0x3F] = (byte) threshold;
					}
				}
			}
			else
			{
				throw new System.ArgumentException("Invalid Channel");
			}
			//      System.out.println("setMissionAlarm: after state=" + Convert.toHexString(missionRegister));
			writeDevice(missionRegister);
		}
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Temperature Interface Functions
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
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
		public virtual bool hasTemperatureAlarms()
		{
			return true;
		}
		
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
		public virtual bool hasSelectableTemperatureResolution()
		{
			return false;
		}
		
		/// <summary> Performs a temperature conversion.  Use the <code>state</code>
		/// information to calculate the conversion time.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// In the case of the DS1922 Thermocron, this could also be due to a
		/// currently running mission.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		public virtual void  doTemperatureConvert(byte[] state)
		{
			/* check for mission in progress */
			if (getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MISSION_IN_PROGRESS, state))
				throw new OneWireIOException("OneWireContainer41-Cant force " + "temperature read during a mission.");
			/* check that the RTC is running */
			if (!getFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_OSCILLATOR, state))
				throw new OneWireIOException("OneWireContainer41-Cant force " + "temperature conversion if the oscillator is not enabled");
			
			/* get the temperature*/
			if (doSpeedEnable)
				doSpeed(); //we aren't worried about how long this takes...we're sleeping for 750 ms!
			
			adapter.reset();
			
			if (adapter.select(address))
			{
				// perform the temperature conversion
				byte[] buffer = new byte[]{FORCED_CONVERSION, (byte) SupportClass.Identity(0xFF)};
				adapter.dataBlock(buffer, 0, 2);
				
				msWait(750);
				
				// grab the temperature
				state[LAST_TEMPERATURE_CONVERSION_LSB & 0x3F] = readByte(LAST_TEMPERATURE_CONVERSION_LSB);
				state[LAST_TEMPERATURE_CONVERSION_MSB & 0x3F] = readByte(LAST_TEMPERATURE_CONVERSION_MSB);
			}
			else
				throw new OneWireException("OneWireContainer41-Device not found!");
		}
		
		/// <summary> Gets the temperature value in Celsius from the <code>state</code>
		/// data retrieved from the <code>readDevice()</code> method.
		/// 
		/// </summary>
		/// <param name="state">byte array with device state information
		/// 
		/// </param>
		/// <returns> temperature in Celsius from the last
		/// <code>doTemperatureConvert()</code>
		/// </returns>
		public virtual double getTemperature(byte[] state)
		{
			double val = decodeTemperature(state, LAST_TEMPERATURE_CONVERSION_LSB & 0x3F, 2, false);
			if (useTempCalibrationRegisters && ((System.Object) partNumber != (System.Object) PART_NUMBER_DS1922E)) // DS1922E does not use calibration registers
			{
                if ((partNumber == PART_NUMBER_DS1922F) && (val < 130.0))
                { 
                    // do nothing -- the DS1922F only gets software corrected above or equal to 130 degrees Celsius
                } 
                else 
                { 
                    double valsq = val * val;
                    double error
                        = tempCoeffA * valsq + tempCoeffB * val + tempCoeffC;
                    val = val - error;
                } 
            }
			return val;
		}
		
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
		/// <seealso cref="hasTemperatureAlarms">
		/// </seealso>
		/// <seealso cref="setTemperatureAlarm">
		/// </seealso>
		public virtual double getTemperatureAlarm(int alarmType, byte[] state)
		{
			double th = 0;
			if (alarmType == com.dalsemi.onewire.container.TemperatureContainer_Fields.ALARM_HIGH)
				th = decodeTemperature(state, TEMPERATURE_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
			else
				th = decodeTemperature(state, TEMPERATURE_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
			if (useTempCalibrationRegisters && ((System.Object) partNumber != (System.Object) PART_NUMBER_DS1922E))// DS1922E does not use calibration registers
			{
	            if ((partNumber == PART_NUMBER_DS1922F) && (th < 130.0))
		        { 
		          // do nothing -- the DS1922F only gets software corrected above or equal to 130 degrees Celsius
		        } 
		        else 
		        { 		
			        double thsq = th*th;
			        double error
				        = tempCoeffA*thsq + tempCoeffB*th + tempCoeffC;
			        th = th - error;
		        }
            }
			return th;
		}
		
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
		public virtual double getTemperatureResolution(byte[] state)
		{
			return temperatureResolutions[1];
		}
		
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
			if (useTempCalibrationRegisters && ((System.Object) partNumber != (System.Object) PART_NUMBER_DS1922E)) // DS1922E does not use calibration registers
			{
                if ((partNumber == PART_NUMBER_DS1922F) && (alarmValue < 130.0)) 
                { 
                    // do nothing -- the DS1922F only gets software corrected above or equal to 130 degrees Celsius
                } 
                else 
                { 
                    alarmValue = ((1 - tempCoeffB) - System.Math.Sqrt(((tempCoeffB - 1) * (tempCoeffB - 1)) - 4 * tempCoeffA * (tempCoeffC + alarmValue))) / (2 * tempCoeffA);
                } 
			}
			
			if (alarmType == com.dalsemi.onewire.container.TemperatureContainer_Fields.ALARM_HIGH)
			{
				encodeTemperature(alarmValue, state, TEMPERATURE_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
			}
			else
			{
				encodeTemperature(alarmValue, state, TEMPERATURE_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
			}
		}
		
		
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
		/// <throws>  OneWireException if the device does not support </throws>
		/// <summary> selectable temperature resolution
		/// 
		/// </summary>
		/// <seealso cref="hasSelectableTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolution">
		/// </seealso>
		/// <seealso cref="getTemperatureResolutions">
		/// </seealso>
		public virtual void  setTemperatureResolution(double resolution, byte[] state)
		{
			throw new OneWireException("Selectable Temperature Resolution Not Supported");
		}
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Humidity Interface Functions
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
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
			return true;
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
			/* check for mission in progress */
			if (getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MISSION_IN_PROGRESS, state))
				throw new OneWireIOException("OneWireContainer41-Cant force " + "Humidity read during a mission.");
			
			/* check that the RTC is running */
			if (!getFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_OSCILLATOR, state))
				throw new OneWireIOException("OneWireContainer41-Cant force " + "Humidity conversion if the oscillator is not enabled");
			
			/* get the temperature*/
			if (doSpeedEnable)
				doSpeed(); //we aren't worried about how long this takes...we're sleeping for 750 ms!
			
			adapter.reset();
			
			if (adapter.select(address))
			{
				// perform the temperature conversion
				byte[] buffer = new byte[]{FORCED_CONVERSION, (byte) SupportClass.Identity(0xFF)};
				adapter.dataBlock(buffer, 0, 2);
				
				msWait(750);
				
				// grab the temperature
				state[LAST_DATA_CONVERSION_LSB & 0x3F] = readByte(LAST_DATA_CONVERSION_LSB);
				state[LAST_DATA_CONVERSION_MSB & 0x3F] = readByte(LAST_DATA_CONVERSION_MSB);
			}
			else
				throw new OneWireException("OneWireContainer41-Device not found!");
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
			double val = decodeHumidity(state, LAST_DATA_CONVERSION_LSB & 0x3F, 2, false);
			if (useTemperatureCompensation)
			{
				double T = decodeTemperature(state, LAST_TEMPERATURE_CONVERSION_LSB & 0x3F, 2, false);
				double gamma = (T > 15)?0.00001:- 0.00005;
				T -= 25;
				val = (val * 0.0307 + .0035 * T - 0.000043 * T * T) / (0.0307 + gamma * T - 0.000002 * T * T);
			}
			return val;
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
			return humidityResolutions[1];
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
			double th;
			if (alarmType == com.dalsemi.onewire.container.HumidityContainer_Fields.ALARM_HIGH)
				th = decodeHumidity(state, DATA_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
			else
				th = decodeHumidity(state, DATA_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
			return th;
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
			if (alarmType == com.dalsemi.onewire.container.HumidityContainer_Fields.ALARM_HIGH)
				encodeHumidity(alarmValue, state, DATA_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
			else
				encodeHumidity(alarmValue, state, DATA_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
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
			throw new OneWireException("Selectable Humidity Resolution Not Supported");
		}
		
		/// <summary> Checks to see if this A/D measuring device has high/low
		/// alarms.
		/// 
		/// </summary>
		/// <returns> true if this device has high/low trip alarms
		/// </returns>
		public virtual bool hasADAlarms()
		{
			return true;
		}
		
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
		public virtual double[] getADRanges(int channel)
		{
			return new double[]{127};
		}
		
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
		public virtual double[] getADResolutions(int channel, double range)
		{
			return new double[]{dataResolutions[1]};
		}
		
		/// <summary> Checks to see if this A/D supports doing multiple voltage
		/// conversions at the same time.
		/// 
		/// </summary>
		/// <returns> true if the device can do multi-channel voltage reads
		/// 
		/// </returns>
		/// <seealso cref="doADConvert(boolean[],byte[])">
		/// </seealso>
		public virtual bool canADMultiChannelRead()
		{
			return true;
		}
		
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
		public virtual void  doADConvert(int channel, byte[] state)
		{
			/* check for mission in progress */
			if (getFlag(GENERAL_STATUS_REGISTER, GSR_BIT_MISSION_IN_PROGRESS, state))
				throw new OneWireIOException("OneWireContainer41-Cant force " + "temperature read during a mission.");
			
			/* check that the RTC is running */
			if (!getFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_OSCILLATOR, state))
				throw new OneWireIOException("OneWireContainer41-Cant force " + "A/D conversion if the oscillator is not enabled");
			
			/* get the temperature*/
			if (doSpeedEnable)
				doSpeed(); //we aren't worried about how long this takes...we're sleeping for 750 ms!
			
			adapter.reset();
			
			if (adapter.select(address))
			{
				// perform the conversion
				byte[] buffer = new byte[]{FORCED_CONVERSION, (byte) SupportClass.Identity(0xFF)};
				adapter.dataBlock(buffer, 0, 2);
				
				msWait(750);
				
				// grab the data
				state[LAST_DATA_CONVERSION_LSB & 0x3F] = readByte(LAST_DATA_CONVERSION_LSB);
				state[LAST_DATA_CONVERSION_MSB & 0x3F] = readByte(LAST_DATA_CONVERSION_MSB);
			}
			else
				throw new OneWireException("OneWireContainer41-Device not found!");
		}
		
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
		public virtual void  doADConvert(bool[] doConvert, byte[] state)
		{
			doADConvert(DATA_CHANNEL, state);
		}
		
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
		public virtual double[] getADVoltage(byte[] state)
		{
			return new double[]{getADVoltage(DATA_CHANNEL, state)};
		}
		
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
		public virtual double getADVoltage(int channel, byte[] state)
		{
			return getADVoltage(state, LAST_DATA_CONVERSION_LSB & 0x3F, 2, false);
		}
		
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
		public virtual double getADAlarm(int channel, int alarmType, byte[] state)
		{
			double th = 0;
			if (alarmType == com.dalsemi.onewire.container.ADContainer_Fields.ALARM_HIGH)
			{
				th = getADVoltage(state, DATA_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
			}
			else
			{
				th = getADVoltage(state, DATA_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
			}
			return th;
		}
		
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
		public virtual bool getADAlarmEnable(int channel, int alarmType, byte[] state)
		{
			bool b = false;
			if (alarmType == com.dalsemi.onewire.container.ADContainer_Fields.ALARM_HIGH)
			{
				b = getFlag(DATA_CONTROL_REGISTER, DCR_BIT_ENABLE_DATA_HIGH_ALARM, state);
			}
			else
			{
				b = getFlag(DATA_CONTROL_REGISTER, DCR_BIT_ENABLE_DATA_LOW_ALARM, state);
			}
			return b;
		}
		
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
		public virtual bool hasADAlarmed(int channel, int alarmType, byte[] state)
		{
			bool b = false;
			if (alarmType == com.dalsemi.onewire.container.ADContainer_Fields.ALARM_HIGH)
			{
				b = getFlag(ALARM_STATUS_REGISTER, ASR_BIT_DATA_HIGH_ALARM, state);
			}
			else
			{
				b = getFlag(ALARM_STATUS_REGISTER, ASR_BIT_DATA_LOW_ALARM, state);
			}
			return b;
		}
		
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
		public virtual double getADResolution(int channel, byte[] state)
		{
			return dataResolutions[1];
		}
		
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
		public virtual double getADRange(int channel, byte[] state)
		{
			return 127;
		}
		
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
		public virtual void  setADAlarm(int channel, int alarmType, double alarm, byte[] state)
		{
			if (alarmType == com.dalsemi.onewire.container.ADContainer_Fields.ALARM_HIGH)
			{
				setADVoltage(alarm, state, DATA_HIGH_ALARM_THRESHOLD & 0x3F, 1, false);
			}
			else
			{
				setADVoltage(alarm, state, DATA_LOW_ALARM_THRESHOLD & 0x3F, 1, false);
			}
		}
		
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
		public virtual void  setADAlarmEnable(int channel, int alarmType, bool alarmEnable, byte[] state)
		{
			if (alarmType == com.dalsemi.onewire.container.ADContainer_Fields.ALARM_HIGH)
			{
				setFlag(DATA_CONTROL_REGISTER, DCR_BIT_ENABLE_DATA_HIGH_ALARM, alarmEnable, state);
			}
			else
			{
				setFlag(DATA_CONTROL_REGISTER, DCR_BIT_ENABLE_DATA_LOW_ALARM, alarmEnable, state);
			}
		}
		
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
		public virtual void  setADResolution(int channel, double resolution, byte[] state)
		{
			//throw new OneWireException("Selectable A-D Resolution Not Supported");
		}
		
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
		public virtual void  setADRange(int channel, double range, byte[] state)
		{
			//throw new OneWireException("Selectable A-D Range Not Supported");
		}
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Clock Interface Functions
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		/// <summary> Checks to see if the clock has an alarm feature.
		/// 
		/// </summary>
		/// <returns> false, since this device does not have clock alarms
		/// 
		/// </returns>
		/// <seealso cref="getClockAlarm(byte[])">
		/// </seealso>
		/// <seealso cref="isClockAlarmEnabled(byte[])">
		/// </seealso>
		/// <seealso cref="isClockAlarming(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarm(long,byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarmEnable(boolean,byte[])">
		/// </seealso>
		public virtual bool hasClockAlarm()
		{
			return false;
		}
		
		/// <summary> Checks to see if the clock can be disabled.
		/// 
		/// </summary>
		/// <returns> true if the clock can be enabled and disabled
		/// 
		/// </returns>
		/// <seealso cref="isClockRunning(byte[])">
		/// </seealso>
		/// <seealso cref="setClockRunEnable(boolean,byte[])">
		/// </seealso>
		public virtual bool canDisableClock()
		{
			return true;
		}
		
		//--------
		//-------- Clock 'get' Methods
		//--------
		
		/// <summary> Extracts the Real-Time clock value in milliseconds.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> the time represented in this clock in milliseconds since 1970
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="setClock(long,byte[])">
		/// </seealso>
		public virtual long getClock(byte[] state)
		{
			//grab the time
			int[] time = getTime(RTC_TIME & 0x3F, state);
			//grab the date
			int[] date = getDate(RTC_DATE & 0x3F, state);
			
			//date[1] - 1 because Java months are 0 offset
			System.Globalization.GregorianCalendar temp_calendar;
			temp_calendar = new System.Globalization.GregorianCalendar();
			SupportClass.CalendarManager.manager.Set(temp_calendar, date[0], date[1] - 1, date[2], time[2], time[1], time[0]);
			System.Globalization.Calendar d = temp_calendar;
			
			//UPGRADE_TODO: Method 'java.util.Date.getTime' was converted to 'System.DateTime.Ticks' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDategetTime'"
			//UPGRADE_TODO: The equivalent in .NET for method 'java.util.Calendar.getTime' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return SupportClass.CalendarManager.manager.GetDateTime(d).Ticks;
		}
		
		/// <summary> Extracts the clock alarm value for the Real-Time clock.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> milliseconds since 1970 that the clock alarm is set to
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="hasClockAlarm()">
		/// </seealso>
		/// <seealso cref="isClockAlarmEnabled(byte[])">
		/// </seealso>
		/// <seealso cref="isClockAlarming(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarm(long,byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarmEnable(boolean,byte[])">
		/// </seealso>
		//public virtual long getClockAlarm(byte[] state)
        public virtual DateTime getClockAlarm(byte[] state)
		{
			throw new OneWireException("Device does not support clock alarms");
		}
		
		/// <summary> Checks if the clock alarm flag has been set.
		/// This will occur when the value of the Real-Time
		/// clock equals the value of the clock alarm.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> true if the Real-Time clock is alarming
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="hasClockAlarm()">
		/// </seealso>
		/// <seealso cref="isClockAlarmEnabled(byte[])">
		/// </seealso>
		/// <seealso cref="getClockAlarm(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarm(long,byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarmEnable(boolean,byte[])">
		/// </seealso>
		public virtual bool isClockAlarming(byte[] state)
		{
			return false;
		}
		
		/// <summary> Checks if the clock alarm is enabled.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> true if clock alarm is enabled
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="hasClockAlarm()">
		/// </seealso>
		/// <seealso cref="isClockAlarming(byte[])">
		/// </seealso>
		/// <seealso cref="getClockAlarm(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarm(long,byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarmEnable(boolean,byte[])">
		/// </seealso>
		public virtual bool isClockAlarmEnabled(byte[] state)
		{
			return false;
		}
		
		/// <summary> Checks if the device's oscillator is enabled.  The clock
		/// will not increment if the clock oscillator is not enabled.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> true if the clock is running
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="canDisableClock()">
		/// </seealso>
		/// <seealso cref="setClockRunEnable(boolean,byte[])">
		/// </seealso>
		public virtual bool isClockRunning(byte[] state)
		{
			return getFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_OSCILLATOR, state);
		}
		
		//--------
		//-------- Clock 'set' Methods
		//--------
		
		/// <summary> Sets the Real-Time clock.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.
		/// 
		/// </summary>
		/// <param name="time">new value for the Real-Time clock, in milliseconds
		/// since January 1, 1970
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="getClock(byte[])">
		/// </seealso>
		public virtual void  setClock(long time, byte[] state)
		{
			//UPGRADE_TODO: Constructor 'java.util.Date.Date' was converted to 'System.DateTime.DateTime' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilDateDate_long'"
			System.DateTime x = new System.DateTime(time);
			System.Globalization.Calendar d = new System.Globalization.GregorianCalendar();
			
			//UPGRADE_TODO: The differences in the format  of parameters for method 'java.util.Calendar.setTime'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			SupportClass.CalendarManager.manager.SetDateTime(d, x);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			setTime(RTC_TIME & 0x3F, SupportClass.CalendarManager.manager.Get(d, SupportClass.CalendarManager.HOUR_OF_DAY), SupportClass.CalendarManager.manager.Get(d, SupportClass.CalendarManager.MINUTE), SupportClass.CalendarManager.manager.Get(d, SupportClass.CalendarManager.SECOND), false, state);
			//UPGRADE_TODO: Method 'java.util.Calendar.get' was converted to 'SupportClass.CalendarManager.Get' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilCalendarget_int'"
			setDate(RTC_DATE & 0x3F, SupportClass.CalendarManager.manager.Get(d, SupportClass.CalendarManager.YEAR), SupportClass.CalendarManager.manager.Get(d, SupportClass.CalendarManager.MONTH) + 1, SupportClass.CalendarManager.manager.Get(d, SupportClass.CalendarManager.DATE), state);
			
			if (!getFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_OSCILLATOR, state))
				setFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_OSCILLATOR, true, state);
			
			lock (this)
			{
				updatertc = true;
			}
		}
		
		/// <summary> Sets the clock alarm.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.  Also note that
		/// not all clock devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasClockAlarm()</code> method.
		/// 
		/// </summary>
		/// <param name="time">- new value for the Real-Time clock alarm, in milliseconds
		/// since January 1, 1970
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <throws>  OneWireException if this device does not have clock alarms </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="hasClockAlarm()">
		/// </seealso>
		/// <seealso cref="isClockAlarmEnabled(byte[])">
		/// </seealso>
		/// <seealso cref="getClockAlarm(byte[])">
		/// </seealso>
		/// <seealso cref="isClockAlarming(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarmEnable(boolean,byte[])">
		/// </seealso>
		public virtual void  setClockAlarm(long time, byte[] state)
		{
			throw new OneWireException("Device does not support clock alarms");
		}
		
		/// <summary> Enables or disables the oscillator, turning the clock 'on' and 'off'.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.  Also note that
		/// not all clock devices can disable their oscillators.  Check to see if this device can
		/// disable its oscillator first by calling the <code>canDisableClock()</code> method.
		/// 
		/// </summary>
		/// <param name="runEnable">true to enable the clock oscillator
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="canDisableClock()">
		/// </seealso>
		/// <seealso cref="isClockRunning(byte[])">
		/// </seealso>
		public virtual void  setClockRunEnable(bool runEnable, byte[] state)
		{
			setFlag(RTC_CONTROL_REGISTER, RCR_BIT_ENABLE_OSCILLATOR, runEnable, state);
		}
		
		/// <summary> Enables or disables the clock alarm.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.  Also note that
		/// not all clock devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasClockAlarm()</code> method.
		/// 
		/// </summary>
		/// <param name="alarmEnable">true to enable the clock alarm
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="hasClockAlarm()">
		/// </seealso>
		/// <seealso cref="isClockAlarmEnabled(byte[])">
		/// </seealso>
		/// <seealso cref="getClockAlarm(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarm(long,byte[])">
		/// </seealso>
		/// <seealso cref="isClockAlarming(byte[])">
		/// </seealso>
		public virtual void  setClockAlarmEnable(bool alarmEnable, byte[] state)
		{
			throw new OneWireException("Device does not support clock alarms");
		}
		
		/// <summary> Gets the time of day fields in 24-hour time from button
		/// returns int[] = {seconds, minutes, hours}
		/// 
		/// </summary>
		/// <param name="timeReg">which register offset to pull the time from
		/// </param>
		/// <param name="state">acquired from call to readDevice
		/// </param>
		/// <returns> array representing {seconds, minutes, hours}
		/// </returns>
		private int[] getTime(int timeReg, byte[] state)
		{
			byte upper, lower;
			int[] result = new int[3];
			
			// First grab the seconds. Upper half holds the 10's of seconds
			lower = state[timeReg++];
			upper = (byte) ((SupportClass.URShift(lower, 4)) & 0x07);
			lower = (byte) (lower & 0x0f);
			result[0] = (int) lower + (int) upper * 10;
			
			// now grab minutes. The upper half holds the 10s of minutes
			lower = state[timeReg++];
			upper = (byte) ((SupportClass.URShift(lower, 4)) & 0x07);
			lower = (byte) (lower & 0x0f);
			result[1] = (int) lower + (int) upper * 10;
			
			// now grab the hours. The lower half is single hours again, but the
			// upper half of the byte is determined by the 2nd bit - specifying
			// 12/24 hour time.
			lower = state[timeReg];
			upper = (byte) ((SupportClass.URShift(lower, 4)) & 0x07);
			lower = (byte) (lower & 0x0f);
			
			byte PM = 0;
			// if the 2nd bit is 1, convert 12 hour time to 24 hour time.
			if ((upper & 0x04) != 0)
			{
				// extract the AM/PM byte (PM is indicated by a 1)
				if ((upper & 0x02) > 0)
					PM = 12;
				
				// isolate the 10s place
				upper &= (byte) (0x01);
			}
			
			result[2] = (int) (upper * 10) + (int) lower + (int) PM;
			
			return result;
		}
		
		/// <summary> Set the time in the DS1922 time register format.</summary>
		private void  setTime(int timeReg, int hours, int minutes, int seconds, bool AMPM, byte[] state)
		{
			byte upper, lower;
			
			/* format in bytes and write seconds */
			upper = (byte) (((seconds / 10) << 4) & 0xf0);
			lower = (byte) ((seconds % 10) & 0x0f);
			state[timeReg++] = (byte) (upper | lower);
			
			/* format in bytes and write minutes */
			upper = (byte) (((minutes / 10) << 4) & 0xf0);
			lower = (byte) ((minutes % 10) & 0x0f);
			state[timeReg++] = (byte) (upper | lower);
			
			/* format in bytes and write hours/(12/24) bit */
			if (AMPM)
			{
				upper = (byte) 0x04;
				
				if (hours > 11)
					upper = (byte) (upper | 0x02);
				
				// this next logic simply checks for a decade hour
				if (((hours % 12) == 0) || ((hours % 12) > 9))
					upper = (byte) (upper | 0x01);
				
				if (hours > 12)
					hours = hours - 12;
				
				if (hours == 0)
					lower = (byte) 0x02;
				else
					lower = (byte) ((hours % 10) & 0x0f);
			}
			else
			{
				upper = (byte) (hours / 10);
				lower = (byte) (hours % 10);
			}
			
			upper = (byte) ((upper << 4) & 0xf0);
			lower = (byte) (lower & 0x0f);
			state[timeReg] = (byte) (upper | lower);
		}
		
		/// <summary> Grab the date from one of the time registers.
		/// returns int[] = {year, month, date}
		/// 
		/// </summary>
		/// <param name="timeReg">which register offset to pull the date from
		/// </param>
		/// <param name="state">acquired from call to readDevice
		/// </param>
		/// <returns> array representing {year, month, date}
		/// </returns>
		private int[] getDate(int timeReg, byte[] state)
		{
			byte upper, lower;
			int[] result = new int[]{0, 0, 0};
			
			/* extract the day of the month */
			lower = state[timeReg++];
			upper = (byte) ((SupportClass.URShift(lower, 4)) & 0x0f);
			lower = (byte) (lower & 0x0f);
			result[2] = upper * 10 + lower;
			
			/* extract the month */
			lower = state[timeReg++];
			if ((lower & 0x80) == 0x80)
				result[0] = 100;
			upper = (byte) ((SupportClass.URShift(lower, 4)) & 0x01);
			lower = (byte) (lower & 0x0f);
			result[1] = upper * 10 + lower;
			
			/* grab the year */
			lower = state[timeReg++];
			upper = (byte) ((SupportClass.URShift(lower, 4)) & 0x0f);
			lower = (byte) (lower & 0x0f);
			result[0] += upper * 10 + lower + FIRST_YEAR_EVER;
			
			return result;
		}
		
		/// <summary> Set the current date in the DS1922's real time clock.
		/// 
		/// year - The year to set to, i.e. 2001.
		/// month - The month to set to, i.e. 1 for January, 12 for December.
		/// day - The day of month to set to, i.e. 1 to 31 in January, 1 to 30 in April.
		/// </summary>
		private void  setDate(int timeReg, int year, int month, int day, byte[] state)
		{
			byte upper, lower;
			
			/* write the day byte (the upper holds 10s of days, lower holds single days) */
			upper = (byte) (((day / 10) << 4) & 0xf0);
			lower = (byte) ((day % 10) & 0x0f);
			state[timeReg++] = (byte) (upper | lower);
			
			/* write the month bit in the same manner, with the MSBit indicating
			the century (1 for 2000, 0 for 1900) */
			upper = (byte) (((month / 10) << 4) & 0xf0);
			lower = (byte) ((month % 10) & 0x0f);
			state[timeReg++] = (byte) (upper | lower);
			
			// now write the year
			year = year - FIRST_YEAR_EVER;
			if (year > 100)
			{
				state[timeReg - 1] |= (byte) SupportClass.Identity(0x80);
				year -= 100;
			}
			upper = (byte) (((year / 10) << 4) & 0xf0);
			lower = (byte) ((year % 10) & 0x0f);
			state[timeReg] = (byte) (upper | lower);
		}
		
		
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Private initilizers
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		/// <summary> Construct the memory banks used for I/O.</summary>
		private void  initMem()
		{
			
			// scratchpad
			scratch = new MemoryBankScratchCRCPW(this);
			
			// User Data Memory
			userDataMemory = new MemoryBankNVCRCPW(this, scratch);
			userDataMemory.numberPages = 16;
			userDataMemory.size = 512;
			userDataMemory.bankDescription = "User Data Memory";
			userDataMemory.startPhysicalAddress = 0x0000;
			userDataMemory.generalPurposeMemory = true;
			userDataMemory.readOnly = false;
			userDataMemory.readWrite = true;
			
			// Register
			register = new MemoryBankNVCRCPW(this, scratch);
			register.numberPages = 32;
			register.size = 1024;
			register.bankDescription = "Register control";
			register.startPhysicalAddress = 0x0200;
			register.generalPurposeMemory = false;
			
			// Data Log
			log = new MemoryBankNVCRCPW(this, scratch);
			log.numberPages = 256;
			log.size = 8192;
			log.bankDescription = "Data log";
			log.startPhysicalAddress = 0x1000;
			log.generalPurposeMemory = false;
			log.readOnly = true;
			log.readWrite = false;
		}
		
		/// <summary> helper method to pause for specified milliseconds</summary>
		private static void  msWait(long ms)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
				System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * ms));
			}
			catch (System.Threading.ThreadInterruptedException ie)
			{
				;
			}
		}
		
		/// <summary> helper method for decoding temperature values</summary>
		private double decodeTemperature(byte[] data, int offset, int length, bool reverse)
		{
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			if (DEBUG)
				Debug.debug("decodeTemperature, data", data, offset, length);
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			double whole, fraction = 0;
			if (reverse && length == 2)
			{
				fraction = ((data[offset + 1] & 0x0FF) / 512d);
				whole = (data[offset] & 0x0FF) / 2d + (temperatureRangeLow - 1);
			}
			else if (length == 2)
			{
				fraction = ((data[offset] & 0x0FF) / 512d);
				whole = (data[offset + 1] & 0x0FF) / 2d + (temperatureRangeLow - 1);
			}
			else
			{
				whole = (data[offset] & 0x0FF) / 2d + (temperatureRangeLow - 1);
			}
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			if (DEBUG)
			{
				Debug.debug("decodeTemperature, temperatureRangeLow= " + temperatureRangeLow);
				Debug.debug("decodeTemperature, whole= " + whole);
				Debug.debug("decodeTemperature, fraction= " + fraction);
				Debug.debug("decodeTemperature, (whole+fraction)= " + (whole + fraction));
			}
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			return whole + fraction;
		}
		
		/// <summary> helper method for encoding temperature values</summary>
		private void  encodeTemperature(double temperature, byte[] data, int offset, int length, bool reverse)
		{
			double val = 2 * ((temperature) - (temperatureRangeLow - 1));
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			if (DEBUG)
				Debug.debug("encodeTemperature, temperature=" + temperature + ", temperatureRangeLow=" + temperatureRangeLow + ", val=" + val);
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			
			if (reverse && length == 2)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				data[offset + 1] = (byte) (0x0C0 & (byte) (val * 256));
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				data[offset] = (byte) val;
			}
			else if (length == 2)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				data[offset] = (byte) (0x0C0 & (byte) (val * 256));
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				data[offset + 1] = (byte) val;
			}
			else
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				data[offset] = (byte) val;
			}
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			if (DEBUG)
				Debug.debug("encodeTemperature, data", data, offset, length);
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		}
		
		/// <summary> helper method for decoding humidity values</summary>
		private double decodeHumidity(byte[] data, int offset, int length, bool reverse)
		{
			// get the 10-bit value of Vout
			double val = getADVoltage(data, offset, length, reverse);
			
			// convert Vout to a humidity reading
			// this formula is from HIH-3610 sensor datasheet
			val = (val - .958) / .0307;
			
			if (useHumdCalibrationRegisters)
			{
				double valsq = val * val;
				double error = humdCoeffA * valsq + humdCoeffB * val + humdCoeffC;
				val = val - error;
			}
			
			return val;
		}
		
		/// <summary> helper method for encoding humidity values</summary>
		private void  encodeHumidity(double humidity, byte[] data, int offset, int length, bool reverse)
		{
			// uncalibrate the alarm value before writing
			if (useHumdCalibrationRegisters)
			{
				humidity = ((1 - humdCoeffB) - System.Math.Sqrt(((humdCoeffB - 1) * (humdCoeffB - 1)) - 4 * humdCoeffA * (humdCoeffC + humidity))) / (2 * humdCoeffA);
			}
			
			// convert humidity value to Vout value
			double val = (humidity * .0307) + .958;
			// convert Vout to byte[]
			setADVoltage(val, data, offset, length, reverse);
		}
		
		private double getADVoltage(byte[] data, int offset, int length, bool reverse)
		{
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			if (DEBUG)
				Debug.debug("getADVoltage, data", data, offset, length);
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			// get the 10-bit value of vout
			int ival = 0;
			if (reverse && length == 2)
			{
				ival = ((data[offset] & 0x0FF) << (adDeviceBits - 8));
				ival |= ((data[offset + 1] & 0x0FF) >> (16 - adDeviceBits));
			}
			else if (length == 2)
			{
				ival = ((data[offset + 1] & 0x0FF) << (adDeviceBits - 8));
				ival |= ((data[offset] & 0x0FF) >> (16 - adDeviceBits));
			}
			else
			{
				ival = ((data[offset] & 0x0FF) << (adDeviceBits - 8));
			}
			
			double dval = (ival * adReferenceVoltage) / (1 << adDeviceBits);
			
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			if (DEBUG)
			{
				Debug.debug("getADVoltage, ival=" + ival);
				Debug.debug("getADVoltage, voltage=" + dval);
			}
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			return dval;
		}
		
		private void  setADVoltage(double voltage, byte[] data, int offset, int length, bool reverse)
		{
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			if (DEBUG)
				Debug.debug("setADVoltage, voltage=" + voltage);
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int val = (int) ((voltage * (1 << adDeviceBits)) / adReferenceVoltage);
			val = val & ((1 << adDeviceBits) - 1);
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			if (DEBUG)
				Debug.debug("setADVoltage, val=" + val);
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			if (reverse && length == 2)
			{
				data[offset] = (byte) (val >> (adDeviceBits - 8));
				data[offset + 1] = (byte) (val << (16 - adDeviceBits));
			}
			else if (length == 2)
			{
				data[offset + 1] = (byte) (val >> (adDeviceBits - 8));
				data[offset] = (byte) (val << (16 - adDeviceBits));
			}
			else
			{
				data[offset] = (byte) ((val & 0x3FC) >> (adDeviceBits - 8));
			}
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
			if (DEBUG)
				Debug.debug("setADVoltage, data", data, offset, length);
			//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		}
	}
}