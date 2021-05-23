/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Models;
using NUnit.Framework;
using System;

namespace MeetingCalendarTest.Exceptions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class TimeSlotExceptionTests
	{
		[Test]
		public void Constructor_Throws_Exception_When_TimeSlot_StartTime_Is_Invalid()
			=> Assert.Throws<ArgumentException>(() => { _ = new TimeSlot(DateTime.MinValue, DateTime.Now); });

		[Test]
		public void Constructor_Throws_Exception_When_TimeSlot_EndTime_Is_Invalid()
			=> Assert.Throws<ArgumentException>(() => { _ = new TimeSlot(DateTime.Now, DateTime.MaxValue); });

		[Test]
		public void Constructor_Throws_Exception_When_TimeSlot_StartTime_Is_GreaterThan_EndTime()
			=> Assert.Throws<ArgumentException>(() => { _ = new TimeSlot(DateTime.Now.AddMinutes(10), DateTime.Now); });
	}
}