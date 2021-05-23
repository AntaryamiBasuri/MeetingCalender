/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MeetingCalendarTest.Models
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class AttendeeTests
	{
		[Test]
		public void Constructor_Sets_Properties()
		{
			var name = "Person11";
			var email = "test@email.com";
			var attendee = new Attendee(name, email, true, new List<MeetingInfo>
			{
				new(DateTime.Now, DateTime.Now.AddHours(1))
			});
			var newId = Guid.NewGuid();
			attendee.AttendeeId = newId;

			Assert.That(attendee.AttendeeName, Is.EqualTo(name));
			Assert.That(attendee.AttendeeEmailId, Is.EqualTo(email));
			Assert.That(attendee.IsOptionalAttendee, Is.True);
			Assert.That(attendee.AttendeeId, Is.EqualTo(newId));
		}
	}
}