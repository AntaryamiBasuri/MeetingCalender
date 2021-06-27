/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MeetingCalendar.Tests")]

namespace MeetingCalendar.Extensions
{
	/// <summary>
	/// Provide extension methods for an <see cref="ICollection{T}"/> instance.
	/// </summary>
	internal static class CollectionExtensions
	{
		/// <summary>
		/// Concatenate two <see cref="ICollection{T}"/>.
		/// </summary>
		/// <param name="first">The first source to concatenate.</param>
		/// <param name="second">The second source to concatenate.</param>
		/// <typeparam name="T">The type of the source collection.</typeparam>
		/// <returns>The first source with items concatenated from second source.</returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown when:
		/// The first source is null.
		/// The second source is null.
		/// </exception>
		internal static ICollection<T> Concat<T>(this ICollection<T> first, ICollection<T> second)
		{
			if (first == null)
			{
				throw new ArgumentNullException(nameof(first), "The collection parameter can not be null.");
			}

			if (second == null)
			{
				throw new ArgumentNullException(nameof(second), "The collection parameter can not be null.");
			}

			second.ForEach(first.Add);

			return first;
		}
	}
}