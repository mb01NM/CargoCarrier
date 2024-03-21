namespace CargoCarrier.Mvc.Services;

public class TruckLoadCalculator
{
    private const int LargeTruckCapacity = 12; // in terms of small parcels
    private const int SmallTruckCapacity = 4; // in terms of small parcels

    // Large trucks can carry 3 large parcels or 6 medium parcels or 12 small parcels
    // Small trucks can carry 1 large parcel or 2 medium parcels or 4 small parcels
    public (int smallTrucks, int largeTrucks) CalculateTruckLoads(int smallParcels, int mediumParcels, int largeParcels)
    {
        int totalSmallParcelEquivalent = smallParcels + (mediumParcels * 2) + (largeParcels * 4);
        int largeTrucks = totalSmallParcelEquivalent / LargeTruckCapacity;
        int remainingParcels = totalSmallParcelEquivalent % LargeTruckCapacity;
        int smallTrucks = (remainingParcels + SmallTruckCapacity - 1) / SmallTruckCapacity; // round up

        return (smallTrucks, largeTrucks);
    }
}