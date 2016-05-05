/*---------------------------------------------------------------------------
* Copyright (C) 2008 Maxim Integrated Products, All Rights Reserved.
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
using com.dalsemi.onewire.adapter;
using OneWireException = com.dalsemi.onewire.OneWireException;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> <P>1-Wire&reg; container for the '1K-Bit protected 1-Wire EEPROM 
	/// family type <B>43</B> (hex), Maxim Integrated Products part number:
	/// <B>DS28EC20</B>.
	/// 
	/// <H3> Features </H3>
	/// <UL>
	/// <LI> 20480 bits of 5V EEPROM memory partitioned into 80 pages of 256 bits
	/// <LI> Individual 8-Page Groups of Memory Pages (Blocks) can be Permanently 
	/// Write Protected or Put in OTP EPROM-Emulation Mode ("Write to 0")
	/// <LI> 200k Write/Erase Cycle Endurance at +25C
	/// <LI> 256-Bit Scratchpad with Strict Read/Write Protocols Ensures Integrity 
	/// of Data Transfer
	/// <LI> Unique Factory-Programmed 64-Bit Registration Number Ensures Error-Free 
	/// Device Selection and Absolute Part Identity
	/// <LI> Switchpoint Hysteresis and Filtering to Optimize Performance in the 
	/// Presence of Noise
	/// <LI> Communicates to Host at 15.4kbps or 125kbps Using 1-Wire Protocol
	/// <LI> Reduces control, address, data and power to a single data pin
	/// <LI> Low cost TO92 package or 6-pin TSOC package
	/// <LI> Reads and writes at 5.0V + or - 5% from -40C
	/// to +85C.
	/// <LI> IEC 1000-4-2 Level 4 ESD Protection (8kV Contact, 15kV Air, Typical) 
	/// for I/O Pin
	/// </UL>
	/// 
	/// <P> The memory can also be accessed through the objects that are returned
	/// from the {@link #getMemoryBanks() getMemoryBanks} method. </P>
	/// 
	/// <DL>
	/// <DD> </A>
	/// </DL>
	/// 
	/// </summary>
	/// <version>  	0.00, 19 February 2008
	/// </version>
	/// <author>  DS
	/// </author>
	public class OneWireContainer43:OneWireContainer
	{
		/// <summary> Retrieve the Maxim Integrated Products part number of the iButton
		/// as a string.  For example 'DS1992'.
		/// 
		/// </summary>
		/// <returns> string represetation of the iButton name.
		/// </returns>
		override public System.String Name
		{
			get
			{
				return "DS28EC20";
			}
			
		}
		/// <summary> Retrieve the alternate Maxim Integrated Products part numbers or names.
		/// A 'family' of MicroLAN devices may have more than one part number
		/// depending on packaging.
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
		/// <summary> Retrieve a short description of the function of the iButton type.
		/// 
		/// </summary>
		/// <returns> string represetation of the function description.
		/// </returns>
		override public System.String Description
		{
			get
			{
				return "20Kb 1-Wire EEPROM";
			}
			
		}
		/// <summary> Returns the maximum speed this iButton can communicate at.
		/// 
		/// </summary>
		/// <returns>  max. communication speed.
		/// </returns>
		override public int MaxSpeed
		{
			get
			{
				return DSPortAdapter.SPEED_OVERDRIVE;
			}
			
		}
		/// <summary> Get an enumeration of memory bank instances that implement one or more
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
				System.Collections.ArrayList bank_vector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(2));
				
				// scratchpad
				//	  bank_vector.addElement(( MemoryBank ) sp);
				
				// main memory
				bank_vector.Add((MemoryBank) main_mem);
				
				// register memory
				bank_vector.Add((MemoryBank) register);
				
				return bank_vector.GetEnumerator();
			}
			
		}
		// Scratchpad access memory bank
		private MemoryBankScratchEE sp;
		
		/*
		* registery memory bank to control write-once (EPROM) mode
		*/
		private MemoryBankEEPROM register;
		
		/*
		* main memory bank 
		*/
		private MemoryBankEEPROM main_mem;
		
		/// <summary> Page Lock Flag</summary>
		public static byte WRITEONCE_FLAG = (byte) SupportClass.Identity(0xAA);
		
		//--------
		//-------- Static Final Variables
		//--------
		
		/// <summary> Default Constructor OneWireContainer43.
		/// Must call setupContainer before using.
		/// </summary>
		public OneWireContainer43():base()
		{
		}
		
		/// <summary> Create a container with a provided adapter object
		/// and the address of the iButton or 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton.
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public OneWireContainer43(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Create a container with a provided adapter object
		/// and the address of the iButton or 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton.
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public OneWireContainer43(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Create a container with a provided adapter object
		/// and the address of the iButton or 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton.
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public OneWireContainer43(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the memory banks
			initMem();
		}
		
		//--------
		//-------- Methods
		//--------
		
		/// <summary> Provide this container the adapter object used to access this device
		/// and provide the address of this iButton or 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton.
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public override void  setupContainer(DSPortAdapter sourceAdapter, byte[] newAddress)
		{
			base.setupContainer(sourceAdapter, newAddress);
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Provide this container the adapter object used to access this device
		/// and provide the address of this iButton or 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton.
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public override void  setupContainer(DSPortAdapter sourceAdapter, long newAddress)
		{
			base.setupContainer(sourceAdapter, newAddress);
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Provide this container the adapter object used to access this device
		/// and provide the address of this iButton or 1-Wire device.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter object required to communicate with
		/// this iButton.
		/// </param>
		/// <param name="newAddress">       address of this 1-Wire device
		/// </param>
		public override void  setupContainer(DSPortAdapter sourceAdapter, System.String newAddress)
		{
			base.setupContainer(sourceAdapter, newAddress);
			
			// initialize the memory banks
			initMem();
		}
		
		/// <summary> Construct the memory banks used for I/O.</summary>
		private void  initMem()
		{
			// scratch pad
			sp = new MemoryBankScratchEE(this);
			sp.size = 32;
			sp.pageLength = 32;
			sp.maxPacketDataLength = 29;
			sp.pageAutoCRC = true;
			sp.COPY_DELAY_LEN = 10;
			sp.ES_MASK = (byte) 0;
			
			// main memory
			main_mem = new MemoryBankEEPROM(this, sp);
			
			// initialize main memory
			main_mem.size = 2560;
			main_mem.numberPages = 80;
			main_mem.scratchPadSize = 32;
			//readOnly = false;
			//nonVolatile = true;
			//pageAutoCRC = false;
			//lockPage = true;
			//programPulse = false;
			//powerDelivery = true;
			//extraInfo = false;
			//extraInfoLength = 0;
			//extraInfoDescription = null;
			//writeVerification = false;
			//startPhysicalAddress = 0;
			//doSetSpeed = true;
			
			// register memory
			register = new MemoryBankEEPROM(this, sp);
			
			// initialize attributes of this memory bank 
			register.generalPurposeMemory = false;
			register.bankDescription = "Write-protect/EPROM-Mode control register";
			register.numberPages = 2;
			register.size = 64;
			register.pageLength = 32;
			register.maxPacketDataLength = 0;
			register.readWrite = true;
			register.writeOnce = false;
			register.readOnly = false;
			register.nonVolatile = true;
			register.pageAutoCRC = false;
			register.lockPage_Renamed_Field = false;
			register.programPulse = false;
			register.powerDelivery = true;
			register.extraInfo = false;
			register.extraInfoLength = 0;
			register.extraInfoDescription = null;
			register.writeVerification = false;
			register.startPhysicalAddress = 2560;
			register.doSetSpeed = true;
			
			// set the lock mb
			main_mem.mbLock = register;
		}
		
		//--------
		//-------- Custom Methods for this 1-Wire Device Type
		//--------
		
		/// <summary> Query to see if current memory bank is write write once such
		/// as with EPROM technology.
		/// 
		/// </summary>
		/// <returns>  'true' if current memory bank can only be written once
		/// </returns>
		public virtual bool isPageWriteOnce(int page)
		{
			byte[] rd_byte = new byte[1];
			
			register.read(page, false, rd_byte, 0, 1);
			
			return (rd_byte[0] == WRITEONCE_FLAG);
		}
		
		/// <summary> Lock the specifed page in the current memory bank.  Not supported
		/// by all devices.  See the method 'canLockPage()'.
		/// 
		/// </summary>
		/// <param name="page">  number of page to lock
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  setPageWriteOnce(int page)
		{
			byte[] wr_byte = new byte[1];
			
			wr_byte[0] = WRITEONCE_FLAG;
			
			register.write(page, wr_byte, 0, 1);
			
			// read back to verify
			if (!isPageWriteOnce(page))
				throw new OneWireIOException("Failed to set page to write-once mode.");
		}
	}
}