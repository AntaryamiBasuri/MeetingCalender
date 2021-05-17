/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using NUnit.Framework;
using System;

namespace MeetingCalendarTest
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class DateTimeExtensionTests
	{
		[Test]
		public void DateTime_Comparision_Fails_When_DateTime_Subtraction_Used()
		{
			var input = DateTime.Now;

			var expected = input.AddSeconds(-input.Second).AddMilliseconds(-input.Millisecond);
			var secondExpectedValue = DateTime.Parse(input.ToString("f"));

			var actual = input.CalibrateToMinutes();

			Assert.That(actual, Is.Not.EqualTo(expected));
			Assert.That(DateTime.Compare(actual, expected), Is.Not.Zero);
			Assert.That(expected, Is.Not.EqualTo(secondExpectedValue));
			Assert.That(DateTime.Compare(expected, secondExpectedValue), Is.Not.Zero);

			Assert.That(actual, Is.EqualTo(secondExpectedValue));
			Assert.That(DateTime.Compare(actual, secondExpectedValue), Is.Zero);
		}

		[Test]
		public void CalibrateDateTimeToMinutes()
		{
			var input = DateTime.Now;

			var temp = input.AddSeconds(-input.Second).AddMilliseconds(-input.Millisecond);
			var expected = temp.AddTicks(-(temp.Ticks % TimeSpan.TicksPerSecond));

			var actual = input.CalibrateToMinutes();

			Assert.That(actual, Is.EqualTo(expected));
			Assert.That(DateTime.Compare(actual, expected), Is.Zero);
		}

		[Test]
		public void CalibrateUtcDateTimeToMinutes()
		{
			var input = DateTime.UtcNow;

			var temp = input.AddSeconds(-input.Second).AddMilliseconds(-input.Millisecond);
			var expected = temp.AddTicks(-(temp.Ticks % TimeSpan.TicksPerSecond));

			var actual = input.CalibrateToMinutes();

			Assert.That(actual, Is.EqualTo(expected));
			Assert.That(DateTime.Compare(actual, expected), Is.Zero);
			Assert.That(actual.Kind, Is.EqualTo(expected.Kind));
		}

		[Test]
		public void IsInvalidDate_Returns_True_When_DateTime_Is_DateTimeMinVal_DateTimeMaxVal()
		{
			Assert.That(DateTime.MinValue.IsInvalidDate(), Is.True);
			Assert.That(DateTime.MaxValue.IsInvalidDate(), Is.True);
		}
	}
}