/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using System;
using System.Collections.Generic;

namespace MeetingCalendar
{
	/// <summary>
	/// Provides information about an <see cref="Attendee"/> along with its meeting details.
	/// </summary>
	public class Attendee
	{
		/// <summary>
		/// Gets or sets Id of the <see cref="Attendee"/>.
		/// </summary>
		public Guid AttendeeId { get; set; }

		/// <summary>
		/// Gets or sets name of the <see cref="Attendee"/>.
		/// </summary>
		public string AttendeeName { get; }

		/// <summary>
		/// Gets or sets email id of the <see cref="Attendee"/>.
		/// </summary>
		public string AttendeeEmailId { get; }

		/// <summary>
		/// Gets or sets as an optional <see cref="Attendee"/> or mandatory.
		/// </summary>
		public bool IsOptionalAttendee { get; }

		/// <summary>
		/// Gets or sets a list of <see cref="MeetingInfo"/> associated with the <see cref="Attendee"/>.
		/// </summary>
		public IList<MeetingInfo> MeetingInfo { get; }

		/// <summary>
		/// Initializes a new instance of <see cref="Attendee"/>
		/// </summary>
		/// <param name="attendeeName">The name of the Attendee.</param>
		/// <param name="meetingInfo">The <see cref="MeetingInfo"/>.</param>
		public Attendee(string attendeeName, IList<MeetingInfo> meetingInfo)
		{
			AttendeeName = attendeeName;
			MeetingInfo = meetingInfo;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Attendee"/>
		/// </summary>
		/// <param name="attendeeName">The name of the Attendee.</param>
		/// <param name="isOptionalAttendee">The flag to mark the <see cref="Attendee"/> as optional, mandatory otherwise.</param>
		/// <param name="meetingInfo">A list of <see cref="MeetingInfo"/>.</param>
		/// <param name="attendeeEmailId">THe email id of the <see cref="Attendee"/> </param>
		public Attendee(string attendeeName, string attendeeEmailId, bool isOptionalAttendee, IList<MeetingInfo> meetingInfo) :
			this(attendeeName, meetingInfo)
		{
			AttendeeEmailId = attendeeEmailId;
			IsOptionalAttendee = isOptionalAttendee;
		}
	}
}