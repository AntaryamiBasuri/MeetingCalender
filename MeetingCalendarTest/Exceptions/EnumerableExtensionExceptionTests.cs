/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingCalendar.Tests.Exceptions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class EnumerableExtensionExceptionTests
	{
		[Test]
		public void Throws_Exception_When_Source_Collection_IsNull() =>
			Assert.Throws<ArgumentNullException>(() => ((IEnumerable<int>)null).ForEach(i => { }));

		[Test]
		public void Throws_Exception_When_Action_IsNull()
		{
			var mock = Enumerable.Range(1, 10);

			Assert.Throws<ArgumentNullException>(() => mock.ForEach(null));
		}
	}
}