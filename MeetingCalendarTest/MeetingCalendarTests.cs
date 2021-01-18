using MeetingCalendar;
using MeetingCalendar.Extensions;
using MeetingCalendar.Interfaces;
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
        private ICalendar _meetingCalender;
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

            _meetingCalender = new Calendar(startTime, endTime, attendeesWithMeetingTimings);
        }

        [Test]
        public void TimeSlotAvailableForRequestedDuration()
        {
            var availableSlot = _meetingCalender.GetFirstAvailableSlot(1);
            Assert.That(availableSlot, Is.Not.Null);
            Assert.That(availableSlot.AvailableDuration, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void FetchAttendees()
        {
            Assert.That(_meetingCalender.Attendees.Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddNewAttendeesToMeeting()
        {
            var meetingCalender = new Calendar(DateTime.Now, DateTime.Now.AddDays(1));
            meetingCalender.AddAttendees(new List<Attendee>()
            {
                new Attendee("Person4", new List<MeetingInfo>
                {
                    new MeetingInfo(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
                    new MeetingInfo(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
                })
            });
            
            Assert.That(meetingCalender.Attendees.Count(), Is.EqualTo(1));
            Assert.That(meetingCalender.Attendees.First().AttendeeName, Is.EqualTo("Person4"));
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

            Assert.That(_meetingCalender.Attendees.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AppendNewAttendeesToMeeting_When_AttendeeList_Is_Null()
        {
            Assert.DoesNotThrow(() =>
            {
                _meetingCalender.AddAttendees(null);
                _meetingCalender.AppendAttendees(new List<Attendee>()
                {
                    new Attendee("Person3", new List<MeetingInfo>
                    {
                        new MeetingInfo(DateTime.Now.AddMinutes(5),DateTime.Now.AddMinutes(7)),
                        new MeetingInfo(DateTime.Now.AddMinutes(12),DateTime.Now.AddMinutes(18))
                    })
                });

            });
            Assert.That(_meetingCalender.Attendees.Count(), Is.EqualTo(1));
        }

        [Test]
        public void TimeSlotFor200MinutesNotAvailableForRequestedDuration()
        {
            var availableSlot = _meetingCalender.GetFirstAvailableSlot(200);
            Assert.That(availableSlot, Is.Null);
        }

        [Test]
        public void GetFirstAvailableSlot_Returns_Null_When_NoSlot_Available()
        {
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(8);
            var meetingCalender = new Calendar(startTime, endTime, new List<Attendee>
            {
                new Attendee("Person1", new List<MeetingInfo>
                {
                    new MeetingInfo(startTime, endTime)
                })
            });

            var availableSlot = meetingCalender.GetFirstAvailableSlot(1);
            Assert.That(availableSlot, Is.Null);
        }

        [Test]
        public void GetFirstAvailableSlot_Returns_Null_Beyond_Allowed_MeetingHours()
        {
            var endTime = DateTime.Now;
            var startTime = endTime.AddHours(-8);

            var meetingCalender = new Calendar(startTime, endTime, new List<Attendee>());

            var availableSlot = meetingCalender.GetFirstAvailableSlot(1);
            Assert.That(availableSlot, Is.Null);
        }

        [Test]
        public void AvailableTimeSlotShouldBeGreaterThanEqualsToCurrentTime()
        {
            var now = DateTime.Now;
            var meetingCalender = new Calendar(now.AddHours(-1), now.AddHours(1));

            meetingCalender.AddAttendees(new List<Attendee>());
            var firstAvailableTimeSlot = meetingCalender.GetFirstAvailableSlot(10);
            Assert.That(firstAvailableTimeSlot.StartTime, Is.GreaterThanOrEqualTo(now.CalibrateToMinutes()));
        }

        [Test]
        public void GetFirstAvailableSlot_Includes_Meeting_Duration_When_Not_Over()
        {
            var startTime = DateTime.Now.AddHours(-4);
            var endTime = startTime.AddHours(8);

            var meetingStartTime = DateTime.Now.AddMinutes(-15);
            var meetingEndTime = DateTime.Now.AddMinutes(15);

            var meetingCalender = new Calendar(startTime, endTime, new List<Attendee>
            {
                new Attendee("Person1", new List<MeetingInfo>
                {
                    new MeetingInfo(meetingStartTime, meetingEndTime)
                })
            });

            var availableSlot = meetingCalender.GetFirstAvailableSlot(10);
            Assert.That(availableSlot.StartTime, Is.GreaterThanOrEqualTo(meetingEndTime.CalibrateToMinutes()));
        }
        [Test]
        public void GetFirstAvailableSlot_When_Attendees_Count_Greater_Than_Eight()
        {
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(9);

            var meetingStartTime = DateTime.Now.AddMinutes(-15);
            var meetingEndTime = DateTime.Now.AddMinutes(15);

            var meetingCalender = new Calendar(startTime, endTime, new List<Attendee>
            {
                new Attendee("Person1", new List<MeetingInfo>
                {
                    new MeetingInfo(meetingStartTime, meetingEndTime)
                }),
                new Attendee("Person2", new List<MeetingInfo>
                {
                    new MeetingInfo(meetingStartTime, meetingEndTime)
                }),
                new Attendee("Person3", new List<MeetingInfo>
                {
                    new MeetingInfo(meetingStartTime, meetingEndTime)
                }),
                new Attendee("Person4", new List<MeetingInfo>
                {
                    new MeetingInfo(meetingStartTime, meetingEndTime)
                }),
                new Attendee("Person5", new List<MeetingInfo>
                {
                    new MeetingInfo(meetingStartTime, meetingEndTime)
                }),
                new Attendee("Person6", new List<MeetingInfo>
                {
                    new MeetingInfo(meetingStartTime, meetingEndTime)
                }),
                new Attendee("Person7", new List<MeetingInfo>
                {
                    new MeetingInfo(meetingStartTime, meetingEndTime)
                }),
                new Attendee("Person8", new List<MeetingInfo>
                {
                    new MeetingInfo(meetingStartTime, meetingEndTime)
                }),
                new Attendee("Person9", new List<MeetingInfo>
                {
                    new MeetingInfo(meetingStartTime, meetingEndTime)
                }),
            });

            var availableSlot = meetingCalender.GetFirstAvailableSlot(10);
            Assert.That(availableSlot.StartTime, Is.GreaterThanOrEqualTo(meetingEndTime.CalibrateToMinutes()));
        }
    }
}