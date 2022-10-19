using App.Models;

namespace App.Model.Common.Tests;

public class LegTests
{
	[Theory]
	[InlineData(5)]
	[InlineData(10)]
	[InlineData(20)]
	[InlineData(25)]
	public void PlayingTurnIncrementsPointsByTurnPoints(int points)
	{
		// Arrange
		var turnRules = new TurnRules(1);
		var legRules = new LegRules(turnRules, 501);
		var leg = new Leg(1);
		var turn = CreateTurnMock(points, true);

		// Act
		leg.PlayTurn(legRules, turn);

		// Assert
		Assert.Equal(leg.Points[0], points);
	}

	[Fact]
	public void ValidLastTurnEndsLeg()
	{
		// Arrange
		var turnRules = new TurnRules(1);
		var legRules = new LegRules(turnRules, 1);
		var leg = new Leg(1);
		var turn = CreateTurnMock(1, true);

		// Act
		leg.PlayTurn(legRules, turn);

		// Assert
		Assert.True(leg.IsDone);
	}

	[Fact]
	public void InvalidLastTurnDoesntEndLeg()
	{
		// Arrange
		var turnRules = new TurnRules(1);
		var legRules = new LegRules(turnRules, 1);
		var leg = new Leg(1);
		var turn = CreateTurnMock(1, false);

		// Act
		leg.PlayTurn(legRules, turn);

		// Assert
		Assert.False(leg.IsDone);
	}

	[Fact]
	public void CurrentPlayerIndexIncrementsAfterTurn()
	{
		// Arrange
		var turnRules = new TurnRules(1);
		var legRules = new LegRules(turnRules, 1);
		var leg = new Leg(2);
		var turn = CreateTurnMock();

		// Act
		leg.PlayTurn(legRules, turn);

		// Assert
		Assert.Equal(1, leg.CurrentPlayerIndex);
	}

	[Fact]
	public void CurrentPlayerIndexWrapsAround()
	{
		// Arrange
		var turnRules = new TurnRules(1);
		var legRules = new LegRules(turnRules, 1);
		var leg = new Leg(2);
		var turn = CreateTurnMock();

		// Act
		leg.PlayTurn(legRules, turn);
		leg.PlayTurn(legRules, turn);

		// Assert
		Assert.Equal(0, leg.CurrentPlayerIndex);
	}

	private ITurn CreateTurnMock(int points = 0, bool isValid = true)
	{
		var mock = new Moq.Mock<ITurn>();
		mock.Setup(x => x.ThrownPoints).Returns(points);

		if (isValid)
			mock.Setup(x => x.AssignedPoints).Returns(points);
		else
			mock.Setup(x => x.AssignedPoints).Returns(0);

		mock.Setup(x => x.IsValid).Returns(isValid);
		mock.Setup(x => x.IsValidLastTurn()).Returns(isValid);

		return mock.Object;
	}
}