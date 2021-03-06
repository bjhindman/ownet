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
using com.dalsemi.onewire.utils;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> Memory bank class for the Scratchpad section of NVRAM iButtons and
	/// 1-Wire devices.
	/// 
	/// </summary>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS
	/// </author>
	//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
	public class MemoryBankScratch : PagedMemoryBank, ScratchPad
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
		//--------Static Final Variables
		//--------
		
		/// <summary> Write Scratchpad Command</summary>
		public static byte WRITE_SCRATCHPAD_COMMAND = (byte) 0x0F;
		
		/// <summary> Read Scratchpad Command</summary>
		public static byte READ_SCRATCHPAD_COMMAND = (byte) SupportClass.Identity(0xAA);
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Reference to the OneWireContainer this bank resides on.</summary>
		protected internal OneWireContainer ib;
		
		/// <summary> block of 0xFF's used for faster read pre-fill of 1-Wire blocks</summary>
		protected internal byte[] ffBlock;
		
		/// <summary> Copy Scratchpad Command</summary>
		protected internal byte COPY_SCRATCHPAD_COMMAND;
		
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
		//-------- Constructor
		//--------
		
		/// <summary> Memory bank contstuctor.  Requires reference to the OneWireContainer
		/// this memory bank resides on.
		/// </summary>
		public MemoryBankScratch(OneWireContainer ibutton)
		{
			
			// keep reference to ibutton where memory bank is
			ib = ibutton;
			
			// initialize attributes of this memory bank - DEFAULT: DS199X scratchapd
			bankDescription = "Scratchpad";
			generalPurposeMemory = false;
			startPhysicalAddress = 0;
			size = 32;
			readWrite = true;
			writeOnce = false;
			readOnly = false;
			nonVolatile = false;
			programPulse = false;
			powerDelivery = false;
			writeVerification = true;
			numberPages = 1;
			pageLength = 32;
			maxPacketDataLength = 29;
			pageAutoCRC = false;
			extraInfo = true;
			extraInfoLength = 3;
			extraInfoDescription = "Target address, offset";
			
			// create the ffblock (used for faster 0xFF fills)
			ffBlock = new byte[96];
			
			for (int i = 0; i < 96; i++)
				ffBlock[i] = (byte) SupportClass.Identity(0xFF);
			
			// default copy scratchpad command
			COPY_SCRATCHPAD_COMMAND = (byte) 0x55;
			
			// indicate speed has not been set
			doSetSpeed = true;
		}
		
		//--------
		//-------- Memory Bank methods
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
		//-------- I/O methods
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
		/// <param name="startAddr">    starting address
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
			if ((startAddr + len) > size)
				throw new OneWireException("Read exceeds memory bank end");
			
			// attempt to put device at speed
			checkSpeed();
			
			// read the scratchpad, discard extra information
			readScratchpad(readBuf, offset, len, null);
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
			
			// attempt to put device at speed
			checkSpeed();
			
			// check if write exceeds memory
			if ((startAddr + len) > size)
				throw new OneWireException("Write exceeds memory bank end");
			
			// write the page of data to scratchpad
			writeScratchpad(startPhysicalAddress + startAddr, writeBuf, offset, len);
			
			// read to verify ok
			byte[] raw_buf = new byte[pageLength];
			byte[] extra_buf = new byte[extraInfoLength];
			
			readScratchpad(raw_buf, 0, pageLength, extra_buf);
			
			// check to see if the same
			for (int i = 0; i < len; i++)
				if (raw_buf[i] != writeBuf[i + offset])
				{
					forceVerify();
					
					throw new OneWireIOException("Read back verify had incorrect data");
				}
			
			// check to make sure that the address is correct
			if ((((extra_buf[0] & 0x00FF) | ((extra_buf[1] << 8) & 0x00FF00)) & 0x00FFFF) != (startPhysicalAddress + startAddr))
			{
				forceVerify();
				
				throw new OneWireIOException("Read back address had incorrect data");
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
			
			// check if read exceeds memory
			if (page != 0)
				throw new OneWireException("Page read exceeds memory bank end");
			
			// attempt to put device at speed
			checkSpeed();
			
			// read the scratchpad, discard extra information
			readScratchpad(readBuf, offset, pageLength, null);
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
			
			// check if read exceeds memory
			if (page != 0)
				throw new OneWireException("Page read exceeds memory bank end");
			
			// attempt to put device at speed
			checkSpeed();
			
			// read the scratchpad, discard extra information
			readScratchpad(readBuf, offset, pageLength, extraInfo);
		}
		
		/// <summary> Read a Universal Data Packet.
		/// 
		/// The Universal Data Packet always starts on page boundaries but
		/// can end anywhere in the page.  The structure specifies the length of
		/// data bytes not including the length byte and the CRC16 bytes.
		/// There is one length byte. The CRC16 is first initialized to
		/// the page number.  This provides a check to verify the page that
		/// was intended is being read.  The CRC16 is then calculated over
		/// the length and data bytes.  The CRC16 is then inverted and stored
		/// low byte first followed by the high byte.  This is structure is
		/// used by this method to verify the data but is not returned, only
		/// the data payload is returned.
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
		/// <returns>  number of data bytes read from the device and written to
		/// readBuf at the offset.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual int readPagePacket(int page, bool readContinue, byte[] readBuf, int offset)
		{
			byte[] raw_buf = new byte[pageLength];
			
			// attempt to put device at speed
			checkSpeed();
			
			// read the scratchpad, discard extra information
			readScratchpad(raw_buf, 0, pageLength, null);
			
			// check if length is realistic
			if (raw_buf[0] > maxPacketDataLength)
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
		/// </param>
		/// <param name="extraInfo">    byte array to put extra info read into
		/// 
		/// </param>
		/// <returns>  number of data bytes read from the device and written to
		/// readBuf at the offset.
		/// 
		/// </returns>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual int readPagePacket(int page, bool readContinue, byte[] readBuf, int offset, byte[] extraInfo)
		{
			byte[] raw_buf = new byte[pageLength];
			
			// attempt to put device at speed
			checkSpeed();
			
			// read the scratchpad, discard extra information
			readScratchpad(raw_buf, 0, pageLength, extraInfo);
			
			// check if length is realistic
			if (raw_buf[0] > maxPacketDataLength)
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
				throw new OneWireException("Length of packet requested exceeds page size");
			
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
			throw new OneWireException("Read page with CRC and extra-info not supported by this memory bank");
		}
		
		//--------
		//-------- ScratchPad methods
		//--------
		
		/// <summary> Read the scratchpad page of memory from a NVRAM device
		/// This method reads and returns the entire scratchpad after the byte
		/// offset regardless of the actual ending offset
		/// 
		/// </summary>
		/// <param name="readBuf">      byte array to place read data into
		/// length of array is always pageLength.
		/// </param>
		/// <param name="offset">       offset into readBuf to pug data
		/// </param>
		/// <param name="len">          length in bytes to read
		/// </param>
		/// <param name="extraInfo">    byte array to put extra info read into
		/// (TA1, TA2, e/s byte)
		/// length of array is always extraInfoLength.
		/// Can be 'null' if extra info is not needed.
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  readScratchpad(byte[] readBuf, int offset, int len, byte[] extraInfo)
		{
			// select the device
			if (!ib.adapter.select(ib.address))
			{
				forceVerify();
				
				throw new OneWireIOException("Device select failed");
			}
			
			// build first block
			byte[] raw_buf = new byte[1 + extraInfoLength];
			
			raw_buf[0] = READ_SCRATCHPAD_COMMAND;
			
			Array.Copy(ffBlock, 0, raw_buf, 1, extraInfoLength);
			
			// do the first block for TA1, TA2, and E/S
			ib.adapter.dataBlock(raw_buf, 0, 1 + extraInfoLength);
			
			// optionally extract the extra info
			if (extraInfo != null)
				Array.Copy(raw_buf, 1, extraInfo, 0, extraInfoLength);
			
			// build the next block
			Array.Copy(ffBlock, 0, readBuf, offset, len);
			
			// send second block to read data, return result
			ib.adapter.dataBlock(readBuf, offset, len);
		}
		
		/// <summary> Write to the scratchpad page of memory a NVRAM device.
		/// 
		/// </summary>
		/// <param name="startAddr">    starting address
		/// </param>
		/// <param name="writeBuf">     byte array containing data to write
		/// </param>
		/// <param name="offset">       offset into readBuf to place data
		/// </param>
		/// <param name="len">          length in bytes to write
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  writeScratchpad(int startAddr, byte[] writeBuf, int offset, int len)
		{
			
			// select the device
			if (!ib.adapter.select(ib.address))
			{
				forceVerify();
				
				throw new OneWireIOException("Device select failed");
			}
			
			// build block to send
			byte[] raw_buf = new byte[3 + len];
			
			raw_buf[0] = WRITE_SCRATCHPAD_COMMAND;
			raw_buf[1] = (byte) (startAddr & 0xFF);
			raw_buf[2] = (byte) ((SupportClass.URShift((startAddr & 0xFFFF), 8)) & 0xFF);
			
			Array.Copy(writeBuf, offset, raw_buf, 3, len);
			
			// send block, return result
			ib.adapter.dataBlock(raw_buf, 0, len + 3);
		}
		
		/// <summary> Copy the scratchpad page to memory.
		/// 
		/// </summary>
		/// <param name="startAddr">    starting address
		/// </param>
		/// <param name="len">          length in bytes that was written already
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  copyScratchpad(int startAddr, int len)
		{
			
			// select the device
			if (!ib.adapter.select(ib.address))
			{
				forceVerify();
				
				throw new OneWireIOException("Device select failed");
			}
			
			// build block to send
			byte[] raw_buf = new byte[5];
			
			raw_buf[0] = COPY_SCRATCHPAD_COMMAND;
			raw_buf[1] = (byte) (startAddr & 0xFF);
			raw_buf[2] = (byte) ((SupportClass.URShift((startAddr & 0xFFFF), 8)) & 0xFF);
			raw_buf[3] = (byte) ((startAddr + len - 1) & 0x1F);
			raw_buf[4] = (byte) SupportClass.Identity(0xFF);
			
			// send block (check copy indication complete)
			ib.adapter.dataBlock(raw_buf, 0, 5);
			
			if ((raw_buf[4] & 0x0F0) != 0)
			{
				forceVerify();
				
				throw new OneWireIOException("Copy scratchpad complete not found");
			}
		}
		
		/// <summary> Copy the scratchpad page to memory.
		/// 
		/// </summary>
		/// <param name="startAddr">    starting address
		/// </param>
		/// <param name="len">          length in bytes that was written already
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  copyScratchpad(int startAddr, int len, bool WriteProtect)
		{
			int i, j;
			
			if (WriteProtect)
				i = 2;
			else
				i = 0;
			
			for (j = 0; j < i; j++)
			{
				// select the device
				if (!ib.adapter.select(ib.address))
				{
					forceVerify();
					
					throw new OneWireIOException("Device select failed");
				}
				
				// build block to send
				byte[] raw_buf = new byte[5];
				
				raw_buf[0] = COPY_SCRATCHPAD_COMMAND;
				raw_buf[1] = (byte) (startAddr & 0xFF);
				raw_buf[2] = (byte) ((SupportClass.URShift((startAddr & 0xFFFF), 8)) & 0xFF);
				raw_buf[3] = (byte) ((startAddr + len - 1) & 0x1F);
				raw_buf[3] = (byte) ((byte)raw_buf[3] | 0x80); // !!! added (byte)
				raw_buf[4] = (byte) SupportClass.Identity(0xFF);
				
				// send block (check copy indication complete)
				ib.adapter.dataBlock(raw_buf, 0, 5);
				
				if ((raw_buf[4] & 0x0F0) != 0)
				{
					forceVerify();
					
					throw new OneWireIOException("Copy scratchpad complete not found");
				}
			}
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
				// 9-23-2003 shughes: added the check to see if the adapter is
				// currently at the ibutton's max speed.  If it isn't, we should
				// call the doSpeed() method, since the adapter might have
				// changed speeds.
				if (doSetSpeed || (ib.adapter.Speed != ib.MaxSpeed))
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