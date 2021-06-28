/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using MeetingCalendar.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MeetingCalendar.Tests.Exceptions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class DictionaryExtensionExceptionTests
	{
		[Test]
		public void FillWith_Throws_Exception_When_Source_Is_IsNull()
		{
			Assert.Throws<ArgumentNullException>(() =>
				((IDictionary<DateTime, AvailabilityTypes>)null).FillWith(AvailabilityTypes.Available));
		}

		[Test]
		public void ToConcurrentDictionary_Throws_Exception_When_Source_Is_IsNull()
		{
			Assert.Throws<ArgumentNullException>(() =>
				((IDictionary<DateTime, AvailabilityTypes>)null).ToConcurrentDictionary());
		}
	}
}