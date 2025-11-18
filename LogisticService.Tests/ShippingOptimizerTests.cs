using LogisticService.Application.DTO;
using LogisticService.Application.Services;
using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping;
using LogisticService.Domain.Models.Shipping.Abstract;
using LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;
using Moq;

namespace architectureProject.Tests.Services
{
    public class ShippingOptimizerTests
    {
        private readonly Mock<IShippingFactory> _truckFactoryMock;
        private readonly Mock<IShippingFactory> _airFactoryMock;
        private readonly ShippingRequest _request;

        public ShippingOptimizerTests()
        {
            _truckFactoryMock = new Mock<IShippingFactory>();
            _airFactoryMock = new Mock<IShippingFactory>();

            _truckFactoryMock.Setup(x => x.Type).Returns(ShippingType.Truck);
            _airFactoryMock.Setup(x => x.Type).Returns(ShippingType.Air);

            _request = new ShippingRequest
            {
                Distance = 100,
                Weight = 50,
                Volume = 10
            };
        }

        private Shipping CreateRealShipping(ShippingType type)
        {
            return type switch
            {
                ShippingType.Truck => new TruckShipping(),
                ShippingType.Air => new AirShipping(),
                ShippingType.Sea => new SeaShipping(),
                ShippingType.Train => new TrainShipping(),
                _ => throw new ArgumentException()
            };
        }

        [Fact]
        public void SelectOptimalShipping_WhenNoFactoryIsValid_ReturnsNull()
        {
            _truckFactoryMock.Setup(f => f.ValidateShipping(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Returns(false);
            _airFactoryMock.Setup(f => f.ValidateShipping(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Returns(false);

            var optimizer = new ShippingOptimizer(new[] { _truckFactoryMock.Object, _airFactoryMock.Object });
            
            var result = optimizer.SelectOptimalShipping(_request);
            
            Assert.Null(result);
        }

        [Fact]
        public void SelectOptimalShipping_WhenOneValidFactory_ReturnsThatShipping()
        {
            var truckShipping = CreateRealShipping(ShippingType.Truck);

            _truckFactoryMock.Setup(f => f.ValidateShipping(100, 50, 10)).Returns(true);
            _truckFactoryMock.Setup(f => f.CreateShipping()).Returns(truckShipping);

            _airFactoryMock.Setup(f => f.ValidateShipping(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Returns(false);

            var optimizer = new ShippingOptimizer(new[] { _truckFactoryMock.Object, _airFactoryMock.Object });
            
            var result = optimizer.SelectOptimalShipping(_request);
            
            Assert.NotNull(result);
            Assert.Equal(ShippingType.Truck, result.ShippingType);
        }

        [Fact]
        public void SelectOptimalShipping_WhenMultipleValid_ReturnsMinimumScore()
        {
            var req = new ShippingRequest { Distance = 100, Weight = 50, Volume = 10 };
            
            _truckFactoryMock.Setup(f => f.ValidateShipping(req.Distance, req.Weight, req.Volume)).Returns(true);
            _airFactoryMock.Setup(f => f.ValidateShipping(req.Distance, req.Weight, req.Volume)).Returns(true);
            
            _truckFactoryMock.Setup(f => f.CreateShipping()).Returns(() => {
                var s = CreateRealShipping(ShippingType.Truck);
                s.Distance = req.Distance; s.Weight = req.Weight; s.Volume = req.Volume;
                return s;
            });
            _airFactoryMock.Setup(f => f.CreateShipping()).Returns(() => { 
                var s = CreateRealShipping(ShippingType.Air);
                s.Distance = req.Distance; s.Weight = req.Weight; s.Volume = req.Volume;
                return s;
            });

            var optimizer = new ShippingOptimizer(new[] { _truckFactoryMock.Object, _airFactoryMock.Object });
            
            var result = optimizer.SelectOptimalShipping(req);
            
            Assert.NotNull(result);
            
            var resultCost = result.CalculateCost();
            var resultDuration = result.CalculateDuration();
            Assert.True(resultCost > 0, "CalculateCost() should be > 0");
            Assert.True(resultDuration > TimeSpan.Zero, "CalculateDuration() should be > 0");
            
            var truckCandidate = CreateRealShipping(ShippingType.Truck);
            truckCandidate.Distance = req.Distance;
            truckCandidate.Weight = req.Weight;
            truckCandidate.Volume = req.Volume;
            var truckScore = truckCandidate.CalculateCost() * 0.6 + truckCandidate.CalculateDuration().TotalHours * 10 * 0.3;

            var airCandidate = CreateRealShipping(ShippingType.Air);
            airCandidate.Distance = req.Distance;
            airCandidate.Weight = req.Weight;
            airCandidate.Volume = req.Volume;
            var airScore = airCandidate.CalculateCost() * 0.6 + airCandidate.CalculateDuration().TotalHours * 10 * 0.3;
            
            var expectedIsTruck = truckScore <= airScore;
            
            if (result.ShippingType != default)
            {
                Assert.Equal(expectedIsTruck ? ShippingType.Truck : ShippingType.Air, result.ShippingType);
            }
            else
            {
                var expectedType = expectedIsTruck ? truckCandidate.GetType() : airCandidate.GetType();
                Assert.Equal(expectedType, result.GetType());
            }
        }
        
        [Fact]
        public void SelectOptimalShipping_SetsShippingPropertiesCorrectly()
        {
            var truckShipping = CreateRealShipping(ShippingType.Truck);

            _truckFactoryMock.Setup(f => f.ValidateShipping(100, 50, 10)).Returns(true);
            _truckFactoryMock.Setup(f => f.CreateShipping()).Returns(truckShipping);

            var optimizer = new ShippingOptimizer(new[] { _truckFactoryMock.Object });
            
            var result = optimizer.SelectOptimalShipping(_request);
            
            Assert.NotNull(result);
            Assert.Equal(_request.Distance, result.Distance);
            Assert.Equal(_request.Weight, result.Weight);
            Assert.Equal(_request.Volume, result.Volume);
        }

        [Fact]
        public void GetShippingQuotes_ReturnsOnlyValidFactories()
        {
            _truckFactoryMock.Setup(f => f.Type).Returns(ShippingType.Truck);
            _truckFactoryMock.Setup(f => f.ValidateShipping(100, 50, 10)).Returns(true);

            _airFactoryMock.Setup(f => f.Type).Returns(ShippingType.Air);
            _airFactoryMock.Setup(f => f.ValidateShipping(100, 50, 10)).Returns(false);

            var truckShipping = CreateRealShipping(ShippingType.Truck);
            _truckFactoryMock.Setup(f => f.CreateShipping()).Returns(truckShipping);

            _truckFactoryMock.Setup(f => f.GetMaxWeight()).Returns(1000);
            _truckFactoryMock.Setup(f => f.GetMaxVolume()).Returns(200);

            var optimizer = new ShippingOptimizer(new[] { _truckFactoryMock.Object, _airFactoryMock.Object });
            
            var result = optimizer.GetShippingQuotes(_request);
            
            Assert.Single(result);
            Assert.Equal(ShippingType.Truck, result[0].Type);
        }

        [Fact]
        public void GetShippingQuotes_QuotesAreSortedByCost()
        {
            var truckShipping = CreateRealShipping(ShippingType.Truck);
            var airShipping = CreateRealShipping(ShippingType.Air);
            
            _truckFactoryMock.Setup(f => f.ValidateShipping(100, 50, 10)).Returns(true);
            _airFactoryMock.Setup(f => f.ValidateShipping(100, 50, 10)).Returns(true);

            _truckFactoryMock.Setup(f => f.CreateShipping()).Returns(truckShipping);
            _airFactoryMock.Setup(f => f.CreateShipping()).Returns(airShipping);
            
            _truckFactoryMock.Setup(f => f.GetMaxWeight()).Returns(500);
            _truckFactoryMock.Setup(f => f.GetMaxVolume()).Returns(100);

            _airFactoryMock.Setup(f => f.GetMaxWeight()).Returns(200);
            _airFactoryMock.Setup(f => f.GetMaxVolume()).Returns(20);

            var optimizer = new ShippingOptimizer(new[] { _truckFactoryMock.Object, _airFactoryMock.Object });
            
            var result = optimizer.GetShippingQuotes(_request);
            
            Assert.Equal(2, result.Count);
            
            Assert.True(result[0].Cost <= result[1].Cost);
        }

        [Fact]
        public void GetShippingQuotes_SetsPropertiesCorrectly()
        {
            var truckShipping = CreateRealShipping(ShippingType.Truck);

            _truckFactoryMock.Setup(f => f.ValidateShipping(100, 50, 10)).Returns(true);
            _truckFactoryMock.Setup(f => f.CreateShipping()).Returns(truckShipping);
            _truckFactoryMock.Setup(f => f.GetMaxWeight()).Returns(500);
            _truckFactoryMock.Setup(f => f.GetMaxVolume()).Returns(100);

            var optimizer = new ShippingOptimizer(new[] { _truckFactoryMock.Object });
            
            var quotes = optimizer.GetShippingQuotes(_request);
            
            Assert.Single(quotes);
            var q = quotes[0];

            Assert.Equal(500, q.MaxWeight);
            Assert.Equal(100, q.MaxVolume);
            Assert.Equal(ShippingType.Truck, q.Type);
            Assert.True(q.Cost > 0);
            Assert.True(q.Duration > TimeSpan.Zero);
        }
    }
}
