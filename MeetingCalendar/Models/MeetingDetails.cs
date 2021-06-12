/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Interfaces;
using System;
using System.Collections.Generic;

namespace MeetingCalendar.Models
{
	/// <summary>
	/// Provides information about the details of a meeting.
	/// </summary>
	public class MeetingDetails : MeetingInfo, IMeetingDetails
	{
		/// <summary>
		/// Gets the title of the <see cref="MeetingDetails"/>.
		/// </summary>
		public string MeetingTitle { get; }

		/// <summary>
		/// Gets the agenda of the <see cref="MeetingDetails"/>.
		/// </summary>
		public string MeetingAgenda { get; }

		/// <summary>
		/// Gets a list of Attendees associated with the <see cref="MeetingDetails"/>.
		/// </summary>
		public IList<IAttendee> Attendees { get; }

		/// <summary>
		/// Gets or sets a list of file paths as Attachment associated with the <see cref="MeetingDetails"/>.
		/// </summary>
		public IList<string> AttachmentFilePaths { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MeetingDetails"/> class.
		/// </summary>
		/// <param name="startTime">The meeting start time.</param>
		/// <param name="endTime">The meeting end time.</param>
		public MeetingDetails(DateTime startTime, DateTime endTime)
			: base(startTime, endTime)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MeetingDetails"/> class.
		/// </summary>
		/// <param name="startTime">The meeting start time.</param>
		/// <param name="endTime">The meeting end time.</param>
		/// <param name="meetingTitle">The title of meeting.</param>
		/// <param name="meetingAgenda">The agenda of meeting.</param>
		/// <param name="attendees">A list of Attendees.</param>
		/// <param name="attachmentFilePaths">A list of life paths as attachment files.</param>
		public MeetingDetails(DateTime startTime, DateTime endTime, string meetingTitle, string meetingAgenda, IList<IAttendee> attendees, IList<string> attachmentFilePaths = null)
			: this(startTime, endTime)
		{
			MeetingTitle = meetingTitle;
			MeetingAgenda = meetingAgenda;
			Attendees = attendees;
			AttachmentFilePaths = attachmentFilePaths;
		}
	}
}