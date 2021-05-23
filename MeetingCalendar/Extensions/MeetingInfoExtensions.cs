/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Models;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MeetingCalendarTest")]

namespace MeetingCalendar.Extensions
{
	/// <summary>
	/// Extension method for an <see cref="MeetingInfo"/>
	/// </summary>
	internal static class MeetingInfoExtensions
	{
		/// <summary>
		/// Returns True if the meeting is over, false otherwise.
		/// </summary>
		/// <param name="meeting"></param>
		/// <returns></returns>
		internal static bool IsOver(this MeetingInfo meeting) => meeting.EndTime <= DateTime.Now.CalibrateToMinutes();
	}
}