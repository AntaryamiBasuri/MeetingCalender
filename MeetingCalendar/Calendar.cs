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
	/// Provides an instance of <see cref="ICalendar"/>.
	/// </summary>
	public class Calendar : ICalendar, ITimeSlot
	{
		#region Members

		private static DateTime FutureDateTime => DateTime.MaxValue.AddMinutes(-1);

		private readonly DateTime _startTime;
		private readonly DateTime _endTime;
		private readonly IList<ITimeSlot> _availableMeetingSlots;

		private ICollection<IAttendee> _attendees;

		#endregion Members

		#region Private computed properties

		private double CalendarWindowInMinutes => _endTime.Subtract(_startTime).TotalMinutes;

		//Calculate the availability only from NOW onwards - Performance improvement
		private DateTime TimeSlotComputationStartTime => (_startTime >= CurrentTime) ? _startTime : CurrentTime;

		#endregion Private computed properties

		#region Public Properties

		/// <summary>
		/// Gets the start time of <see cref="Calendar"/>.
		/// </summary>
		public DateTime StartTime => _startTime;

		/// <summary>
		/// Gets the end time of <see cref="Calendar"/>.
		/// </summary>
		public DateTime EndTime => _endTime;

		/// <summary>
		/// Gets the current date and time calibrated to minutes.
		/// </summary>
		public DateTime CurrentTime => DateTime.Now.CalibrateToMinutes();

		/// <summary>
		/// Gets the list of <see cref="IAttendee"/>.
		/// </summary>
		public IReadOnlyCollection<IAttendee> Attendees => _attendees?.ToList().AsReadOnly();

		#endregion Public Properties

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Calendar"/> class.
		/// </summary>
		/// <param name="timeSlot">The <see cref="ITimeSlot"/> as calender window.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The <see cref="Calendar"/> start time is invalid.
		/// The <see cref="Calendar"/> end time is invalid.
		/// The <see cref="Calendar"/> start time is greater than or equals to start time.
		/// </exception>
		public Calendar(ITimeSlot timeSlot)
			: this(timeSlot.StartTime, timeSlot.EndTime)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Calendar"/> class.
		/// </summary>
		/// <param name="startTime">The lower bound of allowed meeting hours.</param>
		/// <param name="endTime">The upper bound of allowed meeting hours.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The <see cref="Calendar"/> start time is invalid.
		/// The <see cref="Calendar"/> end time is invalid.
		/// The <see cref="Calendar"/> start time is greater than or equals to start time.
		/// </exception>
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

			_availableMeetingSlots = new List<ITimeSlot>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Calendar"/> class.
		/// </summary>
		/// <param name="timeSlot">The <see cref="ITimeSlot"/> as calender window.</param>
		/// <param name="attendees">A list of <see cref="Attendee"/> along with their <see cref="MeetingInfo"/>.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The <see cref="Calendar"/> start time is invalid.
		/// The <see cref="Calendar"/> end time is invalid.
		/// The <see cref="Calendar"/> start time is greater than or equals to start time.
		/// </exception>
		public Calendar(ITimeSlot timeSlot, ICollection<IAttendee> attendees)
			: this(timeSlot.StartTime, timeSlot.EndTime)
			=> _attendees = attendees;

		/// <summary>
		/// Initializes a new instance of the <see cref="Calendar"/> class.
		/// </summary>
		/// <param name="startTime">The lower bound of allowed meeting hours.</param>
		/// <param name="endTime">The upper bound of allowed meeting hours.</param>
		/// <param name="attendees">A list of <see cref="Attendee"/> along with their <see cref="MeetingInfo"/>.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The <see cref="Calendar"/> start time is invalid.
		/// The <see cref="Calendar"/> end time is invalid.
		/// The <see cref="Calendar"/> start time is greater than or equals to start time.
		/// </exception>
		public Calendar(DateTime startTime, DateTime endTime, ICollection<IAttendee> attendees)
			: this(startTime, endTime)
			=> _attendees = attendees;

		#endregion Constructors

		#region Public methods

		/// <summary>
		/// Add attendees to the calendar.
		/// </summary>
		/// <param name="attendees">The list of <see cref="Attendee"/>.</param>
		public void AddAttendees(ICollection<IAttendee> attendees) => _attendees = attendees;

		/// <summary>
		/// Append additional attendees to existing attendees list.
		/// </summary>
		/// <param name="additionalAttendees">The list of additional <see cref="Attendee"/> to append.</param>
		public void AppendAttendees(ICollection<IAttendee> additionalAttendees)
			=> _attendees = _attendees?.Concat(additionalAttendees ?? new List<IAttendee>()) ?? additionalAttendees;

		/// <summary>
		/// Finds the first available time slot for the requested meeting duration.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration in minutes.</param>
		/// <returns>A time slot or null.</returns>
		public ITimeSlot FindFirstAvailableSlot(int meetingDuration)
		{
			var availableMeetingSlots = GetAllAvailableTimeSlots();

			return availableMeetingSlots.FindFirst(t => t.GetDuration() >= meetingDuration);
		}

		/// <summary>
		/// Finds the first available time slot for the requested meeting duration, optionally within a specific time frame.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration in minutes.</param>
		/// <param name="timeSlot">The time slot as the search range.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The meeting duration is less than or equal to zero.
		/// The meeting duration is longer than the search range.
		/// The search range upper limit is less than current time.
		/// The search range start time is invalid.
		/// The search range upper bound is less than the lower bound time.
		/// </exception>
		/// <returns>A an instance of <see cref="ITimeSlot"/> or null.</returns>
		public ITimeSlot FindFirstAvailableSlot(int meetingDuration, ITimeSlot timeSlot)
		{
			var (fromTime, toTime) = timeSlot;
			return FindFirstAvailableSlot(meetingDuration, fromTime, toTime);
		}

		/// <summary>
		/// Finds the first available time slot for the requested meeting duration, optionally within a specific time frame.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration in minutes.</param>
		/// <param name="fromTime">The lower bound date time for a search.</param>
		/// <param name="toTime">The upper bound date time for a search.</param>
		/// <exception cref="ArgumentException">
		/// Thrown when:
		/// The meeting duration is less than or equal to zero.
		/// The meeting duration is longer than the search range.
		/// The search range upper limit is less than current time.
		/// The search range start time is invalid.
		/// The search range upper bound is less than the lower bound time.
		/// </exception>
		/// <returns>A an instance of <see cref="ITimeSlot"/> or null.</returns>
		public ITimeSlot FindFirstAvailableSlot(int meetingDuration, DateTime fromTime, DateTime toTime = default)
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
		/// <returns>A list of <see cref="ITimeSlot"/>.</returns>
		public IEnumerable<ITimeSlot> GetAllAvailableTimeSlots()
		{
			_availableMeetingSlots.Clear();

			//Do not calculate available meeting slots for past meetings - Performance improvement
			if (_endTime <= CurrentTime)
			{
				return _availableMeetingSlots;
			}

			var meetingHoursByMinutes = this.GetTimeSeriesByMinutes(false, TimeSlotComputationStartTime);

			//Map the meeting timings of each attendees
			if (Attendees != null && Attendees.Any())
			{
				if (Attendees.Count <= 8 && CalendarWindowInMinutes <= 960)
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

		/// <summary>
		/// Deconstruct a <see cref="TimeSlot"/>.
		/// </summary>
		/// <param name="startTime">The start time.</param>
		/// <param name="endTime">The end time.</param>
		public void Deconstruct(out DateTime startTime, out DateTime endTime)
		{
			startTime = StartTime;
			endTime = EndTime;
		}

		#endregion Public methods

		#region Private methods

		/// <summary>
		/// Projects a new time slot.
		/// </summary>
		/// <param name="timeSlot">The time slot.</param>
		/// <param name="lowerBound">The search lower bound.</param>
		/// <param name="upperBound">The search upper bound.</param>
		/// <returns> A new instance of <see cref="ITimeSlot"/> mapped to calendar LB and UB.</returns>
		private static ITimeSlot TimeSlotMapper(ITimeSlot timeSlot, DateTime lowerBound, DateTime upperBound)
		{
			var (startTime, endTime) = timeSlot;

			return new TimeSlot(
				startTime <= lowerBound && lowerBound < endTime ? lowerBound : startTime,
				startTime < upperBound && upperBound < endTime ? upperBound : endTime);
		}

		/// <summary>
		/// Find all available <see cref="TimeSlot"/>.
		/// </summary>
		/// <param name="meetingHoursByMinutes">The meeting duration converted to a list of minutes.</param>
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
							}));
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
							}));
			});

		private void CalculateAvailableSlots(IEnumerable<KeyValuePair<DateTime, bool>> scheduledHoursByMinutes, ITimeSlot availableTimeSlot = null, bool searchVal = false)
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
		/// Validate the search parameters.
		/// </summary>
		/// <param name="meetingDuration">The meeting duration.</param>
		/// <param name="fromTime">The fromTime.</param>
		/// <param name="toTime">The toTime.</param>
		private void ValidateSearchRange(int meetingDuration, DateTime fromTime, DateTime toTime)
		{
			if (fromTime.IsInvalidDate())
				throw new ArgumentException("Invalid start time.", nameof(fromTime));

			if (meetingDuration <= 0)
			{
				throw new ArgumentException("The meeting duration can not be less than or equal to zero.",
					nameof(meetingDuration));
			}

			if (toTime != default && toTime.CalibrateToMinutes() <= fromTime.CalibrateToMinutes())
			{
				throw new ArgumentException("Search range upper limit can not be less than the lower limit time.", nameof(toTime));
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
				throw new ArgumentException("Search range upper limit can not be less than the current time.", nameof(toTime));
			}
		}

		#endregion Private methods
	}
}