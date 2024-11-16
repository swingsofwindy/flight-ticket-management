using NUnit.Framework;
using System.Windows;
using BLL;
using DAL;
using DTO;
using GUI;
using Microsoft.

namespace NUnit_Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //ARRANGE

            var user = new ACCOUNT
            {
                UserName = "",
                Phone="",
                PermissonID=2
            };

            var addMember = new SignUp1();
            string message;

            //ACT
            var result=addMember.AddMember(user, out message);

            //ASSERT
            
            Assert.Pass();
        }
        [Test]
        public void SignUpUser_ReturnsError_WhenPhoneHasSpecialCharacter()
        {
            // Arrange
            var user = new ACCOUNT
            {
                UserName = "Valid Name",
                Phone = "1234#5678",
                PermissonID = 2
            };
            var addMember = new SignUp1();
            string message;

            // Act
            var result = addMember.AddMember(user, out message);

            // Assert
            Assert.AreEqual("User Phone has special character", result);
        }

        [Test]
        public void SignUpUser_ReturnsSuccess_WhenValidData()
        {
            // Arrange
            var user = new ACCOUNT
            {
                UserName = "Valid Name",
                Phone = "123456789",
                PermissonID = 2
            };
            var addMember = new SignUp1();
            string message;

            // Act
            var result = addMember.AddMember(user, out message);

            // Assert
            Assert.AreEqual("Sign Up Success", result);
        }
    }
}