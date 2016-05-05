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
using DSPortAdapter = com.dalsemi.onewire.adapter.DSPortAdapter;
using OneWireIOException = com.dalsemi.onewire.adapter.OneWireIOException;
using Bit = com.dalsemi.onewire.utils.Bit;
using Convert = com.dalsemi.onewire.utils.Convert;
namespace com.dalsemi.onewire.container
{
	
	
	/// <summary> <P> 1-Wire container for 512 byte memory iButton Plus Time, DS1994 and 1-Wire Chip, DS2404.
	/// This container encapsulates the functionality of the iButton family
	/// type <B>04</B> (hex)</P>
	/// 
	/// <P> This iButton is primarily used as a read/write portable memory device with real-time-clock,
	/// timer and experation features. </P>
	/// 
	/// <H3> Features </H3>
	/// <UL>
	/// <LI> 4096 bits (512 bytes) of read/write nonvolatile memory
	/// <LI> 256-bit (32-byte) scratchpad ensures integrity of data
	/// transfer
	/// <LI> Memory partitioned into 256-bit (32-byte) pages for
	/// packetizing data
	/// <LI> Data integrity assured with strict read/write
	/// protocols
	/// <LI> Contains real time clock/calendar in binary
	/// format
	/// <LI> Interval timer can automatically accumulate
	/// time when power is applied
	/// <LI> Programmable cycle counter can accumulate
	/// the number of system power-on/off cycles
	/// <LI> Programmable alarms can be set to generate
	/// interrupts for interval timer, real time clock,
	/// and/or cycle counter
	/// <LI> Write protect feature provides tamper-proof
	/// time data
	/// <LI> Programmable expiration date that will limit
	/// access to SRAM and timekeeping
	/// <LI> Clock accuracy is better than @htmlonly &#177 @endhtmlonly 2 minute/
	/// month at 25@htmlonly &#176C @endhtmlonly
	/// <LI> Operating temperature range from -40@htmlonly &#176C @endhtmlonly to
	/// +70@htmlonly &#176C @endhtmlonly
	/// <LI> Over 10 years of data retention
	/// </UL>
	/// 
	/// <P> Appended to the clock page data retrieved with 'readDevice'
	/// are 4 bytes that represent a bitmap
	/// of changed bytes.  These bytes are used in the 'writeDevice' method
	/// in conjuction with the 'set' methods to only write back the changed
	/// clock register bytes.  The 'readDevice' method will clear any
	/// pending alarms. </P>
	/// 
	/// <P> WARNING: If write-protect alarm options have been set prior
	/// to a call to 'writeDevice' then the operation is non-reversable. </P>
	/// 
	/// <H3> Alternate Names </H3>
	/// <UL>
	/// <LI> D2504
	/// <LI> D1427 (family type 84 hex)
	/// </UL>
	/// 
	/// <H3> Memory </H3>
	/// 
	/// <P> The memory can be accessed through the objects that are returned
	/// from the {@link #getMemoryBanks() getMemoryBanks} method. </P>
	/// 
	/// The following is a list of the MemoryBank instances that are returned:
	/// 
	/// <UL>
	/// <LI> <B> Scratchpad </B>
	/// <UL>
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 32 starting at physical address 0
	/// <LI> <I> Features</I> Read/Write not-general-purpose volatile
	/// <LI> <I> Pages</I> 1 pages of length 32 bytes
	/// <LI> <I> Extra information for each page</I>  Target address, offset, length 3
	/// </UL>
	/// <LI> <B> Main Memory </B>
	/// <UL>
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 512 starting at physical address 0
	/// <LI> <I> Features</I> Read/Write general-purpose non-volatile
	/// <LI> <I> Pages</I> 16 pages of length 32 bytes giving 29 bytes Packet data payload
	/// </UL>
	/// <LI> <B> Clock/alarm registers </B>
	/// <UL>
	/// <LI> <I> Implements </I> {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// <LI> <I> Size </I> 32 starting at physical address 512
	/// <LI> <I> Features</I> Read/Write not-general-purpose non-volatile
	/// <LI> <I> Pages</I> 1 pages of length 32 bytes
	/// </UL>
	/// </UL>
	/// 
	/// <H3> Clock </H3>
	/// 
	/// <P>The clock methods can be organized into the following categories.  Note that methods
	/// that are implemented for the {@link com.dalsemi.onewire.container.ClockContainer ClockContainer}
	/// interface are marked with (*): </P>
	/// <UL>
	/// <LI> <B> Information </B>
	/// <UL>
	/// <LI> {@link #hasClockAlarm() hasClockAlarm} *
	/// <LI> {@link #canDisableClock() canDisableClock} *
	/// <LI> {@link #getClockResolution() getClockResolution} *
	/// </UL>
	/// <LI> <B> Read </B>
	/// <UL>
	/// <LI> <I> Clock </I>
	/// <UL>
	/// <LI> {@link #getClock(byte[]) getClock} *
	/// <LI> {@link #getClockAlarm(byte[]) getClockAlarm} *
	/// <LI> {@link #isClockAlarming(byte[]) isClockAlarming} *
	/// <LI> {@link #isClockAlarmEnabled(byte[]) isClockAlarmEnabled} *
	/// <LI> {@link #isClockRunning(byte[]) isClockRunning} *
	/// <LI> {@link #isClockWriteProtected(byte[]) isClockWriteProtected}
	/// </UL>
	/// <LI> <I> Interval Timer </I>
	/// <UL>
	/// <LI> {@link #getIntervalTimer(byte[]) getIntervalTimer}
	/// <LI> {@link #getIntervalTimerAlarm(byte[]) getIntervalTimerAlarm}
	/// <LI> {@link #isIntervalTimerAlarming(byte[]) isIntervalTimerAlarming}
	/// <LI> {@link #isIntervalTimerAlarmEnabled(byte[]) isIntervalTimerAlarmEnabled}
	/// <LI> {@link #isIntervalTimerWriteProtected(byte[]) isIntervalTimerWriteProtected}
	/// <LI> {@link #isIntervalTimerAutomatic(byte[]) isIntervalTimerAutomatic}
	/// <LI> {@link #isIntervalTimerStopped(byte[]) isIntervalTimerStopped}
	/// </UL>
	/// <LI> <I> Power Cycle Counter </I>
	/// <UL>
	/// <LI> {@link #getCycleCounter(byte[]) getCycleCounter}
	/// <LI> {@link #getCycleCounterAlarm(byte[]) getCycleCounterAlarm}
	/// <LI> {@link #isCycleCounterAlarming(byte[]) isCycleCounterAlarming}
	/// <LI> {@link #isCycleCounterAlarmEnabled(byte[]) isCycleCounterAlarmEnabled}
	/// <LI> {@link #isCycleCounterWriteProtected(byte[]) isCycleCounterWriteProtected}
	/// </UL>
	/// <LI> <I> Misc </I>
	/// <UL>
	/// <LI> {@link #readDevice() readDevice} *
	/// <LI> {@link #isAutomaticDelayLong(byte[]) isAutomaticDelayLong}
	/// <LI> {@link #canReadAfterExpire(byte[]) canReadAfterExpire}
	/// </UL>
	/// </UL>
	/// <LI> <B> Write </B>
	/// <UL>
	/// <LI> <I> Clock </I>
	/// <UL>
	/// <LI> {@link #setClock(long, byte[]) setClock} *
	/// <LI> {@link #setClockAlarm(long, byte[]) setClockAlarm} *
	/// <LI> {@link #setClockRunEnable(boolean, byte[]) setClockRunEnable} *
	/// <LI> {@link #setClockAlarmEnable(boolean, byte[]) setClockAlarmEnable} *
	/// <LI> {@link #writeProtectClock(byte[]) writeProtectClock}
	/// </UL>
	/// <LI> <I> Interval Timer </I>
	/// <UL>
	/// <LI> {@link #setIntervalTimer(long, byte[]) setIntervalTimer}
	/// <LI> {@link #setIntervalTimerAlarm(long, byte[]) setIntervalTimerAlarm}
	/// <LI> {@link #writeProtectIntervalTimer(byte[]) writeProtectIntervalTimer}
	/// <LI> {@link #setIntervalTimerAutomatic(boolean, byte[]) setIntervalTimerAutomatic}
	/// <LI> {@link #setIntervalTimerRunState(boolean, byte[]) setIntervalTimerRunState}
	/// <LI> {@link #setIntervalTimerAlarmEnable(boolean, byte[]) setIntervalTimerAlarmEnable}
	/// </UL>
	/// <LI> <I> Power Cycle Counter </I>
	/// <UL>
	/// <LI> {@link #setCycleCounter(long, byte[]) setCycleCounter}
	/// <LI> {@link #setCycleCounterAlarm(long, byte[]) setCycleCounterAlarm}
	/// <LI> {@link #writeProtectCycleCounter(byte[]) writeProtectCycleCounter}
	/// <LI> {@link #setCycleCounterAlarmEnable(boolean, byte[]) setCycleCounterAlarmEnable}
	/// </UL>
	/// <LI> <I> Misc </I>
	/// <UL>
	/// <LI> {@link #writeDevice(byte[]) writeDevice} *
	/// <LI> {@link #setReadAfterExpire(boolean, byte[]) setReadAfterExpire}
	/// <LI> {@link #setAutomaticDelayLong(boolean, byte[]) setAutomaticDelayLong}
	/// </UL>
	/// </UL>
	/// </UL>
	/// 
	/// <H3> Usage </H3>
	/// 
	/// <DL>
	/// <DD> See the usage examples in
	/// {@link com.dalsemi.onewire.container.ClockContainer ClockContainer}
	/// for basic clock operations.
	/// <DD> See the usage example in
	/// {@link com.dalsemi.onewire.container.OneWireContainer OneWireContainer}
	/// to enumerate the MemoryBanks.
	/// <DD> See the usage examples in
	/// {@link com.dalsemi.onewire.container.MemoryBank MemoryBank} and
	/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank}
	/// for bank specific operations.
	/// </DL>
	/// 
	/// <H3> DataSheets </H3>
	/// <DL>
	/// <DD><A HREF="http://pdfserv.maxim-ic.com/arpdf/DS1992-DS1994.pdf"> http://pdfserv.maxim-ic.com/arpdf/DS1992-DS1994.pdf</A>
	/// <DD><A HREF="http://pdfserv.maxim-ic.com/arpdf/DS2404.pdf"> http://pdfserv.maxim-ic.com/arpdf/DS2404.pdf</A>
	/// <DD><A HREF="http://pdfserv.maxim-ic.com/arpdf/DS1427.pdf"> http://pdfserv.maxim-ic.com/arpdf/DS1427.pdf</A>
	/// </DL>
	/// 
	/// </summary>
	/// <seealso cref="com.dalsemi.onewire.container.MemoryBank">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.PagedMemoryBank">
	/// </seealso>
	/// <seealso cref="com.dalsemi.onewire.container.ClockContainer">
	/// 
	/// </seealso>
	/// <version>     0.00, 28 Aug 2000
	/// </version>
	/// <author>      DS
	/// </author>
	public class OneWireContainer04:OneWireContainer, ClockContainer
	{
		/// <summary> Get the Maxim Integrated Products part number of the iButton
		/// or 1-Wire Device as a string.  For example 'DS1992'.
		/// 
		/// </summary>
		/// <returns> iButton or 1-Wire device name
		/// </returns>
		override public System.String Name
		{
			get
			{
				return "DS1994";
			}
			
		}
		/// <summary> Get the alternate Maxim Integrated Products part numbers or names.
		/// A 'family' of 1-Wire Network devices may have more than one part number
		/// depending on packaging.  There can also be nicknames such as
		/// 'Crypto iButton'.
		/// 
		/// </summary>
		/// <returns> 1-Wire device alternate names
		/// </returns>
		override public System.String AlternateNames
		{
			get
			{
				return "DS2404, Time-in-a-can, DS1427";
			}
			
		}
		/// <summary> Get a short description of the function of this iButton
		/// or 1-Wire Device type.
		/// 
		/// </summary>
		/// <returns> device description
		/// </returns>
		override public System.String Description
		{
			get
			{
				return "4096 bit read/write nonvolatile memory partitioned " + "into sixteen pages of 256 bits each and a real " + "time clock/calendar in binary format.";
			}
			
		}
		/// <summary> Get an enumeration of memory bank instances that implement one or more
		/// of the following interfaces:
		/// {@link com.dalsemi.onewire.container.MemoryBank MemoryBank},
		/// {@link com.dalsemi.onewire.container.PagedMemoryBank PagedMemoryBank},
		/// and {@link com.dalsemi.onewire.container.OTPMemoryBank OTPMemoryBank}.
		/// </summary>
		/// <returns> <CODE>Enumeration</CODE> of memory banks
		/// </returns>
		override public System.Collections.IEnumerator MemoryBanks
		{
			get
			{
				System.Collections.ArrayList bank_vector = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(3));
				
				// scratchpad
				bank_vector.Add(scratch);
				
				// NVRAM
				bank_vector.Add(new MemoryBankNV(this, scratch));
				
				// clock
				bank_vector.Add(clock);
				
				return bank_vector.GetEnumerator();
			}
			
		}
		/// <summary> Query to get the clock resolution in milliseconds
		/// 
		/// </summary>
		/// <returns> the clock resolution in milliseconds
		/// </returns>
		virtual public long ClockResolution
		{
			get
			{
				return 4;
			}
			
		}
		
		//--------
		//-------- Finals
		//--------
		
		/// <summary>Offset of BITMAP in array returned from read registers </summary>
		protected internal const int BITMAP_OFFSET = 32;
		
		/// <summary>Offset of status register from read registers </summary>
		protected internal const int STATUS_OFFSET = 0;
		
		/// <summary>Offset of control register from read registers </summary>
		protected internal const int CONTROL_OFFSET = 1;
		
		/// <summary>Offset of real-time-clock in array returned from read registers </summary>
		protected internal const int RTC_OFFSET = 2;
		
		/// <summary>Offset of inverval-counter in array returned from read registers </summary>
		protected internal const int INTERVAL_OFFSET = 7;
		
		/// <summary>Offset of counter in array returned from read registers </summary>
		protected internal const int COUNTER_OFFSET = 12;
		
		/// <summary>Offset of real-time-clock-alarm in array returned from read registers </summary>
		protected internal const int RTC_ALARM_OFFSET = 16;
		
		/// <summary>Offset of inverval-counter-alarm in array returned from read registers </summary>
		protected internal const int INTERVAL_ALARM_OFFSET = 21;
		
		/// <summary>Offset of counter-alarm in array returned from read registers </summary>
		protected internal const int COUNTER_ALARM_OFFSET = 26;
		
		//--------
		//-------- Variables
		//--------
		
		/// <summary> Scratchpad access memory bank</summary>
		private MemoryBankScratch scratch;
		
		/// <summary> Clock memory access memory bank</summary>
		private MemoryBankNV clock;
		
		//--------
		//-------- Constructors
		//--------
		
		/// <summary> Create an empty container that is not complete until after a call
		/// to <code>setupContainer</code>. <p>
		/// 
		/// This is one of the methods to construct a container.  The others are
		/// through creating a OneWireContainer with parameters.
		/// 
		/// </summary>
		/// <seealso cref="setupContainer(com.dalsemi.onewire.adapter.DSPortAdapter,byte[]) super.setupContainer()">
		/// </seealso>
		public OneWireContainer04():base()
		{
			
			// initialize the clock memory bank
			initClock();
		}
		
		/// <summary> Create a container with the provided adapter instance
		/// and the address of the iButton or 1-Wire device.<p>
		/// 
		/// This is one of the methods to construct a container.  The other is
		/// through creating a OneWireContainer with NO parameters.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter instance used to communicate with
		/// this iButton
		/// </param>
		/// <param name="newAddress">       {@link com.dalsemi.onewire.utils.Address Address}
		/// of this 1-Wire device
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer04() OneWireContainer04">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer04(DSPortAdapter sourceAdapter, byte[] newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the clock memory bank
			initClock();
		}
		
		/// <summary> Create a container with the provided adapter instance
		/// and the address of the iButton or 1-Wire device.<p>
		/// 
		/// This is one of the methods to construct a container.  The other is
		/// through creating a OneWireContainer with NO parameters.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter instance used to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       {@link com.dalsemi.onewire.utils.Address Address}
		/// of this 1-Wire device
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer04() OneWireContainer04">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer04(DSPortAdapter sourceAdapter, long newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the clock memory bank
			initClock();
		}
		
		/// <summary> Create a container with the provided adapter instance
		/// and the address of the iButton or 1-Wire device.<p>
		/// 
		/// This is one of the methods to construct a container.  The other is
		/// through creating a OneWireContainer with NO parameters.
		/// 
		/// </summary>
		/// <param name="sourceAdapter">    adapter instance used to communicate with
		/// this 1-Wire device
		/// </param>
		/// <param name="newAddress">       {@link com.dalsemi.onewire.utils.Address Address}
		/// of this 1-Wire device
		/// 
		/// </param>
		/// <seealso cref="OneWireContainer04() OneWireContainer04">
		/// </seealso>
		/// <seealso cref="com.dalsemi.onewire.utils.Address utils.Address">
		/// </seealso>
		public OneWireContainer04(DSPortAdapter sourceAdapter, System.String newAddress):base(sourceAdapter, newAddress)
		{
			
			// initialize the clock memory bank
			initClock();
		}
		
		//--------
		//-------- Methods
		//--------
		
		//--------
		//-------- Clock Feature methods
		//--------
		
		/// <summary> Query to see if the clock has an alarm feature.
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
		public virtual bool hasClockAlarm()
		{
			return true;
		}
		
		/// <summary> Query to see if the clock can be disabled.
		/// 
		/// </summary>
		/// <returns> true if the clock can be enabled and disabled
		/// 
		/// </returns>
		/// <seealso cref="isClockRunning(byte[])">
		/// </seealso>
		/// <seealso cref="setClockRunEnable(boolean,byte[])">
		/// </seealso>
		public virtual bool canDisableClock()
		{
			return true;
		}
		
		//--------
		//-------- Clock IO Methods
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
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		public virtual byte[] readDevice()
		{
			byte[][] read_buf = new byte[2][];
			for (int i = 0; i < 2; i++)
			{
				read_buf[i] = new byte[36];
			}
			bool alarming;
			int buf_num = 0, attempt = 0, i2;
			
			// put zero's in the bitmap of changed bytes
			for (i2 = 32; i2 < 36; i2++)
			{
				read_buf[0][i2] = 0;
				read_buf[1][i2] = 0;
			}
			
			// check if device alarming
			alarming = Alarming;
			
			// loop up to 5 times to read clock register page
			do 
			{
				
				// only read status byte once if device was alarming (will be cleared)
				if (alarming && (attempt != 0))
					clock.read(1, false, read_buf[buf_num], 1, 31);
				else
					clock.read(0, false, read_buf[buf_num], 0, 32);
				
				// compare if this is not the first read
				if (attempt++ != 0)
				{
					
					// loop to see if same
					for (i2 = 1; i2 < 32; i2++)
					{
						if ((i2 != 2) && (i2 != 7))
						{
							if (read_buf[0][i2] != read_buf[1][i2])
								break;
						}
					}
					
					// check on compare, if ok then return most recent read_buf
					if (i2 == 32)
						return read_buf[buf_num];
				}
				
				// alternate buffer
				buf_num = (buf_num == 0)?1:0;
			}
			while (attempt < 5);
			
			// failed to get a match
			throw new OneWireIOException("Failed to read the clock register page");
		}
		
		/// <summary> Writes the 1-Wire device sensor state that
		/// have been changed by 'set' methods.  Only the state registers that
		/// changed are updated.  This is done by referencing a field information
		/// appended to the state data.
		/// 
		/// </summary>
		/// <param name="state">1-Wire device sensor state
		/// 
		/// </param>
		/// <throws>  OneWireIOException on a 1-Wire communication error such as </throws>
		/// <summary>         reading an incorrect CRC from a 1-Wire device.  This could be
		/// caused by a physical interruption in the 1-Wire Network due to
		/// shorts or a newly arriving 1-Wire device issuing a 'presence pulse'.
		/// </summary>
		/// <throws>  OneWireException on a communication or setup error with the 1-Wire </throws>
		/// <summary>         adapter
		/// </summary>
		public virtual void  writeDevice(byte[] state)
		{
			int start_offset = 0, len = 0, i;
			bool got_block = false;
			
			// loop to collect changed bytes and write them in blocks
			for (i = 0; i < 32; i++)
			{
				
				// check to see if this byte needs writing (skip control register for now)
				if ((Bit.arrayReadBit(i, BITMAP_OFFSET, state) == 1) && (i != 1))
				{
					
					// check if already in a block
					if (got_block)
						len++;
					// new block
					else
					{
						got_block = true;
						start_offset = i;
						len = 1;
					}
					
					// check for last byte exception, write current block
					if (i == 31)
						clock.write(start_offset, state, start_offset, len);
				}
				else if (got_block)
				{
					
					// done with this block so write it
					clock.write(start_offset, state, start_offset, len);
					
					got_block = false;
				}
			}
			
			// check if need to write control register
			if (Bit.arrayReadBit(CONTROL_OFFSET, BITMAP_OFFSET, state) == 1)
			{
				
				// write normaly
				clock.write(CONTROL_OFFSET, state, CONTROL_OFFSET, 1);
				
				// check if any write-protect bits set
				if ((state[CONTROL_OFFSET] & 0x07) != 0)
				{
					
					// need to do a copy scratchpad 2 more times to become write-protected
					scratch.copyScratchpad(clock.StartPhysicalAddress + CONTROL_OFFSET, 1, true);
				}
			}
			
			// clear out the bitmap
			for (i = BITMAP_OFFSET; i < state.Length; i++)
				state[i] = 0;
		}
		
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
		public virtual long getClock(byte[] state)
		{
			return Convert.toLong(state, RTC_OFFSET, 5) * 1000 / 256;
		}
		
		/// <summary> Extracts the clock alarm value for the Real-Time clock.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> milliseconds since 1970 that the clock alarm is set to
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
		//public virtual long getClockAlarm(byte[] state)
        public virtual DateTime getClockAlarm(byte[] state)
		{
			//return Convert.toLong(state, RTC_ALARM_OFFSET, 5) * 1000 / 256;
            long UnixTime = Convert.toLong(state, RTC_ALARM_OFFSET, 5) * 1000 / 256;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(UnixTime);     // ???      !!!  

		}
		
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
		public virtual bool isClockAlarming(byte[] state)
		{
			return (Bit.arrayReadBit(0, STATUS_OFFSET, state) == 1);
		}
		
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
		public virtual bool isClockAlarmEnabled(byte[] state)
		{
			return (Bit.arrayReadBit(3, STATUS_OFFSET, state) == 0);
		}
		
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
		public virtual bool isClockRunning(byte[] state)
		{
			return (Bit.arrayReadBit(4, CONTROL_OFFSET, state) == 1);
		}
		
		//--------
		//-------- DS1994 Specific Clock 'get' Methods
		//--------
		
		/// <summary> Get the Interval Timer Value in milliseconds.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> time in milliseconds
		/// that have occured since the interval counter was started
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="setIntervalTimer(long,byte[])">
		/// </seealso>
		public virtual long getIntervalTimer(byte[] state)
		{
			return Convert.toLong(state, INTERVAL_OFFSET, 5) * 1000 / 256;
		}
		
		/// <summary> Get the power cycle count value.  This is the total number of
		/// power cycles that the DS1994 has seen since the counter was reset.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> power cycle count
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="setCycleCounter(long,byte[]) setCycleCounter">
		/// </seealso>
		public virtual long getCycleCounter(byte[] state)
		{
			return Convert.toLong(state, COUNTER_OFFSET, 4);
		}
		
		/// <summary> Get the Interval Timer Alarm Value.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> time in milliseconds that have
		/// the interval timer alarm is set to
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="setIntervalTimerAlarm(long,byte[]) setIntervalTimerAlarm">
		/// </seealso>
		public virtual long getIntervalTimerAlarm(byte[] state)
		{
			return Convert.toLong(state, INTERVAL_ALARM_OFFSET, 5) * 1000 / 256;
		}
		
		/// <summary> Get the cycle count Alarm Value.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> total number of power cycles
		/// that the DS1994 has to to see before the alarm will be triggered
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="setCycleCounterAlarm(long,byte[]) setCycleCounterAlarm">
		/// </seealso>
		public virtual long getCycleCounterAlarm(byte[] state)
		{
			return Convert.toLong(state, COUNTER_ALARM_OFFSET, 4);
		}
		
		/// <summary> Check if the Interval Timer Alarm flag has been set.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <CODE>true</CODE> if interval timer is alarming
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="isIntervalTimerAlarmEnabled(byte[]) isIntervalTimerAlarmEnabled">
		/// </seealso>
		/// <seealso cref="setIntervalTimerAlarmEnable(boolean,byte[]) setIntervalTimerAlarmEnable">
		/// </seealso>
		public virtual bool isIntervalTimerAlarming(byte[] state)
		{
			return (Bit.arrayReadBit(1, STATUS_OFFSET, state) == 1);
		}
		
		/// <summary> Check if the Cycle Alarm flag has been set.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if cycle counter is alarming
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="isCycleCounterAlarmEnabled(byte[]) isCycleCounterAlarmEnabled">
		/// </seealso>
		/// <seealso cref="setCycleCounterAlarmEnable(boolean,byte[]) setCycleCounterAlarmEnable">
		/// </seealso>
		public virtual bool isCycleCounterAlarming(byte[] state)
		{
			return (Bit.arrayReadBit(2, STATUS_OFFSET, state) == 1);
		}
		
		/// <summary> Check if the Interval Timer Alarm is enabled.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if interval timer alarm is enabled
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="isIntervalTimerAlarming(byte[]) isIntervalTimerAlarming">
		/// </seealso>
		/// <seealso cref="setIntervalTimerAlarmEnable(boolean,byte[]) setIntervalTimerAlarmEnable">
		/// </seealso>
		public virtual bool isIntervalTimerAlarmEnabled(byte[] state)
		{
			return (Bit.arrayReadBit(4, STATUS_OFFSET, state) == 0);
		}
		
		/// <summary> Check if the Cycle Alarm is enabled.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> true if cycle counter alarm is enabled
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="isCycleCounterAlarming(byte[]) isCycleCounterAlarming">
		/// </seealso>
		/// <seealso cref="setCycleCounterAlarmEnable(boolean,byte[]) setCycleCounterAlarmEnable">
		/// </seealso>
		public virtual bool isCycleCounterAlarmEnabled(byte[] state)
		{
			return (Bit.arrayReadBit(5, STATUS_OFFSET, state) == 0);
		}
		
		/// <summary> Check if the Real-Time clock/Alarm is write protected.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if real time clock/alarm is write
		/// protected
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="writeProtectClock(byte[]) writeProtectClock">
		/// </seealso>
		public virtual bool isClockWriteProtected(byte[] state)
		{
			return (Bit.arrayReadBit(0, CONTROL_OFFSET, state) == 1);
		}
		
		/// <summary> Check if the Interval Timer and Interval Timer Alarm
		/// register is write protected.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if interval timer and interval timer alarm is
		/// write protected
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="writeProtectIntervalTimer(byte[]) writeProtectIntervalTimer">
		/// </seealso>
		public virtual bool isIntervalTimerWriteProtected(byte[] state)
		{
			return (Bit.arrayReadBit(1, CONTROL_OFFSET, state) == 1);
		}
		
		/// <summary> Check if the Cycle Counter and Alarm is write protected.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if cycle counter/alarm is write
		/// protected
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="writeProtectCycleCounter(byte[]) writeProtectCycleCounter">
		/// </seealso>
		public virtual bool isCycleCounterWriteProtected(byte[] state)
		{
			return (Bit.arrayReadBit(2, CONTROL_OFFSET, state) == 1);
		}
		
		/// <summary> Check if the device can be read after a write protected
		/// alarm has occured.
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if the device can be read after a
		/// write protected alarm has occured
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="setReadAfterExpire(boolean, byte[]) setReadAfterExpire">
		/// </seealso>
		public virtual bool canReadAfterExpire(byte[] state)
		{
			return (Bit.arrayReadBit(3, CONTROL_OFFSET, state) == 1);
		}
		
		/// <summary> Checks if the Interval timer is automatic or manual.  If it is
		/// automatic then the interval counter will increment while the devices I/O line
		/// is high after the delay select period has elapsed (either 3.5 or 123 ms, see
		/// the isAutomaticDelayLong() method).
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if the interval timer is set to automatic
		/// mode
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="setIntervalTimerAutomatic(boolean, byte[]) setIntervalTimerAutomatic">
		/// </seealso>
		public virtual bool isIntervalTimerAutomatic(byte[] state)
		{
			return (Bit.arrayReadBit(5, CONTROL_OFFSET, state) == 1);
		}
		
		/// <summary> Check if the Interval timer is stopped.  This only has meaning
		/// if the interval timer is in manual mode (not <CODE>isIntervalTimerAutomatic</CODE>).
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if the interval timer is stopped
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="isIntervalTimerAutomatic(byte[]) isIntervalTimerAutomatic">
		/// </seealso>
		/// <seealso cref="setIntervalTimerAutomatic(boolean, byte[]) setIntervalTimerAutomatic">
		/// </seealso>
		/// <seealso cref="setIntervalTimerRunState(boolean, byte[]) setIntervalTimerRunState">
		/// </seealso>
		public virtual bool isIntervalTimerStopped(byte[] state)
		{
			return (Bit.arrayReadBit(6, CONTROL_OFFSET, state) == 1);
		}
		
		/// <summary> Checks if the automatic delay for the Inteval Timer and the Cycle
		/// counter is either 3.5ms (regular) or 123ms (long).
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <returns> <code>true</code> if the automatic interval/cycle counter
		/// delay is in the long (123ms) mode, else it is 3.5ms
		/// 
		/// </returns>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.readDevice()">
		/// </seealso>
		/// <seealso cref="setAutomaticDelayLong(boolean,byte[]) setAutomaticDelayLong">
		/// </seealso>
		public virtual bool isAutomaticDelayLong(byte[] state)
		{
			return (Bit.arrayReadBit(7, CONTROL_OFFSET, state) == 1);
		}
		
		//--------
		//-------- Clock 'set' Methods
		//--------
		
		/// <summary> Sets the Real-Time clock.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
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
		public virtual void  setClock(long time, byte[] state)
		{
			Convert.toByteArray(time * 256 / 1000, state, RTC_OFFSET, 5);
			
			// set bitmap field to indicate these clock registers were changed
			for (int i = 0; i < 5; i++)
				Bit.arrayWriteBit(1, RTC_OFFSET + i, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the clock alarm.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.  Also note that
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
		public virtual void  setClockAlarm(long time, byte[] state)
		{
			Convert.toByteArray(time * 256 / 1000, state, RTC_ALARM_OFFSET, 5);
			
			// set bitmap field to indicate these clock registers were changed
			for (int i = 0; i < 5; i++)
				Bit.arrayWriteBit(1, RTC_ALARM_OFFSET + i, BITMAP_OFFSET, state);
		}
		
		/// <summary> Enables or disables the oscillator, turning the clock 'on' and 'off'.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.  Also note that
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
		public virtual void  setClockRunEnable(bool runEnable, byte[] state)
		{
			Bit.arrayWriteBit(runEnable?1:0, 4, CONTROL_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, CONTROL_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Enables or disables the clock alarm.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.  Also note that
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
		public virtual void  setClockAlarmEnable(bool alarmEnable, byte[] state)
		{
			Bit.arrayWriteBit(alarmEnable?0:1, 3, STATUS_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, STATUS_OFFSET, BITMAP_OFFSET, state);
		}
		
		//--------
		//-------- DS1994 Specific Clock 'set' Methods
		//--------
		
		/// <summary> Sets the Interval Timer.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="time">interval in milliseconds to set (truncated to 1/256th of second)
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="getIntervalTimer(byte[]) getIntervalTimer">
		/// </seealso>
		public virtual void  setIntervalTimer(long time, byte[] state)
		{
			Convert.toByteArray(time * 256 / 1000, state, INTERVAL_OFFSET, 5);
			
			// set bitmap field to indicate these clock registers were changed
			for (int i = 0; i < 5; i++)
				Bit.arrayWriteBit(1, INTERVAL_OFFSET + i, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets power Cycle Counter.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="cycles">initialize cycle counter value
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="getCycleCounter(byte[]) getCycleCounter">
		/// </seealso>
		public virtual void  setCycleCounter(long cycles, byte[] state)
		{
			Convert.toByteArray(cycles, state, COUNTER_OFFSET, 4);
			
			// set bitmap field to indicate these clock registers were changed
			for (int i = 0; i < 4; i++)
				Bit.arrayWriteBit(1, COUNTER_OFFSET + i, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the Interval Timer Alarm.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="time">in milliseconds to set the inverval timer
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="getIntervalTimerAlarm(byte[]) getIntervalTimerAlarm">
		/// </seealso>
		public virtual void  setIntervalTimerAlarm(long time, byte[] state)
		{
			Convert.toByteArray(time * 256 / 1000, state, INTERVAL_ALARM_OFFSET, 5);
			
			// set bitmap field to indicate these clock registers were changed
			for (int i = 0; i < 5; i++)
				Bit.arrayWriteBit(1, INTERVAL_ALARM_OFFSET + i, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the power Cycle Count Alarm. This counter holds the number
		/// of times the DS1994 must experience power cycles
		/// before it generates an alarm.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="cycles">power Cycle Count alarm
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="getCycleCounterAlarm(byte[]) getCycleCounterAlarm">
		/// </seealso>
		public virtual void  setCycleCounterAlarm(long cycles, byte[] state)
		{
			Convert.toByteArray(cycles, state, COUNTER_ALARM_OFFSET, 4);
			
			// set bitmap field to indicate these clock registers were changed
			for (int i = 0; i < 4; i++)
				Bit.arrayWriteBit(1, COUNTER_ALARM_OFFSET + i, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the write protect options for the Real-Time
		/// clock/Alarm.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// <P>WARNING: after calling this method and then
		/// <CODE> writeDevice </CODE> the device will be permanently write
		/// protected. </P> <BR>
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="isClockWriteProtected(byte[]) isClockWriteProtected">
		/// </seealso>
		public virtual void  writeProtectClock(byte[] state)
		{
			Bit.arrayWriteBit(1, 0, CONTROL_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, CONTROL_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the write protect options for Interval Timer and
		/// Interval Timer Alarm register.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// <P>WARNING: after calling this method and then
		/// <CODE> writeDevice </CODE> the device will be permanently write
		/// protected. </P> <BR>
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="isIntervalTimerWriteProtected(byte[]) isIntervalTimerWriteProtected">
		/// </seealso>
		public virtual void  writeProtectIntervalTimer(byte[] state)
		{
			Bit.arrayWriteBit(1, 1, CONTROL_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, CONTROL_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the write protect options for the Cycle Counter
		/// and Alarm register.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// <P>WARNING: after calling this method and then
		/// <CODE> writeDevice </CODE> the device will be permanently write
		/// protected. </P> <BR>
		/// 
		/// </summary>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="isCycleCounterWriteProtected(byte[]) isCycleCounterWriteProtected">
		/// </seealso>
		public virtual void  writeProtectCycleCounter(byte[] state)
		{
			Bit.arrayWriteBit(1, 2, CONTROL_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, CONTROL_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the read state of the device after a
		/// write protected alarm has occured.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="readAfter"><CODE>true</CODE> to read device after it
		/// expires from a write protected alarm event
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="canReadAfterExpire(byte[]) canReadAfterExpire">
		/// </seealso>
		public virtual void  setReadAfterExpire(bool readAfter, byte[] state)
		{
			Bit.arrayWriteBit(readAfter?1:0, 3, CONTROL_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, CONTROL_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the Interval timer to automatic or manual mode.
		/// When in automatic mode, the interval counter will increment
		/// while the devices I/O line is high after the delay select
		/// period has elapsed (either 3.5 or 123 ms, see the
		/// <CODE>isAutomaticDelayLong()</CODE> method).
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="autoTimer"><CODE>true</CODE> for the interval timer to operate in
		/// automatic mode
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="isIntervalTimerAutomatic(byte[]) isIntervalTimerAutomatic">
		/// </seealso>
		public virtual void  setIntervalTimerAutomatic(bool autoTimer, byte[] state)
		{
			Bit.arrayWriteBit(autoTimer?1:0, 5, CONTROL_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, CONTROL_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the Interval timer run/stop mode.  This only
		/// has meaning if the interval timer is in manual mode
		/// (not <CODE>isIntervalTimerAutomatic()</CODE>).
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="runState"><CODE>true</CODE> to set the interval timer to run
		/// 
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="isIntervalTimerAutomatic(byte[]) isIntervalTimerAutomatic">
		/// </seealso>
		/// <seealso cref="isIntervalTimerStopped(byte[]) isIntervalTimerStopped">
		/// </seealso>
		public virtual void  setIntervalTimerRunState(bool runState, byte[] state)
		{
			Bit.arrayWriteBit(runState?1:0, 6, CONTROL_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, CONTROL_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the automatic delay for the Inteval Timer and the Cycle
		/// counter to either 123ms (long) or 3.5ms (regular).
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="delayLong"><CODE>true</CODE> to set the interval timer to
		/// cycle counter to increment after 123ms or <CODE>false</CODE>
		/// for 3.5ms
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="isAutomaticDelayLong(byte[]) isAutomaticDelayLong">
		/// </seealso>
		public virtual void  setAutomaticDelayLong(bool delayLong, byte[] state)
		{
			Bit.arrayWriteBit(delayLong?1:0, 7, CONTROL_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, CONTROL_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the Interval Timer Alarm enable.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="alarmEnable"><CODE>true</CODE> to enable the interval timer alarm
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="isIntervalTimerAlarmEnabled(byte[]) isIntervalTimerAlarmEnabled">
		/// </seealso>
		public virtual void  setIntervalTimerAlarmEnable(bool alarmEnable, byte[] state)
		{
			Bit.arrayWriteBit(alarmEnable?0:1, 4, STATUS_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, STATUS_OFFSET, BITMAP_OFFSET, state);
		}
		
		/// <summary> Sets the Cycle counter Alarm enable.
		/// The method <code>writeDevice(byte[])</code> must be called to finalize
		/// changes to the device.  Note that multiple 'set' methods can
		/// be called before one call to <code>writeDevice(byte[])</code>.
		/// 
		/// </summary>
		/// <param name="alarmEnable"><CODE>true</CODE> to enable the cycle counter alarm
		/// 
		/// </param>
		/// <param name="state">current state of the device returned from <code>readDevice()</code>
		/// 
		/// </param>
		/// <seealso cref="com.dalsemi.onewire.container.OneWireSensor.writeDevice(byte[])">
		/// </seealso>
		/// <seealso cref="isCycleCounterAlarmEnabled(byte[]) isCycleCounterAlarmEnabled">
		/// </seealso>
		public virtual void  setCycleCounterAlarmEnable(bool alarmEnable, byte[] state)
		{
			Bit.arrayWriteBit(alarmEnable?0:1, 5, STATUS_OFFSET, state);
			
			// set bitmap field to indicate this clock register has changed
			Bit.arrayWriteBit(1, STATUS_OFFSET, BITMAP_OFFSET, state);
		}
		
		//--------
		//-------- Private
		//--------
		
		/// <summary> Create the memory bank interface to read/write the clock</summary>
		private void  initClock()
		{
			
			// scratchpad
			scratch = new MemoryBankScratch(this);
			
			// clock
			clock = new MemoryBankNV(this, scratch);
			clock.numberPages = 1;
			clock.startPhysicalAddress = 512;
			clock.size = 32;
			clock.generalPurposeMemory = false;
			clock.maxPacketDataLength = 0;
			clock.bankDescription = "Clock/alarm registers";
		}
	}
}