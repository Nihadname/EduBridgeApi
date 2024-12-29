using AutoMapper;
using LearningManagementSystem.Application.Dtos.Address;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.DataAccess.Data.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Implementations
{
    public class AddressService:IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public AddressService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, HttpClient httpClient)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task<AddressReturnDto> Create(AddressCreateDto addressCreateDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new CustomException(401, "Id", "User ID cannot be null");
            }
            var existedUser=await _userManager.Users
    .Include(u => u.Address) 
    .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser is null) throw new CustomException(404, "User", "User  cannot be null or not  found");
            addressCreateDto.AppUserId= userId;
            if (existedUser.Address is not null) throw new CustomException(400, "Address", "User already has an address. Update or delete the existing address instead.");
            var isExistedLocation =await IsLocationExist(addressCreateDto);
            if(!isExistedLocation) throw new CustomException(404, "location", "location doesnt exist in the map");
            var mappedAddress = _mapper.Map<Address>(addressCreateDto);
            mappedAddress.appUser= existedUser;
            await _unitOfWork.AddressRepository.Create(mappedAddress);
            await _unitOfWork.Commit();
            var mappedExistedAddress=_mapper.Map<AddressReturnDto>(mappedAddress);
            return mappedExistedAddress;
        }
        private async Task<bool> IsLocationExist(AddressCreateDto addressCreateDto)
        {
            var apiKey = _configuration.GetSection("MapApiKey").Value;

            var url = $"https://us1.locationiq.com/v1/search?key={apiKey}&q={addressCreateDto.Country}%20{addressCreateDto.Region}%20{addressCreateDto.City}%20{addressCreateDto.Street}&format=json";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var content = await response.Content.ReadAsStringAsync();

                var locations = JsonDocument.Parse(content).RootElement;

                if (locations.ValueKind == JsonValueKind.Array && locations.GetArrayLength() > 0)
                {
                    foreach (var location in locations.EnumerateArray())
                    {
                        if (location.TryGetProperty("place_id", out _) &&
                            location.TryGetProperty("lat", out _) &&
                            location.TryGetProperty("lon", out _))
                        {
                            return true; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return false;
        }

    }
}
