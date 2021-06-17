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

		/// <summary>
		/// Deconstruct a <see cref="ITimeSlot"/>.
		/// </summary>
		/// <param name="startTime">The start time.</param>
		/// <param name="endTime">The end time.</param>
		void Deconstruct(out DateTime startTime, out DateTime endTime);
	}
}