/*---------------------------------------------------------------------------
* Copyright (C) 2001 Maxim Integrated Products, All Rights Reserved.
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
	
	/// <summary> Memory bank class for the DS2438.
	/// 
	/// </summary>
	/// <version>     0.00, 30 Oct 2001
	/// </version>
	/// <author>      DS
	/// </author>
	class MemoryBankSBM : MemoryBank
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
		
		/// <summary> Read scratchpad command</summary>
		private static byte READ_SCRATCHPAD_COMMAND = (byte) SupportClass.Identity(0xBE);
		
		/// <summary> Recall memory command</summary>
		private static byte RECALL_MEMORY_COMMAND = (byte) SupportClass.Identity(0xB8);
		
		/// <summary> Copy scratchpad command</summary>
		private static byte COPY_SCRATCHPAD_COMMAND = (byte) 0x48;
		
		/// <summary> Write scratchpad command</summary>
		private static byte WRITE_SCRATCHPAD_COMMAND = (byte) 0x4E;
		
		//--------
		//-------- Protected Variables for MemoryBank implementation
		//--------
		
		/// <summary> Starting physical address in memory bank.  Needed for different
		/// types of memory in the same logical memory bank.  This can be
		/// used to seperate them into two virtual memory banks.  Example:
		/// DS2406 status page has mixed EPROM and Volatile RAM.
		/// </summary>
		protected internal int startPhysicalAddress;
		
		/// <summary> Size of memory bank in bytes</summary>
		protected internal int size;
		
		/// <summary> Memory bank descriptions</summary>
		protected internal System.String bankDescription;
		
		/// <summary> Memory bank usage flags</summary>
		protected internal bool generalPurposeMemory;
		
		/// <summary> Flag if memory bank is read/write</summary>
		protected internal bool readWrite;
		
		/// <summary> Flag if memory bank is write once (EPROM)</summary>
		//protected internal bool writeOnce; // never used and will alwasy be zero
		
		/// <summary> Flag if memory bank is read only</summary>
		protected internal bool readOnly;
		
		/// <summary> Flag if memory bank is non volatile
		/// (will not erase when power removed)
		/// </summary>
		protected internal bool nonVolatile;
		
		/// <summary> Flag if memory bank needs power delivery to write</summary>
		protected internal bool powerDelivery;
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Reference to the OneWireContainer this bank resides on.</summary>
		protected internal OneWireContainer ib;
		
		/// <summary> block of 0xFF's used for faster read pre-fill of 1-Wire blocks</summary>
		protected internal byte[] ffBlock;
		
		/// <summary> Flag if read back verification is enabled in 'write()'.</summary>
		protected internal bool writeVerification;
		
		/// <summary> Flag to indicate that speed needs to be set</summary>
		protected internal bool doSetSpeed;
		
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Memory bank contstuctor.  Requires reference to the OneWireContainer
		/// this memory bank resides on.
		/// </summary>
		public MemoryBankSBM(OneWireContainer ibutton)
		{
			// keep reference to ibutton where memory bank is
			ib = ibutton;
			
			// initialize attributes of this memory bank - DEFAULT: DS2438 Status byte
			bankDescription = "Status/Configuration";
			generalPurposeMemory = false;
			startPhysicalAddress = 0;
			size = 1;
			readWrite = true;
			readOnly = false;
			nonVolatile = true;
			powerDelivery = true;
			writeVerification = true;
			
			// create the ffblock (used for faster 0xFF fills)
			ffBlock = new byte[20];
			
			for (int i = 0; i < 20; i++)
				ffBlock[i] = (byte) SupportClass.Identity(0xFF);
			
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
			return powerDelivery;
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
			byte[] temp_buf;
			
			// check for valid address
			if ((startAddr < 0) || ((startAddr + len) > size))
				throw new OneWireException("Read exceeds memory bank");
			
			// check for zero length read (saves time)
			if (len == 0)
				return ;
			
			// attempt to put device at max desired speed
			checkSpeed();
			
			// translate the address into a page and offset
			int page = (startAddr + startPhysicalAddress) / 8;
			int page_offset = (startAddr + startPhysicalAddress) % 8;
			int data_len = 8 - page_offset;
			if (data_len > len)
				data_len = len;
			int page_cnt = 0;
			int data_read = 0;
			
			// loop while reading pages
			while (data_read < len)
			{
				// read a page
				temp_buf = readRawPage(page + page_cnt);
				
				// copy contents to the readBuf
				Array.Copy(temp_buf, page_offset, readBuf, offset + data_read, data_len);
				
				// increment counters
				page_cnt++;
				data_read += data_len;
				page_offset = 0;
				data_len = (len - data_read);
				if (data_len > 8)
					data_len = 8;
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
			byte[] temp_buf;
			
			// return if nothing to do
			if (len == 0)
				return ;
			
			// check if power delivery is available
			if (!ib.adapter.canDeliverPower())
				throw new OneWireException("Power delivery required but not available");
			
			// attempt to put device at max desired speed
			checkSpeed();
			
			// translate the address into a page and offset
			int page = (startAddr + startPhysicalAddress) / 8;
			int page_offset = (startAddr + startPhysicalAddress) % 8;
			int data_len = 8 - page_offset;
			if (data_len > len)
				data_len = len;
			int page_cnt = 0;
			int data_written = 0;
			byte[] page_buf = new byte[8];
			
			// loop while writing pages
			while (data_written < len)
			{
				// check if only writing part of page
				// pre-fill write page buff with current page contents
				if ((page_offset != 0) || (data_len != 8))
				{
					temp_buf = readRawPage(page + page_cnt);
					Array.Copy(temp_buf, 0, page_buf, 0, 8);
				}
				
				// fill in the data to write
				Array.Copy(writeBuf, offset + data_written, page_buf, page_offset, data_len);
				
				// write the page
				writeRawPage(page + page_cnt, page_buf, 0);
				
				// increment counters
				page_cnt++;
				data_written += data_len;
				page_offset = 0;
				data_len = (len - data_written);
				if (data_len > 8)
					data_len = 8;
			}
		}
		
		//--------
		//-------- Bank specific methods
		//--------
		
		
		/// <summary> Reads the specified 8 byte page and returns the data in an array.
		/// 
		/// </summary>
		/// <param name="page">the page number to read
		/// 
		/// </param>
		/// <returns>  eight byte array that make up the page
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		protected internal virtual byte[] readRawPage(int page)
		{
			byte[] buffer = new byte[11];
			byte[] result = new byte[8];
			int crc8; // this device uses a crc 8
			
			if (ib.adapter.select(ib.address))
			{
				/* recall memory to the scratchpad */
				buffer[0] = RECALL_MEMORY_COMMAND;
				buffer[1] = (byte) page;
				
				ib.adapter.dataBlock(buffer, 0, 2);
				
				/* perform the read scratchpad */
				ib.adapter.select(ib.address);
				
				buffer[0] = READ_SCRATCHPAD_COMMAND;
				buffer[1] = (byte) page;
				
				for (int i = 2; i < 11; i++)
					buffer[i] = (byte) SupportClass.Identity(0x0ff);
				
				ib.adapter.dataBlock(buffer, 0, 11);
				
				/* do the crc check */
				crc8 = CRC8.compute(buffer, 2, 9);
				
				if (crc8 != 0x0)
					throw new OneWireIOException("Bad CRC during page read " + crc8);
				
				// copy the data into the result
				Array.Copy(buffer, 2, result, 0, 8);
			}
			else
				throw new OneWireIOException("Device not found during read page.");
			
			return result;
		}
		
		/// <summary> Writes a page of memory to this device. Pages 3-6 are always
		/// available for user storage and page 7 is available if the CA bit is set
		/// to 0 (false) with <CODE>setFlag()</CODE>.
		/// 
		/// </summary>
		/// <param name="page">   the page number
		/// </param>
		/// <param name="source"> data to be written to page
		/// </param>
		/// <param name="offset"> offset with page to begin writting
		/// 
		/// </param>
		/// <returns>s 'true' if the write was successfull
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		/// <throws>  IllegalArgumentException Bad parameters passed </throws>
		protected internal virtual void  writeRawPage(int page, byte[] source, int offset)
		{
			byte[] buffer = new byte[12];
			int crc8;
			
			if (ib.adapter.select(ib.address))
			{
				// write the page to the scratchpad first
				buffer[0] = WRITE_SCRATCHPAD_COMMAND;
				buffer[1] = (byte) page;
				
				Array.Copy(source, offset, buffer, 2, 8);
				ib.adapter.dataBlock(buffer, 0, 10);
				
				// read back the scrathpad
				if (ib.adapter.select(ib.address))
				{
					// write the page to the scratchpad first
					buffer[0] = READ_SCRATCHPAD_COMMAND;
					buffer[1] = (byte) page;
					
					Array.Copy(ffBlock, 0, buffer, 2, 9);
					ib.adapter.dataBlock(buffer, 0, 11);
					
					/* do the crc check */
					crc8 = CRC8.compute(buffer, 2, 9);
					
					if (crc8 != 0x0)
						throw new OneWireIOException("Bad CRC during scratchpad read " + crc8);
					
					// now copy that part of the scratchpad to memory
					if (ib.adapter.select(ib.address))
					{
						buffer[0] = COPY_SCRATCHPAD_COMMAND;
						buffer[1] = (byte) page;
						
						ib.adapter.dataBlock(buffer, 0, 2);
						
						// give it some time to write
						try
						{
							//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
							System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 12));
						}
						catch (System.Threading.ThreadInterruptedException e)
						{
						}
						
						// check the result
						if ((byte) ib.adapter.Byte != (byte) SupportClass.Identity(0xFF))
							throw new OneWireIOException("Copy scratchpad verification not found.");
						
						return ;
					}
				}
			}
			
			throw new OneWireIOException("Device not found during write page.");
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