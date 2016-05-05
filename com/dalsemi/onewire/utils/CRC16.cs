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
using System;
namespace com.dalsemi.onewire.utils
{
	
	/// <summary> CRC16 is a class containing an implementation of the
	/// Cyclic-Redundency-Check (CRC) CRC16.  The CRC16 is used in
	/// iButton memory packet structure.
	/// <p>
	/// CRC16 is based on the polynomial = X^16 + X^15 + X^2 + 1.
	/// 
	/// </summary>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public class CRC16
	{
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> used in CRC16 calculation</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'ODD_PARITY'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] ODD_PARITY = new int[]{0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0};
		
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Private constructor to prevent instantiation.</summary>
		private CRC16()
		{
		}
		
		//--------
		//-------- Methods
		//--------
		
		/// <summary> Perform the CRC16 on the data element based on a zero seed.
		/// <p>
		/// CRC16 is based on the polynomial = X^16 + X^15 + X^2 + 1.
		/// 
		/// </summary>
		/// <param name="dataToCrc">    data element on which to perform the CRC16
		/// </param>
		/// <returns>  CRC16 value
		/// </returns>
		public static int compute(int dataToCrc)
		{
			return compute(dataToCrc, 0);
		}
		
		/// <summary> Perform the CRC16 on the data element based on the provided seed.
		/// <p>
		/// CRC16 is based on the polynomial = X^16 + X^15 + X^2 + 1.
		/// 
		/// </summary>
		/// <param name="dataToCrc">    data element on which to perform the CRC16
		/// </param>
		/// <returns>  CRC16 value
		/// </returns>
		public static int compute(int dataToCrc, int seed)
		{
			int dat = ((dataToCrc ^ (seed & 0xFF)) & 0xFF);
			
			seed = SupportClass.URShift((seed & 0xFFFF), 8);
			
			int indx1 = (dat & 0x0F);
			int indx2 = (SupportClass.URShift(dat, 4));
			
			if ((ODD_PARITY[indx1] ^ ODD_PARITY[indx2]) == 1)
				seed = seed ^ 0xC001;
			
			dat = (dat << 6);
			seed = seed ^ dat;
			dat = (dat << 1);
			seed = seed ^ dat;
			
			return seed;
		}
		
		/// <summary> Perform the CRC16 on an array of data elements based on a
		/// zero seed.
		/// <p>
		/// CRC16 is based on the polynomial = X^16 + X^15 + X^2 + 1.
		/// 
		/// </summary>
		/// <param name="dataToCrc">  array of data elements on which to perform the CRC16
		/// 
		/// </param>
		/// <returns>  CRC16 value
		/// </returns>
		public static int compute(byte[] dataToCrc)
		{
			return compute(dataToCrc, 0, dataToCrc.Length, 0);
		}
		
		/// <summary> Perform the CRC16 on an array of data elements based on a
		/// zero seed.
		/// <p>
		/// CRC16 is based on the polynomial = X^16 + X^15 + X^2 + 1.
		/// 
		/// </summary>
		/// <param name="dataToCrc">  array of data elements on which to perform the CRC16
		/// </param>
		/// <param name="off">        offset into the data array
		/// </param>
		/// <param name="len">        length of data to CRC16
		/// 
		/// </param>
		/// <returns>  CRC16 value
		/// </returns>
		public static int compute(byte[] dataToCrc, int off, int len)
		{
			return compute(dataToCrc, off, len, 0);
		}
		
		/// <summary> Perform the CRC16 on an array of data elements based on the
		/// provided seed.
		/// <p>
		/// CRC16 is based on the polynomial = X^16 + X^15 + X^2 + 1.
		/// 
		/// </summary>
		/// <param name="dataToCrc">  array of data elements on which to perform the CRC16
		/// </param>
		/// <param name="off">        offset into the data array
		/// </param>
		/// <param name="len">        length of data to CRC16
		/// </param>
		/// <param name="seed">       seed to use for CRC16
		/// 
		/// </param>
		/// <returns>  CRC16 value
		/// </returns>
		public static int compute(byte[] dataToCrc, int off, int len, int seed)
		{
			
			// loop to do the crc on each data element
			for (int i = 0; i < len; i++)
				seed = compute(dataToCrc[i + off], seed);
			
			return seed;
		}
		
		/// <summary> Perform the CRC16 on an array of data elements based on the
		/// provided seed.
		/// <p>
		/// CRC16 is based on the polynomial = X^16 + X^15 + X^2 + 1.
		/// 
		/// </summary>
		/// <param name="dataToCrc">  array of data elements on which to perform the CRC16
		/// </param>
		/// <param name="seed">       seed to use for CRC16
		/// </param>
		/// <returns>  CRC16 value
		/// </returns>
		public static int compute(byte[] dataToCrc, int seed)
		{
			return compute(dataToCrc, 0, dataToCrc.Length, seed);
		}
	}
}