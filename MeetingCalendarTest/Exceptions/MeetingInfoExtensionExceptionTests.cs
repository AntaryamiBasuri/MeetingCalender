/*
 * Author: Antaryami Basuri
 * Email: a.basuri2002@gmail.com
 */

using MeetingCalendar.Extensions;
using MeetingCalendar.Interfaces;
using NUnit.Framework;
using System;

namespace MeetingCalendar.Tests.Exceptions
{
	[TestFixture]
	[Author("A Basuri", "a.basuri2002@gmail.com")]
	public class MeetingInfoExtensionExceptionTests
	{
		[Test]
		public void IsOver_Throws_Exception_When_Source_Collection_IsNull() =>
			Assert.Throws<ArgumentNullException>(() => ((IMeetingInfo)null).IsOver());
	}
}