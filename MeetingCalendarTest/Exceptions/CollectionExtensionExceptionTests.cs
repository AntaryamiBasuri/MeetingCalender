/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MeetingCalendar.Tests.Exceptions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class CollectionExtensionExceptionTests
	{
		[Test]
		public void Throws_Exception_When_Source_Collection_IsNull()
		{
			ICollection<int> collection = null;

			Assert.Throws<ArgumentNullException>(() => collection.Concat(new List<int>()));
		}

		[Test]
		public void Throws_Exception_When_Second_Collection_IsNull()
		{
			ICollection<int> collection = new Collection<int>() { 1, 2, 3, 4, 5 };

			Assert.Throws<ArgumentNullException>(() => collection.Concat(null));
		}
	}
}