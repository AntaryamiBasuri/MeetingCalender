/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Interfaces;
using MeetingCalendar.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MeetingCalendar.Tests.Models
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class AttendeeTests
	{
		[Test]
		public void Constructor_Sets_Properties_Without_Meetings()
		{
			const string name = "Person11";

			var attendee = new Attendee(name, null);
			var newId = Guid.NewGuid();
			attendee.AttendeeId = newId;

			Assert.That(attendee.AttendeeName, Is.EqualTo(name));
			Assert.That(attendee.Meetings, Is.Not.Null);
			Assert.That(attendee.Meetings.Count, Is.Zero);
		}

		[Test]
		public void Constructor_Sets_All_Properties()
		{
			const string name = "Person11";
			const string email = "test@email.com";
			const string phoneNumber = "1234567890";
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
			const string name = "Person11";
			const string email = "test@email.com";
			var attendee = new Attendee(name, email, "", true, null);

			Assert.That(attendee.Meetings, Is.Not.Null);
			Assert.That(attendee.Meetings.Count, Is.EqualTo(0));
		}

		[Test()]
		public void Equals_Returns_True_When_Both_Objects_Are_Same()
		{
			var attendee = new Attendee("", new List<MeetingInfo>());
			var attendee2 = attendee;

			Assert.That(attendee.Equals(attendee2), Is.True);
		}

		[Test()]
		public void Equals_Returns_True_When_Both_Objects_Have_Same_Property_Values()
		{
			var guid = Guid.NewGuid();

			var attendee = new Attendee("", "this@test.com", "1234567890", true, new List<MeetingInfo>());
			attendee.AttendeeId = guid;

			var attendee2 = new Attendee("", "this@test.com", "1234567890", true,
				new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now, DateTime.Now.AddMinutes(30)),
					new MeetingInfo(DateTime.Now.AddMinutes(30), DateTime.Now.AddHours(2))
				});
			attendee2.AttendeeId = guid;

			Assert.That(attendee.Equals(attendee2), Is.True);
		}

		[Test()]
		public void Equals_Returns_False_When_Type_is_Different() =>
			Assert.That(
				new Attendee("", new List<MeetingInfo>()).Equals(new { AttendeeName = "", Meetings = new List<MeetingInfo>() }),
				Is.False);

		[Test()]
		public void Equals_Returns_False_When_Type_Is_Same_And_Property_Values_Are_Different()
		{
			var attendee1 = new Attendee("Person1", "this@test.com", "1234567890", true, new List<MeetingInfo>());
			attendee1.AttendeeId = Guid.NewGuid();

			var attendee2 = new Attendee("Person2", "another@test.com", "0123456789", true,
				new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now, DateTime.Now.AddMinutes(30)),
					new MeetingInfo(DateTime.Now.AddMinutes(30), DateTime.Now.AddHours(2))
				});
			attendee2.AttendeeId = Guid.NewGuid();

			Assert.That(attendee1.Equals(attendee2), Is.False);
		}

		[Test()]
		public void GetHashCodeTest()
		{
			var id = Guid.NewGuid();
			var attendee = new Attendee("Person1", "this@test.com", "1234567890", true, new List<MeetingInfo>());
			var hashCodeBefore = attendee.GetHashCode();
			attendee.AttendeeId = id;
			var hashCodeAfter = attendee.GetHashCode();
			Assert.That(hashCodeBefore == hashCodeAfter, Is.False);
			var newCopyId = Guid.Parse(id.ToString());

			attendee.AttendeeId = newCopyId;

			var hashCodeFinal = attendee.GetHashCode();
			Assert.That(hashCodeAfter == hashCodeFinal, Is.True);
		}
	}
}