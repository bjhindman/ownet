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
namespace com.dalsemi.onewire.container
{
	
	/// <summary> <P> 1-Wire container for 8192 byte memory iButton, DS1996.  This container
	/// encapsulates the functionality of the iButton family 
	/// type <B>0C</B> (hex)</P>
	/// 
	/// <P> This iButton is primarily used as a read/write portable memory device. </P>
	/// 
	/// <H3> Features </H3> 
	/// <UL>
	/// <LI> 65536 bits (8192 bytes) of read/write nonvolatile memory
	/// <LI> 256-bit (32-byte) scratchpad ensures integrity of data
	/// transfer
	/// <LI> Memory partitioned into 256-bit (32-byte) pages for
	/// packetizing data
	/// <LI> Overdrive mode boosts communication to
	/// 142 kbits per second
	/// <LI> Data integrity assured with strict read/write
	/// protocols
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
	/// <LI> <B> Scratchpad </B> 
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
	/// <LI> <I> Size </I> 8192 starting at physical address 0
	/// <LI> <I> Features</I> Read/Write general-purpose non-volatile
	/// <LI> <I> Pages</I> 256 pages of length 32 bytes giving 29 bytes Packet data payload
	/// </UL> 
	/// </UL>
	/// 
	/// <H3> Usage </H3> 
	/// 
	/// <DL> 
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
	/// <DD><A HREF="http://pdfserv.maxim-ic.com/arpdf/DS1996.pdf"> http://pdfserv.maxim-ic.com/arpdf/DS1996.pdf</A>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.MemoryBank">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.PagedMemoryBank">
	/// 
	/// </seealso>
	/// <version>     0.01, 11 Dec 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public class OneWireContainer0C:OneWireContainer
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
				return "DS1996";
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
				return "65536 bit read/write nonvolatile memory partitioned " + "into two-hundred fifty-six pages of 256 bits each.";
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
				System.Collections.ArrayList bank_vector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(2));
				
				// scratchpad
				MemoryBankScratch scratch = new MemoryBankScratch(this);
				
				bank_vector.Add((MemoryBank) scratch);
				
				// NVRAM
				MemoryBankNV nv = new MemoryBankNV(this, scratch);
				
				nv.numberPages = 256;
				nv.size = 8192;
				
				bank_vector.Add((MemoryBank) nv);
				
				return bank_vector.GetEnumerator();
			}
			
		}
		
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
		public OneWireContainer0C():base()
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
		/// <seealso cref="OneWireContainer0C() OneWireContainer0C">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer0C(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
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
		/// <seealso cref="OneWireContainer0C() OneWireContainer0C">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer0C(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
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
		/// <seealso cref="OneWireContainer0C() OneWireContainer0C">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer0C(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
		}
		
		//--------
		//-------- Methods
		//--------
	}
}