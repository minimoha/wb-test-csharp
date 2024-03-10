

namespace TestApi.Tests;

public class CustomerServiceTests
{
    [Fact]
    public async Task CreateOrUpdate_WithValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_Database")
            .Options;
        var context = new AppDbContext(contextOptions);

        var otpServiceMock = new Mock<IOtpService>();
        otpServiceMock.Setup(s => s.GetOtp(It.IsAny<long>())).ReturnsAsync(new ResponseParam { Successful = true, Data = "OTP data" });

        var appConfigMock = new Mock<IOptions<AppConfig>>();
        appConfigMock.Setup(x => x.Value).Returns(new AppConfig());

        var customerService = new CustomerService(context, otpServiceMock.Object, appConfigMock.Object);
        var request = new CustomerRequestDto
        {
            PhoneNumber = "09087654678",
            Email = "new@email.com",
            Password = "Pa$$w0rd",
            StateOfResidence = "lagos",
            LGA = "epe"
        };

        // Act
        var result = await customerService.CreateOrUpdate(request);

        // Assert
        Assert.True(result.Successful);
    }

    [Fact]
    public async Task ResendOtp_WithInvalidUserId_ReturnsErrorResponse()
    {
        // Arrange
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_Database")
            .Options;
        var context = new AppDbContext(contextOptions);

        var otpServiceMock = new Mock<IOtpService>();
        var appConfigMock = new Mock<IOptions<AppConfig>>();
        appConfigMock.Setup(x => x.Value).Returns(new AppConfig());

        var customerService = new CustomerService(context, otpServiceMock.Object, appConfigMock.Object);
        var request = new ResendOtpDto { UserId = 0 };

        // Act
        var result = await customerService.ResendOtp(request);

        // Assert
        Assert.False(result.Successful);
        Assert.Equal(ErrorConstants.INVALID_ID, result.Message);
    }

    [Fact]
    public async Task FetchVerifiedUsers_ReturnsCorrectUsers()
    {
        // Arrange
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_Database")
            .Options;
        var context = new AppDbContext(contextOptions);

        var customerList = new List<Customer>
        {
            new Customer { IsUserVerified = true },
            new Customer { IsUserVerified = false },
            new Customer { IsUserVerified = true }
        };
        context.Customers.AddRange(customerList);
        context.SaveChanges();

        var otpServiceMock = new Mock<IOtpService>();
        var appConfigMock = new Mock<IOptions<AppConfig>>();
        appConfigMock.Setup(x => x.Value).Returns(new AppConfig());

        var customerService = new CustomerService(context, otpServiceMock.Object, appConfigMock.Object);

        // Act
        var result = await customerService.FetchVerifiedUsers();

        // Assert
        Assert.True(result.Successful);
        Assert.NotNull(result.Data);
        Assert.Equal(customerList.Where(x => x.IsUserVerified).Count(), ((IEnumerable<Customer>)result.Data).Count());
    }
    
    [Fact]
    public async Task FetchAllUsers_ReturnsAllUsers()
    {
        // Arrange
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Customer_Database")
            .Options;
        var context = new AppDbContext(contextOptions);

        var customerList = new List<Customer>
        {
            new Customer { IsUserVerified = true },
            new Customer { IsUserVerified = false },
            new Customer { IsUserVerified = true }
        };
        context.Customers.AddRange(customerList);
        context.SaveChanges();

        var otpServiceMock = new Mock<IOtpService>();
        var appConfigMock = new Mock<IOptions<AppConfig>>();
        appConfigMock.Setup(x => x.Value).Returns(new AppConfig());

        var customerService = new CustomerService(context, otpServiceMock.Object, appConfigMock.Object);

        // Act
        var result = await customerService.FetchAllUsers();

        // Assert
        Assert.True(result.Successful);
        Assert.NotNull(result.Data);
        Assert.Equal(customerList.Count, ((IEnumerable<Customer>)result.Data).Count());
    }
    
    
   

    [Fact]
    public async Task VerifyUser_WithInvalidUserId_ReturnsErrorResponse()
    {
        // Arrange
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_Database")
            .Options;
        var context = new AppDbContext(contextOptions);

        var otpServiceMock = new Mock<IOtpService>();
        var appConfigMock = new Mock<IOptions<AppConfig>>();
        appConfigMock.Setup(x => x.Value).Returns(new AppConfig());

        var customerService = new CustomerService(context, otpServiceMock.Object, appConfigMock.Object);
        //Invalid UserId
        var request = new OtpDto { UserId = 0, OTP = "123456" };

        // Act
        var result = await customerService.VerifyUser(request);

        // Assert
        Assert.False(result.Successful);
        Assert.Equal(ErrorConstants.INVALID_ID, result.Message);
    }

    [Fact]
    public async Task VerifyUser_WithInvalidOTP_ReturnsErrorResponse()
    {
        // Arrange
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_Database")
            .Options;
        var context = new AppDbContext(contextOptions);

        var otpServiceMock = new Mock<IOtpService>();
        var appConfigMock = new Mock<IOptions<AppConfig>>();
        appConfigMock.Setup(x => x.Value).Returns(new AppConfig());

        var customerService = new CustomerService(context, otpServiceMock.Object, appConfigMock.Object);
        // Invalid OTP
        var request = new OtpDto { UserId = 1, OTP = null };

        // Act
        var result = await customerService.VerifyUser(request);

        // Assert
        Assert.False(result.Successful);
        Assert.Equal(ErrorConstants.INVALID_INPUT, result.Message);
    }

    [Fact]
    public async Task FetchStates_ReturnsStates()
    {
        // Arrange
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "Test_Database")
                    .Options;
        var context = new AppDbContext(contextOptions);

        var otpServiceMock = new Mock<IOtpService>();
        var appConfigMock = new Mock<IOptions<AppConfig>>();
        appConfigMock.Setup(x => x.Value).Returns(new AppConfig());

        var customerService = new CustomerService(context, otpServiceMock.Object, appConfigMock.Object);
        // Act
        var result = await customerService.FetchStates();

        // Assert
        Assert.True(result.Successful);
        Assert.NotNull(result.Data);
        Assert.IsType<List<State>>(result.Data);
    }

    [Fact]
    public async Task FetchState_WithValidState_ReturnsState()
    {
        // Arrange
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "Test_Database")
                    .Options;
        var context = new AppDbContext(contextOptions);

        var otpServiceMock = new Mock<IOtpService>();
        var appConfigMock = new Mock<IOptions<AppConfig>>();
        appConfigMock.Setup(x => x.Value).Returns(new AppConfig());

        var customerService = new CustomerService(context, otpServiceMock.Object, appConfigMock.Object);
        var validState = "Lagos"; 

        // Act
        var result = await customerService.FetchState(validState);

        // Assert
        Assert.True(result.Successful);
        Assert.NotNull(result.Data);
        Assert.IsType<State>(result.Data);
    }

    [Fact]
    public async Task FetchState_WithInvalidState_ReturnsErrorResponse()
    {
        // Arrange
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                          .UseInMemoryDatabase(databaseName: "Test_Database")
                          .Options;
        var context = new AppDbContext(contextOptions);

        var otpServiceMock = new Mock<IOtpService>();
        var appConfigMock = new Mock<IOptions<AppConfig>>();
        appConfigMock.Setup(x => x.Value).Returns(new AppConfig());

        var customerService = new CustomerService(context, otpServiceMock.Object, appConfigMock.Object); var invalidState = "Alaska"; // Change to an invalid state from your test data

        // Act
        var result = await customerService.FetchState(invalidState);

        // Assert
        Assert.False(result.Successful);
        Assert.Equal(ErrorConstants.INVALID_INPUT, result.Message);
    }

    

}
