using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqDemo.Tests
{
    [TestFixture]
    public class WorkingClassTest
    {
        public MockRepository MockRepository { get; set; }

        [SetUp]
        public void Setup()
        {
            MockRepository = new MockRepository(MockBehavior.Strict) { DefaultValue = DefaultValue.Mock };
        }

        [TearDown]
        public void TearDown()
        {
            MockRepository.VerifyAll();
        }

        [Test]
        public void NotifyNiceUsersTest_WithoutMocking()
        {
            var service = new MoqYouService();
            var worker = new WorkingClass(service);
            worker.NotifyNiceUsers("user@abc.com", UserType.Nice);
            //don't want to actually send an email plus how do we 
            //test if statement
        }

        [Test]
        public void NotifyNiceUsersTest_NiceTest_Success()
        {
            var userEmail = "user@abc.com";
            var mockService = new Mock<IMoqYouService>();
            mockService.Setup(m => m.SendEmail(userEmail, "test@abc.com", It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var worker = new WorkingClass(mockService.Object);
            worker.NotifyNiceUsers(userEmail, UserType.Nice);

            mockService.VerifyAll();
        }

        [Test]
        public void NotifyNiceUsersTest_NiceTest_Fail_BadFromAddress1()
        {
            //in this test, purposely set expected from address to be incorrect
            //using verify all
            var userEmail = "user@abc.com";
            var mockService = new Mock<IMoqYouService>();
            mockService.Setup(m => m.SendEmail(It.IsAny<string>(), "wrong", It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var worker = new WorkingClass(mockService.Object);
            worker.NotifyNiceUsers(userEmail, UserType.Nice);

            mockService.VerifyAll();
        }

        [Test]
        public void NotifyNiceUsersTest_NiceTest_Fail_BadFromAddress2()
        {
            //in this test, purposely set expected from address to be incorrect
            var userEmail = "user@abc.com";
            var mockService = new Mock<IMoqYouService>();
            mockService.Setup(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var worker = new WorkingClass(mockService.Object);
            worker.NotifyNiceUsers(userEmail, UserType.Nice);

            mockService.Verify(m => m.SendEmail(userEmail, "wrong", It.IsAny<string>(), It.IsAny<string>()), Times.Once, "Invalid SendEmail Call");
        }

        [Test]
        public void NotifyNiceUsersTest_IntelligentTest_Fail_SendEmailNotCalled()
        {
            //in this test, we falsely hoped that Intelligent users would also get an email
            var userEmail = "user@abc.com";
            var mockService = new Mock<IMoqYouService>();
            mockService.Setup(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var worker = new WorkingClass(mockService.Object);
            worker.NotifyNiceUsers(userEmail, UserType.Intelligent);

            mockService.Verify(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once, "SendEmail not called");
        }

        [Test]
        public void NotifyNiceUsersTest_AnnoyingTest_Success_SendEmailNotCalled()
        {
            //in this test, purposely set expected from address to be incorrect
            //using verify all
            var userEmail = "user@abc.com";
            var mockService = new Mock<IMoqYouService>();
            mockService.Setup(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var worker = new WorkingClass(mockService.Object);
            worker.NotifyNiceUsers(userEmail, UserType.Annoying);

            mockService.Verify(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never, "SendEmail should not be not called");
        }

        [Test]
        public void NotifyNiceUsersTest_SendEmailException_Success()
        {
            //in this test, purposely set expected from address to be incorrect
            //using verify all
            var userEmail = "user@abc.com";
            var mockService = new Mock<IMoqYouService>();
            var exceptionThrown = false;
            mockService.Setup(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var worker = new WorkingClass(mockService.Object);
            try
            {
                worker.NotifyNiceUsers(userEmail, UserType.Nice);
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Assert.AreEqual(typeof(ApplicationException), ex.GetType(), "Invalid exception type");
            }

            mockService.Verify(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            if (!exceptionThrown)
            {
                Assert.Fail("Exception should be thrown");
            }
            mockService.VerifyAll();
        }

    }
}
