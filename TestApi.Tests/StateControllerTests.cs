

namespace TestApi.Tests;
    public class StateControllerTests
    {
        [Fact]
        public async Task FetchStates_ReturnsOk()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var controller = new StateController(customerServiceMock.Object);
            var response = new ResponseParam { Successful = true };

            customerServiceMock.Setup(s => s.FetchStates())
                               .ReturnsAsync(response);

            // Act
            var result = await controller.FetchStates();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<ResponseParam>(okResult.Value);
            Assert.True(resultValue.Successful);
        }

        [Fact]
        public async Task FetchState_ValidRequest_ReturnsOk()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var controller = new StateController(customerServiceMock.Object);
            var state = "Lagos";
            var response = new ResponseParam { Successful = true };

            customerServiceMock.Setup(s => s.FetchState(state))
                               .ReturnsAsync(response);

            // Act
            var result = await controller.FetchState(state);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<ResponseParam>(okResult.Value);
            Assert.True(resultValue.Successful);
        }

        [Fact]
        public async Task FetchBanks_ReturnsOk()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var controller = new StateController(customerServiceMock.Object);
            var response = new ResponseParam { Successful = true };

            customerServiceMock.Setup(s => s.FetchBanks())
                               .ReturnsAsync(response);

            // Act
            var result = await controller.FetchBanks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<ResponseParam>(okResult.Value);
            Assert.True(resultValue.Successful);
        }
    }
