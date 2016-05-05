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
	//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
	public class MemoryBankScratchEx:MemoryBankScratch
	{
		
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Memory bank contstuctor.  Requires reference to the OneWireContainer
		/// this memory bank resides on.
		/// </summary>
		public MemoryBankScratchEx(OneWireContainer ibutton):base(ibutton)
		{
			
			// initialize attributes of this memory bank
			bankDescription = "Scratchpad Ex";
			
			// change copy scratchpad command
			COPY_SCRATCHPAD_COMMAND = (byte) 0x5A;
		}
		
		//--------
		//-------- ScratchPad methods
		//--------
		
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
		public override void  writeScratchpad(int startAddr, byte[] writeBuf, int offset, int len)
		{
			bool calcCRC = false;
			
			if (len > pageLength)
				throw new OneWireException("Write exceeds memory bank end");
			
			// select the device
			if (!ib.adapter.select(ib.address))
			{
				forceVerify();
				
				throw new OneWireIOException("Device select failed");
			}
			
			// build block to send
			byte[] raw_buf = new byte[pageLength + 5]; //[37];
			
			raw_buf[0] = WRITE_SCRATCHPAD_COMMAND;
			raw_buf[1] = (byte) (startAddr & 0xFF);
			raw_buf[2] = (byte) ((SupportClass.URShift((startAddr & 0xFFFF), 8)) & 0xFF);
			
			Array.Copy(writeBuf, offset, raw_buf, 3, len);
			
			// check if full page (can utilize CRC)
			if (((startAddr + len) % pageLength) == 0)
			{
				Array.Copy(ffBlock, 0, raw_buf, len + 3, 2);
				
				calcCRC = true;
			}
			
			// send block, return result
			ib.adapter.dataBlock(raw_buf, 0, len + 3 + ((calcCRC)?2:0));
			//System.out.println("WriteScratchpad: " + com.dalsemi.onewire.utils.Convert.toHexString(raw_buf));
			
			// check crc
			if (calcCRC)
			{
				if (CRC16.compute(raw_buf, 0, len + 5, 0) != 0x0000B001)
				{
					forceVerify();
					
					throw new OneWireIOException("Invalid CRC16 read from device");
				}
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
		public override void  copyScratchpad(int startAddr, int len)
		{
			
			// select the device
			if (!ib.adapter.select(ib.address))
			{
				forceVerify();
				
				throw new OneWireIOException("Device select failed");
			}
			
			// build block to send
			byte[] raw_buf = new byte[6];
			
			raw_buf[0] = COPY_SCRATCHPAD_COMMAND;
			raw_buf[1] = (byte) (startAddr & 0xFF);
			raw_buf[2] = (byte) ((SupportClass.URShift((startAddr & 0xFFFF), 8)) & 0xFF);
			raw_buf[3] = (byte) ((startAddr + len - 1) & 0x1F);
			
			Array.Copy(ffBlock, 0, raw_buf, 4, 2);
			
			// send block (check copy indication complete)
			ib.adapter.dataBlock(raw_buf, 0, raw_buf.Length);
			
			if (((byte) (raw_buf[raw_buf.Length - 1] & 0x0F0) != (byte) SupportClass.Identity(0xA0)) && ((byte) (raw_buf[raw_buf.Length - 1] & 0x0F0) != (byte) 0x50))
			{
				forceVerify();
				
				throw new OneWireIOException("Copy scratchpad complete not found");
			}
		}
	}
}