/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using System;

namespace MeetingCalendar
{
    public class MeetingInfo: TimeSlot
    {
        //NOTE:This class has been created to carry some additional info about the meeting

        /// <summary>
        /// Initializes a new instance of <see cref="MeetingInfo"/>
        /// </summary>
        /// <param name="startTime">The meeting start time.</param>
        /// <param name="endTime">The meeting end time.</param>
        public MeetingInfo(DateTime startTime, DateTime endTime) 
            : base(startTime, endTime)
        {
           
        }
    }
}
