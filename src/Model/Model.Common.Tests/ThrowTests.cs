using App.Models;

namespace App.Model.Common.Tests;

public class ThrowTests
{
	[Theory]
	[InlineData(ThrowKind.None, 1, 0)]
	[InlineData(ThrowKind.Foul, 1, 0)]
	[InlineData(ThrowKind.Single, 1, 1)]
	[InlineData(ThrowKind.Double, 1, 2)]
	[InlineData(ThrowKind.Triple, 1, 3)]
	[InlineData(ThrowKind.OuterBull, 25, 25)]
	[InlineData(ThrowKind.InnerBull, 25, 50)]
	public void ValueRegionGetsMultipliedByThrowKindMultiplierWhenValid(ThrowKind throwKind, int valueRegion, int expectedPoints)
	{
		// Arrange
		var @throw = new Throw(valueRegion, throwKind);
		@throw.IsValid = true;

		// Assert
		Assert.Equal(expectedPoints, @throw.AssignedPoints);
	}

	[Theory]
	[InlineData(ThrowKind.None, 1)]
	[InlineData(ThrowKind.Foul, 1)]
	[InlineData(ThrowKind.Single, 1)]
	[InlineData(ThrowKind.Double, 1)]
	[InlineData(ThrowKind.Triple, 1)]
	[InlineData(ThrowKind.OuterBull, 25)]
	[InlineData(ThrowKind.InnerBull, 25)]
	public void AssignedPointsIsZeroWhenInvalid(ThrowKind throwKind, int valueRegion)
	{
		// Arrange
		var @throw = new Throw(valueRegion, throwKind);
		@throw.IsValid = false;

		// Assert
		Assert.Equal(0, @throw.AssignedPoints);
	}

	[Theory]
	[InlineData(ThrowKind.None, false)]
	[InlineData(ThrowKind.Foul, false)]
	[InlineData(ThrowKind.Single, false)]
	[InlineData(ThrowKind.Double, true)]
	[InlineData(ThrowKind.Triple, false)]
	[InlineData(ThrowKind.OuterBull, false)]
	[InlineData(ThrowKind.InnerBull, true)]
	public void ValidLastThrowKinds(ThrowKind throwKind, bool isValidLastThrow)
	{
		// Arrange
		var @throw = new Throw(1, throwKind);
		@throw.IsValid = true;

		// Assert
		Assert.Equal(isValidLastThrow, @throw.IsValidLastThrow());
	}
}