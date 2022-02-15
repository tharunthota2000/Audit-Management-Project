using AuthorizationWebAPI.Controllers;
using AuthorizationWebAPI.Models;
using AuthorizationWebAPI.Repository;
using AuthorizationWebAPI.Services;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace AuthorizationTestProject
{
    public class Tests
    {
        Mock<ICredentialRepo> mockRepo;
        Mock<IAuthService> mockServ;
        Mock<IConfiguration> config;
        Mock<AuthorizeDBContext> mockContext;
        [SetUp]
        public void Setup()
        {
            mockContext = new Mock<AuthorizeDBContext>();
            mockRepo = new Mock<ICredentialRepo>();
            mockServ = new Mock<IAuthService>();
            config = new Mock<IConfiguration>();
        }

        [Test]
        public void GenerateJSONWebToken_ValidMember_ReturnsToken()
        {
            //Arrange
            AuthService serv = new AuthService(mockRepo.Object);
            Authenticate cred = new Authenticate()
            {
                Name = "username",
                Password = "password"
            };
            config.Setup(p => p["Jwt:Key"]).Returns("ThisIsMySecretKey");

            config.Setup(p => p["Jwt:Issuer"]).Returns("https://localhost:44392");
            mockServ.Setup(m => m.GenerateJSONWebToken(cred, config.Object));
            //Act
            var data = serv.GenerateJSONWebToken(cred, config.Object);
            //Assert
            Assert.IsNotNull(data);
            Assert.AreEqual("string".GetType(), data.GetType());
        }

        [Test]
        public void GenerateJSONWebToken_InvalidMember_ThrowsException()
        {
            //Arrange
            AuthService serv = new AuthService(mockRepo.Object);
            Authenticate cred = new Authenticate()
            {
                Name = "username",
                Password = "password"
            };
            config.Setup(p => p["Jwt:Key"]).Returns("ThisIsMySecretKey");
            config.Setup(p => p["Jwr:Issuer"]).Returns("https://localhost:43172");
            mockServ.Setup(m => m.GenerateJSONWebToken(cred, config.Object));
            var output = serv.GenerateJSONWebToken(null, config.Object);
            Assert.AreEqual(null, output);
        }
        [Test]
        public void CheckValidUser()
        {
            AuthService serv = new AuthService(mockRepo.Object);
            mockRepo.Setup(p => p.GetCredentials()).Returns(new Dictionary<string, string>() { { "username", "password" } });
            Authenticate user = new Authenticate()
            {
                Name = "username",
                Password = "password"

            };
            var res = serv.AuthenticateUser(user);
            Assert.Pass();


        }
        [Test]
        public void CheckInValidUser()
        {
            AuthService serv = new AuthService(mockRepo.Object);
            mockRepo.Setup(p => p.GetCredentials()).Returns(new Dictionary<string, string>() { { "name", "pass123" } });
            Authenticate user = new Authenticate()
            {
                Name = "username",
                Password = "password"

            };
            var res = serv.AuthenticateUser(user);
            Assert.Pass();
        }
        [Test]
        public void ControllerTest()
        {
            AuthService serv = new AuthService(mockRepo.Object);
            Authenticate cred = new Authenticate()
            {
                Name = "username",
                Password = "password"
            };
            config.Setup(p => p["Jwt:Key"]).Returns("ThisIsMySecretKey");
            config.Setup(p => p["Jwr:Issuer"]).Returns("https://localhost:43172");
            mockServ.Setup(p => p.AuthenticateUser(cred)).Returns(cred);
            mockServ.Setup(m => m.GenerateJSONWebToken(cred, config.Object));
            Token tok = new Token()
            {
                AuditToken = "hjdfgdsgfjhsd"
            };
            TokenController con = new TokenController(config.Object, mockServ.Object);
            var res = con.Login(cred) as OkObjectResult;
            Assert.AreEqual(tok.GetType(), res.Value.GetType());
        }

    }
}