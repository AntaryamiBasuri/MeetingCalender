/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

namespace MeetingCalendar.Interfaces
{
	/// <summary>
	/// Interface for <see cref="ITimeFrame"/>.
	/// </summary>
	public interface ITimeFrame : ITimeSlot
	{
		/// <summary>
		/// Moves the <see cref="ITimeFrame"/> window backward.
		/// </summary>/// <param name="isClearAttendees">Clears the list of attendees, if <c>true</c>; otherwise, <c>false</c>. The default is <c>true</c>.</param>
		void MoveBackward(bool isClearAttendees = true);

		/// <summary>
		/// Moves the <see cref="ITimeFrame"/> window forward.
		/// </summary>/// <param name="isClearAttendees">Clears the list of attendees, if <c>true</c>; otherwise, <c>false</c>. The default is <c>true</c>.</param>
		void MoveForward(bool isClearAttendees = true);
	}
}