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
	/// Interface for <see cref="ICalendar"/>.
	/// </summary>
	public interface ICalendar : ITimeSlot
	{
		/// <summary>
		/// Gets the current date and time calibrated to minutes.
		/// </summary>
		DateTime CurrentTime { get; }

		/// <summary>
		/// Gets the list of attendees.
		/// </summary>
		IEnumerable<IAttendee> Attendees { get; }

		/// <summary>
		/// Add attendees to the calendar.
		/// </summary>
		/// <param name="attendees">The attendees.</param>
		void AddAttendees(IEnumerable<IAttendee> attendees);

		/// <summary>
		/// Appends a list of additional attendees to the existing attendees list.
		/// </summary>
		/// <param name="additionalAttendees">The additional attendees.</param>
		void AppendAttendees(IEnumerable<IAttendee> additionalAttendees);

		/// <summary>
		/// Finds the first available time slot for the requested meeting duration.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration in minutes.</param>
		/// <returns>A time slot or null.</returns>
		ITimeSlot FindFirstAvailableSlot(int meetingDuration);

		/// <summary>
		/// Finds the first available time slot for the requested meeting duration within a specific time frame.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration in minutes.</param>
		/// <param name="fromTime">The lower bound date time for a search.</param>
		/// <param name="toTime">The upper bound date time for a search.</param>
		/// <returns>A <see cref="TimeSlot"/> or null.</returns>
		ITimeSlot FindFirstAvailableSlot(int meetingDuration, DateTime fromTime, DateTime toTime = default);

		/// <summary>
		/// Find all available meeting slots.
		/// </summary>
		/// <returns>A list of <see cref="TimeSlot"/>.</returns>
		IEnumerable<ITimeSlot> GetAllAvailableTimeSlots();
	}
}