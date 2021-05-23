/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar;
using MeetingCalendar.Extensions;
using MeetingCalendar.Interfaces;
using MeetingCalendar.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingCalendarTest
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class MeetingCalendarTests
	{
		private ICalendar _meetingCalendar;

		[SetUp]
		public void Setup()
		{
			//Get the allowed meeting hours
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(3);

			var attendeesWithMeetingTimings = new List<Attendee>
			{
				new("Person1", new List<MeetingInfo>
				{
					new(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
					new(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
				}),
				new("Person2", new List<MeetingInfo>
				{
					new(DateTime.Now.AddMinutes(6),DateTime.Now.AddMinutes(10)),
					new(DateTime.Now.AddMinutes(15),DateTime.Now.AddMinutes(20))
				})
			};

			_meetingCalendar = new Calendar(startTime, endTime, attendeesWithMeetingTimings);
		}

		[Test]
		public void TimeSlotAvailableForRequestedDuration()
		{
			var availableSlot = _meetingCalendar.GetFirstAvailableSlot(1);
			Assert.That(availableSlot, Is.Not.Null);
			Assert.That(availableSlot.GetDuration(), Is.GreaterThanOrEqualTo(1));
		}

		[Test]
		public void FetchAttendees()
		{
			Assert.That(_meetingCalendar.Attendees.Count(), Is.EqualTo(2));
		}

		[Test]
		public void AddNewAttendeesToMeeting()
		{
			var meetingCalendar = new Calendar(DateTime.Now, DateTime.Now.AddDays(1));
			meetingCalendar.AddAttendees(new List<Attendee>()
			{
				new("Person4", new List<MeetingInfo>
				{
					new(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
					new(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
				})
			});

			Assert.That(meetingCalendar.Attendees.Count(), Is.EqualTo(1));
			Assert.That(meetingCalendar.Attendees.First().AttendeeName, Is.EqualTo("Person4"));
		}

		[Test]
		public void AppendNewAttendeesToMeeting()
		{
			_meetingCalendar.AppendAttendees(new List<Attendee>()
			{
				new("Person3", new List<MeetingInfo>
				{
					new(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
					new(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
				})
			});

			Assert.That(_meetingCalendar.Attendees.Count(), Is.EqualTo(3));
		}

		[Test]
		public void AppendNewAttendeesToMeeting_When_AttendeeList_Is_Null()
		{
			Assert.DoesNotThrow(() =>
			{
				_meetingCalendar.AddAttendees(null);
				_meetingCalendar.AppendAttendees(new List<Attendee>()
				{
					new("Person3", new List<MeetingInfo>
					{
						new(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
						new(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
					})
				});
			});
			Assert.That(_meetingCalendar.Attendees.Count(), Is.EqualTo(1));
		}

		[Test]
		public void TimeSlotFor200MinutesNotAvailableForRequestedDuration()
		{
			var availableSlot = _meetingCalendar.GetFirstAvailableSlot(200);
			Assert.That(availableSlot, Is.Null);
		}

		[Test]
		public void GetFirstAvailableSlot_Returns_Null_When_NoSlot_Available()
		{
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(8);
			var meetingCalendar = new Calendar(startTime, endTime, new List<Attendee>
			{
				new("Person1", new List<MeetingInfo>
				{
					new(startTime, endTime)
				})
			});

			var availableSlot = meetingCalendar.GetFirstAvailableSlot(1);
			Assert.That(availableSlot, Is.Null);
		}

		[Test]
		public void GetFirstAvailableSlot_Returns_Null_Beyond_Allowed_MeetingHours()
		{
			var endTime = DateTime.Now;
			var startTime = endTime.AddHours(-8);

			var meetingCalendar = new Calendar(startTime, endTime, new List<Attendee>());

			var availableSlot = meetingCalendar.GetFirstAvailableSlot(1);
			Assert.That(availableSlot, Is.Null);
		}

		[Test]
		public void GetFirstAvailableSlot_Returns_MeetingSlot_When_No_Meetings_Scheduled_Within_Allowed_MeetingHours()
		{
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(8);

			var meetingCalendar = new Calendar(startTime, endTime, new List<Attendee>()
			{
				new("Person10", new List<MeetingInfo>
				{
					new(startTime.AddHours(-2), startTime.AddHours(-1)),
					new(endTime.AddHours(1), endTime.AddHours(2))
				})
			});

			var availableSlot = meetingCalendar.GetFirstAvailableSlot(60);
			Assert.That(availableSlot.GetDuration(), Is.GreaterThan(0));
		}

		[Test]
		public void AvailableTimeSlotShouldBeGreaterThanEqualsToCurrentTime()
		{
			var now = DateTime.Now;
			var meetingCalendar = new Calendar(now.AddHours(-1), now.AddHours(1));

			meetingCalendar.AddAttendees(new List<Attendee>());
			var firstAvailableTimeSlot = meetingCalendar.GetFirstAvailableSlot(10);
			Assert.That(firstAvailableTimeSlot.StartTime, Is.GreaterThanOrEqualTo(now.CalibrateToMinutes()));
		}

		[Test]
		public void GetFirstAvailableSlot_Includes_Meeting_Duration_When_Not_Over()
		{
			var startTime = DateTime.Now.AddHours(-4);
			var endTime = startTime.AddHours(8);

			var meetingStartTime = DateTime.Now.AddMinutes(-15);
			var meetingEndTime = DateTime.Now.AddMinutes(15);

			var meetingCalendar = new Calendar(startTime, endTime, new List<Attendee>
			{
				new("Person1", new List<MeetingInfo>
				{
					new(meetingStartTime, meetingEndTime)
				})
			});

			var availableSlot = meetingCalendar.GetFirstAvailableSlot(10);
			Assert.That(availableSlot.StartTime, Is.GreaterThanOrEqualTo(meetingEndTime.CalibrateToMinutes()));
		}

		[Test]
		public void GetFirstAvailableSlot_When_Attendees_Count_Greater_Than_Eight()
		{
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(9);

			var meetingStartTime = DateTime.Now.AddMinutes(-15);
			var meetingEndTime = DateTime.Now.AddMinutes(15);

			var meetingCalendar = new Calendar(startTime, endTime, new List<Attendee>
			{
				new("Person1", new List<MeetingInfo>
				{
					new(meetingStartTime, meetingEndTime)
				}),
				new("Person2", new List<MeetingInfo>
				{
					new(meetingStartTime, meetingEndTime)
				}),
				new("Person3", new List<MeetingInfo>
				{
					new(meetingStartTime, meetingEndTime)
				}),
				new("Person4", new List<MeetingInfo>
				{
					new(meetingStartTime, meetingEndTime)
				}),
				new("Person5", new List<MeetingInfo>
				{
					new(meetingStartTime, meetingEndTime)
				}),
				new("Person6", new List<MeetingInfo>
				{
					new(meetingStartTime, meetingEndTime)
				}),
				new("Person7", new List<MeetingInfo>
				{
					new(meetingStartTime, meetingEndTime)
				}),
				new("Person8", new List<MeetingInfo>
				{
					new(meetingStartTime, meetingEndTime)
				}),
				new("Person9", new List<MeetingInfo>
				{
					new(endTime.AddMinutes(-15), endTime.AddMinutes(15))
				}),
				new("Person10", new List<MeetingInfo>
				{
					new(meetingStartTime, meetingStartTime.AddMinutes(10))
				}),
				new("Person11", "test@email.com", true, new List<MeetingInfo>
				{
					new(meetingStartTime, meetingEndTime)
				})
			});

			var availableSlot = meetingCalendar.GetFirstAvailableSlot(10);
			Assert.That(availableSlot.StartTime, Is.GreaterThanOrEqualTo(meetingEndTime.CalibrateToMinutes()));
		}
	}
}