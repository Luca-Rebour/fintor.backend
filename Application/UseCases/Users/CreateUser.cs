using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Users;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.Users
{
    public class CreateUser : ICreateUser
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public CreateUser(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordService passwordService,
            ICurrencyRepository currencyRepository,
            IUnitOfWork unitOfWork,
            IJwtService jwtService
            ) 
        { 
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordService = passwordService;
            _currencyRepository = currencyRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }
        public async Task<CreateUserResponseDTO> Execute(CreateUserDTO dto)
        {
            dto.Validate();
            if (await _userRepository.GetUserByEmail(dto.Email) != null)
            {
                throw new AuthenticationException(); 
            }
            Currency currency = await _currencyRepository.GetCurrencyByCodeAsync(dto.BaseCurrencyCode);
            if (currency == null)
            {
               currency = new Currency(dto.BaseCurrencyCode);
                _currencyRepository.CreateCurrency(currency);
                await _unitOfWork.SaveChangesAsync();
            }

            User newUser = _mapper.Map<User>(dto);
            newUser.setBaseCurrencyId(currency.Id);
            newUser.SetPassword(_passwordService.HashPassword(dto.Password));
            _userRepository.CreateUser(newUser);
            await _unitOfWork.SaveChangesAsync();
            CreateUserResponseDTO ret = new CreateUserResponseDTO();
            ret.User = _mapper.Map<UserDTO>(newUser);
            ret.Token = _jwtService.GenerateToken(newUser.Id, newUser.Email);
            return ret;
        }

    }
}
