using AuditChecklistWebAPI.Controllers;
using AuditChecklistWebAPI.Models;
using AuditChecklistWebAPI.Repositeries;
using AuditChecklistWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace AuditChecklistTestProject
{
    public class Tests
    {
        Mock<IAuditChecklistRepo> moqRepo;
        Mock<IAuditChecklistService> moqService;


        [SetUp]
        public void Setup()
        {

            moqRepo = new Mock<IAuditChecklistRepo>();
            moqService = new Mock<IAuditChecklistService>();

        }

        List<AuditTypesAndQuestions> soxlist = new List<AuditTypesAndQuestions>()
        {
            new AuditTypesAndQuestions() { Questions = "1. Have all Change requests followed SDLC before PROD move?", AuditType = "SOX" },
            new AuditTypesAndQuestions() { Questions = "2. Have all Change requests been approved by the application owner?", AuditType = "SOX" },
            new AuditTypesAndQuestions() { Questions = "3. For a major change, was there a database backup taken before and after PROD move?", AuditType = "SOX" },
            new AuditTypesAndQuestions() { Questions = "4. Has the application owner approval obtained while adding a user to the system?", AuditType = "SOX" },
            new AuditTypesAndQuestions() { Questions = "5. Is data deletion from the system done with application owner approval?",AuditType="SOX"}
        };
        List<AuditTypesAndQuestions> intlist = new List<AuditTypesAndQuestions>()
        {
            new AuditTypesAndQuestions(){ Questions = "1. Have all Change requests followed SDLC before PROD move?" , AuditType = "Internal"},
            new AuditTypesAndQuestions() { Questions = "2. Have all Change requests been approved by the application owner?", AuditType = "Internal" },
            new AuditTypesAndQuestions() { Questions = "3. Are all artifacts like CR document, Unit test cases available?", AuditType = "Internal" },
            new AuditTypesAndQuestions() { Questions = "4. Has the application owner approval obtained while adding a user to the system?", AuditType = "Internal" },
           new AuditTypesAndQuestions() { Questions = "5. Is data deletion from the system done with application owner approval?", AuditType = "Internal" },
        };

        [Test]
        public void GetAllChecklistQuestionsListInternalTest()
        {

            moqService.Setup(p => p.QuestionsProvider("Internal")).Returns(intlist);
            AuditChecklistController con = new AuditChecklistController(moqService.Object);
            var res = con.GetQuestions("Internal") as OkObjectResult;
            Assert.AreEqual(200, res.StatusCode);
        }
        [Test]
        public void GetAllChecklistQuestionsListSOXTest()
        {

            AuditChecklistController con = new AuditChecklistController(moqService.Object);
            moqService.Setup(p => p.QuestionsProvider("SOX")).Returns(soxlist);
            var res = con.GetQuestions("SOX") as OkObjectResult;
            Assert.AreEqual(200, res.StatusCode);
        }
        [Test]
        public void WrongInputTest()
        {

            AuditChecklistController con = new AuditChecklistController(moqService.Object);
            var res = con.GetQuestions("SOX") as BadRequestObjectResult;
            Assert.AreEqual("Wrong Input", res.Value);
        }
        [Test]
        public void NoInputTest()
        {

            AuditChecklistController con = new AuditChecklistController(moqService.Object);
            var res = con.GetQuestions(null) as BadRequestObjectResult;
            Assert.AreEqual("No Input", res.Value);
        }
        [Test]
        public void InternalServiceTest()
        {
            moqRepo.Setup(p => p.AuditChecklistQuestions("Internal")).Returns(intlist);
            AuditChecklistService serv = new AuditChecklistService(moqRepo.Object);
            var res = serv.QuestionsProvider("Internal");
            Assert.AreEqual(5, res.Count);
        }
        [Test]
        public void SOXServiceTest()
        {
            moqRepo.Setup(p => p.AuditChecklistQuestions("SOX")).Returns(soxlist);
            AuditChecklistService serv = new AuditChecklistService(moqRepo.Object);
            var res = serv.QuestionsProvider("SOX");
            Assert.AreEqual(5, res.Count);
        }
        [Test]
        public void ServiceNullTest()
        {

            AuditChecklistService serv = new AuditChecklistService(moqRepo.Object);
            var res = serv.QuestionsProvider("Internal");
            Assert.IsNull(res);

        }
        [Test]
        public void TypeNullTest()
        {

            AuditChecklistService serv = new AuditChecklistService(moqRepo.Object);
            var res = serv.QuestionsProvider(null);
            Assert.IsNull(res);

        }
        [Test]
        public void WrongTypeTest()
        {

            AuditChecklistService serv = new AuditChecklistService(moqRepo.Object);
            var res = serv.QuestionsProvider("External");
            Assert.IsNull(res);

        }
    }
}
