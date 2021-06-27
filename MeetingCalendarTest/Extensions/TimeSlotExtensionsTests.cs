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
using System.Linq;

namespace MeetingCalendar.Tests.Extensions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class TimeSlotExtensionsTests
	{
		[Test]
		public void AvailableDuration_Is_Zero_When_StartTime_And_EndTime_Are_Equal()
			=> Assert.That(new TimeSlot(DateTime.Now, DateTime.Now).GetDuration(), Is.Zero);

		[Test]
		public void AvailableDuration_Is_GreaterThan_Zero_When_EndTime_Is_Greater_Than_StartTime()
			=> Assert.That(new TimeSlot(DateTime.Now, DateTime.Now.AddHours(1)).GetDuration, Is.GreaterThan(0));

		[Test]
		public void GetTimeSeriesByMinutes_Returns_Series_Of_DateTime()
			=> Assert.That(new TimeSlot(DateTime.Now, DateTime.Now.AddMinutes(5)).GetTimeSeriesByMinutes().Count,
				Is.EqualTo(5));

		[Test]
		public void FillWith_Fill_All_Values_With_Available_As_Default()
			=> Assert.That(new TimeSlot(DateTime.Now, DateTime.Now.AddMinutes(5)).GetTimeSeriesByMinutes()
				.Values.All(t => t == AvailabilityTypes.Available), Is.True);

		[Test]
		public void GetTimeSeriesByMinutes_Includes_LB_And_Exclude_UB_Value()
		{
			var startTime = DateTime.Now;
			var endTime = DateTime.Now.AddMinutes(5);

			var timeSlot = new TimeSlot(startTime, endTime);

			Assert.That(timeSlot.GetTimeSeriesByMinutes().ContainsKey(startTime.CalibrateToMinutes()), Is.True);
			Assert.That(timeSlot.GetTimeSeriesByMinutes().ContainsKey(endTime.AddMinutes(-1).CalibrateToMinutes()), Is.True);
		}

		[Test]
		public void GetTimeSeriesByMinutes_Returns_A_Time_Series_By_Minutes()
		{
			var startTime = DateTime.Now;
			var endTime = DateTime.Now.AddHours(1);

			var timeSlot = new TimeSlot(startTime, endTime);

			Assert.That(timeSlot.GetTimeSeriesByMinutes().Count, Is.EqualTo(60));
		}

		[Test]
		public void FindFirst_Returns_Null_When_Source_Is_Null()
			=> Assert.That(((IEnumerable<TimeSlot>)null).FindFirst(default), Is.Null);

		[Test]
		public void FindFirst_Returns_TimeSlot_When_TimeSLot_Available_And_Predicate_Is_Null()
		{
			var calendar = new Calendar(DateTime.Now, DateTime.Now.AddHours(8));

			var source = calendar.GetAllAvailableTimeSlots().ToList();
			Assert.That(source.FindFirst(null), Is.Not.Null);
		}

		[Test]
		public void TimeSlotMapper_Returns_TimeSlot_Mapped_LB_And_UB()
		{
			var startTime = DateTime.Now.AddHours(1);
			var endTime = DateTime.Now.AddHours(9);

			var (calendarStartTime, calendarEndTime) = new Calendar(startTime, endTime);

			var timeSlotStartTime = startTime.AddMinutes(-30);
			var timeSlotEndTime = endTime.AddMinutes(30);

			var timeSlot = new TimeSlot(timeSlotStartTime, timeSlotEndTime);

			var mappedTimeSlot = timeSlot.TimeSlotMapper(calendarStartTime, calendarEndTime);

			Assert.That(mappedTimeSlot.StartTime, Is.EqualTo(calendarStartTime));
			Assert.That(mappedTimeSlot.EndTime, Is.EqualTo(calendarEndTime));

			timeSlotStartTime = startTime.AddMinutes(30);
			timeSlotEndTime = endTime.AddMinutes(-30);

			timeSlot = new TimeSlot(timeSlotStartTime, timeSlotEndTime);

			mappedTimeSlot = timeSlot.TimeSlotMapper(calendarStartTime, calendarEndTime);

			Assert.That(mappedTimeSlot.StartTime, Is.EqualTo(timeSlot.StartTime));
			Assert.That(mappedTimeSlot.EndTime, Is.EqualTo(timeSlot.EndTime));
		}

		[Test]
		public void FindFirst_Returns_First_TimeSlot_For_A_Given_Duration_Having_Least_Duration_First()
		{
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(8);

			var forthMeetingEndTime = startTime.AddHours(4.75);

			var meetingCalendar = new Calendar(startTime, endTime, new List<IAttendee>
			{
				new Attendee("Person10", new List<IMeetingInfo>
				{
					new MeetingInfo(startTime.AddHours(1), startTime.AddHours(1.5)),
					new MeetingInfo(startTime.AddHours(2), startTime.AddHours(2.7)),
					new MeetingInfo(startTime.AddHours(3), startTime.AddHours(3.5)),
					new MeetingInfo(startTime.AddHours(4), forthMeetingEndTime),
					//Here is the expected slot having least duration first
					new MeetingInfo(startTime.AddHours(5), startTime.AddHours(7.25))
				})
			});

			var source = meetingCalendar.GetAllAvailableTimeSlots().ToList();
			var actualStartTime = source.FindFirst(t => t.GetDuration() >= 10).StartTime;
			Assert.That(actualStartTime, Is.EqualTo(forthMeetingEndTime.CalibrateToMinutes()));
		}

		[Test]
		public void FindFirst_Returns_First_TimeSlot_For_A_Given_Duration_Having_Least_Duration_First_Within_The_TimeFrame()
		{
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(8);

			var secondMeetingEndTime = startTime.AddHours(2.7);

			var meetingCalendar = new Calendar(startTime, endTime, new List<IAttendee>()
			{
				new Attendee("Person10", new List<IMeetingInfo>
				{
					new MeetingInfo(startTime.AddHours(1), startTime.AddHours(1.5)),
					new MeetingInfo(startTime.AddHours(2), secondMeetingEndTime),
					//Here is the expected slot having least duration first
					new MeetingInfo(startTime.AddHours(3), startTime.AddHours(3.5)),
					new MeetingInfo(startTime.AddHours(4), startTime.AddHours(4.75)),
					new MeetingInfo(startTime.AddHours(5), startTime.AddHours(7.25))
				})
			});

			var source = meetingCalendar.GetAllAvailableTimeSlots().ToList();
			var actualStartTime = source.FindFirst(t
				=> t.StartTime >= startTime && t.EndTime <= startTime.AddHours(4.75) && t.GetDuration() >= 10).StartTime;
			Assert.That(actualStartTime, Is.EqualTo(secondMeetingEndTime.CalibrateToMinutes()));
		}
	}
}