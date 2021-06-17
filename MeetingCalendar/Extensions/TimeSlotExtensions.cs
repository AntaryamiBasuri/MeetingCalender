/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Interfaces;
using MeetingCalendar.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MeetingCalendarTest")]

namespace MeetingCalendar.Extensions
{
	/// <summary>
	/// Extension method for an <see cref="ITimeSlot"/>.
	/// </summary>
	internal static class TimeSlotExtensions
	{
		/// <summary>
		/// Gets the duration of a <see cref="ITimeSlot"/> in minutes.
		/// </summary>
		/// <param name="timeSlot">The time slot.</param>
		/// <returns>The duration.</returns>
		internal static double GetDuration(this ITimeSlot timeSlot)
			=> timeSlot.StartTime.Equals(timeSlot.EndTime) ? 0 : timeSlot.EndTime.Subtract(timeSlot.StartTime).TotalMinutes + 1;

		/// <summary>
		/// Maps a <see cref="TimeSlot"/> of a meeting to Time frame of the the calendar.
		/// </summary>
		/// <param name="timeSlot">The Meeting.</param>
		/// <param name="calendarStartTime">The start time of the calendar.</param>
		/// <param name="calendarEndTime">The end time of the calendar.</param>
		/// <returns>A new Time slot mapped to calendar time frame.</returns>
		internal static TimeSlot GetTimeSlotMappedToCalenderTimeFrame(this ITimeSlot timeSlot, DateTime calendarStartTime, DateTime calendarEndTime)
		{
			var (startTime, endTime) = timeSlot;

			//Outside of calendar time frame
			if (endTime <= calendarStartTime || startTime >= calendarEndTime)
			{
				return null;
			}

			return new TimeSlot(
				(startTime >= calendarStartTime) ? startTime : calendarStartTime,
				(endTime <= calendarEndTime) ? endTime : calendarEndTime);
		}

		/// <summary>
		/// Calculates the scheduled meeting durations only within the time frame of Calendar.
		/// </summary>
		/// <param name="timeSlot">The Time Slot.</param>
		/// <param name="isScheduled">The value indicating whether the time is marked as scheduled or not.</param>
		/// <param name="seriesStartTime">The start time of time series.</param>
		/// <returns></returns>
		internal static ConcurrentDictionary<DateTime, bool> GetTimeSeriesByMinutes(this ITimeSlot timeSlot, bool isScheduled = false, DateTime seriesStartTime = default)
		{
			var timeRange = new ConcurrentDictionary<DateTime, bool>();

			if (timeSlot == null) return timeRange;

			var temp = (seriesStartTime == default) ? timeSlot.StartTime : seriesStartTime;
			while (temp < timeSlot.EndTime)
			{
				timeRange.TryAdd(temp, isScheduled);
				temp = temp.AddMinutes(1);
			}

			return timeRange;
		}

		/// <summary>
		/// Finds the first element that matches the search criteria.
		/// </summary>
		/// <param name="source">The source list.</param>
		/// <param name="predicate">The search criteria.</param>
		/// <returns>A time slot or null.</returns>
		internal static ITimeSlot FindFirst(this IEnumerable<ITimeSlot> source, Func<ITimeSlot, bool> predicate)
		{
			static bool IncludeAllPredicate(ITimeSlot t) => true;
			var filter = predicate ?? IncludeAllPredicate;

			return source?
				.Where(filter)
				.OrderBy(o => o.GetDuration())
				.ThenBy(i => i.StartTime)
				.FirstOrDefault();
		}
	}
}