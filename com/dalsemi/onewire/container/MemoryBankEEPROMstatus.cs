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
	
	
	/// <summary> Memory bank class for the EEPROM section of iButtons and 1-Wire devices on the DS2408.
	/// 
	/// </summary>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS
	/// </author>
	class MemoryBankEEPROMstatus : MemoryBank
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
				return 0;
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
				return null;
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
		
		/// <summary> Write Scratchpad Command</summary>
		public static byte WRITE_SCRATCHPAD_COMMAND = (byte) 0x0F;
		
		/// <summary> Read Scratchpad Command</summary>
		public static byte READ_SCRATCHPAD_COMMAND = (byte) SupportClass.Identity(0xAA);
		
		/// <summary> Copy Scratchpad Command</summary>
		public static byte COPY_SCRATCHPAD_COMMAND = (byte) 0x55;
		
		/// <summary> Channel acces write to change the property of the channel</summary>
		public static byte CHANNEL_ACCESS_WRITE = (byte) 0x5A;
		
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Reference to the OneWireContainer this bank resides on.</summary>
		protected internal OneWireContainer ib;
		
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
        protected internal int pageLength = 0; // never assigned to and will always have default value of false
		
		/// <summary> Max data length in page packet in memory bank</summary>
        protected internal int maxPacketDataLength = 0; // never assigned to and will always have default value of false
		
		/// <summary> Flag if memory bank has page auto-CRC generation</summary>
		protected internal bool pageAutoCRC;
		
		
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Memory bank contstuctor.  Requires reference to the OneWireContainer
		/// this memory bank resides on.  Requires reference to memory banks used
		/// in OTP operations.
		/// </summary>
		public MemoryBankEEPROMstatus(OneWireContainer ibutton)
		{
			
			// keep reference to ibutton where memory bank is
			ib = ibutton;
			
			// initialize attributes of this memory bank - DEFAULT: Main memory DS1985 w/o lock stuff
			generalPurposeMemory = false;
			bankDescription = "Main Memory";
			numberPages = 1;
			readWrite = false;
			writeOnce = false;
			readOnly = false;
			nonVolatile = true;
			pageAutoCRC = true;
			programPulse = false;
			powerDelivery = false;
			writeVerification = false;
			startPhysicalAddress = 0;
			doSetSpeed = true;
			
			// create the ffblock (used for faster 0xFF fills)
			ffBlock = new byte[20];
			
			for (int i = 0; i < 20; i++)
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
			byte[] buff = new byte[20];
			int addr = startPhysicalAddress + startAddr;
			
			Array.Copy(ffBlock, 0, buff, 0, 20);
			
			// check if read exceeds memory
			if ((startAddr + len) > size)
				throw new OneWireException("Read exceeds memory bank end");
			
			// attempt to put device at max desired speed
			// attempt to put device at max desired speed
			if (!readContinue)
			{
				checkSpeed();
				
				// select the device
				if (ib.adapter.select(ib.address))
				{
					buff[0] = READ_MEMORY_COMMAND;
					
					// address 1
					buff[1] = (byte) (addr & 0xFF);
					// address 2
					buff[2] = (byte) ((SupportClass.URShift((addr & 0xFFFF), 8)) & 0xFF);
					
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
			int i;
			byte[] es_data = new byte[3];
			byte[] scratchpad = new byte[8];
			
			// return if nothing to do
			if (len == 0)
				return ;
			
			// attempt to put device at speed
			checkSpeed();
			
			// check if write exceeds memory
			if ((startAddr + len) > size)
				throw new OneWireException("Write exceeds memory bank end");
			
			// check if trying to write read only bank
			if (ReadOnly && (((startPhysicalAddress + startAddr) != 137) && (len != 1)))
				throw new OneWireException("Trying to write read-only memory bank");
			
			if (((startPhysicalAddress + startAddr) == 137) && (len == 1))
			{
				ib.adapter.select(ib.address);
				
				byte[] buffer = new byte[5];
				
				buffer[0] = CHANNEL_ACCESS_WRITE;
				buffer[1] = writeBuf[offset];
				buffer[2] = (byte) ~ writeBuf[offset];
				Array.Copy(ffBlock, 0, buffer, 3, 2);
				
				ib.adapter.dataBlock(buffer, 0, 5);
				
				if (buffer[3] != (byte) SupportClass.Identity(0x00AA))
				{
					//throw new OneWireIOException("Failure to change DS2408 latch state: buffer=" + Convert.toHexString(buffer));
                    throw new OneWireIOException("Failure to change DS2408 latch state: buffer=" + com.dalsemi.onewire.utils.Convert.toHexString(buffer)); // !!!

				}
			}
			else if (((startPhysicalAddress + startAddr) > 138) && ((startPhysicalAddress + startAddr + len) < 143))
			{
				ib.adapter.select(ib.address);
				
				byte[] buffer = new byte[6];
				
				buffer[0] = (byte) SupportClass.Identity(0xCC);
				buffer[1] = (byte) ((startAddr + startPhysicalAddress) & 0xFF);
				buffer[2] = (byte) ((SupportClass.URShift(((startAddr + startPhysicalAddress) & 0xFFFF), 8)) & 0xFF);
				
				Array.Copy(writeBuf, offset, buffer, 3, len);
				
				ib.adapter.dataBlock(buffer, 0, len + 3);
			}
			else if (((startPhysicalAddress + startAddr) > 127) && ((startPhysicalAddress + startAddr + len) < 130))
			{
				byte[] buffer = new byte[8];
				int addr = 128;
				byte[] buff = new byte[11];
				
				Array.Copy(ffBlock, 0, buff, 0, 11);
				
				ib.adapter.select(ib.address);
				
				buff[0] = READ_MEMORY_COMMAND;
				
				// address 1
				buff[1] = (byte) (addr & 0xFF);
				// address 2
				buff[2] = (byte) ((SupportClass.URShift((addr & 0xFFFF), 8)) & 0xFF);
				
				ib.adapter.dataBlock(buff, 0, 11);
				
				// extract the data
				Array.Copy(buff, 3, buffer, 0, 8);
				
				Array.Copy(writeBuf, offset, buffer, 0, len);
				
				// write the page of data to scratchpad
				if (!writeScratchpad(startPhysicalAddress + startAddr, buffer, 0, 8))
					throw new OneWireIOException("Invalid CRC16 in write");
				
				if (!readScratchpad(scratchpad, 0, 8, es_data))
					throw new OneWireIOException("Read scratchpad was not successful.");
				
				if ((es_data[2] & 0x20) == 0x20)
				{
					throw new OneWireIOException("The write scratchpad command was not completed.");
				}
				else
				{
					for (i = 0; i < 8; i++)
						if (scratchpad[i] != buffer[i])
						{
							throw new OneWireIOException("The read back of the data in the scratch pad did " + "not match.");
						}
				}
				
				// Copy data from scratchpad into memory
				copyScratchpad(es_data);
			}
			else
				throw new OneWireIOException("Trying to write read-only memory.");
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
		
		/// <summary> Write to the Scratch Pad, which is a max of 8 bytes...  Note that if
		/// less than 8 bytes are written, the ending offset will still report
		/// that a full eight bytes are on the buffer.  This means that all 8 bytes
		/// of the data in the scratchpad will be copied, not just the bytes user
		/// wrote into it.
		/// 
		/// </summary>
		/// <param name="addr">         the address to write the data to
		/// </param>
		/// <param name="out_buf">      byte array to write into scratch pad
		/// </param>
		/// <param name="offset">       offset into out_buf to write the data
		/// </param>
		/// <param name="len">          length of the write data
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual bool writeScratchpad(int addr, byte[] out_buf, int offset, int len)
		{
			byte[] send_block = new byte[14];
			
			// protect send buffer
			// since the scratchpad is only eight bytes, there is no reason to write
			// more than eight bytes..  and we can optimize our send buffer's size.
			if (len > 8)
				len = 8;
			
			// access the device
			if (ib.adapter.select(ib.Address))
			{
				int cnt = 0;
				// set data block up
				// start by sending the write scratchpad command
				send_block[cnt++] = WRITE_SCRATCHPAD_COMMAND;
				// followed by the target address
				send_block[cnt++] = (byte) (addr & 0x00FF);
				send_block[cnt++] = (byte) ((SupportClass.URShift((addr & 0x00FFFF), 8)) & 0x00FF);
				
				// followed by the data to write to the scratchpad
				Array.Copy(out_buf, offset, send_block, 3, len);
				cnt += len;
				
				// followed by two bytes for reading CRC16 value
				send_block[cnt++] = (byte) SupportClass.Identity(0x00FF);
				send_block[cnt++] = (byte) SupportClass.Identity(0x00FF);
				
				// send the data
				ib.adapter.dataBlock(send_block, 0, cnt);
				
				// verify the CRC is correct
				//         if (CRC16.compute(send_block, 0, cnt) != 0x0000B001)
				//            throw new OneWireIOException("Invalid CRC16 in Writing Scratch Pad");
			}
			else
				throw new OneWireIOException("Device select failed.");
			
			return true;
		}
		
		/// <summary> Copy all 8 bytes of the Sratch Pad to a certain address in memory.
		/// 
		/// </summary>
		/// <param name="addr">the address to copy the data to
		/// </param>
		/// <param name="auth">byte[] containing write authorization
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'copyScratchpad'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual void  copyScratchpad(byte[] es_data)
		{
			lock (this)
			{
				byte[] send_block = new byte[4];
				
				// access the device
				if (ib.adapter.select(ib.Address))
				{
					// ending address with data status
					send_block[3] = es_data[2]; //ES;
					
					// address 2
					send_block[2] = es_data[1]; //TA2
					
					// address 1
					send_block[1] = es_data[0]; //TA1;
					
					// Copy command
					send_block[0] = COPY_SCRATCHPAD_COMMAND;
					
					// send copy scratchpad command
					ib.adapter.dataBlock(send_block, 0, 3);
					
					// provide strong pull-up for copy
					ib.adapter.PowerDuration = DSPortAdapter.DELIVERY_INFINITE;
					ib.adapter.startPowerDelivery(DSPortAdapter.CONDITION_AFTER_BYTE);
					ib.adapter.putByte(send_block[3]);
					
					// pause before checking result
					try
					{
						//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 12));
					}
					catch (System.Threading.ThreadInterruptedException e)
					{
					}
					
					ib.adapter.setPowerNormal();
					
					// get result
					byte test = (byte) ib.adapter.Byte;
					
					if (test == (byte) SupportClass.Identity(0x00FF))
					{
						throw new OneWireIOException("The scratchpad did not get copied to memory.");
					}
				}
				else
					throw new OneWireIOException("Device select failed.");
			}
		}
		
		/// <summary> Read from the Scratch Pad, which is a max of 8 bytes.
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
		/// Can be 'null' if extra info is not needed.
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual bool readScratchpad(byte[] readBuf, int offset, int len, byte[] es_data)
		{
			// select the device
			if (!ib.adapter.select(ib.address))
			{
				forceVerify();
				throw new OneWireIOException("Device select failed");
			}
			
			// build first block
			byte[] raw_buf = new byte[14];
			raw_buf[0] = READ_SCRATCHPAD_COMMAND;
			Array.Copy(ffBlock, 0, raw_buf, 1, 13);
			
			// do data block for TA1, TA2, and E/S
			// followed by 8 bytes of data and 2 bytes of crc
			ib.adapter.dataBlock(raw_buf, 0, 14);
			
			// verify CRC16 is correct
			if (CRC16.compute(raw_buf, 0, 14) == 0x0000B001)
			{
				// optionally extract the extra info
				if (es_data != null)
					Array.Copy(raw_buf, 1, es_data, 0, 3);
				
				Array.Copy(raw_buf, 4, readBuf, offset, len);
				// success
				return true;
			}
			else
				throw new OneWireException("Error due to CRC.");
		}
	}
}