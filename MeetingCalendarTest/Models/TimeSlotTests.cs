/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using MeetingCalendar.Models;
using NUnit.Framework;
using System;

namespace MeetingCalendarTest.Models
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class TimeSlotTests
	{
		[Test]
		public void Constructor_Sets_Properties()
		{
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(8);
			var timeSlot = new TimeSlot(startTime, endTime);

			Assert.That(timeSlot.StartTime, Is.EqualTo(startTime.CalibrateToMinutes()));
			Assert.That(timeSlot.EndTime, Is.EqualTo(endTime.CalibrateToMinutes()));
		}

		[Test]
		public void Deconstruct_Returns_Property_Values()
		{
			var originalStartTime = DateTime.Now;
			var originalEndTime = originalStartTime.AddHours(8);
			var timeSlot = new TimeSlot(originalStartTime, originalEndTime);

			var (startTime, endTime) = timeSlot;

			Assert.That(startTime, Is.EqualTo(timeSlot.StartTime));
			Assert.That(endTime, Is.EqualTo(timeSlot.EndTime));
		}
	}
}