using AutoMapper;
using FenNailStudio.Application.DTOs;
using FenNailStudio.Application.Interfaces;
using FenNailStudio.Domain.Entities;
using FenNailStudio.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository customerRepository,
            IAuthService authService,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            // 如果是管理員，可以查看任何客戶資料
            if (_authService.IsCurrentUserAdmin())
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                return _mapper.Map<CustomerDto>(customer);
            }

            // 如果是普通用戶，只能查看自己的資料
            var currentUserId = _authService.GetCurrentUserId();
            if (id == currentUserId)
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                return _mapper.Map<CustomerDto>(customer);
            }

            // 無權訪問其他用戶的資料
            return null;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            // 如果是管理員，返回所有客戶
            if (_authService.IsCurrentUserAdmin())
            {
                var allCustomers = await _customerRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CustomerDto>>(allCustomers);
            }

            // 如果是普通用戶，只返回自己的資料
            var currentUserId = _authService.GetCurrentUserId();
            var customer = await _customerRepository.GetByIdAsync(currentUserId);

            return customer != null
                ? new List<CustomerDto> { _mapper.Map<CustomerDto>(customer) }
                : new List<CustomerDto>();
        }

        public async Task<CustomerDto> GetByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<int> CreateAsync(CreateCustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            await _customerRepository.AddAsync(customer);
            return customer.Id;
        }

        public async Task UpdateAsync(UpdateCustomerDto customerDto)
        {
            // 檢查是否有權限更新
            if (!_authService.IsCurrentUserAdmin())
            {
                var currentUserId = _authService.GetCurrentUserId();
                if (customerDto.Id != currentUserId)
                {
                    throw new UnauthorizedAccessException("您無權更新其他用戶的資料");
                }
            }

            var customer = await _customerRepository.GetByIdAsync(customerDto.Id);
            _mapper.Map(customerDto, customer);
            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            // 檢查是否有權限刪除
            if (!_authService.IsCurrentUserAdmin())
            {
                var currentUserId = _authService.GetCurrentUserId();
                if (id != currentUserId)
                {
                    throw new UnauthorizedAccessException("您無權刪除其他用戶的資料");
                }
            }

            await _customerRepository.DeleteAsync(id);
        }
    }
}
