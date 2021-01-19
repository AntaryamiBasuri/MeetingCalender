/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using System.Collections.Generic;

namespace MeetingCalendar.Interfaces
{
    /// <summary>
    /// Interface for <see cref="Calendar"/>
    /// </summary>
    public interface ICalendar
    {
        /// <summary>
        /// Gets the list of attendees.
        /// </summary>
        IEnumerable<Attendee> Attendees { get; }

        /// <summary>
        /// Add attendees to the calender.
        /// </summary>
        void AddAttendees(IEnumerable<Attendee> attendees);
        /// <summary>
        /// Appends additional attendees to existing attendees list.
        /// </summary>
        void AppendAttendees(IEnumerable<Attendee> additionalAttendees);
        /// <summary>
        /// Returns the first slot available for the requested meeting duration.
        /// </summary>
        /// <param name="meetingDuration">The meeting duration in minutes.</param>
        /// <returns>A time slot</returns>
        TimeSlot GetFirstAvailableSlot(int meetingDuration);
        /// <summary>
        /// Find all available meeting slots.
        /// </summary>
        /// <returns>A list of <see cref="TimeSlot"/></returns>
        IEnumerable<TimeSlot> GetAllAvailableTimeSlots();
    }
}
