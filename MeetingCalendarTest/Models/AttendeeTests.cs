/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Interfaces;
using MeetingCalendar.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingCalendarTest.Models
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class AttendeeTests
	{
		[Test]
		public void Constructor_Sets_Properties_Without_Meetings()
		{
			var name = "Person11";

			var attendee = new Attendee(name, null);
			var newId = Guid.NewGuid();
			attendee.AttendeeId = newId;

			Assert.That(attendee.AttendeeName, Is.EqualTo(name));
			Assert.That(attendee.Meetings, Is.Not.Null);
			Assert.That(attendee.Meetings.Count, Is.Zero);
		}

		[Test]
		[Obsolete]
		public void Constructor_Sets_Properties()
		{
			var name = "Person11";
			var email = "test@email.com";
			var attendee = new Attendee(name, email, true, new List<IMeetingInfo>
			{
				new MeetingInfo(DateTime.Now, DateTime.Now.AddHours(1))
			});
			var newId = Guid.NewGuid();
			attendee.AttendeeId = newId;

			Assert.That(attendee.AttendeeName, Is.EqualTo(name));
			Assert.That(attendee.AttendeeEmailId, Is.EqualTo(email));
			Assert.That(attendee.IsOptionalAttendee, Is.True);
			Assert.That(attendee.AttendeeId, Is.EqualTo(newId));
		}

		[Test]
		public void Constructor_Sets_All_Properties()
		{
			var name = "Person11";
			var email = "test@email.com";
			var phoneNumber = "1234567890";
			var attendee = new Attendee(name, email, phoneNumber, true, new List<IMeetingInfo>
			{
				new MeetingInfo(DateTime.Now, DateTime.Now.AddHours(1))
			});
			var newId = Guid.NewGuid();
			attendee.AttendeeId = newId;

			Assert.That(attendee.AttendeeName, Is.EqualTo(name));
			Assert.That(attendee.AttendeeEmailId, Is.EqualTo(email));
			Assert.That(attendee.PhoneNumber, Is.EqualTo(phoneNumber));
			Assert.That(attendee.IsOptionalAttendee, Is.True);
			Assert.That(attendee.AttendeeId, Is.EqualTo(newId));
		}

		[Test]
		public void Constructor_Sets_Empty_Attendees_List_When_Null_Value_Passed()
		{
			var name = "Person11";
			var email = "test@email.com";
			var attendee = new Attendee(name, email, "", true, null);

			Assert.That(attendee.Meetings, Is.Not.Null);
			Assert.That(attendee.Meetings.Count(), Is.EqualTo(0));
		}
	}
}