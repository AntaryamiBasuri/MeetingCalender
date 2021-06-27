/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using MeetingCalendar.Models;
using NUnit.Framework;
using System;

namespace MeetingCalendar.Tests.Models
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class MeetingInfoTest
	{
		[Test]
		public void Constructor_Sets_Properties()
		{
			var meetingStartTime = DateTime.Now.AddMinutes(15);
			var meetingEndTime = DateTime.Now.AddMinutes(45);

			var meetingDetails = new MeetingInfo(meetingStartTime, meetingEndTime);

			Assert.That(meetingDetails.StartTime, Is.EqualTo(meetingStartTime.CalibrateToMinutes()));
			Assert.That(meetingDetails.StartTime, Is.EqualTo(meetingStartTime.CalibrateToMinutes()));
		}

		[Test]
		public void Constructor_With_TimesSlot()
		{
			var timeSlot = new TimeSlot(DateTime.Now, DateTime.Now.AddDays(1));
			var meetingInfo = new MeetingInfo(timeSlot);

			Assert.That(meetingInfo.StartTime, Is.EqualTo(timeSlot.StartTime));
			Assert.That(meetingInfo.EndTime, Is.EqualTo(timeSlot.EndTime));
		}
	}
}