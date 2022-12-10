using CovidService.Controllers;
using CovidService.Entities;
using CovidService.Repositories;
using Microsoft.AspNetCore.Mvc;
using CovidService.Contracts;
using Moq;
using Newtonsoft.Json;
using System.Net;
using CovidService.Models;

namespace CovidService.Test
{

    [TestFixture]

    public class CovidServicesControllerTests
    {
        private CovidServicesController _covidServicesController;
        private Mock<ICovidDetailsContract> _covidDetailsContractMock;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _covidDetailsContractMock = new Mock<ICovidDetailsContract>();
            _covidServicesController = new CovidServicesController(_covidDetailsContractMock.Object);
        }

        [Test]
        public void GetAll_ReturnsOkResult_WhenDataIsAvailable()
        {
            // Arrange
            var covidDetails = new List<CovidDetails>
        {
            new CovidDetails { CountryName = "USA", InfectedCases = 100, DeceasedCases = 10, RecoveredCases = 20 },
            new CovidDetails { CountryName = "India", InfectedCases = 200, DeceasedCases = 20, RecoveredCases = 40 }
        };
            _covidDetailsContractMock
                .Setup(x => x.GetCovidDetails())
                .Returns(covidDetails);

            // Act
            var result = _covidServicesController.GetAll();

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.AreEqual(covidDetails, okObjectResult.Value);
        }

        [Test]
        public void GetAll_ReturnsInternalServerErrorResult_WhenDataIsNotAvailable()
        {
            // Arrange
            var exceptionMessage = "Error while fetching covid details";
            _covidDetailsContractMock
                .Setup(x => x.GetCovidDetails())
                .Throws(new Exception(exceptionMessage));

            // Act
            var result = _covidServicesController.GetAll();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual(exceptionMessage, objectResult.Value);
        }



        [Test]
        public void Get_ReturnsOkResult_WhenDataIsAvailable()
        {
            // Arrange
            var covidDetails = new CovidDetails { CountryName = "USA", InfectedCases = 100, DeceasedCases = 10, RecoveredCases = 20 };
            _covidDetailsContractMock
                .Setup(x => x.GetCovidDetail("USA"))
                .Returns(covidDetails);

            // Act
            var result = _covidServicesController.Get("USA");

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.AreEqual(covidDetails, okObjectResult.Value);
        }



    }

}