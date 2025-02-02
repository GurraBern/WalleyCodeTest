namespace WalleyCodeTest.Vehicles;

public interface IVehicle
{
    VehicleType VehicleType { get; }
    
    //Comment: Jag valde att skapa en metod för fordon så att de själva vet om de är toll fee, dels för att slippa gå in i TollFeeProvider
    // klassen om man lägger till nya fordon men även om logik skulle ändras i framtiden t.ex bilar som väger mindre än 1.5 ton är toll fee
    // annars inte. Se exempel på Car
    bool IsTollFree();
}