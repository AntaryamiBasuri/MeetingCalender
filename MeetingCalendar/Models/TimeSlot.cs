/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using MeetingCalendar.Interfaces;
using System;

namespace MeetingCalendar.Models
{
	/// <summary>
	/// Provides a <see cref="TimeSlot"/> with a specific start time and end time.
	/// </summary>
	public class TimeSlot : ITimeSlot
	{
		/// <summary>
		/// Gets the start time of a <see cref="TimeSlot"/>.
		/// </summary>
		public DateTime StartTime { get; }

		/// <summary>
		/// Gets the end time of a <see cref="TimeSlot"/>.
		/// </summary>
		public DateTime EndTime { get; }

		/// <summary>
		/// Initializes a new instance of <see cref="TimeSlot"/>
		/// </summary>
		/// <param name="startTime">The <see cref="DateTime"/> from where the time slot starts.</param>
		/// <param name="endTime">The <see cref="DateTime"/> where the time slot ends.</param>
		public TimeSlot(DateTime startTime, DateTime endTime)
		{
			if (startTime.IsInvalidDate())
				throw new ArgumentException("Invalid TimeSlot start time.", nameof(startTime));

			if (endTime.IsInvalidDate())
				throw new ArgumentException("Invalid TimeSlot end time.", nameof(endTime));

			StartTime = startTime.CalibrateToMinutes();
			EndTime = endTime.CalibrateToMinutes();

			if (StartTime > EndTime)
				throw new ArgumentException("The TimeSlot End time must be greater than the start time.", nameof(endTime));
		}

		/// <summary>
		/// Deconstruct a <see cref="TimeSlot"/>.
		/// </summary>
		/// <param name="startTime">The start time</param>
		/// <param name="endTime">The end time</param>
		public void Deconstruct(out DateTime startTime, out DateTime endTime)
		{
			startTime = StartTime;
			endTime = EndTime;
		}
	}
}