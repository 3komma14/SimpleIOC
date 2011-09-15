using System;
using NUnit.Framework;
using SimpleIOC;
using UnitTests.Testable;


namespace UnitTests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Get_WhenInstanceIsConfigured_ReturnsCorrectInstance()
        {
            // Arrange
            var c = new Container();
            c.For<IVehicle>().Use<Motorcycle>();

            // Act
            var vehicle = c.Get<IVehicle>();

            // Assert
            Assert.IsInstanceOf<Motorcycle>(vehicle);
        }

        [Test]
        public void Get_WhenInstanceIsNotConfigured_Throws()
        {
            // Arrange
            var c = new Container();

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => c.Get<IVehicle>());
        }

        [Test]
        public void Get_WhenInstanceIsConfiguredWithLamdas_ReturnsCorrectInstance()
        {
            // Arrange
            var c = new Container();
            c.For<IVehicle>().Use<Bicycle>(() => new Bicycle(10));

            // Act
            var vehicle = c.Get<IVehicle>();

            // Assert
            Assert.IsInstanceOf<Bicycle>(vehicle);
            Assert.AreEqual(10, ((Bicycle)vehicle).NumberOfGears);
        }

        [Test]
        public void Get_WhenInstanceCtorHasNonConfiguresValueType_ReturnsCorrectInstance()
        {
            // Arrange
            var c = new Container();
            c.For<IVehicle>().Use<Bicycle>();

            // Act
            var vehicle = c.Get<IVehicle>();

            // Assert
            Assert.IsInstanceOf<Bicycle>(vehicle);
            Assert.AreEqual(0, ((Bicycle)vehicle).NumberOfGears);
        }

        [Test]
        public void Get_WhenInstanceHasDependency_ReturnsCorrectInstance()
        {
            // Arrange
            var c = new Container();
            c.For<IVehicle>().Use<Car>();
            c.For<IEngine>().Use<DieselEngine>();

            // Act
            var vehicle = c.Get<IVehicle>();

            // Assert
            Assert.IsInstanceOf<Car>(vehicle);
            Assert.IsInstanceOf<DieselEngine>(((Car)vehicle).Engine);
        }

        [Test]
        public void Get_WhenInstanceDependencyNotConfigures_Throws()
        {
            // Arrange
            var c = new Container();
            c.For<IVehicle>().Use<Car>();

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => c.Get<IVehicle>());
        }

        [Test]
        public void Get_WhenInstanceHasDependencyAndUsingLambdas_ReturnsCorrectInstance()
        {
            // Arrange
            var c = new Container();
            c.For<IVehicle>().Use<Car>(ctx => new Car(ctx.Get<IEngine>()));
            c.For<IEngine>().Use<DieselEngine>();

            // Act
            var vehicle = c.Get<IVehicle>();

            // Assert
            Assert.IsInstanceOf<Car>(vehicle);
            Assert.IsInstanceOf<DieselEngine>(((Car)vehicle).Engine);
        }

        [Test]
        public void Get_WhenInstanceHasMultipleCtors_Throws()
        {
            // Arrange
            var c = new Container();

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => c.For<IVehicle>().Use<Train>());
        }

        [Test]
        public void Get_WhenInstanceIsClassWithDependency_ReturnsClass()
        {
            // Arrange
            var c = new Container();
            c.For<IEngine>().Use<DieselEngine>();

            // Act
            var vehicle = c.Get<Car>();

            // Assert
            Assert.IsNotNull(vehicle);
            Assert.IsNotNull(vehicle.Engine);
        }


        [Test]
        public void Get_WhenInstanceIsExplicitSetAndResolvedSeveralTimes_ReturnsSameInstance()
        {
            // Arrange
            var mc = new Motorcycle();
            var c = new Container();
            c.For<IVehicle>().Use(mc);

            // Act
            var vehicle1 = c.Get<IVehicle>();
            var vehicle2 = c.Get<IVehicle>();

            // Assert
            Assert.IsNotNull(vehicle1);
            Assert.IsNotNull(vehicle2);
            Assert.AreSame(vehicle1, vehicle2);
        }

    }
}

