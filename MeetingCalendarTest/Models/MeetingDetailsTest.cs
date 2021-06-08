/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

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
		public void Constructor_Sets_Properties()
		{
			const string meetingTitle = "QA Meeting";
			const string meetingAgenda = "Unit Testing code Coverage";
			var meetingStartTime = DateTime.Now.AddMinutes(15);
			var meetingEndTime = DateTime.Now.AddMinutes(45);

			var meetingDetails = new MeetingDetails(DateTime.Now, DateTime.Now, meetingTitle, meetingAgenda,
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

			Assert.That(meetingDetails.MeetingTitle, Is.EqualTo(meetingTitle));
			Assert.That(meetingDetails.MeetingAgenda, Is.EqualTo(meetingAgenda));
			Assert.That(meetingDetails.Attendees.Count, Is.EqualTo(2));
			Assert.That(meetingDetails.AttachmentFilePaths, Is.Null);
			Assert.That(meetingDetails.MeetingId, Is.EqualTo(newId));
		}
	}
}