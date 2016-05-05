/*---------------------------------------------------------------------------
* Copyright (C) 1999 - 2007 Maxim Integrated Products, All Rights Reserved.
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
using com.dalsemi.onewire;
using com.dalsemi.onewire.adapter;
using OneWireException = com.dalsemi.onewire.OneWireException;
using com.dalsemi.onewire.debug;
//UPGRADE_TODO: The package 'System_Renamed.Runtime.InteropServices' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace com.dalsemi.onewire.adapter
{
	
	/// <summary> The DSPortAdapterx86 class for all DotNet native adapters (Win32).
	/// 
	/// Instances of valid DSPortAdapter's are retrieved from methods in
	/// {@link com.dalsemi.onewire.OneWireAccessProvider OneWireAccessProvider}.
	/// 
	/// <P>The DotNetAdapterx86 methods can be organized into the following categories: </P>
	/// <UL>
	/// <LI> <B> Information </B>
	/// <UL>
	/// <LI> {@link #getAdapterName() getAdapterName}
	/// <LI> {@link #getPortTypeDescription() getPortTypeDescription}
	/// <LI> {@link #getClassVersion() getClassVersion}
	/// <LI> {@link #adapterDetected() adapterDetected}
	/// <LI> {@link #getAdapterVersion() getAdapterVersion}
	/// <LI> {@link #getAdapterAddress() getAdapterAddress}
	/// </UL>
	/// <LI> <B> Port Selection </B>
	/// <UL>
	/// <LI> {@link #getPortNames() getPortNames}
	/// <LI> {@link #selectPort(String) selectPort}
	/// <LI> {@link #getPortName() getPortName}
	/// <LI> {@link #freePort() freePort}
	/// </UL>
	/// <LI> <B> Adapter Capabilities </B>
	/// <UL>
	/// <LI> {@link #canOverdrive() canOverdrive}
	/// <LI> {@link #canHyperdrive() canHyperdrive}
	/// <LI> {@link #canFlex() canFlex}
	/// <LI> {@link #canProgram() canProgram}
	/// <LI> {@link #canDeliverPower() canDeliverPower}
	/// <LI> {@link #canDeliverSmartPower() canDeliverSmartPower}
	/// <LI> {@link #canBreak() canBreak}
	/// </UL>
	/// <LI> <B> 1-Wire Network Semaphore </B>
	/// <UL>
	/// <LI> {@link #beginExclusive(boolean) beginExclusive}
	/// <LI> {@link #endExclusive() endExclusive}
	/// </UL>
	/// <LI> <B> 1-Wire Device Discovery </B>
	/// <UL>
	/// <LI> Selective Search Options
	/// <UL>
	/// <LI> {@link #targetAllFamilies() targetAllFamilies}
	/// <LI> {@link #targetFamily(int) targetFamily(int)}
	/// <LI> {@link #targetFamily(byte[]) targetFamily(byte[])}
	/// <LI> {@link #excludeFamily(int) excludeFamily(int)}
	/// <LI> {@link #excludeFamily(byte[]) excludeFamily(byte[])}
	/// <LI> {@link #setSearchOnlyAlarmingDevices() setSearchOnlyAlarmingDevices}
	/// <LI> {@link #setNoResetSearch() setNoResetSearch}
	/// <LI> {@link #setSearchAllDevices() setSearchAllDevices}
	/// </UL>
	/// <LI> Search With Automatic 1-Wire Container creation
	/// <UL>
	/// <LI> {@link #getAllDeviceContainers() getAllDeviceContainers}
	/// <LI> {@link #getFirstDeviceContainer() getFirstDeviceContainer}
	/// <LI> {@link #getNextDeviceContainer() getNextDeviceContainer}
	/// </UL>
	/// <LI> Search With NO 1-Wire Container creation
	/// <UL>
	/// <LI> {@link #findFirstDevice() findFirstDevice}
	/// <LI> {@link #findNextDevice() findNextDevice}
	/// <LI> {@link #getAddress(byte[]) getAddress(byte[])}
	/// <LI> {@link #getAddressAsLong() getAddressAsLong}
	/// <LI> {@link #getAddressAsString() getAddressAsString}
	/// </UL>
	/// <LI> Manual 1-Wire Container creation
	/// <UL>
	/// <LI> {@link #getDeviceContainer(byte[]) getDeviceContainer(byte[])}
	/// <LI> {@link #getDeviceContainer(long) getDeviceContainer(long)}
	/// <LI> {@link #getDeviceContainer(String) getDeviceContainer(String)}
	/// <LI> {@link #getDeviceContainer() getDeviceContainer()}
	/// </UL>
	/// </UL>
	/// <LI> <B> 1-Wire Network low level access (usually not called directly) </B>
	/// <UL>
	/// <LI> Device Selection and Presence Detect
	/// <UL>
	/// <LI> {@link #isPresent(byte[]) isPresent(byte[])}
	/// <LI> {@link #isPresent(long) isPresent(long)}
	/// <LI> {@link #isPresent(String) isPresent(String)}
	/// <LI> {@link #isAlarming(byte[]) isAlarming(byte[])}
	/// <LI> {@link #isAlarming(long) isAlarming(long)}
	/// <LI> {@link #isAlarming(String) isAlarming(String)}
	/// <LI> {@link #select(byte[]) select(byte[])}
	/// <LI> {@link #select(long) select(long)}
	/// <LI> {@link #select(String) select(String)}
	/// </UL>
	/// <LI> Raw 1-Wire IO
	/// <UL>
	/// <LI> {@link #reset() reset}
	/// <LI> {@link #putBit(boolean) putBit}
	/// <LI> {@link #getBit() getBit}
	/// <LI> {@link #putByte(int) putByte}
	/// <LI> {@link #getByte() getByte}
	/// <LI> {@link #getBlock(int) getBlock(int)}
	/// <LI> {@link #getBlock(byte[], int) getBlock(byte[], int)}
	/// <LI> {@link #getBlock(byte[], int, int) getBlock(byte[], int, int)}
	/// <LI> {@link #dataBlock(byte[], int, int) dataBlock(byte[], int, int)}
	/// </UL>
	/// <LI> 1-Wire Speed and Power Selection
	/// <UL>
	/// <LI> {@link #setPowerDuration(int) setPowerDuration}
	/// <LI> {@link #startPowerDelivery(int) startPowerDelivery}
	/// <LI> {@link #setProgramPulseDuration(int) setProgramPulseDuration}
	/// <LI> {@link #startProgramPulse(int) startProgramPulse}
	/// <LI> {@link #startBreak() startBreak}
	/// <LI> {@link #setPowerNormal() setPowerNormal}
	/// <LI> {@link #setSpeed(int) setSpeed}
	/// <LI> {@link #getSpeed() getSpeed}
	/// </UL>
	/// </UL>
	/// <LI> <B> Advanced </B>
	/// <UL>
	/// <LI> {@link #registerOneWireContainerClass(int, Class) registerOneWireContainerClass}
	/// </UL>
	/// </UL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.OneWireAccessProvider">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer">
	/// 
	/// </seealso>
	/// <version>     0.01, 20 March 2001
	/// </version>
	/// <author>      DS
	/// 
	/// </author>
	public class DotNetAdapterx86:DSPortAdapter
	{
		/// <summary> Retrieve the name of the port adapter as a string.  The 'Adapter'
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
				// get the adapter name from the version string
				return "{" + getToken(typeVersionBuffer.ToString(), TOKEN_DEV) + "}";
			}
			
		}
		/// <summary> Retrieve a description of the port required by this port adapter.
		/// An example of a 'Port' would 'serial communication port'.
		/// 
		/// </summary>
		/// <returns>  <code>String</code> description of the port type required.
		/// </returns>
		override public System.String PortTypeDescription
		{
			get
			{
				// get the abreviation from the version string
				System.String abrv = getToken(typeVersionBuffer.ToString(), TOKEN_ABRV);
				
				// Change COMU to COM
				if (abrv.Equals("COMU"))
					abrv = "COM";
				
				return abrv + " (native)";
			}
			
		}
		/// <summary> Retrieve a version string for this class.
		/// 
		/// </summary>
		/// <returns>  version string
		/// </returns>
		override public System.String ClassVersion
		{
			get
			{
				// get the version from the version string
				System.String version = DotNetAdapterx86.driver_version + ", native: IBFS32.dll(" + getToken(this.mainVersionBuffer.ToString(), TOKEN_VER) + ") [type" + this.portType + ":" + getToken(this.typeVersionBuffer.ToString(), TOKEN_TAIL) + "](" + getToken(this.typeVersionBuffer.ToString(), TOKEN_VER) + ")";
				
				return version;
			}
			
		}
		/// <summary> Retrieve a list of the platform appropriate port names for this
		/// adapter.  A port must be selected with the method 'selectPort'
		/// before any other communication methods can be used.  Using
		/// a communcation method before 'selectPort' will result in
		/// a <code>OneWireException</code> exception.
		/// 
		/// </summary>
		/// <returns>  enumeration of type <code>String</code> that contains the port
		/// names
		/// </returns>
		override public System.Collections.IEnumerator PortNames
		{
			get
			{
				System.Collections.ArrayList portVector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				System.String header = getToken(typeVersionBuffer.ToString(), TOKEN_ABRV);
				
				if (header.Equals("COMU"))
					header = "COM";
				
				for (int i = 0; i < 16; i++)
					portVector.Add(new System.Text.StringBuilder(header + System.Convert.ToString(i)).ToString());
				
				return (portVector.GetEnumerator());
			}
			
		}
		/// <summary> Retrieve the name of the selected port as a <code>String</code>.
		/// 
		/// </summary>
		/// <returns>  <code>String</code> of selected port
		/// 
		/// </returns>
		/// <throws>  OneWireException if valid port not yet selected </throws>
		override public System.String PortName
		{
			get
			{
				// check if port is selected
				if ((portNum < 0) || (portType < 0))
				{
					throw new OneWireException("Port not selected");
				}
				
				// get the abreviation from the version string
				System.String header = getToken(typeVersionBuffer.ToString(), TOKEN_ABRV);
				
				// create the head and port number combo
				// Change COMU to COM
				if (header.Equals("COMU"))
					header = "COM";
				
				return header + portNum;
			}
			
		}
		/// <summary> Retrieve the version of the adapter.
		/// 
		/// </summary>
		/// <returns>  <code>String</code> of the adapter version.  It will return
		/// "<na>" if the adapter version is not or cannot be known.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as  </throws>
		/// <summary>         no device present.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to 
		/// static native shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		override public System.String AdapterVersion
		{
			get
			{
				if (portNum < 0 || portType < 0)
					return "<na>";
				else if (portType == 5)
				{
					// only know this with DS9097U type 
					// get the adapter name from the version string
					System.String ver = getToken(typeVersionBuffer.ToString(), TOKEN_TAIL);
					
					char rev = '0';
					int ch = ver.IndexOf("Rev");
					if (ch > 0)
						rev = ver[ch + 3];
					
					return "DS2480x revision " + rev + ", " + adapterSpecDescription;
				}
				else
					return adapterSpecDescription;
			}
			
		}
		/// <summary> Retrieve the address of the adapter if it has one.
		/// 
		/// </summary>
		/// <returns>  <code>String</code> of the adapter address.  It will return "<na>" if
		/// the adapter does not have an address.  The address is a string representation of an
		/// 1-Wire address.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as  </throws>
		/// <summary>         no device present.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to 
		/// static native shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		/// <seealso cref="Address">
		/// </seealso>
		override public System.String AdapterAddress
		{
			get
			{
				return "<na>"; //??? implement later
			}
			
		}
		/// <summary> Gets a bit from the 1-Wire Network.
		/// 
		/// </summary>
		/// <returns>  the bit value recieved from the the 1-Wire Network.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		override public bool Bit
		{
			get
			{
				// check if port is selected
				if ((portNum < 0) || (portType < 0))
					throw new OneWireException("Port not selected");
				
				// get a session 
				if (!get_session())
					throw new OneWireException("Port in use");
				
				// do 1-Wire bit
				int rt = TMEX_LIB_x86.TMTouchBit(sessionHandle, (short) 1);
				// release the session
				release_session();
				
				// check for adapter communication problems
				if (rt == - 12)
					throw new OneWireException("1-Wire Adapter communication exception");
				// check for microlan exception
				else if (rt < 0)
					throw new OneWireIOException("native TMEX_LIB_x86 error" + rt);
				
				return (rt > 0);
			}
			
		}
		/// <summary> Gets a byte from the 1-Wire Network.
		/// 
		/// </summary>
		/// <returns>  the byte value received from the the 1-Wire Network.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		override public int Byte
		{
			get
			{
				// check if port is selected
				if ((portNum < 0) || (portType < 0))
					throw new OneWireException("Port not selected");
				
				// get a session 
				if (!get_session())
					throw new OneWireException("Port in use");
				
				int rt = TMEX_LIB_x86.TMTouchByte(sessionHandle, (short) 0x0FF);
				// release the session
				release_session();
				
				// check for adapter communcication problems
				if (rt == - 12)
					throw new OneWireException("1-Wire Adapter communication exception");
				// check for microlan exception
				else if (rt < 0)
					throw new OneWireIOException("native TMEX_LIB_x86 error " + rt);
				
				return rt;
			}
			
		}
		/// <summary> Sets the duration to supply power to the 1-Wire Network.
		/// This method takes a time parameter that indicates the program
		/// pulse length when the method startPowerDelivery().<p>
		/// 
		/// Note: to avoid getting an exception,
		/// use the canDeliverPower() and canDeliverSmartPower()
		/// method to check it's availability. <p>
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
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		override public int PowerDuration
		{
			set
			{
				// Right now we only support infinite pull up.
				if (value != DELIVERY_INFINITE)
					throw new OneWireException("No support for other than infinite power duration");
			}
			
		}
		/// <summary> Sets the duration for providing a program pulse on the
		/// 1-Wire Network.
		/// This method takes a time parameter that indicates the program
		/// pulse length when the method startProgramPulse().<p>
		/// 
		/// Note: to avoid getting an exception,
		/// use the canDeliverPower() method to check it's
		/// availability. <p>
		/// 
		/// </summary>
		/// <param name="timeFactor"><ul>
		/// <li>   6 (DELIVERY_EPROM) provide program pulse for 480 microseconds
		/// <li>   5 (DELIVERY_INFINITE) provide power until the
		/// setPowerNormal() method is called.
		/// </ul>
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		override public int ProgramPulseDuration
		{
			set
			{
				if (value != DELIVERY_EPROM)
					throw new OneWireException("Only support EPROM length program pulse duration");
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> This method returns the current data transfer speed through a
		/// port to a 1-Wire Network. <p>
		/// 
		/// </summary>
		/// <returns>
		/// <ul>
		/// <li>     0 (SPEED_REGULAR) set to normal communication speed
		/// <li>     1 (SPEED_FLEX) set to flexible communication speed used
		/// for long lines
		/// <li>     2 (SPEED_OVERDRIVE) set to normal communication speed to
		/// overdrive
		/// <li>     3 (SPEED_HYPERDRIVE) set to normal communication speed to
		/// hyperdrive
		/// <li>    >3 future speeds
		/// </ul>
		/// </returns>
		/// <summary> This method takes an int representing the new speed of data
		/// transfer on the 1-Wire Network. <p>
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
		/// <param name="desiredSpeed">*
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		/// <summary>         or the adapter does not support this operation
		/// </summary>
		override public int Speed
		{
			get
			{
				// check if port is selected
				if ((portNum < 0) || (portType < 0))
					return SPEED_REGULAR;
				
				// get a session 
				if (!get_session())
					return SPEED_REGULAR;
				
				// change speed
				int rt = TMEX_LIB_x86.TMOneWireCom(sessionHandle, (short) TMEX_LIB_x86.LEVEL_READ, (short) 0);
				
				// release the session
				release_session();
				
				// translate to OWAPI speed
				switch (rt)
				{
					
					case TMEX_LIB_x86.TIME_RELAXED: 
						speed = SPEED_FLEX;
						break;
					
					case TMEX_LIB_x86.TIME_OVERDRV: 
						speed = SPEED_OVERDRIVE;
						break;
					
					// note that this function will return SPEED_REGULAR if
					// an adapter communication exception occurred when
					// TMOneWireCom is called
					case TMEX_LIB_x86.TIME_NORMAL: 
					default: 
						speed = SPEED_REGULAR;
						break;
					}
				
				return speed;
			}
			
			set
			{
				// check if port is selected
				if ((portNum < 0) || (portType < 0))
					throw new OneWireException("Port not selected");
				
				// get a session 
				if (!get_session())
					throw new OneWireException("Port in use");
				
				short tmspeed = 0;
				// translate to TMEX_LIB_x86 speed
				switch (value)
				{
					
					case SPEED_FLEX: 
						tmspeed = TMEX_LIB_x86.TIME_RELAXED;
						break;
					
					case SPEED_OVERDRIVE: 
						tmspeed = TMEX_LIB_x86.TIME_OVERDRV;
						break;
					
					case SPEED_REGULAR: 
					default: 
						tmspeed = TMEX_LIB_x86.TIME_NORMAL;
						break;
					} ;
				// change speed
				int rt = TMEX_LIB_x86.TMOneWireCom(sessionHandle, TMEX_LIB_x86.LEVEL_SET, tmspeed);
				// (1.01) if in overdrive then force an exclusive
				if (value == SPEED_OVERDRIVE)
					inExclusive = true;
				// release the session
				release_session();
				
				// check for adapter communication problems
				if (rt == - 3)
					throw new OneWireException("Adapter type does not support selected speed");
				else if (rt == - 12)
					throw new OneWireException("1-Wire Adapter communication exception");
				// check for microlan exception
				else if (rt < 0)
					throw new OneWireIOException("native TMEX_LIB_x86 error" + rt);
				// check for could not set
				else if (rt != tmspeed)
					throw new OneWireIOException("native TMEX_LIB_x86 error: could not set adapter to desired speed: " + rt);
				
				this.speed = value;
			}
			
		}
		/// <summary> Get the default Adapter Name.
		/// 
		/// </summary>
		/// <returns>  String containing the name of the default adapter
		/// </returns>
		public static System.String DefaultAdapterName
		{
			get
			{
                short portNum = 1;
                short portType = 6;
				System.Text.StringBuilder version = new System.Text.StringBuilder(255);
				System.String adapterName = "<na>";
				
				// get the default num/type
                //if (TMEX_LIB_x86.TMReadDefaultPort() == 1)
                   //int TMExtendedStartSession(short portNum, short portType, ref int sessionOptions);

				if (TMEX_LIB_x86.TMReadDefaultPort(ref portNum, ref portType) == 1)
				{
					// read the version again
					//if (TMEX_LIB_x86.TMGetTypeVersion(portTypeRef[0], version) == 1)
                    if (TMEX_LIB_x86.TMGetTypeVersion(portType, version) == 1)
					{
						adapterName = getToken(version.ToString(), TOKEN_DEV);
					}
				}
				
				return "{" + adapterName + "}";
			}
			
		}
		/// <summary> Get the default Adapter Port name.
		/// 
		/// </summary>
		/// <returns>  String containing the name of the default adapter port
		/// </returns>
		public static System.String DefaultPortName
		{
			get
			{
                short portNum = 1;
                short portType = 6;
				//short[] portNumRef = new short[1];
				//short[] portTypeRef = new short[1];
				System.Text.StringBuilder version = new System.Text.StringBuilder(255);
				System.String portName = "<na>";
				
				// get the default num/type
				if (TMEX_LIB_x86.TMReadDefaultPort(ref portNum, ref portType) == 1)
				{
					// read the version again
					if (TMEX_LIB_x86.TMGetTypeVersion(portType, version) == 1)
					{
						// get the abreviation from the version string
						System.String header = getToken(version.ToString(), TOKEN_ABRV);
						
						// create the head and port number combo
						// Change COMU to COM
						if (header.Equals("COMU"))
							portName = "COM" + portNum;
						else
							portName = header + portNum;
					}
				}
				
				return portName;
			}
			
		}
		/// <summary> Get the default Adapter Type number.
		/// 
		/// </summary>
		/// <returns>  int, the default adapter type
		/// </returns>
		private static int DefaultTypeNumber
		{
			get
			{
				short PortNum = 1;
				short PortType = 6;
				System.Text.StringBuilder zver = new System.Text.StringBuilder(255);
				
				// get the default num/type
				if (TMEX_LIB_x86.TMReadDefaultPort(ref PortNum, ref PortType) == 1)
				{
					// read the version again
					if (TMEX_LIB_x86.TMGetTypeVersion(PortType, zver) == 1)
					{
						return (int) PortType;
					}
				}
				
				return 5; // fail so at least do DS9097U default type
			}
			
		}
		//--------
		//-------- Variables
		//--------
		
		/// <summary>DotNet port type number (0-15) </summary>
		protected internal int portType;
		
		/// <summary>Current 1-Wire Network Address </summary>
		protected internal byte[] RomDta = new byte[8];
		
		/// <summary>Flag to indicate next search will look only for alarming devices </summary>
		private bool doAlarmSearch = false;
		
		/// <summary>Flag to indicate next search will be a 'first' </summary>
		private bool resetSearch = true;
		
		/// <summary>Flag to indicate next search will not be preceeded by a 1-Wire reset </summary>
		private bool skipResetOnSearch = false;
		
		private const System.String driver_version = "V0.02";
		
		/// <summary>current adapter communication speed </summary>
		private int speed = 0;
		
		private int portNum = - 1;
		private int sessionHandle = - 1;
		private bool inExclusive = false; 
		
		//private System_Renamed.Text.StringBuilder mainVersionBuffer = new System_Renamed.Text.StringBuilder(SIZE_VERSION);
        private System.Text.StringBuilder mainVersionBuffer = new System.Text.StringBuilder(SIZE_VERSION);

		
		//private System_Renamed.Text.StringBuilder typeVersionBuffer = new System_Renamed.Text.StringBuilder(SIZE_VERSION);
        private System.Text.StringBuilder typeVersionBuffer = new System.Text.StringBuilder(SIZE_VERSION);
		
		
		private byte[] stateBuffer = new byte[SIZE_STATE];
		
		private bool[] adapterSpecFeatures;
		private System.String adapterSpecDescription;
		
		/// <summary>token indexes into version string </summary>
		private const int TOKEN_ABRV = 0;
		private const int TOKEN_DEV = 1;
		private const int TOKEN_VER = 2;
		private const int TOKEN_DATE = 3;
		private const int TOKEN_TAIL = 255;
		/// <summary>constant for state buffer size </summary>
		private const int SIZE_STATE = 5120;
		/// <summary>constant for size of version string </summary>
		private const int SIZE_VERSION = 256;
		/// <summary>constants for uninitialized ports </summary>
		private const int EMPTY_NEW = - 2;
		private const int EMPTY_FREED = - 1;
		
		
		//--------
		//-------- Constructors/Destructor
		//--------
		
		/// <summary> Constructs a default adapter
		/// 
		/// </summary>
		/// <throws>  ClassNotFoundException </throws>
		public DotNetAdapterx86()
		{
			// set default port type
			portType = DefaultTypeNumber;
			
			// attempt to set the portType, will throw exception if does not exist
			//if (!setPortType_Native(getInfo(), portType))
			if (!setTMEXPortType(portType))
			{
				//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
				throw new System.Exception("DotNet adapter x86 type does not exist");
			}

		}
		
		/// <summary> Constructs with a specified port type
		/// 
		/// 
		/// </summary>
		/// <param name="newPortType">
		/// </param>
		/// <throws>  ClassNotFoundException </throws>
		public DotNetAdapterx86(int newPortType)
		{
			// set default port type
			portType = newPortType;
			
			// attempt to set the portType, will throw exception if does not exist
			//if (!setPortType_Native(getInfo(), portType))
			if (!setTMEXPortType(portType))
			{
				//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
				throw new System.Exception("DotNet adapter type does not exist");
			}
		}
		
		/// <summary> Finalize to Cleanup native</summary>
		~DotNetAdapterx86()
		{
			cleanup();
		}
		
		//--------
		//-------- Methods
		//--------
		
		//--------
		//-------- Port Selection
		//--------
		
		/// <summary> Specify a platform appropriate port name for this adapter.  Note that
		/// even though the port has been selected, it's ownership may be relinquished
		/// if it is not currently held in a 'exclusive' block.  This class will then
		/// try to re-aquire the port when needed.  If the port cannot be re-aquired
		/// ehen the exception <code>PortInUseException</code> will be thrown.
		/// 
		/// </summary>
		/// <param name="portName"> name of the target port, retrieved from
		/// getPortNames()
		/// 
		/// </param>
		/// <returns> <code>true</code> if the port was aquired, <code>false</code>
		/// if the port is not available.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException If port does not exist, or unable to communicate with port. </throws>
		/// <throws>  OneWireException If port does not exist </throws>
		public override bool selectPort(System.String portName)
		{
			int prtnum = 0, i;
			bool rt = false;
			
			// free the last port
			freePort();
			
			// get the abreviation from the version string
			System.String header = getToken(typeVersionBuffer.ToString(), TOKEN_ABRV);
			
			// Change COMU to COM
			if (header.Equals("COMU"))
				header = "COM";
			
			// loop to make sure that the begining of the port name matches the head
			for (i = 0; i < header.Length; i++)
			{
				if (portName[i] != header[i])
					return false;
			}
			
			// i now points to begining of integer (0 TO 15)
			if ((portName[i] >= '0') && (portName[i] <= '9'))
			{
				prtnum = portName[i] - '0';
				if (((i + 1) < portName.Length) && (portName[i + 1] >= '0') && (portName[i + 1] <= '9'))
				{
					prtnum *= 10;
					prtnum += portName[i + 1] - '0';
				}
				
				if (prtnum > 15)
					return false;
			}
			
			// now have prtnum
			// get a session handle, 16 sec timeout
            TimeSpan elapsedTime;
            long tickCounterBegin = DateTime.Now.Ticks;

			do 
			{
               int sessionHandle = TMEX_LIB_x86.TMExtendedStartSession((short)prtnum, (short)portType, null);
				// this port type does not exist
				if (sessionHandle == - 201)
					break;
				// valid handle
				else if (sessionHandle > 0)
				{
					// do setup
					if (TMEX_LIB_x86.TMSetup(sessionHandle) == 1)
					{
						// read the version again
						TMEX_LIB_x86.TMGetTypeVersion(portType, typeVersionBuffer);
						byte[] specBuffer = new byte[319];
						// get the adapter spec
						TMEX_LIB_x86.TMGetAdapterSpec(sessionHandle, specBuffer);
						adapterSpecDescription = TMEX_LIB_x86.getDescriptionFromSpecification(specBuffer);
						adapterSpecFeatures = TMEX_LIB_x86.getFeaturesFromSpecification(specBuffer);
						
						// record the portnum
						this.portNum = (short) prtnum;

						// return success
						rt = true;
					}
					
					break;
				}
                elapsedTime = new TimeSpan(DateTime.Now.Ticks - tickCounterBegin);
			}
            while (elapsedTime.TotalMilliseconds < 16000.0); // while elapsed time is less than 16 seconds
			
			// close the session
			TMEX_LIB_x86.TMEndSession(this.sessionHandle);
			this.sessionHandle = 0;
			
			// check if session was not available
			if (!rt)
			{
				// free up the port
				freePort();
				// throw exception
				throw new OneWireException("1-Wire Net not available");
			}
			
			return rt;
		}
		
		
		
		/// <summary> Free ownership of the selected port if it is currently owned back
		/// to the system.  This should only be called if the recently
		/// selected port does not have an adapter or at the end of
		/// your application's use of the port.
		/// 
		/// </summary>
		/// <throws>  OneWireException If port does not exist </throws>
		public override void  freePort()
		{
			// check for opened port
			if ((portNum >= 0) && (portType >= 0))
			{
				// get a session 
				if (get_session())
				{
					// clean up open port and sessions
					TMEX_LIB_x86.TMClose(sessionHandle);
					
					// release the session (forced, even if in exclusive)
					TMEX_LIB_x86.TMEndSession(sessionHandle);
					sessionHandle = 0;
					inExclusive = false;
				}
			}
			
			// set flag to indicate this port is now free
			portNum = EMPTY_FREED;
		}
		
		//--------
		//-------- Adapter detection 
		//--------
		
		/// <summary> Detect adapter presence on the selected port.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if the adapter is confirmed to be connected to
		/// the selected port, <code>false</code> if the adapter is not connected.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public override bool adapterDetected()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// return success if both port num and type are known
			return ((portNum >= 0) && (portType >= 0));
		}
		
		//--------
		//-------- Adapter features 
		//--------
		
		/* The following interogative methods are provided so that client code
		* can react selectively to underlying states without generating an
		* exception.
		*/
		
		/// <summary> Returns whether adapter can physically support overdrive mode.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if this port adapter can do OverDrive,
		/// <code>false</code> otherwise.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error with the adapter </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		public override bool canOverdrive()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			return (adapterSpecFeatures[TMEX_LIB_x86.FEATURE_OVERDRIVE]);
		}
		
		/// <summary> Returns whether the adapter can physically support hyperdrive mode.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if this port adapter can do HyperDrive,
		/// <code>false</code> otherwise.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error with the adapter </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		public override bool canHyperdrive()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			return false;
		}
		
		/// <summary> Returns whether the adapter can physically support flex speed mode.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if this port adapter can do flex speed,
		/// <code>false</code> otherwise.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error with the adapter </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		public override bool canFlex()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			return (adapterSpecFeatures[TMEX_LIB_x86.FEATURE_FLEX]);
		}
		
		
		/// <summary> Returns whether adapter can physically support 12 volt power mode.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if this port adapter can do Program voltage,
		/// <code>false</code> otherwise.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error with the adapter </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		public override bool canProgram()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			return (adapterSpecFeatures[TMEX_LIB_x86.FEATURE_PROGRAM]);
		}
		
		/// <summary> Returns whether the adapter can physically support strong 5 volt power
		/// mode.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if this port adapter can do strong 5 volt
		/// mode, <code>false</code> otherwise.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error with the adapter </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		public override bool canDeliverPower()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			return (adapterSpecFeatures[TMEX_LIB_x86.FEATURE_POWER]);
		}
		
		/// <summary> Returns whether the adapter can physically support "smart" strong 5
		/// volt power mode.  "smart" power delivery is the ability to deliver
		/// power until it is no longer needed.  The current drop it detected
		/// and power delivery is stopped.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if this port adapter can do "smart" strong
		/// 5 volt mode, <code>false</code> otherwise.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error with the adapter </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		public override bool canDeliverSmartPower()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			return false; // currently not implemented
		}
		
		/// <summary> Returns whether adapter can physically support 0 volt 'break' mode.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if this port adapter can do break,
		/// <code>false</code> otherwise.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error with the adapter </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		public override bool canBreak()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			return (adapterSpecFeatures[TMEX_LIB_x86.FEATURE_BREAK]);
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
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override bool findFirstDevice()
		{
			// reset the internal rom buffer
			resetSearch = true;

			return findNextDevice();
		}
		
		/// <summary> Returns <code>true</code> if the next iButton or 1-Wire device
		/// is found. The previous 1-Wire device found is used
		/// as a starting point in the search.  If no more devices are found
		/// then <code>false</code> will be returned.
		/// 
		/// </summary>
		/// <returns>  <code>true</code> if an iButton or 1-Wire device is found.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override bool findNextDevice()
		{
			while (true)
			{
				short[] ROM = new short[8];
				
				// check if port is selected
				if ((portNum < 0) || (portType < 0))
					throw new OneWireException("Port not selected");
				
				// get a session 
				if (!get_session())
					throw new OneWireException("Port in use");
				
				short rt = TMEX_LIB_x86.TMSearch(sessionHandle, stateBuffer, (short) (resetSearch?1:0), (short) (skipResetOnSearch?0:1), (short) (doAlarmSearch?0xEC:0xF0));
		
				// check for microlan exception
				if (rt < 0)
					throw new OneWireIOException("native TMEX_LIB_x86 error " + rt);
				
				// retrieve the ROM number found
				ROM[0] = 0;
				short romrt = TMEX_LIB_x86.TMRom(sessionHandle, stateBuffer, ROM);
				if (romrt == 1)
				{
					// copy to java array
					for (int i = 0; i < 8; i++)
						this.RomDta[i] = (byte) ROM[i];
				}
				else
					throw new OneWireIOException("native TMEX_LIB_x86 error " + romrt);
				
				// release the session
				release_session();
				
				if (rt > 0)
				{
					resetSearch = false;
					
					// check if this is an OK family type
					if (isValidFamily(RomDta))
						return true;
					
					// Else, loop to the top and do another search.
				}
				else
				{
					resetSearch = true;
					
					return false;
				}
			}
		}
		
		/// <summary> Copies the 'current' iButton address being used by the adapter into
		/// the array.  This address is the last iButton or 1-Wire device found
		/// in a search (findNextDevice()...).
		/// This method copies into a user generated array to allow the
		/// reuse of the buffer.  When searching many iButtons on the one
		/// wire network, this will reduce the memory burn rate.
		/// 
		/// </summary>
		/// <param name="address">An array to be filled with the current iButton address.
		/// </param>
		/// <seealso cref="Address">
		/// </seealso>
		public override void  getAddress(byte[] address)
		{
			Array.Copy(RomDta, 0, address, 0, 8);
		}
		
		/// <summary> Verifies that the iButton or 1-Wire device specified is present on
		/// the 1-Wire Network. This does not affect the 'current' device
		/// state information used in searches (findNextDevice...).
		/// 
		/// </summary>
		/// <param name="address"> device address to verify is present
		/// 
		/// </param>
		/// <returns>  <code>true</code> if device is present else
		/// <code>false</code>.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="Address">
		/// </seealso>
		public override bool isPresent(byte[] address)
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			short[] ROM = new short[8];
			for (int i = 0; i < 8; i++)
				ROM[i] = address[i];
			
			// get the current rom to restore after isPresent() (1.01)
			short[] oldROM = new short[8];
			oldROM[0] = 0;
			TMEX_LIB_x86.TMRom(sessionHandle, stateBuffer, oldROM);
			// set this rom to TMEX_LIB_x86
			TMEX_LIB_x86.TMRom(sessionHandle, stateBuffer, ROM);
			// see if part this present
			int rt = TMEX_LIB_x86.TMStrongAccess(sessionHandle, stateBuffer);
			// restore  
			TMEX_LIB_x86.TMRom(sessionHandle, stateBuffer, oldROM);
			// release the session
			release_session();
			
			// check for adapter communcication problems
			if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error" + rt);
			
			return (rt > 0);
		}
		
		/// <summary> Verifies that the iButton or 1-Wire device specified is present
		/// on the 1-Wire Network and in an alarm state. This does not
		/// affect the 'current' device state information used in searches
		/// (findNextDevice...).
		/// 
		/// </summary>
		/// <param name="address"> device address to verify is present and alarming
		/// 
		/// </param>
		/// <returns>  <code>true</code> if device is present and alarming else
		/// <code>false</code>.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="Address">
		/// </seealso>
		public override bool isAlarming(byte[] address)
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			short[] ROM = new short[8];
			for (int i = 0; i < 8; i++)
				ROM[i] = address[i];
			
			// get the current rom to restore after isPresent() (1.01)
			short[] oldROM = new short[8];
			oldROM[0] = 0;
			TMEX_LIB_x86.TMRom(sessionHandle, stateBuffer, oldROM);
			// set this rom to TMEX_LIB_x86
			TMEX_LIB_x86.TMRom(sessionHandle, stateBuffer, ROM);
			// see if part this present
			int rt = TMEX_LIB_x86.TMStrongAlarmAccess(sessionHandle, stateBuffer);
			// restore  
			TMEX_LIB_x86.TMRom(sessionHandle, stateBuffer, oldROM);
			// release the session
			release_session();
			
			// check for adapter communication problems
			if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error" + rt);
			
			return (rt > 0);
		}
		
		/// <summary> Selects the specified iButton or 1-Wire device by broadcasting its
		/// address.  This operation is refered to a 'MATCH ROM' operation
		/// in the iButton and 1-Wire device data sheets.  This does not
		/// affect the 'current' device state information used in searches
		/// (findNextDevice...).
		/// 
		/// Warning, this does not verify that the device is currently present
		/// on the 1-Wire Network (See isPresent).
		/// 
		/// </summary>
		/// <param name="address">    iButton to select
		/// 
		/// </param>
		/// <returns>  <code>true</code> if device address was sent,<code>false</code>
		/// otherwise.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.adapter.DSPortAdapter.isPresent(byte[] address)">
		/// </seealso>
		/// <seealso cref="Address">
		/// </seealso>
		public override bool select(byte[] address)
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			byte[] send_block = new byte[9];
			send_block[0] = (byte) 0x55; // match command
			for (int i = 0; i < 8; i++)
				send_block[i + 1] = System.Convert.ToByte(address[i]);
			
			// Change to use a block, not TMRom/TMAccess
			int rt = TMEX_LIB_x86.TMBlockIO(sessionHandle, send_block, (short) 9);
			// release the session
			release_session();
			
			// check for adapter communication problems
			if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for no device
			else if (rt == - 7)
				throw new OneWireException("No device detected");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error" + rt);
			
			return (rt >= 1);
		}
		
		//--------
		//-------- Finding iButton/1-Wire device options 
		//--------
		
		/// <summary> Set the 1-Wire Network search to find only iButtons and 1-Wire
		/// devices that are in an 'Alarm' state that signals a need for
		/// attention.  Not all iButton types
		/// have this feature.  Some that do: DS1994, DS1920, DS2407.
		/// This selective searching can be canceled with the
		/// 'setSearchAllDevices()' method.
		/// 
		/// </summary>
		/// <seealso cref="setNoResetSearch">
		/// </seealso>
		public override void  setSearchOnlyAlarmingDevices()
		{
			doAlarmSearch = true;
		}
		
		/// <summary> Set the 1-Wire Network search to not perform a 1-Wire
		/// reset before a search.  This feature is chiefly used with
		/// the DS2409 1-Wire coupler.
		/// The normal reset before each search can be restored with the
		/// 'setSearchAllDevices()' method.
		/// </summary>
		public override void  setNoResetSearch()
		{
			skipResetOnSearch = true;
		}
		
		/// <summary> Set the 1-Wire Network search to find all iButtons and 1-Wire
		/// devices whether they are in an 'Alarm' state or not and
		/// restores the default setting of providing a 1-Wire reset
		/// command before each search. (see setNoResetSearch() method).
		/// 
		/// </summary>
		/// <seealso cref="setNoResetSearch">
		/// </seealso>
		public override void  setSearchAllDevices()
		{
			doAlarmSearch = false;
			skipResetOnSearch = false;
		}
		
		//--------
		//-------- 1-Wire Network Semaphore methods  
		//--------
		
		/// <summary> Gets exclusive use of the 1-Wire to communicate with an iButton or
		/// 1-Wire Device.
		/// This method should be used for critical sections of code where a
		/// sequence of commands must not be interrupted by communication of
		/// threads with other iButtons, and it is permissible to sustain
		/// a delay in the special case that another thread has already been
		/// granted exclusive access and this access has not yet been
		/// relinquished. <p>
		/// 
		/// It can be called through the OneWireContainer
		/// class by the end application if they want to ensure exclusive
		/// use.  If it is not called around several methods then it
		/// will be called inside each method.
		/// 
		/// </summary>
		/// <param name="blocking"><code>true</code> if want to block waiting
		/// for an excluse access to the adapter
		/// </param>
		/// <returns> <code>true</code> if blocking was false and a
		/// exclusive session with the adapter was aquired
		/// 
		/// </returns>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override bool beginExclusive(bool blocking)
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// check if already in exclusive block
			if (inExclusive)
			{
				// make sure still valid (if not get a new one)
				if (TMEX_LIB_x86.TMValidSession(sessionHandle) > 0)
					return true;
			}
			
			// if in exclusive, no longer valid so clear it
			inExclusive = false;
			
			// check if blocking 
			if (blocking)
			{
				// loop forever until get a session
				while (!get_session())
					;
				inExclusive = true;
			}
			else
			{
				// try once for handle (actually blocks for up to 2 seconds)
				if (get_session())
					inExclusive = true;
			}
			
			// throw the port in use exception if could not get a TMEX_LIB_x86 session
			if (!inExclusive)
				throw new OneWireException("Port in use");
			
			return inExclusive;
		}
		
		/// <summary> Relinquishes exclusive control of the 1-Wire Network.
		/// This command dynamically marks the end of a critical section and
		/// should be used when exclusive control is no longer needed.
		/// </summary>
		public override void  endExclusive()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				return ;
			
			// clear any current exclusive
			inExclusive = false;
			
			// release the current handle
			release_session();
		}
		
		//--------
		//-------- Primitive 1-Wire Network data methods 
		//--------
		
		/// <summary> Sends a bit to the 1-Wire Network.
		/// 
		/// </summary>
		/// <param name="bitValue"> the bit value to send to the 1-Wire Network.
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override void  putBit(bool bitValue)
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			// do 1-Wire bit
			int rt = TMEX_LIB_x86.TMTouchBit(sessionHandle, (short) (bitValue?1:0));
			// release the session
			release_session();
			
			// check for adapter communication problems
			if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error" + rt);
			
			if (bitValue != (rt > 0))
				throw new OneWireIOException("Error during putBit()");
		}
		
		/// <summary> Sends a byte to the 1-Wire Network.
		/// 
		/// </summary>
		/// <param name="byteValue"> the byte value to send to the 1-Wire Network.
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override void  putByte(int byteValue)
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			int rt = TMEX_LIB_x86.TMTouchByte(sessionHandle, (short) (0x0FF & byteValue));
			// release the session
			release_session();
			
			// check for adapter communcication problems
			if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error " + rt);
			
			if (rt != ((0x00FF) & byteValue))
				throw new OneWireIOException("Error during putByte(), echo was incorrect ");
		}
		
		/// <summary> Get a block of data from the 1-Wire Network.
		/// 
		/// </summary>
		/// <param name="len"> length of data bytes to receive
		/// 
		/// </param>
		/// <returns>  the data received from the 1-Wire Network.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override byte[] getBlock(int len)
		{
			byte[] barr = new byte[len];
			
			getBlock(barr, 0, len);
			
			return barr;
		}
		
		/// <summary> Get a block of data from the 1-Wire Network and write it into
		/// the provided array.
		/// 
		/// </summary>
		/// <param name="arr">    array in which to write the received bytes
		/// </param>
		/// <param name="len">    length of data bytes to receive
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override void  getBlock(byte[] arr, int len)
		{
			getBlock(arr, 0, len);
		}
		
		/// <summary> Get a block of data from the 1-Wire Network and write it into
		/// the provided array.
		/// 
		/// </summary>
		/// <param name="arr">    array in which to write the received bytes
		/// </param>
		/// <param name="off">    offset into the array to start
		/// </param>
		/// <param name="len">    length of data bytes to receive
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override void  getBlock(byte[] arr, int off, int len)
		{
            // offload arr to a "byte" array
           byte[] myarr = new byte[len];

           Array.Copy(arr, 0, myarr, off, len);
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			int rt;
			if (off == 0)
			{
				for (int i = 0; i < len; i++)
					myarr[i] = (byte) SupportClass.Identity(0x0FF);
				rt = TMEX_LIB_x86.TMBlockStream(sessionHandle, myarr, (short) len);
				// release the session
				release_session();
			}
			else
			{
				byte[] dataBlock = new byte[len];
				for (int i = 0; i < len; i++)
					dataBlock[i] = (byte) SupportClass.Identity(0x0FF);
				rt = TMEX_LIB_x86.TMBlockStream(sessionHandle, dataBlock, (short) len);
				// release the session
				release_session();
				Array.Copy(dataBlock, 0, arr, off, len);
			}
			
			// check for adapter communcication problems
			if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error " + rt);
		}
		
		/// <summary> Sends a block of data and returns the data received in the same array.
		/// This method is used when sending a block that contains reads and writes.
		/// The 'read' portions of the data block need to be pre-loaded with 0xFF's.
		/// It starts sending data from the index at offset 'off' for length 'len'.
		/// 
		/// </summary>
		/// <param name="dataBlock"> array of data to transfer to and from the 1-Wire Network.
		/// </param>
		/// <param name="off">       offset into the array of data to start
		/// </param>
		/// <param name="len">       length of data to send / receive starting at 'off'
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override void  dataBlock(byte[] dataBlock, int off, int len)
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			int rt = 0;
			if (len > 1023)
			{
				byte[] dataBlockBuffer = new byte[1023];
				// Change to only do 1023 bytes at a time
				int numblocks = len / 1023;
				int extra = len % 1023;
				for (int i = 0; i < numblocks; i++)
				{
                    Buffer.BlockCopy(dataBlock, off + i * 1023, dataBlockBuffer, 0, 1023);
					//Array.Copy(dataBlock, off + i * 1023, dataBlockBuffer, 0, 1023);
					rt = TMEX_LIB_x86.TMBlockStream(sessionHandle, dataBlockBuffer, (short) 1023);
                    Buffer.BlockCopy(dataBlockBuffer, 0, dataBlock, off + i * 1023, 1023);
					//Array.Copy(dataBlockBuffer, 0, dataBlock, off + i * 1023, 1023);
					if (rt != 1023)
						break;
				}
				if ((rt >= 0) && (extra > 0))
				{
                    Buffer.BlockCopy(dataBlock, off + numblocks * 1023, dataBlockBuffer, 0, extra);
					//Array.Copy(dataBlock, off + numblocks * 1023, dataBlockBuffer, 0, extra);
					rt = TMEX_LIB_x86.TMBlockStream(sessionHandle, dataBlockBuffer, (short) extra);
                    Buffer.BlockCopy(dataBlockBuffer, 0, dataBlock, off + numblocks * 1023, extra);
					//Array.Copy(dataBlockBuffer, 0, dataBlock, off + numblocks * 1023, extra);
				}
			}
			else if (off > 0)
			{
				byte[] dataBlockOffset = new byte[len];
                Buffer.BlockCopy(dataBlock, off, dataBlockOffset, 0, len);
				//Array.Copy(dataBlock, off, dataBlockOffset, 0, len);
				rt = TMEX_LIB_x86.TMBlockStream(sessionHandle, dataBlockOffset, (short) len);
                Buffer.BlockCopy(dataBlockOffset, 0, dataBlock, off, len);
				//Array.Copy(dataBlockOffset, 0, dataBlock, off, len);
			}
			else
			{
                rt = TMEX_LIB_x86.TMBlockStream(sessionHandle, dataBlock, (short)len);
			}
			// release the session
			release_session();
			
			// check for adapter communcication problems
			if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error " + rt);
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
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override int reset()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			// do 1-Wire reset
			int rt = TMEX_LIB_x86.TMTouchReset(sessionHandle);
			// release the session
			release_session();
			
			// check for adapter communcication problems
			if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error " + rt);
			else if (rt == 3)
				throw new OneWireIOException("1-Wire Net shorted");
			
			return rt;
		}
		
		//--------
		//-------- 1-Wire Network power methods  
		//--------
		
		/// <summary> Sets the 1-Wire Network voltage to supply power to an iButton device.
		/// This method takes a time parameter that indicates whether the
		/// power delivery should be done immediately, or after certain
		/// conditions have been met. <p>
		/// 
		/// Note: to avoid getting an exception,
		/// use the canDeliverPower() and canDeliverSmartPower()
		/// method to check it's availability. <p>
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
		/// <returns> <code>true</code> if the voltage change was successful,
		/// <code>false</code> otherwise.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		public override bool startPowerDelivery(int changeCondition)
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			if (!adapterSpecFeatures[TMEX_LIB_x86.FEATURE_POWER])
				throw new OneWireException("Hardware option not available");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			// start 12Volt pulse
			int rt = TMEX_LIB_x86.TMOneWireLevel(sessionHandle, TMEX_LIB_x86.LEVEL_SET, TMEX_LIB_x86.LEVEL_STRONG_PULLUP, (short) changeCondition);
			// release the session
			release_session();
			
			// check for adapter communication problems
			if (rt == - 3)
				throw new OneWireException("Adapter type does not support power delivery");
			else if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error" + rt);
			// check for could not set
			else if ((rt != TMEX_LIB_x86.LEVEL_STRONG_PULLUP) && (changeCondition == CONDITION_NOW))
				throw new OneWireIOException("native TMEX_LIB_x86 error: could not set adapter to desired level: " + rt);
			
			return true;
		}
		
		/// <summary> Sets the 1-Wire Network voltage to eprom programming level.
		/// This method takes a time parameter that indicates whether the
		/// power delivery should be done immediately, or after certain
		/// conditions have been met. <p>
		/// 
		/// Note: to avoid getting an exception,
		/// use the canProgram() method to check it's
		/// availability. <p>
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
		/// <returns> <code>true</code> if the voltage change was successful,
		/// <code>false</code> otherwise.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		/// <summary>         or the adapter does not support this operation
		/// </summary>
		public override bool startProgramPulse(int changeCondition)
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			if (!adapterSpecFeatures[TMEX_LIB_x86.FEATURE_PROGRAM])
				throw new OneWireException("Hardware option not available");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			int rt;
			// if pulse is 'now' then use TMProgramPulse
			if (changeCondition == TMEX_LIB_x86.PRIMED_NONE)
			{
				rt = TMEX_LIB_x86.TMProgramPulse(sessionHandle);
				// change rt value to be compatible with TMOneWireLevel
				if (rt != 0)
					rt = TMEX_LIB_x86.LEVEL_PROGRAM;
			}
			else
			{
				// start 12Volt pulse
				rt = TMEX_LIB_x86.TMOneWireLevel(sessionHandle, TMEX_LIB_x86.LEVEL_SET, TMEX_LIB_x86.LEVEL_PROGRAM, (short) changeCondition);
			}
			// release the session
			release_session();
			
			// check for adapter communication problems
			if (rt == - 3)
				throw new OneWireException("Adapter type does not support EPROM programming");
			else if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error" + rt);
			// check for could not set
			else if ((rt != TMEX_LIB_x86.LEVEL_PROGRAM) && (changeCondition == CONDITION_NOW))
				throw new OneWireIOException("native TMEX_LIB_x86 error: could not set adapter to desired level: " + rt);
			
			return true;
		}
		
		/// <summary> Sets the 1-Wire Network voltage to 0 volts.  This method is used
		/// rob all 1-Wire Network devices of parasite power delivery to force
		/// them into a hard reset.
		/// 
		/// </summary>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		/// <summary>         or the adapter does not support this operation
		/// </summary>
		public override void  startBreak()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			// start break
			int rt = TMEX_LIB_x86.TMOneWireLevel(sessionHandle, TMEX_LIB_x86.LEVEL_SET, TMEX_LIB_x86.LEVEL_BREAK, TMEX_LIB_x86.PRIMED_NONE);
			// release the session
			release_session();
			
			// check for adapter communication problems
			if (rt == - 3)
				throw new OneWireException("Adapter type does not support break");
			else if (rt == - 12)
				throw new OneWireException("1-Wire Adapter communication exception");
			// check for microlan exception
			else if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error" + rt);
			// check for could not set
			else if (rt != TMEX_LIB_x86.LEVEL_BREAK)
				throw new OneWireIOException("native TMEX_LIB_x86 error: could not set adapter to break: " + rt);
		}
		
		/// <summary> Sets the 1-Wire Network voltage to normal level.  This method is used
		/// to disable 1-Wire conditions created by startPowerDelivery and
		/// startProgramPulse.  This method will automatically be called if
		/// a communication method is called while an outstanding power
		/// command is taking place.
		/// 
		/// </summary>
		/// <throws>  OneWireIOException on a 1-Wire communication error </throws>
		/// <throws>  OneWireException on a setup error with the 1-Wire adapter </throws>
		/// <summary>         or the adapter does not support this operation
		/// </summary>
		public override void  setPowerNormal()
		{
			// check if port is selected
			if ((portNum < 0) || (portType < 0))
				throw new OneWireException("Port not selected");
			
			// get a session 
			if (!get_session())
				throw new OneWireException("Port in use");
			
			// set back to normal
			int rt = TMEX_LIB_x86.TMOneWireLevel(sessionHandle, TMEX_LIB_x86.LEVEL_SET, TMEX_LIB_x86.LEVEL_NORMAL, TMEX_LIB_x86.PRIMED_NONE);
			// release the session
			release_session();
			
			if (rt < 0)
				throw new OneWireIOException("native TMEX_LIB_x86 error" + rt);
		}
		
		//--------
		//-------- 1-Wire Network speed methods 
		//--------
		
		//--------
		//-------- Misc 
		//--------
		
		/// <summary> Select the DotNet specified port type (0 to 15)  Use this
		/// method if the constructor with the PortType cannot be used.
		/// 
		/// 
		/// </summary>
		/// <param name="newPortType">
		/// </param>
		/// <returns>  true if port type valid.  Instance is only usable
		/// if this returns false.
		/// </returns>
		public virtual bool setTMEXPortType(int newPortType)
		{
			// check if already have a session handle open on old port
			if (this.sessionHandle > 0)
				TMEX_LIB_x86.TMEndSession(sessionHandle);
			
			this.sessionHandle = 0;
			this.inExclusive = false;
			
			// read the version strings
			TMEX_LIB_x86.Get_Version(this.mainVersionBuffer);
			// will fail if not valid port type
			if (TMEX_LIB_x86.TMGetTypeVersion(newPortType, this.typeVersionBuffer) > 0)
			{
				// set default port type
				portType = newPortType;
				return true;
			}
			return false;
		}
		
		//--------
		//-------- Additional Native Methods 
		//--------

/*		
		/// <summary> CleanUp the native state for classes owned by the provided
		/// thread.
		/// </summary>
		public static void  CleanUpByThread(SupportClass.ThreadClass thread)
		{
			//cleanup();
		}
*/		
		/// <summary> Cleanup native (called on finalize of this instance)</summary>
		private void  cleanup()
		{
			// used to clear 'class state' variables, which aren't around anymore
			try
			{
				freePort();
			}
			catch (System.Exception e)
			{
				;
			}
		}
		
		
		//--------------------------------------------------------------------------
		// Parse the version string for a token
		//
		private static System.String getToken(System.String verStr, int token)
		{
			int currentToken = - 1;
			bool inToken = false; ;
			System.String toReturn = "";
			
			for (int i = 0; i < verStr.Length; i++)
			{
				if ((verStr[i] != ' ') && (!inToken))
				{
					inToken = true;
					currentToken++;
				}
				
				if ((verStr[i] == ' ') && (inToken))
					inToken = false;
				
				if (((inToken) && (currentToken == token)) || ((token == 255) && (currentToken > TOKEN_DATE)))
					toReturn += verStr[i];
			}
			return toReturn;
		}
		
		/// <summary> Attempt to get a TMEX_LIB_x86 session.  If already in an 'exclusive' block
		/// then just return.  
		/// </summary>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'get_session'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		private bool get_session()
		{
           	lock (this)
			{
				int[] sessionOptions = new int[]{TMEX_LIB_x86.SESSION_INFINITE};
				
				// check if in exclusive block
				if (inExclusive)
				{
					// make sure still valid (if not get a new one)
					if (TMEX_LIB_x86.TMValidSession(sessionHandle) > 0)
						return true;
				}
								
				// attempt to get a session handle (2 sec timeout).
                // Use .NET's TimeSpan class to detect elapsed time.
                TimeSpan elapsedTime;
                long tickCounterBegin = DateTime.Now.Ticks;
            
                do
                {
                   sessionHandle = TMEX_LIB_x86.TMExtendedStartSession((short)portNum, (short)portType, sessionOptions);

                   // this port type does not exist
                   if (sessionHandle == -201)
                   {
                      break;
                   }
                   // valid handle
                   else if (sessionHandle > 0)
                   {
                      // success
                      return true;
                   }
                   elapsedTime = new TimeSpan(DateTime.Now.Ticks - tickCounterBegin);
                }
                while (elapsedTime.TotalMilliseconds < 2000.0); // while elapsed time is less than 2 seconds
				
				// timeout of invalid porttype
				sessionHandle = 0;
				
				return false;
			}
		}
		
		/// <summary>  Release a TMEX_LIB_x86 session.  If already in an 'exclusive' block
		/// then just return.  
		/// </summary>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'release_session'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		private bool release_session()
		{
			lock (this)
			{
				// check if in exclusive block
				if (inExclusive)
					return true;
				
				// close the session
				TMEX_LIB_x86.TMEndSession(sessionHandle);
				
				// clear out handle (used to indicate not session)
				sessionHandle = 0;
				
				return true;
			}
		}
	}
	
	class TMEX_LIB_x86
	{
		/// <summary>feature indexes into adapterSpecFeatures array </summary>
		public const int FEATURE_OVERDRIVE = 0;
		public const int FEATURE_POWER = 1;
		public const int FEATURE_PROGRAM = 2;
		public const int FEATURE_FLEX = 3;
		public const int FEATURE_BREAK = 4;
		/// <summary>Speed settings for TMOneWireCOM </summary>
		public const short TIME_NORMAL = 0;
		public const short TIME_OVERDRV = 1;
		public const short TIME_RELAXED = 2;
		/* for TMOneWireLevel */
		public const short LEVEL_NORMAL = 0;
		public const short LEVEL_STRONG_PULLUP = 1;
		public const short LEVEL_BREAK = 2;
		public const short LEVEL_PROGRAM = 3;
		public const short PRIMED_NONE = 0;
		public const short PRIMED_BIT = 1;
		public const short PRIMED_BYTE = 2;
		public const short LEVEL_READ = 1;
		public const short LEVEL_SET = 0;
		/// <summary>session options </summary>
		public const int SESSION_INFINITE = 1;
		public const int SESSION_RSRC_RELEASE = 2;
		
		//---------------------------------------------------------------------------
		// TMEX_LIB_x86 - Session

        // TMEX - Session
        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMExtendedStartSession", CallingConvention = CallingConvention.StdCall)]
        public static extern int TMExtendedStartSession(short portNum, short portType, int[] sessionOptions);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMFamilySearchSetup", CallingConvention = CallingConvention.StdCall)]
        public static extern int TMFamilySearchSetup(int sessionHandle, byte[] stateBuffer, short familyType);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMStartSession", CallingConvention = CallingConvention.StdCall)]
        public static extern int TMStartSession(short i1);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMValidSession", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMValidSession(int sessionHandle);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMEndSession", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMEndSession(int sessionHandle);

        // TMEX - Transport
        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMBlockIO", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMBlockIO(int sessionHandle, byte[] dataBlock, short len);

        // TMEX - Network
        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMFirst", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMFirst(int sessionHandle, byte[] stateBuffer);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMNext", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMNext(int sessionHandle, byte[] stateBuffer);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMNext", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMAccess(int sessionHandle, byte[] stateBuffer);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMStrongAccess", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMStrongAccess(int sessionHandle, byte[] stateBuffer);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMStrongAlarmAccess", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMStrongAlarmAccess(int sessionHandle, byte[] stateBuffer);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMOverAccess", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMOverAccess(int sessionHandle, byte[] stateBuffer);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMRom", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMRom(int sessionHandle, byte[] stateBuffer, short[] ROM);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMNext", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMSearch(int sessionHandle, byte[] stateBuffer, short doResetFlag, short skipResetOnSearchFlag, short searchCommand);

        // TMEX - Hardware Specific
        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMSetup", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMSetup(int sessionHandle);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMTouchReset", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMTouchReset(int sessionHandle);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMTouchByte", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMTouchByte(int sessionHandle, short byteValue);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMTouchBit", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMTouchBit(int sessionHandle, short bitValue);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMProgramPulse", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMProgramPulse(int sessionHandle);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMClose", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMClose(int sessionHandle);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMOneWireCom", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMOneWireCom(int sessionHandle, short command, short argument);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMOneWireLevel", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMOneWireLevel(int sessionHandle, short command, short argument, short changeCondition);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMGetTypeVersion", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMGetTypeVersion(int portType, System.Text.StringBuilder sbuff);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "Get_Version", CallingConvention = CallingConvention.StdCall)]
        public static extern short Get_Version(System.Text.StringBuilder sbuff);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMBlockStream", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMBlockStream(int sessionHandle, byte[] dataBlock, short len);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMGetAdapterSpec", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMGetAdapterSpec(int sessionHandle, byte[] adapterSpec);

        [DllImportAttribute("IBFS32.dll", EntryPoint = "TMReadDefaultPort", CallingConvention = CallingConvention.StdCall)]
        public static extern short TMReadDefaultPort(ref short portTypeRef, ref short portNumRef);
        //---------------------------------------------------------------------------

        public static bool[] getFeaturesFromSpecification(byte[] adapterSpec)
        {
           bool[] features = new bool[32];
           for (int i = 0; i < 32; i++)
              features[i] = (adapterSpec[i * 2] > 0) || (adapterSpec[i * 2 + 1] > 0);
           return features;
        }

        public static System.String getDescriptionFromSpecification(byte[] adapterSpec)
        {
           int i;
           // find null terminator for string
           for (i = 64; i < 319; i++)
              if (adapterSpec[i] == 0)
                 break;
           return new System.String(
              System.Text.UTF8Encoding.UTF8.GetChars(adapterSpec), 64, i - 64);
        }
	}
}