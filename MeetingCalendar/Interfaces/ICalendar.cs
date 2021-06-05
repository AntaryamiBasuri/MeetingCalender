/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Models;
using System;
using System.Collections.Generic;

namespace MeetingCalendar.Interfaces
{
	/// <summary>
	/// Interface for <see cref="Calendar"/>
	/// </summary>
	public interface ICalendar
	{
		/// <summary>
		/// Gets the current date and time calibrated to minutes.
		/// </summary>
		DateTime CurrentTime { get; }

		/// <summary>
		/// Gets the list of attendees.
		/// </summary>
		IEnumerable<Attendee> Attendees { get; }

		/// <summary>
		/// Add attendees to the calendar.
		/// </summary>
		void AddAttendees(IEnumerable<Attendee> attendees);

		/// <summary>
		/// Appends additional attendees to the existing attendees list.
		/// </summary>
		void AppendAttendees(IEnumerable<Attendee> additionalAttendees);

		/// <summary>
		/// Returns the first available time slot for the requested meeting duration.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration in minutes.</param>
		/// <returns>A time slot</returns>
		[Obsolete("Use FindFirstAvailableSlot instead.")]
		TimeSlot GetFirstAvailableSlot(int meetingDuration);

		/// <summary>
		/// Finds the first available time slot for the requested meeting duration.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration in minutes.</param>
		/// <returns>A time slot or null</returns>

		TimeSlot FindFirstAvailableSlot(int meetingDuration);

		/// <summary>
		/// Finds the first available time slot for the requested meeting duration within a specific time frame.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration in minutes.</param>
		/// <param name="fromTime">The lower bound date time for a search.</param>
		/// <param name="toTime">The upper bound date time for a search.</param>
		/// <returns>A time slot or null</returns>
		TimeSlot FindFirstAvailableSlot(int meetingDuration, DateTime fromTime, DateTime toTime = default);

		/// <summary>
		/// Find all available meeting slots.
		/// </summary>
		/// <returns>A list of <see cref="TimeSlot"/></returns>
		IEnumerable<TimeSlot> GetAllAvailableTimeSlots();
	}
}