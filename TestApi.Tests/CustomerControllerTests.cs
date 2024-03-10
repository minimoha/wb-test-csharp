
namespace TestApi.Tests;
    public class CustomerControllerTests
    {
        [Fact]
        public async Task AddOrUpdateCustomer_ValidRequest_ReturnsOk()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var controller = new CustomerController(customerServiceMock.Object);
            var request = new CustomerRequestDto();
            var response = new ResponseParam { Successful = true };

            customerServiceMock.Setup(s => s.CreateOrUpdate(request))
                               .ReturnsAsync(response);

            // Act
            var result = await controller.AddOrUpdateCustomer(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<ResponseParam>(okResult.Value);
            Assert.True(resultValue.Successful);
        }

        [Fact]
        public async Task VerifyUser_ValidRequest_ReturnsOk()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var controller = new CustomerController(customerServiceMock.Object);
            var request = new OtpDto();
            var response = new ResponseParam { Successful = true };

            customerServiceMock.Setup(s => s.VerifyUser(request))
                               .ReturnsAsync(response);

            // Act
            var result = await controller.VerifyUser(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<ResponseParam>(okResult.Value);
            Assert.True(resultValue.Successful);
        }

        [Fact]
        public async Task FetchVerifiedUsers_ReturnsOk()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var controller = new CustomerController(customerServiceMock.Object);
            var response = new ResponseParam { Successful = true };

            customerServiceMock.Setup(s => s.FetchVerifiedUsers())
                               .ReturnsAsync(response);

            // Act
            var result = await controller.FetchVerifiedUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<ResponseParam>(okResult.Value);
            Assert.True(resultValue.Successful);
        }
    
    [Fact]
        public async Task FetchAllUsers_ReturnsOk()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var controller = new CustomerController(customerServiceMock.Object);
            var response = new ResponseParam { Successful = true };

            customerServiceMock.Setup(s => s.FetchAllUsers())
                               .ReturnsAsync(response);

            // Act
            var result = await controller.FetchAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<ResponseParam>(okResult.Value);
            Assert.True(resultValue.Successful);
        }

        [Fact]
        public async Task ActivateUser_ValidRequest_ReturnsOk()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var controller = new CustomerController(customerServiceMock.Object);
            var request = new ResendOtpDto();
            var response = new ResponseParam { Successful = true };

            customerServiceMock.Setup(s => s.ResendOtp(request))
                               .ReturnsAsync(response);

            // Act
            var result = await controller.ActivateUser(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<ResponseParam>(okResult.Value);
            Assert.True(resultValue.Successful);
        }
    }
