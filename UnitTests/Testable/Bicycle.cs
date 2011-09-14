using System;

namespace UnitTests.Testable
{
    public class Bicycle : IVehicle
    {
        public int NumberOfGears { get; set; }

        public Bicycle(int numberOfGears)
        {
            NumberOfGears = numberOfGears;
        }
    }
}