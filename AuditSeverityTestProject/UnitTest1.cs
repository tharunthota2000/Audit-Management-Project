using AuditSeverityWebAPI.Controllers;
using AuditSeverityWebAPI.Models;
using AuditSeverityWebAPI.Repository;
using AuditSeverityWebAPI.Services;
using Moq;
using NUnit.Framework;

namespace AuditSeverityTestProject
{
    public class Tests
    {
        private Mock<ISeverityService> _provider;
        private AuditSeverityController _controller;
        private Mock<ISeverityRepo> _repomock;
        private SeverityService _auditprov;
        [SetUp]
        public void Setup()
        {
            _provider = new Mock<ISeverityService>();
            _controller = new AuditSeverityController(_provider.Object);
            _repomock = new Mock<ISeverityRepo>();
            _auditprov = new SeverityService(_repomock.Object);
        }
        [Test]
        public void ControllerSuccessTest()
        {
            AuditResponse auditResponse = new AuditResponse()
            {
                AuditId = 348568,
                RemedialActionDuration = "Within week",
                ProjectExexutionStatus = "Green"
            };
            _provider.Setup(x => x.SeverityResponse(It.IsAny<AuditRequest>())).Returns((auditResponse));
            AuditRequest request = new AuditRequest()
            {
                Auditdetails = new AuditDetail()
                {
                    AuditType = "Internal",
                    AuditDate = new System.DateTime(2020, 10, 1)
                },
                ApplicationOwnerName = "Alisha",
                ProjectManagerName = "Jayashree",

            };
            var result = _controller.Post(request) as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);

        }
        [Test]
        public void ControllerFailureTest()
        {

            _provider.Setup(x => x.SeverityResponse(It.IsAny<AuditRequest>())).Returns(((AuditResponse)null));
            AuditRequest request = new AuditRequest()
            {
                Auditdetails = new AuditDetail()
                {
                    AuditType = "SOX",
                    AuditDate = new System.DateTime(2020, 10, 1)
                },
                ApplicationOwnerName = "Alisha",
                ProjectManagerName = "Jayashree",

            };
            var result = _controller.Post(request) as ObjectResult;
            Assert.AreEqual(404, result.StatusCode);

        }

   
    }
}