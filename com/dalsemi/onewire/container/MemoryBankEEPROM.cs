/*---------------------------------------------------------------------------
* Copyright (C) 2004 Maxim Integrated Products, All Rights Reserved.
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
using com.dalsemi.onewire.debug;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> Memory bank class for the EEPROM section of iButtons and 1-Wire devices on the DS2431.
	/// 
	/// </summary>
	/// <version>     1.00, 20 February 2004
	/// </version>
	/// <author>      DS
	/// </author>
	class MemoryBankEEPROM : OTPMemoryBank
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
		
		/// <summary> Page Lock Flag</summary>
		public static byte LOCKED_FLAG = (byte) 0x55;
		
		/// <summary> EPROM Mode Flag</summary>
		public static byte EPROM_MODE_FLAG = (byte) SupportClass.Identity(0xAA);
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> ScratchPad memory bank</summary>
		protected internal ScratchPad sp;
		
		/// <summary> Reference to the OneWireContainer this bank resides on.</summary>
		protected internal OneWireContainer ib;
		
		/// <summary> block of 0xFF's used for faster read pre-fill of 1-Wire blocks</summary>
		protected internal byte[] ffBlock = new byte[500];
		
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
		
		/// <summary> Length of scratch pad</summary>
		protected internal int scratchPadSize;
		
		//--------
		//-------- Protected Variables for OTPMemoryBank implementation
		//--------
		
		/// <summary> Flag if memory bank can have pages locked</summary>
		protected internal bool lockPage_Renamed_Field;
		
		/// <summary> Memory bank to lock pages in 'this' memory bank</summary>
		protected internal MemoryBankEEPROM mbLock;
		
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Memory bank contstuctor.  Requires reference to the OneWireContainer
		/// this memory bank resides on.  Requires reference to memory banks used
		/// in OTP operations.
		/// </summary>
		public MemoryBankEEPROM(OneWireContainer ibutton, ScratchPad scratch)
		{
			
			// keep reference to ibutton where memory bank is
			ib = ibutton;
			
			// keep reference to scratchPad bank
			sp = scratch;
			
			// get references to MemoryBanks used in OTP operations, assume locking
			mbLock = null;
			lockPage_Renamed_Field = true;
			
			// initialize attributes of this memory bank - DEFAULT: Main memory DS2431
			generalPurposeMemory = true;
			bankDescription = "Main memory";
			numberPages = 4;
			size = 128;
			pageLength = 32;
			maxPacketDataLength = 29;
			readWrite = true;
			writeOnce = false;
			readOnly = false;
			nonVolatile = true;
			pageAutoCRC = false;
			lockPage_Renamed_Field = true;
			programPulse = false;
			powerDelivery = true;
			extraInfo = false;
			extraInfoLength = 0;
			extraInfoDescription = null;
			writeVerification = false;
			startPhysicalAddress = 0;
			doSetSpeed = true;
			scratchPadSize = 8;
			
			// create the ffblock (used for faster 0xFF fills)
			for (int i = 0; i < 500; i++)
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
			return false;
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
			byte[] buff = new byte[150];
			
			Array.Copy(ffBlock, 0, buff, 0, 150);
			
			// check if read exceeds memory
			if ((startAddr + len) > (pageLength * numberPages))
				throw new OneWireException("Read exceeds memory bank end");
			
			if (len < 0)
				throw new OneWireException("Invalid length");
			
			
			// attempt to put device at max desired speed
			if (!readContinue)
			{
				sp.checkSpeed();
				
				// select the device
				if (ib.adapter.select(ib.address))
				{
					buff[0] = READ_MEMORY_COMMAND;
					
					// address 1
					buff[1] = (byte) ((startAddr + startPhysicalAddress) & 0xFF);
					// address 2
					buff[2] = (byte) ((SupportClass.URShift(((startAddr + startPhysicalAddress) & 0xFFFF), 8)) & 0xFF);
					
					ib.adapter.dataBlock(buff, 0, len + 3);
					
					// extract the data
					Array.Copy(buff, 3, readBuf, offset, len);
				}
				else
					throw new OneWireIOException("Device select failed");
			}
			else
			{
				ib.adapter.dataBlock(buff, 0, len);
				
				// extract the data
				Array.Copy(buff, 0, readBuf, offset, len);
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
			int room_left;
			
			// return if nothing to do
			if (len == 0)
				return ;
			
			// attempt to put device at speed
			sp.checkSpeed();
			
			// check if write exceeds memory
			if ((startAddr + len) > size)
				throw new OneWireException("Write exceeds memory bank end");
			
			// check if trying to write read only bank
			if (ReadOnly)
				throw new OneWireException("Trying to write read-only memory bank");
			
			// loop while still have pages to write
			int startx = 0, nextx = 0; // (start and next index into writeBuf)
			byte[] raw_buf = new byte[scratchPadSize];
			byte[] memory = new byte[size];
			//byte[] scratchpad = new byte[8];
			//byte[] es_data    = new byte[3];
			int abs_addr = startAddr + startPhysicalAddress;
			int pl = scratchPadSize;
			
			// check to see if we need to read memory for the beginning of the block
			if (scratchPadSize == 8)
			{
				if ((startAddr & 0x07) != 0)
					read((startAddr & 0x00F8), false, memory, (startAddr & 0x00F8), startAddr - (startAddr & 0x00F8) + 1);
			}
			else
			{
				if ((startAddr & 0x1F) != 0)
					read((startAddr & 0xFE0), false, memory, (startAddr & 0xFE0), startAddr - (startAddr & 0xFE0) + 1);
			}
			
			// check to see if we need to read memory for the end of the block
			if (scratchPadSize == 8)
			{
				if (((startAddr + len - 1) & 0x07) != 0x07)
					read((startAddr + len), false, memory, (startAddr + len), ((startAddr + len) | 0x07) - (startAddr + len) + 1);
			}
			else
			{
				if (((startAddr + len - 1) & 0x1F) != 0x1F)
					read((startAddr + len), false, memory, (startAddr + len), ((startAddr + len) | 0x1F) - (startAddr + len) + 1);
			}
			
			do 
			{
				// calculate room left in current page
				room_left = pl - ((abs_addr + startx) % pl);
				
				// check if block left will cross end of page
				if ((len - startx) > room_left)
					nextx = startx + room_left;
				else
					nextx = len;
				
				Array.Copy(memory, (((startx + startAddr) / scratchPadSize) * scratchPadSize), raw_buf, 0, scratchPadSize);
				
				if ((nextx - startx) == scratchPadSize)
				{
					Array.Copy(writeBuf, offset + startx, raw_buf, 0, scratchPadSize);
				}
				else
				{
					if (((startAddr + nextx) % scratchPadSize) == 0)
					{
						Array.Copy(writeBuf, offset + startx, raw_buf, ((startAddr + startx) % scratchPadSize), scratchPadSize - ((startAddr + startx) % scratchPadSize));
					}
					else
					{
						Array.Copy(writeBuf, offset + startx, raw_buf, ((startAddr + startx) % scratchPadSize), ((startAddr + nextx) % scratchPadSize) - ((startAddr + startx) % scratchPadSize));
					}
				}
				
				// write the page of data to scratchpad (always do full scratchpad)
				sp.writeScratchpad(abs_addr + startx + room_left - scratchPadSize, raw_buf, 0, scratchPadSize);
				
				// Copy data from scratchpad into memory
				sp.copyScratchpad(abs_addr + startx + room_left - scratchPadSize, scratchPadSize);
				
				if (startAddr >= pageLength)
					Array.Copy(raw_buf, 0, memory, (((startx + startAddr) / scratchPadSize) * scratchPadSize) - 32, scratchPadSize);
				else
					Array.Copy(raw_buf, 0, memory, (((startx + startAddr) / scratchPadSize) * scratchPadSize), scratchPadSize);
				
				// point to next index
				startx = nextx;
			}
			while (nextx < len);
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
			throw new OneWireException("Read extra information not supported on this memory bank");
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
			throw new OneWireException("Read extra information not supported on this memory bank");
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
			read((page * pageLength), readContinue, raw_buf, 0, pageLength);
			
			// check if length is realistic
			if ((raw_buf[0] & 0x00FF) > maxPacketDataLength)
			{
				sp.forceVerify();
				
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
				sp.forceVerify();
				
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
			throw new OneWireException("Read page with CRC not supported in this memory bank");
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
			throw new OneWireException("Read page with CRC not supported in this memory bank");
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
			byte[] wr_byte = new byte[1];
			
			wr_byte[0] = LOCKED_FLAG;
			
			mbLock.write(page, wr_byte, 0, 1);
			
			// read back to verify
			if (!isPageLocked(page))
			{
				sp.forceVerify();
				
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
			byte[] rd_byte = new byte[1];
			
			mbLock.read(page, false, rd_byte, 0, 1);
			
			return (rd_byte[0] == LOCKED_FLAG);
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
			throw new OneWireException("This memory bank does not support redirection.");
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
			throw new OneWireException("This memory bank does not support redirection.");
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
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         no device present or a CRC read from the device is incorrect.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
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
			throw new OneWireException("This memory bank does not support redirection.");
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
			throw new OneWireException("This memory bank does not support redirection.");
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
			throw new OneWireException("This memory bank does not support redirection.");
		}
	}
}