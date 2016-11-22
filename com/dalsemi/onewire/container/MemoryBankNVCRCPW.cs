/*---------------------------------------------------------------------------
* Copyright (C) 2002 Maxim Integrated Products, All Rights Reserved.
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
using DSPortAdapter = com.dalsemi.onewire.adapter.DSPortAdapter;
using OneWireIOException = com.dalsemi.onewire.adapter.OneWireIOException;
using CRC16 = com.dalsemi.onewire.utils.CRC16;
namespace com.dalsemi.onewire.container
{
	
	
	
	/// <summary> Memory bank class for the NVRAM with built-in CRC generation and Password
	/// protected memory read/write iButtons and 1-Wire Devices.  An example of
	/// such a devices is the DS1922 Thermochron with 8k or password protected
	/// log memory.
	/// 
	/// </summary>
	/// <version>     1.00, 11 Feb 2002
	/// </version>
	/// <author>      SH
	/// </author>
	public class MemoryBankNVCRCPW:MemoryBankNVCRC
	{
		/// <summary> Read Memory (with CRC and Password) Command</summary>
		public static byte READ_MEMORY_CRC_PW_COMMAND = (byte) 0x69;
		
		/// <summary> Scratchpad with Password.  Used as container for password.</summary>
		protected internal MemoryBankScratchCRCPW scratchpadPW = null;
		
		/// <summary> Password Container to access the passwords for the memory bank.</summary>
		protected internal PasswordContainer ibPass = null;
		
		/// <summary> Enable Provided Power for some Password checking.</summary>
		public bool enablePower = false;
		
		/// <summary> Memory bank contstuctor.  Requires reference to the OneWireContainer
		/// this memory bank resides on.
		/// </summary>
		public MemoryBankNVCRCPW(PasswordContainer ibutton, MemoryBankScratchCRCPW scratch):base((OneWireContainer) ibutton, scratch)
		{
			
			ibPass = ibutton;
			
			// initialize attributes of this memory bank
			pageAutoCRC = true;
			readContinuePossible = true;
			numVerifyBytes = 0;
			
			scratchpadPW = scratch;
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
		protected internal override void  readPageCRC(int page, bool readContinue, byte[] readBuf, int offset, byte[] extraInfo, int extraLength)
		{
			int last_crc = 0;
			byte[] raw_buf;
			byte temp;
			
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
				raw_buf = new byte[11];
				raw_buf[0] = READ_MEMORY_CRC_PW_COMMAND;
				
				int addr = page * pageLength + startPhysicalAddress;
				
				raw_buf[1] = (byte) (addr & 0xFF);
				raw_buf[2] = (byte) ((SupportClass.URShift((addr & 0xFFFF), 8)) & 0xFF);
				
				if (ibPass.ContainerReadWritePasswordSet)
					ibPass.getContainerReadWritePassword(raw_buf, 3);
				else
					ibPass.getContainerReadOnlyPassword(raw_buf, 3);
				
				// perform CRC16 on first part (without the password)
				last_crc = CRC16.compute(raw_buf, 0, 3, last_crc);
				
				// do the first block for command, TA1, TA2, and password
				
				if (enablePower)
				{
					ib.adapter.dataBlock(raw_buf, 0, 10);
					
					ib.adapter.startPowerDelivery(DSPortAdapter.CONDITION_AFTER_BYTE);
					
					ib.adapter.putByte(raw_buf[10]);
				}
				else
					ib.adapter.dataBlock(raw_buf, 0, 11);
			}
			else
			{
				if (enablePower)
				{
					ib.adapter.startPowerDelivery(DSPortAdapter.CONDITION_AFTER_BYTE);
					temp = (byte) ib.adapter.Byte;
				}
			}
			
			if (enablePower)
			{
				msWait(3);
				
				ib.adapter.setPowerNormal();
			}
			
			// pre-fill with 0xFF
			raw_buf = new byte[pageLength + extraLength + 2 + numVerifyBytes];
			
			Array.Copy(ffBlock, 0, raw_buf, 0, raw_buf.Length);
			
			// send block to read data + extra info? + crc
			if (enablePower)
			{
				ib.adapter.dataBlock(raw_buf, 0, (raw_buf.Length - 1));
				last_crc = CRC16.compute(raw_buf, 0, raw_buf.Length - numVerifyBytes - 2, last_crc);
				
				if ((last_crc & 0x0FF) != ((~ raw_buf[raw_buf.Length - 2]) & 0x0FF))
				{
					sp.forceVerify();
					throw new OneWireIOException("Invalid CRC16 read from device.  Password may be incorrect.");
				}
			}
			else
			{
				ib.adapter.dataBlock(raw_buf, 0, raw_buf.Length);
				
				// check the CRC
				if (CRC16.compute(raw_buf, 0, raw_buf.Length - numVerifyBytes, last_crc) != 0x0000B001)
				{
					sp.forceVerify();
					throw new OneWireIOException("Invalid CRC16 read from device.  Password may be incorrect.");
				}
			}
			
			// extract the page data
			Array.Copy(raw_buf, 0, readBuf, offset, pageLength);
			
			// optional extract the extra info
			if (extraInfo != null)
				Array.Copy(raw_buf, pageLength, extraInfo, 0, extraLength);
		}
		
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
		public override void  read(int startAddr, bool readContinue, byte[] readBuf, int offset, int len)
		{
			int i;
			byte[] raw_buf = new byte[64];
			
			// attempt to put device at max desired speed
			if (!readContinue)
			{
				sp.checkSpeed();
			}
			
			// check if read exceeds memory
			if ((startAddr + len) > size)
				throw new OneWireException("Read exceeds memory bank end");
			
			// see if need to access the device
			if (!readContinue)
			{
				
				// select the device
				if (!ib.adapter.select(ib.address))
				{
					sp.forceVerify();
					
					throw new OneWireIOException("Device select failed");
				}
				
				// build start reading memory block
				int addr = startAddr + startPhysicalAddress;
				
				raw_buf[0] = READ_MEMORY_CRC_PW_COMMAND;
				
				raw_buf[1] = (byte) (addr & 0xFF);
				raw_buf[2] = (byte) ((SupportClass.URShift((addr & 0xFFFF), 8)) & 0xFF);
				
				if (ibPass.ContainerReadWritePasswordSet)
					ibPass.getContainerReadWritePassword(raw_buf, 3);
				else
					ibPass.getContainerReadOnlyPassword(raw_buf, 3);
				
				// do the first block for command, address. password
				if (enablePower)
				{
					ib.adapter.dataBlock(raw_buf, 0, 10);
					ib.adapter.startPowerDelivery(DSPortAdapter.CONDITION_AFTER_BYTE);
					ib.adapter.putByte(raw_buf[10]);
					
					msWait(10);
					
					ib.adapter.setPowerNormal();
				}
				else
				{
					ib.adapter.dataBlock(raw_buf, 0, 11);
				}
			}
			
			// pre-fill readBuf with 0xFF
			int startOffset = startAddr % pageLength;
			
			// First, count how many bytes are leftover from the
			// whole page reads.
			int bytesLeftover = (len % pageLength);
			
			// Second, account for the whole number of pages
			int pgs = (len / pageLength);
			
			// Third, if read starts in middle, go ahead and count
			// that whole page as 1 more page.
			if (startOffset > 0)
			{
				pgs += 1;
				// subtract the bytes from this extra page from our
				// leftover bytes, since we just accounted for each
				// byte to the end of this first page.
				bytesLeftover -= (pageLength - startOffset);
			}
			
			// Finally, account for 1 more page read if there are
			// any more left over bytes
			if (bytesLeftover > 0)
				pgs += 1;
			
			int startPage = startAddr / pageLength;
			
			for (i = 0; i < pgs; i++)
			{
				readPageCRC(startPage + i, false, raw_buf, 0, null, 0);
				
				if (i == 0)
				{
					Array.Copy(raw_buf, startOffset, readBuf, offset, (pageLength - startOffset));
				}
				else if ((i == (pgs - 1)) && (bytesLeftover != 0))
				{
					Array.Copy(raw_buf, 0, readBuf, (offset + (pageLength - startOffset) + ((i - 1) * pageLength)), startOffset);
				}
				else
				{
					Array.Copy(raw_buf, 0, readBuf, (offset + (pageLength - startOffset) + ((i - 1) * pageLength)), pageLength);
				}
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
		public override void  write(int startAddr, byte[] writeBuf, int offset, int len)
		{
			// find the last (non-inclusive) address for this write
			int endingOffset = (startAddr + len);
			if (((endingOffset & 0x1F) > 0) && (!enablePower))
			{
				// find the number of bytes left until the end of the page
				int numBytes = pageLength - (endingOffset & 0x1F);
				if ((ibPass.hasReadWritePassword() && (0xFFE0 & endingOffset) == (0xFFE0 & ibPass.ReadWritePasswordAddress) && endingOffset < (ibPass.ReadWritePasswordAddress + ibPass.ReadWritePasswordLength)) || (ibPass.hasReadOnlyPassword() && (0xFFE0 & endingOffset) == (0xFFE0 & ibPass.ReadOnlyPasswordAddress) && endingOffset < (ibPass.ReadOnlyPasswordAddress + ibPass.ReadOnlyPasswordLength)) || (ibPass.hasWriteOnlyPassword() && (0xFFE0 & endingOffset) == (0xFFE0 & ibPass.WriteOnlyPasswordAddress) && endingOffset < (ibPass.WriteOnlyPasswordAddress + ibPass.WriteOnlyPasswordLength)))
				{
					
					// password block would be written to with potentially bad data
					throw new OneWireException("Executing write would overwrite password control registers with " + "potentially invalid data.  Please ensure write does not occur over" + "password control register page, or the password control data is " + "specified exactly in the write buffer.");
				}
				
				byte[] tempBuf = new byte[len + numBytes];
				Array.Copy(writeBuf, offset, tempBuf, 0, len);
				read(endingOffset, false, tempBuf, len, numBytes);
				
				base.write(startAddr, tempBuf, 0, tempBuf.Length);
			}
			else
			{
				// write does extend to end of page
				base.write(startAddr, writeBuf, offset, len);
			}
		}
		
		/// <summary> helper method to pause for specified milliseconds</summary>
		private static void  msWait(long ms)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
				System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * ms));
			}
			catch (System.Threading.ThreadInterruptedException ie)
			{
				;
			}
		}
	}
}