using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeetingCalendar.Extensions;
using NUnit.Framework;

namespace MeetingCalendarTest
{
    [TestFixture]
    [Author("A Basuri", "a.basuri2002@gmail.com")]
    public class EnumerableExtensionTests
    {
        [Test]
        public void It_Invokes_Action_Foreach_Items_In_Collection()
        {
            var sum = 0;

            void Summation(int i) => sum += i;

            Enumerable.Range(1, 10).ForEach(Summation);
            
            Assert.That(sum, Is.EqualTo(55));
        }
    }
}
