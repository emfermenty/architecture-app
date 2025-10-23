﻿using System.Text.Json.Serialization;
using architectureProject.Models.enums;
using architectureProject.Models.Observer.Interfaces;
using architectureProject.Models.ShippingImplimitation;

namespace architectureProject.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(AirShipping), "AirShipping")]
[JsonDerivedType(typeof(TrainShipping), "TrainShipping")]
[JsonDerivedType(typeof(SeaShipping), "SeaShipping")]
[JsonDerivedType(typeof(TruckShipping), "TruckShipping")]
public abstract class Shipping : IShippingObservable
{
    protected readonly IShippingImplementation implementation;
    private readonly List<IShippingObserver> _observers = new();
    private string _status = ShippingStatus.Created.ToString();
    public string Status 
    { 
        get => _status;
        set
        {
            if (_status != value)
            {
                var oldStatus = _status;
                _status = value;
                NotifyStatusChanged(oldStatus, value).Wait();
            }
        }
    }

    public DateTime? StartShipping { get; private set; } = DateTime.Now;
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = null!;
    public double Distance { get; set; }
    public double Weight { get; set; }
    public double Volume { get; set; }
    public ShippingType ShippingType { get; set; }
    public double Cost { get; set; }
    public TimeSpan Duration { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public string TypeDescription { get; set; } = null!;
    
    public virtual double CalculateCost()
    {
        return implementation.CalculateCost(Distance, Weight, Volume);
    }

    public virtual TimeSpan CalculateDuration()
    {
        return implementation.CalculateDuration(Distance);
    }
    public void Attach(IShippingObserver observer)
    {
        _observers.Add(observer);
    }
    public void Detach(IShippingObserver observer)
    {
        _observers.Remove(observer);
    }
    public async Task NotifyStatusChanged(string oldStatus, string newStatus)
    {
        StartShipping = DateTime.UtcNow;

        var tasks = _observers.Select(observer => 
            observer.OnShippingStatusChanged(this, oldStatus, newStatus));
        await Task.WhenAll(tasks);
    }
    public async Task NotifyCreated()
    {
        var tasks = _observers.Select(observer => 
            observer.OnShippingCreated(this));
        await Task.WhenAll(tasks);
    }
    public async Task NotifyCompleted()
    {
        var tasks = _observers.Select(observer => 
            observer.OnShippingCompleted(this));
        await Task.WhenAll(tasks);
    }
    public async Task MarkAsInTransit()
    {
        Status = ShippingStatus.InTransit.ToString();
    }

    public async Task MarkAsDelivered()
    {
        Status = ShippingStatus.Delivered.ToString();
    }
    
    public async Task MarkAsCancelled()
    {
        Status = ShippingStatus.Cancelled.ToString();
    }
    protected Shipping(IShippingImplementation implementation)
    {
        this.implementation = implementation;
        Id = Guid.NewGuid();
    }
}