using Users.Dtos.User;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Users.Repositories.UserRepository;

namespace Users.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;
        public UserFunctions _userFunctions = new UserFunctions();

        public UserService(IMapper mapper, IUserRepository userRepo, IConfiguration configuration)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<SignInResponseDto>> signIn(SignInRquestDto user)
        {
            string m = "1";
            SignInResponseDto response = new SignInResponseDto();
            try
            {
                var _user = await _userRepo.GetUserByEmailAsync(user.email);
                string token = "";
                if (_user is null)
                {
                    var errorRes = new ServiceResponse<SignInResponseDto>(
                        false,
                        404,
                        null,
                        "User not found."
                    );
                    return errorRes;
                }
                else if (
                    !_userFunctions.VerifyPasswordHash(
                        user.password,
                        _user.passwordHash,
                        _user.passwordSalt
                    )
                )
                {
                    var errorRes = new ServiceResponse<SignInResponseDto>(
                        false,
                        401,
                        null,
                        "Wrong password."
                    );
                    return errorRes;
                }
                else
                {
                    token = _userFunctions.CreateToken(_user, _configuration);
                }

                response = _mapper.Map<SignInResponseDto>(_user);
                response.token = token;
                var successResponse = new ServiceResponse<SignInResponseDto>(
                    true,
                    200,
                    response,
                    "SignedIn Successfully"
                );
                return successResponse;
            }
            catch (Exception ex)
            {
                var errorRes = new ServiceResponse<SignInResponseDto>(false, 400, null, ex.Message);
                return errorRes;
            }
        }

        public async Task<ServiceResponse<SignInResponseDto>> register(RegisterRequestDto user)
        {
            var response = _mapper.Map<SignInResponseDto>(user);
            var _user = _mapper.Map<User>(user);

            if (await _userFunctions.UserExists(_userRepo, user.email))
            {
                var errorRes = new ServiceResponse<SignInResponseDto>(
                    false,
                    400,
                    null,
                    "User already exists!"
                );
                return errorRes;
            }
            else if (user.password != user.confirmedPassword)
            {
                var errorRes = new ServiceResponse<SignInResponseDto>(
                    false,
                    400,
                    null,
                    "Password and confirmedPassword didn't matched"
                );
                return errorRes;
            }
            try
            {
                (byte[] passwordHash, byte[] passwordSalt) = _userFunctions.CreatePasswordHash(
                    user.password
                );

                _user.passwordHash = passwordHash;
                _user.passwordSalt = passwordSalt;
                await _userRepo.AddUserAsync(_user);
                var successResponse = new ServiceResponse<SignInResponseDto>(
                    true,
                    200,
                    response,
                    "Registered Successfully, please logIn"
                );
                return successResponse;
            }
            catch (Exception ex)
            {
                var errorRes = new ServiceResponse<SignInResponseDto>(false, 400, null, ex.Message);
                return errorRes;
            }
        }

        public async Task<ServiceResponse<SignInResponseDto>> getUser(string email)
        {
            try
            {
                var _user = await _userRepo.GetUserByEmailAsync(email);
                if (_user is null)
                {
                    var errorRes = new ServiceResponse<SignInResponseDto>(
                        false,
                        401,
                        null,
                        "User not found."
                    );
                    return errorRes;
                }
                var response = _mapper.Map<SignInResponseDto>(_user);
                var successResponse = new ServiceResponse<SignInResponseDto>(
                    true,
                    200,
                    response,
                    "User Fetched Successfully"
                );
                return successResponse;
            }
            catch (Exception ex)
            {
                var errorRes = new ServiceResponse<SignInResponseDto>(false, 400, null, ex.Message);
                return errorRes;
            }
        }
    }

    public class UserFunctions
    {
        public async Task<bool> UserExists(IUserRepository _userRepo, string email)
        {
            if (await _userRepo.CheckUserExictenceAsync(email))
            {
                return true;
            }
            return false;
        }

        public (byte[], byte[]) CreatePasswordHash(string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            return (passwordHash, passwordSalt);
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public string CreateToken(User user, IConfiguration _configuration)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.email.ToString()),
                new Claim(ClaimTypes.Name, $"{user.firstName} {user.lastName}")
            };

            var appSettingsTokenSection = _configuration.GetSection("AppSettings:Token");
            var appSettingsToken = appSettingsTokenSection?.Value ?? null;
            if (appSettingsToken is null)
                throw new Exception("AppSettings Token is null!");

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(appSettingsToken)
            );

            SigningCredentials creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha512Signature
            );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
