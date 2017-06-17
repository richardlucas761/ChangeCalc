using System;
using ChangeCalc.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChangeCalc.Tests.Controllers
{
    [TestClass]
    public class CalcControllerTest
    {
        [TestMethod]
        public void GetTwentyPoundsValueIsFivePoundsFifty()
        {
            // Arrange
            var controller = new CalcController();

            const decimal tendered = 20M;
            const decimal itemValue = 5.50M;

            const string expectedResult = "Your change is: 1 x £10, 2 x £2, 1 x 50p";

            // Act
            var result = controller.Get(tendered, itemValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetTwentyPoundsValueIsTenPounds()
        {
            // Arrange
            var controller = new CalcController();

            const decimal tendered = 20M;
            const decimal itemValue = 10M;

            const string expectedResult = "Your change is: 1 x £10";

            // Act
            var result = controller.Get(tendered, itemValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetOneHundredPoundsValueIsFiftyPounds()
        {
            // Arrange
            var controller = new CalcController();

            const decimal tendered = 100M;
            const decimal itemValue = 50M;

            const string expectedResult = "Your change is: 1 x £50";

            // Act
            var result = controller.Get(tendered, itemValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetOneHundredPoundsValueIsOnePenny()
        {
            // Arrange
            var controller = new CalcController();

            const decimal tendered = 100M;
            const decimal itemValue = 0.01M;

            const string expectedResult = "Your change is: 1 x £50, 2 x £20, 1 x £5, 2 x £2, 1 x 50p, 2 x 20p, 1 x 5p, 2 x 2p";

            // Act
            var result = controller.Get(tendered, itemValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetNoChange()
        {
            // Arrange
            var controller = new CalcController();

            const decimal tendered = 30M;
            const decimal itemValue = 30M;

            // Act
            var result = controller.Get(tendered, itemValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("No change needed", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Tendered value is Invalid")]
        public void GetInvalidTendered()
        {
            // Arrange
            var controller = new CalcController();

            const decimal tendered = -1M;
            const decimal itemValue = 30M;

            // Act
            controller.Get(tendered, itemValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Item value is Invalid")]
        public void GetInvalidItemValue()
        {
            // Arrange
            var controller = new CalcController();

            const decimal tendered = 100M;
            const decimal itemValue = -50M;

            // Act
            controller.Get(tendered, itemValue);
        }

        // TODO is this a valid test, we know the invalid item value exception will be thrown first?
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Item value is Invalid")]
        public void GetInvalidItemValueAndInvalidTendered()
        {
            // Arrange
            var controller = new CalcController();

            const decimal tendered = 100M;
            const decimal itemValue = 0M;

            // Act
            controller.Get(tendered, itemValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Tendered value is less than Item value")]
        public void GetInvalidItemValueGreaterThanAmountTendered()
        {
            // Arrange
            var controller = new CalcController();

            const decimal tendered = 150M;
            const decimal itemValue = 250M;

            // Act
            controller.Get(tendered, itemValue);
        }
    }
}
