
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAPI.Interfaces;
using Microsoft.Extensions.Logging;

[TestClass]
public class Environment2DControllerTests
{
    [TestMethod]
    public async Task GetEnvironmentsForUser_ReturnsOk_WithEnvironments()
    {
        var mockRepository = new Mock<IEnvironment2DRepository>(); 
        var mockAuthService = new Mock<IAuthenticationService>();
        var mockLogger = new Mock<ILogger<Environment2DController>>();

        var controller = new Environment2DController(mockRepository.Object, mockAuthService.Object, mockLogger.Object);

        var userId = "user1";
        var environments = new List<Environment2D>
        {
            new Environment2D { id = "env1", name = "TestEnv1" },
            new Environment2D { id = "env2", name = "TestEnv2" }
        };

        mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns(userId);
        mockRepository.Setup(repo => repo.GetEnvironment2DsByUserIdAsync(userId)).ReturnsAsync(environments);

        var result = await controller.GetEnvironmentsForUser();

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedEnvironments = okResult.Value as List<Environment2D>;
        Assert.IsNotNull(returnedEnvironments);
        Assert.AreEqual(2, returnedEnvironments.Count);
    }
}