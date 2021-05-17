using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CabInvoicee;
namespace UnitTestProject1
{
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
        /// <summary>
        /// Givens the multiple rides should return invoice summary.  UC2
        /// </summary>
        [TestMethod]
        public void GivenMultipleRidesShouldReturnInvoiceSummary()
        {
            //Creating instance of invoice generator 
            InvoiceGenerator invoiceGenerator = new InvoiceGenerator();
            Ride[] rides = { new Ride(2.0, 5), new Ride(0.1, 1) };

            //Generating Summary for rides
            InvoiceSummary summary = invoiceGenerator.CalculateFare(rides);
            InvoiceSummary expectedSummary = new InvoiceSummary(2, 30.0);

            //Asserting values
            Assert.AreEqual(expectedSummary, summary);
        }
        /// <summary>
        /// Givens the multiple rides should return invoice summary with average fare.  UC3
        /// </summary>
        [TestMethod]
        public void GivenMultipleRidesShouldReturnInvoiceSummarywithAverageFare()
        {
            //Creating instance of invoice generator 
            InvoiceGenerator invoiceGenerator = new InvoiceGenerator();
            Ride[] rides = { new Ride(2.0, 5), new Ride(0.1, 1) };

            //Generating Summary for rides
            InvoiceSummary summary = invoiceGenerator.CalculateFare(rides);
            InvoiceSummary expectedSummary = new InvoiceSummary(2, 30.0);

            //Asserting values with average in equals to formula in invoice summary
            Assert.AreEqual(expectedSummary, summary);
        }
        /// <summary>
        /// Givens the rides for different users should return invoice summary. UC4
        /// </summary>
        [TestMethod]
        public void GivenRidesForDifferentUsersShouldReturnInvoiceSummary()
        {
            //Creating instance of invoice generator 
            InvoiceGenerator invoiceGenerator = new InvoiceGenerator();
            Ride[] rides = { new Ride(2.0, 5), new Ride(0.1, 1) };
            string userId = "001";
            invoiceGenerator.AddRides(userId, rides);
            string userIdForSecondUser = "002";
            Ride[] ridesForSecondUser = { new Ride(3.0, 10), new Ride(1.0, 2) };
            invoiceGenerator.AddRides(userIdForSecondUser, ridesForSecondUser);
            //Generating Summary for rides
            InvoiceSummary summary = invoiceGenerator.GetInvoiceSummary(userId);
            InvoiceSummary expectedSummary = new InvoiceSummary(2, 30.0);
            //Asserting values with average in equals to formula in invoice summary
            Assert.AreEqual(expectedSummary, summary);
        }
    }
}