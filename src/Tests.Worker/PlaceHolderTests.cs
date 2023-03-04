using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Worker;

public class PlaceHolderTests
{
	[Fact]
	public void JustAPlaceHolder()
	{
		var value = true;

		value.Should()
			.BeTrue();
	}
	
	[Fact]
	public void SecondPlaceHolder()
	{
		JustAPlaceHolder();
	}
}