/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar;
using NUnit.Framework;
using System;

namespace MeetingCalendarTest
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class MeetingCalendarExceptionTests
	{
		[Test]
		public void Constructor_Throws_Exception_When_Calender_StartTime_Is_Invalid()
			=> Assert.Throws<ArgumentException>(() => { _ = new Calendar(DateTime.MinValue, DateTime.Now); });

		[Test]
		public void Constructor_Throws_Exception_When_Calender_EndTime_Is_Invalid()
			=> Assert.Throws<ArgumentException>(() => { _ = new Calendar(DateTime.Now, DateTime.MaxValue); });

		[Test]
		public void Constructor_Throws_Exception_When_Calender_StartTime_Is_GreaterThan_EndTime()
			=> Assert.Throws<ArgumentException>(() => { _ = new Calendar(DateTime.Now.AddMinutes(10), DateTime.Now); });
	}
}