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
	
	
	/// <summary> Memory bank class for the NVRAM with build in CRC section of iButtons
	/// and 1-Wire devices.
	/// 
	/// </summary>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS
	/// </author>
	//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
	public class MemoryBankNVCRC:MemoryBankNV
	{
		
		//--------
		//-------- Static Final Variables
		//--------
		
		/// <summary> Write Scratchpad Command</summary>
		public static byte READ_PAGE_WITH_CRC = (byte) SupportClass.Identity(0xA5);
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Flag to indicate if read-continue is possible</summary>
		protected internal bool readContinuePossible;
		
		/// <summary> Number of extra verify byte in the read CRC page</summary>
		protected internal int numVerifyBytes;
		
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Memory bank contstuctor.  Requires reference to the OneWireContainer
		/// this memory bank resides on.
		/// </summary>
		public MemoryBankNVCRC(OneWireContainer ibutton, ScratchPad scratch):base(ibutton, scratch)
		{
			
			// initialize attributes of this memory bank
			pageAutoCRC = true;
			readContinuePossible = true;
			numVerifyBytes = 0;
		}
		
		//--------
		//-------- I/O methods
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
		public override void  readPage(int page, bool readContinue, byte[] readBuf, int offset)
		{
			
			// all other pages
			readPageCRC(page, readContinue, readBuf, offset, null, extraInfoLength);
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
		public override void  readPage(int page, bool readContinue, byte[] readBuf, int offset, byte[] extraInfo)
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
		public override int readPagePacket(int page, bool readContinue, byte[] readBuf, int offset, byte[] extraInfo)
		{
			byte[] raw_buf = new byte[pageLength];
			
			// read entire page with read page CRC
			readPageCRC(page, readContinue, raw_buf, 0, extraInfo, extraInfoLength);
			
			// check if length is realistic
			if (raw_buf[0] > maxPacketDataLength)
			{
				sp.forceVerify();
				
				throw new OneWireIOException("Invalid length in packet");
			}
			
			// verify the CRC is correct
			int abs_page = (startPhysicalAddress / pageLength) + page;
			if (CRC16.compute(raw_buf, 0, raw_buf[0] + 3, abs_page) == 0x0000B001)
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
		public override void  readPageCRC(int page, bool readContinue, byte[] readBuf, int offset, byte[] extraInfo)
		{
			readPageCRC(page, readContinue, readBuf, offset, extraInfo, extraInfoLength);
		}
		
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
			int last_crc = 0;
			byte[] raw_buf;
			
			// only needs to be implemented if supported by hardware
			if (!pageAutoCRC)
				throw new OneWireException("Read page with CRC not supported in this memory bank");
			
			// attempt to put device at max desired speed
			if (!readContinue)
				sp.checkSpeed();
			
			// check if read exceeds memory
			if (page > numberPages)
				throw new OneWireException("Read exceeds memory bank end");
			
			// see if need to access the device
			if (!readContinue || !readContinuePossible)
			{
				
				// select the device
				if (!ib.adapter.select(ib.address))
				{
					sp.forceVerify();
					
					throw new OneWireIOException("Device select failed");
				}
				
				// build start reading memory block
				raw_buf = new byte[3];
				raw_buf[0] = READ_PAGE_WITH_CRC;
				
				int addr = page * pageLength + startPhysicalAddress;
				
				raw_buf[1] = (byte) (addr & 0xFF);
				raw_buf[2] = (byte) ((SupportClass.URShift((addr & 0xFFFF), 8)) & 0xFF);
				
				// perform CRC16 on first part
				last_crc = CRC16.compute(raw_buf, 0, raw_buf.Length, last_crc);
				
				// do the first block for command, TA1, TA2
				ib.adapter.dataBlock(raw_buf, 0, 3);
			}
			
			// pre-fill with 0xFF 
			raw_buf = new byte[pageLength + extraLength + 2 + numVerifyBytes];
			
			Array.Copy(ffBlock, 0, raw_buf, 0, raw_buf.Length);
			
			// send block to read data + extra info? + crc
			ib.adapter.dataBlock(raw_buf, 0, raw_buf.Length);
			
			// check the CRC
			if (CRC16.compute(raw_buf, 0, raw_buf.Length - numVerifyBytes, last_crc) != 0x0000B001)
			{
				sp.forceVerify();
				
				throw new OneWireIOException("Invalid CRC16 read from device");
			}
			
			// extract the page data
			Array.Copy(raw_buf, 0, readBuf, offset, pageLength);
			
			// optional extract the extra info
			if (extraInfo != null)
				Array.Copy(raw_buf, pageLength, extraInfo, 0, extraLength);
		}
	}
}