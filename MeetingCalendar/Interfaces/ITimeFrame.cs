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
		/// </summary>/// <param name="clearAttendees">Clears the list of attendees, if <c>true</c>; otherwise, <c>false</c>. The default is <c>true</c>.</param>
		void MoveBackward(bool clearAttendees = true);

		/// <summary>
		/// Moves the <see cref="ITimeFrame"/> window forward.
		/// </summary>/// <param name="clearAttendees">Clears the list of attendees, if <c>true</c>; otherwise, <c>false</c>. The default is <c>true</c>.</param>
		void MoveForward(bool clearAttendees = true);
	}
}