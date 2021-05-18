using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabInvoicee
{
    /// <summary>
    /// Custom exception for cab invoice program
    /// </summary>
    public class CabInvoiceException : Exception
    {
        // Enum for defining different type of custom exception       
        public ExceptionType type;

        // Initializes a new instance of the class.

        public CabInvoiceException(ExceptionType type, string message) : base(message)
        {
            this.type = type;
        }
        public enum ExceptionType
        {
            INVALID_DISTANCE, INVALID_TIME, NULL_RIDES, INVALID_USER_ID, INVALID_RIDETYP
        }
    }
    public class InvoiceGenerator
    {

        //Create Variables 
        private RideRepository rideRepository;
        RideType rideType;
        //Create Constants
        private readonly double MINIMUM_COST_PER_KM;
        private readonly int COST_PER_TIME;
        private readonly double MINIMUM_FARE;

        //Initializes a new instance of the class.
        //Creating Method
        /// Initializes a new instance of the <see cref="InvoiceGenerator"/> class.
        /// </summary>
        public InvoiceGenerator(RideType rideType)
        {
            this.rideRepository = new RideRepository();
            this.rideType = rideType;
            try
            {
                if (this.rideType.Equals(RideType.NORMAL))
                {
                    this.MINIMUM_COST_PER_KM = 10;
                    this.COST_PER_TIME = 1;
                    this.MINIMUM_FARE = 5;
                }
                if (this.rideType.Equals(RideType.PREMIUM))
                {
                    this.MINIMUM_COST_PER_KM = 15;
                    this.COST_PER_TIME = 2;
                    this.MINIMUM_FARE = 20;
                }
            }
            catch (CabInvoiceException)
            {
                throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_RIDETYP, "invalid ride type");
            }


        }

        // Calculates the fare.     
        // Invalid Time
        //Create Parameterised Constructor passes value distance and time
        public double CalculateFare(double distance, int time)
        {
            double totalFare = 0;
            try
            {
                totalFare = distance * MINIMUM_COST_PER_KM + time * COST_PER_TIME;
            }
            catch (CabInvoiceException)
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
            return Math.Max(totalFare, MINIMUM_FARE);
        }

        // Calculates the fare for array of rides
        // for checking total fare
        // Adding Method 
        public InvoiceSummary CalculateFare(Ride[] rides)
        {
            double totalFare = 0;
            // checks for rides available and passes them to calculate fare method to calculate fare for each method
            try
            {
                //calculating total fare for all rides
                foreach (Ride ride in rides)
                {
                    totalFare += this.CalculateFare(ride.distance, ride.time);
                }
            }
            //catches exception if available
            catch (CabInvoiceException)
            {
                //If no rides there then throw exception
                if (rides == null)
                {
                    throw new CabInvoiceException(CabInvoiceException.ExceptionType.NULL_RIDES, "no rides found");
                }
            }
            //returns invoice summary object 
            return new InvoiceSummary(rides.Length, totalFare);
        }

        // Adds the rides in dictionary with key as a user id 
        //Adding Method
        public void AddRides(string userId, Ride[] rides)
        {
            try
            {
                rideRepository.AddRide(userId, rides);
            }
            catch (CabInvoiceException)
            {
                if (rides == null)
                {
                    throw new CabInvoiceException(CabInvoiceException.ExceptionType.NULL_RIDES, "Null rides");
                }
            }
        }
        //Gets the invoice summary by passing user id into ride repository and then passing rides array to calculate fares.

        public InvoiceSummary GetInvoiceSummary(string userId)
        {
            try
            {
                return this.CalculateFare(rideRepository.GetRides(userId));
            }
            catch
            {
                throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_USER_ID, "Invalid user id");
            }
        }
    }
    /// <summary>
    /// Generate Invoice summery
    /// Invoice summary generates number of rides And  total fare
    /// And Also generates average fare for summary
    /// Create constructor and pass 3 variables of private type
    /// </summary>
    public class InvoiceSummary
    {
        private int numberOfRides;
        private double totalFare;
        private double averageFare;


        // Initializes a new instance of the class.
        // initializes number of rides, total fare and generates average fare for rides.
        //Creating Parameterised constructor for numberofrides and totalfare
        //getting value for that element
        public InvoiceSummary(int numberOfRides, double totalFare)
        {
            this.numberOfRides = numberOfRides;
            this.totalFare = totalFare;
            this.averageFare = totalFare / numberOfRides;
        }

        // Determines whether the specified ,is equal to this instance.
        //to compare with this instance.</param>
        // <returns>
        //  value isntrue if the specified  is equal to this instance; otherwise, <c>false</c>.

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is InvoiceSummary))
            {
                return false;
            }
            InvoiceSummary imputedObject = (InvoiceSummary)obj;
            return this.numberOfRides == imputedObject.numberOfRides && this.totalFare == imputedObject.totalFare && this.averageFare == imputedObject.averageFare;
        }
    }
    /// <summary>
    /// Ride class to set data for particular ride. 
    /// </summary>
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
    public class RideRepository
    {
        //Dictionary to store UserId and Rides int List.
        Dictionary<string, List<Ride>> userRides = null;
        /// Initializes a new instance of the  class.
       // for getting element
        public RideRepository()
        {
            this.userRides = new Dictionary<string, List<Ride>>();
        }

        //Adding Method AddRide
        // Adding ride of a customer in list and then to a dictionary with user id as a key.
        public void AddRide(string userId, Ride[] rides)
        {
            //Boolean variable for ridelist
            bool rideList = this.userRides.ContainsKey(userId);
            try
            {
                //If there is not ridelist 
                if (!rideList)
                {
                    //Creating list of having name list 
                    //creating list of class ride
                    List<Ride> list = new List<Ride>();
                    //Adds collection of element in list
                    list.AddRange(rides);
                    //getting D
                    this.userRides.Add(userId, list);
                }
            }
            catch
            {
                throw new CabInvoiceException(CabInvoiceException.ExceptionType.NULL_RIDES, "Rides are null");
            }
        }

        // Gets the rides for specific user id.     
        //The user identifier userID
        //retruns the array of all the rides of user in form of array
        public Ride[] GetRides(string userId)
        {
            try
            {
                return this.userRides[userId].ToArray();
            }
            catch (CabInvoiceException)
            {
                throw new CabInvoiceException(CabInvoiceException.ExceptionType.INVALID_USER_ID, "Invalid User id");
            }
        }
    }
    /// <summary>
    /// creating enum to choose between two ride types- normal and premium
    /// </summary>
    public enum RideType
    {
        NORMAL, PREMIUM
    }
    public class Program
    {/// <summary>
     /// Defines Entry point of application
     /// </summary>
     /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("**********Welcome to Cab Invoice Program***********");
            //Creating Object
            InvoiceGenerator invoiceGenerator = new InvoiceGenerator(RideType.NORMAL);
            //Calculate Fare double
            double fare = invoiceGenerator.CalculateFare(2.0, 5);
            Console.WriteLine($"Fare: {fare}");

            Console.ReadLine();

        }
    }
}

