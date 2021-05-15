/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MeetingCalendarTest")]

namespace MeetingCalendar.Extensions
{
	/// <summary>
	/// Extension class for <see cref="DateTime"/> type.
	/// </summary>
	internal static class DateTimeExtensions
	{
		/// <summary>
		/// Calibrates the datetime without its seconds and milliseconds component.
		/// </summary>
		/// <param name="dateTime">The date time instance</param>
		/// <returns>A new calibrated datetime instance.</returns>

		internal static DateTime CalibrateToMinutes(this DateTime dateTime)
			=> new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, dateTime.Kind);
	}
}