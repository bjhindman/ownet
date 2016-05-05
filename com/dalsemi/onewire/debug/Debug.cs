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
using OneWireAccessProvider = com.dalsemi.onewire.OneWireAccessProvider;
using Convert = com.dalsemi.onewire.utils.Convert;
namespace com.dalsemi.onewire.debug
{
	
	/// <summary> <p>This class is intended to help both developers of the 1-Wire API for
	/// Java and developers using the 1-Wire API for Java to have a standard
	/// method for printing debug messages.  Applications that want to see debug messages
	/// should call  the <code>setDebugMode(boolean)</code> method.
	/// Classes that want to print information under debugging
	/// circumstances should call the <code>debug(String)</code>
	/// method.</p>
	/// 
	/// <p>Debug printing is turned off by default.</p>
	/// 
	/// </summary>
	/// <version>     1.00, 1 Sep 2003
	/// </version>
	/// <author>      KA, SH
	/// </author>
	public class Debug
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Gets the debug printing mode for this application.
		/// 
		/// </summary>
		/// <returns> <code>true</code> indicates debug messages are on,
		/// <code>false</code> supresses them.
		/// </returns>
		/// <summary> Sets the debug printing mode for this application.
		/// 
		/// </summary>
		/// <param name="<code>true</code>">to see debug messages, <code>false</code>
		/// to suppress them
		/// </param>
		public static bool DebugMode
		{
			get
			{
				return DEBUG;
			}
			
			set
			{
				DEBUG = value;
			}
			
		}
		/// <summary> Sets the output stream for printing the debug info.
		/// 
		/// </summary>
		/// <param name="out">the output stream for printing the debug info.
		/// </param>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.PrintStream' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public static System.IO.StreamWriter PrintStream
		{
			set
			{
				out_Renamed = value;
			}
			
		}
		private static bool DEBUG = false;
		//UPGRADE_NOTE: The initialization of  'out_Renamed' was moved to static method 'com.dalsemi.onewire.debug.Debug'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static System.IO.StreamWriter out_Renamed;
		
		/// <summary> Prints the specified <code>java.lang.String</code> object
		/// if debug mode is enabled.  This method calls <code>PrintStream.println(String)</code>,
		/// and pre-pends the <code>String</code> ">> " to the message, so
		/// taht if a program were to call (when debug mode was enabled):
		/// <code><pre>
		/// com.dalsemi.onewire.debug.Debug.debug("Some notification...");
		/// </pre></code>
		/// the resulting output would look like:
		/// <code><pre>
		/// >> Some notification...
		/// </pre></code>
		/// 
		/// </summary>
		/// <param name="x">the message to print out if in debug mode
		/// </param>
		public static void  debug(System.String x)
		{
			if (DEBUG)
				out_Renamed.WriteLine(">> " + x);
		}
		
		/// <summary> Prints the specified array of bytes with a given label
		/// if debug mode is enabled.  This method calls
		/// <code>PrintStream.println(String)</code>,
		/// and pre-pends the <code>String</code> ">> " to the message, so
		/// taht if a program were to call (when debug mode was enabled):
		/// <code><pre>
		/// com.dalsemi.onewire.debug.Debug.debug("Some notification...", myBytes);
		/// </pre></code>
		/// the resulting output would look like:
		/// <code><pre>
		/// >> my label
		/// >>   FF F1 F2 F3 F4 F5 F6 FF
		/// </pre></code>
		/// 
		/// </summary>
		/// <param name="lbl">the message to print out above the array
		/// </param>
		/// <param name="bytes">the byte array to print out
		/// </param>
		public static void  debug(System.String lbl, byte[] bytes)
		{
			if (DEBUG)
				debug(lbl, bytes, 0, bytes.Length);
		}
		
		/// <summary> Prints the specified array of bytes with a given label
		/// if debug mode is enabled.  This method calls
		/// <code>PrintStream.println(String)</code>,
		/// and pre-pends the <code>String</code> ">> " to the message, so
		/// taht if a program were to call (when debug mode was enabled):
		/// <code><pre>
		/// com.dalsemi.onewire.debug.Debug.debug("Some notification...", myBytes, 0, 8);
		/// </pre></code>
		/// the resulting output would look like:
		/// <code><pre>
		/// >> my label
		/// >>   FF F1 F2 F3 F4 F5 F6 FF
		/// </pre></code>
		/// 
		/// </summary>
		/// <param name="lbl">the message to print out above the array
		/// </param>
		/// <param name="bytes">the byte array to print out
		/// </param>
		/// <param name="offset">the offset to start printing from the array
		/// </param>
		/// <param name="length">the number of bytes to print from the array
		/// </param>
		public static void  debug(System.String lbl, byte[] bytes, int offset, int length)
		{
			if (DEBUG)
			{
				out_Renamed.Write(">> " + lbl + ", offset=" + offset + ", length=" + length);
				length += offset;
				int inc = 8;
				bool printHead = true;
				for (int i = offset; i < length; i += inc)
				{
					if (printHead)
					{
						out_Renamed.WriteLine();
						out_Renamed.Write(">>    ");
					}
					else
					{
						out_Renamed.Write(" : ");
					}
					int len = System.Math.Min(inc, length - i);
					out_Renamed.Write(Convert.toHexString(bytes, i, len, " "));
					printHead = !printHead;
				}
				out_Renamed.WriteLine();
			}
		}
		
		/// <summary> Prints the specified exception with a given label
		/// if debug mode is enabled.  This method calls
		/// <code>PrintStream.println(String)</code>,
		/// and pre-pends the <code>String</code> ">> " to the message, so
		/// taht if a program were to call (when debug mode was enabled):
		/// <code><pre>
		/// com.dalsemi.onewire.debug.Debug.debug("Some notification...", exception);
		/// </pre></code>
		/// the resulting output would look like:
		/// <code><pre>
		/// >> my label
		/// >>   OneWireIOException: Device Not Present
		/// </pre></code>
		/// 
		/// </summary>
		/// <param name="lbl">the message to print out above the array
		/// </param>
		/// <param name="bytes">the byte array to print out
		/// </param>
		/// <param name="offset">the offset to start printing from the array
		/// </param>
		/// <param name="length">the number of bytes to print from the array
		/// </param>
		//UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
		public static void  debug(System.String lbl, System.Exception t)
		{
			if (DEBUG)
			{
				out_Renamed.WriteLine(">> " + lbl);
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				out_Renamed.WriteLine(">>    " + t.Message);
				SupportClass.WriteStackTrace(t, out_Renamed);
			}
		}
		
		/// <summary> Prints out an exception stack trace for debugging purposes.
		/// This is useful to figure out which functions are calling
		/// a particular function at runtime.
		/// 
		/// </summary>
		public static void  stackTrace()
		{
			if (DEBUG)
			{
				try
				{
					throw new System.Exception("DEBUG STACK TRACE");
				}
				catch (System.Exception e)
				{
					SupportClass.WriteStackTrace(e, out_Renamed);
				}
			}
		}
		/// <summary> Static constructor.  Checks system properties to see if debugging
		/// is enabled by default.  Also, will redirect debug output to a log
		/// file if specified.
		/// </summary>
        /// 
       /*
		static Debug()
		{
			//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.PrintStream' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
			System.IO.StreamWriter temp_writer;
			temp_writer = new System.IO.StreamWriter(System.Console.OpenStandardOutput(), System.Console.Out.Encoding);
			temp_writer.AutoFlush = true;
			out_Renamed = temp_writer;
			{
				System.String enable = OneWireAccessProvider.getProperty("onewire.debug");
				if (enable != null && enable.ToLower().Equals("true"))
					DEBUG = true;
				else
					DEBUG = false;
				
				if (DEBUG)
				{
					System.String logFile = OneWireAccessProvider.getProperty("onewire.debug.logfile");
					if (logFile != null)
					{
						try
						{
							//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.PrintStream' and 'System.IO.StreamWriter' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
							//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
							out_Renamed = new System.IO.StreamWriter(new System.IO.FileStream(logFile, System.IO.FileMode.Create));
						}
						catch (System.Exception e)
						{
							System.IO.StreamWriter temp_writer;
							temp_writer = new System.IO.StreamWriter(System.Console.OpenStandardOutput(), System.Console.Out.Encoding);
							temp_writer.AutoFlush = true;
							out_Renamed = temp_writer;
							debug("Error in Debug Static Constructor", e);
						}
					}
				}
			}
		}
        */
	}
}