/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

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
	/// Extension method for an <see cref="TimeSlot"/>
	/// </summary>
	internal static class TimeSlotExtensions
	{
		/// <summary>
		/// Gets the duration of a <see cref="TimeSlot"/> in minutes.
		/// </summary>
		internal static double GetDuration(this TimeSlot timeSlot)
			=> timeSlot.StartTime.Equals(timeSlot.EndTime) ? 0 : timeSlot.EndTime.Subtract(timeSlot.StartTime).TotalMinutes + 1;

		/// <summary>
		/// Calculates the scheduled meeting durations only within the time frame of Calendar
		/// </summary>
		/// <param name="timeSlot"></param>
		/// <param name="isScheduled"></param>
		/// <returns></returns>
		internal static ConcurrentDictionary<DateTime, bool> GetTimeSeriesByMinutes(this TimeSlot timeSlot, bool isScheduled = false)
		{
			var timeRange = new ConcurrentDictionary<DateTime, bool>();

			if (timeSlot == null) return timeRange;

			var temp = timeSlot.StartTime;
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
		/// <returns>A time slot or null</returns>
		internal static TimeSlot FindFirst(this IEnumerable<TimeSlot> source, Func<TimeSlot, bool> predicate)
		{
			static bool IncludeAllPredicate(TimeSlot t) => true;
			var filter = predicate ?? IncludeAllPredicate;

			return source?
				.Where(filter)
				.OrderBy(o => o.GetDuration())
				.ThenBy(i => i.StartTime)
				.FirstOrDefault();

			//TODO: Find the TImeSlot //.Where(t => DateTime.Now is between Start Time and End Time )
			//TODO: Then Count the duration from Now till EndTime if it is greater than requested Duration then include else discard
		}
	}
}