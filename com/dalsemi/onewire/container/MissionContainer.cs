/*---------------------------------------------------------------------------
* Copyright (C) 2002 Maxim Integrated Products, All Rights Reserved.
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
using OneWireIOException = com.dalsemi.onewire.adapter.OneWireIOException;
namespace com.dalsemi.onewire.container
{
	
	/// <summary> <p>Interface class for 1-Wire&reg; devices that perform analog measuring
	/// operations. This class should be implemented for each A/D
	/// type 1-Wire device.</p>
	/// 
	/// <h3> Features </h3>
	/// 
	/// <ul>
	/// <li>
	/// Allows multi-channel voltage readings
	/// </li>
	/// <li>
	/// Supports A/D Alarm enabling on devices with A/D Alarms
	/// </li>
	/// <li>
	/// Supports selectable A/D ranges on devices with selectable ranges
	/// </li>
	/// <li>
	/// Supports selectable A/D resolutions on devices with selectable resolutions
	/// </li>
	/// </ul>
	/// 
	/// <h3> Usage </h3>
	/// 
	/// <p><code>ADContainer</code> extends <code>OneWireSensor</code>, so the general usage
	/// model applies to any <code>ADContainer</code>:
	/// <ol>
	/// <li> readDevice()  </li>
	/// <li> perform operations on the <code>ADContainer</code>  </li>
	/// <li> writeDevice(byte[]) </li>
	/// </ol>
	/// 
	/// <p>Consider this interaction with an <code>ADContainer</code> that reads from all of its
	/// A/D channels, then tries to set its high alarm on its first channel (channel 0):
	/// 
	/// <pre><code>
	/// //adcontainer is a com.dalsemi.onewire.container.ADContainer
	/// byte[] state = adcontainer.readDevice();
	/// double[] voltages = new double[adcontainer.getNumberADChannels()];
	/// for (int i=0; i &lt; adcontainer.getNumberADChannels(); i++)
	/// {
	/// adcontainer.doADConvert(i, state);
	/// voltages[i] = adc.getADVoltage(i, state);
	/// }
	/// if (adcontainer.hasADAlarms())
	/// {
	/// double highalarm = adcontainer.getADAlarm(0, ADContainer.ALARM_HIGH, state);
	/// adcontainer.setADAlarm(0, ADContainer.ALARM_HIGH, highalarm + 1.0, state);
	/// adcontainer.writeDevice(state);
	/// }
	/// 
	/// </code></pre>
	/// 
	/// </summary>
	/// <version>     1.00, 20 February 2002
	/// </version>
	/// <author>      SH
	/// </author>
	public struct MissionContainer_Fields{
		/// <summary> Indicates the high alarm.</summary>
		public readonly static int ALARM_HIGH = 1;
		/// <summary> Indicates the low alarm.</summary>
		public readonly static int ALARM_LOW = 0;
	}
	public interface MissionContainer:ClockContainer
	{
		//UPGRADE_NOTE: Members of interface 'MissionContainer' were extracted into structure 'MissionContainer_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		/// <summary> Returns <code>true</code> if a mission is currently running.</summary>
		/// <returns> <code>true</code> if a mission is currently running.
		/// </returns>
		bool MissionRunning
		{
			get;
			
		}
		/// <summary> Returns <code>true</code> if a rollover is enabled.</summary>
		/// <returns> <code>true</code> if a rollover is enabled.
		/// </returns>
		bool MissionRolloverEnabled
		{
			get;
			
		}
		/// <summary> </summary>
        bool isMissionLoaded();

		/// <summary> Gets the number of channels supported by this Missioning device.
		/// Channel specific methods will use a channel number specified
		/// by an integer from [0 to (<code>getNumberOfMissionChannels()</code> - 1)].
		/// 
		/// </summary>
		/// <returns> the number of channels
		/// </returns>
		int NumberMissionChannels
		{
			get;
			
		}
		
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// - Mission Start/Stop/Status
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		
		/// <summary> Begins a new mission on this missioning device.
		/// 
		/// </summary>
		/// <param name="sampleRate">indicates the sampling rate, in seconds, that
		/// this missioning device should log samples.
		/// </param>
		/// <param name="missionStartDelay">indicates the amount of time, in seconds,
		/// that should pass before the mission begins.
		/// </param>
		/// <param name="rolloverEnabled">if <code>false</code>, this device will stop
		/// recording new samples after the data log is full.  Otherwise,
		/// it will replace samples starting at the beginning.
		/// </param>
		/// <param name="syncClock">if <code>true</code>, the real-time clock of this
		/// missioning device will be synchronized with the current time
		/// according to this <code>java.util.Date</code>.
		/// </param>
		void  startNewMission(int sampleRate, int missionStartDelay, bool rolloverEnabled, bool syncClock, bool[] channelEnabled);
		
		/// <summary> Ends the currently running mission.</summary>
		void  stopMission();
		
		/// <summary> Returns <code>true</code> if a mission has rolled over.</summary>
		/// <returns> <code>true</code> if a mission has rolled over.
		/// </returns>
		bool hasMissionRolloverOccurred();
		
		/// <summary> Loads the results of the currently running mission.  Must be called
		/// before all mission result/status methods.
		/// </summary>
		void  loadMissionResults();
		
		/// <summary> Clears the mission results and erases the log memory from this
		/// missioning device.
		/// </summary>
		void  clearMissionResults();
		
		/// <summary> Enables/disables the specified mission channel, indicating whether or
		/// not the channel's readings will be recorded in the mission log.
		/// 
		/// </summary>
		/// <param name="channel">the channel to enable/disable
		/// </param>
		/// <param name="enable">if true, the channel is enabled
		/// </param>
		void  setMissionChannelEnable(int channel, bool enable);
		
		/// <summary> Returns true if the specified mission channel is enabled, indicating
		/// that the channel's readings will be recorded in the mission log.
		/// 
		/// </summary>
		/// <param name="channel">the channel to enable/disable
		/// </param>
		/// <param name="enable">if true, the channel is enabled
		/// </param>
		bool getMissionChannelEnable(int channel);
		
		/// <summary> Returns a default friendly label for each channel supported by this
		/// Missioning device.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> friendly label for the specified channel
		/// </returns>
		System.String getMissionLabel(int channel);
		
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// - Mission Results
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		
		/// <summary> Returns the time, in milliseconds, that the mission began.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> time, in milliseconds, that the mission began
		/// </returns>
		long getMissionTimeStamp(int channel);
		
		/// <summary> Returns the amount of time, in milliseconds, before the first sample
		/// occurred.  If rollover disabled, or datalog didn't fill up, this
		/// will be 0.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> time, in milliseconds, before first sample occurred
		/// </returns>
		long getFirstSampleOffset(int channel);
		
		/// <summary> Returns the amount of time, in seconds, between samples taken
		/// by this missioning device.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> time, in seconds, between sampling
		/// </returns>
		int getMissionSampleRate(int channel);
		
		/// <summary> Returns the number of samples taken for the specified channel
		/// during the current mission.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> number of samples taken for the specified channel
		/// </returns>
		int getMissionSampleCount(int channel);
		
		/// <summary> Returns the total number of samples taken for the specified channel
		/// during the current mission.  This number can be more than the actual
		/// sample count if rollover is enabled and the log has been filled.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> number of samples taken for the specified channel
		/// </returns>
		int getMissionSampleCountTotal(int channel);
		
		/// <summary> Returns the value of each sample taken by the current mission.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="sampleNum">the sample number to return, between <code>0</code> and
		/// <code>(getMissionSampleCount(channel)-1)</code>
		/// </param>
		/// <returns> the value of the specified sample on the specified channel
		/// </returns>
		double getMissionSample(int channel, int sampleNum);
		
		/// <summary> Returns the sample as an integer value.  This value is not converted to
		/// degrees Celsius for temperature or to percent RH for Humidity.  It is
		/// simply the 8 or 16 bits of digital data written in the mission log for
		/// this sample entry.  It is up to the user to mask off the unused bits
		/// and convert this value to it's proper units.  This method is primarily
		/// for users of the DS2422 who are using an input device which is not an
		/// A-D or have an A-D wholly dissimilar to the one specified in the
		/// datasheet.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="sampleNum">the sample number to return, between <code>0</code> and
		/// <code>(getMissionSampleCount(channel)-1)</code>
		/// </param>
		/// <returns> the sample as a whole integer
		/// </returns>
		int getMissionSampleAsInteger(int channel, int sampleNum);
		
		/// <summary> Returns the time, in milliseconds, that each sample was taken by the
		/// current mission.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="sampleNum">the sample number to return, between <code>0</code> and
		/// <code>(getMissionSampleCount(channel)-1)</code>
		/// </param>
		/// <returns> the sample's timestamp, in milliseconds
		/// </returns>
		long getMissionSampleTimeStamp(int channel, int sampleNum);
		
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// - Mission Resolution and Range
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		
		/// <summary> Returns all available resolutions for the specified mission channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> all available resolutions for the specified mission channel.
		/// </returns>
		double[] getMissionResolutions(int channel);
		
		/// <summary> Returns the currently selected resolution for the specified
		/// channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> the currently selected resolution for the specified channel.
		/// </returns>
		double getMissionResolution(int channel);
		
		/// <summary> Sets the selected resolution for the specified channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="resolution">the new resolution for the specified channel.
		/// </param>
		void  setMissionResolution(int channel, double resolution);
		
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// - Mission Alarms
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		
		/// <summary> Indicates whether or not the specified channel of this missioning device
		/// has mission alarm capabilities.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <returns> true if the device has mission alarms for the specified channel.
		/// </returns>
		bool hasMissionAlarms(int channel);
		
		/// <summary> Returns true if the specified channel's alarm value of the specified
		/// type has been triggered during the mission.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <returns> true if the alarm was triggered.
		/// </returns>
		bool hasMissionAlarmed(int channel, int alarmType);
		
		/// <summary> Returns true if the alarm of the specified type has been enabled for
		/// the specified channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <returns> true if the alarm of the specified type has been enabled for
		/// the specified channel.
		/// </returns>
		bool getMissionAlarmEnable(int channel, int alarmType);
		
		/// <summary> Enables/disables the alarm of the specified type for the specified channel
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <param name="enable">if true, alarm is enabled.
		/// </param>
		void  setMissionAlarmEnable(int channel, int alarmType, bool enable);
		
		/// <summary> Returns the threshold value which will trigger the alarm of the
		/// specified type on the specified channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <returns> the threshold value which will trigger the alarm
		/// </returns>
		double getMissionAlarm(int channel, int alarmType);
		
		/// <summary> Sets the threshold value which will trigger the alarm of the
		/// specified type on the specified channel.
		/// 
		/// </summary>
		/// <param name="channel">the mission channel, between <code>0</code> and
		/// <code>(getNumberOfMissionChannels()-1)</code>
		/// </param>
		/// <param name="alarmType">valid value: <code>ALARM_HIGH</code> or
		/// <code>ALARM_LOW</code>
		/// </param>
		/// <param name="threshold">the threshold value which will trigger the alarm
		/// </param>
		void  setMissionAlarm(int channel, int alarmType, double threshold);
	}
}