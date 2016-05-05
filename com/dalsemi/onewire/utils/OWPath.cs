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
using OneWireException = com.dalsemi.onewire.OneWireException;
using OneWireContainer = com.dalsemi.onewire.container.OneWireContainer;
using SwitchContainer = com.dalsemi.onewire.container.SwitchContainer;
using DSPortAdapter = com.dalsemi.onewire.adapter.DSPortAdapter;
using OneWireIOException = com.dalsemi.onewire.adapter.OneWireIOException;
namespace com.dalsemi.onewire.utils
{
	
	
	/// <summary> 1-Wire&reg; Network path.  Large 1-Wire networks can be sub-divided into branches
	/// for load, location, or organizational reasons.  Once 1-Wire devices are placed
	/// on this branches there needs to be a mechanism to reach these devices.  The
	/// OWPath class was designed to provide a convenient method to open and close
	/// 1-Wire paths to reach remote devices.
	/// 
	/// <H3> Usage </H3>
	/// 
	/// <DL>
	/// <DD> <H4> Example</H4>
	/// Open the path 'path' to the 1-Wire temperature device 'tc' and read the temperature:
	/// <PRE> <CODE>
	/// // open a path to the temp device
	/// path.open();
	/// 
	/// // read the temp device
	/// byte[] state = tc.readDevice();
	/// tc.doTemperatureConvert(state);
	/// state = tc.readDevice();
	/// System.out.println("Temperature of " +
	/// address + " is " +
	/// tc.getTemperature(state) + " C");
	/// 
	/// // close the path to the device
	/// path.close();
	/// </CODE> </PRE>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.utils.OWPathElement">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.SwitchContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer05">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer12">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer1F">
	/// 
	/// </seealso>
	/// <version>     0.00, 12 September 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public class OWPath
	{
		/// <summary> Get an enumeration of all of the 1-Wire path elements in
		/// this 1-Wire path.
		/// 
		/// </summary>
		/// <returns> enumeration of all of the 1-Wire path elements
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.utils.OWPathElement">
		/// </seealso>
		virtual public System.Collections.IEnumerator AllOWPathElements
		{
			get
			{
				return elements.GetEnumerator();
			}
			
		}
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary>Elements of the path in a Vector </summary>
		private System.Collections.ArrayList elements;
		
		/// <summary>Adapter where this path is based </summary>
		private DSPortAdapter adapter;
		
		//--------
		//-------- Constructor
		//--------
		
		/// <summary> Create a new 1-Wire path with no elemements.  Elements
		/// can be added by using <CODE> copy </CODE> and/or
		/// <CODE> add </CODE>.
		/// 
		/// </summary>
		/// <param name="adapter">where the path is based
		/// 
		/// </param>
		/// <seealso cref="copy(OWPath) copy">
		/// </seealso>
		/// <seealso cref="add(OneWireContainer, int) add">
		/// </seealso>
		public OWPath(DSPortAdapter adapter)
		{
			this.adapter = adapter;
			elements = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(2));
		}
		
		/// <summary> Create a new path with a starting path.  New elements
		/// can be added with <CODE>add</CODE>.
		/// 
		/// </summary>
		/// <param name="adapter">where the 1-Wire path is based
		/// </param>
		/// <param name="currentPath">starting value of this 1-Wire path
		/// 
		/// </param>
		/// <seealso cref="add(OneWireContainer, int) add">
		/// </seealso>
		public OWPath(DSPortAdapter adapter, OWPath currentOWPath)
		{
			this.adapter = adapter;
			elements = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(2));
			
			copy(currentOWPath);
		}
		
		/// <summary> Copy the elements from the provided 1-Wire path into this 1-Wire path.
		/// 
		/// </summary>
		/// <param name="currentOWPath">path to copy from
		/// </param>
		public virtual void  copy(OWPath currentOWPath)
		{
			elements.Clear();
			
			if (currentOWPath != null)
			{
				
				// enumerature through elements in current path
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				for (System.Collections.IEnumerator path_enum = currentOWPath.AllOWPathElements; path_enum.MoveNext(); )
				{
					
					// cast the enum as a OWPathElements and add to vector
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					elements.Add((OWPathElement) path_enum.Current);
				}
			}
		}
		
		/// <summary> Add a 1-Wire path element to this 1-Wire path.
		/// 
		/// </summary>
		/// <param name="owc">1-Wire device switch
		/// </param>
		/// <param name="channel">of device that represents this 1-Wire path element
		/// 
		/// </param>
		/// <seealso cref="copy(OWPath) copy">
		/// </seealso>
		public virtual void  add(OneWireContainer owc, int channel)
		{
			elements.Add(new OWPathElement(owc, channel));
		}
		
		/// <summary> Compare this 1-Wire path with another.
		/// 
		/// </summary>
		/// <param name="compareOWPath">1-Wire path to compare to
		/// 
		/// </param>
		/// <returns> <CODE> true </CODE> if the 1-Wire paths are the same
		/// </returns>
		public bool equals(OWPath compareOWPath)
		{
			return (this.ToString().Equals(compareOWPath.ToString()));
		}
		
		/// <summary> Get a string representation of this 1-Wire path.
		/// 
		/// </summary>
		/// <returns> string 1-Wire path as string
		/// </returns>
		public override System.String ToString()
		{
			System.String st = new System.Text.StringBuilder("").ToString();
			OWPathElement element;
			OneWireContainer owc;
			
			// append 'drive'
			try
			{
				st = adapter.AdapterName + "_" + adapter.PortName + "/";
			}
			catch (OneWireException e)
			{
				st = adapter.AdapterName + "/";
			}
			
			for (int i = 0; i < elements.Count; i++)
			{
				element = (OWPathElement) elements[i];
				owc = element.Container;
				
				// append 'directory' name
				st += (owc.AddressAsString + "_" + element.Channel + "/");
			}
			
			return st;
		}
		
		/// <summary> Open this 1-Wire path so that a remote device can be accessed.
		/// 
		/// </summary>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         no device present or a CRC read from the device is incorrect.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter.
		/// </summary>
		public virtual void  open()
		{
			OWPathElement path_element;
			SwitchContainer sw;
			byte[] sw_state;
			
			// enumerature through elements in path
			for (int i = 0; i < elements.Count; i++)
			{
				
				// cast the enum as a OWPathElement
				path_element = (OWPathElement) elements[i];
				
				// get the switch
				sw = (SwitchContainer) path_element.Container;
				
				// turn on the elements channel
				sw_state = sw.readDevice();
				
				sw.setLatchState(path_element.Channel, true, sw.hasSmartOn(), sw_state);
				sw.writeDevice(sw_state);
			}
			
			// check if not depth in path, do a reset so a resetless search will work
			if (elements.Count == 0)
			{
				adapter.reset();
			}
		}
		
		/// <summary> Close each element in this 1-Wire path in reverse order.
		/// 
		/// </summary>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         no device present or a CRC read from the device is incorrect.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter.
		/// </summary>
		public virtual void  close()
		{
			OWPathElement path_element;
			SwitchContainer sw;
			byte[] sw_state;
			
			// loop through elements in path in reverse order
			for (int i = elements.Count - 1; i >= 0; i--)
			{
				
				// cast the element as a OWPathElement
				path_element = (OWPathElement) elements[i];
				
				// get the switch
				sw = (SwitchContainer) path_element.Container;
				
				// turn off the elements channel
				sw_state = sw.readDevice();
				sw.setLatchState(path_element.Channel, false, false, sw_state);
				sw.writeDevice(sw_state);
			}
		}
	}
}