/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MeetingCalendar.Tests")]

namespace MeetingCalendar.Extensions
{
	/// <summary>
	/// Provide extension methods for an <see cref="IDictionary{TKey,TValue}"/> instance.
	/// </summary>
	internal static class DictionaryExtensions
	{
		/// <summary>
		/// Fill with the value <c>U</c>.
		/// </summary>
		/// <typeparam name="TKey">The DateTime or any <c>struct</c> type.</typeparam>
		/// <typeparam name="TValue">The AvailabilityTypes or any <see cref="Enum"/> type.</typeparam>
		/// <param name="timeSeries">The time series.</param>
		/// <param name="availabilityValue">The scheduled or not.</param>
		/// <returns>A <see cref="IDictionary{TKey,TValue}"/>.</returns>
		internal static IDictionary<TKey, TValue> FillWith<TKey, TValue>(this IDictionary<TKey, TValue> timeSeries, TValue availabilityValue)
			where TKey : struct
			where TValue : Enum
		{
			if (timeSeries == null)
			{
				throw new ArgumentNullException(nameof(timeSeries), "The timeSeries parameter can not be null.");
			}

			timeSeries.ForEach(item => timeSeries[item.Key] = availabilityValue);

			return timeSeries;
		}

		/// <summary>
		/// Converts an <see cref="IDictionary{TKey, TValue}"/> to a <see cref="ConcurrentDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <typeparam name="TKey">The DateTime or any <c>struct</c> type.</typeparam>
		/// <typeparam name="TValue">The AvailabilityTypes or any <see cref="Enum"/> type.</typeparam>
		/// <param name="timeSeries">The time series.</param>
		/// <returns>A <see cref="ConcurrentDictionary{TKey,TValue}"/>.</returns>
		internal static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(this IDictionary<TKey, TValue> timeSeries)
			where TKey : struct
			where TValue : Enum
		{
			if (timeSeries == null)
			{
				throw new ArgumentNullException(nameof(timeSeries), "The timeSeries parameter can not be null.");
			}

			return new ConcurrentDictionary<TKey, TValue>(timeSeries);
		}
	}
}