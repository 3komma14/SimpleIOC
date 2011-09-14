using System;

namespace UnitTests.Testable
{
    public class Car : IVehicle
    {
        public IEngine Engine { get; set; }

        public Car(IEngine engine)
        {
            Engine = engine;
        }
    }
}