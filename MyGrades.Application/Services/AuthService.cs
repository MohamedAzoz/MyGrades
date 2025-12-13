using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.User;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Application.Helper;
using MyGrades.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyGrades.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        //private readonly IEmailVerification emailVerification;
        private readonly IMapper mapper;
        private readonly JWT jwt;

        public AuthService( UserManager<AppUser> _userManager,
            IUnitOfWork _unitOfWork,
            IConfiguration _configuration,
            IMapper mapper, IOptions<JWT> jwt
            )
        {
            userManager = _userManager;
            unitOfWork = _unitOfWork;
            configuration = _configuration;
            this.mapper = mapper;
            this.jwt = jwt.Value;
        }
        public async Task<Result<AppUser>> AddAdminAsync(string name, string nationalId, string Parameter)
        {
            var user = await userManager.FindByIdAsync(nationalId);
            if (user != null)
                return Result<AppUser>.Failure("User found");
            user = new AppUser { NationalId = nationalId, UserName = nationalId, FullName = name };
            var result = await userManager.CreateAsync(user, Parameter);
            if (!result.Succeeded)
                return Result<AppUser>.Failure("Failed to create user");
            await userManager.AddToRoleAsync(user, "Admin");

            return Result<AppUser>.Success(user);
        }

        public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await userManager.FindByNameAsync(changePasswordDto.NationalId);
            if (user == null)
                return Result<bool>.Failure("User not found");

            var TokenValid = await userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);
            if (!TokenValid)
                return Result<bool>.Failure("Current password is incorrect");

            IdentityResult result = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword
                                    , changePasswordDto.NewPassword);
            if (!result.Succeeded)
                return Result<bool>.Failure("Failed to change password");
            
            return Result<bool>.Success(true);
        }

        public async Task<Result> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure("User not found");

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return Result.Failure("Failed to delete user");

            return Result.Success();
        }

        public async Task<Result<AuthModelDto>> LoginAsync(UserLoginDto userLoginDto)
        {
            AuthModelDto authModel = new AuthModelDto();
            var result1 = await unitOfWork.Users.FindAsync(u=>u.NationalId==userLoginDto.NationalId);
            if (result1==null || result1.Data==null)
                return Result<AuthModelDto>.Failure("NationalId or password is incorrect");
            var user = result1.Data;
           
            var result = await userManager.CheckPasswordAsync(user, userLoginDto.Password);
            if (!result || user == null)
                return Result<AuthModelDto>.Failure("NationalId or password is incorrect");

            var jwtToken = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.NationalId = user.NationalId;
            authModel.UserId= user.Id;
            authModel.FullName = user.FullName;
            authModel.ExpiresOn = jwtToken.ValidTo;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var roles = await userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();

            return Result<AuthModelDto>.Success(authModel);
        }

        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var roles = await userManager.GetRolesAsync(user);
            var claims = await userManager.GetClaimsAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var userClaims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.NationalId),
                new Claim("uid", user.Id)
            }
            .Union(roleClaims)
            .Union(claims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(jwt.Duration),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return jwtSecurityToken;
        }




    }
}
