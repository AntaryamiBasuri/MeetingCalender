/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Interfaces;
using MeetingCalendar.Models;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MeetingCalendar.Tests")]

namespace MeetingCalendar.Extensions
{
	/// <summary>
	/// Extension method for an <see cref="MeetingInfo"/>.
	/// </summary>
	internal static class MeetingInfoExtensions
	{
		/// <summary>
		/// Evaluates whether the meeting is over or not.
		/// </summary>
		/// <param name="meeting">The meeting.</param>
		/// <returns>Returns <c>true</c> if the meeting is over, <c>false</c> otherwise.</returns>
		internal static bool IsOver(this IMeetingInfo meeting)
		{
			if (meeting == null)
			{
				throw new ArgumentNullException(nameof(meeting), "The meeting parameter can not be null.");
			}

			return meeting.EndTime <= DateTime.Now.CalibrateToMinutes();
		}
	}
}