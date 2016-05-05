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
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> <p>Interface class for 1-Wire&reg; devices that contain Real-Time clocks.
	/// This class should be implemented for each Clock type 1-Wire device.</p>
	/// 
	/// <h3> Features </h3>
	/// 
	/// <ul>
	/// <li>
	/// Supports clock alarm enabling and setting on devices with clock alarms
	/// </li>
	/// <li>
	/// Supports enabling and disabling the clock on devices that can disable their oscillators
	/// </li>
	/// </ul>
	/// 
	/// <h3> Usage </h3>
	/// 
	/// <p><code>ClockContainer</code> extends <code>com.dalsemi.onewire.container.OneWireSensor</code>, so the general usage
	/// model applies to any <code>ClockContainer</code>:
	/// <ol>
	/// <li> readDevice()  </li>
	/// <li> perform operations on the <code>ClockContainer</code>  </li>
	/// <li> writeDevice(byte[]) </li>
	/// </ol>
	/// 
	/// <p>Consider this interaction with a <code>ClockContainer</code> that reads from the
	/// Real-Time clock, then tries to set it to the system's current clock setting before
	/// disabling the oscillator:
	/// 
	/// <pre><code>
	/// //clockcontainer is a com.dalsemi.onewire.container.ClockContainer
	/// byte[] state = clockcontainer.readDevice();
	/// long current_time = clockcontainer.getClock(state);
	/// System.out.println("Current time is :"+(new Date(current_time)));
	/// 
	/// long system_time = System.currentTimeMillis();
	/// clockcontainer.setClock(system_time,state);
	/// clockcontainer.writeDevice(state);
	/// 
	/// //now try to disable to clock oscillator
	/// if (clockcontainer.canDisableClock())
	/// {
	/// state = clockcontainer.readDevice();
	/// clockcontainer.setClockRunEnable(false,state);
	/// clockcontainer.writeDevice(state);
	/// }
	/// 
	/// </code></pre>
	/// 
	/// </summary>
	/// <seealso cref="OneWireSensor">
	/// </seealso>
	/// <seealso cref="ADContainer">
	/// </seealso>
	/// <seealso cref="TemperatureContainer">
	/// </seealso>
	/// <seealso cref="PotentiometerContainer">
	/// </seealso>
	/// <seealso cref="SwitchContainer">
	/// 
	/// </seealso>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS, KLA
	/// </author>
	public interface ClockContainer:OneWireSensor
	{
		/// <summary> Gets the clock resolution in milliseconds.
		/// 
		/// </summary>
		/// <returns> the clock resolution in milliseconds
		/// </returns>
		long ClockResolution
		{
			get;
			
		}
		
		//--------
		//-------- Clock Feature methods
		//--------
		
		/// <summary> Checks to see if the clock has an alarm feature.
		/// 
		/// </summary>
		/// <returns> true if the Real-Time clock has an alarm
		/// 
		/// </returns>
		/// <seealso cref="getClockAlarm(byte[])">
		/// </seealso>
		/// <seealso cref="isClockAlarmEnabled(byte[])">
		/// </seealso>
		/// <seealso cref="isClockAlarming(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarm(long,byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarmEnable(boolean,byte[])">
		/// </seealso>
		bool hasClockAlarm();
		
		/// <summary> Checks to see if the clock can be disabled.
		/// 
		/// </summary>
		/// <returns> true if the clock can be enabled and disabled
		/// 
		/// </returns>
		/// <seealso cref="isClockRunning(byte[])">
		/// </seealso>
		/// <seealso cref="setClockRunEnable(boolean,byte[])">
		/// </seealso>
		bool canDisableClock();
		
		//--------
		//-------- Clock 'get' Methods
		//--------
		
		/// <summary> Extracts the Real-Time clock value in milliseconds.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> the time represented in this clock in milliseconds since 1970
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="setClock(long,byte[])">
		/// </seealso>
		long getClock(byte[] state);
		
		/// <summary> Extracts the clock alarm value for the Real-Time clock.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> the set value of the clock alarm in milliseconds since 1970
		/// 
		/// </returns>
		/// <throws>  OneWireException if this device does not have clock alarms </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="hasClockAlarm()">
		/// </seealso>
		/// <seealso cref="isClockAlarmEnabled(byte[])">
		/// </seealso>
		/// <seealso cref="isClockAlarming(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarm(long,byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarmEnable(boolean,byte[])">
		/// </seealso>
		//long getClockAlarm(byte[] state);
        DateTime getClockAlarm(byte[] state);
		
		/// <summary> Checks if the clock alarm flag has been set.
		/// This will occur when the value of the Real-Time
		/// clock equals the value of the clock alarm.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> true if the Real-Time clock is alarming
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="hasClockAlarm()">
		/// </seealso>
		/// <seealso cref="isClockAlarmEnabled(byte[])">
		/// </seealso>
		/// <seealso cref="getClockAlarm(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarm(long,byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarmEnable(boolean,byte[])">
		/// </seealso>
		bool isClockAlarming(byte[] state);
		
		/// <summary> Checks if the clock alarm is enabled.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> true if clock alarm is enabled
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="hasClockAlarm()">
		/// </seealso>
		/// <seealso cref="isClockAlarming(byte[])">
		/// </seealso>
		/// <seealso cref="getClockAlarm(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarm(long,byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarmEnable(boolean,byte[])">
		/// </seealso>
		bool isClockAlarmEnabled(byte[] state);
		
		/// <summary> Checks if the device's oscillator is enabled.  The clock
		/// will not increment if the clock oscillator is not enabled.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> true if the clock is running
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="canDisableClock()">
		/// </seealso>
		/// <seealso cref="setClockRunEnable(boolean,byte[])">
		/// </seealso>
		bool isClockRunning(byte[] state);
		
		//--------
		//-------- Clock 'set' Methods
		//--------
		
		/// <summary> Sets the Real-Time clock.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.
		/// 
		/// </summary>
		/// <param name="time">new value for the Real-Time clock, in milliseconds 
		/// since January 1, 1970
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="getClock(byte[])">
		/// </seealso>
		void  setClock(long time, byte[] state);
		
		/// <summary> Sets the clock alarm.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.  Also note that
		/// not all clock devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasClockAlarm()</code> method.
		/// 
		/// </summary>
		/// <param name="time">- new value for the Real-Time clock alarm, in milliseconds 
		/// since January 1, 1970
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <throws>  OneWireException if this device does not have clock alarms </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="hasClockAlarm()">
		/// </seealso>
		/// <seealso cref="isClockAlarmEnabled(byte[])">
		/// </seealso>
		/// <seealso cref="getClockAlarm(byte[])">
		/// </seealso>
		/// <seealso cref="isClockAlarming(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarmEnable(boolean,byte[])">
		/// </seealso>
		void  setClockAlarm(long time, byte[] state);
		
		/// <summary> Enables or disables the oscillator, turning the clock 'on' and 'off'.
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.  Also note that
		/// not all clock devices can disable their oscillators.  Check to see if this device can
		/// disable its oscillator first by calling the <code>canDisableClock()</code> method.
		/// 
		/// </summary>
		/// <param name="runEnable">true to enable the clock oscillator
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <throws>  OneWireException if the clock oscillator cannot be disabled </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="canDisableClock()">
		/// </seealso>
		/// <seealso cref="isClockRunning(byte[])">
		/// </seealso>
		void  setClockRunEnable(bool runEnable, byte[] state);
		
		/// <summary> Enables or disables the clock alarm. 
		/// The method <code>writeDevice()</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice()</code>.  Also note that
		/// not all clock devices have alarms.  Check to see if this device has
		/// alarms first by calling the <code>hasClockAlarm()</code> method.
		/// 
		/// </summary>
		/// <param name="alarmEnable">true to enable the clock alarm
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <throws>  OneWireException if this device does not have clock alarms </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="hasClockAlarm()">
		/// </seealso>
		/// <seealso cref="isClockAlarmEnabled(byte[])">
		/// </seealso>
		/// <seealso cref="getClockAlarm(byte[])">
		/// </seealso>
		/// <seealso cref="setClockAlarm(long,byte[])">
		/// </seealso>
		/// <seealso cref="isClockAlarming(byte[])">
		/// </seealso>
		void  setClockAlarmEnable(bool alarmEnable, byte[] state);
	}
}