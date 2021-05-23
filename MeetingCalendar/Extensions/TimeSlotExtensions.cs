/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Models;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MeetingCalendarTest")]

namespace MeetingCalendar.Extensions
{
	/// <summary>
	/// Extension method for an <see cref="TimeSlot"/>
	/// </summary>
	internal static class TimeSlotExtensions
	{
		/// <summary>
		/// Gets the duration of a <see cref="TimeSlot"/> in minutes.
		/// </summary>
		internal static double GetDuration(this TimeSlot timeSlot)
			=> timeSlot.StartTime.Equals(timeSlot.EndTime) ? 0 : timeSlot.EndTime.Subtract(timeSlot.StartTime).TotalMinutes + 1;
	}
}