/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using MeetingCalendar.Interfaces;
using MeetingCalendar.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MeetingCalendarTest.Models
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class MeetingDetailsTest
	{
		[Test]
		[Obsolete]
		public void Constructor_Sets_Properties()
		{
			var startTime = DateTime.Now;
			var endTime = DateTime.Now.AddHours(1);
			const string meetingTitle = "QA Meeting";
			const string meetingAgenda = "Unit Testing code Coverage";
			var meetingStartTime = DateTime.Now.AddMinutes(15);
			var meetingEndTime = DateTime.Now.AddMinutes(45);

			var meetingDetails = new MeetingDetails(startTime, endTime, meetingTitle, meetingAgenda,
				new List<IAttendee>()
				{
					new Attendee("Person1", new List<IMeetingInfo>
					{
						new MeetingInfo(meetingStartTime, meetingEndTime)
					}),
					new Attendee("Person2", new List<IMeetingInfo>
					{
						new MeetingInfo(meetingStartTime, meetingEndTime)
					}),
				});

			var newId = Guid.NewGuid();
			meetingDetails.MeetingId = newId;

			Assert.That(meetingDetails.StartTime, Is.EqualTo(startTime.CalibrateToMinutes()));
			Assert.That(meetingDetails.EndTime, Is.EqualTo(endTime.CalibrateToMinutes()));
			Assert.That(meetingDetails.MeetingTitle, Is.EqualTo(meetingTitle));
			Assert.That(meetingDetails.MeetingAgenda, Is.EqualTo(meetingAgenda));
			Assert.That(meetingDetails.Attendees.Count, Is.EqualTo(2));
			Assert.That(meetingDetails.AttachmentFilePaths, Is.Null);
			Assert.That(meetingDetails.MeetingId, Is.EqualTo(newId));
		}

		[Test]
		public void Constructor_Sets_All_Properties()
		{
			var startTime = DateTime.Now;
			var endTime = DateTime.Now.AddHours(1);
			const string meetingTitle = "QA Meeting";
			const string meetingAgenda = "Unit Testing code Coverage";
			var meetingStartTime = DateTime.Now.AddMinutes(15);
			var meetingEndTime = DateTime.Now.AddMinutes(45);
			var meetingLocation = "Online";

			var meetingDetails = new MeetingDetails(startTime, endTime, meetingTitle, meetingLocation, meetingAgenda,
				new List<IAttendee>()
				{
					new Attendee("Person1", new List<IMeetingInfo>
					{
						new MeetingInfo(meetingStartTime, meetingEndTime)
					}),
					new Attendee("Person2", new List<IMeetingInfo>
					{
						new MeetingInfo(meetingStartTime, meetingEndTime)
					}),
				});

			var newId = Guid.NewGuid();
			meetingDetails.MeetingId = newId;

			Assert.That(meetingDetails.StartTime, Is.EqualTo(startTime.CalibrateToMinutes()));
			Assert.That(meetingDetails.EndTime, Is.EqualTo(endTime.CalibrateToMinutes()));
			Assert.That(meetingDetails.MeetingTitle, Is.EqualTo(meetingTitle));
			Assert.That(meetingDetails.MeetingAgenda, Is.EqualTo(meetingAgenda));
			Assert.That(meetingDetails.MeetingLocation, Is.EqualTo(meetingLocation));
			Assert.That(meetingDetails.Attendees.Count, Is.EqualTo(2));
			Assert.That(meetingDetails.AttachmentFilePaths, Is.Null);
			Assert.That(meetingDetails.MeetingId, Is.EqualTo(newId));
		}

		[Test]
		[Obsolete]
		public void Constructor_Sets_Using_TimeSlot()
		{
			var timeSlot = new TimeSlot(DateTime.Now, DateTime.Now.AddHours(1));
			var meetingDetails = new MeetingDetails(timeSlot);

			Assert.That(meetingDetails.StartTime, Is.EqualTo(timeSlot.StartTime));
			Assert.That(meetingDetails.EndTime, Is.EqualTo(timeSlot.EndTime));
		}

		[Test]
		public void Constructor_Sets_Using_TimeSlot_And_Other_Properties()
		{
			var timeSlot = new TimeSlot(DateTime.Now, DateTime.Now.AddHours(1));
			const string meetingTitle = "QA Meeting";
			const string meetingAgenda = "Unit Testing code Coverage";
			var meetingLocation = "Online";

			var meetingDetails = new MeetingDetails(timeSlot, meetingTitle, meetingLocation, meetingAgenda, null);

			var newId = Guid.NewGuid();
			meetingDetails.MeetingId = newId;

			Assert.That(meetingDetails.StartTime, Is.EqualTo(timeSlot.StartTime));
			Assert.That(meetingDetails.EndTime, Is.EqualTo(timeSlot.EndTime));
			Assert.That(meetingDetails.MeetingTitle, Is.EqualTo(meetingTitle));
			Assert.That(meetingDetails.MeetingAgenda, Is.EqualTo(meetingAgenda));
			Assert.That(meetingDetails.MeetingLocation, Is.EqualTo(meetingLocation));
			Assert.That(meetingDetails.Attendees, Is.Null);
			Assert.That(meetingDetails.AttachmentFilePaths, Is.Null);
			Assert.That(meetingDetails.MeetingId, Is.EqualTo(newId));
		}
	}
}