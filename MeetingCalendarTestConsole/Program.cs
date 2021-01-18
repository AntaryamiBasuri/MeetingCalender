using MeetingCalendar;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MeetingCalendarTestConsole
{
    class Program
    {
        static void Main()
        {
            //Get the allowed meeting hours
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(8);

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
                }),
                new Attendee("Person3", new List<MeetingInfo>
                {
                    new MeetingInfo(DateTime.Now.AddMinutes(25),DateTime.Now.AddMinutes(27)),
                    new MeetingInfo(DateTime.Now.AddMinutes(32),DateTime.Now.AddMinutes(48)),
                    new MeetingInfo(DateTime.Now.AddMinutes(65),DateTime.Now.AddMinutes(120)),
                    new MeetingInfo(DateTime.Now.AddMinutes(130),DateTime.Now.AddMinutes(160))
                }),
                new Attendee("Person4", new List<MeetingInfo>
                {
                    new MeetingInfo(DateTime.Now.AddMinutes(46),DateTime.Now.AddMinutes(50)),
                    new MeetingInfo(DateTime.Now.AddMinutes(55),DateTime.Now.AddMinutes(60)),
                    new MeetingInfo(DateTime.Now.AddMinutes(85),DateTime.Now.AddMinutes(150)),
                    new MeetingInfo(DateTime.Now.AddMinutes(150),DateTime.Now.AddMinutes(180))
                })
            };

            var meetingCalendar = new Calendar(startTime, endTime, attendeesWithMeetingTimings);


            while (true)
            {
                Console.WriteLine("Please provide the duration (in minutes) of the meeting that you want to reserve.");
                var meetingRequestDuration = Console.ReadLine();

                if (int.TryParse(meetingRequestDuration, out int duration) && duration > 0)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    var firstAvailableMeetingSlot = meetingCalendar.GetFirstAvailableSlot(duration);
                    sw.Stop();
                    Console.WriteLine(sw.ElapsedMilliseconds);

                    if (firstAvailableMeetingSlot != null)
                    {
                        Console.WriteLine($"A meeting slot for {duration} minutes is available between:" +
                                          $"{firstAvailableMeetingSlot.StartTime:hh:mm tt} and {firstAvailableMeetingSlot.EndTime:hh:mm tt}.");
                    }
                    else
                    {
                        Console.WriteLine(
                            $"Sorry ! There is no meeting slot of {duration} minutes is available for today. " +
                            "Please check for tomorrow.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid meeting duration.");
                }

                Console.ReadLine();
            }
        }
    }
}
