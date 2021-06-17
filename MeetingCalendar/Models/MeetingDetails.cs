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
	/// Provides information about the details of a meeting.
	/// </summary>
	public class MeetingDetails : MeetingInfo, IMeetingDetails
	{
		private readonly IEnumerable<IAttendee> _attendees;

		/// <summary>
		/// Gets the title of the <see cref="MeetingDetails"/>.
		/// </summary>
		public string MeetingTitle { get; }

		/// <summary>
		/// Gets the location/venue of the <see cref="MeetingDetails"/>.
		/// </summary>
		public string MeetingLocation { get; }

		/// <summary>
		/// Gets the agenda of the <see cref="MeetingDetails"/>.
		/// </summary>
		public string MeetingAgenda { get; }

		//TODO: Make it read only
		/// <summary>
		/// Gets a list of Attendees associated with the <see cref="MeetingDetails"/>.
		/// </summary>
		public IList<IAttendee> Attendees => _attendees?.ToList().AsReadOnly();

		/// <summary>
		/// Gets or sets a list of file paths as Attachment associated with the <see cref="MeetingDetails"/>.
		/// </summary>
		public IList<string> AttachmentFilePaths { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MeetingDetails"/> class.
		/// </summary>
		/// <param name="startTime">The meeting start time.</param>
		/// <param name="endTime">The meeting finish time.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The <see cref="MeetingDetails"/> start time is invalid.
		/// The <see cref="MeetingDetails"/> finish time is invalid.
		/// The <see cref="MeetingDetails"/> start time is greater than or equals to start time.
		/// </exception>
		[Obsolete("Use any latest overloaded constructor instead.")]
		public MeetingDetails(DateTime startTime, DateTime endTime)
			: base(startTime, endTime)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MeetingDetails"/> class.
		/// </summary>
		/// <param name="timeSlot">The <see cref="ITimeSlot"/> for the <see cref="MeetingDetails"/>.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The <see cref="MeetingDetails"/> start time is invalid.
		/// The <see cref="MeetingDetails"/> finish time is invalid.
		/// The <see cref="MeetingDetails"/> start time is greater than or equals to start time.
		/// </exception>
		[Obsolete("Use any latest overloaded constructor instead.")]
		public MeetingDetails(ITimeSlot timeSlot)
			: this(timeSlot.StartTime, timeSlot.EndTime)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MeetingDetails"/> class.
		/// </summary>
		/// <param name="startTime">The start time of the <see cref="MeetingDetails"/>.</param>
		/// <param name="endTime">The finish time of the <see cref="MeetingDetails"/>.</param>
		/// <param name="meetingTitle">The title of <see cref="MeetingDetails"/>.</param>
		/// <param name="meetingAgenda">The agenda of <see cref="MeetingDetails"/>.</param>
		/// <param name="attendees">A list of <see cref="IAttendee"/>.</param>
		/// <param name="attachmentFilePaths">A list of file paths as attachment files.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The <see cref="MeetingDetails"/> start time is invalid.
		/// The <see cref="MeetingDetails"/> finish time is invalid.
		/// The <see cref="MeetingDetails"/> start time is greater than or equals to start time.
		/// </exception>
		[Obsolete("Use any latest overloaded constructor instead.")]
		public MeetingDetails(DateTime startTime, DateTime endTime, string meetingTitle, string meetingAgenda, IEnumerable<IAttendee> attendees, IList<string> attachmentFilePaths = null)
			: this(startTime, endTime)
		{
			MeetingTitle = meetingTitle;
			MeetingAgenda = meetingAgenda;
			AttachmentFilePaths = attachmentFilePaths;

			_attendees = attendees;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MeetingDetails"/> class.
		/// </summary>
		/// <param name="startTime">The start time of the <see cref="MeetingDetails"/>.</param>
		/// <param name="endTime">The finish time of the <see cref="MeetingDetails"/>.</param>
		/// <param name="meetingTitle">The title of <see cref="MeetingDetails"/>.</param>
		/// <param name="meetingLocation">The location of the <see cref="MeetingDetails"/>.</param>
		/// <param name="meetingAgenda">The agenda of <see cref="MeetingDetails"/>.</param>
		/// <param name="attendees">A list of <see cref="IAttendee"/>.</param>
		/// <param name="attachmentFilePaths">A list of file paths as attachment files.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The <see cref="MeetingDetails"/> start time is invalid.
		/// The <see cref="MeetingDetails"/> finish time is invalid.
		/// The <see cref="MeetingDetails"/> start time is greater than or equals to start time.
		/// </exception>
		public MeetingDetails(DateTime startTime, DateTime endTime, string meetingTitle, string meetingLocation, string meetingAgenda, IEnumerable<IAttendee> attendees, IList<string> attachmentFilePaths = null)
			: this(startTime, endTime)
		{
			MeetingTitle = meetingTitle;
			MeetingAgenda = meetingAgenda;
			AttachmentFilePaths = attachmentFilePaths;
			MeetingLocation = meetingLocation;

			_attendees = attendees;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MeetingDetails"/> class.
		/// </summary>
		/// <param name="timeSlot">The <see cref="ITimeSlot"/> for the <see cref="MeetingDetails"/>.</param>
		/// <param name="meetingTitle">The title of the <see cref="MeetingDetails"/>.</param>
		/// <param name="meetingLocation">The location of the <see cref="MeetingDetails"/>.</param>
		/// <param name="meetingAgenda">The agenda of the <see cref="MeetingDetails"/>.</param>
		/// <param name="attendees">A list of <see cref="IAttendee"/>.</param>
		/// <param name="attachmentFilePaths">A list of file paths as attachment files.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The <see cref="MeetingDetails"/> start time is invalid.
		/// The <see cref="MeetingDetails"/> finish time is invalid.
		/// The <see cref="MeetingDetails"/> start time is greater than or equals to start time.
		/// </exception>
		public MeetingDetails(ITimeSlot timeSlot, string meetingTitle, string meetingLocation, string meetingAgenda, IEnumerable<IAttendee> attendees, IList<string> attachmentFilePaths = null)
			: this(timeSlot.StartTime, timeSlot.EndTime)
		{
			MeetingTitle = meetingTitle;
			MeetingAgenda = meetingAgenda;
			AttachmentFilePaths = attachmentFilePaths;
			MeetingLocation = meetingLocation;

			_attendees = attendees;
		}
	}
}