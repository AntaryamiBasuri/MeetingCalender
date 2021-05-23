/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using MeetingCalendar.Models;
using NUnit.Framework;
using System;

namespace MeetingCalendarTest.Extensions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class TimeSlotExtensionsTests
	{
		[Test]
		public void AvailableDuration_Is_Zero_When_StartTime_And_EndTime_Are_Equal()
			=> Assert.That(new TimeSlot(DateTime.Now, DateTime.Now).GetDuration(), Is.Zero);

		[Test]
		public void AvailableDuration_Is_GreaterThan_Zero_When_EndTime_Is_Greater_Than_StartTime()
			=> Assert.That(new TimeSlot(DateTime.Now, DateTime.Now.AddHours(1)).GetDuration, Is.GreaterThan(0));
	}
}