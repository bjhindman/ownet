/*---------------------------------------------------------------------------
* Copyright (C) 1999 - 2007 Maxim Integrated Products, All Rights Reserved.
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

// OneWireAccessProvider.java
// imports
using System;
using com.dalsemi.onewire.adapter;
namespace com.dalsemi.onewire
{
		
	/// <summary> The OneWireAccessProvider class manages the Maxim
	/// adapter class derivatives of <code>DSPortAdapter</code>.  An enumeration of all
	/// available adapters can be accessed through the
	/// member function <code>EnumerateAllAdapters</code>.  This enables an
	/// application to be adapter independent. There are also facilities to get a system
	/// appropriate default adapter/port combination.<p>
	/// 
	/// <H3> Usage </H3>
	/// 
	/// <DL>
	/// <DD> <H4> Example 1</H4>
	/// Get an instance of the default 1-Wire adapter.  The adapter will be ready
	/// to use if no exceptions are thrown.
	/// <PRE> <CODE>
	/// try
	/// {
	/// DSPortAdapter adapter = OneWireAccessProvider.getDefaultAdapter();
	/// 
	/// System.out.println("Adapter: " + adapter.getAdapterName() + " Port: " + adapter.getPortName());
	/// 
	/// // use the adapter ...
	/// 
	/// }
	/// catch(Exception e)
	/// {
	/// System.out.println("Default adapter not present: " + e);
	/// }
	/// </CODE> </PRE>
	/// </DL>
	/// 
	/// <DL>
	/// <DD> <H4> Example 2</H4>
	/// Enumerate through the available adapters and ports.
	/// <PRE> <CODE>
	/// DSPortAdapter adapter;
	/// String        port;
	/// 
	/// // get the adapters
	/// for (Enumeration adapter_enum = OneWireAccessProvider.enumerateAllAdapters();
	/// adapter_enum.hasMoreElements(); )
	/// {
	/// // cast the enum as a DSPortAdapter
	/// adapter = ( DSPortAdapter ) adapter_enum.nextElement();
	/// 
	/// System.out.print("Adapter: " + adapter.getAdapterName() + " with ports: ");
	/// 
	/// // get the ports
	/// for (Enumeration port_enum = adapter.getPortNames();
	/// port_enum.hasMoreElements(); )
	/// {
	/// // cast the enum as a String
	/// port = ( String ) port_enum.nextElement();
	/// 
	/// System.out.print(port + " ");
	/// }
	/// 
	/// System.out.println();
	/// }
	/// </CODE> </PRE>
	/// </DL>
	/// 
	/// <DL>
	/// <DD> <H4> Example 3</H4>
	/// Display the default adapter name and port without getting an instance of the adapter.
	/// <PRE> <CODE>
	/// System.out.println("Default Adapter: " +
	/// OneWireAccessProvider.getProperty("onewire.adapter.default"));
	/// System.out.println("Default Port: " +
	/// OneWireAccessProvider.getProperty("onewire.port.default"));
	/// </CODE> </PRE>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.adapter.DSPortAdapter">
	/// 
	/// </seealso>
	/// <version>     0.00, 30 August 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public class OneWireAccessProvider
	{
		/// <summary> Returns a version string, representing the release number on official releases,
		/// or release number and release date on incrememental releases.
		/// 
		/// </summary>
		/// <returns> Current OneWireAPI version
		/// </returns>
		public static System.String Version
		{
			get
			{
				return owapi_version;
			}
			
		}
		/// <summary> Finds, opens, and verifies the default adapter and
		/// port.  Looks for the default adapter/port in the following locations:
		/// <p>
		/// <ul>
		/// <li> Use adapter/port in System.properties for onewire.adapter.default,
		/// and onewire.port.default properties tags.</li>
		/// <li> Use adapter/port from onewire.properties file in current directory
		/// or < java.home >/lib/ (Desktop) or /etc/ (TINI)</li>
		/// <li> Use smart default
		/// <ul>
		/// <li> Desktop
		/// <ul>
		/// <li> First, TMEX default (Win32 only)
		/// <li> Second, if TMEX not present, then DS9097U/(first serial port)
		/// </ul>
		/// <li> TINI, TINIExternalAdapter on port serial1
		/// </ul>
		/// </ul>
		/// 
		/// </summary>
		/// <returns>  <code>DSPortAdapter</code> if default adapter present
		/// 
		/// </returns>
		/// <throws>  OneWireIOException when communcation with the adapter fails </throws>
		/// <throws>  OneWireException when the port or adapter not present </throws>
		public static DSPortAdapter DefaultAdapter
		{
			get
			{
				if (useOverrideAdapter)
				{
					return overrideAdapter;
				}
				
				return getAdapter(getProperty("onewire.adapter.default"), getProperty("onewire.port.default"));
                //return getAdapter("{DS9097U}", "COM1");
			}
			
		}
		/// <summary> Sets an overriding adapter.  This adapter will be returned from
		/// getAdapter and getDefaultAdapter despite what was requested.
		/// 
		/// </summary>
		/// <param name="adapter">adapter to be the override
		/// 
		/// </param>
		/// <seealso cref="getAdapter">
		/// </seealso>
		/// <seealso cref="getDefaultAdapter">
		/// </seealso>
		/// <seealso cref="clearUseOverridingAdapter">
		/// </seealso>
		public static DSPortAdapter UseOverridingAdapter
		{
			set
			{
				useOverrideAdapter = true;
				overrideAdapter = value;
			}
			
		}
		
		/// <summary> Smart default port</summary>
		//private static System.String smartDefaultPort = "COM1"; // not used
		
		/// <summary> Override adapter variables</summary>
		private static bool useOverrideAdapter = false;
		private static DSPortAdapter overrideAdapter = null;
		
		/// <summary> System Version String</summary>
		private const System.String owapi_version = "@AUTOVERSION@";
		
		/// <summary> Don't allow anyone to instantiate.</summary>
		private OneWireAccessProvider()
		{
		}

        //public static System.Collections.IList enumerateAllAdapters()
        public static AdapterEnumerator enumerateAllAdapters()
        {
           //System.Collections.IList il;
           
           return ((AdapterEnumerator)GetAllAdapters());
        }
		
		/// <summary> Gets a list of all 1-Wire
		/// adapter types supported.  A search can be done 
        /// afterwards to find all available hardware adapters.
		/// 
		/// </summary>
		/// <returns>  <code>Enumeration</code> of <code>DSPortAdapters</code> in the system
		/// </returns>
        public static AdapterEnumerator GetAllAdapters()
        {
           System.Collections.ArrayList al = new System.Collections.ArrayList();
           com.dalsemi.onewire.adapter.AdapterEnumerator adapterArray;
           /*
           try
           {
              al.Add(GetAppropriateSerialAdapter());
           }
           catch { }
           */
		   // loop through the TMEX adapters
           for (int port_type = 0; port_type <= 15; port_type++)
           {

              try
              {
                 if (IntPtr.Size == 4)
                    al.Add(new DotNetAdapterx86(port_type));
                 else
                    al.Add(new DotNetAdapterx64(port_type));
              }
              catch { }
           }
           adapterArray = new com.dalsemi.onewire.adapter.AdapterEnumerator(al);
           return adapterArray;
        }
		

		/// <summary> Finds, opens, and verifies the specified adapter on the
		/// indicated port.
		/// 
		/// </summary>
		/// <param name="adapterName">string name of the adapter (match to result
		/// of call to getAdapterName() method in DSPortAdapter)
		/// </param>
		/// <param name="portName">string name of the port used in the method
		/// selectPort() in DSPortAdapter
		/// 
		/// </param>
		/// <returns>  <code>DSPortAdapter</code> if adapter present
		/// 
		/// </returns>
		/// <throws>  OneWireIOException when communcation with the adapter fails </throws>
		/// <throws>  OneWireException when the port or adapter not present </throws>
		public static DSPortAdapter getAdapter(System.String adapterName, System.String portName)
		{
			DSPortAdapter adapter, found_adapter = null;
			
			// check for override
			if (useOverrideAdapter)
				return overrideAdapter;
			
			// enumerature through available adapters to find the correct one
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator adapter_enum = (System.Collections.IEnumerator) enumerateAllAdapters(); adapter_enum.MoveNext(); )
			{
				// cast the enum as a DSPortAdapter
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				adapter = (DSPortAdapter) adapter_enum.Current;
				
				// see if this is the type of adapter we want
				if ((found_adapter != null) || (!adapter.AdapterName.Equals(adapterName)))
				{
					// not this adapter, then just cleanup
					try
					{
						adapter.freePort();
					}
					catch (System.Exception e)
					{
						// DRAIN
					}
					continue;
				}
				
				// attempt to open and verify the adapter
				if (adapter.selectPort(portName))
				{
					adapter.beginExclusive(true);
					
					try
					{
						// check for the adapter
						if (adapter.adapterDetected())
							found_adapter = adapter;
						else
						{
							
							// close the port just opened
							adapter.freePort();
							
							throw new OneWireException("Port found \"" + portName + "\" but Adapter \"" + adapterName + "\" not detected");
						}
					}
					finally
					{
						adapter.endExclusive();
					}
				}
				else
					throw new OneWireException("Specified port \"" + portName + "\" could not be selected for adapter \"" + adapterName + "\"");
			}
			
			// if adapter found then return it
            if (found_adapter != null)
            {
               
               return found_adapter;
            }
			// adapter by that name not found
			throw new OneWireException("Specified adapter name \"" + adapterName + "\" is not known");
		}

        /// <summary> Gets the specfied ownet.dll property.
        /// Looks for the property in the following locations:
        /// <p>
        /// <ul>
        /// <li> In ownet.properties file in current directory
        /// <li> 'smart' default if property is 'onewire.adapter.default'
        /// or 'onewire.port.default'
        /// </ul>
        /// 
        /// </summary>
        /// <param name="propName">string name of the property to read
        /// 
        /// </param>
        /// <returns>  <code>String</code> representing the property value or <code>null</code> if
        /// it could not be found (<code>onewire.adapter.default</code> and
        /// <code>onewire.port.default</code> may
        /// return a 'smart' default even if property not present)
        /// </returns>
        public static System.String getProperty(System.String propName)
        {
           try
           {
              if (useOverrideAdapter)
              {
                 if (propName.Equals("onewire.adapter.default"))
                    return overrideAdapter.AdapterName;
                 if (propName.Equals("onewire.port.default"))
                    return overrideAdapter.PortName;
              }
           }
           catch (System.Exception e)
           {
              //just drain it and let the normal method run...
           }

           //UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
           //UPGRADE_TODO: Format of property file may need to be changed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1089'"
           Properties ownet_properties;
           System.IO.FileStream prop_file = null;
           System.String ret_str = null;
           DSPortAdapter adapter_instance;
           System.Type adapter_class;

           /* system properties does not exist in .NET
           // try system properties
           try
           {
              //UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
              ret_str = System_Renamed.getProperty(propName, null);
           }
           catch (System.Exception e)
           {
              ret_str = null;
           }
            */

           // if defaults not found then try ownet.properties file
           if (ret_str == null)
           {

              // loop to attempt to open the onewire.properties file in following location
              // .\ownet.properties 
              
              System.String path = new System.Text.StringBuilder("").ToString();

              for (int i = 0; i <= 1; i++)
              {

                 // attempt to open the ownet.properties file
                 try
                 {
                    //UPGRADE_TODO: Constructor 'java.io.FileInputStream.FileInputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileInputStreamFileInputStream_javalangString'"
                    prop_file = new System.IO.FileStream(path + "ownet.properties", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                 }
                 catch (System.Exception e)
                 {
                    prop_file = null;
                 }

                 // if open, then try to read value
                 if (prop_file != null)
                 {

                    // attempt to read the ownet.properties
                    try
                    {
                       ownet_properties = new Properties(path + "ownet.properties");

                       ret_str = ownet_properties.get(propName, null);

                       //UPGRADE_TODO: Method 'java.util.Properties.load' was converted to 'System.Collections.Specialized.NameValueCollection' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilPropertiesload_javaioInputStream'"
                      
                       //ownet_properties.reload(
                       //ownet_properties = new System.Collections.Specialized.NameValueCollection(System.Configuration.ConfigurationSettings.AppSettings);
                       ret_str = ownet_properties.get(propName, null);
                       //ret_str = ownet_properties.getProperty(propName, null);
                       //ret_str = onewire_properties[propName] == null ? null : onewire_properties[propName];
                    }
                    catch (System.Exception e)
                    {
                       ret_str = null;
                    }
                 }

                 // check to see if we now have the value
                 if (ret_str != null)
                    break;

                 // try the second path
                 //UPGRADE_ISSUE: Method 'java.lang.System.getProperty' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangSystem'"
                 //path = System_Renamed.getProperty("java.home") + System.IO.Path.DirectorySeparatorChar.ToString() + "lib" + System.IO.Path.DirectorySeparatorChar.ToString();
              }
           }

           // if defaults still not found then check DotNet default
           if (ret_str == null)
           {
              try
              {
                 if (propName.Equals("onewire.adapter.default"))
                 {
                    //UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.GetEnvironmentVariable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
                    if (IntPtr.Size == 4)
                       ret_str = DotNetAdapterx86.DefaultAdapterName;
                    else
                       ret_str = DotNetAdapterx64.DefaultAdapterName;
                 }
                 else if (propName.Equals("onewire.port.default"))
                 {
                    //UPGRADE_TODO: Method 'java.lang.System.getProperty' was converted to 'System.Environment.GetEnvironmentVariable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangSystemgetProperty_javalangString'"
                    if (IntPtr.Size == 4)
                       ret_str = DotNetAdapterx86.DefaultPortName;
                    else
                       ret_str = DotNetAdapterx64.DefaultPortName;
                 }

                 // if did not get real string then null out
                 if (ret_str != null)
                 {
                    if (ret_str.Length <= 0)
                       ret_str = null;
                 }
              }
              catch (System.Exception e)
              {
                 // DRAIN
              }
              /*
              catch (System.ApplicationException e)
              {
                 // DRAIN
              }
              */
           }
           /*
           // if STILL not found then just pick DS9097U on 'smartDefaultPort'
           if (ret_str == null)
           {
              if (propName.Equals("onewire.adapter.default"))
                 ret_str = "DS9097U";
              else if (propName.Equals("onewire.port.default"))
              {
                 try
                 {
                    //UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                    adapter_class = System.Type.GetType("com.dalsemi.onewire.adapter.USerialAdapter");
                    adapter_instance = (DSPortAdapter)System.Activator.CreateInstance(adapter_class);

                    // check if has any ports (common javax.comm problem)
                    //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                    if (adapter_instance.PortNames.MoveNext())
                    {
                       //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                       ret_str = ((System.String)adapter_instance.PortNames.Current);
                    }
                 }
                 catch (System.Exception e)
                 {
                    // DRAIN
                 }
                 catch (System.ApplicationException e)
                 {
                    // DRAIN
                 }
              }
           }
           */
           return ret_str;
        }

		
		/// <summary> Clears the overriding adapter.  The operation of
		/// getAdapter and getDefaultAdapter will be returned to normal.
		/// 
		/// </summary>
		/// <seealso cref="getAdapter">
		/// </seealso>
		/// <seealso cref="getDefaultAdapter">
		/// </seealso>
		/// <seealso cref="setUseOverridingAdapter">
		/// </seealso>
		public static void  clearUseOverridingAdapter()
		{
			useOverrideAdapter = false;
			overrideAdapter = null;
		}
	}
}