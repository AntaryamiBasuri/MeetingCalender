/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingCalendarTest.Exceptions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class EnumerableExtensionExceptionTests
	{
		[Test]
		public void Throws_Exception_When_Source_Collection_IsNull()
		{
			IEnumerable<int> sut = null;

			Assert.Throws<ArgumentNullException>(() => sut.ForEach(i => { }));
		}

		[Test]
		public void Throws_Exception_When_Action_IsNull()
		{
			Action<int> mockAction = null;
			var mock = Enumerable.Range(1, 10);

			Assert.Throws<ArgumentNullException>(() => mock.ForEach(mockAction));
		}
	}
}