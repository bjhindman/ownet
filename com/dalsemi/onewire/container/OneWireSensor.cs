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
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> <P>1-Wire sensor interface class for basic sensor operations. </P>
	/// 
	/// <P> Typically the operations of 1-Wire Sensors are memory mapped so
	/// writing to a particular location causes the state to change.  To
	/// accommodate this type of architecture and reduce the number of 1-
	/// Wire operations that need to take place, a 'read-modify-write'
	/// technique is used.  Each Sensor interface is derived from this
	/// super-interface that contain just two methods: 
	/// {@link #readDevice() readDevice}, 
	/// {@link #writeDevice(byte[]) writeDevice}.  The
	/// read returns a byte array and the write takes a byte array.  The
	/// byte array is the state of the device.  The interfaces that
	/// extend this interface have 'get' and 'set' methods that
	/// manipulate the byte array.  So a OneWireSensor operation is: </P>
	/// 
	/// <OL>
	/// <LI> state = readDevice()
	/// <LI> 'get' and 'set' methods on state
	/// <LI> writeDevice(state)
	/// </OL>
	/// 
	/// <H3> Usage </H3> 
	/// 
	/// <DL> 
	/// <DD> <H4> Example 1</H4> 
	/// Read the sensed level of a 
	/// {@link com.dalsemi.onewire.container.SwitchContainer SwitchContainer} instance 'sw': 
	/// <PRE> <CODE>
	/// byte[] state = sw.readDevice();
	/// if (sw.hasLevelSensing())
	/// {
	/// for (int ch = 0; ch < sw.getNumberChannels(state); ch++)
	/// {
	/// System.out.print("Level for channel " + ch + " is : ");
	/// if (sw.getLevel(ch, state))
	/// System.out.println("HIGH");
	/// else
	/// System.out.println("HIGH");
	/// }
	/// }
	/// else
	/// System.out.println("This SwitchContainer can not sense level");
	/// </CODE> </PRE>
	/// 
	/// <DD> <H4> Example 2</H4> 
	/// Set the clock of a 
	/// {@link com.dalsemi.onewire.container.ClockContainer ClockContainer} instance 'cl': 
	/// <PRE> <CODE>
	/// byte[] state = cl.readDevice();
	/// cl.setClock((new Date().getTime()), state);
	/// cl.setClockRunEnable(true, state);
	/// cl.writeDevice(state);
	/// </CODE> </PRE>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.ADContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.ClockContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.PotentiometerContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.SwitchContainer">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.TemperatureContainer">
	/// 
	/// </seealso>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public interface OneWireSensor
	{
		
		//--------
		//-------- Sensor I/O methods
		//--------
		
		/// <summary> Retrieves the 1-Wire device sensor state.  This state is
		/// returned as a byte array.  Pass this byte array to the 'get'
		/// and 'set' methods.  If the device state needs to be changed then call
		/// the 'writeDevice' to finalize the changes.
		/// 
		/// </summary>
		/// <returns> 1-Wire device sensor state    
		/// 
		/// </returns>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as  </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to 
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		byte[] readDevice();
		
		/// <summary> Writes the 1-Wire device sensor state that
		/// have been changed by 'set' methods.  Only the state registers that
		/// changed are updated.  This is done by referencing a field information
		/// appended to the state data.
		/// 
		/// </summary>
		/// <param name="state">1-Wire device sensor state
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as  </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to 
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire  </throws>
		/// <summary>         adapter
		/// </summary>
		void  writeDevice(byte[] state);
	}
}