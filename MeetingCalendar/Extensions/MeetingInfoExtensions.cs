/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Interfaces;
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
		/// Evaluates whether the meeting is over or not.
		/// </summary>
		/// <param name="meeting"></param>
		/// <returns>Returns True if the meeting is over, false otherwise.</returns>
		internal static bool IsOver(this IMeetingInfo meeting) => meeting.EndTime <= DateTime.Now.CalibrateToMinutes();
	}
}