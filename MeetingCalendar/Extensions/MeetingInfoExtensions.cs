/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Models;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MeetingCalendarTest")]

namespace MeetingCalendar.Extensions
{
	/// <summary>
	/// Extension method for an <see cref="MeetingInfo"/>
	/// </summary>
	internal static class MeetingInfoExtensions
	{
		/// <summary>
		/// Evaluates whether the meeting is over or not.
		/// </summary>
		/// <param name="meeting"></param>
		/// <returns>Returns True if the meeting is over, false otherwise.</returns>
		internal static bool IsOver(this MeetingInfo meeting) => meeting.EndTime <= DateTime.Now.CalibrateToMinutes();

		/// <summary>
		/// Maps a <see cref="TimeSlot"/> of a meeting to Time frame of the the calendar.
		/// </summary>
		/// <param name="meeting">The Meeting.</param>
		/// <param name="calendarStartTime">The start time of the calendar.</param>
		/// <param name="calendarEndTime">The end time of the calendar.</param>
		/// <returns>A new Time slot mapped to calendar time frame</returns>
		internal static TimeSlot GetTimeSlotMappedToCalenderTimeFrame(this MeetingInfo meeting, DateTime calendarStartTime, DateTime calendarEndTime)
		{
			//Outside of calendar time frame
			if (meeting.EndTime <= calendarStartTime || meeting.StartTime >= calendarEndTime)
			{
				return null;
			}

			return new TimeSlot(
				(meeting.StartTime >= calendarStartTime) ? meeting.StartTime : calendarStartTime,
				(meeting.EndTime <= calendarEndTime) ? meeting.EndTime : calendarEndTime
			);
		}
	}
}