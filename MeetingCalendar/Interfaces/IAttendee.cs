using System;
using System.Collections.Generic;

namespace MeetingCalendar.Interfaces
{
	/// <summary>
	/// Interface for <see cref="IAttendee"/>
	/// </summary>
	public interface IAttendee
	{
		/// <summary>
		/// Gets or sets Id of the <see cref="IAttendee"/>.
		/// </summary>
		Guid AttendeeId { get; set; }

		/// <summary>
		/// Gets or sets name of the <see cref="IAttendee"/>.
		/// </summary>
		string AttendeeName { get; }

		/// <summary>
		/// Gets or sets email id of the <see cref="IAttendee"/>.
		/// </summary>
		string AttendeeEmailId { get; }

		/// <summary>
		/// Gets or sets as an optional <see cref="IAttendee"/> or mandatory.
		/// </summary>
		bool IsOptionalAttendee { get; }

		/// <summary>
		/// Gets or sets a list of <see cref="IMeetingInfo"/> associated with the <see cref="IAttendee"/>.
		/// </summary>
		IEnumerable<IMeetingInfo> Meetings { get; }
	}
}