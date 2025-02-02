using System;

namespace WalleyCodeTest.Vehicles;

public static class VehicleFactory
{
    // Comment: detta blev snyggare för testerna så jag valde att göra det på detta sätt. I framtiden händer det säkert att motorcycklar
    // kan vara Diplomat t.ex, då kanske det är snyggare som nedan att när man skapar upp en bil specifik(eller annat fordon) att man där
    // specifierar vad det är för typ, sedan innehåller även fordonet logiken för om den ska betala skatt.
    public static IVehicle Create(VehicleType vehicleType)
    {
        return vehicleType switch
        {
            VehicleType.Motorbike => new Motorbike(),
            VehicleType.Car => new Car(),
            VehicleType.Diplomat => new Car(vehicleType),
            VehicleType.Foreign => new Car(vehicleType),
            VehicleType.Tractor => new Tractor(),
            VehicleType.Emergency => new Ambulance(),
            VehicleType.Military => new Tank(),
            _ => throw new ArgumentException($"Unsupported vehicle type: {vehicleType}")
        };
    }

    public static IVehicle CreateCar(VehicleType vehicleType)
    {
        return new Car(vehicleType);
    }
}