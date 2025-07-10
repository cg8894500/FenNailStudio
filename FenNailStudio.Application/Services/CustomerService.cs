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
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
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
            var customer = await _customerRepository.GetByIdAsync(customerDto.Id);
            _mapper.Map(customerDto, customer);
            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            await _customerRepository.DeleteAsync(id);
        }
    }
}
