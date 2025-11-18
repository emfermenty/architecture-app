using LogisticService.Application.Commands;
using LogisticService.Application.Commands.interfaces;
using LogisticService.Application.DTO;
using LogisticService.Application.Services;
using LogisticService.Domain.Enums;
using LogisticService.Domain.Models.Shipping;
using LogisticService.Domain.Models.Shipping.Abstract;
using LogisticService.Domain.Models.Shipping.ShippingFactory.Interfaces;
using LogisticService.Domain.Observer;
using LogisticService.Infrastructure.Repository.Interfaces;
using Moq;
using ICommand = System.Windows.Input.ICommand;

namespace LogisticService.Tests.Services
{
    public class ShippingServiceTests
    {
        private readonly Mock<IShippingsRepository> _repoMock;
        private readonly Mock<ShippingOptimizer> _optimizerMock;
        private readonly Mock<IShippingFactory> _truckFactoryMock;
        private readonly Mock<IShippingFactory> _airFactoryMock;
        private readonly Mock<ICommandHandler> _commandHandlerMock;
        private readonly Mock<IObserverManager> _observerMock;
        private readonly Mock<IVehicleProvider> _vehicleProviderMock;

        private readonly ShippingService _service;
        private readonly ShippingRequest _request;

        public ShippingServiceTests()
        {
            _repoMock = new Mock<IShippingsRepository>();
            _optimizerMock = new Mock<ShippingOptimizer>(new List<IShippingFactory>());
            _truckFactoryMock = new Mock<IShippingFactory>();
            _airFactoryMock = new Mock<IShippingFactory>();
            _commandHandlerMock = new Mock<ICommandHandler>();
            _observerMock = new Mock<IObserverManager>();
            _vehicleProviderMock = new Mock<IVehicleProvider>();

            _truckFactoryMock.Setup(f => f.Type).Returns(ShippingType.Truck);
            _airFactoryMock.Setup(f => f.Type).Returns(ShippingType.Air);

            var factories = new[] { _truckFactoryMock.Object, _airFactoryMock.Object };

            _service = new ShippingService(
                _repoMock.Object,
                _optimizerMock.Object,
                factories,
                _commandHandlerMock.Object,
                _observerMock.Object,
                _vehicleProviderMock.Object
            );

            _request = new ShippingRequest
            {
                Distance = 100,
                Weight = 50,
                Volume = 10
            };
        }
        
        [Fact]
        public void CreateShipping_WhenFactoryInvalid_ReturnsNull()
        {
            var command = new CreateShippingCommand
            {
                Distance = 100,
                Weight = 50,
                Volume = 10,
                Type = ShippingType.Truck
            };

            _truckFactoryMock
                .Setup(f => f.ValidateShipping(100, 50, 10))
                .Returns(false);

            var result = _service.CreateShipping(command);

            Assert.Null(result);
        }

        [Fact]
        public void CreateShipping_WhenFactoryValid_ReturnsShippingDto()
        {
            var command = new CreateShippingCommand
            {
                Distance = 100,
                Weight = 50,
                Volume = 10,
                Type = ShippingType.Truck
            };

            var shipping = new TruckShipping
            {
                Id = Guid.NewGuid(),
                Distance = 10,
                Weight = 10,
                Volume = 10
            };

            _truckFactoryMock.Setup(f => f.ValidateShipping(100, 50, 10)).Returns(true);
            _truckFactoryMock.Setup(f => f.CreateShipping()).Returns(shipping);

            var result = _service.CreateShipping(command);

            Assert.NotNull(result);
            Assert.Equal(16, result.Cost);
            Assert.Equal(ShippingType.Truck, result.ShippingType);
        }

        [Fact]
        public async Task CreateShippingWithVehicleAsync_UsesCommandHandler()
        {
            var dto = new CreateShippingCommandDTO { Distance = 100, Weight = 20, Volume = 5 };

            _commandHandlerMock
                .Setup(h => h.HandleAsync(It.IsAny<CreateShippingWithVehicleCommand>()))
                .Callback<CreateShippingWithVehicleCommand>(cmd =>
                {
                    cmd.SetResult(new { Id = 1 }); 
                })
                .Returns(Task.CompletedTask);

            var result = await _service.CreateShippingWithVehicleAsync(dto);

            Assert.NotNull(result);
        }
        

        [Fact]
        public async Task UndoLastCommandAsync_CallsHandler()
        {
            await _service.UndoLastCommandAsync();

            _commandHandlerMock.Verify(h => h.UndoAsync(), Times.Once);
        }

        [Fact]
        public void CanUndo_ReturnsHandlerValue()
        {
            _commandHandlerMock.SetupGet(h => h.CanUndo).Returns(true);

            Assert.True(_service.CanUndo);
        }

        // -------------------------------
        // GetAllShippings
        // -------------------------------

        [Fact]
        public async Task GetAllShippings_CallsRepository()
        {
            var list = new List<Shipping>();
            _repoMock.Setup(r => r.GetAllShippingsAsync()).ReturnsAsync(list);

            var result = await _service.GetAllShippings();

            Assert.Equal(list, result);
        }
    }
}
