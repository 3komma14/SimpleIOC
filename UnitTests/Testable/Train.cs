using System;

namespace UnitTests.Testable
{
    public class Train : IVehicle
    {
        public IEngine Engine { get; set; }

        public Train()
        {
            
        }

        public Train(IEngine engine)
        {
            Engine = engine;
        }

    }
}