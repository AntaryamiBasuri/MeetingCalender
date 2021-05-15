/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using System.Collections.Generic;

namespace MeetingCalendar
{
	/// <summary>
	/// Provides information about an <see cref="Attendee"/> along with its meeting details.
	/// </summary>
	public class Attendee
	{
		public string AttendeeName { get; }
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
	}
}