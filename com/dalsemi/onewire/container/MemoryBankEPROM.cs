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
	class MemoryBankEPROM : OTPMemoryBank
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
				return generalPurposeMemory;
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
				return readWrite;
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
				return writeOnce;
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
				return readOnly;
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
				return nonVolatile;
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
				return startPhysicalAddress;
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
				return size;
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
				return numberPages;
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
				return pageLength;
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
				return maxPacketDataLength;
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
				return extraInfoLength;
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
		
		/// <summary> Read Memory Command</summary>
		public static byte READ_MEMORY_COMMAND = (byte) SupportClass.Identity(0xF0);
		
		/// <summary> Main memory read command</summary>
		public static byte MAIN_READ_PAGE_COMMAND = (byte) SupportClass.Identity(0xA5);
		
		/// <summary> Status memory read command</summary>
		public static byte STATUS_READ_PAGE_COMMAND = (byte) SupportClass.Identity(0xAA);
		
		/// <summary> Main memory write command</summary>
		public static byte MAIN_WRITE_COMMAND = (byte) 0x0F;
		
		/// <summary> Status memory write command</summary>
		public static byte STATUS_WRITE_COMMAND = (byte) 0x55;
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Reference to the OneWireContainer this bank resides on.</summary>
		protected internal OneWireContainer ib;
		
		/// <summary> Read page with CRC command</summary>
		protected internal byte READ_PAGE_WITH_CRC;
		
		/// <summary> Number of CRC bytes (1-2)</summary>
		protected internal int numCRCBytes;
		
		/// <summary> Get crc after sending command,address</summary>
		protected internal bool crcAfterAddress;
		
		/// <summary> Get crc during a normal read</summary>
		protected internal bool normalReadCRC;
		
		/// <summary> Program Memory Command</summary>
		protected internal byte WRITE_MEMORY_COMMAND;
		
		/// <summary> block of 0xFF's used for faster read pre-fill of 1-Wire blocks</summary>
		protected internal byte[] ffBlock;
		
		/// <summary> Flag to indicate that speed needs to be set</summary>
		protected internal bool doSetSpeed;
		
		//--------
		//-------- Protected Variables for MemoryBank implementation 
		//--------
		
		/// <summary> Size of memory bank in bytes</summary>
		protected internal int size;
		
		/// <summary> Memory bank descriptions</summary>
		protected internal System.String bankDescription;
		
		/// <summary> Memory bank usage flags</summary>
		protected internal bool generalPurposeMemory;
		
		/// <summary> Flag if memory bank is read/write</summary>
		protected internal bool readWrite;
		
		/// <summary> Flag if memory bank is write once (EPROM)</summary>
		protected internal bool writeOnce;
		
		/// <summary> Flag if memory bank is read only</summary>
		protected internal bool readOnly;
		
		/// <summary> Flag if memory bank is non volatile
		/// (will not erase when power removed)
		/// </summary>
		protected internal bool nonVolatile;
		
		/// <summary> Flag if memory bank needs program Pulse to write</summary>
		protected internal bool programPulse;
		
		/// <summary> Flag if memory bank needs power delivery to write</summary>
		protected internal bool powerDelivery;
		
		/// <summary> Starting physical address in memory bank.  Needed for different
		/// types of memory in the same logical memory bank.  This can be
		/// used to seperate them into two virtual memory banks.  Example:
		/// DS2406 status page has mixed EPROM and Volatile RAM.
		/// </summary>
		protected internal int startPhysicalAddress;
		
		/// <summary> Flag if read back verification is enabled in 'write()'.</summary>
		protected internal bool writeVerification;
		
		//--------
		//-------- Protected Variables for PagedMemoryBank implementation 
		//--------
		
		/// <summary> Number of pages in memory bank</summary>
		protected internal int numberPages;
		
		/// <summary>  page length in memory bank</summary>
		protected internal int pageLength;
		
		/// <summary> Max data length in page packet in memory bank</summary>
		protected internal int maxPacketDataLength;
		
		/// <summary> Flag if memory bank has page auto-CRC generation</summary>
		protected internal bool pageAutoCRC;
		
		/// <summary> Flag if reading a page in memory bank provides optional
		/// extra information (counter, tamper protection, SHA-1...)
		/// </summary>
		protected internal bool extraInfo;
		
		/// <summary> Length of extra information when reading a page in memory bank</summary>
		protected internal int extraInfoLength;
		
		/// <summary> Extra information descriptoin when reading page in memory bank</summary>
		protected internal System.String extraInfoDescription;
		
		//--------
		//-------- Protected Variables for OTPMemoryBank implementation 
		//--------
		
		/// <summary> Flag if memory bank can have pages redirected</summary>
		protected internal bool redirectPage_Renamed_Field;
		
		/// <summary> Flag if memory bank can have pages locked</summary>
		protected internal bool lockPage_Renamed_Field;
		
		/// <summary> Flag if memory bank can have pages locked from redirected</summary>
		protected internal bool lockRedirectPage_Renamed_Field;
		
		/// <summary> Memory bank to lock pages in 'this' memory bank</summary>
		protected internal PagedMemoryBank mbLock;
		
		/// <summary> Memory bank to redirect pages in 'this' memory bank</summary>
		protected internal PagedMemoryBank mbRedirect;
		
		/// <summary> Memory bank to lock redirect bytes in 'this' memory bank</summary>
		protected internal PagedMemoryBank mbLockRedirect;
		
		/// <summary> Byte offset into memory bank 'mbLock' to indicate where page 0 can be locked</summary>
		protected internal int lockOffset;
		
		/// <summary> Byte offset into memory bank 'mbRedirect' to indicate where page 0 can be redirected</summary>
		protected internal int redirectOffset;
		
		/// <summary> Byte offset into memory bank 'mbLockRedirect' to indicate where page 0 can have
		/// its redirection byte locked
		/// </summary>
		protected internal int lockRedirectOffset;
		
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Memory bank contstuctor.  Requires reference to the OneWireContainer
		/// this memory bank resides on.  Requires reference to memory banks used
		/// in OTP operations.
		/// </summary>
		public MemoryBankEPROM(OneWireContainer ibutton)
		{
			
			// keep reference to ibutton where memory bank is
			ib = ibutton;
			
			// get references to MemoryBanks used in OTP operations, assume no locking/redirection
			mbLock = null;
			mbRedirect = null;
			mbLockRedirect = null;
			lockOffset = 0;
			redirectOffset = 0;
			lockRedirectOffset = 0;
			
			// initialize attributes of this memory bank - DEFAULT: Main memory DS1985 w/o lock stuff
			generalPurposeMemory = true;
			bankDescription = "Main Memory";
			numberPages = 64;
			size = 2048;
			pageLength = 32;
			maxPacketDataLength = 29;
			readWrite = false;
			writeOnce = true;
			readOnly = false;
			nonVolatile = true;
			pageAutoCRC = true;
			redirectPage_Renamed_Field = false;
			lockPage_Renamed_Field = false;
			lockRedirectPage_Renamed_Field = false;
			programPulse = true;
			powerDelivery = false;
			extraInfo = true;
			extraInfoLength = 1;
			extraInfoDescription = "Inverted redirection page";
			writeVerification = true;
			startPhysicalAddress = 0;
			READ_PAGE_WITH_CRC = MAIN_READ_PAGE_COMMAND;
			WRITE_MEMORY_COMMAND = MAIN_WRITE_COMMAND;
			numCRCBytes = 2;
			crcAfterAddress = true;
			normalReadCRC = false;
			doSetSpeed = true;
			
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
			return programPulse;
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
			return powerDelivery;
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
			return pageAutoCRC;
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
			return extraInfo;
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
			return extraInfo;
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
			return redirectPage_Renamed_Field;
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
			return lockPage_Renamed_Field;
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
			return lockRedirectPage_Renamed_Field;
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
			int i;
			
			// check if read exceeds memory
			if ((startAddr + len) > (pageLength * numberPages))
				throw new OneWireException("Read exceeds memory bank end");
			
			// attempt to put device at max desired speed
			if (!readContinue)
				checkSpeed();
			
			// check if status memory
			if (READ_PAGE_WITH_CRC == STATUS_READ_PAGE_COMMAND)
			{
				
				// no regular read memory so must use readPageCRC
				int start_pg = startAddr / pageLength;
				int end_pg = ((startAddr + len) / pageLength) - 1;
				
				if (((startAddr + len) % pageLength) > 0)
					end_pg++;
				
				byte[] raw_buf = new byte[(end_pg - start_pg + 1) * pageLength];
				
				// loop to read the pages
				for (int pg = start_pg; pg <= end_pg; pg++)
					readPageCRC(pg, !(pg == start_pg), raw_buf, (pg - start_pg) * pageLength, null, 0);
				
				// extract out the data
				Array.Copy(raw_buf, (startAddr % pageLength), readBuf, offset, len);
			}
			// regular memory so use standard read memory command
			else
			{
				
				// see if need to access the device
				if (!readContinue)
				{
					
					// select the device
					if (!ib.adapter.select(ib.address))
					{
						forceVerify();
						
						throw new OneWireIOException("Device select failed");
					}
					
					// build start reading memory block
					byte[] raw_buf = new byte[4];
					
					raw_buf[0] = READ_MEMORY_COMMAND;
					raw_buf[1] = (byte) ((startAddr + startPhysicalAddress) & 0xFF);
					raw_buf[2] = (byte) ((SupportClass.URShift(((startAddr + startPhysicalAddress) & 0xFFFF), 8)) & 0xFF);
					raw_buf[3] = (byte) SupportClass.Identity(0xFF);
					
					// check if get a 1 byte crc in a normal read.
					int num_bytes = (normalReadCRC)?4:3;
					
					// do the first block for command, address
					ib.adapter.dataBlock(raw_buf, 0, num_bytes);
				}
				
				// pre-fill readBuf with 0xFF 
				int pgs = len / pageLength;
				int extra = len % pageLength;
				
				for (i = 0; i < pgs; i++)
					Array.Copy(ffBlock, 0, readBuf, offset + i * pageLength, pageLength);
				Array.Copy(ffBlock, 0, readBuf, offset + pgs * pageLength, extra);
				
				// send second block to read data, return result
				ib.adapter.dataBlock(readBuf, offset, len);
			}
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
			int i;
			byte result;
			
			// return if nothing to do
			if (len == 0)
				return ;
			
			// check if power delivery is available
			if (!ib.adapter.canProgram())
				throw new OneWireException("Program voltage required but not available");
			
			// check if trying to write read only bank
			if (ReadOnly)
				throw new OneWireException("Trying to write read-only memory bank");
			
			// check if write exceeds memory
			if ((startAddr + len) > (pageLength * numberPages))
				throw new OneWireException("Write exceeds memory bank end");
			
			// set the program pulse duration
			ib.adapter.ProgramPulseDuration = DSPortAdapter.DELIVERY_EPROM;
			
			// attempt to put device at max desired speed
			checkSpeed();
			
			// loop while still have bytes to write
			bool write_continue = false;
			
			for (i = 0; i < len; i++)
			{
				result = programByte(startAddr + i + startPhysicalAddress, writeBuf[offset + i], write_continue);
				
				if (writeVerification)
				{
					if ((byte) result == (byte) writeBuf[offset + i])
						write_continue = true;
					else
					{
						forceVerify();
						
						throw new OneWireIOException("Read back byte on EPROM programming did not match");
					}
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
			if (pageAutoCRC)
				readPageCRC(page, readContinue, readBuf, offset, null, extraInfoLength);
			else
				read(page * pageLength, readContinue, readBuf, offset, pageLength);
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
			
			// check if current bank is not scratchpad bank, or not page 0
			if (!this.extraInfo)
				throw new OneWireException("Read extra information not supported on this memory bank");
			
			readPageCRC(page, readContinue, readBuf, offset, extraInfo, extraInfoLength);
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
			byte[] raw_buf = new byte[pageLength];
			
			// check if current bank is not scratchpad bank, or not page 0
			if (!this.extraInfo)
				throw new OneWireException("Read extra information not supported on this memory bank");
			
			// read entire page with read page CRC
			readPageCRC(page, readContinue, raw_buf, 0, extraInfo, extraInfoLength);
			
			// check if length is realistic
			if ((raw_buf[0] & 0x00FF) > maxPacketDataLength)
			{
				forceVerify();
				
				throw new OneWireIOException("Invalid length in packet");
			}
			
			// verify the CRC is correct
			if (CRC16.compute(raw_buf, 0, raw_buf[0] + 3, page) == 0x0000B001)
			{
				
				// extract the data out of the packet
				Array.Copy(raw_buf, 1, readBuf, offset, (byte) raw_buf[0]);
				
				// return the length
				return raw_buf[0];
			}
			else
			{
				forceVerify();
				
				throw new OneWireIOException("Invalid CRC16 in packet read");
			}
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
			byte[] raw_buf = new byte[pageLength];
			
			// read entire page with read page CRC
			readPageCRC(page, readContinue, raw_buf, 0, null, extraInfoLength);
			
			// check if length is realistic
			if ((raw_buf[0] & 0x00FF) > maxPacketDataLength)
			{
				forceVerify();
				
				throw new OneWireIOException("Invalid length in packet");
			}
			
			// verify the CRC is correct
			if (CRC16.compute(raw_buf, 0, raw_buf[0] + 3, page) == 0x0000B001)
			{
				
				// extract the data out of the packet
				Array.Copy(raw_buf, 1, readBuf, offset, (byte) raw_buf[0]);
				
				// return the length
				return raw_buf[0];
			}
			else
			{
				forceVerify();
				
				throw new OneWireIOException("Invalid CRC16 in packet read");
			}
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
			if (len > maxPacketDataLength)
				throw new OneWireIOException("Length of packet requested exceeds page size");
			
			// see if this bank is general read/write
			if (!generalPurposeMemory)
				throw new OneWireException("Current bank is not general purpose memory");
			
			// construct the packet to write
			byte[] raw_buf = new byte[len + 3];
			
			raw_buf[0] = (byte) len;
			
			Array.Copy(writeBuf, offset, raw_buf, 1, len);
			
			int crc = CRC16.compute(raw_buf, 0, len + 1, page);
			
			raw_buf[len + 1] = (byte) (~ crc & 0xFF);
			raw_buf[len + 2] = (byte) ((SupportClass.URShift((~ crc & 0xFFFF), 8)) & 0xFF);
			
			// write the packet, return result
			write(page * pageLength, raw_buf, 0, len + 3);
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
			readPageCRC(page, readContinue, readBuf, offset, null, extraInfoLength);
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
			readPageCRC(page, readContinue, readBuf, offset, extraInfo, extraInfoLength);
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
			
			// create byte to write to mlLock to lock page
			int nbyt = (SupportClass.URShift(page, 3));
			int nbit = page - (nbyt << 3);
			byte[] wr_byte = new byte[1];
			
			wr_byte[0] = (byte) ~ (0x01 << nbit);
			
			// bit field so turn off write verification
			mbLock.WriteVerification = false;
			
			// write the lock bit
			mbLock.write(nbyt + lockOffset, wr_byte, 0, 1);
			
			// read back to verify
			if (!isPageLocked(page))
			{
				forceVerify();
				
				throw new OneWireIOException("Read back from write incorrect, could not lock page");
			}
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
			
			// read page that locked bit is on 
			int pg_len = mbLock.PageLength;
			int read_pg = (page + lockOffset) / (pg_len * 8);
			
			// read page with bit
			byte[] read_buf = new byte[pg_len];
			
			mbLock.readPageCRC(read_pg, false, read_buf, 0);
			
			// return boolean on locked bit
			int index = (page + lockOffset) - (read_pg * 8 * pg_len);
			int nbyt = (SupportClass.URShift(index, 3));
			int nbit = index - (nbyt << 3);
			
			return !(((SupportClass.URShift(read_buf[nbyt], nbit)) & 0x01) == 0x01);
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
			
			// create byte to redirect page
			byte[] wr_byte = new byte[1];
			
			wr_byte[0] = (byte) ~ newPage;
			
			// writing byte so turn on write verification
			mbRedirect.WriteVerification = true;
			
			// write the redirection byte
			mbRedirect.write(page + redirectOffset, wr_byte, 0, 1);
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
			
			// read page that redirect byte is on 
			int pg_len = mbRedirect.PageLength;
			int read_pg = (page + redirectOffset) / pg_len;
			
			// read page with byte
			byte[] read_buf = new byte[pg_len];
			
			mbRedirect.readPageCRC(read_pg, false, read_buf, 0);
			
			// return page
			return ~ read_buf[(page + redirectOffset) % pg_len] & 0x000000FF;
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
			
			// read page that redirect byte is on 
			int pg_len = mbRedirect.PageLength;
			int read_pg = (page + redirectOffset) / pg_len;
			
			// read page with byte
			byte[] read_buf = new byte[pg_len];
			
			mbRedirect.readPageCRC(read_pg, false, read_buf, 0);
			
			// return page
			return ~ read_buf[(page + redirectOffset) % pg_len] & 0x000000FF;
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
			
			// create byte to write to mlLock to lock page
			int nbyt = (SupportClass.URShift(page, 3));
			int nbit = page - (nbyt << 3);
			byte[] wr_byte = new byte[1];
			
			wr_byte[0] = (byte) ~ (0x01 << nbit);
			
			// bit field so turn off write verification
			mbLockRedirect.WriteVerification = false;
			
			// write the lock bit
			mbLockRedirect.write(nbyt + lockRedirectOffset, wr_byte, 0, 1);
			
			// read back to verify
			if (!isRedirectPageLocked(page))
			{
				forceVerify();
				
				throw new OneWireIOException("Read back from write incorrect, could not lock redirect byte");
			}
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
			
			// read page that lock redirect bit is on 
			int pg_len = mbLockRedirect.PageLength;
			int read_pg = (page + lockRedirectOffset) / (pg_len * 8);
			
			// read page with bit
			byte[] read_buf = new byte[pg_len];
			
			mbLockRedirect.readPageCRC(read_pg, false, read_buf, 0);
			
			// return boolean on lock redirect bit
			int index = (page + lockRedirectOffset) - (read_pg * 8 * pg_len);
			int nbyt = (SupportClass.URShift(index, 3));
			int nbit = index - (nbyt << 3);
			
			return !(((SupportClass.URShift(read_buf[nbyt], nbit)) & 0x01) == 0x01);
		}
		
		//--------
		//-------- Bank specific methods
		//--------
		
		/// <summary> Read a complete memory page with CRC verification provided by the
		/// device with extra information.  Not supported by all devices.
		/// If not extra information available then just call with extraLength=0.
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
		/// </param>
		/// <param name="extraLength">  length of extra information
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		protected internal virtual void  readPageCRC(int page, bool readContinue, byte[] readBuf, int offset, byte[] extraInfo, int extraLength)
		{
			int len = 0, lastcrc = 0;
			byte[] raw_buf = new byte[pageLength + numCRCBytes];
			
			// only needs to be implemented if supported by hardware
			if (!pageAutoCRC)
				throw new OneWireException("Read page with CRC not supported in this memory bank");
			
			// attempt to put device at max desired speed
			if (!readContinue)
				checkSpeed();
			
			// check if read exceeds memory
			if (page > numberPages)
				throw new OneWireException("Read exceeds memory bank end");
			
			// see if need to access the device
			if (!readContinue)
			{
				
				// select the device
				if (!ib.adapter.select(ib.address))
				{
					forceVerify();
					
					throw new OneWireIOException("Device select failed");
				}
				
				// build start reading memory block with: command, address, (extraInfo?), (crc?)
				len = 3 + extraInfoLength;
				
				if (crcAfterAddress)
					len += numCRCBytes;
				
				Array.Copy(ffBlock, 0, raw_buf, 0, len);
				
				raw_buf[0] = READ_PAGE_WITH_CRC;
				
				int addr = page * pageLength + startPhysicalAddress;
				
				raw_buf[1] = (byte) (addr & 0xFF);
				raw_buf[2] = (byte) ((SupportClass.URShift((addr & 0xFFFF), 8)) & 0xFF);
				
				// do the first block 
				ib.adapter.dataBlock(raw_buf, 0, len);
			}
			else if (extraInfoLength > 0)
			{
				
				// build first block with: extraInfo, crc
				len = extraInfoLength + numCRCBytes;
				
				Array.Copy(ffBlock, 0, raw_buf, 0, len);
				
				// do the first block 
				ib.adapter.dataBlock(raw_buf, 0, len);
			}
			
			// check CRC
			if (numCRCBytes == 2)
				lastcrc = CRC16.compute(raw_buf, 0, len, 0);
			else
				lastcrc = CRC8.compute(raw_buf, 0, len, 0);
			
			if ((extraInfoLength > 0) || (crcAfterAddress))
			{
				
				// check CRC
				if (numCRCBytes == 2)
				{
					if (lastcrc != 0x0000B001)
					{
						forceVerify();
						
						throw new OneWireIOException("Invalid CRC16 read from device");
					}
				}
				else
				{
					if (lastcrc != 0)
					{
						forceVerify();
						
						throw new OneWireIOException("Invalid CRC8 read from device");
					}
				}
				
				lastcrc = 0;
				
				// extract the extra information
				if ((extraInfoLength > 0) && (extraInfo != null))
					Array.Copy(raw_buf, len - extraInfoLength - numCRCBytes, extraInfo, 0, extraLength);
			}
			
			// pre-fill with 0xFF 
			Array.Copy(ffBlock, 0, raw_buf, 0, raw_buf.Length);
			
			// send block to read data + crc
			ib.adapter.dataBlock(raw_buf, 0, raw_buf.Length);
			
			// check the CRC
			if (numCRCBytes == 2)
			{
				if (CRC16.compute(raw_buf, 0, raw_buf.Length, lastcrc) != 0x0000B001)
				{
					forceVerify();
					
					throw new OneWireIOException("Invalid CRC16 read from device");
				}
			}
			else
			{
				if (CRC8.compute(raw_buf, 0, raw_buf.Length, lastcrc) != 0)
				{
					forceVerify();
					
					throw new OneWireIOException("Invalid CRC8 read from device");
				}
			}
			
			// extract the page data
			Array.Copy(raw_buf, 0, readBuf, offset, pageLength);
		}
		
		/// <summary> Program an EPROM byte at the specified address.
		/// 
		/// </summary>
		/// <param name="addr">         address
		/// </param>
		/// <param name="data">         data byte to program
		/// </param>
		/// <param name="writeContinue">if 'true' then device programming is continued without
		/// re-selecting.  This can only be used if the new
		/// programByte() continious where the last one
		/// stopped and it is inside a
		/// 'beginExclusive/endExclusive' block.
		/// 
		/// </param>
		/// <returns>  the echo byte after programming.  This should be the desired
		/// byte to program if the location was previously unprogrammed.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		protected internal virtual byte programByte(int addr, byte data, bool writeContinue)
		{
			int lastcrc = 0, len;
			
			if (!writeContinue)
			{
				
				// select the device
				if (!ib.adapter.select(ib.address))
				{
					forceVerify();
					
					throw new OneWireIOException("device not present");
				}
				
				// pre-fill with 0xFF 
				byte[] raw_buf = new byte[6];
				
				Array.Copy(ffBlock, 0, raw_buf, 0, raw_buf.Length);
				
				// contruct packet
				raw_buf[0] = (byte) WRITE_MEMORY_COMMAND;
				raw_buf[1] = (byte) (addr & 0xFF);
				raw_buf[2] = (byte) ((SupportClass.URShift((addr & 0xFFFF), 8)) & 0xFF);
				raw_buf[3] = (byte) data;
				
				if (numCRCBytes == 2)
				{
					lastcrc = CRC16.compute(raw_buf, 0, 4, 0);
					len = 6;
				}
				else
				{
					lastcrc = CRC8.compute(raw_buf, 0, 4, 0);
					len = 5;
				}
				
				// send block to read data + crc
				ib.adapter.dataBlock(raw_buf, 0, len);
				
				// check CRC
				if (numCRCBytes == 2)
				{
					if (CRC16.compute(raw_buf, 4, 2, lastcrc) != 0x0000B001)
					{
						forceVerify();
						
						throw new OneWireIOException("Invalid CRC16 read from device");
					}
				}
				else
				{
					if (CRC8.compute(raw_buf, 4, 1, lastcrc) != 0)
					{
						forceVerify();
						
						throw new OneWireIOException("Invalid CRC8 read from device");
					}
				}
			}
			else
			{
				
				// send the data
				ib.adapter.putByte(data);
				
				// check CRC from device
				if (numCRCBytes == 2)
				{
					lastcrc = CRC16.compute(data, addr);
					lastcrc = CRC16.compute(ib.adapter.Byte, lastcrc);
					
					if (CRC16.compute(ib.adapter.Byte, lastcrc) != 0x0000B001)
					{
						forceVerify();
						
						throw new OneWireIOException("Invalid CRC16 read from device");
					}
				}
				else
				{
					lastcrc = CRC8.compute(data, addr);
					
					if (CRC8.compute(ib.adapter.Byte, lastcrc) != 0)
					{
						forceVerify();
						
						throw new OneWireIOException("Invalid CRC8 read from device");
					}
				}
			}
			
			// send the pulse 
			ib.adapter.startProgramPulse(DSPortAdapter.CONDITION_NOW);
			
			// return the result
			return (byte) ib.adapter.Byte;
		}
		
		//--------
		//-------- checkSpeed methods
		//--------
		
		/// <summary> Check the device speed if has not been done before or if
		/// an error was detected.
		/// 
		/// </summary>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  checkSpeed()
		{
			lock (this)
			{
				
				// only check the speed 
				if (doSetSpeed)
				{
					
					// attempt to set the correct speed and verify device present
					ib.doSpeed();
					
					// no execptions so clear flag 
					doSetSpeed = false;
				}
			}
		}
		
		/// <summary> Set the flag to indicate the next 'checkSpeed()' will force
		/// a speed set and verify 'doSpeed()'.
		/// </summary>
		public virtual void  forceVerify()
		{
			lock (this)
			{
				doSetSpeed = true;
			}
		}
	}
}