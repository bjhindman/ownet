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
	
	
	/// <summary> 1-Wire&reg; Potentiometer interface class for basic potentiometer operations.
	/// This class should be implemented for each potentiometer type
	/// 1-Wire device.
	/// 
	/// Currently there is only the DS2890, but it appears that plans have
	/// been made for more complex parts with more wipers, different
	/// possible number of wiper positions, etc.
	/// 
	/// <P>The PotentiometerContainer methods can be organized into the following categories: </P>
	/// <UL>
	/// <LI> <B> Information </B>
	/// <UL>
	/// <LI> {@link #isLinear                  isLinear}
	/// <LI> {@link #wiperSettingsAreVolatile  wiperSettingsAreVolatile}
	/// <LI> {@link #numberOfPotentiometers    numberOfPotentiometers}
	/// <LI> {@link #numberOfWiperSettings     numberOfWiperSettings}
	/// <LI> {@link #potentiometerResistance   potentiometerResistance}
	/// <LI> {@link #getCurrentWiperNumber     getCurrentWiperNumber}
	/// <LI> {@link #getWiperPosition          getWiperPosition}
	/// <LI> {@link #isChargePumpOn            isChargePumpOn}
	/// </UL>
	/// <LI> <B> Options </B>
	/// <UL>
	/// <LI> {@link #setCurrentWiperNumber     setCurrentWiperNumber}
	/// <LI> {@link #setChargePump             setChargePump}
	/// <LI> {@link #setWiperPosition          setWiperPosition}
	/// <LI> {@link #increment                 increment}, {@link #increment() increment}
	/// <LI> {@link #decrement                 decrement}, {@link #decrement() decrement}
	/// </UL>
	/// <LI> <B> I/O </B>
	/// <UL>
	/// <LI> {@link #readDevice  readDevice}
	/// <LI> {@link #writeDevice writeDevice}
	/// </UL>
	/// </UL>
	/// 
	/// <H3> Usage </H3>
	/// 
	/// Display some features of PotentiometerContainer instance '<code>pc</code>':
	/// <PRE> <CODE>
	/// byte[] state = pc.readDevice();
	/// if (!(pc.isChargePumpOn()))
	/// pc.setChargePump(true, state);
	/// pc.writeDevice(state);
	/// pc.setWiperPosition(127);
	/// pc.increment();
	/// pc.decrement();
	/// </CODE> </PRE>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.OneWireContainer2C">
	/// </seealso>
	/// <seealso cref="OneWireSensor">
	/// </seealso>
	/// <seealso cref="ClockContainer">
	/// </seealso>
	/// <seealso cref="TemperatureContainer">
	/// </seealso>
	/// <seealso cref="SwitchContainer">
	/// 
	/// </seealso>
	/// <version>     0.00, 31 August 2000
	/// </version>
	/// <author>      KLA
	/// 
	/// </author>
	public interface PotentiometerContainer:OneWireSensor
	{
		
		//--------
		//-------- Potentiometer Feature methods
		//--------
		
		/// <summary> Querys to see if this Potentiometer 1-Wire Device
		/// has linear potentiometer element(s) or logarithmic
		/// potentiometer element(s).
		/// 
		/// </summary>
		/// <param name="state">state buffer of the Potentiometer 1-Wire Device (returned by <CODE>readDevice()</CODE>)
		/// </param>
		/// <returns> <CODE>true</CODE> if this device has linear potentiometer element(s),
		/// <CODE>false</CODE> if this device has logarithmic potentiometer element(s)
		/// </returns>
		bool isLinear(byte[] state);
		
		/// <summary> Querys to see if this Potentiometer 1-Wire Device's
		/// wiper settings are volatile or non-volatile.
		/// 
		/// </summary>
		/// <param name="state">state buffer of the Potentiometer 1-Wire Device (returned by <CODE>readDevice()</CODE>)
		/// </param>
		/// <returns> <CODE>true</CODE> if the wiper settings are volatile,
		/// <CODE>false</CODE> if the wiper settings are non-volatile.
		/// </returns>
		bool wiperSettingsAreVolatile(byte[] state);
		
		/// <summary> Querys to see how many potentiometers this
		/// Potentiometer 1-Wire Device has.
		/// 
		/// </summary>
		/// <param name="state">state buffer of the Potentiometer 1-Wire Device (returned by <CODE>readDevice()</CODE>)
		/// </param>
		/// <returns> number of potentiometers on this device
		/// </returns>
		int numberOfPotentiometers(byte[] state);
		
		/// <summary> Querys to find the number of wiper settings
		/// that any wiper on this Potentiometer 1-Wire
		/// Device can have.
		/// 
		/// </summary>
		/// <param name="state">state buffer of the Potentiometer 1-Wire Device (returned by <CODE>readDevice()</CODE>)
		/// </param>
		/// <returns> number of wiper positions available
		/// </returns>
		int numberOfWiperSettings(byte[] state);
		
		/// <summary> Querys to find the resistance value of the potentiometer.
		/// 
		/// </summary>
		/// <param name="state">state buffer of the Potentiometer 1-Wire Device (returned by <CODE>readDevice()</CODE>)
		/// </param>
		/// <returns> resistance value in k-Ohms
		/// </returns>
		int potentiometerResistance(byte[] state);
		
		//--------
		//-------- Potentiometer State methods
		//--------
		
		/// <summary> Gets the currently selected wiper number.  All wiper actions
		/// affect this wiper.  The number of wipers is the same as
		/// <CODE>numberOfPotentiometers()</CODE>.
		/// 
		/// </summary>
		/// <param name="state">state buffer of the Potentiometer 1-Wire Device (returned by <CODE>readDevice()</CODE>)
		/// </param>
		/// <returns> current wiper number
		/// </returns>
		int getCurrentWiperNumber(byte[] state);
		
		/// <summary> Sets the currently selected wiper number.  All wiper actions will
		/// then affect this wiper.  The number of wipers is the same as
		/// <CODE>numberOfPotentiometers()</CODE>.
		/// 
		/// </summary>
		/// <param name="wiper_number">wiper number to select for communication
		/// </param>
		/// <param name="state">state buffer of the Potentiometer 1-Wire Device (returned by <CODE>readDevice()</CODE>)
		/// </param>
		void  setCurrentWiperNumber(int wiper_number, byte[] state);
		
		/// <summary> Determines if the Potentiometer's charge pump is enabled.
		/// 
		/// </summary>
		/// <param name="state">state buffer of the Potentiometer 1-Wire Device (returned by <CODE>readDevice()</CODE>)
		/// </param>
		/// <returns> <CODE>true</CODE> if enabled, <CODE>false</CODE> if not
		/// 
		/// </returns>
		bool isChargePumpOn(byte[] state);
		
		/// <summary> Sets the state of the Potentiometer's charge pump.  This decreases the wiper's resistance,
		/// but increases the power consumption by the part.  Vdd must be
		/// connected to use the charge pump.
		/// 
		/// </summary>
		/// <param name="charge_pump_on"><CODE>true</CODE> to enable the charge pump, <CODE>false</CODE> to disable charge pump
		/// </param>
		/// <param name="state">state buffer of the Potentiometer 1-Wire Device (returned by <CODE>readDevice()</CODE>)
		/// </param>
		/// <returns> <CODE>true</CODE> if the operation was successful, <CODE>false</CODE> if there was an error
		/// </returns>
		void  setChargePump(bool charge_pump_on, byte[] state);
		
		//--------
		//-------- Potentiometer Wiper access methods
		//--------
		
		/// <summary> Gets the current wiper position of the Potentiometer.  The wiper position
		/// is between 0 and <CODE>numberOfWiperPositions()</CODE>, and describes the voltage output.
		/// 
		/// </summary>
		/// <returns> wiper position between 0 and <CODE>numberOfWiperPositions()</CODE>
		/// 
		/// </returns>
		/// <exception cref="com.dalsemi.onewire.adapter.OneWireIOException">Data was not read correctly
		/// </exception>
		/// <exception cref="com.dalsemi.onewire.OneWireException">Could not find device
		/// </exception>
		int getWiperPosition();
		
		/// <summary> Sets the wiper position of the potentiometer.
		/// 
		/// </summary>
		/// <param name="position">position to set the wiper to
		/// </param>
		/// <returns> <CODE>true</CODE> if the operation was successful, <CODE>false</CODE> otherwise
		/// 
		/// </returns>
		/// <exception cref="com.dalsemi.onewire.adapter.OneWireIOException">Data was not written correctly
		/// </exception>
		/// <exception cref="com.dalsemi.onewire.OneWireException">Could not find device
		/// </exception>
		bool setWiperPosition(int position);
		
		/// <summary> Increments the wiper position by one.
		/// 
		/// </summary>
		/// <param name="reselect"><CODE>increment()</CODE> can be called without resetting
		/// the part if the last call was an <CODE>increment()</CODE> or <CODE>decrement()</CODE>.
		/// <CODE>true</CODE> if device is to be selected (must be called with <CODE>true</CODE>
		/// after any other 1-wire method)
		/// 
		/// </param>
		/// <returns> new position of wiper (0 to <CODE>numberOfWiperPositions()</CODE>)
		/// 
		/// </returns>
		/// <exception cref="com.dalsemi.onewire.adapter.OneWireIOException">Data was not written correctly
		/// </exception>
		/// <exception cref="com.dalsemi.onewire.OneWireException">Could not find device
		/// </exception>
		int increment(bool reselect);
		
		/// <summary> Decrements the wiper position.
		/// 
		/// </summary>
		/// <param name="reselect"><CODE>decrement()</CODE> can be called without resetting
		/// the part if the last call was an <CODE>increment()</CODE> or <CODE>decrement()</CODE>.
		/// <CODE>true</CODE> if device is to be selected (must be called with <CODE>true</CODE>
		/// after any other 1-wire method)
		/// 
		/// </param>
		/// <returns> new position of wiper (0 to <CODE>numberOfWiperPositions()</CODE>).
		/// 
		/// </returns>
		/// <exception cref="com.dalsemi.onewire.adapter.OneWireIOException">Data was not written correctly
		/// </exception>
		/// <exception cref="com.dalsemi.onewire.OneWireException">Could not find device
		/// </exception>
		int decrement(bool reselect);
		
		/// <summary> Increments the wiper position after selecting the part.
		/// 
		/// </summary>
		/// <returns> new position of wiper (0 to <CODE>numberOfWiperPositions()</CODE>)
		/// 
		/// </returns>
		/// <exception cref="com.dalsemi.onewire.adapter.OneWireIOException">Data was not written correctly
		/// </exception>
		/// <exception cref="com.dalsemi.onewire.OneWireException">Could not find device
		/// </exception>
		int increment();
		
		/// <summary> Decrements the wiper position after selecting the part.
		/// 
		/// </summary>
		/// <returns> new position of wiper (0 to <CODE>numberOfWiperPositions()</CODE>)
		/// 
		/// </returns>
		/// <exception cref="com.dalsemi.onewire.adapter.OneWireIOException">Data was not written correctly
		/// </exception>
		/// <exception cref="com.dalsemi.onewire.OneWireException">Could not find device
		/// </exception>
		int decrement();
	}
}