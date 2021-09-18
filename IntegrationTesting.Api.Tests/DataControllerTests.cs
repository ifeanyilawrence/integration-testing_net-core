namespace IntegrationTesting.Api.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using IntegrationTesting.Api.Controllers;
    using IntegrationTesting.Service;
    using IntegrationTesting.Shared;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DataControllerTests
    {
        private Mock<IFetchData> _mockFetchData;
        private DataController _controllerUnderTest;
        private static Fixture _fixture;
        private Mock<IAlternateClient> _mockHttpClient;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();

            _mockFetchData = new Mock<IFetchData>();

            _mockHttpClient = new Mock<IAlternateClient>();

            _controllerUnderTest = new DataController(_mockFetchData.Object, _mockHttpClient.Object);
        }


        [TestMethod]
        public async Task GetData_ShouldReturnNotFound_WhenQueryResultIsNull()
        {
            // Arrange
            _mockFetchData
                .Setup(exec => exec.GetRecords())
                .ReturnsAsync((DataResult)null);

            // Act
            var apiResult = await _controllerUnderTest
                .GetData() as NotFoundResult;

            // Assert
            apiResult
                .Should()
                .NotBeNull();

            apiResult
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.NotFound);

            _mockFetchData.Verify(exp => exp.GetRecords(), Times.Exactly(1));
        }


        [TestMethod]
        public async Task GetData_ShouldReturnOK_WhenResultIsNotNull()
        {
            // Arrange
            var expectedResult = _fixture.Create<DataResult>();
            _mockFetchData
                .Setup(s => s.GetRecords())
                .ReturnsAsync(expectedResult);

            // Act
            var apiResult = await _controllerUnderTest
                .GetData() as ObjectResult;

            // Assert
            apiResult
                .Should()
                .NotBeNull();

            apiResult
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            //apiResult
            //    .Value
            //    .Should()
            //    .Be(expectedResult);

            Assert.AreEqual(expectedResult, apiResult.Value);

            _mockFetchData.Verify(exp => exp.GetRecords(), Times.Exactly(1));
        }

        [TestMethod]
        public async Task GetDataFromAPI_ShouldReturnOK_WhenResultIsNotNull()
        {
            // Arrange
            var expectedResult = _fixture.Create<DataResult>();

            _mockHttpClient
                .Setup(exec => exec.GetStringAsync("/todogs/1"))
                .ReturnsAsync(JsonSerializer.Serialize(expectedResult));

            // Act
            var apiResult = await _controllerUnderTest
                .GetDataFromAPI() as ObjectResult;

            //Func<Task> func = async () => await _controllerUnderTest.GetDataFromAPI();
            //await func.Should().ThrowAsync<Exception>();

            // Assert
            apiResult
                .Should()
                .NotBeNull();

            apiResult
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            apiResult
                .Value
                .Should()
                .BeEquivalentTo(expectedResult);

            _mockHttpClient.Verify(exp => exp.GetStringAsync("/todogs/1"), Times.Exactly(1));
        }

        [TestMethod]
        public async Task GetDataFromAPI_ShouldNotThrowException()
        {
            // Arrange
            var expectedResult = _fixture.Create<DataResult>();

            _mockHttpClient
                .Setup(exec => exec.GetStringAsync("/todogs/1"))
                .ReturnsAsync(JsonSerializer.Serialize(expectedResult));

            // Act
            Func<Task> func = async () => await _controllerUnderTest.GetDataFromAPI();

            // Assert
            await func.Should().NotThrowAsync<Exception>();

            _mockHttpClient.Verify(exp => exp.GetStringAsync("/todogs/1"), Times.Exactly(1));
        }
    }
}
