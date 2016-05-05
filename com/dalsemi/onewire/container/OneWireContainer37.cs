/*---------------------------------------------------------------------------
* Copyright (C) 2003 Maxim Integrated Products, All Rights Reserved.
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
using com.dalsemi.onewire.adapter;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> <P> 1-Wire&reg; container for a 32K bytes of read-only and read/write password
	/// protected memory, DS1977.  This container encapsulates the functionality
	/// of the 1-Wire family type <B>37</B> (hex).
	/// </P>
	/// 
	/// <H3> Features </H3>
	/// <UL>
	/// <LI> 32K bytes EEPROM organized as pages of 64 bytes.
	/// <LI> 512-bit scratchpad ensures integrity of data transfer
	/// <LI> On-chip 16-bit CRC generator
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
	/// <LI> <I> Size </I> 64 starting at physical address 0
	/// <LI> <I> Features</I> Read/Write not-general-purpose volatile
	/// <LI> <I> Pages</I> 1 page of length 64 bytes
	/// <LI> <I> Page Features </I> page-device-CRC
	/// <li> <i> Extra information for each page</i>  Target address, offset, length 3
	/// <LI> <i> Supports Copy Scratchpad With Password command </I>
	/// </UL>
	/// <LI> <B> Main Memory </B>
	/// <UL>
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 32704 starting at physical address 0
	/// <LI> <I> Features</I> Read/Write general-purpose non-volatile
	/// <LI> <I> Pages</I> 511 pages of length 64 bytes giving 61 bytes Packet data payload
	/// <LI> <I> Page Features </I> page-device-CRC
	/// <LI> <I> Read-Only and Read/Write password </I> if enabled, passwords are required for
	/// reading from and writing to the device.
	/// </UL>
	/// <LI> <B> Register control </B>
	/// <UL>
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 64 starting at physical address 32704
	/// <LI> <I> Features</I> Read/Write not-general-purpose non-volatile
	/// <LI> <I> Pages</I> 1 pages of length 64 bytes
	/// <LI> <I> Page Features </I> page-device-CRC
	/// <LI> <I> Read-Only and Read/Write password </I> if enabled, passwords are required for
	/// reading from and writing to the device.
	/// </UL>
	/// </UL>
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
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.PasswordContainer">
	/// 
	/// </seealso>
	/// <version>     1.00, 18 Aug 2003
	/// </version>
	/// <author>      jevans
	/// 
	/// </author>
	public class OneWireContainer37:OneWireContainer, PasswordContainer
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
				System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(3));
				
				v.Add(scratch);
				v.Add(userDataMemory);
				v.Add(register);
				
				return v.GetEnumerator();
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
		/// For example "DS1977".
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
				return "";
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
		/// <summary> Retrieves the password length for the read-only password.
		/// 
		/// </summary>
		/// <returns>  the read-only password length
		/// 
		/// </returns>
		/// <throws>  OneWireException </throws>
		virtual public int ReadOnlyPasswordLength
		{
			get
			{
				return PASSWORD_LENGTH;
			}
			
		}
		/// <summary> Retrieves the password length for the read/write password.
		/// 
		/// </summary>
		/// <returns>  the read/write password length
		/// 
		/// </returns>
		/// <throws>  OneWireException </throws>
		virtual public int ReadWritePasswordLength
		{
			get
			{
				return PASSWORD_LENGTH;
			}
			
		}
		/// <summary> Retrieves the password length for the write-only password.
		/// 
		/// </summary>
		/// <returns>  the write-only password length
		/// 
		/// </returns>
		/// <throws>  OneWireException </throws>
		virtual public int WriteOnlyPasswordLength
		{
			get
			{
				throw new OneWireException("The DS1977 does not have a write password.");
			}
			
		}
		/// <summary> Retrieves the address the read only password starts
		/// 
		/// </summary>
		/// <returns> the address of the read only password
		/// </returns>
		virtual public int ReadOnlyPasswordAddress
		{
			get
			{
				return READ_ACCESS_PASSWORD;
			}
			
		}
		/// <summary> Retrieves the address the read/write password starts
		/// 
		/// </summary>
		/// <returns> the address of the read/write password
		/// </returns>
		virtual public int ReadWritePasswordAddress
		{
			get
			{
				return READ_WRITE_ACCESS_PASSWORD;
			}
			
		}
		/// <summary> Retrieves the address the write only password starts
		/// 
		/// </summary>
		/// <returns> the address of the write only password
		/// </returns>
		virtual public int WriteOnlyPasswordAddress
		{
			get
			{
				throw new OneWireException("The DS1977 does not have a write password.");
			}
			
		}
		/// <summary> Tells whether the read only password has been enabled.
		/// 
		/// </summary>
		/// <returns>  the enabled status of the read only password
		/// 
		/// </returns>
		/// <throws>  OneWireException </throws>
		virtual public bool DeviceReadOnlyPasswordEnable
		{
			get
			{
				return readOnlyPasswordEnabled;
			}
			
		}
		/// <summary> Tells whether the read/write password has been enabled.
		/// 
		/// </summary>
		/// <returns>  the enabled status of the read/write password
		/// 
		/// </returns>
		/// <throws>  OneWireException </throws>
		virtual public bool DeviceReadWritePasswordEnable
		{
			get
			{
				return readWritePasswordEnabled;
			}
			
		}
		/// <summary> Tells whether the write only password has been enabled.
		/// 
		/// </summary>
		/// <returns>  the enabled status of the write only password
		/// 
		/// </returns>
		/// <throws>  OneWireException </throws>
		virtual public bool DeviceWriteOnlyPasswordEnable
		{
			get
			{
				throw new OneWireException("The DS1977 does not have a Write Only Password.");
			}
			
		}
		/// <summary> <p>Enables/Disables passwords for this device.  If the part has more than one
		/// type of password (Read-Only, Write-Only, or Read/Write), all passwords
		/// will be enabled.  This function is equivalent to the following:
		/// <code> owc37.setDevicePasswordEnable(
		/// owc37.hasReadOnlyPassword(),
		/// owc37.hasReadWritePassword(),
		/// owc37.hasWriteOnlyPassword() ); </code></p>
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
		/// <summary> Returns true if the container's read password has been set.  The return
		/// value is not affected by whether or not the read password of the container
		/// actually matches the value in the device's password register.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the container's read password has been set
		/// </returns>
		virtual public bool ContainerReadOnlyPasswordSet
		{
			get
			{
				return readPasswordSet;
			}
			
		}
		/// <summary> Returns true if the container's read/write password has been set.  The
		/// return value is not affected by whether or not the read/write password of
		/// the container actually matches the value in the device's password
		/// register.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the container's read/write password has been
		/// set.
		/// </returns>
		virtual public bool ContainerReadWritePasswordSet
		{
			get
			{
				return readWritePasswordSet;
			}
			
		}
		/// <summary> Returns true if the container's read/write password has been set.  The
		/// return value is not affected by whether or not the read/write password of
		/// the container actually matches the value in the device's password
		/// register.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the container's read/write password has been
		/// set.
		/// </returns>
		virtual public bool ContainerWriteOnlyPasswordSet
		{
			get
			{
				throw new OneWireException("The DS1977 does not have a write only password.");
			}
			
		}
		// enables/disables debugging
		private const bool DEBUG = false;
		
		// when reading a page, the memory bank may throw a crc exception if the device
		// is sampling or starts sampling during the read.  This value sets how many
		// times the device retries before passing the exception on to the application.
		private const int MAX_READ_RETRY_CNT = 15;
		
		// the length of the Read-Only and Read/Write password registers
		private const int PASSWORD_LENGTH = 8;
		
		// memory bank for scratchpad
		private MemoryBankScratchCRCPW scratch = null;
		// memory bank for general-purpose user data
		private MemoryBankNVCRCPW userDataMemory = null;
		// memory bank for control register
		private MemoryBankNVCRCPW register = null;
		
		// Maxim/Maxim Integrated Products Part number
		private System.String partNumber = "DS1977";
		
		// Letter appended at end of partNumber (S/H/L/T)
		//private char partLetter = '0'; // assigned but value never used
		
		
		// should we check the speed
		private bool doSpeedEnable = true;
		
		/// <summary> The current password for readingfrom this device.</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'readPassword '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		protected internal byte[] readPassword = new byte[8];
		protected internal bool readPasswordSet = false;
		
		/// <summary> The current password for reading/writing from/to this device.</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'readWritePassword '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		protected internal byte[] readWritePassword = new byte[8];
		protected internal bool readWritePasswordSet = false;
		
		// used to tell if the passwords have been enabled
		private bool readOnlyPasswordEnabled = false;
		private bool readWritePasswordEnabled = false;
		
		// used to 'enable' passwords
		private static byte ENABLE_BYTE = (byte) SupportClass.Identity(0xAA);
		// used to 'disable' passwords
		private const byte DISABLE_BYTE = (byte) (0x00);
		
		private System.String descriptionString = "Rugged, self-sufficient 1-Wire device that, once setup can " + "store 32KB of password protected memory with a read only " + "and a read/write password.";
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// 1-Wire Commands
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		/// <summary>1-Wire command for Write Scratchpad </summary>
		public static byte WRITE_SCRATCHPAD_COMMAND = (byte) 0x0F;
		/// <summary>1-Wire command for Read Scratchpad </summary>
		public static byte READ_SCRATCHPAD_COMMAND = (byte) SupportClass.Identity(0xAA);
		/// <summary>1-Wire command for Copy Scratchpad With Password </summary>
		public static byte COPY_SCRATCHPAD_PW_COMMAND = (byte) SupportClass.Identity(0x99);
		/// <summary>1-Wire command for Read Memory With Password </summary>
		public static byte READ_MEMORY_PW_COMMAND = (byte) 0x69;
		/// <summary>1-Wire command for Verifing the Password </summary>
		public static byte VERIFY_PSW_COMMAND = (byte) SupportClass.Identity(0xC3);
		/// <summary>1-Wire command for getting Read Version </summary>
		public static byte READ_VERSION = (byte) SupportClass.Identity(0xCC);
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Register addresses and control bits
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		// 1 byte, alternating ones and zeroes indicates passwords are enabled
		/// <summary>Address of the Password Control Register. </summary>
		public const int PASSWORD_CONTROL_REGISTER = 0x7FD0;
		
		// 8 bytes, write only, for setting the Read Access Password
		/// <summary>Address of Read Access Password. </summary>
		public const int READ_ACCESS_PASSWORD = 0x7FC0;
		
		// 8 bytes, write only, for setting the Read Access Password
		/// <summary>Address of the Read Write Access Password. </summary>
		public const int READ_WRITE_ACCESS_PASSWORD = 0x7FC8;
		
		public const int READ_WRITE_PWD = 0;
		public const int READ_ONLY_PWD = 1;
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Constructors and Initializers
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a
		/// DS1977.
		/// Note that the method <code>setupContainer(DSPortAdapter,byte[])</code>
		/// must be called to set the correct <code>DSPortAdapter</code> device address.
		/// 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer.setupContainer(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) setupContainer(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer37(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) OneWireContainer37(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer37(com.dalsemi.onewire.adapter.DSPortAdapter,long)   OneWireContainer37(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer37(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer37(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer37():base()
		{
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a
		/// DS1977.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       address of this DS1977
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer37()">
		/// </seealso>
		/// <seealso cref="OneWireContainer37(com.dalsemi.onewire.adapter.DSPortAdapter,long)   OneWireContainer37(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer37(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer37(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer37(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a
		/// DS1977.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       address of this DS1977
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer37()">
		/// </seealso>
		/// <seealso cref="OneWireContainer37(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) OneWireContainer37(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer37(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer37(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer37(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Creates a new <code>OneWireContainer</code> for communication with a
		/// DS1977.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       address of this DS1977
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer37()">
		/// </seealso>
		/// <seealso cref="OneWireContainer37(com.dalsemi.onewire.adapter.DSPortAdapter,long) OneWireContainer37(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer37(com.dalsemi.onewire.adapter.DSPortAdapter,java.lang.String) OneWireContainer37(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer37(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
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
		}
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Container Functions
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		// *****************************************************************************
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Read/Write Password Functions
		//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// *****************************************************************************
		
		/// <summary> Tells whether the device has a read only password.
		/// 
		/// </summary>
		/// <returns>  if the device has a read only password
		/// </returns>
		public virtual bool hasReadOnlyPassword()
		{
			return true;
		}
		
		/// <summary> Tells whether the device has a read/write password.
		/// 
		/// </summary>
		/// <returns>  if the device has a read/write password
		/// </returns>
		public virtual bool hasReadWritePassword()
		{
			return true;
		}
		
		/// <summary> Tells whether the device has a write only password.
		/// 
		/// </summary>
		/// <returns>  if the device has a write only password
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
		
		/// <summary> Attempts to change the value of the read password in the device's
		/// register. For this to be successful, either passwords must be disabled,
		/// or the read/write password for this container must be set and must match
		/// the value of the read/write password in the device's register.
		/// 
		/// <P><B>
		/// WARNING: Setting the read password requires that both the read password
		/// and the read/write password be written to the part.  Before calling this
		/// method, you should set the container read/write password value.
		/// This will ensure that the correct value is written into the part.
		/// </B></P>
		/// 
		/// </summary>
		/// <param name="password">the new value of 8-byte device read password, to be copied
		/// into the devices register.
		/// </param>
		/// <param name="offset">the offset to start copying the 8 bytes from the array
		/// </param>
		public virtual void  setDeviceReadOnlyPassword(byte[] password, int offset)
		{
			register.write(READ_ACCESS_PASSWORD & 0x3F, password, offset, 8);
			
			if (verifyPassword(password, offset, READ_ONLY_PWD))
				setContainerReadOnlyPassword(password, offset);
		}
		
		/// <summary> Attempts to change the value of the read/write password in the device's
		/// register.  For this to be successful, either passwords must be disabled,
		/// or the read/write password for this container must be set and must match
		/// the current value of the read/write password in the device's register.
		/// 
		/// </summary>
		/// <param name="password">the new value of 8-byte device read/write password, to be
		/// copied into the devices register.
		/// </param>
		/// <param name="offset">the offset to start copying the 8 bytes from the array
		/// </param>
		public virtual void  setDeviceReadWritePassword(byte[] password, int offset)
		{
			register.write(READ_WRITE_ACCESS_PASSWORD & 0x3F, password, offset, 8);
			
			if (verifyPassword(password, offset, READ_WRITE_PWD))
				setContainerReadWritePassword(password, offset);
		}
		
		/// <summary> Attempts to change the value of the write only password in the device's
		/// register.  For this to be successful, either passwords must be disabled,
		/// or the read/write password for this container must be set and must match
		/// the current value of the read/write password in the device's register.
		/// 
		/// </summary>
		/// <param name="password">the new value of 8-byte device read/write password, to be
		/// copied into the devices register.
		/// </param>
		/// <param name="offset">the offset to start copying the 8 bytes from the array
		/// </param>
		public virtual void  setDeviceWriteOnlyPassword(byte[] password, int offset)
		{
			throw new OneWireException("The DS1977 does not have a write only password.");
		}
		
		/// <summary> <P>Enables/disables passwords by writing to the devices password control
		/// register.  For this to be successful, either passwords must be disabled,
		/// or the read/write password for this container must be set and must match
		/// the current value of the read/write password in the device's register.</P>
		/// 
		/// <P><B>
		/// WARNING: Enabling passwords requires that both the read password and the
		/// read/write password be re-written to the part.  Before calling this method,
		/// you should set the container read password and read/write password values.
		/// This will ensure that the correct value is written into the part.
		/// </B></P>
		/// 
		/// </summary>
		/// <param name="enable">if <code>true</code>, device passwords will be enabled.
		/// All subsequent read and write operations will require that the
		/// passwords for the container are set.
		/// </param>
		public virtual void  setDevicePasswordEnable(bool enableReadOnly, bool enableReadWrite, bool enableWriteOnly)
		{
			if (enableWriteOnly)
				throw new OneWireException("The DS1922 does not have a write only password.");
			
			if (!ContainerReadOnlyPasswordSet && enableReadOnly)
				throw new OneWireException("Container Read Password is not set");
			if (!ContainerReadWritePasswordSet)
				throw new OneWireException("Container Read/Write Password is not set");
			if (enableReadOnly != enableReadWrite)
				throw new OneWireException("Both read only and read/write passwords " + "will both be disable or enabled");
			
			// must write both passwords for this to work
			byte[] enableCommand = new byte[1];
			enableCommand[0] = (enableReadWrite?ENABLE_BYTE:DISABLE_BYTE);
			
			register.write(PASSWORD_CONTROL_REGISTER & 0x3F, enableCommand, 0, 1);
			
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
		
		/// <summary> Sets the value of the read password for the container.  This is the value
		/// used by this container to read the memory of the device.  If this password
		/// does not match the value of the read password in the device's password
		/// register, all subsequent read operations will fail.
		/// 
		/// </summary>
		/// <param name="password">New 8-byte value of container's read password.
		/// </param>
		/// <param name="offset">Index to start copying the password from the array.
		/// </param>
		public virtual void  setContainerReadOnlyPassword(byte[] password, int offset)
		{
			Array.Copy(password, offset, readPassword, 0, PASSWORD_LENGTH);
			readPasswordSet = true;
		}
		
		/// <summary> Returns the read password used by this container to read the memory
		/// of the device.
		/// 
		/// </summary>
		/// <param name="password">Holds the 8-byte value of container's read password.
		/// </param>
		/// <param name="offset">Index to start copying the password into the array.
		/// </param>
		public virtual void  getContainerReadOnlyPassword(byte[] password, int offset)
		{
			Array.Copy(readPassword, 0, password, offset, PASSWORD_LENGTH);
		}
		
		/// <summary> Sets the value of the read/write password for the container.  This is the
		/// value used by this container to read and write to the memory of the
		/// device.  If this password does not match the value of the read/write
		/// password in the device's password register, all subsequent read and write
		/// operations will fail.
		/// 
		/// </summary>
		/// <param name="password">New 8-byte value of container's read/write password.
		/// </param>
		/// <param name="offset">Index to start copying the password from the array.
		/// </param>
		public virtual void  setContainerReadWritePassword(byte[] password, int offset)
		{
			Array.Copy(password, offset, readWritePassword, 0, 8);
			readWritePasswordSet = true;
		}
		
		/// <summary> Returns the read/write password used by this container to read from and
		/// write to the memory of the device.
		/// 
		/// </summary>
		/// <param name="password">Holds the 8-byte value of container's read/write password.
		/// </param>
		/// <param name="offset">Index to start copying the password into the array.
		/// </param>
		public virtual void  getContainerReadWritePassword(byte[] password, int offset)
		{
			Array.Copy(readWritePassword, 0, password, offset, PASSWORD_LENGTH);
		}
		
		/// <summary> Sets the value of the read/write password for the container.  This is the
		/// value used by this container to read and write to the memory of the
		/// device.  If this password does not match the value of the read/write
		/// password in the device's password register, all subsequent read and write
		/// operations will fail.
		/// 
		/// </summary>
		/// <param name="password">New 8-byte value of container's read/write password.
		/// </param>
		/// <param name="offset">Index to start copying the password from the array.
		/// </param>
		public virtual void  setContainerWriteOnlyPassword(byte[] password, int offset)
		{
			throw new OneWireException("The DS1977 does not have a write only password.");
		}
		
		/// <summary> Returns the read/write password used by this container to read from and
		/// write to the memory of the device.
		/// 
		/// </summary>
		/// <param name="password">Holds the 8-byte value of container's read/write password.
		/// </param>
		/// <param name="offset">Index to start copying the password into the array.
		/// </param>
		public virtual void  getContainerWriteOnlyPassword(byte[] password, int offset)
		{
			throw new OneWireException("The DS1977 does not have a write only password.");
		}
		
		public virtual bool verifyPassword(byte[] password, int offset, int type)
		{
			byte[] raw_buf = new byte[15];
			int addr = ((type == READ_ONLY_PWD)?READ_ACCESS_PASSWORD:READ_WRITE_ACCESS_PASSWORD);
			
			// command, address, offset, password (except last byte)
			raw_buf[0] = VERIFY_PSW_COMMAND;
			raw_buf[1] = (byte) (addr & 0xFF);
			raw_buf[2] = (byte) ((SupportClass.URShift((addr & 0xFFFF), 8)) & 0xFF);
			
			Array.Copy(password, offset, raw_buf, 3, 8);
			
			// send block (check copy indication complete)
			register.ib.adapter.dataBlock(raw_buf, 0, 10);
			
			if (register.ib.adapter.startPowerDelivery(DSPortAdapter.CONDITION_AFTER_BYTE))
			{
				
				// send last byte of password and enable strong pullup
				register.ib.adapter.putByte(raw_buf[11]);
				
				// delay for read to complete
				msWait(5);
				
				// turn off strong pullup
				register.ib.adapter.setPowerNormal();
				
				// read the confirmation byte
				if (register.ib.adapter.Byte != 0xAA)
				{
					return false;
				}
				
				return true;
			}
			
			return false;
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
			scratch.pageLength = 64;
			scratch.size = 64;
			scratch.numberPages = 1;
			scratch.maxPacketDataLength = 61;
			scratch.enablePower = true;
			
			// User Data Memory
			userDataMemory = new MemoryBankNVCRCPW(this, scratch);
			userDataMemory.numberPages = 511;
			userDataMemory.size = 32704;
			userDataMemory.pageLength = 64;
			userDataMemory.maxPacketDataLength = 61;
			userDataMemory.bankDescription = "Data Memory";
			userDataMemory.startPhysicalAddress = 0x0000;
			userDataMemory.generalPurposeMemory = true;
			userDataMemory.readOnly = false;
			userDataMemory.readWrite = true;
			userDataMemory.enablePower = true;
			
			// Register
			register = new MemoryBankNVCRCPW(this, scratch);
			register.numberPages = 1;
			register.size = 64;
			register.pageLength = 64;
			register.maxPacketDataLength = 61;
			register.bankDescription = "Register control";
			register.startPhysicalAddress = 0x7FC0;
			register.generalPurposeMemory = false;
			register.enablePower = true;
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
	}
}