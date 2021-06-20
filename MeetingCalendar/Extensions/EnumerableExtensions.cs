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
	/// Provide extension methods for an <see cref="IEnumerable{T}"/> instance.
	/// </summary>
	internal static class EnumerableExtensions
	{
		/// <summary>
		/// Iterates over a sequence to execute the given Action on each of the items.
		/// </summary>
		/// <param name="source">The instance.</param>
		/// <param name="action">The action to execute.</param>
		/// <typeparam name="T">The generic type parameter.</typeparam>
		internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source), "The source parameter can not be null.");
			}

			if (action == null)
			{
				throw new ArgumentNullException(nameof(action), "Action parameter can not be null.");
			}

			foreach (var item in source)
				action(item);
		}
	}
}