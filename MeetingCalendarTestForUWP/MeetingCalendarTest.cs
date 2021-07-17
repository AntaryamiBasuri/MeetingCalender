
using System;
using System.Collections.Generic;
using System.Linq;
using MeetingCalendar;
using MeetingCalendar.Interfaces;
using MeetingCalendar.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MeetingCalendarTestForUWP
{
	[TestClass]
	public class MeetingCalendarTest
	{
		[TestMethod]
		public void CalendarTest()
		{
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(3);

			var attendeesWithMeetingTimings = new List<IAttendee>
			{
				new Attendee("Person1", new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now.AddMinutes(5), DateTime.Now.AddMinutes(7)),
					new MeetingInfo(DateTime.Now.AddMinutes(12), DateTime.Now.AddMinutes(18))
				}),
				new Attendee("Person2", new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now.AddMinutes(6), DateTime.Now.AddMinutes(10)),
					new MeetingInfo(DateTime.Now.AddMinutes(15), DateTime.Now.AddMinutes(20))
				})
			};

			var meetingCalendar = new Calendar(startTime, endTime, attendeesWithMeetingTimings);

			Assert.IsTrue(meetingCalendar.Attendees.Count == 2);
			Assert.IsTrue(meetingCalendar.GetAllAvailableTimeSlots().Count() == 3);
		}
	}
}
