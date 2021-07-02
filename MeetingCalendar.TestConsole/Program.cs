using MeetingCalendar;
using MeetingCalendar.Interfaces;
using MeetingCalendar.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Math;

namespace MeetingCalendar.TestConsole
{
	internal static class Program
	{
		private static void Main()
		{
			//Get the allowed meeting hours
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(8);

			IList<IMeetingInfo> meetings = new List<IMeetingInfo>
			{
				new MeetingInfo(DateTime.Now.AddMinutes(5), DateTime.Now.AddMinutes(7)),
				new MeetingInfo(DateTime.Now.AddMinutes(12), DateTime.Now.AddMinutes(18))
			};

			IList<IAttendee> attendeesWithMeetingTimings = new List<IAttendee>
			{
				new Attendee("Person1", meetings ),
				new Attendee("Person2", new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now.AddMinutes(6),DateTime.Now.AddMinutes(10)),
					new MeetingInfo(DateTime.Now.AddMinutes(15),DateTime.Now.AddMinutes(20))
				}),
				new Attendee("Person3", new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now.AddMinutes(25),DateTime.Now.AddMinutes(27)),
					new MeetingInfo(DateTime.Now.AddMinutes(32),DateTime.Now.AddMinutes(48)),
					new MeetingInfo(DateTime.Now.AddMinutes(65),DateTime.Now.AddMinutes(120)),
					new MeetingInfo(DateTime.Now.AddMinutes(130),DateTime.Now.AddMinutes(160))
				}),
				new Attendee("Person4", new List<IMeetingInfo>
				{
					new MeetingInfo(DateTime.Now.AddMinutes(46),DateTime.Now.AddMinutes(50)),
					new MeetingInfo(DateTime.Now.AddMinutes(55),DateTime.Now.AddMinutes(60)),
					new MeetingInfo(DateTime.Now.AddMinutes(85),DateTime.Now.AddMinutes(150)),
					new MeetingInfo(DateTime.Now.AddMinutes(150),DateTime.Now.AddMinutes(180))
				})
			};

			ICalendar meetingCalendar = new Calendar(startTime, endTime, attendeesWithMeetingTimings);

			var (calendarStartTime, calendarEndTime, _, calendarWindowInMinutes, attendees) = meetingCalendar;

			while (true)
			{
				Console.WriteLine("Please provide the duration (in minutes) of the meeting that you want to reserve.");
				var meetingRequestDuration = Console.ReadLine();

				if (int.TryParse(meetingRequestDuration, out var duration) && duration > 0)
				{
					var sw = new Stopwatch();

					sw.Start();
					var firstAvailableMeetingSlot = meetingCalendar.FindFirstAvailableSlot(duration);
					sw.Stop();

					Console.WriteLine($"Number of Attendees in the meeting: { meetingCalendar.Attendees.Count }");
					attendees.ToList().ForEach(attendee => Console.WriteLine(attendee.AttendeeName));

					Console.WriteLine("");

					if (firstAvailableMeetingSlot != null)
					{
						Console.WriteLine($"A meeting slot for {GetHoursAndMinutes(duration)} is available between:" +
										  $"{firstAvailableMeetingSlot.StartTime:hh:mm tt} and {firstAvailableMeetingSlot.EndTime:hh:mm tt}.");
					}
					else
					{
						Console.WriteLine(
							$"Sorry ! There is no meeting slot available for {GetHoursAndMinutes(duration)}. The Meeting Calendar time frame is: " +
							$"{calendarStartTime:hh:mm tt} to {calendarEndTime:hh:mm tt} having total duration of " +
							$"{GetHoursAndMinutes(calendarWindowInMinutes)}.");
					}
					Console.WriteLine("");
					Console.WriteLine($"Time taken to calculate the result is: { sw.ElapsedMilliseconds }ms.");
				}
				else
				{
					Console.WriteLine("Invalid meeting duration.");
				}
				Console.ReadLine();
			}
		}

		private static string GetHoursAndMinutes(double totalMinutes)
		{
			var ts = TimeSpan.FromMinutes(Abs(totalMinutes));

			return $"{ts.Hours} {(ts.Hours > 1 ? "hours" : "hour")} and {ts.Minutes} {(ts.Minutes > 1 ? "minutes" : "minute")}";
		}
	}
}
