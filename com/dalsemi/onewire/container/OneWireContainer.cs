/*---------------------------------------------------------------------------
* Copyright (C) 1999,2004 Maxim Integrated Products, All Rights Reserved.
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
using Address = com.dalsemi.onewire.utils.Address;
using com.dalsemi.onewire.adapter;
using OneWireException = com.dalsemi.onewire.OneWireException;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> A <code>OneWireContainer</code> encapsulates the <code>DSPortAdapter</code>,
	/// the 1-Wire&reg; network address, and methods to manipulate a specific 1-Wire device. A
	/// 1-Wire device may be in the form of a stainless steel armored can, called an iButton&reg;,
	/// or in standard IC plastic packaging.
	/// 
	/// <p>General 1-Wire device container class with basic communication functions.
	/// This class should only be used if a device specific class is not available
	/// or known.  Most <code>OneWireContainer</code> classes will extend this basic class.
	/// 
	/// <P> 1-Wire devices with memory can be accessed through the objects that
	/// are returned from the {@link #getMemoryBanks() getMemoryBanks} method. See the
	/// usage example below. </P>
	/// 
	/// <H3> Usage </H3>
	/// 
	/// <DL>
	/// <DD> <H4> Example 1</H4>
	/// Enumerate memory banks retrieved from the OneWireContainer
	/// instance 'owd' and cast to the highest interface.  See the
	/// interface descriptions
	/// {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}, and
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank OTPMemoryBank}
	/// for specific examples.
	/// <PRE> <CODE>
	/// MemoryBank      mb;
	/// PagedMemoryBank pg_mb;
	/// OTPMemoryBank   otp_mb;
	/// 
	/// for(Enumeration bank_enum = owd.getMemoryBanks();
	/// bank_enum.hasMoreElements(); )
	/// {
	/// // get the next memory bank, cast to MemoryBank
	/// mb = (MemoryBank)bank_enum.nextElement();
	/// 
	/// // check if has paged services
	/// if (mb instanceof PagedMemoryBank)
	/// pg_mb = (PagedMemoryBank)mb;
	/// 
	/// // check if has One-Time-Programable services
	/// if (mb instanceof OTPMemoryBank)
	/// otp_mb = (OTPMemoryBank)mb;
	/// }
	/// </CODE> </PRE>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.MemoryBank">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.PagedMemoryBank">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OTPMemoryBank">
	/// 
	/// </seealso>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public class OneWireContainer
	{
		/// <summary> Retrieves the port adapter object used to create this container.
		/// 
		/// </summary>
		/// <returns> port adapter instance
		/// </returns>
		virtual public DSPortAdapter Adapter
		{
			get
			{
				return adapter;
			}
			
		}
		/// <summary> Retrieves the Maxim Integrated Products part number of the 1-Wire device
		/// as a <code>String</code>.  For example 'Crypto iButton' or 'DS1992'.
		/// 
		/// </summary>
		/// <returns> 1-Wire device name
		/// </returns>
		virtual public System.String Name
		{
			get
			{
				lock (this)
				{
					return "Device type: " + (((address[0] & 0x0FF) < 16)?("0" + System.Convert.ToString(address[0] & 0x0FF, 16)):System.Convert.ToString(address[0] & 0x0FF, 16));
				}
			}
			
		}
		/// <summary> Retrieves the alternate Maxim Integrated Products part numbers or names.
		/// A 'family' of 1-Wire Network devices may have more than one part number
		/// depending on packaging.  There can also be nicknames such as
		/// 'Crypto iButton'.
		/// 
		/// </summary>
		/// <returns> 1-Wire device alternate names
		/// </returns>
		virtual public System.String AlternateNames
		{
			get
			{
				return "";
			}
			
		}
		/// <summary> Retrieves a short description of the function of the 1-Wire device type.
		/// 
		/// </summary>
		/// <returns> device functional description
		/// </returns>
		virtual public System.String Description
		{
			get
			{
				return "No description available.";
			}
			
		}
		/// <summary> Returns the maximum speed this iButton or 1-Wire device can
		/// communicate at.
		/// Override this method if derived iButton type can go faster then
		/// SPEED_REGULAR(0).
		/// 
		/// </summary>
		/// <returns> maximum speed
		/// </returns>
		/// <seealso cref="DSPortAdapter.setSpeed">
		/// </seealso>
		virtual public int MaxSpeed
		{
			get
			{
				return DSPortAdapter.SPEED_REGULAR;
			}
			
		}
		/// <summary> Gets the 1-Wire Network address of this device as an array of bytes.
		/// 
		/// </summary>
		/// <returns> 1-Wire address
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		virtual public byte[] Address
		{
			get
			{
				return address;
			}
			
		}
		/// <summary> Gets this device's 1-Wire Network address as a String.
		/// 
		/// </summary>
		/// <returns> 1-Wire address
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		virtual public System.String AddressAsString
		{
			get
			{
				//return Address.toString(address);
                return com.dalsemi.onewire.utils.Address.toString(address);
			}
			
		}
		/// <summary> Gets this device's 1-Wire Network address as a long.
		/// 
		/// </summary>
		/// <returns> 1-Wire address
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		virtual public long AddressAsLong
		{
			get
			{
				//return Address.toLong(address);
                return com.dalsemi.onewire.utils.Address.toLong(address); // !!!
			}
			
		}
		/// <summary> Returns an <code>Enumeration</code> of <code>MemoryBank</code>.  Default is no memory banks.
		/// 
		/// </summary>
		/// <returns> enumeration of memory banks to read and write memory
		/// on this iButton or 1-Wire device
		/// </returns>
		/// <seealso cref="MemoryBank">
		/// </seealso>
		virtual public System.Collections.IEnumerator MemoryBanks
		{
			get
			{
				return System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(0)).GetEnumerator();
			}
			
		}
		/// <summary> Verifies that the iButton or 1-Wire device is present on
		/// the 1-Wire Network.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if device present on the 1-Wire Network
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         a read back verification fails.
		/// </summary>
		/// <throws>  OneWireException if adapter is not open </throws>
		virtual public bool Present
		{
			get
			{
				lock (this)
				{
					return adapter.isPresent(address);
				}
			}
			
		}
		/// <summary> Verifies that the iButton or 1-Wire device is present
		/// on the 1-Wire Network and in an alarm state.  This does not
		/// apply to all device types.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if device present and in alarm condition
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         a read back verification fails.
		/// </summary>
		/// <throws>  OneWireException if adapter is not open </throws>
		virtual public bool Alarming
		{
			get
			{
				lock (this)
				{
					return adapter.isAlarming(address);
				}
			}
			
		}
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Reference to the adapter that is needed to communicate with this
		/// iButton or 1-Wire device.
		/// </summary>
		protected internal DSPortAdapter adapter;
		
		/// <summary> 1-Wire Network Address of this iButton or 1-Wire
		/// device.
		/// Family code is byte at offset 0.
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		protected internal byte[] address;
		
		/// <summary> Temporary copy of 1-Wire Network Address of this
		/// iButton or 1-Wire device.
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		private byte[] addressCopy;
		
		/// <summary> Communication speed requested.
		/// <ul>
		/// <li>     0 (SPEED_REGULAR)
		/// <li>     1 (SPEED_FLEX)
		/// <li>     2 (SPEED_OVERDRIVE)
		/// <li>     3 (SPEED_HYPERDRIVE)
		/// <li>    >3 future speeds
		/// </ul>
		/// 
		/// </summary>
		/// <seealso cref="DSPortAdapter.setSpeed">
		/// </seealso>
		protected internal int speed;
		
		/// <summary> Flag to indicate that falling back to a slower speed then requested
		/// is OK.
		/// </summary>
		protected internal bool speedFallBackOK;
		
		//--------
		//-------- Constructors
		//--------
		
		/// <summary> Create an empty container.  Must call <code>setupContainer</code> before
		/// using this new container.<p>
		/// 
		/// This is one of the methods to construct a container.  The others are
		/// through creating a OneWireContainer with parameters.
		/// 
		/// </summary>
		/// <seealso cref="OneWireContainer(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="OneWireContainer(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="OneWireContainer(DSPortAdapter,String)">
		/// </seealso>
		/// <seealso cref="setupContainer(DSPortAdapter,byte[])">
		/// </seealso>
		/// <seealso cref="setupContainer(DSPortAdapter,long)">
		/// </seealso>
		/// <seealso cref="setupContainer(DSPortAdapter,String)">
		/// </seealso>
		public OneWireContainer()
		{
		}
		
		/// <summary> Create a container with a provided adapter object
		/// and the address of the iButton or 1-Wire device.<p>
		/// 
		/// This is one of the methods to construct a container.  The other is
		/// through creating a OneWireContainer with NO parameters.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton.
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		/// <seealso cref="OneWireContainer()">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public OneWireContainer(DSPortAdapter sourceAdapter, byte[] newAddress)
		{
			this.setupContainer(sourceAdapter, newAddress);
		}
		
		/// <summary> Create a container with a provided adapter object
		/// and the address of the iButton or 1-Wire device.<p>
		/// 
		/// This is one of the methods to construct a container.  The other is
		/// through creating a OneWireContainer with NO parameters.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton.
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		/// <seealso cref="OneWireContainer()">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public OneWireContainer(DSPortAdapter sourceAdapter, long newAddress)
		{
			this.setupContainer(sourceAdapter, newAddress);
		}
		
		/// <summary> Create a container with a provided adapter object
		/// and the address of the iButton or 1-Wire device.<p>
		/// 
		/// This is one of the methods to construct a container.  The other is
		/// through creating a OneWireContainer with NO parameters.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton.
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		/// <seealso cref="OneWireContainer()">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public OneWireContainer(DSPortAdapter sourceAdapter, System.String newAddress)
		{
			this.setupContainer(sourceAdapter, newAddress);
		}
		
		//--------
		//-------- Setup and adapter methods
		//--------
		
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
		public virtual void  setupContainer(DSPortAdapter sourceAdapter, byte[] newAddress)
		{
			
			// get a reference to the source adapter (will need this to communicate)
			adapter = sourceAdapter;
			
			// set the Address
			lock (this)
			{
				address = new byte[8];
				addressCopy = new byte[8];
				
				Array.Copy(newAddress, 0, address, 0, 8);
			}
			
			// set desired speed to be SPEED_REGULAR by default with no fallback
			speed = DSPortAdapter.SPEED_REGULAR;
			speedFallBackOK = false;
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
		public virtual void  setupContainer(DSPortAdapter sourceAdapter, long newAddress)
		{
			
			// get a reference to the source adapter (will need this to communicate)
			adapter = sourceAdapter;
			
			// set the Address
			lock (this)
			{
				address = com.dalsemi.onewire.utils.Address.toByteArray(newAddress); // !!!
				addressCopy = new byte[8];
			}
			
			// set desired speed to be SPEED_REGULAR by default with no fallback
			speed = DSPortAdapter.SPEED_REGULAR;
			speedFallBackOK = false;
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
		public virtual void  setupContainer(DSPortAdapter sourceAdapter, System.String newAddress)
		{
			
			// get a reference to the source adapter (will need this to communicate)
			adapter = sourceAdapter;
			
			// set the Address
			lock (this)
			{
                address = com.dalsemi.onewire.utils.Address.toByteArray(newAddress);
				addressCopy = new byte[8];
			}
			
			// set desired speed to be SPEED_REGULAR by default with no fallback
			speed = DSPortAdapter.SPEED_REGULAR;
			speedFallBackOK = false;
		}
		
		//--------
		//-------- Device information methods
		//--------
		
		/// <summary> Sets the maximum speed for this container.  Note this may be slower then the
		/// devices maximum speed.  This method can be used by an application
		/// to restrict the communication rate due 1-Wire line conditions. <p>
		/// 
		/// </summary>
		/// <param name="newSpeed"><ul>
		/// <li>     0 (SPEED_REGULAR) set to normal communciation speed
		/// <li>     1 (SPEED_FLEX) set to flexible communciation speed used
		/// for long lines
		/// <li>     2 (SPEED_OVERDRIVE) set to normal communciation speed to
		/// overdrive
		/// <li>     3 (SPEED_HYPERDRIVE) set to normal communciation speed to
		/// hyperdrive
		/// <li>    >3 future speeds
		/// </ul>
		/// 
		/// </param>
		/// <param name="fallBack">boolean indicating it is OK to fall back to a slower
		/// speed if true
		/// 
		/// </param>
		public virtual void  setSpeed(int newSpeed, bool fallBack)
		{
			speed = newSpeed;
			speedFallBackOK = fallBack;
		}
		
		//--------
		//-------- I/O Methods
		//--------
		
		/// <summary> Go to the specified speed for this container.  This method uses the
		/// containers selected speed (method setSpeed(speed, fallback)) and
		/// will optionally fall back to a slower speed if communciation failed.
		/// Only call this method once to get the device into the desired speed
		/// as long as the device is still responding.
		/// 
		/// </summary>
		/// <throws>  OneWireIOException WHEN selected speed fails and fallback </throws>
		/// <summary>                                 is false
		/// </summary>
		/// <throws>  OneWireException WHEN hypterdrive is selected speed </throws>
		/// <seealso cref="setSpeed(int,boolean)">
		/// </seealso>
		public virtual void  doSpeed()
		{
			bool is_present = false;
			
			try
			{
				// check if already at speed and device present
				if ((speed == adapter.Speed) && adapter.isPresent(address))
					return ;
			}
			catch (OneWireIOException e)
			{
				// VOID
			}
			
			// speed Overdrive
			if (speed == DSPortAdapter.SPEED_OVERDRIVE)
			{
				try
				{
					// get this device and adapter to overdrive
					adapter.Speed = DSPortAdapter.SPEED_REGULAR;
					adapter.reset();
					adapter.putByte((byte) 0x69);
					adapter.Speed = DSPortAdapter.SPEED_OVERDRIVE;
				}
				catch (OneWireIOException e)
				{
					// VOID
				}
				
				// get copy of address
				lock (this)
				{
					Array.Copy(address, 0, addressCopy, 0, 8);
					adapter.dataBlock(addressCopy, 0, 8);
				}
				
				try
				{
					is_present = adapter.isPresent(address);
				}
				catch (OneWireIOException e)
				{
					// VOID
				}
				
				// check if new speed is OK
				if (!is_present)
				{
					
					// check if allow fallback
					if (speedFallBackOK)
						adapter.Speed = DSPortAdapter.SPEED_REGULAR;
					else
						throw new OneWireIOException("Failed to get device to selected speed (overdrive)");
				}
			}
			// speed regular or flex
			else if ((speed == DSPortAdapter.SPEED_REGULAR) || (speed == DSPortAdapter.SPEED_FLEX))
				adapter.Speed = speed;
			// speed hyperdrive, don't know how to do this
			else
				throw new OneWireException("Speed selected (hyperdrive) is not supported by this method");
		}
		
		//--------
		//-------- Object Methods
		//--------
		
		/// <summary> Returns a hash code value for the object. This method is
		/// supported for the benefit of hashtables such as those provided by
		/// <code>java.util.Hashtable</code>.
		/// 
		/// </summary>
		/// <returns>  a hash code value for this object.
		/// </returns>
		/// <seealso cref="java.util.Hashtable">
		/// </seealso>
		public override int GetHashCode()
		{
			if (this.address == null)
				return 0;
			else
                return ((long)com.dalsemi.onewire.utils.Address.toLong(this.address)).GetHashCode(); // !!!
		}
		
		/// <summary> Indicates whether some other object is "equal to" this one.</summary>
		/// <param name="obj">  the reference object with which to compare.
		/// </param>
		/// <returns>  <code>true</code> if this object is the same as the obj
		/// argument; <code>false</code> otherwise.
		/// </returns>
		public  override bool Equals(System.Object obj)
		{
			if (obj == this)
				return true;
			
			if (obj is OneWireContainer)
			{
				OneWireContainer owc = (OneWireContainer) obj;
				// don't claim that all subclasses of a specific container are
				// equivalent to the parent container
				if (owc.GetType() == this.GetType())
					return owc.AddressAsLong == this.AddressAsLong;
			}
			
			return false;
		}
		
		/// <summary> Returns a string representation of the object.
		/// 
		/// </summary>
		/// <returns>  a string representation of the object.
		/// </returns>
		public override System.String ToString()
		{
           return com.dalsemi.onewire.utils.Address.toString(this.address) + " " + this.Name; // !!!
		}
	}
}