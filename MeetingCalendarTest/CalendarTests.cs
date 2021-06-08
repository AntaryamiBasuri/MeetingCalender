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
	public class CalendarTests
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
				new ("Person1", new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
					new MeetingInfo(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
				}),
				new ("Person2", new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now.AddMinutes(6),DateTime.Now.AddMinutes(10)),
					new MeetingInfo(DateTime.Now.AddMinutes(15),DateTime.Now.AddMinutes(20))
				})
			};

			_meetingCalendar = new Calendar(startTime, endTime, attendeesWithMeetingTimings);
		}

		[Test]
		public void TimeSlotAvailableForRequestedDuration()
		{
			var availableSlot = _meetingCalendar.FindFirstAvailableSlot(1);
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
				new("Person4", new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
					new MeetingInfo(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
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
				new("Person3", new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
					new MeetingInfo(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
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
					new("Person3", new List<IMeetingInfo>
					{
						new MeetingInfo(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
						new MeetingInfo(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
					})
				});
			});
			Assert.That(_meetingCalendar.Attendees.Count(), Is.EqualTo(1));
		}

		[Test]
		public void GetAllAvailableTimeSlots_Returns_No_TimeSlot_When_Calendar_Upper_Limit_Approaches_Current_Time()
		{
			var calendar = new Calendar(DateTime.Now.AddHours(-8), DateTime.Now.AddMinutes(1), new List<Attendee>()
			{
				new ("Attendee1", new List<IMeetingInfo>())
			});

			var source = calendar.GetAllAvailableTimeSlots().ToList();
			Assert.That(source.Count, Is.Zero);
		}

		[Test]
		public void FindFirstAvailableSlot_Returns_No_TimeSlot_For_200Minutes_Meeting_Duration()
		{
			var availableSlot = _meetingCalendar.FindFirstAvailableSlot(200);
			Assert.That(availableSlot, Is.Null);
		}

		[Test]
		public void FindFirstAvailableSlot_Returns_Null_When_NoSlot_Available()
		{
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(8);
			var meetingCalendar = new Calendar(startTime, endTime, new List<Attendee>
			{
				new("Person1", new List<IMeetingInfo>
				{
					new MeetingInfo(startTime, endTime)
				})
			});

			var availableSlot = meetingCalendar.FindFirstAvailableSlot(1);
			Assert.That(availableSlot, Is.Null);
		}

		[Test]
		public void FindFirstAvailableSlot_Returns_Null_Beyond_Allowed_MeetingHours()
		{
			var endTime = DateTime.Now;
			var startTime = endTime.AddHours(-8);

			var meetingCalendar = new Calendar(startTime, endTime, new List<Attendee>());

			var availableSlot = meetingCalendar.FindFirstAvailableSlot(1);
			Assert.That(availableSlot, Is.Null);
		}

		[Test]
		public void FindFirstAvailableSlot_Returns_TimeSlot_Within_Given_Time_Range()
		{
			var startTime = DateTime.Now.CalibrateToMinutes();
			var endTime = startTime.AddHours(8);
			var secondMeetingStartTime = startTime.AddHours(2.5);
			var secondMeetingEndTime = startTime.AddHours(3.5);

			var meetingCalendar = new Calendar(startTime, endTime, new List<Attendee>
			{
				new("Person1", new List<IMeetingInfo>
				{
					new MeetingInfo(startTime.AddHours(1), startTime.AddHours(2)),
					new MeetingInfo(secondMeetingStartTime, secondMeetingEndTime),
					//Here is the start time of available slot
					new MeetingInfo(startTime.AddHours(7), startTime.AddHours(7.5))
				})
			});

			var availableSlot = meetingCalendar.FindFirstAvailableSlot(15, secondMeetingStartTime);
			Assert.That(availableSlot, Is.Not.Null);
			Assert.That(availableSlot.StartTime, Is.GreaterThanOrEqualTo(secondMeetingEndTime));
		}

		[Test]
		public void FindFirstAvailableSlot_Returns_TimeSlot_Equals_To_Calendar_TimeFrame_When_Search_Range_Longer_Than_Calendar_TimeFrame()
		{
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(4);
			var meetingCalendar = new Calendar(startTime, endTime);

			const int meetingDuration = 120;
			var availableSlot = meetingCalendar.FindFirstAvailableSlot(meetingDuration, startTime.AddHours(-1), endTime.AddHours(1));

			Assert.That(availableSlot, Is.Not.Null);
			Assert.That(availableSlot.GetDuration(), Is.GreaterThanOrEqualTo(meetingDuration));
			Assert.That(availableSlot.StartTime, Is.EqualTo(meetingCalendar.StartTime));
			Assert.That(availableSlot.EndTime, Is.LessThanOrEqualTo(meetingCalendar.EndTime));
		}

		[Test]
		public void FindFirstAvailableSlot_Returns_TimeSlot_Equals_To_Search_Range_When_Range_Shorter_Than_Calendar_TimeFrame()
		{
			var startTime = DateTime.Now.CalibrateToMinutes();
			var endTime = startTime.AddHours(4).CalibrateToMinutes();
			var meetingCalendar = new Calendar(startTime, endTime);

			var searchRangeStartTime = startTime.AddHours(1.5);
			var searchEndTime = startTime.AddHours(3.75);

			const int meetingDuration = 60;
			var availableSlot = meetingCalendar.FindFirstAvailableSlot(meetingDuration, searchRangeStartTime, searchEndTime);

			Assert.That(availableSlot, Is.Not.Null);
			Assert.That(availableSlot.GetDuration(), Is.GreaterThanOrEqualTo(meetingDuration));
			Assert.That(availableSlot.StartTime, Is.EqualTo(searchRangeStartTime));
			Assert.That(availableSlot.EndTime, Is.EqualTo(searchEndTime));
		}

		[Test]
		public void FindFirstAvailableSlot_Returns_TimeSlot_Having_EndTime_Shorter_Than_Calendar_EndTime_When_SearchRange_UB_Longer_Than_Calender_TimeFrame()
		{
			var startTime = DateTime.Now.CalibrateToMinutes();
			var endTime = startTime.AddHours(4).CalibrateToMinutes();
			var firstMeetingEndTime = startTime.AddHours(2);
			var meetingCalendar = new Calendar(startTime, endTime, new List<Attendee>
			{
				new("Person1", new List<IMeetingInfo>
				{
					new MeetingInfo(startTime, firstMeetingEndTime),
				})
			});

			var searchRangeStartTime = startTime.AddHours(1.5);
			var searchEndTime = endTime.AddMinutes(-15);

			const int meetingDuration = 60;
			var availableSlot = meetingCalendar.FindFirstAvailableSlot(meetingDuration, searchRangeStartTime, searchEndTime);

			Assert.That(availableSlot, Is.Not.Null);
			Assert.That(availableSlot.GetDuration(), Is.GreaterThanOrEqualTo(meetingDuration));
			Assert.That(availableSlot.StartTime, Is.EqualTo(firstMeetingEndTime));
			Assert.That(availableSlot.EndTime, Is.EqualTo(searchEndTime));
		}

		[Test]
		public void FindFirstAvailableSlot_Returns_TimeSlot_StartTime_Equals_To_Search_Range_Starting_Time_When_SearchRange_Is_Longer()
		{
			var startTime = DateTime.Now.CalibrateToMinutes();
			var endTime = startTime.AddHours(4).CalibrateToMinutes();
			var meetingCalendar = new Calendar(startTime, endTime);

			var searchRangeStartTime = startTime.AddHours(1.5);

			const int meetingDuration = 120;
			var availableSlot = meetingCalendar.FindFirstAvailableSlot(meetingDuration, searchRangeStartTime, endTime.AddHours(1));

			Assert.That(availableSlot, Is.Not.Null);
			Assert.That(availableSlot.GetDuration(), Is.GreaterThanOrEqualTo(meetingDuration));
			Assert.That(availableSlot.StartTime, Is.EqualTo(searchRangeStartTime));
			Assert.That(availableSlot.EndTime, Is.LessThanOrEqualTo(meetingCalendar.EndTime));
		}

		[Test]
		public void GetFirstAvailableSlot_Returns_MeetingSlot_When_No_Meetings_Scheduled_Within_Allowed_MeetingHours()
		{
			var startTime = DateTime.Now.CalibrateToMinutes();
			var endTime = startTime.AddHours(8);

			var meetingCalendar = new Calendar(startTime, endTime, new List<Attendee>()
			{
				new("Person10", new List<IMeetingInfo>
				{
					new MeetingInfo(startTime.AddHours(-2), startTime.AddHours(-1)),
					new MeetingInfo(endTime.AddHours(1), endTime.AddHours(2))
				})
			});

			var availableSlot = meetingCalendar.FindFirstAvailableSlot(60);

			Assert.That(availableSlot.GetDuration(), Is.GreaterThanOrEqualTo(60));
			Assert.That(availableSlot.StartTime, Is.GreaterThanOrEqualTo(startTime));
		}

		[Test]
		public void AvailableTimeSlotShouldBeGreaterThanEqualsToCurrentTime()
		{
			var now = DateTime.Now;
			var meetingCalendar = new Calendar(now.AddHours(-1), now.AddHours(1));

			meetingCalendar.AddAttendees(new List<Attendee>());
			var firstAvailableTimeSlot = meetingCalendar.FindFirstAvailableSlot(10);

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
				new("Person1", new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingEndTime)
				})
			});

			var availableSlot = meetingCalendar.FindFirstAvailableSlot(10);

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
				new("Person1", new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingEndTime)
				}),
				new("Person2", new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingEndTime)
				}),
				new("Person3", new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingEndTime)
				}),
				new("Person4", new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingEndTime)
				}),
				new("Person5", new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingEndTime)
				}),
				new("Person6", new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingEndTime)
				}),
				new("Person7", new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingEndTime)
				}),
				new("Person8", new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingEndTime)
				}),
				new("Person9", new List<IMeetingInfo>
				{
					new MeetingInfo(endTime.AddMinutes(-15), endTime.AddMinutes(15))
				}),
				new("Person10", new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingStartTime.AddMinutes(10))
				}),
				new("Person11", "test@email.com", true, new List<IMeetingInfo>
				{
					new MeetingInfo(meetingStartTime, meetingEndTime)
				})
			});

			var availableSlot = meetingCalendar.FindFirstAvailableSlot(10);

			Assert.That(availableSlot.StartTime, Is.GreaterThanOrEqualTo(meetingEndTime.CalibrateToMinutes()));
		}
	}
}