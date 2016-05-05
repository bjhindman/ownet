/*---------------------------------------------------------------------------
* Copyright (C) 2006 Maxim Integrated Products, All Rights Reserved.
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
using com.dalsemi.onewire.adapter;
using com.dalsemi.onewire.utils;
namespace com.dalsemi.onewire.container
{
	
	/// <summary> Memory bank class for the DS28EA00.
	/// 
	/// </summary>
	/// <version>  	0.00, 15 October 2006
	/// </version>
	/// <author>  	
	/// </author>
	class MemoryBankScratchTemp : MemoryBank
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
		/// <summary> Reads the way power is supplied to the DS28EA00.
		/// 
		/// </summary>
		/// <returns> <code>true</code> for external power, <BR>
		/// <code>false</code> for parasite power
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer42</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		virtual public bool ExternalPowerSupplied
		{
			get
			{
				int intresult = 0;
				bool result = false;
				
				// select the device
				if (ib.adapter.select(ib.Address))
				{
					// send the "Read Power Supply" memory command
					ib.adapter.putByte(READ_POWER_SUPPLY_COMMAND);
					
					// read results
					intresult = ib.adapter.Byte;
				}
				else
				{
					
					// device must not have been present
					throw new OneWireIOException("Device not found on 1-Wire Network");
				}
				if (intresult != 0x00)
					result = true; // reads 0xFF for true and 0x00 for false
				
				return result;
			}
			
		}
		//--------
		//--------Static Final Variables
		//--------
		
		/// <summary> Read power supply command. This command is used to determine if external power is supplied.</summary>
		public static byte READ_POWER_SUPPLY_COMMAND = (byte) 0xB4;
		
		/// <summary> Read scratchpad command</summary>
		private static byte READ_SCRATCHPAD_COMMAND = (byte) 0xBE;
		
		/// <summary> Recall memory command</summary>
		private static byte RECALL_MEMORY_COMMAND = (byte) 0xB8;
		
		/// <summary> Copy scratchpad command</summary>
		private static byte COPY_SCRATCHPAD_COMMAND = (byte) 0x48;
		
		/// <summary> Write scratchpad command</summary>
		//private static byte WRITE_SCRATCHPAD_COMMAND = (byte) 0x4E; // not used
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
		//protected internal bool writeOnce; // never assigned to and will always have default value of false
		
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
		public MemoryBankScratchTemp(OneWireContainer ibutton)
		{
			// keep reference to ibutton where memory bank is
			ib = ibutton;
			
			// initialize attributes of this memory bank
			
			bankDescription = "Temperature";
			generalPurposeMemory = false;
			startPhysicalAddress = 0;
			size = 2;
			readWrite = false;
			readOnly = true;
			nonVolatile = false;
			powerDelivery = true;
			writeVerification = false; // not needed here but required for MemoryBank implementation
			
			// create the ffblock (used for faster 0xFF fills)
			ffBlock = new byte[15];
			
			for (int i = 0; i < 15; i++)
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
		/// <param name="startAddr">    starting physical address
		/// </param>
		/// <param name="readContinue"> ignored by method
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
			
			// translate the address into a page_offset and offset
			int page_offset = startAddr + startPhysicalAddress;
			int data_len = 8 - page_offset;
			if (data_len > len)
				data_len = len;
			
			// read scratchpad
			temp_buf = readScratchpad();
			
			// copy contents to the readBuf
			Array.Copy(temp_buf, page_offset, readBuf, offset, data_len);
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
		/// <param name="offset">       offset into writeBuf to start writing data
		/// </param>
		/// <param name="len">          length in bytes to write
		/// 
		/// </param>
		/// <throws>  OneWireIOException </throws>
		/// <throws>  OneWireException </throws>
		public virtual void  write(int startAddr, byte[] writeBuf, int offset, int len)
		{
			byte[] temp_buf;
			
			// startAddr = starting address of the memory bank
			// writeBuf = array of bytes or unknown size given to method
			// offset = offset into writeBuf to start writing from
			// len = how many bytes to write
			
			
			// return if nothing to do
			if (len == 0)
				return ;
			
			// check if power delivery is available
			if (!ib.adapter.canDeliverPower())
				throw new OneWireException("Power delivery required but not available");
			
			// attempt to put device at max desired speed
			checkSpeed();
			
			// since writing is a bit odd in that the startPhysicalAddress 
			// is off by 2 due to being able to read 9 bytes but only writing 3,
			// let's correct...
			
			
			// translate the address into a page offset
			int page_offset = startAddr + startPhysicalAddress;
			byte[] page_buf = new byte[3];
			
			// pre-fill buff with current page contents
			temp_buf = readRecallScratchpad();
			
			// we have temp_buf, so now write the data we want changed to it.
			Array.Copy(writeBuf, offset, temp_buf, page_offset, len);
			
			// since only 3 bytes can be written to scratchpad (TH/TL/Config) then 
			// prefill this array with the correct three bytes
			Array.Copy(temp_buf, 2, page_buf, 0, 3);
			
			// write the page
			writeScratchpad(page_buf);
		}
		
		//--------
		//-------- Bank specific methods
		//--------
		
		/// <summary> Reads the 8 byte scratchpad and returns the data in an array.
		/// 
		/// </summary>
		/// <param name="none">*
		/// </param>
		/// <returns>  eight byte array that make up the scratchpad
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		protected internal virtual byte[] readScratchpad()
		{
			byte[] send_block = new byte[10];
			byte[] result_block = new byte[8];
			int crc8; // this device uses a crc 8
			
			if (ib.adapter.select(ib.address))
			{
				/* recall memory to the scratchpad */
				//ib.adapter.putByte(RECALL_MEMORY_COMMAND);
				
				/* perform the read scratchpad */
				//ib.adapter.select(ib.address);
				
				/* read scratchpad command */
				send_block[0] = (byte) READ_SCRATCHPAD_COMMAND;
				
				/* now add the read bytes for data bytes and crc8 */
				for (int i = 1; i < 10; i++)
					send_block[i] = (byte) SupportClass.Identity(0xFF);
				
				/* send the block */
				ib.adapter.dataBlock(send_block, 0, send_block.Length);
				
				/*
				Now, send_block contains the 8-byte Scratchpad plus READ_SCRATCHPAD_COMMAND byte, and CRC8.
				So, convert the block to a 8-byte array representing Scratchpad (get rid of first byte and CRC8)
				*/
				
				// see if CRC8 is correct
				crc8 = CRC8.compute(send_block, 1, 9);
				if (crc8 != 0)
					throw new OneWireIOException("Bad CRC during page read " + crc8);
				
				// copy the data into the result
				Array.Copy(send_block, 1, result_block, 0, 8);
				
				return (result_block);
			}
			
			// device must not have been present
			throw new OneWireIOException("Device not found during scratchpad read");
		}
		
		/// <summary> Reads the 8 byte scratchpad with Esquared recalled and returns the data in an array.
		/// 
		/// </summary>
		/// <param name="none">*
		/// </param>
		/// <returns>  eight byte array that make up the scratchpad
		/// 
		/// </returns>
		/// <throws>  OneWireIOException Error reading data </throws>
		/// <throws>  OneWireException Could not find part </throws>
		protected internal virtual byte[] readRecallScratchpad()
		{
			byte[] recallData;
			if (ib.adapter.select(ib.address))
			{
				// recall memory to the scratchpad
				ib.adapter.putByte(RECALL_MEMORY_COMMAND);
				// read scratchpad
				recallData = readScratchpad();
				return (recallData);
			}
			
			// device must not have been present
			throw new OneWireIOException("Device not found during E-squared recall scratchpad read");
		}
		
		
		/// <summary> Writes to the Scratchpad of the DS28EA00 and similar devices.
		/// 
		/// </summary>
		/// <param name="data">data to be written to the scratchpad.  First
		/// byte of data must be the temperature High Trip Point, the
		/// second byte must be the temperature Low Trip Point, and
		/// the third must be the Resolution (configuration register).
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from this <code>OneWireContainer42</code>.
		/// This could be caused by a physical interruption in the 1-Wire
		/// Network due to shorts or a newly arriving 1-Wire device issuing a
		/// 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		/// <throws>  IllegalArgumentException when data is of invalid length </throws>
		public virtual void  writeScratchpad(byte[] data)
		{
			/*
			OneWireContainer42 ib42;
			
			ib42 = (OneWireContainer42) ib;
			ib42.writeScratchpad(data);
			*/
			
			// setup buffer to write to scratchpad
			byte[] writeBuffer = new byte[4];
			//byte[] writeBuffer = new byte [6];
			byte[] readBuffer;
			//boolean vddSupplied = false;
			
			/*
			writeBuffer [0] = 0x4E;
			writeBuffer [1] = 0x00;
			writeBuffer [2] = 0x00;
			writeBuffer [3] = data [0];
			writeBuffer [4] = data [1];
			writeBuffer [5] = data [2];*/
			writeBuffer[0] = (byte) (0x4E);
			writeBuffer[1] = data[0];
			writeBuffer[2] = data[1];
			writeBuffer[3] = data[2];
			
			// send command block to device
			if (ib.adapter.select(ib.AddressAsString))
			{
				ib.adapter.dataBlock(writeBuffer, 0, writeBuffer.Length);
			}
			else
			{
				
				// device must not have been present
				throw new OneWireIOException("Device not found on 1-Wire Network");
			}
			
			
			// double check by reading scratchpad without recallEsquared
			readBuffer = readScratchpad();
			
			if ((readBuffer[2] != data[0]) || (readBuffer[3] != data[1]) || (readBuffer[4] != data[2]))
			{
				
				// writing to scratchpad failed
				throw new OneWireIOException("Error writing to scratchpad " + data[0] + " " + readBuffer[2] + " " + data[1] + " " + readBuffer[3] + " " + data[2] + " " + readBuffer[4] + " ");
			}
			
			
			// second, let's copy the scratchpad.
			if (ib.adapter.select(ib.AddressAsString))
			{
				
				// apply the power delivery
				ib.adapter.PowerDuration = DSPortAdapter.DELIVERY_INFINITE;
				ib.adapter.startPowerDelivery(DSPortAdapter.CONDITION_AFTER_BYTE);
				
				// send the copy scratchpad command
				ib.adapter.putByte(COPY_SCRATCHPAD_COMMAND);
				
				// sleep for 10 milliseconds to allow copy to take place.
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 10));
				}
				catch (System.Threading.ThreadInterruptedException e)
				{
				}
				
				// Turn power back to normal.
				ib.adapter.setPowerNormal();
			}
			/*
			
			// now copy scratchpad
			// second, let's copy the scratchpad.
			if (ib.adapter.select(ib.getAddressAsString()))
			{
			// apply the power delivery
			vddSupplied = isExternalPowerSupplied();
			if (!vddSupplied)
			{
			ib.adapter.setPowerDuration(DSPortAdapter.DELIVERY_INFINITE);
			ib.adapter.startPowerDelivery(DSPortAdapter.CONDITION_AFTER_BYTE);
			}
			
			// send the convert temperature command
			ib.adapter.putByte(COPY_SCRATCHPAD_COMMAND);
			
			// sleep for 10 milliseconds to allow copy to take place.
			try
			{
			Thread.sleep(10);
			}
			catch (InterruptedException e){}
			;
			
			// Turn power back to normal.
			if (!vddSupplied)
			{
			ib.adapter.setPowerNormal();
			}
			
			}
			*/
			else
			{
				// device must not have been present
				throw new OneWireIOException("Device not found on 1-Wire Network");
			}
			return ;
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