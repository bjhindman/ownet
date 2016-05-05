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
using OneWireContainer = com.dalsemi.onewire.container.OneWireContainer;
using com.dalsemi.onewire.utils;
using OneWireException = com.dalsemi.onewire.OneWireException;
namespace com.dalsemi.onewire.adapter
{
	
	
	/// <summary> <p>This <code>DSPortAdapter</code> class was designed to be used for
	/// the iB-IDE's emulator.  The <code>DumbAdapter</code> allows
	/// programmers to add and remove <code>OneWireContainer</code>
	/// objects that will be found in its search.  The Java iButton
	/// emulator works by creating a class that subclasses all of
	/// <code>OneWireContainer16</code>'s relevant methods and redirecting them
	/// to the emulation code.  That object is then added to this class's
	/// list of <code>OneWireContainer</code>s.</p>
	/// 
	/// <p>Note that methods such as <code>selectPort</code> and
	/// <code>beginExclusive</code> by default do nothing.  This class is
	/// mainly meant for debugging using an emulated iButton.  It will do
	/// a poor job of debugging any multi-threading, port-sharing issues.
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.adapter.DSPortAdapter">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer">
	/// 
	/// </seealso>
	/// <version>     0.00, 16 Mar 2001
	/// </version>
	/// <author>      K
	/// </author>
	public class DumbAdapter:DSPortAdapter
	{
		/// <summary> Retrieves the name of the port adapter as a string.  The 'Adapter'
		/// is a device that connects to a 'port' that allows one to
		/// communicate with an iButton or other 1-Wire device.  As example
		/// of this is 'DS9097U'.
		/// 
		/// </summary>
		/// <returns>  <code>String</code> representation of the port adapter.
		/// </returns>
		override public System.String AdapterName
		{
			get
			{
				return "DumbAdapter";
			}
			
		}
		/// <summary> Retrieves a description of the port required by this port adapter.
		/// An example of a 'Port' would 'serial communication port'.
		/// 
		/// </summary>
		/// <returns>  <code>String</code> description of the port type required.
		/// </returns>
		override public System.String PortTypeDescription
		{
			get
			{
				return "Virtual Emulated Port";
			}
			
		}
		/// <summary> Retrieves a version string for this class.
		/// 
		/// </summary>
		/// <returns>  version string
		/// </returns>
		override public System.String ClassVersion
		{
			get
			{
				return "0.00";
			}
			
		}
		/// <summary> Retrieves a list of the platform appropriate port names for this
		/// adapter.  A port must be selected with the method 'selectPort'
		/// before any other communication methods can be used.  Using
		/// a communcation method before 'selectPort' will result in
		/// a <code>OneWireException</code> exception.
		/// 
		/// </summary>
		/// <returns>  <code>Enumeration</code> of type <code>String</code> that contains the port
		/// names
		/// </returns>
		override public System.Collections.IEnumerator PortNames
		{
			get
			{
				System.Collections.ArrayList portNames = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				portNames.Add("NULL0");
				return portNames.GetEnumerator();
			}
			
		}
		/// <summary> Retrieves the name of the selected port as a <code>String</code>.
		/// 
		/// </summary>
		/// <returns>  always returns the <code>String</code> "NULL0"
		/// </returns>
		override public System.String PortName
		{
			get
			{
				return "NULL0";
			}
			
		}
		/// <summary> Returns an enumeration of <code>OneWireContainer</code> objects corresponding
		/// to all of the iButtons or 1-Wire devices found on the 1-Wire Network.  In the case of
		/// the <code>DumbAdapter</code>, this method returns a simple copy of the internal
		/// <code>java.util.Vector</code> that stores all the 1-Wire devices this class finds
		/// in a search.
		/// 
		/// </summary>
		/// <returns>  <code>Enumeration</code> of <code>OneWireContainer</code> objects
		/// found on the 1-Wire Network.
		/// </returns>
		override public System.Collections.IEnumerator AllDeviceContainers
		{
			get
			{
				System.Collections.ArrayList copy_vector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				lock (containers.SyncRoot)
				{
					for (int i = 0; i < containers.Count; i++)
					{
						copy_vector.Add(containers[i]);
					}
				}
				return copy_vector.GetEnumerator();
			}
			
		}
		/// <summary> Returns a <code>OneWireContainer</code> object corresponding to the first iButton
		/// or 1-Wire device found on the 1-Wire Network. If no devices are found,
		/// then a <code>null</code> reference will be returned. In most cases, all further
		/// communication with the device is done through the <code>OneWireContainer</code>.
		/// 
		/// </summary>
		/// <returns>  The first <code>OneWireContainer</code> object found on the
		/// 1-Wire Network, or <code>null</code> if no devices found.
		/// </returns>
		override public OneWireContainer FirstDeviceContainer
		{
			get
			{
				lock (containers.SyncRoot)
				{
					if (containers.Count > 0)
					{
						containers_index = 1;
						return (OneWireContainer) containers[0];
					}
					else
						return null;
				}
			}
			
		}
		/// <summary> Returns a <code>OneWireContainer</code> object corresponding to the next iButton
		/// or 1-Wire device found. The previous 1-Wire device found is used
		/// as a starting point in the search.  If no devices are found,
		/// then a <code>null</code> reference will be returned. In most cases, all further
		/// communication with the device is done through the <code>OneWireContainer</code>.
		/// 
		/// </summary>
		/// <returns>  The next <code>OneWireContainer</code> object found on the
		/// 1-Wire Network, or <code>null</code> if no iButtons found.
		/// </returns>
		override public OneWireContainer NextDeviceContainer
		{
			get
			{
				lock (containers.SyncRoot)
				{
					if (containers.Count > containers_index)
					{
						containers_index++;
						return (OneWireContainer) containers[containers_index - 1];
					}
					else
						return null;
				}
			}
			
		}
		/// <summary> Gets the 'current' 1-Wire device address being used by the adapter as a long.
		/// This address is the last iButton or 1-Wire device found
		/// in a search (findNextDevice()...).
		/// 
		/// </summary>
		/// <returns> <code>long</code> representation of the iButton address
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		override public long AddressAsLong
		{
			get
			{
				byte[] address = new byte[8];
				
				getAddress(address);
				
				return Address.toLong(address);
			}
			
		}
		/// <summary> Gets the 'current' 1-Wire device address being used by the adapter as a String.
		/// This address is the last iButton or 1-Wire device found
		/// in a search (findNextDevice()...).
		/// 
		/// </summary>
		/// <returns> <code>String</code> representation of the iButton address
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		override public System.String AddressAsString
		{
			get
			{
				byte[] address = new byte[8];
				
				getAddress(address);
				
				return Address.toString(address);
			}
			
		}
		/// <summary> Gets a bit from the 1-Wire Network.
		/// This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <returns>  <code>true</code>
		/// </returns>
		override public bool Bit
		{
			get
			{
				//this will not be implemented
				return true;
			}
			
		}
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <returns> the value 0x0ff
		/// </returns>
		override public int Byte
		{
			get
			{
				//this will not be implemented
				return 0x0ff;
			}
			
		}
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="timeFactor"><ul>
		/// <li>   0 (DELIVERY_HALF_SECOND) provide power for 1/2 second.
		/// <li>   1 (DELIVERY_ONE_SECOND) provide power for 1 second.
		/// <li>   2 (DELIVERY_TWO_SECONDS) provide power for 2 seconds.
		/// <li>   3 (DELIVERY_FOUR_SECONDS) provide power for 4 seconds.
		/// <li>   4 (DELIVERY_SMART_DONE) provide power until the
		/// the device is no longer drawing significant power.
		/// <li>   5 (DELIVERY_INFINITE) provide power until the
		/// setPowerNormal() method is called.
		/// </ul>
		/// </param>
		override public int PowerDuration
		{
			set
			{
			}
			
		}
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="timeFactor"><ul>
		/// <li>   7 (DELIVERY_EPROM) provide program pulse for 480 microseconds
		/// <li>   5 (DELIVERY_INFINITE) provide power until the
		/// setPowerNormal() method is called.
		/// </ul>
		/// </param>
		override public int ProgramPulseDuration
		{
			set
			{
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <returns> <the last value passed to the <code>setSpeed(int)</code>
		/// method, or 0
		/// </returns>
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="speed"><ul>
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
		override public int Speed
		{
			get
			{
				return sp;
			}
			
			set
			{
				sp = value;
			}
			
		}
		//--------
		//-------- Variables
		//--------
		
		
		internal int containers_index = 0;
		
		private System.Collections.ArrayList containers = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		
		/// <summary> Adds a <code>OneWireContainer</code> to the list of containers that
		/// this adapter object will find.
		/// 
		/// </summary>
		/// <param name="c">represents a 1-Wire device that this adapter will report from a search
		/// </param>
		public virtual void  addContainer(OneWireContainer c)
		{
			lock (containers.SyncRoot)
			{
				containers.Add(c);
			}
		}
		
		/// <summary> Removes a <code>OneWireContainer</code> from the list of containers that
		/// this adapter object will find.
		/// 
		/// </summary>
		/// <param name="c">represents a 1-Wire device that this adapter should no longer
		/// report as found by a search
		/// </param>
		public virtual void  removeContainer(OneWireContainer c)
		{
			lock (containers.SyncRoot)
			{
				containers.Remove(c);
			}
		}
		
		
		/// <summary> Hashtable to contain the user replaced OneWireContainers</summary>
		private System.Collections.Hashtable registeredOneWireContainerClasses = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable(5));
		
		/// <summary> Byte array of families to include in search</summary>
		private byte[] include;
		
		/// <summary> Byte array of families to exclude from search</summary>
		private byte[] exclude;
		
		//--------
		//-------- Methods
		//--------
		
		//--------
		//-------- Port Selection
		//--------
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		public override void  registerOneWireContainerClass(int family, System.Type OneWireContainerClass)
		{
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="portName"> name of the target port, retrieved from
		/// getPortNames()
		/// 
		/// </param>
		/// <returns> always returns <code>true</code>
		/// </returns>
		public override bool selectPort(System.String portName)
		{
			//be lazy, allow anything
			return true;
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.</summary>
		public override void  freePort()
		{
			//airball
		}
		
		//--------
		//-------- Adapter detection
		//--------
		
		/// <summary> Detects adapter presence on the selected port.  In <code>DumbAdapter</code>,
		/// the adapter is always detected.
		/// 
		/// </summary>
		/// <returns>  <code>true</code>
		/// </returns>
		public override bool adapterDetected()
		{
			return true;
		}
		
		//--------
		//-------- Adapter features
		//--------
		
		/* The following interogative methods are provided so that client code
		* can react selectively to underlying states without generating an
		* exception.
		*/
		
		/// <summary> Applications might check this method and not attempt operation unless this method
		/// returns <code>true</code>. To make sure that a wide variety of applications can use this class,
		/// this method always returns <code>true</code>.
		/// 
		/// </summary>
		/// <returns>  <code>true</code>
		/// 
		/// </returns>
		public override bool canOverdrive()
		{
			//don't want someone to bail because of this
			return true;
		}
		
		/// <summary> Applications might check this method and not attempt operation unless this method
		/// returns <code>true</code>. To make sure that a wide variety of applications can use this class,
		/// this method always returns <code>true</code>.
		/// 
		/// </summary>
		/// <returns>  <code>true</code>
		/// </returns>
		public override bool canHyperdrive()
		{
			//don't want someone to bail because of this, although it doesn't exist yet
			return true;
		}
		
		/// <summary> Applications might check this method and not attempt operation unless this method
		/// returns <code>true</code>. To make sure that a wide variety of applications can use this class,
		/// this method always returns <code>true</code>.
		/// 
		/// </summary>
		/// <returns>  <code>true</code>
		/// </returns>
		public override bool canFlex()
		{
			//don't want someone to bail because of this
			return true;
		}
		
		/// <summary> Applications might check this method and not attempt operation unless this method
		/// returns <code>true</code>. To make sure that a wide variety of applications can use this class,
		/// this method always returns <code>true</code>.
		/// 
		/// </summary>
		/// <returns>  <code>true</code>
		/// </returns>
		public override bool canProgram()
		{
			//don't want someone to bail because of this
			return true;
		}
		
		/// <summary> Applications might check this method and not attempt operation unless this method
		/// returns <code>true</code>. To make sure that a wide variety of applications can use this class,
		/// this method always returns <code>true</code>.
		/// 
		/// </summary>
		/// <returns>  <code>true</code>
		/// </returns>
		public override bool canDeliverPower()
		{
			//don't want someone to bail because of this
			return true;
		}
		
		/// <summary> Applications might check this method and not attempt operation unless this method
		/// returns <code>true</code>. To make sure that a wide variety of applications can use this class,
		/// this method always returns <code>true</code>.
		/// 
		/// </summary>
		/// <returns>  <code>true</code>
		/// </returns>
		public override bool canDeliverSmartPower()
		{
			//don't want someone to bail because of this
			return true;
		}
		
		/// <summary> Applications might check this method and not attempt operation unless this method
		/// returns <code>true</code>. To make sure that a wide variety of applications can use this class,
		/// this method always returns <code>true</code>.
		/// 
		/// </summary>
		/// <returns>  <code>true</code>
		/// </returns>
		public override bool canBreak()
		{
			//don't want someone to bail because of this
			return true;
		}
		
		//--------
		//-------- Finding iButtons and 1-Wire devices
		//--------
		
		/// <summary> Returns <code>true</code> if the first iButton or 1-Wire device
		/// is found on the 1-Wire Network.
		/// If no devices are found, then <code>false</code> will be returned.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if an iButton or 1-Wire device is found.
		/// </returns>
		public override bool findFirstDevice()
		{
			lock (containers.SyncRoot)
			{
				if (containers.Count > 0)
				{
					containers_index = 1;
					return true;
				}
				else
					return false;
			}
		}
		
		/// <summary> Returns <code>true</code> if the next iButton or 1-Wire device
		/// is found. The previous 1-Wire device found is used
		/// as a starting point in the search.  If no more devices are found
		/// then <code>false</code> will be returned.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if an iButton or 1-Wire device is found.
		/// </returns>
		public override bool findNextDevice()
		{
			lock (containers.SyncRoot)
			{
				if (containers.Count > containers_index)
				{
					containers_index++;
					return true;
				}
				else
					return false;
			}
		}
		
		/// <summary> Copies the 'current' 1-Wire device address being used by the adapter into
		/// the array.  This address is the last iButton or 1-Wire device found
		/// in a search (findNextDevice()...).
		/// This method copies into a user generated array to allow the
		/// reuse of the buffer.  When searching many iButtons on the one
		/// wire network, this will reduce the memory burn rate.
		/// 
		/// </summary>
		/// <param name="address">An array to be filled with the current iButton address.
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override void  getAddress(byte[] address)
		{
			OneWireContainer temp = (OneWireContainer) containers[containers_index - 1];
			if (temp != null)
			{
				Array.Copy(temp.Address, 0, address, 0, 8);
			}
		}
		
		/// <summary> Verifies that the iButton or 1-Wire device specified is present on
		/// the 1-Wire Network. This does not affect the 'current' device
		/// state information used in searches (findNextDevice...).
		/// 
		/// </summary>
		/// <param name="address"> device address to verify is present
		/// 
		/// </param>
		/// <returns>  <code>true</code> if device is present, else
		/// <code>false</code>.
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override bool isPresent(byte[] address)
		{
			return isPresent(Address.toLong(address));
		}
		
		/// <summary> Verifies that the iButton or 1-Wire device specified is present on
		/// the 1-Wire Network. This does not affect the 'current' device
		/// state information used in searches (findNextDevice...).
		/// 
		/// </summary>
		/// <param name="address"> device address to verify is present
		/// 
		/// </param>
		/// <returns>  <code>true</code> if device is present, else
		/// <code>false</code>.
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override bool isPresent(long address)
		{
			lock (containers.SyncRoot)
			{
				for (int i = 0; i < containers.Count; i++)
				{
					OneWireContainer temp = (OneWireContainer) containers[i];
					long addr = temp.AddressAsLong;
					if (addr == address)
						return true;
				}
			}
			return false;
		}
		
		/// <summary> Verifies that the iButton or 1-Wire device specified is present on
		/// the 1-Wire Network. This does not affect the 'current' device
		/// state information used in searches (findNextDevice...).
		/// 
		/// </summary>
		/// <param name="address"> device address to verify is present
		/// 
		/// </param>
		/// <returns>  <code>true</code> if device is present, else
		/// <code>false</code>.
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override bool isPresent(System.String address)
		{
			return isPresent(Address.toByteArray(address));
		}
		
		/// <summary> Verifies that the iButton or 1-Wire device specified is present
		/// on the 1-Wire Network and in an alarm state. This method is currently
		/// not implemented in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="address"> device address to verify is present and alarming
		/// 
		/// </param>
		/// <returns>  <code>false</code>
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override bool isAlarming(byte[] address)
		{
			return false;
		}
		
		/// <summary> Verifies that the iButton or 1-Wire device specified is present
		/// on the 1-Wire Network and in an alarm state. This method is currently
		/// not implemented in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="address"> device address to verify is present and alarming
		/// 
		/// </param>
		/// <returns>  <code>false</code>
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override bool isAlarming(long address)
		{
			return isAlarming(Address.toByteArray(address));
		}
		
		/// <summary> Verifies that the iButton or 1-Wire device specified is present
		/// on the 1-Wire Network and in an alarm state. This method is currently
		/// not implemented in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="address"> device address to verify is present and alarming
		/// 
		/// </param>
		/// <returns>  <code>false</code>
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override bool isAlarming(System.String address)
		{
			return isAlarming(Address.toByteArray(address));
		}
		
		/// <summary> Selects the specified iButton or 1-Wire device by broadcasting its
		/// address.  With a <code>DumbAdapter</code>, this method simply
		/// returns true.
		/// 
		/// Warning, this does not verify that the device is currently present
		/// on the 1-Wire Network (See isPresent).
		/// 
		/// </summary>
		/// <param name="address">   address of iButton or 1-Wire device to select
		/// 
		/// </param>
		/// <returns>  <code>true</code> if device address was sent, <code>false</code>
		/// otherwise.
		/// 
		/// </returns>
		/// <seealso cref="isPresent(byte[])">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override bool select(byte[] address)
		{
			return isPresent(address);
		}
		
		/// <summary> Selects the specified iButton or 1-Wire device by broadcasting its
		/// address.  With a <code>DumbAdapter</code>, this method simply
		/// returns true.
		/// 
		/// Warning, this does not verify that the device is currently present
		/// on the 1-Wire Network (See isPresent).
		/// 
		/// </summary>
		/// <param name="address">   address of iButton or 1-Wire device to select
		/// 
		/// </param>
		/// <returns>  <code>true</code> if device address was sent, <code>false</code>
		/// otherwise.
		/// 
		/// </returns>
		/// <seealso cref="isPresent(byte[])">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override bool select(long address)
		{
			return select(Address.toByteArray(address));
		}
		
		/// <summary> Selects the specified iButton or 1-Wire device by broadcasting its
		/// address.  With a <code>DumbAdapter</code>, this method simply
		/// returns true.
		/// 
		/// Warning, this does not verify that the device is currently present
		/// on the 1-Wire Network (See isPresent).
		/// 
		/// </summary>
		/// <param name="address">   address of iButton or 1-Wire device to select
		/// 
		/// </param>
		/// <returns>  <code>true</code> if device address was sent, <code>false</code>
		/// otherwise.
		/// 
		/// </returns>
		/// <seealso cref="isPresent(byte[])">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override bool select(System.String address)
		{
			return select(Address.toByteArray(address));
		}
		
		//--------
		//-------- Finding iButton/1-Wire device options
		//--------
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <seealso cref="setNoResetSearch">
		/// </seealso>
		public override void  setSearchOnlyAlarmingDevices()
		{
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		public override void  setNoResetSearch()
		{
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <seealso cref="setNoResetSearch">
		/// </seealso>
		public override void  setSearchAllDevices()
		{
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <seealso cref="targetFamily">
		/// </seealso>
		/// <seealso cref="targetFamily(byte[])">
		/// </seealso>
		/// <seealso cref="excludeFamily">
		/// </seealso>
		/// <seealso cref="excludeFamily(byte[])">
		/// </seealso>
		public override void  targetAllFamilies()
		{
			include = null;
			exclude = null;
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="family">  the code of the family type to target for searches
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		/// <seealso cref="targetAllFamilies">
		/// </seealso>
		public override void  targetFamily(int family)
		{
			if ((include == null) || (include.Length != 1))
				include = new byte[1];
			
			include[0] = (byte) family;
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="family"> array of the family types to target for searches
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		/// <seealso cref="targetAllFamilies">
		/// </seealso>
		public override void  targetFamily(byte[] family)
		{
			if ((include == null) || (include.Length != family.Length))
				include = new byte[family.Length];
			
			Array.Copy(family, 0, include, 0, family.Length);
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="family">  the code of the family type NOT to target in searches
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		/// <seealso cref="targetAllFamilies">
		/// </seealso>
		public override void  excludeFamily(int family)
		{
			if ((exclude == null) || (exclude.Length != 1))
				exclude = new byte[1];
			
			exclude[0] = (byte) family;
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="family"> array of family cods NOT to target for searches
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		/// <seealso cref="targetAllFamilies">
		/// </seealso>
		public override void  excludeFamily(byte[] family)
		{
			if ((exclude == null) || (exclude.Length != family.Length))
				exclude = new byte[family.Length];
			
			Array.Copy(family, 0, exclude, 0, family.Length);
		}
		
		//--------
		//-------- 1-Wire Network Semaphore methods
		//--------
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="blocking"><code>true</code> if want to block waiting
		/// for an excluse access to the adapter
		/// </param>
		/// <returns> <code>true</code>
		/// </returns>
		public override bool beginExclusive(bool blocking)
		{
			//DEBUG!!! RIGHT NOW THIS IS NOT IMPLEMENTED!!!
			return true;
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		public override void  endExclusive()
		{
			//DEBUG!!! RIGHT NOW THIS IS NOT IMPLEMENTED!!!
		}
		
		//--------
		//-------- Primitive 1-Wire Network data methods
		//--------
		
		/// <summary> Sends a bit to the 1-Wire Network.
		/// This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="bitValue"> the bit value to send to the 1-Wire Network.
		/// </param>
		public override void  putBit(bool bitValue)
		{
			//this will not be implemented
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="byteValue"> the byte value to send to the 1-Wire Network.
		/// </param>
		public override void  putByte(int byteValue)
		{
			//this will not be implemented
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="len"> length of data bytes to receive
		/// 
		/// </param>
		/// <returns> a new byte array of length <code>len</code>
		/// </returns>
		public override byte[] getBlock(int len)
		{
			//this will not be implemented
			return new byte[len];
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="arr">    array in which to write the received bytes
		/// </param>
		/// <param name="len">    length of data bytes to receive
		/// </param>
		public override void  getBlock(byte[] arr, int len)
		{
			//this will not be implemented
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="arr">    array in which to write the received bytes
		/// </param>
		/// <param name="off">    offset into the array to start
		/// </param>
		/// <param name="len">    length of data bytes to receive
		/// </param>
		public override void  getBlock(byte[] arr, int off, int len)
		{
			//this will not be implemented
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="dataBlock"> array of data to transfer to and from the 1-Wire Network.
		/// </param>
		/// <param name="off">       offset into the array of data to start
		/// </param>
		/// <param name="len">       length of data to send / receive starting at 'off'
		/// </param>
		public override void  dataBlock(byte[] dataBlock, int off, int len)
		{
			//this will not be implemented
		}
		
		/// <summary> Sends a Reset to the 1-Wire Network.
		/// 
		/// </summary>
		/// <returns>  the result of the reset. Potential results are:
		/// <ul>
		/// <li> 0 (RESET_NOPRESENCE) no devices present on the 1-Wire Network.
		/// <li> 1 (RESET_PRESENCE) normal presence pulse detected on the 1-Wire
		/// Network indicating there is a device present.
		/// <li> 2 (RESET_ALARM) alarming presence pulse detected on the 1-Wire
		/// Network indicating there is a device present and it is in the
		/// alarm condition.  This is only provided by the DS1994/DS2404
		/// devices.
		/// <li> 3 (RESET_SHORT) inticates 1-Wire appears shorted.  This can be
		/// transient conditions in a 1-Wire Network.  Not all adapter types
		/// can detect this condition.
		/// </ul>
		/// Note that in <code>DumbAdapter</code>, the only possible results are 0 and 1.
		/// </returns>
		public override int reset()
		{
			//this will not be implemented
			if (containers.Count > 0)
				return 1;
			return 0;
		}
		
		//--------
		//-------- 1-Wire Network power methods
		//--------
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="changeCondition"><ul>
		/// <li>   0 (CONDITION_NOW) operation should occur immediately.
		/// <li>   1 (CONDITION_AFTER_BIT) operation should be pending
		/// execution immediately after the next bit is sent.
		/// <li>   2 (CONDITION_AFTER_BYTE) operation should be pending
		/// execution immediately after next byte is sent.
		/// </ul>
		/// 
		/// </param>
		/// <returns> <code>true</code>
		/// </returns>
		public override bool startPowerDelivery(int changeCondition)
		{
			return true;
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		/// <param name="changeCondition"><ul>
		/// <li>   0 (CONDITION_NOW) operation should occur immediately.
		/// <li>   1 (CONDITION_AFTER_BIT) operation should be pending
		/// execution immediately after the next bit is sent.
		/// <li>   2 (CONDITION_AFTER_BYTE) operation should be pending
		/// execution immediately after next byte is sent.
		/// </ul>
		/// 
		/// </param>
		/// <returns> <code>true</code>
		/// </returns>
		public override bool startProgramPulse(int changeCondition)
		{
			return true;
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		public override void  startBreak()
		{
		}
		
		/// <summary> This method does nothing in <code>DumbAdapter</code>.
		/// 
		/// </summary>
		public override void  setPowerNormal()
		{
			return ;
		}
		
		//--------
		//-------- 1-Wire Network speed methods
		//--------
		
		private int sp = 0;
		
		//--------
		//-------- Misc
		//--------
		
		/// <summary> Gets the container from this adapter whose address matches the address of a container
		/// in the <code>DumbAdapter</code>'s internal <code>java.util.Vector</code>.
		/// 
		/// </summary>
		/// <param name="address"> device address with which to find a container
		/// 
		/// </param>
		/// <returns>  The <code>OneWireContainer</code> object, or <code>null</code> if no match could be found.
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override OneWireContainer getDeviceContainer(byte[] address)
		{
			long addr = Address.toLong(address);
			lock (containers.SyncRoot)
			{
				for (int i = 0; i < containers.Count; i++)
				{
					if (((OneWireContainer) containers[i]).AddressAsLong == addr)
						return (OneWireContainer) containers[i];
				}
			}
			return null;
		}
		
		/// <summary> Gets the container from this adapter whose address matches the address of a container
		/// in the <code>DumbAdapter</code>'s internal <code>java.util.Vector</code>.
		/// 
		/// </summary>
		/// <param name="address"> device address with which to find a container
		/// 
		/// </param>
		/// <returns>  The <code>OneWireContainer</code> object, or <code>null</code> if no match could be found.
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override OneWireContainer getDeviceContainer(long address)
		{
			return getDeviceContainer(Address.toByteArray(address));
		}
		
		/// <summary> Gets the container from this adapter whose address matches the address of a container
		/// in the <code>DumbAdapter</code>'s internal <code>java.util.Vector</code>.
		/// 
		/// </summary>
		/// <param name="address"> device address with which to find a container
		/// 
		/// </param>
		/// <returns>  The <code>OneWireContainer</code> object, or <code>null</code> if no match could be found.
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.Address">
		/// </seealso>
		public override OneWireContainer getDeviceContainer(System.String address)
		{
			return getDeviceContainer(Address.toByteArray(address));
		}
		
		/// <summary> Returns a <code>OneWireContainer</code> object using the current 1-Wire network address.
		/// The internal state of the port adapter keeps track of the last
		/// address found and is able to create container objects from this
		/// state.
		/// 
		/// </summary>
		/// <returns>  the <code>OneWireContainer</code> object
		/// </returns>
		public override OneWireContainer getDeviceContainer()
		{
			
			// Mask off the upper bit.
			byte[] address = new byte[8];
			
			getAddress(address);
			
			return getDeviceContainer(address);
		}
		
		/// <summary> Checks to see if the family found is in the desired
		/// include group.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if in include group
		/// </returns>
		protected internal override bool isValidFamily(byte[] address)
		{
			byte familyCode = address[0];
			
			if (exclude != null)
			{
				for (int i = 0; i < exclude.Length; i++)
				{
					if (familyCode == exclude[i])
					{
						return false;
					}
				}
			}
			
			if (include != null)
			{
				for (int i = 0; i < include.Length; i++)
				{
					if (familyCode == include[i])
					{
						return true;
					}
				}
				
				return false;
			}
			
			return true;
		}
	}
}