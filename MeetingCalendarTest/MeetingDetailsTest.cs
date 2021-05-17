/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MeetingCalendarTest
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
				new List<Attendee>()
				{
					new Attendee("Person1", new List<MeetingInfo>
					{
						new MeetingInfo(meetingStartTime, meetingEndTime)
					}),
					new Attendee("Person2", new List<MeetingInfo>
					{
						new MeetingInfo(meetingStartTime, meetingEndTime)
					}),
				},
				null);

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