using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    [TestMethod]
    public void ReserveSeat()
    {
        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Mock<SeatsController> SeatsControllerMock = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };

        Seat seat = new Seat()
        {
            Id = 1,
            Number = 1
        };

        serviceMock.Setup(x => x.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Returns(seat);

        SeatsControllerMock.Setup(t => t.UserId).Returns("1");

        //var result = SeatsControllerMock.Setup(t => t.ReserveSeat(It.Is<int>(i => i > -1 && i < 101)));

        var actionResult = SeatsControllerMock.Object.ReserveSeat(seat.Number);
        var result = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReserveSeatTaken()
    {
        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Mock<SeatsController> SeatsControllerMock = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };

        serviceMock.Setup(x => x.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatAlreadyTakenException());

        SeatsControllerMock.Setup(t => t.UserId).Returns("1");

        var actionResult = SeatsControllerMock.Object.ReserveSeat(1);
        var result = actionResult.Result as UnauthorizedResult;

        Assert.IsNotNull(result);
        
    }

    [TestMethod]
    public void ReserveSeatOutBounds()
    {
        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Mock<SeatsController> SeatsControllerMock = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };

        serviceMock.Setup(x => x.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new SeatOutOfBoundsException());

        SeatsControllerMock.Setup(t => t.UserId).Returns("1");

        var actionResult = SeatsControllerMock.Object.ReserveSeat(1);
        var result = actionResult.Result as NotFoundObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual("Could not find " + 1, result.Value);
    }

    [TestMethod]
    public void ReserveSeatAlreadySeated()
    {
        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Mock<SeatsController> SeatsControllerMock = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };

        serviceMock.Setup(x => x.ReserveSeat(It.IsAny<string>(), It.IsAny<int>())).Throws(new UserAlreadySeatedException());

        SeatsControllerMock.Setup(t => t.UserId).Returns("1");

        var actionResult = SeatsControllerMock.Object.ReserveSeat(1);
        var result = actionResult.Result as BadRequestResult;

        Assert.IsNotNull(result);

    }
}
