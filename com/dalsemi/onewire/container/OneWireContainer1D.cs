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
using OneWireException = com.dalsemi.onewire.OneWireException;
using OneWireIOException = com.dalsemi.onewire.adapter.OneWireIOException;
using CRC16 = com.dalsemi.onewire.utils.CRC16;
using com.dalsemi.onewire.adapter;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> <P> 1-Wire container for 512 byte memory with external counters, DS2423.  This container
	/// encapsulates the functionality of the 1-Wire family 
	/// type <B>1D</B> (hex)</P>
	/// 
	/// <P> This 1-Wire device is primarily used as a counter with memory. </P>
	/// 
	/// <P> Each counter is assosciated with a memory page.  The counters for pages 
	/// 12 and 13 are incremented with a write to the memory on that page.  The counters
	/// for pages 14 and 15 are externally triggered. See the method
	/// {@link #readCounter(int) readCounter} to read a counter directly.  Note that the
	/// the counters may also be read with the <CODE> PagedMemoryBank </CODE> interface 
	/// as 'extra' information on a page read. </P>
	/// 
	/// <H3> Features </H3> 
	/// <UL>
	/// <LI> 4096 bits (512 bytes) of read/write nonvolatile memory
	/// <LI> 256-bit (32-byte) scratchpad ensures integrity of data
	/// transfer
	/// <LI> Memory partitioned into 256-bit (32-byte) pages for
	/// packetizing data
	/// <LI> Data integrity assured with strict read/write
	/// protocols
	/// <LI> Overdrive mode boosts communication to
	/// 142 kbits per second
	/// <LI> Four 32-bit read-only non rolling-over page
	/// write cycle counters
	/// <LI> Active-low external trigger inputs for two of
	/// the counters with on-chip debouncing
	/// compatible with reed and Wiegand switches
	/// <LI> 32 factory-preset tamper-detect bits to
	/// indicate physical intrusion
	/// <LI> On-chip 16-bit CRC generator for
	/// safeguarding data transfers
	/// <LI> Operating temperature range from -40@htmlonly &#176C @endhtmlonly to
	/// +70@htmlonly &#176C @endhtmlonly
	/// <LI> Over 10 years of data retention
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
	/// <LI> <B> Scratchpad Ex </B> 
	/// <UL> 
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank}, 
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 32 starting at physical address 0
	/// <LI> <I> Features</I> Read/Write not-general-purpose volatile
	/// <LI> <I> Pages</I> 1 pages of length 32 bytes 
	/// <LI> <I> Extra information for each page</I>  Target address, offset, length 3
	/// </UL> 
	/// <LI> <B> Main Memory </B>
	/// <UL> 
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank}, 
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 384 starting at physical address 0
	/// <LI> <I> Features</I> Read/Write general-purpose non-volatile
	/// <LI> <I> Pages</I> 12 pages of length 32 bytes giving 29 bytes Packet data payload
	/// <LI> <I> Page Features </I> page-device-CRC 
	/// </UL> 
	/// <LI> <B> Memory with write cycle counter </B>
	/// <UL> 
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank}, 
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 64 starting at physical address 384
	/// <LI> <I> Features</I> Read/Write general-purpose non-volatile
	/// <LI> <I> Pages</I> 2 pages of length 32 bytes giving 29 bytes Packet data payload
	/// <LI> <I> Page Features </I> page-device-CRC 
	/// <LI> <I> Extra information for each page</I>  Write cycle counter, length 8
	/// </UL> 
	/// <LI> <B> Memory with externally triggered counter </B>
	/// <UL> 
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank}, 
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 64 starting at physical address 448
	/// <LI> <I> Features</I> Read/Write general-purpose non-volatile
	/// <LI> <I> Pages</I> 2 pages of length 32 bytes giving 29 bytes Packet data payload
	/// <LI> <I> Page Features </I> page-device-CRC 
	/// <LI> <I> Extra information for each page</I>  Externally triggered counter, length 8
	/// </UL> 
	/// </UL>
	/// 
	/// <H3> Usage </H3> 
	/// 
	/// <DL> 
	/// <DD> <H4> Example</H4> 
	/// Read the two external counters of this containers instance 'owd': 
	/// <PRE> <CODE>
	/// System.out.print("Counter on page 14: " + owd.readCounter(14));
	/// System.out.print("Counter on page 15: " + owd.readCounter(15));
	/// </CODE> </PRE>
	/// <DD> See the usage example in 
	/// {@link com.dalsemi.onewire.container.OneWireContainer OneWireContainer}
	/// to enumerate the MemoryBanks.
	/// <DD> See the usage examples in 
	/// {@link com.dalsemi.onewire.container.MemoryBank MemoryBank} and
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// for bank specific operations.
	/// </DL>
	/// 
	/// <H3> DataSheet </H3> 
	/// <DL>
	/// <DD><A HREF="http://pdfserv.maxim-ic.com/arpdf/DS2422-DS2423.pdf"> http://pdfserv.maxim-ic.com/arpdf/DS2422-DS2423.pdf</A>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.MemoryBank">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.PagedMemoryBank">
	/// 
	/// </seealso>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public class OneWireContainer1D:OneWireContainer
	{
		/// <summary> Get the Maxim Integrated Products part number of the iButton
		/// or 1-Wire Device as a string.  For example 'DS1992'.
		/// 
		/// </summary>
		/// <returns> iButton or 1-Wire device name
		/// </returns>
		override public System.String Name
		{
			get
			{
				return "DS2423";
			}
			
		}
		/// <summary> Get a short description of the function of this iButton 
		/// or 1-Wire Device type.
		/// 
		/// </summary>
		/// <returns> device description
		/// </returns>
		override public System.String Description
		{
			get
			{
				return "1-Wire counter with 4096 bits of read/write, nonvolatile " + "memory.  Memory is partitioned into sixteen pages of 256 bits each.  " + "256 bit scratchpad ensures data transfer integrity.  " + "Has overdrive mode.  Last four pages each have 32 bit " + "read-only non rolling-over counter.  The first two counters " + "increment on a page write cycle and the second two have " + "active-low external triggers.";
			}
			
		}
		/// <summary> Get the maximum speed this iButton or 1-Wire device can
		/// communicate at.
		/// Override this method if derived iButton type can go faster then
		/// SPEED_REGULAR(0).
		/// 
		/// </summary>
		/// <returns> maximum speed
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer.setSpeed super.setSpeed">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.adapter.DSPortAdapter.SPEED_REGULAR DSPortAdapter.SPEED_REGULAR">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.adapter.DSPortAdapter.SPEED_OVERDRIVE DSPortAdapter.SPEED_OVERDRIVE">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.adapter.DSPortAdapter.SPEED_FLEX DSPortAdapter.SPEED_FLEX">
		/// </seealso>
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
				System.Collections.ArrayList bank_vector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(4));
				
				// scratchpad
				MemoryBankScratchEx scratch = new MemoryBankScratchEx(this);
				
				bank_vector.Add(scratch);
				
				// NVRAM 
				MemoryBankNVCRC nv = new MemoryBankNVCRC(this, scratch);
				
				nv.numberPages = 12;
				nv.size = 384;
				nv.extraInfoLength = 8;
				nv.readContinuePossible = false;
				nv.numVerifyBytes = 8;
				
				bank_vector.Add(nv);
				
				// NVRAM (with write cycle counters)
				nv = new MemoryBankNVCRC(this, scratch);
				nv.numberPages = 2;
				nv.size = 64;
				nv.bankDescription = "Memory with write cycle counter";
				nv.startPhysicalAddress = 384;
				nv.extraInfo = true;
				nv.extraInfoDescription = "Write cycle counter";
				nv.extraInfoLength = 8;
				nv.readContinuePossible = false;
				nv.numVerifyBytes = 8;
				
				bank_vector.Add(nv);
				
				// NVRAM (with external counters)
				nv = new MemoryBankNVCRC(this, scratch);
				nv.numberPages = 2;
				nv.size = 64;
				nv.bankDescription = "Memory with externally triggered counter";
				nv.startPhysicalAddress = 448;
				nv.extraInfo = true;
				nv.extraInfoDescription = "Externally triggered counter";
				nv.extraInfoLength = 8;
				
				bank_vector.Add(nv);
				
				return bank_vector.GetEnumerator();
			}
			
		}
		
		//--------
		//-------- Static Final Variables
		//--------
		
		/// <summary> DS2423 read memory command</summary>
		private static byte READ_MEMORY_COMMAND = (byte) SupportClass.Identity(0xA5);
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Internal buffer</summary>
		private byte[] buffer = new byte[14];
		
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
		public OneWireContainer1D():base()
		{
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
		/// <seealso cref="OneWireContainer1D() OneWireContainer1D">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer1D(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
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
		/// <seealso cref="OneWireContainer1D() OneWireContainer1D">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer1D(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
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
		/// <seealso cref="OneWireContainer1D() OneWireContainer1D">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer1D(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
		}
		
		//--------
		//-------- Information methods
		//--------
		
		//--------
		//-------- Custom Methods for this 1-Wire Device Type  
		//--------
		
		/// <summary> Read the counter value associated with a page on this 
		/// 1-Wire Device.
		/// 
		/// </summary>
		/// <param name="counterPage">   page number of the counter to read
		/// 
		/// </param>
		/// <returns>  4 byte value counter stored in a long integer
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as  </throws>
		/// <summary>         no 1-Wire device present.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to 
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		public virtual long readCounter(int counterPage)
		{
			
			// check if counter page provided is valid
			if ((counterPage < 12) || (counterPage > 15))
				throw new OneWireException("OneWireContainer1D-invalid counter page");
			
			// select the device 
			if (adapter.select(this.address))
			{
				int crc16;
				
				// read memory command
				buffer[0] = READ_MEMORY_COMMAND;
				crc16 = CRC16.compute(READ_MEMORY_COMMAND);
				
				// address of last data byte before counter
				int address = (counterPage << 5) + 31;
				
				// append the address
				buffer[1] = (byte) address;
				crc16 = CRC16.compute(buffer[1], crc16);
				buffer[2] = (byte) (SupportClass.URShift(address, 8));
				crc16 = CRC16.compute(buffer[2], crc16);
				
				// now add the read bytes for data byte,counter,zero bits, crc16
				for (int i = 3; i < 14; i++)
					buffer[i] = (byte) SupportClass.Identity(0xFF);
				
				// send the block
				adapter.dataBlock(buffer, 0, 14);
				
				// calculate the CRC16 on the result and check if correct
				if (CRC16.compute(buffer, 3, 11, crc16) == 0xB001)
				{	
					// extract the counter out of this verified packet
                    ulong return_count = 0;
					
					for (int i = 4; i >= 1; i--)
					{
						return_count <<= 8;
                        return_count |= (byte)(buffer[i + 3] & (byte)0xFF);
					}
					
					// return the result count
					return (long)return_count;
				}
			}
			
			// device must not have been present
			throw new OneWireIOException("OneWireContainer1D-device not present");
		}
	}
}