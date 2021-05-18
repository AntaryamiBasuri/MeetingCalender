/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MeetingCalendarTestConsole
{
	internal class Program
	{
		private static void Main()
		{
			//Get the allowed meeting hours
			var startTime = DateTime.Now;
			var endTime = startTime.AddHours(8);

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
				}),
				new("Person3", new List<MeetingInfo>
				{
					new(DateTime.Now.AddMinutes(25),DateTime.Now.AddMinutes(27)),
					new(DateTime.Now.AddMinutes(32),DateTime.Now.AddMinutes(48)),
					new(DateTime.Now.AddMinutes(65),DateTime.Now.AddMinutes(120)),
					new(DateTime.Now.AddMinutes(130),DateTime.Now.AddMinutes(160))
				}),
				new("Person4", new List<MeetingInfo>
				{
					new(DateTime.Now.AddMinutes(46),DateTime.Now.AddMinutes(50)),
					new(DateTime.Now.AddMinutes(55),DateTime.Now.AddMinutes(60)),
					new(DateTime.Now.AddMinutes(85),DateTime.Now.AddMinutes(150)),
					new(DateTime.Now.AddMinutes(150),DateTime.Now.AddMinutes(180))
				})
			};

			var meetingCalendar = new Calendar(startTime, endTime, attendeesWithMeetingTimings);

			while (true)
			{
				Console.WriteLine("Please provide the duration (in minutes) of the meeting that you want to reserve.");
				var meetingRequestDuration = Console.ReadLine();

				if (int.TryParse(meetingRequestDuration, out var duration) && duration > 0)
				{
					var sw = new Stopwatch();
					sw.Start();
					var firstAvailableMeetingSlot = meetingCalendar.GetFirstAvailableSlot(duration);
					sw.Stop();
					Console.WriteLine($"Number of Attendees in the meeting: { meetingCalendar.Attendees.Count() }");
					Console.WriteLine($"Time taken to calculate: { sw.ElapsedMilliseconds }ms.");

					if (firstAvailableMeetingSlot != null)
					{
						Console.WriteLine($"A meeting slot for {duration} minutes is available between:" +
										  $"{firstAvailableMeetingSlot.StartTime:hh:mm tt} and {firstAvailableMeetingSlot.EndTime:hh:mm tt}.");
					}
					else
					{
						Console.WriteLine(
							$"Sorry ! There is no meeting slot of {duration} minutes is available for today. Please check for tomorrow.");
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