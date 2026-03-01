using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Users;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users
{
    public class Me : IMe
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public Me(
            IUserRepository userRepository,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDTO> Execute(Guid userId)
        {
            UserDTO dto = _mapper.Map<UserDTO>(await _userRepository.GetUserById(userId));
            return dto;
        }
    }
}
