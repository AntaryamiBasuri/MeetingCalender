/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MeetingCalendarTest")]

namespace MeetingCalendar.Extensions
{
	/// <summary>
	/// Extension method for an Enumerable instance to invoke an action.
	/// </summary>
	internal static class EnumerableExtensions
	{
		/// <summary>
		/// Iterates over a list to executes the given Action on each of the items.
		/// </summary>
		/// <param name="source">The instance.</param>
		/// <param name="action">The action to execute.</param>
		/// <typeparam name="T">The generic type parameter.</typeparam>
		internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var item in source)
				action(item);
		}
	}
}