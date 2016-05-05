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
	class MemoryBankScratchCRC:MemoryBankScratchEx
	{
		
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Memory bank contstuctor.  Requires reference to the OneWireContainer
		/// this memory bank resides on.
		/// </summary>
		public MemoryBankScratchCRC(OneWireContainer ibutton):base(ibutton)
		{
			
			// initialize attributes of this memory bank - DEFAULT: DS1963L scratchapd
			bankDescription = "Scratchpad with CRC";
			pageAutoCRC = true;
			
			// default copy scratchpad command
			COPY_SCRATCHPAD_COMMAND = (byte) 0x55;
		}
		
		//--------
		//-------- PagedMemoryBank I/O methods
		//--------
		
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
		public override void  readPageCRC(int page, bool readContinue, byte[] readBuf, int offset)
		{
			byte[] extraInfo = new byte[extraInfoLength];
			
			readPageCRC(page, readContinue, readBuf, offset, extraInfo);
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
		public override void  readPageCRC(int page, bool readContinue, byte[] readBuf, int offset, byte[] extraInfo)
		{
			
			// only needs to be implemented if supported by hardware
			if (!pageAutoCRC)
				throw new OneWireException("Read page with CRC not supported in this memory bank");
			
			// attempt to put device at max desired speed
			if (!readContinue)
				checkSpeed();
			
			// check if read exceeds memory
			if (page > numberPages)
				throw new OneWireException("Read exceeds memory bank end");
			
			// read the scratchpad
			readScratchpad(readBuf, offset, pageLength, extraInfo);
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
		public override void  readScratchpad(byte[] readBuf, int offset, int len, byte[] extraInfo)
		{
			// select the device
			if (!ib.adapter.select(ib.address))
			{
				forceVerify();
				
				throw new OneWireIOException("Device select failed");
			}
			
			// build block
			byte[] raw_buf = new byte[extraInfoLength + pageLength + 3];
			
			raw_buf[0] = READ_SCRATCHPAD_COMMAND;
			
			Array.Copy(ffBlock, 0, raw_buf, 1, raw_buf.Length - 1);
			
			// send block, command + (extra) + page data + CRC
			ib.adapter.dataBlock(raw_buf, 0, raw_buf.Length);
			
			// get the starting offset to see when the crc will show up
			int addr = raw_buf[1];
			
			addr = (addr | ((raw_buf[2] << 8) & 0xFF00)) & 0xFFFF;
			
			int num_crc = 35 - (addr & 0x001F) + extraInfoLength;
			
			// check crc of entire block
			if (CRC16.compute(raw_buf, 0, num_crc, 0) != 0x0000B001)
			{
				forceVerify();
				
				throw new OneWireIOException("Invalid CRC16 read from device");
			}
			
			// optionally extract the extra info
			if (extraInfo != null)
				Array.Copy(raw_buf, 1, extraInfo, 0, extraInfoLength);
			
			// extract the page data
			Array.Copy(raw_buf, extraInfoLength + 1, readBuf, 0, pageLength);
		}
	}
}