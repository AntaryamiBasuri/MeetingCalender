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
	public class MeetingInfoExtensionsTests
	{
		[Test]
		public void IsOver_Returns_True_When_Meeting_EndTime_Is_Less_Than_CurrentTime()
			=> Assert.That(new MeetingInfo(DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-1)).IsOver(), Is.True);

		[Test]
		public void AvailableDuration_Is_GreaterThan_Zero_When_EndTime_Is_Greater_Than_StartTime()
			=> Assert.That(new MeetingInfo(DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1)).IsOver(), Is.False);
	}
}