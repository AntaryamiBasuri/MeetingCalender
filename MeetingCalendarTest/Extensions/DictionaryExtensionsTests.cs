/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using MeetingCalendar.Models;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MeetingCalendar.Tests.Extensions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class DictionaryExtensionsTests
	{
		[Test]
		public void FillWith_Fill_All_Values_With_Available()
			=> Assert.That(new TimeSlot(DateTime.Now, DateTime.Now.AddMinutes(5)).GetTimeSeriesByMinutes().FillWith(AvailabilityTypes.Available)
				.Values.All(t => t == AvailabilityTypes.Available), Is.True);

		[Test]
		public void FillWith_Fill_All_Values_As_Scheduled()
			=> Assert.That(new TimeSlot(DateTime.Now, DateTime.Now.AddMinutes(5)).GetTimeSeriesByMinutes().FillWith(AvailabilityTypes.Scheduled)
				.Values.All(t => t == AvailabilityTypes.Scheduled), Is.True);

		[Test]
		public void ToConcurrentDictionary_Returns_ConcurrentDictionary()
			=> Assert.That(
				new Dictionary<DateTime, AvailabilityTypes>().ToConcurrentDictionary().GetType() ==
				typeof(ConcurrentDictionary<DateTime, AvailabilityTypes>), Is.True);
	}
}