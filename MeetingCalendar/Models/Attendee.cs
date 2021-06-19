/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingCalendar.Models
{
	/// <summary>
	/// Provides information about an <see cref="Attendee"/> along with its meeting details.
	/// </summary>
	public class Attendee : IAttendee
	{
		private readonly IEnumerable<IMeetingInfo> _meetings;

		/// <summary>
		/// Gets or sets the Id of the <see cref="Attendee"/>.
		/// </summary>
		public Guid AttendeeId { get; set; }

		/// <summary>
		/// Gets the name of the <see cref="Attendee"/>.
		/// </summary>
		public string AttendeeName { get; }

		/// <summary>
		/// Gets the mobile number or phone number of the <see cref="Attendee"/>.
		/// </summary>
		public string PhoneNumber { get; }

		/// <summary>
		/// Gets the email id of the <see cref="Attendee"/>.
		/// </summary>
		public string AttendeeEmailId { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="Attendee"/> an optional or mandatory.
		/// </summary>
		public bool IsOptionalAttendee { get; }

		/// <summary>
		/// Gets a list of <see cref="MeetingInfo"/> associated with the <see cref="Attendee"/>.
		/// </summary>
		public IReadOnlyCollection<IMeetingInfo> Meetings => _meetings.ToList().AsReadOnly();

		/// <summary>
		/// Initializes a new instance of the <see cref="Attendee"/> class.
		/// </summary>
		/// <param name="attendeeName">The name of the <see cref="IAttendee"/>.</param>
		/// <param name="meetings">The list of <see cref="IMeetingInfo"/>.</param>
		public Attendee(string attendeeName, IEnumerable<IMeetingInfo> meetings)
		{
			AttendeeName = attendeeName;
			_meetings = meetings ?? new List<IMeetingInfo>();
		}

		//TODO: Remove the below constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="Attendee"/> class.
		/// </summary>
		/// <param name="attendeeName">The name of the <see cref="IAttendee"/>.</param>
		/// <param name="isOptionalAttendee">The flag to mark the <see cref="IAttendee"/> as optional, mandatory otherwise.</param>
		/// <param name="meetings">A list of <see cref="IMeetingInfo"/>.</param>
		/// <param name="attendeeEmailId">THe email id of the <see cref="IAttendee"/>.</param>
		[Obsolete("Use any latest overloaded constructor instead.")]
		public Attendee(string attendeeName, string attendeeEmailId, bool isOptionalAttendee, IEnumerable<IMeetingInfo> meetings) :
			this(attendeeName, meetings)
		{
			AttendeeEmailId = attendeeEmailId;
			IsOptionalAttendee = isOptionalAttendee;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Attendee"/> class.
		/// </summary>
		/// <param name="attendeeName">The name of the <see cref="IAttendee"/>.</param>
		/// <param name="isOptionalAttendee">The flag to mark the <see cref="IAttendee"/> as optional, mandatory otherwise.</param>
		/// <param name="meetings">A list of <see cref="IMeetingInfo"/>.</param>
		/// <param name="attendeeEmailId">THe email id of the <see cref="IAttendee"/>.</param>
		/// <param name="phoneNumber">The mobile number of the <see cref="IAttendee"/>.</param>
		public Attendee(string attendeeName, string attendeeEmailId, string phoneNumber, bool isOptionalAttendee, IEnumerable<IMeetingInfo> meetings) :
			this(attendeeName, meetings)
		{
			AttendeeEmailId = attendeeEmailId;
			IsOptionalAttendee = isOptionalAttendee;
			PhoneNumber = phoneNumber;
		}
	}
}