using AuditBenchmarkWebAPI.Controllers;
using AuditBenchmarkWebAPI.Models;
using AuditBenchmarkWebAPI.Repository;
using AuditBenchmarkWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace AuditBenchmarkTestProject
{
    public class Tests
    {
        Dictionary<string, int> internalandsoxtestdict = new Dictionary<string, int>();
        Mock<IBenchmarkRepo> mockrepo;
        Mock<BenchmarkContext> con;
        Mock<IBenchmarkService> serv;
        [SetUp]
        public void Setup()
        {
            mockrepo = new Mock<IBenchmarkRepo>();
            con = new Mock<BenchmarkContext>();
            serv = new Mock<IBenchmarkService>();
        }

        [Test]
        public void GetInternalCount()
        {
            mockrepo.Setup(p => p.GetNoCount()).Returns(new Dictionary<string, int>() { { "Internal", 3 } });
            BenchmarkService serv = new BenchmarkService(mockrepo.Object);
            var res = serv.GetInternaAndSOXNoCount();
            Assert.AreEqual(3, res["Internal"]);
        }

        [Test]
        public void GetInternalCountFail()
        {

            mockrepo.Setup(p => p.GetNoCount()).Returns(new Dictionary<string, int>() { { "Internal", 3 } });
            BenchmarkService serv = new BenchmarkService(mockrepo.Object);
            var res = serv.GetInternaAndSOXNoCount();
            Assert.AreEqual(5, res["Internal"]);
        }

        [Test]
        public void GetSOXCount()
        {

            mockrepo.Setup(p => p.GetNoCount()).Returns(new Dictionary<string, int>() { { "SOX", 1 } });
            BenchmarkService serv = new BenchmarkService(mockrepo.Object);
            var res = serv.GetInternaAndSOXNoCount();
            Assert.AreEqual(1, res["SOX"]);
        }

        [Test]
        public void GetSOXCountFail()
        {

            mockrepo.Setup(p => p.GetNoCount()).Returns(new Dictionary<string, int>() { { "SOX", 1 } });
            BenchmarkService serv = new BenchmarkService(mockrepo.Object);
            var res = serv.GetInternaAndSOXNoCount();
            Assert.AreEqual(3, res["SOX"]);
        }
        [Test]
        public void NullRepoTest()
        {

            mockrepo.Setup(p => p.GetNoCount());
            BenchmarkService serv = new BenchmarkService(mockrepo.Object);
            var res = serv.GetInternaAndSOXNoCount();
            Assert.IsNull(res);
        }

        [Test]
        public void ControllerPassTest()
        {
            serv.Setup(p => p.GetInternaAndSOXNoCount()).Returns(new Dictionary<string, int>() { { "SOX", 1 } });
            AuditBenchmarkController con = new AuditBenchmarkController(serv.Object);
            var res = con.Get() as OkObjectResult;
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void ControllerBadRequestTest()
        {
            serv.Setup(p => p.GetInternaAndSOXNoCount()).Returns(new Dictionary<string, int>());
            AuditBenchmarkController con = new AuditBenchmarkController(serv.Object);
            var res = con.Get() as OkObjectResult;
            Assert.AreEqual(200, res.StatusCode);
        }



    }
}



