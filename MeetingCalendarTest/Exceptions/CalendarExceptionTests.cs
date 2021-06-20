/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar;
using MeetingCalendar.Models;
using NUnit.Framework;
using System;

namespace MeetingCalendarTest.Exceptions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class CalendarExceptionTests
	{
		[Test]
		public void Constructor_Throws_Exception_When_Calendar_StartTime_Is_Invalid()
			=> Assert.Throws<ArgumentException>(() => { _ = new Calendar(DateTime.MinValue, DateTime.Now); });

		[Test]
		public void Constructor_Throws_Exception_When_Calendar_EndTime_Is_Invalid()
			=> Assert.Throws<ArgumentException>(() => { _ = new Calendar(DateTime.Now, DateTime.MaxValue); });

		[Test]
		public void Constructor_Throws_Exception_When_Calendar_StartTime_Is_GreaterThan_EndTime()
			=> Assert.Throws<ArgumentException>(() => { _ = new Calendar(DateTime.Now.AddMinutes(10), DateTime.Now); });

		[Test]
		public void Constructor_With_TimeSlot_Throws_Exception_When_TmeSlot_StartTime_Is_GreaterThan_EndTime()
			=> Assert.Throws<ArgumentException>(() => { _ = new Calendar(new TimeSlot(DateTime.Now.AddMinutes(10), DateTime.Now)); });

		[Test]
		public void FindFirstAvailableSlot_Throws_Exception_When_Meeting_Duration_Is_Invalid()
			=> Assert.Throws<ArgumentException>(() =>
			{
				_ = new Calendar(DateTime.Now, DateTime.Now.AddHours(8))
					.FindFirstAvailableSlot(0, DateTime.Now.AddMinutes(30), DateTime.Now.AddMinutes(60));
			});

		[Test]
		public void FindFirstAvailableSlot_Throws_Exception_When_Search_Duration_StartTime_Is_Invalid()
			=> Assert.Throws<ArgumentException>(() =>
			{
				_ = new Calendar(DateTime.Now, DateTime.Now.AddHours(8))
					.FindFirstAvailableSlot(0, DateTime.MinValue, DateTime.Now.AddMinutes(60));
			});

		[Test]
		public void FindFirstAvailableSlot_Throws_Exception_When_Invalid_Search_Range()
			=> Assert.Throws<ArgumentException>(() =>
			{
				_ = new Calendar(DateTime.Now, DateTime.Now.AddHours(8))
					.FindFirstAvailableSlot(10, DateTime.Now.AddHours(10), DateTime.Now.AddMinutes(60));
			});

		[Test]
		public void FindFirstAvailableSlot_Throws_Exception_When_Search_Range_Is_Invalid()
			=> Assert.Throws<ArgumentException>(() =>
			{
				_ = new Calendar(DateTime.Now.AddHours(-1), DateTime.Now.AddHours(7))
					.FindFirstAvailableSlot(60, DateTime.Now.AddMinutes(30), DateTime.Now.AddMinutes(-30));
			});

		[Test]
		public void FindFirstAvailableSlot_Throws_Exception_When_Search_UB_Is_Invalid()
			=> Assert.Throws<ArgumentException>(() =>
			{
				_ = new Calendar(DateTime.Now.AddHours(-1), DateTime.Now.AddHours(7))
					.FindFirstAvailableSlot(10, DateTime.Now.AddMinutes(-30), DateTime.Now);
			});

		[Test]
		public void FindFirstAvailableSlot_Throws_Exception_When_Meeting_Duration_Is_Longer_Than_Search_Range()
			=> Assert.Throws<ArgumentException>(() =>
			{
				_ = new Calendar(DateTime.Now.AddHours(-1), DateTime.Now.AddHours(7))
					.FindFirstAvailableSlot(60, DateTime.Now.AddMinutes(30), DateTime.Now.AddHours(1));
			});
	}
}