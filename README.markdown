SimpleIOC
=========

A simple IOC container. 
Support for For<>.Use<> syntax.
If a class has a constructor with a value object and no value is given, then the default value for the type will be used.

Examples:
-----------------

Setup container

	var c = new Container();
	c.For<IVehicle>().Use<Motorcycle>();

If more flexibility needed, then use lamdas

	c.For<IVehicle>().Use<Bicycle>(() => new Bicycle(10));

To resolve

	var vehicle = c.Get<IVehicle>();

If a class (B) has constructor with a dependency on another class (B): The B class is injected if found in container.
Example:
	
	public class Car : IVehicle
	{
		public Car(IEngine engine) { /* ... */ } 
	}

	var c = new Container();
	c.For<IVehicle>().Use<Car>();
	c.For<IEngine>().Use<DieselEngine>();

	var vehicle = c.Get<IVehicle>();
