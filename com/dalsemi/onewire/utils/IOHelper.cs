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
	
	/// <summary> Generic IO routines.  Supports printing and reading arrays of bytes.
	/// Also, using the setReader and setWriter methods, the source of the
	/// bytes can come from any stream as well as the destination for
	/// written bytes.  All routines are static and final and handle all
	/// exceptional cases by returning a default value.
	/// 
	/// </summary>
	/// <version>     0.02, 2 June 2001
	/// </version>
	/// <author>      SH
	/// </author>
	public sealed class IOHelper
	{
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'setReader'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static System.IO.StreamReader Reader
		{
			set
			{
				lock (typeof(com.dalsemi.onewire.utils.IOHelper))
				{
					//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
					br = new System.IO.StreamReader(value.BaseStream, value.CurrentEncoding);
				}
			}
			
		}
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Writer' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'setWriter'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static System.IO.StreamWriter Writer
		{
			set
			{
				lock (typeof(com.dalsemi.onewire.utils.IOHelper))
				{
					pw = new System.IO.StreamWriter(value.BaseStream, value.Encoding);
				}
			}
			
		}
		/// <summary>Do not instantiate this class </summary>
		private IOHelper()
		{
			;
		}
		
		/*----------------------------------------------------------------*/
		/*   Reading Helper Methods                                       */
		/*----------------------------------------------------------------*/
		
		private static System.IO.StreamReader br = null;
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'readLine'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static System.String readLine()
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				try
				{
					return br.ReadLine();
				}
				catch (System.IO.IOException ioe)
				{
					return "";
				}
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'readBytes'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static byte[] readBytes(int count, int pad, bool hex)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				if (hex)
					return readBytesHex(count, pad);
				else
					return readBytesAsc(count, pad);
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'readBytesHex'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static byte[] readBytesHex(int count, int pad)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				try
				{
					System.String s = br.ReadLine();
					int len = s.Length > count?count:s.Length;
					byte[] ret;
					
					if (count > 0)
						ret = new byte[count];
					else
						ret = new byte[s.Length];
					
					byte[] temp = parseHex(s, 0);
					
					if (count == 0)
						return temp;
					
					len = temp.Length;
					
					Array.Copy(temp, 0, ret, 0, len);
					
					for (; len < count; len++)
						ret[len] = (byte) pad;
					
					return ret;
				}
				catch (System.Exception e)
				{
					return new byte[count];
				}
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'readBytesAsc'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static byte[] readBytesAsc(int count, int pad)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				try
				{
					System.String s = br.ReadLine();
					int len = s.Length > count?count:s.Length;
					byte[] ret;
					
					if (count > 0)
						ret = new byte[count];
					else
						ret = new byte[s.Length];
					
					if (count == 0)
					{
						//Array.Copy(SupportClass.ToByteArray(SupportClass.ToByteArray(s)), 0, ret, 0, s.Length);
                        Array.Copy(SupportClass.ToByteArray(s), 0, ret, 0, s.Length);
						
						return ret;
					}
					
					//Array.Copy(SupportClass.ToByteArray(SupportClass.ToByteArray(s)), 0, ret, 0, len);
                    Array.Copy(SupportClass.ToByteArray(s), 0, ret, 0, len);
					
					for (; len < count; len++)
						ret[len] = (byte) pad;
					
					return ret;
				}
				catch (System.IO.IOException e)
				{
					return new byte[count];
				}
			}
		}
		
		private static byte[] parseHex(System.String s, int size)
		{
			byte[] temp;
			int index = 0;
			char[] x = s.ToLower().ToCharArray();
			
			if (size > 0)
				temp = new byte[size];
			else
				temp = new byte[x.Length];
			
			try
			{
				for (int i = 0; i < x.Length && index < temp.Length; index++)
				{
					int digit = - 1;
					
					while (i < x.Length && digit == - 1)
					{
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Character.digit' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						digit = (int)System.Char.GetNumericValue(x[i++]);
					}
					if (digit != - 1)
						temp[index] = (byte) ((digit << 4) & 0xF0);
					
					digit = - 1;
					
					while (i < x.Length && digit == - 1)
					{
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Character.digit' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						digit = (int)System.Char.GetNumericValue(x[i++]);
					}
					if (digit != - 1)
						temp[index] |= (byte) (digit & 0x0F);
				}
			}
			catch (System.Exception e)
			{
				;
			}
			
			byte[] t;
			
			if (size == 0 && temp.Length != index)
			{
				t = new byte[index];
				Array.Copy(temp, 0, t, 0, t.Length);
			}
			else
				t = temp;
			
			return t;
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'readInt'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static int readInt()
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				return readInt(- 1);
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'readInt'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static int readInt(int def)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				try
				{
					return System.Int32.Parse(br.ReadLine());
				}
				catch (System.FormatException nfe)
				{
					return def;
				}
				catch (System.IO.IOException ioe)
				{
					return def;
				}
			}
		}
		
		/*----------------------------------------------------------------*/
		/*   Writing Helper Methods                                       */
		/*----------------------------------------------------------------*/
		
		private static System.IO.StreamWriter pw = null;
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeBytesHex'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeBytesHex(System.String delim, byte[] b, int offset, int cnt)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				int i = offset;
				for (; i < (offset + cnt); )
				{
					if (i != offset && ((i - offset) & 15) == 0)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln'"
						pw.WriteLine();
					}
					pw.Write(byteStr(b[i++]));
					pw.Write(delim);
				}
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln'"
				pw.WriteLine();
				pw.Flush();
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeBytesHex'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeBytesHex(byte[] b, int offset, int cnt)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				writeBytesHex(".", b, offset, cnt);
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeBytesHex'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeBytesHex(byte[] b)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				writeBytesHex(".", b, 0, b.Length);
			}
		}
		
		/// <summary> Writes a <code>byte[]</code> to the specified output stream.  This method
		/// writes a combined hex and ascii representation where each line has
		/// (at most) 16 bytes of data in hex followed by three spaces and the ascii
		/// representation of those bytes.  To write out just the Hex representation,
		/// use <code>writeBytesHex(byte[],int,int)</code>.
		/// 
		/// </summary>
		/// <param name="b">the byte array to print out.
		/// </param>
		/// <param name="offset">the starting location to begin printing
		/// </param>
		/// <param name="cnt">the number of bytes to print.
		/// </param>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeBytes'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeBytes(System.String delim, byte[] b, int offset, int cnt)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				int last, i;
				last = i = offset;
				for (; i < (offset + cnt); )
				{
					if (i != offset && ((i - offset) & 15) == 0)
					{
						pw.Write("  ");
						for (; last < i; last++)
							pw.Write((char) b[last]);
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln'"
						pw.WriteLine();
					}
					pw.Write(byteStr(b[i++]));
					pw.Write(delim);
				}
				for (int k = i; ((k - offset) & 15) != 0; k++)
				{
					pw.Write("  ");
					pw.Write(delim);
				}
				pw.Write("  ");
				for (; last < i; last++)
					pw.Write((char) b[last]);
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln'"
				pw.WriteLine();
				pw.Flush();
			}
		}
		
		/// <summary> Writes a <code>byte[]</code> to the specified output stream.  This method
		/// writes a combined hex and ascii representation where each line has
		/// (at most) 16 bytes of data in hex followed by three spaces and the ascii
		/// representation of those bytes.  To write out just the Hex representation,
		/// use <code>writeBytesHex(byte[],int,int)</code>.
		/// 
		/// </summary>
		/// <param name="b">the byte array to print out.
		/// </param>
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeBytes'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeBytes(byte[] b)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				writeBytes(".", b, 0, b.Length);
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeBytes'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeBytes(byte[] b, int offset, int cnt)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				writeBytes(".", b, offset, cnt);
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'write'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  write(System.String s)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				pw.Write(s);
				pw.Flush();
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'write'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  write(System.Object o)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				pw.Write(o);
				pw.Flush();
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'write'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  write(bool b)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				pw.Write(b);
				pw.Flush();
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'write'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  write(int i)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				pw.Write(i);
				pw.Flush();
			}
		}
		
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeLine'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeLine()
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln'"
				pw.WriteLine();
				pw.Flush();
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeLine'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeLine(System.String s)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				pw.WriteLine(s);
				pw.Flush();
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeLine'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeLine(System.Object o)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangObject'"
				pw.WriteLine(o);
				pw.Flush();
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeLine'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeLine(bool b)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_boolean'"
				pw.WriteLine(b);
				pw.Flush();
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeLine'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeLine(int i)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_int'"
				pw.WriteLine(i);
				pw.Flush();
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeHex'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeHex(byte b)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				pw.Write(byteStr(b));
				pw.Flush();
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeHex'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeHex(long l)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				pw.Write(System.Convert.ToString(l, 16));
				pw.Flush();
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeLineHex'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeLineHex(byte b)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				pw.WriteLine(byteStr(b));
				pw.Flush();
			}
		}
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'writeLineHex'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public static void  writeLineHex(long l)
		{
			lock (typeof(com.dalsemi.onewire.utils.IOHelper))
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				pw.WriteLine(System.Convert.ToString(l, 16));
				pw.Flush();
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'hex '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly char[] hex = "0123456789ABCDEF".ToCharArray();
		private static System.String byteStr(byte b)
		{
			return "" + hex[((b >> 4) & 0x0F)] + hex[(b & 0x0F)];
		}
		static IOHelper()
		{
			// default the buffered reader to read from STDIN
			{
				try
				{
					//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
					//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
					br = new System.IO.StreamReader(new System.IO.StreamReader(System.Console.OpenStandardInput(), System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(System.Console.OpenStandardInput(), System.Text.Encoding.Default).CurrentEncoding);
				}
				catch (System.Exception e)
				{
					System.Console.Error.WriteLine("IOHelper: Catastrophic Failure!");
					System.Environment.Exit(1);
				}
			}
			// default the print writer to write to STDOUT
			{
				try
				{
					//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
					pw = new System.IO.StreamWriter(new System.IO.StreamWriter(System.Console.OpenStandardOutput(), System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(System.Console.OpenStandardOutput(), System.Text.Encoding.Default).Encoding);
				}
				catch (System.Exception e)
				{
					System.Console.Error.WriteLine("IOHelper: Catastrophic Failure!");
					System.Environment.Exit(1);
				}
			}
		}
	}
}