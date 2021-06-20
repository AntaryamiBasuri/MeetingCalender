/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace MeetingCalendarTest.Extensions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class CollectionExtensionsTests
	{
		[Test]
		public void Concatenates_Two_Collection_And_Returns_The_First_Collection_With_Concatenated_Items()
		{
			var firstGroup = new Collection<int> { 0, 1, 2, 3, 4 };
			var secondGroup = new Collection<int> { 5, 6, 7, 8, 9 };

			var result = firstGroup.Concat(secondGroup);

			Assert.That(result.Count, Is.EqualTo(10));
			Assert.That(result, Is.SameAs(firstGroup));
			Assert.That(ReferenceEquals(firstGroup, result), Is.True);
		}
	}
}