namespace WalleyCodeTest.Vehicles;

public static class VehicleFactory
{
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