using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CabInvoicee;
namespace UnitTestProject1
{
    [TestClass]
    /// <summary>
    /// Adding class to do tests for program
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Test1s this instance.
        /// </summary>
        [TestMethod]
        public void GivenDistanceAndTimeShouldReturnTotalFare()
        {
            //Creting Object of InvoiceGenerator
            InvoiceGenerator invoiceGenerator = new InvoiceGenerator();
            double distance = 2.0;
            int time = 5;

            //For calculating fare
            double fare = invoiceGenerator.CalculateFare(distance, time);
            double expected = 25;
            Assert.AreEqual(expected, fare);
        }
    }
}
