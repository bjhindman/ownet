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
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> <P>Memory bank interface class for basic memory communication with
	/// iButtons (or 1-Wire devices).  The method <CODE> getMemoryBanks </CODE>
	/// in all 1-Wire Containers ({@link com.dalsemi.onewire.container.OneWireContainer OneWireContainer})
	/// returns an Enumeration of this interface to be used to read or write it's
	/// memory. If the 1-Wire device does not have memory or the memory is non-standard,
	/// then this enumeration may be empty. 
	/// A MemoryBank returned from this method may also implement the
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}, 
	/// or {@link com.dalsemi.onewire.container.OTPMemoryBank OTPMemoryBank} interfaces, 
	/// to provide additional functionality. </P> 
	/// 
	/// <P>The MemoryBank methods can be organized into the following categories: </P>
	/// <UL>
	/// <LI> <B> Information </B>
	/// <UL>
	/// <LI> {@link #getBankDescription() getBankDescription} 
	/// <LI> {@link #getSize() getSize} 
	/// <LI> {@link #getStartPhysicalAddress() getStartPhysicalAddress} 
	/// <LI> {@link #isGeneralPurposeMemory() isGeneralPurposeMemory} 
	/// <LI> {@link #isReadWrite() isReadWrite}
	/// <LI> {@link #isWriteOnce() isWriteOnce}
	/// <LI> {@link #isReadOnly() isReadOnly}
	/// <LI> {@link #isNonVolatile() isNonVolatile}
	/// <LI> {@link #needsProgramPulse() needsProgramPulse}
	/// <LI> {@link #needsPowerDelivery() needsPowerDelivery}
	/// </UL>
	/// <LI> <B> Options </B>
	/// <UL>
	/// <LI> {@link #setWriteVerification(boolean) setWriteVerification}
	/// </UL>
	/// <LI> <B> I/O </B>
	/// <UL>
	/// <LI> {@link #read(int,boolean,byte[],int,int) read}
	/// <LI> {@link #write(int,byte[], int, int) write}
	/// </UL>
	/// </UL>
	/// 
	/// <H3> Usage </H3> 
	/// 
	/// <DL> 
	/// <DD> <H4> Example 1</H4> 
	/// Display some features of MemoryBank instance 'mb': 
	/// <PRE> <CODE>
	/// if (mb.isWriteOnce())
	/// System.out.println("MemoryBank is write-once");
	/// 
	/// if (mb.needsProgramPulse())
	/// System.out.println("MemoryBank requires program-pulse to write");
	/// </CODE> </PRE>
	/// 
	/// <DD> <H4> Example 2</H4> 
	/// Write the entire contents of a MemoryBank instance 'mb' with zeros: 
	/// <PRE> <CODE>
	/// byte[] write_buf = new byte[mb.getSize()];
	/// for (int i = 0; i < write_buf.length; i++)
	/// write_buf[i] = (byte)0;
	/// 
	/// mb.write(0, write_buf, 0, write_buf.length);
	/// </CODE> </PRE>
	/// 
	/// <DD> <H4> Example 3</H4>
	/// Read the entire contents of a MemoryBank instance 'mb': 
	/// <PRE> <CODE>
	/// byte[] read_buf = new byte[mb.getSize()];
	/// 
	/// mb.read(0, false, read_buf, 0, read_buf.length);
	/// </CODE> </PRE>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.PagedMemoryBank">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OTPMemoryBank">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer04">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer06">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer08">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer09">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer0A">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer0B">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer0C">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer0F">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer12">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer13">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer14">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer18">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer1A">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer1D">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer20">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer21">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer23">
	/// 
	/// </seealso>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public interface MemoryBank
	{
		/// <summary> Gets a string description of this memory bank.
		/// 
		/// </summary>
		/// <returns>  the memory bank description
		/// </returns>
		System.String BankDescription
		{
			get;
			
		}
		/// <summary> Checks to see if this memory bank is general purpose
		/// user memory.  If it is NOT then it may be Memory-Mapped and writing
		/// values to this memory may affect the behavior of the 1-Wire
		/// device.
		/// 
		/// </summary>
		/// <returns>  <CODE> true </CODE> if this memory bank is general purpose
		/// </returns>
		bool GeneralPurposeMemory
		{
			get;
			
		}
		/// <summary> Gets the size of this memory bank in bytes.
		/// 
		/// </summary>
		/// <returns>  number of bytes in current memory bank
		/// </returns>
		int Size
		{
			get;
			
		}
		/// <summary> Checks to see if this memory bank is read/write.
		/// 
		/// </summary>
		/// <returns>  <CODE> true </CODE> if this memory bank is read/write
		/// </returns>
		bool ReadWrite
		{
			get;
			
		}
		/// <summary> Checks to see if this memory bank is write once such
		/// as with EPROM technology.
		/// 
		/// </summary>
		/// <returns>  <CODE>  true </CODE> if this memory bank can only be written once
		/// </returns>
		bool WriteOnce
		{
			get;
			
		}
		/// <summary> Checks to see if this memory bank is read only.
		/// 
		/// </summary>
		/// <returns>  <CODE>  true </CODE> if this memory bank can only be read
		/// </returns>
		bool ReadOnly
		{
			get;
			
		}
		/// <summary> Checks to see if this memory bank is non-volatile.  Memory is
		/// non-volatile if it retains its contents even when removed from
		/// the 1-Wire network.
		/// 
		/// </summary>
		/// <returns>  <CODE> true </CODE>  if this memory bank is non volatile
		/// </returns>
		bool NonVolatile
		{
			get;
			
		}
		/// <summary> Gets the starting physical address of this bank.  Physical
		/// banks are sometimes sub-divided into logical banks due to changes
		/// in attributes.  Note that this method is for information only.  The read
		/// and write methods will automatically calculate the physical address
		/// when writing to a logical memory bank.
		/// 
		/// </summary>
		/// <returns>  physical starting address of this logical bank
		/// </returns>
		int StartPhysicalAddress
		{
			get;
			
		}
		/// <summary> Sets or clears write verification for the 
		/// {@link #write(int,byte[],int,int) write} method.
		/// 
		/// </summary>
		/// <param name="doReadVerf">  <CODE> true </CODE>  (default) 
		/// verify write in 'write',
		/// <CODE> false </CODE> don't verify write (used on
		/// Write-Once bit manipulation)
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OTPMemoryBank">
		/// </seealso>
		bool WriteVerification
		{
			set;
			
		}
		
		//--------
		//-------- Memory Bank Feature methods
		//--------
		
		/// <summary> Checks to see if this  memory bank requires a 
		/// 'ProgramPulse' in order to write.
		/// 
		/// </summary>
		/// <returns>  <CODE> true </CODE>  if writing to this memory bank 
		/// requires a 'ProgramPulse' from the 1-Wire Adapter.
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.adapter.DSPortAdapter">
		/// </seealso>
		bool needsProgramPulse();
		
		/// <summary> Checks to see if this memory bank requires 'PowerDelivery'
		/// in order to write.
		/// 
		/// </summary>
		/// <returns>  <CODE> true </CODE> if writing to this memory bank 
		/// requires 'PowerDelivery' from the 1-Wire Adapter
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.adapter.DSPortAdapter">
		/// </seealso>
		bool needsPowerDelivery();
		
		//--------
		//-------- I/O methods
		//--------
		
		/// <summary> Reads memory in this bank with no CRC checking (device or
		/// data). The resulting data from this API may or may not be what is on
		/// the 1-Wire device.  It is recommended that the data contain some kind
		/// of checking (CRC) like in the <CODE> readPagePacket </CODE> method in
		/// the
		/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
		/// interface.  Some 1-Wire devices provide thier own CRC as in 
		/// <CODE> readPageCRC </CODE> also found in the 
		/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank} 
		/// interface.  The <CODE> readPageCRC </CODE> method 
		/// is not supported on all memory types, see <CODE> hasPageAutoCRC </CODE>
		/// in the same interface.
		/// If neither is an option then this method could be called more
		/// then once to at least verify that the same data is read consistently.  The
		/// readContinue parameter is used to eliminate the overhead in re-accessing
		/// a part already being read from. For example, if pages 0 - 4 are to
		/// be read, readContinue would be set to false for page 0 and would be set
		/// to true for the next four calls.
		/// 
		/// <P> Note: Using readContinue = true  can only be used if the new
		/// read continuous where the last one led off
		/// and it is inside a 'beginExclusive/endExclusive'
		/// block.
		/// 
		/// </summary>
		/// <param name="startAddr">    starting address
		/// </param>
		/// <param name="readContinue"> <CODE> true </CODE> then device read is 
		/// continued without re-selecting
		/// </param>
		/// <param name="readBuf">      location for data read
		/// </param>
		/// <param name="offset">       offset into readBuf to place data
		/// </param>
		/// <param name="len">          length in bytes to read
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as  </throws>
		/// <summary>         no device present.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to 
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		void  read(int startAddr, bool readContinue, byte[] readBuf, int offset, int len);
		
		/// <summary> Writes memory in this bank. It is recommended that a structure with some
		/// built in error checking is used to provide data integrity on read.
		/// The method <CODE> writePagePacket </CODE> found in the 
		/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank} 
		/// interface, which automatically wraps the data in a length and CRC, could
		/// be used for this purpose.
		/// 
		/// <P> When using on Write-Once devices care must be taken to write into
		/// into empty space.  If <CODE> write </CODE> is used to write over an unlocked
		/// page on a Write-Once device it will fail.  If write verification
		/// is turned off with the method 
		/// {@link #setWriteVerification(boolean) setWriteVerification(false)} 
		/// then the result will be an 'AND' of the existing data and the new data. 
		/// 
		/// </summary>
		/// <param name="startAddr">    starting address
		/// </param>
		/// <param name="writeBuf">     data to write
		/// </param>
		/// <param name="offset">       offset into writeBuf to get data
		/// </param>
		/// <param name="len">          length in bytes to write
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as  </throws>
		/// <summary>         no device present or a read back verification fails.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to 
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		void  write(int startAddr, byte[] writeBuf, int offset, int len);
	}
}