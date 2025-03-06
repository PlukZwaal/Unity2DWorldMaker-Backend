using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using Microsoft.Extensions.Logging;
using System;

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

[TestClass]
public class Object2DControllerTests
{
    [TestMethod]
    public async Task GetObjectsByEnvironment_ReturnsOk_WithObjects()
    {
        var mockRepository = new Mock<IObject2DRepository>();
        var mockAuthService = new Mock<IAuthenticationService>();
        var mockLogger = new Mock<ILogger<Object2DController>>();

        var controller = new Object2DController(mockRepository.Object, mockAuthService.Object, mockLogger.Object);

        var environmentId = "env1";
        var objects = new List<Object2D>
        {
            new Object2D { id = Guid.NewGuid().ToString(), environmentId = environmentId, prefabId = "prefab1" },
            new Object2D { id = Guid.NewGuid().ToString(), environmentId = environmentId, prefabId = "prefab2" }
        };

        mockRepository.Setup(repo => repo.GetObjectsByEnvironmentIdAsync(environmentId)).ReturnsAsync(objects);

        var result = await controller.GetObjectsByEnvironment(environmentId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedObjects = okResult.Value as List<Object2D>;
        Assert.IsNotNull(returnedObjects);
        Assert.AreEqual(2, returnedObjects.Count);
    }

    [TestMethod]
    public async Task CreateObject2D_ReturnsForbidden_WhenUserDoesNotOwnEnvironment()
    {
        var mockRepository = new Mock<IObject2DRepository>();
        var mockAuthService = new Mock<IAuthenticationService>();
        var mockLogger = new Mock<ILogger<Object2DController>>();

        var controller = new Object2DController(mockRepository.Object, mockAuthService.Object, mockLogger.Object);

        var environmentId = "env1";
        var userId = "user2"; 
        var objectToCreate = new Object2D { id = Guid.NewGuid().ToString(), environmentId = environmentId, prefabId = "prefab1" };

        mockRepository.Setup(repo => repo.CheckEnvironmentOwnership(environmentId, userId)).ReturnsAsync(false);
        mockAuthService.Setup(auth => auth.GetCurrentAuthenticatedUserId()).Returns(userId);

        var result = await controller.CreateObject2D(environmentId, objectToCreate);

        var forbidResult = result as ForbidResult;
        Assert.IsNotNull(forbidResult);
    }
}
