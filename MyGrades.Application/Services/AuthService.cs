using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Application.Helper;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        //private readonly IEmailVerification emailVerification;
        private readonly IMapper mapper;
        private readonly IOptions<JWT> jwt;

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
            this.jwt = jwt;
        }

        public Task<Result> Add()
        {
            throw new NotImplementedException();
        }

        public Task<Result> AddRang()
        {
            throw new NotImplementedException();
        }

        public Task<Result> Delete()
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAllRang()
        {
            throw new NotImplementedException();
        }

        public Task<Result> Find()
        {
            throw new NotImplementedException();
        }

        public Task<Result> getAllUser()
        {
            throw new NotImplementedException();
        }

        public Task<Result<AppUser>> Search()
        {
            throw new NotImplementedException();
        }

        public Task<Result> Update()
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateRang()
        {
            throw new NotImplementedException();
        }
    }
}
