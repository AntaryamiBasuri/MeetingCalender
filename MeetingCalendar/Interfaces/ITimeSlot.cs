/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using System;

namespace MeetingCalendar.Interfaces
{
	/// <summary>
	/// Interface for <see cref="ITimeSlot"/>.
	/// </summary>
	public interface ITimeSlot
	{
		/// <summary>
		/// Gets the start time of a <see cref="ITimeSlot"/>.
		/// </summary>
		DateTime StartTime { get; }

		/// <summary>
		/// Gets the end time of a <see cref="ITimeSlot"/>.
		/// </summary>
		DateTime EndTime { get; }
	}
}