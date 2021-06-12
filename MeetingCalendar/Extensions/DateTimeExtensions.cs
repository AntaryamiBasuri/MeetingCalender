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
		/// Calibrates the date time without its seconds and milliseconds component.
		/// </summary>
		/// <param name="dateTime">The date time instance.</param>
		/// <returns>A new calibrated date time instance.</returns>
		internal static DateTime CalibrateToMinutes(this DateTime dateTime)
			=> new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, dateTime.Kind);

		/// <summary>
		/// Validates a date time against Min and Max value of DateTi.me.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>True, if the value is not DateTime.MinValue or DateTime.MaxValue, true otherwise.</returns>
		internal static bool IsInvalidDate(this DateTime dateTime)
			=> dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue;
	}
}