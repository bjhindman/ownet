/*---------------------------------------------------------------------------
* Copyright (C) 1999,2004 Maxim Integrated Products, All Rights Reserved.
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
using System;
namespace com.dalsemi.onewire.utils
{
	
	/// <summary> Utilities to translate and verify the 1-Wire Network address.
	/// <p>
	/// Q: What is a 1-Wire Network Address?<p>
	/// A: A 1-Wire address is 64 bits consisting of an eight bit family code, forty eight
	/// bits of serialized data and an eight bit CRC8 of the first 56 bits.
	/// <p>
	/// For example given the following address in hexadecimal:
	/// <p>
	/// 10 28 E9 14 00 00 00 F3
	/// <p>
	/// The above is a family code 10 device with a serialized data
	/// of 28 E9 14 00 00 00, and a CRC8 of F3.
	/// <p>
	/// The address can be stored in several ways:
	/// <ul>
	/// <li>
	/// As a little-endian byte array:<p>
	/// <code>byte[] address = { 0x10, (byte)0xE9, 0x14, 0x00, 0x00, 0x00, (byte)0xF3 };</code><p>
	/// </li>
	/// <li>
	/// As a big-endian long:<p>
	/// <code>long address = (long)0xF300000014E92810;</code><p>
	/// </li>
	/// <li>
	/// As a big-endian String:<p>
	/// <code>String address = "F300000014E92810";</code><p>
	/// </li>
	/// </ul>
	/// </summary>
	/// <version>     0.00, 21 August 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public class Address
	{
		
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Private constructor to prevent instantiation.</summary>
		private Address()
		{
		}
		
		//--------
		//-------- Methods
		//--------
		
		/// <summary> Checks the CRC8 calculation of this 1-Wire Network address.
		/// <p>
		/// The address is valid if the CRC8 of the first seven bytes of the address gives
		/// a result equal to the eighth byte.
		/// 
		/// </summary>
		/// <param name="address"> iButton or 1-Wire Network address to verify
		/// 
		/// </param>
		/// <returns> <code>true</code> if the family code is non-zero and the
		/// CRC8 calculation is correct.
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.CRC8">
		/// </seealso>
		public static bool isValid(byte[] address)
		{
			if ((address[0] != 0) && (CRC8.compute(address) == 0))
				return true;
			else if ((address[0] & 0x7F) == 0x1C)
			// DS28E04
			{
				// The DS28E04 has a pin selectable ROM ID input.  However,
				// the CRC8 for the ROM ID assumes that the selecatable bits
				// are always 1.
				return 0 == CRC8.compute(address, 2, 6, CRC8.compute(0x7F, CRC8.compute(address[0], 0)));
			}
			else
				return false;
		}
		
		/// <summary> Checks the CRC8 calculation of this 1-Wire Network address.
		/// <p>
		/// The address is valid if the CRC8 of the first seven bytes of the address gives
		/// a result equal to the eighth byte.
		/// 
		/// </summary>
		/// <param name="address"> iButton or 1-Wire Network address to verify
		/// 
		/// </param>
		/// <returns> <code>true</code> if the family code is non-zero and the
		/// CRC8 calculation is correct.
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.CRC8">
		/// </seealso>
		public static bool isValid(System.String address)
		{
			return isValid(toByteArray(address));
		}
		
		/// <summary> Checks the CRC8 calculation of this 1-Wire Network address.
		/// <p>
		/// The address is valid if the CRC8 of the first seven bytes of the address gives
		/// a result equal to the eighth byte.
		/// 
		/// </summary>
		/// <param name="address"> iButton or 1-Wire Network address to verify
		/// 
		/// </param>
		/// <returns> <code>true</code> if the family code is non-zero and the
		/// CRC8 calculation is correct.
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.CRC8">
		/// </seealso>
		public static bool isValid(long address)
		{
			return isValid(toByteArray(address));
		}
		
		/// <summary> Converts a 1-Wire Network address byte array (little endian)
		/// to a hex string representation (big endian).
		/// 
		/// </summary>
		/// <param name="address">family code first.
		/// 
		/// </param>
		/// <returns> address represented in a String, family code last.
		/// </returns>
		public static System.String toString(byte[] address)
		{
			// When displaying, the CRC is first, family code is last so
			// that the center 6 bytes are a real serial number (not byte reversed).
			
			byte[] barr = new byte[16];
			int index = 0;
			int ch;
			
			for (int i = 7; i >= 0; i--)
			{
				ch = (address[i] >> 4) & 0x0F;
				ch += ((ch > 9)?'A' - 10:'0');
				barr[index++] = (byte) ch;
				ch = address[i] & 0x0F;
				ch += ((ch > 9)?'A' - 10:'0');
				barr[index++] = (byte) ch;
			}
			
			return new System.String(SupportClass.ToCharArray(barr));
		}
		
		/// <summary> Converts a 1-Wire Network address long (little endian)
		/// to a hex string representation (big endian).
		/// 
		/// </summary>
		/// <param name="address">family code first.
		/// 
		/// </param>
		/// <returns> address represented in a long, little endian.
		/// </returns>
		public static System.String toString(long address)
		{
			return toString(toByteArray(address));
		}
		
		/// <summary> Converts a 1-Wire Network Address string (big endian)
		/// to a byte array (little endian).
		/// 
		/// </summary>
		/// <param name="address">family code last.
		/// 
		/// </param>
		/// <returns> address represented in a byte array, family
		/// code (LS byte) first.
		/// </returns>
		public static byte[] toByteArray(System.String address)
		{
			byte[] address_byte = new byte[8];
			
			for (int i = 0; i < 8; i++)
			{
               //System.Char.GetNumericValue(
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Character.digit' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				//address_byte[7 - i] = (byte) ((System.Char.GetNumericValue((address[i * 2])) << 4) | (System.Char.GetNumericValue(address[i * 2 + 1])));
				//System.Char.GetNumericValue(address_byte[7 - i] = (byte) (System.Char.GetNumericValue(address[i * 2]) + System.Char.GetNumericValue(address[i * 2 + 1])));

                //address_byte[7-i] = System.Convert.Tobyte(address.Substring(i * 2,2),16); // !!!
                address_byte[7 - i] = System.Convert.ToByte(address.Substring(i * 2, 2), 16); // !!!


			}
			
			return address_byte;
		}
		
		/// <summary> Convert an iButton or 1-Wire device address as a long
		/// (little endian) into an array of bytes.
		/// </summary>
		public static byte[] toByteArray(long address)
		{
			
			/* This looks funny, but it should actually take
			less time since I do 7 eight bit shifts instead
			of 8+16+24+32+40+48+56 shifts.
			*/
			byte[] address_byte = new byte[8];
			
			address_byte[0] = (byte) address;
			address = SupportClass.URShift(address, 8);
			address_byte[1] = (byte) address;
			address = SupportClass.URShift(address, 8);
			address_byte[2] = (byte) address;
			address = SupportClass.URShift(address, 8);
			address_byte[3] = (byte) address;
			address = SupportClass.URShift(address, 8);
			address_byte[4] = (byte) address;
			address = SupportClass.URShift(address, 8);
			address_byte[5] = (byte) address;
			address = SupportClass.URShift(address, 8);
			address_byte[6] = (byte) address;
			address = SupportClass.URShift(address, 8);
			address_byte[7] = (byte) address;
			
			return address_byte;
		}
		
		/// <summary> Converts a 1-Wire Network Address to a long (little endian).
		/// 
		/// </summary>
		/// <returns> address represented as a long.
		/// </returns>
		public static long toLong(byte[] address)
		{
			/* This looks funny, but it should actually take
			less time since I do 7 eight bit shifts instead
			of 8+16+24+32+40+48+56 shifts.
			*/

            /*
			long longVal = (long) (address[7] & 0xFF);
			longVal <<= 8;
			longVal |= (long) (address[6] & 0xFF);
			longVal <<= 8;
			longVal |= (long) (address[5] & 0xFF);
			longVal <<= 8;
			longVal |= (long) (address[4] & 0xFF);
			longVal <<= 8;
			longVal |= (long) (address[3] & 0xFF);
			longVal <<= 8;
			longVal |= (long) (address[2] & 0xFF);
			longVal <<= 8;
			longVal |= (long) (address[1] & 0xFF);
			longVal <<= 8;
			longVal |= (long) (address[0] & 0xFF);
			
			return longVal;
            */
            //BitConverter.IsLittleEndian = true;
            
            return ((long)BitConverter.ToUInt64(address, 0));


		}
		
		/// <summary> Converts a 1-Wire Network Address to a long (little endian).
		/// 
		/// </summary>
		/// <returns> address represented as a String.
		/// </returns>
		public static long toLong(System.String address)
		{
			return toLong(toByteArray(address));
		}
	}
}