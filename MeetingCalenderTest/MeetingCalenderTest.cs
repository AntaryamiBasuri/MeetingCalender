using MeetingCalender;
using MeetingCalender.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingCalenderTest
{
    public class Tests
    {
        private ICalender _meetingCalender;
        [SetUp]
        public void Setup()
        {

            //Get the allowed meeting hours
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(3);

            var attendeesWithMeetingTimings = new List<Attendee>
            {
                new Attendee("Person1", new List<MeetingInfo>
                {
                    new MeetingInfo(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
                    new MeetingInfo(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
                }),
                new Attendee("Person2", new List<MeetingInfo>
                {
                    new MeetingInfo(DateTime.Now.AddMinutes(6),DateTime.Now.AddMinutes(10)),
                    new MeetingInfo(DateTime.Now.AddMinutes(15),DateTime.Now.AddMinutes(20))
                })
            };

            _meetingCalender = new Calender(startTime, endTime, attendeesWithMeetingTimings);
        }

        [Test]
        public void TimeSlotAvailableForRequestedDuration()
        {
            var availableSlot = _meetingCalender.GetFirstAvailableSlot(1);
            Assert.IsNotNull(availableSlot);
            Assert.GreaterOrEqual(availableSlot.AvailableDuration,1);
        }

        [Test]
        public void FetchAttendees()
        {
            Assert.AreEqual(_meetingCalender.Attendees.Count(),2);
        }
        [Test]
        public void AddNewAttendeesToMeeting()
        {
            var meetingCalender = new Calender(DateTime.Now, DateTime.Now.AddDays(1));
            meetingCalender.AddAttendees(new List<Attendee>()
            {
                new Attendee("Person4", new List<MeetingInfo>
                {
                    new MeetingInfo(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
                    new MeetingInfo(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
                })
            });

            Assert.AreEqual(meetingCalender.Attendees.Count(), 1);
        }

        [Test]
        public void AppendNewAttendeesToMeeting()
        {
            _meetingCalender.AppendAttendees(new List<Attendee>()
            {
                new Attendee("Person3", new List<MeetingInfo>
                {
                    new MeetingInfo(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
                    new MeetingInfo(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
                })
            });
            
            Assert.AreEqual(_meetingCalender.Attendees.Count(), 3);
        }

        [Test]
        public void TimeSlotFor200MinutesNotAvailableForRequestedDuration()
        {
            var availableSlot = _meetingCalender.GetFirstAvailableSlot(200);
            Assert.IsNull(availableSlot);
        }
    }
}