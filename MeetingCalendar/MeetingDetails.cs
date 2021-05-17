/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using System;
using System.Collections.Generic;

namespace MeetingCalendar
{
	/// <summary>
	/// Provides information about the details of a meeting.
	/// </summary>
	public class MeetingDetails : MeetingInfo
	{
		/// <summary>
		/// Gets or sets the title of the <see cref="MeetingDetails"/>.
		/// </summary>
		public string MeetingTitle { get; }

		/// <summary>
		/// Gets or sets the agenda of the <see cref="MeetingDetails"/>.
		/// </summary>
		public string MeetingAgenda { get; }

		/// <summary>
		/// Gets or sets a list of Attendees associated with the <see cref="MeetingDetails"/>.
		/// </summary>
		public IList<Attendee> Attendees { get; }

		/// <summary>
		/// Gets or sets a list of file paths as Attachment associated with the <see cref="MeetingDetails"/>.
		/// </summary>
		public IList<string> AttachmentFilePaths { get; set; }

		/// <summary>
		/// Initializes a new instance of <see cref="MeetingInfo"/>
		/// </summary>
		/// <param name="startTime">The meeting start time.</param>
		/// <param name="endTime">The meeting end time.</param>
		public MeetingDetails(DateTime startTime, DateTime endTime)
			: base(startTime, endTime)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="MeetingInfo"/>
		/// </summary>
		/// <param name="startTime">The meeting start time.</param>
		/// <param name="endTime">The meeting end time.</param>
		/// <param name="meetingTitle">The title of meeting</param>
		/// <param name="meetingAgenda">The agenda of meeting</param>
		/// <param name="attendees">A list of Attendees</param>
		/// <param name="attachmentFilePaths">A list of life paths as attachment files</param>
		public MeetingDetails(DateTime startTime, DateTime endTime, string meetingTitle, string meetingAgenda, IList<Attendee> attendees, IList<string> attachmentFilePaths = null)
			: this(startTime, endTime)
		{
			MeetingTitle = meetingTitle;
			MeetingAgenda = meetingAgenda;
			Attendees = attendees;
			AttachmentFilePaths = attachmentFilePaths;
		}
	}
}