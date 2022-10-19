using App.Models;

namespace App.Model.Common.Tests;

public class SetTests
{
	public Set MakeSet(int legPlayerCount)
	{
		var mock = new Moq.Mock<Set>();

		mock.CallBase = true;
		mock.Setup(x => x.CreateLeg(0)).Returns(() =>
		{
			var legMock = new Moq.Mock<Leg>();
			legMock.Setup(x => x.CurrentPlayerIndex).Returns(0);

			return legMock.Object;
		});
		// mock.Protected()
		// .Setup("CreateLeg", )

		return mock.Object;
	}
}