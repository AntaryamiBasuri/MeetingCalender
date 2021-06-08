using System.Collections.Generic;

namespace MeetingCalendar.Interfaces
{
	/// <summary>
	/// Interface for <see cref="IMeetingDetails"/>
	/// </summary>
	public interface IMeetingDetails
	{
		/// <summary>
		/// Gets or sets the title of the <see cref="IMeetingDetails"/>.
		/// </summary>
		string MeetingTitle { get; }

		/// <summary>
		/// Gets or sets the agenda of the <see cref="IMeetingDetails"/>.
		/// </summary>
		string MeetingAgenda { get; }

		/// <summary>
		/// Gets or sets a list of Attendees associated with the <see cref="IMeetingDetails"/>.
		/// </summary>
		IList<IAttendee> Attendees { get; }

		/// <summary>
		/// Gets or sets a list of file paths as Attachment associated with the <see cref="IMeetingDetails"/>.
		/// </summary>
		IList<string> AttachmentFilePaths { get; set; }
	}
}