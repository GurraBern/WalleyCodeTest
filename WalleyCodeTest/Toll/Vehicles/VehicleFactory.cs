namespace WalleyCodeTest.Vehicles;

public static class VehicleFactory
{
    public static IVehicle Create(VehicleType vehicleType)
    {
        return vehicleType switch
        {
            VehicleType.Motorbike => new Motorbike(),
            VehicleType.Car => new Car(),
            // VehicleType.Tractor => expr,
            // VehicleType.Emergency => expr,
            // VehicleType.Diplomat => expr,
            // VehicleType.Foreign => expr,
            // VehicleType.Military => expr,
            _ => throw new ArgumentException($"Unsupported vehicle type: {vehicleType}")
        };
    }
}