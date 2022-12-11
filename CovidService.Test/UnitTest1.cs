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

        private CovidDetailsRepository _covidDetailsRepository;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _covidDetailsContractMock = new Mock<ICovidDetailsContract>();
            _covidServicesController = new CovidServicesController(_covidDetailsContractMock.Object);
            _covidDetailsRepository = new CovidDetailsRepository();

        }




        [Test]
        public void GetAll_ReturnsOkResult_WhenDataIsAvailable()
        {
            // Arrange
            var covidDetails = _covidDetailsRepository.GetCovidDetails();

            //Assert
            Assert.AreEqual(230, covidDetails.Count);
            

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
            List<CovidDetails> covidDetails = new List<CovidDetails>();
            //ACT
            var covidDetail = _covidDetailsRepository.GetCovidDetail("USA");
            if(covidDetail != null)
            covidDetails.Add(covidDetail);
            
            //Assert

            Assert.AreEqual(1, covidDetails.Count); 


           
        
        }



    }

}