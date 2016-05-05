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
using com.dalsemi.onewire.adapter;
using com.dalsemi.onewire.utils;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> Memory bank class for the EPROM section of iButtons and 1-Wire devices.
	/// 
	/// </summary>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS
	/// </author>
	class MemoryBankAppReg : OTPMemoryBank
	{
		/// <summary> Query to see get a string description of the current memory bank.
		/// 
		/// </summary>
		/// <returns>  String containing the memory bank description
		/// </returns>
		virtual public System.String BankDescription
		{
			get
			{
				return bankDescription;
			}
			
		}
		/// <summary> Query to see if the current memory bank is general purpose
		/// user memory.  If it is NOT then it is Memory-Mapped and writing
		/// values to this memory will affect the behavior of the 1-Wire
		/// device.
		/// 
		/// </summary>
		/// <returns>  'true' if current memory bank is general purpose
		/// </returns>
		virtual public bool GeneralPurposeMemory
		{
			get
			{
				return true;
			}
			
		}
		/// <summary> Query to see if current memory bank is read/write.
		/// 
		/// </summary>
		/// <returns>  'true' if current memory bank is read/write
		/// </returns>
		virtual public bool ReadWrite
		{
			get
			{
				return true;
			}
			
		}
		/// <summary> Query to see if current memory bank is write write once such
		/// as with EPROM technology.
		/// 
		/// </summary>
		/// <returns>  'true' if current memory bank can only be written once
		/// </returns>
		virtual public bool WriteOnce
		{
			get
			{
				return false;
			}
			
		}
		/// <summary> Query to see if current memory bank is read only.
		/// 
		/// </summary>
		/// <returns>  'true' if current memory bank can only be read
		/// </returns>
		virtual public bool ReadOnly
		{
			get
			{
				return false;
			}
			
		}
		/// <summary> Query to see if current memory bank non-volatile.  Memory is
		/// non-volatile if it retains its contents even when removed from
		/// the 1-Wire network.
		/// 
		/// </summary>
		/// <returns>  'true' if current memory bank non volatile.
		/// </returns>
		virtual public bool NonVolatile
		{
			get
			{
				return false;
			}
			
		}
		/// <summary> Query to get the starting physical address of this bank.  Physical
		/// banks are sometimes sub-divided into logical banks due to changes
		/// in attributes.
		/// 
		/// </summary>
		/// <returns>  physical starting address of this logical bank.
		/// </returns>
		virtual public int StartPhysicalAddress
		{
			get
			{
				return 0;
			}
			
		}
		/// <summary> Query to get the memory bank size in bytes.
		/// 
		/// </summary>
		/// <returns>  memory bank size in bytes.
		/// </returns>
		virtual public int Size
		{
			get
			{
				return PAGE_SIZE;
			}
			
		}
		/// <summary> Query to get the number of pages in current memory bank.
		/// 
		/// </summary>
		/// <returns>  number of pages in current memory bank
		/// </returns>
		virtual public int NumberPages
		{
			get
			{
				return 1;
			}
			
		}
		/// <summary> Query to get  page length in bytes in current memory bank.
		/// 
		/// </summary>
		/// <returns>   page length in bytes in current memory bank
		/// </returns>
		virtual public int PageLength
		{
			get
			{
				return PAGE_SIZE;
			}
			
		}
		/// <summary> Query to get Maximum data page length in bytes for a packet
		/// read or written in the current memory bank.  See the 'ReadPagePacket()'
		/// and 'WritePagePacket()' methods.  This method is only usefull
		/// if the current memory bank is general purpose memory.
		/// 
		/// </summary>
		/// <returns>  max packet page length in bytes in current memory bank
		/// </returns>
		virtual public int MaxPacketDataLength
		{
			get
			{
				return PAGE_SIZE - 2;
			}
			
		}
		/// <summary> Query to get the length in bytes of extra information that
		/// is read when read a page in the current memory bank.  See
		/// 'hasExtraInfo()'.
		/// 
		/// </summary>
		/// <returns>  number of bytes in Extra Information read when reading
		/// pages in the current memory bank.
		/// </returns>
		virtual public int ExtraInfoLength
		{
			get
			{
				return 1;
			}
			
		}
		/// <summary> Query to get a string description of what is contained in
		/// the Extra Informationed return when reading pages in the current
		/// memory bank.  See 'hasExtraInfo()'.
		/// 
		/// </summary>
		/// <returns> string describing extra information.
		/// </returns>
		virtual public System.String ExtraInfoDescription
		{
			get
			{
				return extraInfoDescription;
			}
			
		}
		/// <summary> Set the write verification for the 'write()' method.
		/// 
		/// </summary>
		/// <param name="doReadVerf">  true (default) verify write in 'write'
		/// false, don't verify write (used on
		/// Write-Once bit manipulation)
		/// </param>
		virtual public bool WriteVerification
		{
			set
			{
				writeVerification = value;
			}
			
		}
		
		//--------
		//-------- Static Final Variables
		//--------
		
		/// <summary> Memory page size</summary>
		public const int PAGE_SIZE = 8;
		
		/// <summary> Read Memory Command</summary>
		public static byte READ_MEMORY_COMMAND = (byte) SupportClass.Identity(0xC3);
		
		/// <summary> Main memory write command</summary>
		public static byte WRITE_MEMORY_COMMAND = (byte) SupportClass.Identity(0x99);
		
		/// <summary> Copy/Lock command</summary>
		public static byte COPY_LOCK_COMMAND = (byte) 0x5A;
		
		/// <summary> Copy/Lock command</summary>
		public static byte READ_STATUS_COMMAND = (byte) 0x66;
		
		/// <summary> Copy/Lock validation key</summary>
		public static byte VALIDATION_KEY = (byte) SupportClass.Identity(0xA5);
		
		/// <summary> Flag in status register indicated the page is locked</summary>
		public static byte LOCKED_FLAG = (byte) SupportClass.Identity(0xFC);
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Reference to the OneWireContainer this bank resides on.</summary>
		protected internal OneWireContainer ib;
		
		/// <summary> block of 0xFF's used for faster read pre-fill of 1-Wire blocks</summary>
		protected internal byte[] ffBlock;
		
		//--------
		//-------- Protected Variables for MemoryBank implementation 
		//--------
		
		/// <summary> Size of memory bank in bytes</summary>
		//protected internal int size;  // size is never assigned to and will always be 0
		
		/// <summary> Memory bank descriptions</summary>
		protected internal static System.String bankDescription = "Application register, non-volatile when locked";
		
		/// <summary> Flag if read back verification is enabled in 'write()'.</summary>
		protected internal bool writeVerification;
		
		//--------
		//-------- Protected Variables for PagedMemoryBank implementation 
		//--------
		
		/// <summary> Length of extra information when reading a page in memory bank</summary>
		//protected internal int extraInfoLength;  // never assigned to and will always be 0
		
		/// <summary> Extra information descriptoin when reading page in memory bank</summary>
		protected internal static System.String extraInfoDescription = "Page Locked flag";
		
		//--------
		//-------- Protected Variables for OTPMemoryBank implementation 
		//--------
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Memory bank contstuctor.  Requires reference to the OneWireContainer
		/// this memory bank resides on.  Requires reference to memory banks used
		/// in OTP operations.
		/// </summary>
		public MemoryBankAppReg(OneWireContainer ibutton)
		{
			
			// keep reference to ibutton where memory bank is
			ib = ibutton;
			
			// defaults
			writeVerification = true;
			
			// create the ffblock (used for faster 0xFF fills)
			ffBlock = new byte[50];
			
			for (int i = 0; i < 50; i++)
				ffBlock[i] = (byte) SupportClass.Identity(0xFF);
		}
		
		//--------
		//-------- MemoryBank query methods
		//--------
		
		/// <summary> Query to see if current memory bank pages need the adapter to
		/// have a 'ProgramPulse' in order to write to the memory.
		/// 
		/// </summary>
		/// <returns>  'true' if writing to the current memory bank pages
		/// requires a 'ProgramPulse'.
		/// </returns>
		public virtual bool needsProgramPulse()
		{
			return false;
		}
		
		/// <summary> Query to see if current memory bank pages need the adapter to
		/// have a 'PowerDelivery' feature in order to write to the memory.
		/// 
		/// </summary>
		/// <returns>  'true' if writing to the current memory bank pages
		/// requires 'PowerDelivery'.
		/// </returns>
		public virtual bool needsPowerDelivery()
		{
			return true;
		}
		
		//--------
		//-------- PagedMemoryBank query methods
		//--------
		
		/// <summary> Query to see if current memory bank pages can be read with
		/// the contents being verified by a device generated CRC.
		/// This is used to see if the 'ReadPageCRC()' can be used.
		/// 
		/// </summary>
		/// <returns>  'true' if current memory bank can be read with self
		/// generated CRC.
		/// </returns>
		public virtual bool hasPageAutoCRC()
		{
			return false;
		}
		
		/// <summary> Query to see if current memory bank pages when read deliver
		/// extra information outside of the normal data space.  Examples
		/// of this may be a redirection byte, counter, tamper protection
		/// bytes, or SHA-1 result.  If this method returns true then the
		/// methods 'ReadPagePacket()' and 'readPageCRC()' with 'extraInfo'
		/// parameter can be used.
		/// 
		/// </summary>
		/// <returns>  'true' if reading the current memory bank pages
		/// provides extra information.
		/// 
		/// </returns>
		/// <deprecated>  As of 1-Wire API 0.01, replaced by {@link #hasExtraInfo()}
		/// </deprecated>
		public virtual bool haveExtraInfo()
		{
			return true;
		}
		
		/// <summary> Checks to see if this memory bank's pages deliver extra 
		/// information outside of the normal data space,  when read.  Examples
		/// of this may be a redirection byte, counter, tamper protection
		/// bytes, or SHA-1 result.  If this method returns true then the
		/// methods with an 'extraInfo' parameter can be used:
		/// {@link #readPage(int,boolean,byte[],int,byte[]) readPage},
		/// {@link #readPageCRC(int,boolean,byte[],int,byte[]) readPageCRC}, and
		/// {@link #readPagePacket(int,boolean,byte[],int,byte[]) readPagePacket}.
		/// 
		/// </summary>
		/// <returns>  <CODE> true </CODE> if reading the this memory bank's 
		/// pages provides extra information
		/// 
		/// </returns>
		/// <seealso cref="readPage(int,boolean,byte[],int,byte[]) readPage(extra)">
		/// </seealso>
		/// <seealso cref="readPageCRC(int,boolean,byte[],int,byte[]) readPageCRC(extra)">
		/// </seealso>
		/// <seealso cref="readPagePacket(int,boolean,byte[],int,byte[]) readPagePacket(extra)">
		/// </seealso>
		/// <since> 1-Wire API 0.01
		/// </since>
		public virtual bool hasExtraInfo()
		{
			return true;
		}
		
		//--------
		//-------- OTPMemoryBank query methods
		//--------
		
		/// <summary> Query to see if current memory bank pages can be redirected
		/// to another pages.  This is mostly used in Write-Once memory
		/// to provide a means to update.
		/// 
		/// </summary>
		/// <returns>  'true' if current memory bank pages can be redirected
		/// to a new page.
		/// </returns>
		public virtual bool canRedirectPage()
		{
			return false;
		}
		
		/// <summary> Query to see if current memory bank pages can be locked.  A
		/// locked page would prevent any changes to the memory.
		/// 
		/// </summary>
		/// <returns>  'true' if current memory bank pages can be redirected
		/// to a new page.
		/// </returns>
		public virtual bool canLockPage()
		{
			return true;
		}
		
		/// <summary> Query to see if current memory bank pages can be locked from
		/// being redirected.  This would prevent a Write-Once memory from
		/// being updated.
		/// 
		/// </summary>
		/// <returns>  'true' if current memory bank pages can be locked from
		/// being redirected to a new page.
		/// </returns>
		public virtual bool canLockRedirectPage()
		{
			return false;
		}
		
		//--------
		//-------- MemoryBank I/O methods
		//--------
		
		/// <summary> Read  memory in the current bank with no CRC checking (device or
		/// data). The resulting data from this API may or may not be what is on
		/// the 1-Wire device.  It is recommends that the data contain some kind
		/// of checking (CRC) like in the readPagePacket() method or have
		/// the 1-Wire device provide the CRC as in readPageCRC().  readPageCRC()
		/// however is not supported on all memory types, see 'hasPageAutoCRC()'.
		/// If neither is an option then this method could be called more
		/// then once to at least verify that the same thing is read consistantly.
		/// 
		/// </summary>
		/// <param name="startAddr">    starting physical address
		/// </param>
		/// <param name="readContinue"> if 'true' then device read is continued without
		/// re-selecting.  This can only be used if the new
		/// read() continious where the last one led off
		/// and it is inside a 'beginExclusive/endExclusive'
		/// block.
		/// </param>
		/// <param name="readBuf">      byte array to place read data into
		/// </param>
		/// <param name="offset">       offset into readBuf to place data
		/// </param>
		/// <param name="len">          length in bytes to read
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  read(int startAddr, bool readContinue, byte[] readBuf, int offset, int len)
		{
			
			// check if read exceeds memory
			if ((startAddr + len) > PAGE_SIZE)
				throw new OneWireException("Read exceeds memory bank end");
			
			// ignore readContinue, silly with a 8 byte memory bank
			// attempt to put device at the correct speed
			ib.doSpeed();
			
			// select the device
			if (!ib.adapter.select(ib.address))
				throw new OneWireIOException("Device select failed");
			
			// start the read
			ib.adapter.putByte(READ_MEMORY_COMMAND);
			ib.adapter.putByte(startAddr & 0xFF);
			
			// file the read block with 0xFF
			Array.Copy(ffBlock, 0, readBuf, offset, len);
			
			// do the read
			ib.adapter.dataBlock(readBuf, offset, len);
		}
		
		/// <summary> Write  memory in the current bank.  It is recommended that
		/// when writing  data that some structure in the data is created
		/// to provide error free reading back with read().  Or the
		/// method 'writePagePacket()' could be used which automatically
		/// wraps the data in a length and CRC.
		/// 
		/// When using on Write-Once devices care must be taken to write into
		/// into empty space.  If write() is used to write over an unlocked
		/// page on a Write-Once device it will fail.  If write verification
		/// is turned off with the method 'setWriteVerification(false)' then
		/// the result will be an 'AND' of the existing data and the new data.
		/// 
		/// </summary>
		/// <param name="startAddr">    starting address
		/// </param>
		/// <param name="writeBuf">     byte array containing data to write
		/// </param>
		/// <param name="offset">       offset into writeBuf to get data
		/// </param>
		/// <param name="len">          length in bytes to write
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  write(int startAddr, byte[] writeBuf, int offset, int len)
		{
			// return if nothing to do
			if (len == 0)
				return ;
			
			// check if power delivery is available
			if (!ib.adapter.canDeliverPower())
				throw new OneWireException("Power delivery required but not available");
			
			// check if write exceeds memory
			if ((startAddr + len) > PAGE_SIZE)
				throw new OneWireException("Write exceeds memory bank end");
			
			// attempt to put device at the correct speed
			ib.doSpeed();
			
			// select the device
			if (!ib.adapter.select(ib.address))
				throw new OneWireIOException("Device select failed");
			
			// start the write
			ib.adapter.putByte(WRITE_MEMORY_COMMAND);
			ib.adapter.putByte(startAddr & 0xFF);
			
			// do the write
			ib.adapter.dataBlock(writeBuf, offset, len);
			
			// check for write verification
			if (writeVerification)
			{
				
				// read back
				byte[] read_buf = new byte[len];
				
				read(startAddr, true, read_buf, 0, len);
				
				// compare
				for (int i = 0; i < len; i++)
				{
					if ((byte) read_buf[i] != (byte) writeBuf[i + offset])
						throw new OneWireIOException("Read back from write compare is incorrect, page may be locked");
				}
			}
		}
		
		//--------
		//-------- PagedMemoryBank I/O methods
		//--------
		
		/// <summary> Read  page in the current bank with no
		/// CRC checking (device or data). The resulting data from this API
		/// may or may not be what is on the 1-Wire device.  It is recommends
		/// that the data contain some kind of checking (CRC) like in the
		/// readPagePacket() method or have the 1-Wire device provide the
		/// CRC as in readPageCRC().  readPageCRC() however is not
		/// supported on all memory types, see 'hasPageAutoCRC()'.
		/// If neither is an option then this method could be called more
		/// then once to at least verify that the same thing is read consistantly.
		/// 
		/// </summary>
		/// <param name="page">         page number to read packet from
		/// </param>
		/// <param name="readContinue"> if 'true' then device read is continued without
		/// re-selecting.  This can only be used if the new
		/// readPage() continious where the last one
		/// led off and it is inside a
		/// 'beginExclusive/endExclusive' block.
		/// </param>
		/// <param name="readBuf">      byte array to place read data into
		/// </param>
		/// <param name="offset">       offset into readBuf to place data
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  readPage(int page, bool readContinue, byte[] readBuf, int offset)
		{
			
			// check if for valid page
			if (page != 0)
				throw new OneWireException("Invalid page number for this memory bank");
			
			// do the read
			read(0, true, readBuf, offset, PAGE_SIZE);
		}
		
		/// <summary> Read  page with extra information in the current bank with no
		/// CRC checking (device or data). The resulting data from this API
		/// may or may not be what is on the 1-Wire device.  It is recommends
		/// that the data contain some kind of checking (CRC) like in the
		/// readPagePacket() method or have the 1-Wire device provide the
		/// CRC as in readPageCRC().  readPageCRC() however is not
		/// supported on all memory types, see 'hasPageAutoCRC()'.
		/// If neither is an option then this method could be called more
		/// then once to at least verify that the same thing is read consistantly.
		/// See the method 'hasExtraInfo()' for a description of the optional
		/// extra information some devices have.
		/// 
		/// </summary>
		/// <param name="page">         page number to read packet from
		/// </param>
		/// <param name="readContinue"> if 'true' then device read is continued without
		/// re-selecting.  This can only be used if the new
		/// readPage() continious where the last one
		/// led off and it is inside a
		/// 'beginExclusive/endExclusive' block.
		/// </param>
		/// <param name="readBuf">      byte array to place read data into
		/// </param>
		/// <param name="offset">       offset into readBuf to place data
		/// </param>
		/// <param name="extraInfo">    byte array to put extra info read into
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  readPage(int page, bool readContinue, byte[] readBuf, int offset, byte[] extraInfo)
		{
			
			// read the page data
			read(page, true, readBuf, offset, PAGE_SIZE);
			
			// read the extra information (status)
			readStatus(extraInfo);
		}
		
		/// <summary> Read a Universal Data Packet and extra information.  See the
		/// method 'readPagePacket()' for a description of the packet structure.
		/// See the method 'hasExtraInfo()' for a description of the optional
		/// extra information some devices have.
		/// 
		/// </summary>
		/// <param name="page">         page number to read packet from
		/// </param>
		/// <param name="readContinue"> if 'true' then device read is continued without
		/// re-selecting.  This can only be used if the new
		/// readPagePacket() continious where the last one
		/// stopped and it is inside a
		/// 'beginExclusive/endExclusive' block.
		/// </param>
		/// <param name="readBuf">      byte array to put data read. Must have at least
		/// 'getMaxPacketDataLength()' elements.
		/// </param>
		/// <param name="offset">       offset into readBuf to place data
		/// </param>
		/// <param name="extraInfo">    byte array to put extra info read into
		/// 
		/// </param>
		/// <returns>  number of data bytes written to readBuf at the offset.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual int readPagePacket(int page, bool readContinue, byte[] readBuf, int offset, byte[] extraInfo)
		{
			byte[] raw_buf = new byte[PAGE_SIZE];
			
			// read the entire page data
			read(page, true, raw_buf, 0, PAGE_SIZE);
			
			// check if length is realistic
			if (raw_buf[0] > (PAGE_SIZE - 2))
				throw new OneWireIOException("Invalid length in packet");
			
			// verify the CRC is correct
			if (CRC16.compute(raw_buf, 0, raw_buf[0] + 3, page) == 0x0000B001)
			{
				
				// extract the data out of the packet
				Array.Copy(raw_buf, 1, readBuf, offset, (byte) raw_buf[0]);
				
				// read the extra info
				readStatus(extraInfo);
				
				// return the length
				return raw_buf[0];
			}
			else
				throw new OneWireIOException("Invalid CRC16 in packet read");
		}
		
		/// <summary> Read a Universal Data Packet and extra information.  See the
		/// method 'readPagePacket()' for a description of the packet structure.
		/// See the method 'hasExtraInfo()' for a description of the optional
		/// extra information some devices have.
		/// 
		/// </summary>
		/// <param name="page">         page number to read packet from
		/// </param>
		/// <param name="readContinue"> if 'true' then device read is continued without
		/// re-selecting.  This can only be used if the new
		/// readPagePacket() continious where the last one
		/// stopped and it is inside a
		/// 'beginExclusive/endExclusive' block.
		/// </param>
		/// <param name="readBuf">      byte array to put data read. Must have at least
		/// 'getMaxPacketDataLength()' elements.
		/// </param>
		/// <param name="offset">       offset into readBuf to place data
		/// 
		/// </param>
		/// <returns>  number of data bytes written to readBuf at the offset.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual int readPagePacket(int page, bool readContinue, byte[] readBuf, int offset)
		{
			byte[] raw_buf = new byte[PAGE_SIZE];
			
			// read the entire page data
			read(page, true, raw_buf, 0, PAGE_SIZE);
			
			// check if length is realistic
			if (raw_buf[0] > (PAGE_SIZE - 2))
				throw new OneWireIOException("Invalid length in packet");
			
			// verify the CRC is correct
			if (CRC16.compute(raw_buf, 0, raw_buf[0] + 3, page) == 0x0000B001)
			{
				
				// extract the data out of the packet
				Array.Copy(raw_buf, 1, readBuf, offset, (byte) raw_buf[0]);
				
				// return the length
				return raw_buf[0];
			}
			else
				throw new OneWireIOException("Invalid CRC16 in packet read");
		}
		
		/// <summary> Write a Universal Data Packet.  See the method 'readPagePacket()'
		/// for a description of the packet structure.
		/// 
		/// </summary>
		/// <param name="page">         page number to write packet to
		/// </param>
		/// <param name="writeBuf">     data byte array to write
		/// </param>
		/// <param name="offset">       offset into writeBuf where data to write is
		/// </param>
		/// <param name="len">          number of bytes to write
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  writePagePacket(int page, byte[] writeBuf, int offset, int len)
		{
			
			// make sure length does not exceed max
			if (len > (PAGE_SIZE - 2))
				throw new OneWireIOException("Length of packet requested exceeds page size");
			
			// construct the packet to write
			byte[] raw_buf = new byte[len + 3];
			
			raw_buf[0] = (byte) len;
			
			Array.Copy(writeBuf, offset, raw_buf, 1, len);
			
			int crc = CRC16.compute(raw_buf, 0, len + 1, page);
			
			raw_buf[len + 1] = (byte) (~ crc & 0xFF);
			raw_buf[len + 2] = (byte) ((SupportClass.URShift((~ crc & 0xFFFF), 8)) & 0xFF);
			
			// write the packet, return result
			write(page * PAGE_SIZE, raw_buf, 0, len + 3);
		}
		
		/// <summary> Read a complete memory page with CRC verification provided by the
		/// device.  Not supported by all devices.  See the method
		/// 'hasPageAutoCRC()'.
		/// 
		/// </summary>
		/// <param name="page">         page number to read
		/// </param>
		/// <param name="readContinue"> if 'true' then device read is continued without
		/// re-selecting.  This can only be used if the new
		/// readPagePacket() continious where the last one
		/// stopped and it is inside a
		/// 'beginExclusive/endExclusive' block.
		/// </param>
		/// <param name="readBuf">      byte array to put data read. Must have at least
		/// 'getMaxPacketDataLength()' elements.
		/// </param>
		/// <param name="offset">       offset into readBuf to place data
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  readPageCRC(int page, bool readContinue, byte[] readBuf, int offset)
		{
			
			// only needs to be implemented if supported by hardware
			throw new OneWireException("Read page with CRC not supported by this memory bank");
		}
		
		/// <summary> Read a complete memory page with CRC verification provided by the
		/// device with extra information.  Not supported by all devices.
		/// See the method 'hasPageAutoCRC()'.
		/// See the method 'hasExtraInfo()' for a description of the optional
		/// extra information.
		/// 
		/// </summary>
		/// <param name="page">         page number to read
		/// </param>
		/// <param name="readContinue"> if 'true' then device read is continued without
		/// re-selecting.  This can only be used if the new
		/// readPagePacket() continious where the last one
		/// stopped and it is inside a
		/// 'beginExclusive/endExclusive' block.
		/// </param>
		/// <param name="readBuf">      byte array to put data read. Must have at least
		/// 'getMaxPacketDataLength()' elements.
		/// </param>
		/// <param name="offset">       offset into readBuf to place data
		/// </param>
		/// <param name="extraInfo">    byte array to put extra info read into
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  readPageCRC(int page, bool readContinue, byte[] readBuf, int offset, byte[] extraInfo)
		{
			
			// only needs to be implemented if supported by hardware
			throw new OneWireException("Read page with CRC not supported by this memory bank");
		}
		
		//--------
		//-------- OTPMemoryBank I/O methods
		//--------
		
		/// <summary> Lock the specifed page in the current memory bank.  Not supported
		/// by all devices.  See the method 'canLockPage()'.
		/// 
		/// </summary>
		/// <param name="page">  number of page to lock
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  lockPage(int page)
		{
			
			// attempt to put device at the correct speed
			ib.doSpeed();
			
			// select the device
			if (!ib.adapter.select(ib.address))
				throw new OneWireIOException("Device select failed");
			
			// do the copy/lock sequence
			ib.adapter.putByte(COPY_LOCK_COMMAND);
			ib.adapter.putByte(VALIDATION_KEY);
			
			// read back to verify
			if (!isPageLocked(page))
				throw new OneWireIOException("Read back from write incorrect, could not lock page");
		}
		
		/// <summary> Query to see if the specified page is locked.
		/// See the method 'canLockPage()'.
		/// 
		/// </summary>
		/// <param name="page"> number of page to see if locked
		/// 
		/// </param>
		/// <returns>  'true' if page locked.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual bool isPageLocked(int page)
		{
			
			// check if for valid page
			if (page != 0)
				throw new OneWireException("Invalid page number for this memory bank");
			
			// attempt to put device at the correct speed
			ib.doSpeed();
			
			// read status and return result
			return ((byte) readStatus() == (byte) LOCKED_FLAG);
		}
		
		/// <summary> Redirect the specifed page in the current memory bank to a new page.
		/// Not supported by all devices.  See the method 'canRedirectPage()'.
		/// 
		/// </summary>
		/// <param name="page">     number of page to redirect
		/// </param>
		/// <param name="newPage">  new page number to redirect to
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  redirectPage(int page, int newPage)
		{
			
			// only needs to be implemented if supported by hardware
			throw new OneWireException("Page redirection not supported by this memory bank");
		}
		
		/// <summary> Query to see if the specified page is redirected.
		/// Not supported by all devices.  See the method 'canRedirectPage()'.
		/// 
		/// </summary>
		/// <param name="page">     number of page check for redirection
		/// 
		/// </param>
		/// <returns>  return the new page number or 0 if not redirected
		/// 
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		/// <summary> 
		/// </summary>
		/// <deprecated>  As of 1-Wire API 0.01, replaced by {@link #getRedirectedPage(int)}
		/// </deprecated>
		public virtual int isPageRedirected(int page)
		{
			return 0;
		}
		
		/// <summary> Gets the page redirection of the specified page.
		/// Not supported by all devices. 
		/// 
		/// </summary>
		/// <param name="page"> page to check for redirection
		/// 
		/// </param>
		/// <returns>  the new page number or 0 if not redirected
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as  </throws>
		/// <summary>         no device present or a CRC read from the device is incorrect.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to 
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire  </throws>
		/// <summary>         adapter.  
		/// 
		/// </summary>
		/// <seealso cref="canRedirectPage() canRedirectPage">
		/// </seealso>
		/// <seealso cref="redirectPage(int,int) redirectPage">
		/// </seealso>
		/// <since> 1-Wire API 0.01
		/// </since>
		public virtual int getRedirectedPage(int page)
		{
			return 0;
		}
		
		/// <summary> Lock the redirection option for the specifed page in the current
		/// memory bank. Not supported by all devices.  See the method
		/// 'canLockRedirectPage()'.
		/// 
		/// </summary>
		/// <param name="page">     number of page to redirect
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  lockRedirectPage(int page)
		{
			
			// only needs to be implemented if supported by hardware
			throw new OneWireException("Lock Page redirection not supported by this memory bank");
		}
		
		/// <summary> Query to see if the specified page has redirection locked.
		/// Not supported by all devices.  See the method 'canRedirectPage()'.
		/// 
		/// </summary>
		/// <param name="page">     number of page check for locked redirection
		/// 
		/// </param>
		/// <returns>  return 'true' if redirection is locked for this page
		/// 
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual bool isRedirectPageLocked(int page)
		{
			return false;
		}
		
		//--------
		//-------- Bank specific methods
		//--------
		
		/// <summary> Read the status register for this memory bank.
		/// 
		/// </summary>
		/// <param name="readBuf">  byte array to put data read. Must have at least
		/// 'getExtraInfoLength()' elements.
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		protected internal virtual void  readStatus(byte[] readBuf)
		{
			readBuf[0] = (byte) readStatus();
		}
		
		/// <summary> Read the status register for this memory bank.
		/// 
		/// </summary>
		/// <returns>  the status register byte
		/// 
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		protected internal virtual byte readStatus()
		{
			
			// select the device
			if (!ib.adapter.select(ib.address))
				throw new OneWireIOException("Device select failed");
			
			// do the read status sequence
			ib.adapter.putByte(READ_STATUS_COMMAND);
			
			// validation key
			ib.adapter.putByte(0);
			
			return (byte) ib.adapter.Byte;
		}
	}
}