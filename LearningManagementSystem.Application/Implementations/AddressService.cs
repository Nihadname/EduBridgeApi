using AutoMapper;
using LearningManagementSystem.Application.Dtos.Address;
using LearningManagementSystem.Application.Dtos.Course;
using LearningManagementSystem.Application.Dtos.Note;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Application.Interfaces;
using LearningManagementSystem.Core.Entities;
using LearningManagementSystem.Core.Entities.Common;
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
        public async Task<Result<AddressReturnDto>> Create(AddressCreateDto addressCreateDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<AddressReturnDto>.Failure("UserId", "User ID cannot be null",null, ErrorType.UnauthorizedError);
            }
            var existedUser=await _userManager.Users
    .Include(u => u.Address) 
    .FirstOrDefaultAsync(u => u.Id == userId);
            if (existedUser is null) return Result<AddressReturnDto>.Failure("User", "User  cannot be null or not  found",null, ErrorType.NotFoundError);
            addressCreateDto.AppUserId= userId;
            if (existedUser.Address is not null)
                return Result<AddressReturnDto>.Failure("Address", "User already has an address. Update or delete the existing address instead.",null, ErrorType.SystemError);
            var isExistedLocation =await IsLocationExist(addressCreateDto);
            if(!isExistedLocation) return Result<AddressReturnDto>.Failure("location", "location doesnt exist in the map",null, ErrorType.BusinessLogicError);
            var mappedAddress = _mapper.Map<Address>(addressCreateDto);
            mappedAddress.appUser= existedUser;
            await _unitOfWork.AddressRepository.Create(mappedAddress);
            await _unitOfWork.Commit();
            var mappedExistedAddress=_mapper.Map<AddressReturnDto>(mappedAddress);
            return Result<AddressReturnDto>.Success(mappedExistedAddress);
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
        public async Task<Result<string>> DeleteForUser(Guid id)
        {
            if (id == Guid.Empty) return Result<string>.Failure(null, "Invalid GUID provided.",null, ErrorType.ValidationError);
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
               return Result<string>.Failure("UserId", "User ID cannot be null",null, ErrorType.UnauthorizedError);
            }
           var existedUser=await _userManager.FindByIdAsync(userId);
            if(existedUser is null) return Result<string>.Failure("User", "User  cannot be null or not  found",null, ErrorType.NotFoundError);
            var existedAddress = await _unitOfWork.AddressRepository.GetEntity(s => s.Id == id & s.IsDeleted == false&s.AppUserId==existedUser.Id);
            if (existedAddress is null) return Result<string>.Failure("Adress", "Address is null",null, ErrorType.NotFoundError);
            await _unitOfWork.AddressRepository.Delete(existedAddress);
            await _unitOfWork.Commit();
            return Result<string>.Success("Deleted By User");
        }
        public async Task<Result<PaginationDto<AddressListItemDto>>> GetAll(string appUserId=null,int pageNumber = 1,
           int pageSize = 10, string searchQuery = null)
        {
            var addressQuery = await _unitOfWork.AddressRepository.GetQuery(s=>s.IsDeleted==false, true, true);
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                addressQuery = addressQuery.Where(s => s.City.Contains(searchQuery) || s.Country.Contains(searchQuery));
            }
            if(!string.IsNullOrWhiteSpace(appUserId))
            {
                var existedUserWithGivenId =await _userManager.FindByIdAsync(appUserId);
                if (existedUserWithGivenId is null)
                    return Result<PaginationDto<AddressListItemDto>>.Failure("User", "User  cannot be null or not  found",null, ErrorType.NotFoundError);
                addressQuery=addressQuery.Where(s=>s.AppUserId==existedUserWithGivenId.Id);

            }
            addressQuery = addressQuery.OrderByDescending(s => s.CreatedTime);
            var paginationResult = await PaginationDto<AddressListItemDto>.Create(
                addressQuery.Select(f => _mapper.Map<AddressListItemDto>(f)), pageNumber, pageSize);
            return Result<PaginationDto<AddressListItemDto>>.Success(paginationResult);
        }
    }
}
