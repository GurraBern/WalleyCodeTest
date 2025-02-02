Under kodtestet försökte jag jobba mer TDD aktigt vilket jag tyckte var väldigt schysst med uppgifter som denna eftersom man hela tiden kan ifrågasätta sin kod och hur man vill att den ska bete sig. Jag tror att pågrund av detta kunde jag hitta en del buggar men också jobba effektivare för man såg att tester började faila när man refaktorisade med något antagande. Jag lämande en del kommentar i koden men också lite om mina tankar nedan, i ett verkligt projekt hade jag inte lämnat kommentrar i kod eftersom de blir utdaterade och misvisande.

Lite av mina tankar:
Jag lade märke till GetTollFee metoden i TollCalculator, det var svårt att förstå vad den gör, mycket if satser som känns onödiga. Så denna vill jag skriva om pågrund av läsbarhet.

IsTollFreeVehicle metoden känns lite dum då varje gång man lägger till ett nytt fordon så kommer man behöva gå in i klassen och lägga till om den ska ha en avgift eller inte. Valde därför att flytta denna logiken till själva fordonet särskilt eftersom det vi bygger är till för trängselskatt för fördon, då ska det vara lätt att utöka. T.ex kanske det kommer special regler som att om det är en bil som är av typen Emergency så ska den inte betala, då vill jag hellre att den logik ligger på fordonet.

I GetTollFee tyckte jag inte att det var TollCalculator's ansvar att veta alla priser, calculator ska innehålla logiken för att räkna ut totala avgiften medan TollFeeProvider ger hur mycket avgiften blir för ett exakt datum. Jag skrev tester tidigt som dessutom hittade en bug när klockan var 9. Jag gjorde en record eftersom jag ville att TollFee skulle vara immutable, den ska inte kunna justeras under programmets gång.

Jag vet inte om det är lite fusk MEN valde att använda en nuget för att hitta högtider eftersom annars behöver man skriva manuellt för varje år vilket inte känns hållbart, jag hade mycket hellre ge bort detta ansvaret till ett nuget paket eller externpart(typ api).

Mindre detaljer:
En mindre detalj som jag störde mig på när jag började var att GetTollFee metoderna tar in en vehicle och datum, mellan båda metoderna så ligger dem inte iordning vilket gör det lite ointuitivt att använda, lätt att man skriver in vehicle i datum och måste redigera snabbt men lite störande. 

Så flyttade runt parametrarna:
    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    public int GetTollFee(DateTime date, Vehicle vehicle)
