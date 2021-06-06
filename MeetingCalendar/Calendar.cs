/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using MeetingCalendar.Interfaces;
using MeetingCalendar.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MeetingCalendar
{
	/// <summary>
	/// Provides a <see cref="Calendar"/>
	/// </summary>
	public class Calendar : ICalendar
	{
		#region Members

		private readonly DateTime _startTime;
		private readonly DateTime _endTime;
		private readonly IList<TimeSlot> _availableMeetingSlots;

		#endregion Members

		#region Private computed properties

		private static DateTime FutureDateTime => DateTime.MaxValue.AddMinutes(-1);
		private double CalendarWindowInMinutes => _endTime.Subtract(_startTime).TotalMinutes;

		#endregion Private computed properties

		#region Public Properties

		/// <summary>
		/// Gets the current date and time calibrated to minutes.
		/// </summary>
		public DateTime CurrentTime => DateTime.Now.CalibrateToMinutes();

		/// <summary>
		/// Gets the list of <see cref="Attendee"/>.
		/// </summary>
		public IEnumerable<Attendee> Attendees { get; private set; }

		#endregion Public Properties

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="Calendar"/>
		/// </summary>
		/// <param name="startTime">The lower bound of allowed meeting hours.</param>
		/// <param name="endTime">The upper bound of allowed meeting hours.</param>
		public Calendar(DateTime startTime, DateTime endTime)

		{
			if (startTime.IsInvalidDate())
				throw new ArgumentException("Invalid Calendar start time.", nameof(startTime));

			if (endTime.IsInvalidDate())
				throw new ArgumentException("Invalid Calendar end time.", nameof(endTime));

			_startTime = startTime.CalibrateToMinutes();
			_endTime = endTime.CalibrateToMinutes();

			if (_startTime >= _endTime)
				throw new ArgumentException("The Calendar end time must be greater than the start time.", nameof(endTime));

			_availableMeetingSlots = new List<TimeSlot>();
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Calendar"/>
		/// </summary>
		/// <param name="startTime">The lower bound of allowed meeting hours.</param>
		/// <param name="endTime">The upper bound of allowed meeting hours.</param>
		/// <param name="attendees">Attendees along with their <see cref="MeetingInfo"/></param>
		public Calendar(DateTime startTime, DateTime endTime, IEnumerable<Attendee> attendees)
			: this(startTime, endTime)
			=> Attendees = attendees;

		#endregion Constructors

		#region Public methods

		/// <summary>
		/// Add attendees to the calendar.
		/// </summary>
		public void AddAttendees(IEnumerable<Attendee> attendees) => Attendees = attendees;

		/// <summary>
		/// Append additional attendees to existing attendees list.
		/// </summary>
		/// <param name="additionalAttendees"></param>
		public void AppendAttendees(IEnumerable<Attendee> additionalAttendees)
			=> Attendees = Attendees?.Concat(additionalAttendees) ?? additionalAttendees;

		/// <summary>
		/// Finds the first available time slot for the requested meeting duration.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration in minutes.</param>
		/// <returns>A time slot or null</returns>
		public TimeSlot FindFirstAvailableSlot(int meetingDuration)
		{
			var availableMeetingSlots = GetAllAvailableTimeSlots();

			return availableMeetingSlots.FindFirst(t => t.GetDuration() >= meetingDuration);
		}

		/// <summary>
		/// Finds the first available time slot for the requested meeting duration within a specific time frame.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration in minutes.</param>
		/// <param name="fromTime">The lower bound date time for a search.</param>
		/// <param name="toTime">The upper bound date time for a search.</param>
		/// <exception cref="ArgumentException">
		/// The argument exception, when toTime is less than fromTime OR
		/// less than equal to current time.
		/// </exception>
		/// <returns>A time slot or null</returns>
		public TimeSlot FindFirstAvailableSlot(int meetingDuration, DateTime fromTime, DateTime toTime = default)
		{
			ValidateSearchRange(meetingDuration, fromTime, toTime);

			toTime = (toTime == default) ? _endTime : toTime;

			var lowerBound = (fromTime >= _startTime) ? fromTime.CalibrateToMinutes() : _startTime;
			var upperBound = (toTime >= _endTime) ? _endTime : toTime.CalibrateToMinutes();

			var availableMeetingSlots = GetAllAvailableTimeSlots();

			return availableMeetingSlots
				.Select(t => TimeSlotMapper(t, lowerBound, upperBound))
				.FindFirst(t => t.GetDuration() >= meetingDuration && t.StartTime >= lowerBound && t.EndTime <= upperBound);
		}

		/// <summary>
		/// Find all available meeting slots.
		/// </summary>
		/// <returns>A list of <see cref="TimeSlot"/></returns>
		public IEnumerable<TimeSlot> GetAllAvailableTimeSlots()
		{
			_availableMeetingSlots.Clear();

			//Do not calculate available meeting slots for past meetings - Performance improvement
			if (_endTime <= CurrentTime)
			{
				return _availableMeetingSlots;
			}

			//Calculate the availability only from NOW onwards - Performance improvement
			var startTime = (_startTime >= CurrentTime) ? _startTime : CurrentTime;
			//TODO: Use Base as TimeSlot to avoid creating new TimeSLot
			var meetingHoursByMinutes = new TimeSlot(startTime, _endTime).GetTimeSeriesByMinutes();

			//Map the meeting timings of each attendees
			if (Attendees != null && Attendees.Any())
			{
				if (Attendees.Count() <= 8 && CalendarWindowInMinutes <= 960)
				{
					GetAllAvailableTimeSlots(meetingHoursByMinutes);
				}
				else
				{
					GetAllAvailableTimeSlotsAsParallel(meetingHoursByMinutes);
				}

				//Calculate only if any slot is available.
				if (meetingHoursByMinutes.Any(i => !i.Value))
				{
					CalculateAvailableSlots(meetingHoursByMinutes.OrderBy(i => i.Key));
				}
			}
			else
			{
				var meetingHoursByMinutesList = meetingHoursByMinutes.OrderBy(i => i.Key).ToList();
				_availableMeetingSlots.Add(new TimeSlot(meetingHoursByMinutesList.First().Key, meetingHoursByMinutesList.Last().Key));
			}

			return _availableMeetingSlots;
		}

		#endregion Public methods

		#region Private methods

		/// <summary>
		/// Find all available <see cref="TimeSlot"/>
		/// </summary>
		/// <param name="meetingHoursByMinutes"></param>
		private void GetAllAvailableTimeSlots(IDictionary<DateTime, bool> meetingHoursByMinutes) =>
			Attendees.ForEach(attendee =>
			{
				attendee.Meetings.Where(meeting => !meeting.IsOver())
					.ForEach(scheduledMeeting =>
						scheduledMeeting
							.GetTimeSlotMappedToCalenderTimeFrame(_startTime, _endTime)
							.GetTimeSeriesByMinutes(true)
							.ForEach(item =>
							{
								//Update the value only when the minute has not been marked yet, as unavailable - Performance improvement
								if (meetingHoursByMinutes.TryGetValue(item.Key, out var prevValue) && !prevValue)
								{
									meetingHoursByMinutes[item.Key] = item.Value; //Sets to true - i.e. unavailable
								}
							})
					);
			});

		private void GetAllAvailableTimeSlotsAsParallel(ConcurrentDictionary<DateTime, bool> meetingHoursByMinutes) =>
			Attendees.ForEach(attendee =>
			{
				attendee.Meetings
					.Where(meeting => !meeting.IsOver())
					.AsParallel().ForAll(scheduledMeeting =>
						scheduledMeeting
							.GetTimeSlotMappedToCalenderTimeFrame(_startTime, _endTime)
							.GetTimeSeriesByMinutes(true)
							.AsParallel().ForAll(item =>
							{
								// Updates and sets the value to true - i.e. unavailable, only when the minute has not been marked yet - Performance improvement
								meetingHoursByMinutes.TryUpdate(item.Key, item.Value, false);
							})
					);
			});

		private void CalculateAvailableSlots(IEnumerable<KeyValuePair<DateTime, bool>> scheduledHoursByMinutes, TimeSlot availableTimeSlot = null, bool searchVal = false)
		{
			var hoursByMinutes = scheduledHoursByMinutes.ToList();
			if (!hoursByMinutes.Any()) return;

			var foundItem = hoursByMinutes.FirstOrDefault(item => item.Value == searchVal);
			if (foundItem.Key != default)
			{
				var subSet = hoursByMinutes.Where(dict => dict.Key > foundItem.Key);
				if (searchVal && availableTimeSlot != null)
				{
					availableTimeSlot = new TimeSlot(availableTimeSlot.StartTime, foundItem.Key.AddMinutes(-1));
					_availableMeetingSlots.Add(availableTimeSlot);
					CalculateAvailableSlots(subSet);
				}
				else
				{
					availableTimeSlot = new TimeSlot(foundItem.Key, FutureDateTime);
					CalculateAvailableSlots(subSet, availableTimeSlot, true);
				}
			}
			else if (availableTimeSlot != null)
			{
				foundItem = hoursByMinutes.Last();
				availableTimeSlot = new TimeSlot(availableTimeSlot.StartTime, foundItem.Key);
				_availableMeetingSlots.Add(availableTimeSlot);
			}
		}

		/// <summary>
		/// Validates search range parameters.
		/// </summary>
		/// <param name="meetingDuration"></param>
		/// <param name="fromTime"></param>
		/// <param name="toTime"></param>
		private void ValidateSearchRange(int meetingDuration, DateTime fromTime, DateTime toTime)
		{
			if (meetingDuration <= 0)
			{
				throw new ArgumentException("The meeting duration can not be less than or equal to zero.",
					nameof(meetingDuration));
			}

			if (toTime != default &&
				meetingDuration > toTime.CalibrateToMinutes().Subtract(fromTime.CalibrateToMinutes()).TotalMinutes)
			{
				throw new ArgumentException(
					"The meeting duration can not be longer than the search range.Consider to increase the search range.",
					nameof(meetingDuration));
			}

			if (toTime != default && toTime.CalibrateToMinutes() <= CurrentTime)
			{
				throw new ArgumentException("Search range upper limit can not be less than current time.", nameof(toTime));
			}
		}

		/// <summary>
		/// Projects a new time slot.
		/// </summary>
		/// <param name="timeSlot">The time slot.</param>
		/// <param name="lowerBound">The search lower bound.</param>
		/// <param name="upperBound">The search upper bound.</param>
		/// <returns> A new time slot mapped to calendar LB and UB</returns>
		private static TimeSlot TimeSlotMapper(TimeSlot timeSlot, DateTime lowerBound, DateTime upperBound)
		{
			var (startTime, endTime) = timeSlot;

			return new TimeSlot(
				((timeSlot.StartTime <= lowerBound && lowerBound < timeSlot.EndTime) ? lowerBound : startTime),
				((timeSlot.StartTime < upperBound && upperBound < timeSlot.EndTime) ? upperBound : endTime));
		}

		#endregion Private methods
	}
}