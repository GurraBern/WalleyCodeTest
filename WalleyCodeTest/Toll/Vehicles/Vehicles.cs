namespace WalleyCodeTest.Vehicles;

public enum VehicleType
{
    Motorbike = 0,
    Tractor = 1,
    Emergency = 2,
    Diplomat = 3,
    Foreign = 4,
    Military = 5,
    Car = 6
}

public class Motorbike : IVehicle
{
    public VehicleType VehicleType => VehicleType.Motorbike;
    public bool IsTollFree() => true;
}

public class Car : IVehicle
{
    public VehicleType VehicleType { get; } = VehicleType.Car;
    
    public bool IsTollFree()
    {
        return VehicleType switch
        {
            VehicleType.Diplomat => true,
            VehicleType.Foreign => true,
            _ => false
        };
    }

    public Car() {}

    public Car(VehicleType vehicleType)
    {
        VehicleType = vehicleType;
    }
}

public class Tractor : IVehicle
{
    public VehicleType VehicleType => VehicleType.Tractor;
    public bool IsTollFree() => true;
}

public class Ambulance : IVehicle
{
    public VehicleType VehicleType => VehicleType.Emergency;
    public bool IsTollFree() => true;
}

public class Tank : IVehicle
{
    public VehicleType VehicleType => VehicleType.Military;
    public bool IsTollFree() => true;
}
