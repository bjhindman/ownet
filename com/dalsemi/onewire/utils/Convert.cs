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
	
	/// <summary> Utilities for conversion between miscellaneous datatypes.
	/// 
	/// </summary>
	/// <version>     1.00, 28 December 2001
	/// </version>
	/// <author>      SH
	/// </author>
	public class Convert
	{
		/// <summary>returns hex character for each digit, 0-15 </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'lookup_hex '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly char[] lookup_hex = new char[]{'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
		
		/// <summary> Inner class for conversion exceptions
		/// 
		/// </summary>
		[Serializable]
		public class ConvertException:System.Exception
		{
			public ConvertException(System.String message):base(message)
			{
			}
			public ConvertException():base()
			{
			}
		}
		
		/// <summary> Not to be instantiated</summary>
		private Convert()
		{
			;
		}
		
		// ----------------------------------------------------------------------
		// Temperature conversions
		// ----------------------------------------------------------------------
		
		// ??? does this optimization help out on TINI, where double-division is
		// ??? potentially slower?  If not, feel free to delete it.
		// ???
		/// <summary>cache the value of five divided by nine, which is irrational </summary>
		private const double FIVE_NINTHS = (5.0d / 9.0d);
		
		/// <summary> Converts a temperature reading from Celsius to Fahrenheit.
		/// 
		/// </summary>
		/// <param name="celsiusTemperature">temperature value in Celsius
		/// 
		/// </param>
		/// <returns>  the Fahrenheit conversion of the supplied temperature
		/// </returns>
		public static double toFahrenheit(double celsiusTemperature)
		{
			// (9/5)=1.8
			return celsiusTemperature * 1.8d + 32.0d;
		}
		
		/// <summary> Converts a temperature reading from Fahrenheit to Celsius.
		/// 
		/// </summary>
		/// <param name="fahrenheitTemperature">temperature value in Fahrenheit
		/// 
		/// </param>
		/// <returns>  the Celsius conversion of the supplied temperature
		/// </returns>
		public static double toCelsius(double fahrenheitTemperature)
		{
			return (fahrenheitTemperature - 32.0d) * FIVE_NINTHS;
		}
		
		// ----------------------------------------------------------------------
		// Long <-> ByteArray conversions
		// ----------------------------------------------------------------------
		
		/// <summary> This method constructs a long from a Lbyte byte array of specified length.
		/// 
		/// </summary>
		/// <param name="byteArray">byte array to convert to a long (Lbyte first)
		/// </param>
		/// <param name="offset">byte offset into the array where to start to convert
		/// </param>
		/// <param name="len">number of bytes to use to convert to a long
		/// 
		/// </param>
		/// <returns>s value constructed from bytes
		/// </returns>
		public static long toLong(byte[] byteArray, int offset, int len)
		{
			long val = 0;
			
			len = System.Math.Min(len, 8);
			
			// Concatanate the byte array into one variable.
			for (int i = (len - 1); i >= 0; i--)
			{
				val <<= 8;
				//val |= (byteArray[offset + i] & 0x00FF);
                val |= byteArray[offset + i];
			}
			
			return val;        
		}
		
		/// <summary> This method constructs a long from a Lbyte byte array of specified length.
		/// Uses 8 bytes starting at the first index.
		/// 
		/// </summary>
		/// <param name="byteArray">byte array to convert to a long (Lbyte first)
		/// 
		/// </param>
		/// <returns>s value constructed from bytes
		/// </returns>
		public static long toLong(byte[] byteArray)
		{
			return toLong(byteArray, 0, System.Math.Min(8, byteArray.Length));
		}
		
		/// <summary> This method constructs a Lbyte byte array with specified length from a long.
		/// 
		/// </summary>
		/// <param name="longVal">the long value to convert to a byte array.
		/// </param>
		/// <param name="byteArray">Lbyte first byte array, holds bytes from long
		/// </param>
		/// <param name="offset">byte offset into the array
		/// </param>
		/// <param name="len">number of bytes to get
		/// 
		/// </param>
		/// <returns>s value constructed from bytes
		/// </returns>
		public static void  toByteArray(long longVal, byte[] byteArray, int offset, int len)
		{
			int max = offset + len;
			
			// Concatanate the byte array into one variable.
			for (int i = offset; i < max; i++)
			{
				byteArray[i] = (byte) longVal;
				longVal = SupportClass.URShift(longVal, 8);
			}
		}
		
		/// <summary> This method constructs a Lbyte byte array with 8 bytes from a long.
		/// 
		/// </summary>
		/// <param name="longVal">the long value to convert to a byte array.
		/// </param>
		/// <param name="byteArray">Lbyte first byte array, holds bytes from long
		/// 
		/// </param>
		public static void  toByteArray(long longVal, byte[] byteArray)
		{
			toByteArray(longVal, byteArray, 0, 8);
		}
		
		/// <summary> This method constructs a Lbyte byte array with 8 bytes from a long.
		/// 
		/// </summary>
		/// <param name="longVal">the long value to convert to a byte array.
		/// 
		/// </param>
		/// <returns>s value constructed from bytes
		/// </returns>
		public static byte[] toByteArray(long longVal)
		{
			byte[] byteArray = new byte[8];
			toByteArray(longVal, byteArray, 0, 8);
			return byteArray;
		}
		
		// ----------------------------------------------------------------------
		// Int <-> ByteArray conversions
		// ----------------------------------------------------------------------
		
		/// <summary> This method constructs an int from a Lbyte byte array of specified length.
		/// 
		/// </summary>
		/// <param name="byteArray">byte array to convert to an int (Lbyte first)
		/// </param>
		/// <param name="offset">byte offset into the array where to start to convert
		/// </param>
		/// <param name="len">number of bytes to use to convert to an int
		/// 
		/// </param>
		/// <returns>s value constructed from bytes
		/// </returns>
		public static int toInt(byte[] byteArray, int offset, int len)
		{
			int val = 0;
			
			len = System.Math.Min(len, 4);
			
			// Concatanate the byte array into one variable.
			for (int i = (len - 1); i >= 0; i--)
			{
				val <<= 8;
				val |= (byteArray[offset + i] & 0x00FF);
			}
			
			return val;
		}
		
		/// <summary> This method constructs an int from a Lbyte byte array of specified length.
		/// Uses 4 bytes starting at the first index.
		/// 
		/// </summary>
		/// <param name="byteArray">byte array to convert to an int (Lbyte first)
		/// 
		/// </param>
		/// <returns>s value constructed from bytes
		/// </returns>
		public static int toInt(byte[] byteArray)
		{
			return toInt(byteArray, 0, System.Math.Min(4, byteArray.Length));
		}
		
		/// <summary> This method constructs a Lbyte byte array with specified length from an int.
		/// 
		/// </summary>
		/// <param name="intVal">the int value to convert to a byte array.
		/// </param>
		/// <param name="byteArray">Lbyte first byte array, holds bytes from int
		/// </param>
		/// <param name="offset">byte offset into the array
		/// </param>
		/// <param name="len">number of bytes to get
		/// </param>
		public static void  toByteArray(int intVal, byte[] byteArray, int offset, int len)
		{
			int max = offset + len;
			
			// Concatanate the byte array into one variable.
			for (int i = offset; i < max; i++)
			{
				byteArray[i] = (byte) intVal;
				intVal = SupportClass.URShift(intVal, 8);
			}
		}
		
		/// <summary> This method constructs a Lbyte byte array with 4 bytes from an int.
		/// 
		/// </summary>
		/// <param name="intVal">the int value to convert to a byte array.
		/// </param>
		/// <param name="byteArray">Lbyte first byte array, holds bytes from long
		/// 
		/// </param>
		public static void  toByteArray(int intVal, byte[] byteArray)
		{
			toByteArray(intVal, byteArray, 0, 4);
		}
		
		/// <summary> This method constructs a Lbyte byte array with 4 bytes from an int.
		/// 
		/// </summary>
		/// <param name="longVal">the long value to convert to a byte array.
		/// 
		/// </param>
		/// <returns>s value constructed from bytes
		/// </returns>
		public static byte[] toByteArray(int intVal)
		{
			byte[] byteArray = new byte[4];
			toByteArray(intVal, byteArray, 0, 4);
			return byteArray;
		}
		
		// ----------------------------------------------------------------------
		// String <-> ByteArray conversions
		// ----------------------------------------------------------------------
		
		/// <summary> <P>Converts a hex-encoded string into an array of bytes.</P>
		/// <P>To illustrate the rules for parsing, the following String:<br>
		/// "FF 0 1234 567"<br>
		/// becomes:<br>
		/// byte[]{0xFF,0x00,0x12,0x34,0x56,0x07}
		/// </P>
		/// 
		/// </summary>
		/// <param name="strData">hex-encoded numerical string
		/// </param>
		/// <returns> byte[] the decoded bytes
		/// </returns>
		public static byte[] toByteArray(System.String strData)
		{
			byte[] bDataTmp = new byte[strData.Length * 2];
			int len = toByteArray(strData, bDataTmp, 0, bDataTmp.Length);
			byte[] bData = new byte[len];
			Array.Copy(bDataTmp, 0, bData, 0, len);
			return bData;
		}
		
		/// <summary> <P>Converts a hex-encoded string into an array of bytes.</P>
		/// <P>To illustrate the rules for parsing, the following String:<br>
		/// "FF 0 1234 567"<br>
		/// becomes:<br>
		/// byte[]{0xFF,0x00,0x12,0x34,0x56,0x07}
		/// </P>
		/// 
		/// </summary>
		/// <param name="strData">hex-encoded numerical string
		/// </param>
		/// <param name="bData">byte[] which will hold the decoded bytes
		/// </param>
		/// <returns> The number of bytes converted
		/// </returns>
		public static int toByteArray(System.String strData, byte[] bData)
		{
			return toByteArray(strData, bData, 0, bData.Length);
		}
		
		/// <summary> <P>Converts a hex-encoded string into an array of bytes.</P>
		/// <P>To illustrate the rules for parsing, the following String:<br>
		/// "FF 0 1234 567"<br>
		/// becomes:<br>
		/// byte[]{0xFF,0x00,0x12,0x34,0x56,0x07}
		/// </P>
		/// 
		/// </summary>
		/// <param name="strData">hex-encoded numerical string
		/// </param>
		/// <param name="bData">byte[] which will hold the decoded bytes
		/// </param>
		/// <param name="offset">the offset into bData to start placing bytes
		/// </param>
		/// <param name="length">the maximum number of bytes to convert
		/// </param>
		/// <returns> The number of bytes converted
		/// </returns>
		public static int toByteArray(System.String strData, byte[] bData, int offset, int length)
		{
			int strIndex = 0, strLength = strData.Length;
			int index = offset;
			int end = length + offset;
			char upper, lower;
			int uVal, lVal;
			
			while (index < end && strIndex < strLength)
			{
				lower = '0';
				do 
				{
					upper = strData[strIndex++];
				}
				while (strIndex < strLength && System.Char.IsWhiteSpace(upper));
				
				// still haven't reached the end of the string
				if (strIndex < strLength)
				{
					lower = strData[strIndex++];
					if (System.Char.IsWhiteSpace(lower))
					{
						lower = upper;
						upper = '0';
					}
				}
				// passed the end of the string with only one character
				else if (!System.Char.IsWhiteSpace(upper))
				{
					lower = upper;
					upper = '0';
				}
				// passed the end of string with no characters
				else
					continue;
				
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Character.digit' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				uVal = (int)System.Char.GetNumericValue(upper);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Character.digit' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				lVal = (int)System.Char.GetNumericValue(lower);
				if (uVal != - 1 && lVal != - 1)
				{
					bData[index++] = (byte) (((uVal & 0x0F) << 4) | (lVal & 0x0F));
				}
				else
					throw new ConvertException(("Bad character in input string: " + upper) + lower);
			}
			return (index - offset);
		}
		
		/// <summary> Converts a byte array into a hex-encoded String, using the provided
		/// delimeter.
		/// 
		/// </summary>
		/// <param name="data">The byte[] to convert to a hex-encoded string
		/// </param>
		/// <returns> Hex-encoded String
		/// </returns>
		public static System.String toHexString(byte[] data)
		{
			return toHexString(data, 0, data.Length, "");
		}
		
		/// <summary> Converts a byte array into a hex-encoded String, using the provided
		/// delimeter.
		/// 
		/// </summary>
		/// <param name="data">The byte[] to convert to a hex-encoded string
		/// </param>
		/// <param name="offset">the offset to start converting bytes
		/// </param>
		/// <param name="length">the number of bytes to convert
		/// </param>
		/// <returns> Hex-encoded String
		/// </returns>
		public static System.String toHexString(byte[] data, int offset, int length)
		{
			return toHexString(data, offset, length, "");
		}
		
		
		/// <summary> Converts a byte array into a hex-encoded String, using the provided
		/// delimeter.
		/// 
		/// </summary>
		/// <param name="data">The byte[] to convert to a hex-encoded string
		/// </param>
		/// <param name="delimeter">the delimeter to place between each byte of data
		/// </param>
		/// <returns> Hex-encoded String
		/// </returns>
		public static System.String toHexString(byte[] data, System.String delimeter)
		{
			return toHexString(data, 0, data.Length, delimeter);
		}
		
		/// <summary> Converts a byte array into a hex-encoded String, using the provided
		/// delimeter.
		/// 
		/// </summary>
		/// <param name="data">The byte[] to convert to a hex-encoded string
		/// </param>
		/// <param name="offset">the offset to start converting bytes
		/// </param>
		/// <param name="length">the number of bytes to convert
		/// </param>
		/// <param name="delimeter">the delimeter to place between each byte of data
		/// </param>
		/// <returns> Hex-encoded String
		/// </returns>
		public static System.String toHexString(byte[] data, int offset, int length, System.String delimeter)
		{
			System.Text.StringBuilder value_Renamed = new System.Text.StringBuilder(length * (2 + delimeter.Length));
			int max = length + offset;
			int lastDelim = max - 1;
			for (int i = offset; i < max; i++)
			{
				byte bits = data[i];
				value_Renamed.Append(lookup_hex[(bits >> 4) & 0x0F]);
				value_Renamed.Append(lookup_hex[bits & 0x0F]);
				if (i < lastDelim)
					value_Renamed.Append(delimeter);
			}
			return value_Renamed.ToString();
		}
		
		/// <summary> <P>Converts a single byte into a hex-encoded string.</P>
		/// 
		/// </summary>
		/// <param name="bValue">the byte to encode
		/// </param>
		/// <returns> String Hex-encoded String
		/// </returns>
		public static System.String toHexString(byte bValue)
		{
			char[] hexValue = new char[2];
			hexValue[1] = lookup_hex[bValue & 0x0F];
			hexValue[0] = lookup_hex[(bValue >> 4) & 0x0F];
			return new System.String(hexValue);
		}
		
		/// <summary> Converts a char array into a hex-encoded String, using the provided
		/// delimeter.
		/// 
		/// </summary>
		/// <param name="data">The char[] to convert to a hex-encoded string
		/// </param>
		/// <returns> Hex-encoded String
		/// </returns>
		public static System.String toHexString(char[] data)
		{
			return toHexString(data, 0, data.Length, "");
		}
		
		/// <summary> Converts a byte array into a hex-encoded String, using the provided
		/// delimeter.
		/// 
		/// </summary>
		/// <param name="data">The char[] to convert to a hex-encoded string
		/// </param>
		/// <param name="offset">the offset to start converting bytes
		/// </param>
		/// <param name="length">the number of bytes to convert
		/// </param>
		/// <returns> Hex-encoded String
		/// </returns>
		public static System.String toHexString(char[] data, int offset, int length)
		{
			return toHexString(data, offset, length, "");
		}
		
		
		/// <summary> Converts a char array into a hex-encoded String, using the provided
		/// delimeter.
		/// 
		/// </summary>
		/// <param name="data">The char[] to convert to a hex-encoded string
		/// </param>
		/// <param name="delimeter">the delimeter to place between each byte of data
		/// </param>
		/// <returns> Hex-encoded String
		/// </returns>
		public static System.String toHexString(char[] data, System.String delimeter)
		{
			return toHexString(data, 0, data.Length, delimeter);
		}
		
		/// <summary> Converts a char array into a hex-encoded String, using the provided
		/// delimeter.
		/// 
		/// </summary>
		/// <param name="data">The char[] to convert to a hex-encoded string
		/// </param>
		/// <param name="offset">the offset to start converting bytes
		/// </param>
		/// <param name="length">the number of bytes to convert
		/// </param>
		/// <param name="delimeter">the delimeter to place between each byte of data
		/// </param>
		/// <returns> Hex-encoded String
		/// </returns>
		public static System.String toHexString(char[] data, int offset, int length, System.String delimeter)
		{
			System.Text.StringBuilder value_Renamed = new System.Text.StringBuilder(length * (2 + delimeter.Length));
			int max = length + offset;
			int lastDelim = max - 1;
			for (int i = offset; i < max; i++)
			{
				char bits = data[i];
				value_Renamed.Append(lookup_hex[(bits >> 4) & 0x0F]);
				value_Renamed.Append(lookup_hex[bits & 0x0F]);
				if (i < lastDelim)
					value_Renamed.Append(delimeter);
			}
			return value_Renamed.ToString();
		}
		
		/// <summary> <P>Converts a single character into a hex-encoded string.</P>
		/// 
		/// </summary>
		/// <param name="bValue">the byte to encode
		/// </param>
		/// <returns> String Hex-encoded String
		/// </returns>
		public static System.String toHexString(char bValue)
		{
			char[] hexValue = new char[2];
			hexValue[1] = lookup_hex[bValue & 0x0F];
			hexValue[0] = lookup_hex[(bValue >> 4) & 0x0F];
			return new System.String(hexValue);
		}
		
		
		// ----------------------------------------------------------------------
		// String <-> Long conversions
		// ----------------------------------------------------------------------
		
		/// <summary> <P>Converts a hex-encoded string (Lbyte) into a long.</P>
		/// <P>To illustrate the rules for parsing, the following String:<br>
		/// "FF 0 1234 567 12 03"<br>
		/// becomes:<br>
		/// long 0x03120756341200ff
		/// </P>
		/// 
		/// </summary>
		/// <param name="strData">hex-encoded numerical string
		/// </param>
		/// <returns> the decoded long
		/// </returns>
		public static long toLong(System.String strData)
		{
			return toLong(toByteArray(strData));
		}
		
		/// <summary> <P>Converts a long into a hex-encoded string (Lbyte).</P>
		/// 
		/// </summary>
		/// <param name="lValue">the long integer to encode
		/// </param>
		/// <returns> String Hex-encoded String
		/// </returns>
		public static System.String toHexString(long lValue)
		{
			return toHexString(toByteArray(lValue), "");
		}
		
		// ----------------------------------------------------------------------
		// String <-> Int conversions
		// ----------------------------------------------------------------------
		
		/// <summary> <P>Converts a hex-encoded string (Lbyte) into an int.</P>
		/// <P>To illustrate the rules for parsing, the following String:<br>
		/// "FF 0 1234 567 12 03"<br>
		/// becomes:<br>
		/// long 0x03120756341200ff
		/// </P>
		/// 
		/// </summary>
		/// <param name="strData">hex-encoded numerical string
		/// </param>
		/// <returns> the decoded int
		/// </returns>
		public static int toInt(System.String strData)
		{
			return toInt(toByteArray(strData));
		}
		
		/// <summary> <P>Converts an integer into a hex-encoded string (Lbyte).</P>
		/// 
		/// </summary>
		/// <param name="iValue">the integer to encode
		/// </param>
		/// <returns> String Hex-encoded String
		/// </returns>
		public static System.String toHexString(int iValue)
		{
			return toHexString(toByteArray(iValue), "");
		}
		
		// ----------------------------------------------------------------------
		// Double conversions
		// ----------------------------------------------------------------------
		
		/// <summary>Field Double NEGATIVE_INFINITY </summary>
		internal const double d_POSITIVE_INFINITY = 1.0d / 0.0d;
		/// <summary>Field Double NEGATIVE_INFINITY </summary>
		internal const double d_NEGATIVE_INFINITY = (- 1.0d) / 0.0d;
		
		/// <summary> <P>Converts a double value into a string with the specified number of
		/// digits after the decimal place.</P>
		/// 
		/// </summary>
		/// <param name="dubbel">the double value to convert to a string
		/// </param>
		/// <param name="nFrac">the number of digits to display after the decimal point
		/// 
		/// </param>
		/// <returns> String representation of the double value with the specified
		/// number of digits after the decimal place.
		/// </returns>
		public static System.String toString(double dubbel, int nFrac)
		{
			// check for special case
			if (dubbel == d_POSITIVE_INFINITY)
				return "Infinity";
			else if (dubbel == d_NEGATIVE_INFINITY)
				return "-Infinity";
			//else if (dubbel != dubbel)  // 
				//return "NaN";           // I don't think this is necessary
			
			// check for fast out (no fractional digits)
			if (nFrac <= 0)
			// round the whole portion
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				return System.Convert.ToString((long) (dubbel + 0.5d));
			}
			
			// extract the non-fractional portion
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			long dWhole = (long) dubbel;
			
			// figure out if it's positive or negative.  We need to remove
			// the sign from the fractional part
			double sign = (dWhole < 0)?- 1d:1d;
			
			// figure out how many places to shift fractional portion
			double shifter = 1;
			for (int i = 0; i < nFrac; i++)
				shifter *= 10;
			
			// extract, unsign, shift, and round the fractional portion
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			long dFrac = (long) ((dubbel - dWhole) * sign * shifter + 0.5d);
			
			// convert the fractional portion to a string
			System.String fracString = System.Convert.ToString(dFrac);
			int fracLength = fracString.Length;
			
			// ensure that rounding the fraction didn't carry into the whole portion
			if (fracLength > nFrac)
			{
				dWhole += 1;
				fracLength = 0;
			}
			
			// convert the whole portion to a string
			System.String wholeString = System.Convert.ToString(dWhole);
			int wholeLength = wholeString.Length;
			
			// create the string buffer
			char[] dubbelChars = new char[wholeLength + 1 + nFrac];
			
			// append the non-fractional portion
			SupportClass.GetCharsFromString(wholeString, 0, wholeLength, dubbelChars, 0);
			
			// and the decimal place
			dubbelChars[wholeLength] = '.';
			
			// append any necessary leading zeroes
			int i2 = wholeLength + 1;
			int max = i2 + nFrac - fracLength;
			for (; i2 < max; i2++)
				dubbelChars[i2] = '0';
			
			// append the fractional portion
			if (fracLength > 0)
				SupportClass.GetCharsFromString(fracString, 0, fracLength, dubbelChars, max);
			
			return new System.String(dubbelChars, 0, dubbelChars.Length);
		}
		
		
		// ----------------------------------------------------------------------
		// Float conversions
		// ----------------------------------------------------------------------
		
		/// <summary>Field Float NEGATIVE_INFINITY </summary>
		internal const float f_POSITIVE_INFINITY = 1.0f / 0.0f;
		/// <summary>Field Float NEGATIVE_INFINITY </summary>
		internal const float f_NEGATIVE_INFINITY = (- 1.0f) / 0.0f;
		
		/// <summary> <P>Converts a float value into a string with the specified number of
		/// digits after the decimal place.</P>
		/// <P>Note: this function does not properly handle special case float
		/// values such as Infinity and NaN.</P>
		/// 
		/// </summary>
		/// <param name="flote">the float value to convert to a string
		/// </param>
		/// <param name="nFrac">the number of digits to display after the decimal point
		/// 
		/// </param>
		/// <returns> String representation of the float value with the specified
		/// number of digits after the decimal place.
		/// </returns>
		public static System.String toString(float flote, int nFrac)
		{
			// check for special case
			if (flote == f_POSITIVE_INFINITY)
				return "Infinity";
			else if (flote == f_NEGATIVE_INFINITY)
				return "-Infinity";
			//else if (flote != flote) // this is probably not necessary
				//return "NaN";
			
			// check for fast out (no fractional digits)
			if (nFrac <= 0)
			// round the whole portion
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				return System.Convert.ToString((long) (flote + 0.5f));
			}
			
			// extract the non-fractional portion
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			long fWhole = (long) flote;
			
			// figure out if it's positive or negative.  We need to remove
			// the sign from the fractional part
			float sign = (fWhole < 0)?- 1f:1f;
			
			// figure out how many places to shift fractional portion
			float shifter = 1;
			for (int i = 0; i < nFrac; i++)
				shifter *= 10;
			
			// extract, shift, and round the fractional portion
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			long fFrac = (long) ((flote - fWhole) * sign * shifter + 0.5f);
			
			// convert the fractional portion to a string
			System.String fracString = System.Convert.ToString(fFrac);
			int fracLength = fracString.Length;
			
			// ensure that rounding the fraction didn't carry into the whole portion
			if (fracLength > nFrac)
			{
				fWhole += 1;
				fracLength = 0;
			}
			
			// convert the whole portion to a string
			System.String wholeString = System.Convert.ToString(fWhole);
			int wholeLength = wholeString.Length;
			
			// create the string buffer
			char[] floteChars = new char[wholeLength + 1 + nFrac];
			
			// append the non-fractional portion
			SupportClass.GetCharsFromString(wholeString, 0, wholeLength, floteChars, 0);
			
			// and the decimal place
			floteChars[wholeLength] = '.';
			
			// append any necessary leading zeroes
			int i2 = wholeLength + 1;
			int max = i2 + nFrac - fracLength;
			for (; i2 < max; i2++)
				floteChars[i2] = '0';
			
			// append the fractional portion
			if (fracLength > 0)
				SupportClass.GetCharsFromString(fracString, 0, fracLength, floteChars, max);
			
			return new System.String(floteChars, 0, floteChars.Length);
		}
	}
}