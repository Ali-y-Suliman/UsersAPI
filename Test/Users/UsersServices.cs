using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Users.Dtos.User;
using Users.Repositories.UserRepository;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Users.Test.Users
{
    public class UsersServices
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<IConfiguration> configurationMock;
        public UserFunctions _userFunctions = new UserFunctions();

        public UsersServices(ITestOutputHelper output)
        {
            mapperMock = new Mock<IMapper>();
            userRepositoryMock = new Mock<IUserRepository>();
            configurationMock = new Mock<IConfiguration>();
            _output = output;
        }

        [Fact]
        public async void signInService_ReturnsUserNotFound()
        {
            // Arrange
            SignInRquestDto user = new SignInRquestDto
            {
                email = "ali.s@gmail.com",
                password = "abcd"
            };
            User? expectedUser = null;

            mapperMock
                .Setup(mapper => mapper.Map<User, SignInResponseDto>(It.IsAny<User>()))
                .Returns(new SignInResponseDto { });

            configurationMock
                .Setup(config => config["AppSettings:Token"])
                .Returns("my top secret key");

            userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(user.email))
                .ReturnsAsync(expectedUser);

            var mockedMapper = mapperMock.Object;

            var mockedConfiguration = configurationMock.Object;

            var mockedUserRepository = userRepositoryMock.Object;

            var userService = new UserService(
                mockedMapper,
                mockedUserRepository,
                mockedConfiguration
            );

            // Act
            var signInResult = await userService.signIn(user);

            // Assert
            Assert.False(signInResult.Success);
            Assert.Equal(404, signInResult.StatusCode);
            Assert.Equal("User not found.", signInResult.Message);
            Assert.Null(signInResult.Data);
        }

        [Fact]
        public async void signInService_Wrong_password()
        {
            // Arrange
            SignInRquestDto user = new SignInRquestDto
            {
                email = "ali.s@gmail.com",
                password = "abcd"
            };

            UserFunctions _userFunctions = new UserFunctions();

            (byte[] passwordHash, byte[] passwordSalt) = _userFunctions.CreatePasswordHash("aaaa");

            var _passwordHash = passwordHash;
            var _passwordSalt = passwordSalt;

            User? expectedUser = new User
            {
                id = 1,
                firstName = "ali",
                lastName = "suliman",
                email = "ali.s@gmail.com",
                passwordHash = _passwordHash,
                passwordSalt = _passwordSalt
            };

            mapperMock
                .Setup(mapper => mapper.Map<User, SignInResponseDto>(It.IsAny<User>()))
                .Returns(new SignInResponseDto { });

            configurationMock.Setup(config => config["SomeKey"]).Returns("SomeValue");

            userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(user.email))
                .ReturnsAsync(expectedUser);

            var mockedMapper = mapperMock.Object;

            var mockedConfiguration = configurationMock.Object;

            var mockedUserRepository = userRepositoryMock.Object;

            var userService = new UserService(
                mockedMapper,
                mockedUserRepository,
                mockedConfiguration
            );

            // Act
            var signInResult = await userService.signIn(user);

            // Assert
            Assert.False(signInResult.Success);
            Assert.Equal(401, signInResult.StatusCode);
            Assert.Equal("Wrong password.", signInResult.Message);
            Assert.Null(signInResult.Data);
        }

        [Fact]
        public async void signInService_Success()
        {
            // Arrange
            SignInRquestDto user = new SignInRquestDto();
            user.email = "ali.s@gmail.com";
            user.password = "abcd";

            (byte[] passwordHash, byte[] passwordSalt) = _userFunctions.CreatePasswordHash(
                user.password
            );

            User expectedUser = new User
            {
                id = 1,
                firstName = "ali",
                lastName = "suliman",
                email = "ali.s@gmail.com"
            };

            expectedUser.passwordHash = passwordHash;
            expectedUser.passwordSalt = passwordSalt;

            mapperMock
                .Setup(mapper => mapper.Map<SignInResponseDto>(It.IsAny<User>()))
                .Returns(new SignInResponseDto { });

            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationMock
                .Setup(config => config.GetSection("AppSettings:Token"))
                .Returns(configurationSectionMock.Object);

            configurationSectionMock.Setup(s => s.Value).Returns("my top secret key");

            userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(user.email))
                .ReturnsAsync(expectedUser);

            var mockedMapper = mapperMock.Object;

            var mockedConfiguration = configurationMock.Object;

            var mockedUserRepository = userRepositoryMock.Object;

            var userService = new UserService(
                mockedMapper,
                mockedUserRepository,
                mockedConfiguration
            );

            string expectedToken = _userFunctions.CreateToken(expectedUser, mockedConfiguration);

            // Act
            var signInResult = await userService.signIn(user);

            // Assert
            Assert.True(signInResult.Success);
            Assert.Equal(200, signInResult.StatusCode);
            Assert.Equal("SignedIn Successfully", signInResult.Message);
            Assert.Equal(signInResult?.Data?.token, expectedToken);
        }

        [Fact]
        public async void registerService_Wrong_ExistedUser()
        {
            // Arrange
            RegisterRequestDto user = new RegisterRequestDto();
            user.email = "ali.s@gmail.com";
            user.password = "abcd";
            user.confirmedPassword = "abcd";
            user.firstName = "ali";
            user.lastName = "suliman";

            (byte[] passwordHash, byte[] passwordSalt) = _userFunctions.CreatePasswordHash(
                user.password
            );

            User expectedUser = new User
            {
                id = 1,
                firstName = "ali",
                lastName = "suliman",
                email = "ali.s@gmail.com"
            };

            expectedUser.passwordHash = passwordHash;
            expectedUser.passwordSalt = passwordSalt;

            mapperMock
                .Setup(mapper => mapper.Map<SignInResponseDto>(It.IsAny<User>()))
                .Returns(new SignInResponseDto { });

            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationMock
                .Setup(config => config.GetSection("AppSettings:Token"))
                .Returns(configurationSectionMock.Object);

            configurationSectionMock.Setup(s => s.Value).Returns("my top secret key");

            userRepositoryMock
                .Setup(repo => repo.CheckUserExictenceAsync(user.email))
                .ReturnsAsync(true);

            var mockedMapper = mapperMock.Object;

            var mockedConfiguration = configurationMock.Object;

            var mockedUserRepository = userRepositoryMock.Object;

            var userService = new UserService(
                mockedMapper,
                mockedUserRepository,
                mockedConfiguration
            );

            // Act
            var signInResult = await userService.register(user);

            // Assert
            Assert.False(signInResult.Success);
            Assert.Equal(400, signInResult.StatusCode);
            Assert.Equal("User already exists!", signInResult.Message);
        }

        [Fact]
        public async void registerService_Wrong_Password_MissMatched()
        {
            // Arrange
            RegisterRequestDto user = new RegisterRequestDto();
            user.email = "ali.s@gmail.com";
            user.password = "abcd";
            user.confirmedPassword = "ssss";
            user.firstName = "ali";
            user.lastName = "suliman";

            (byte[] passwordHash, byte[] passwordSalt) = _userFunctions.CreatePasswordHash(
                user.password
            );

            User expectedUser = new User
            {
                id = 1,
                firstName = "ali",
                lastName = "suliman",
                email = "ali.s@gmail.com"
            };

            expectedUser.passwordHash = passwordHash;
            expectedUser.passwordSalt = passwordSalt;

            mapperMock
                .Setup(mapper => mapper.Map<SignInResponseDto>(It.IsAny<User>()))
                .Returns(new SignInResponseDto { });

            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationMock
                .Setup(config => config.GetSection("AppSettings:Token"))
                .Returns(configurationSectionMock.Object);

            configurationSectionMock.Setup(s => s.Value).Returns("my top secret key");

            userRepositoryMock
                .Setup(repo => repo.CheckUserExictenceAsync(user.email))
                .ReturnsAsync(false);

            var mockedMapper = mapperMock.Object;

            var mockedConfiguration = configurationMock.Object;

            var mockedUserRepository = userRepositoryMock.Object;

            var userService = new UserService(
                mockedMapper,
                mockedUserRepository,
                mockedConfiguration
            );

            // Act
            var signInResult = await userService.register(user);

            // Assert
            Assert.False(signInResult.Success);
            Assert.Equal(400, signInResult.StatusCode);
            Assert.Equal("Password and confirmedPassword didn't matched", signInResult.Message);
        }

        [Fact]
        public async void registerService_Success()
        {
            // Arrange
            RegisterRequestDto user = new RegisterRequestDto();
            user.email = "ali.suliman.95@gmail.com";
            user.password = "abcd";
            user.confirmedPassword = "abcd";
            user.firstName = "ali";
            user.lastName = "suliman";

            (byte[] passwordHash, byte[] passwordSalt) = _userFunctions.CreatePasswordHash(
                user.password
            );

            User expectedUser = new User
            {
                id = 1,
                firstName = "ali",
                lastName = "suliman",
                email = "ali.s@gmail.com"
            };

            expectedUser.passwordHash = passwordHash;
            expectedUser.passwordSalt = passwordSalt;

            mapperMock
                .Setup(mapper => mapper.Map<SignInResponseDto>(It.IsAny<User>()))
                .Returns(new SignInResponseDto { });

            mapperMock
                .Setup(mapper => mapper.Map<SignInResponseDto>(It.IsAny<RegisterRequestDto>()))
                .Returns(new SignInResponseDto { });

            mapperMock
                .Setup(mapper => mapper.Map<User>(It.IsAny<RegisterRequestDto>()))
                .Returns(new User { });

            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationMock
                .Setup(config => config.GetSection("AppSettings:Token"))
                .Returns(configurationSectionMock.Object);

            configurationSectionMock.Setup(s => s.Value).Returns("my top secret key");

            userRepositoryMock
                .Setup(repo => repo.CheckUserExictenceAsync(user.email))
                .ReturnsAsync(false);

            var mockedMapper = mapperMock.Object;

            var mockedConfiguration = configurationMock.Object;

            var mockedUserRepository = userRepositoryMock.Object;

            var userService = new UserService(
                mockedMapper,
                mockedUserRepository,
                mockedConfiguration
            );

            // Act
            var signInResult = await userService.register(user);

            // Assert
            Assert.True(signInResult.Success);
            Assert.Equal(200, signInResult.StatusCode);
            Assert.Equal("Registered Successfully, please logIn", signInResult.Message);
        }

        [Fact]
        public async void getUser_Success()
        {
            // Arrange
            SignInResponseDto user = new SignInResponseDto();
            user.email = "ali.suliman.95@gmail.com";
            user.firstName = "ali";
            user.lastName = "suliman";

            (byte[] passwordHash, byte[] passwordSalt) = _userFunctions.CreatePasswordHash("abcd");

            User expectedUser = new User
            {
                id = 1,
                firstName = "ali",
                lastName = "suliman",
                email = "ali.s@gmail.com"
            };

            expectedUser.passwordHash = passwordHash;
            expectedUser.passwordSalt = passwordSalt;

            mapperMock
                .Setup(mapper => mapper.Map<SignInResponseDto>(It.IsAny<User>()))
                .Returns(new SignInResponseDto { });

            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationMock
                .Setup(config => config.GetSection("AppSettings:Token"))
                .Returns(configurationSectionMock.Object);

            configurationSectionMock.Setup(s => s.Value).Returns("my top secret key");

            userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(user.email))
                .ReturnsAsync(expectedUser);

            var mockedMapper = mapperMock.Object;

            var mockedConfiguration = configurationMock.Object;

            var mockedUserRepository = userRepositoryMock.Object;

            var userService = new UserService(
                mockedMapper,
                mockedUserRepository,
                mockedConfiguration
            );

            // Act
            var signInResult = await userService.getUser(user.email);

            // Assert
            Assert.True(signInResult.Success);
            Assert.Equal(200, signInResult.StatusCode);
            Assert.Equal("User Fetched Successfully", signInResult.Message);
        }

        [Fact]
        public async void getUser_Failde_User_Not_Found()
        {
            // Arrange
            SignInResponseDto user = new SignInResponseDto();
            user.email = "ali.suliman.95@gmail.com";
            user.firstName = "ali";
            user.lastName = "suliman";

            (byte[] passwordHash, byte[] passwordSalt) = _userFunctions.CreatePasswordHash("abcd");

            User? expectedUser = null;

            mapperMock
                .Setup(mapper => mapper.Map<SignInResponseDto>(It.IsAny<User>()))
                .Returns(new SignInResponseDto { });

            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationMock
                .Setup(config => config.GetSection("AppSettings:Token"))
                .Returns(configurationSectionMock.Object);

            configurationSectionMock.Setup(s => s.Value).Returns("my top secret key");

            userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(user.email))
                .ReturnsAsync(expectedUser);

            var mockedMapper = mapperMock.Object;

            var mockedConfiguration = configurationMock.Object;

            var mockedUserRepository = userRepositoryMock.Object;

            var userService = new UserService(
                mockedMapper,
                mockedUserRepository,
                mockedConfiguration
            );

            // Act
            var signInResult = await userService.getUser(user.email);

            // Assert
            Assert.False(signInResult.Success);
            Assert.Equal(401, signInResult.StatusCode);
            Assert.Equal("User not found.", signInResult.Message);
        }
    }
}
