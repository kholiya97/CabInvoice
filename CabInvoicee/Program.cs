using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabInvoicee
{
       public class CabInvoiceException : Exception // extended used concept of inhertence by extending exception class
        {
            // Enum for defining different type of custom exception     
            public ExceptionType type;

            // Initializes a new instance of the class.

            public CabInvoiceException(ExceptionType type, string message) : base(message) // i am calling baseclass constructor and giving appropriate message
            {
                this.type = type;
            }
            public enum ExceptionType // created a method with return type enum.
            {
                INVALID_DISTANCE, INVALID_TIME, NULL_RIDES, INVALID_USER_ID
            }
        }
        public class InvoiceGenerator
        {

            //Create Variables 
            private RideType rideRepository; // created a variable with datatype as ridetype
                                             //Create Constants
            private readonly double MINIMUM_COST_PER_KM;
            private readonly int COST_PER_TIME;
            private readonly double MINIMUM_FARE;

            //Initializes a new instance of the class.
            //Creating Method
            public InvoiceGenerator()
            {
                this.rideRepository = new RideType(); // initiliziling all the variables using constructor 
                this.MINIMUM_COST_PER_KM = 10;
                this.COST_PER_TIME = 1;
                this.MINIMUM_FARE = 5;
            }

            // Calculates the fare.     
            // Create Method
            // Invalid Time
            public double CalculateFare(double distance, int time)
            {
                double totalFare = 0;
                try // exception handling to calculate total fare
                {
                    totalFare = distance * MINIMUM_COST_PER_KM + time * COST_PER_TIME;
                }
                catch (CabInvoiceException) // it is user defined exception which is use to catch the exception
                {
                    if (distance <= 0)
                    {
                        throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_DISTANCE, "Invalid Distance");
                    }
                    if (time <= 0)
                    {
                        throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_TIME, "Invalid Time");
                    }
                }
                return Math.Max(totalFare, MINIMUM_FARE); // returnig maximum between totalfare and min. fare..
            }
        }
        public class Ride
        {
            //Creating Variables
            public double distance;
            public int time;

            //Initializes a new instance of the Ride class.
            //getting values of distance and times
            public Ride(double distance, int time)
            {
                this.distance = distance;
                this.time = time;
            }
        }
        public class RideType
        {
            //Dictionary to store UserId and Rides int List. 
            Dictionary<string, List<Ride>> userRides = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="RideRepository"/> class.
            /// </summary>
            public RideType() // creating a instance of ridetype assigning values to dictionary userrides with key as string and value as list of distance and time.
            {
                this.userRides = new Dictionary<string, List<Ride>>();
            }

            /// <summary>
            /// Adds ride of a customer in list and then to a dictionary with user id as a key.
            /// </summary>
            /// <param name="userId">The user identifier.</param>
            /// <param name="rides">The rides array with datatype ride</param>
            /// <exception cref="CabInvoiceDay23.CabInvoiceException">Rides are null</exception>
            public void AddRide(string userId, Ride[] rides) // created method to add rides in a list
            {
                bool rideList = this.userRides.ContainsKey(userId);// created a boolean variable ridelist to check if the user id is present in list.
                try
                {
                    if (!rideList) // if it in not present in list then create a new list and add in the dictonary 
                    {
                        List<Ride> list = new List<Ride>();
                        list.AddRange(rides); // adding rides in list 
                        this.userRides.Add(userId, list); // adding user id and list in dictonary
                    }
                }
                catch
                {
                    throw new CabInvoiceException(CabInvoiceException.ExceptionType.NULL_RIDES, "Rides are null"); // if there are no rides then throw this exception
                }
            }

            /// <summary>
            /// Gets the rides for specific user id.
            /// </summary>
            /// <param name="userId">The user identifier.</param>
            /// <returns> retruns the array of all the rides of user in form of array</returns>
            /// <exception cref="CabInvoiceDay23.CabInvoiceException">Invalid User id</exception>
            public Ride[] GetRides(string userId) // created method for getrides
            {
                try
                {
                    return this.userRides[userId].ToArray(); // returning all the rides with specified user id
                }
                catch (CabInvoiceException)
                {
                    throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_USER_ID, "Invalid User id"); // if not throw error 
                }
            }
        }
        public class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("**********Welcome to Cab Invoice Program***********");
                //Creating Object
                InvoiceGenerator invoiceGenerator = new InvoiceGenerator();
                //Calculate Fare double
                double fare = invoiceGenerator.CalculateFare(2.0, 5);
                Console.WriteLine($"Fare: {fare}");
                Console.ReadLine();

            }
        }
    
}


