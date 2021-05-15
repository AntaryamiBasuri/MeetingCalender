/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using NUnit.Framework;
using System.Linq;

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
			var callCount = 0;

			void MockAction(int i)
			{
				sum += i;
				callCount++;
			}

			var mock = Enumerable.Range(1, 10);
			mock.ForEach(MockAction);
			const int expected = (10 * (10 + 1)) >> 1;

			Assert.That(callCount, Is.EqualTo(10));
			Assert.That(sum, Is.EqualTo(expected));
		}
	}
}