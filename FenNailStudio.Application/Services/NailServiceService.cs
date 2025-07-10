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
    public class NailServiceService : INailServiceService
    {
        private readonly INailServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public NailServiceService(INailServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<NailServiceDto> GetByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            return _mapper.Map<NailServiceDto>(service);
        }

        public async Task<IEnumerable<NailServiceDto>> GetAllAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NailServiceDto>>(services);
        }

        public async Task<int> CreateAsync(CreateNailServiceDto serviceDto)
        {
            var service = _mapper.Map<NailService>(serviceDto);
            await _serviceRepository.AddAsync(service);
            return service.Id;
        }

        public async Task UpdateAsync(UpdateNailServiceDto serviceDto)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceDto.Id);
            _mapper.Map(serviceDto, service);
            await _serviceRepository.UpdateAsync(service);
        }

        public async Task DeleteAsync(int id)
        {
            await _serviceRepository.DeleteAsync(id);
        }
    }
}
