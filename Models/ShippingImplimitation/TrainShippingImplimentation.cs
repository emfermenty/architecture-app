﻿namespace architectureProject.Models.ShippingImplimitation;

public class TrainShippingImplimentation : IShippingImplementation
{
    public double CalculateCost(double Distance, double weight, double Volume)
    {
        double baseCost = Distance * 0.05; 
        return baseCost;
    }
    public TimeSpan CalculateDuration(double Distance)
    {
        double hours = (Distance / 60);
        return TimeSpan.FromHours(hours);
    }
    public double GetMaxWeight() => 50000;
    public double GetMaxVolume() => 5000;
}