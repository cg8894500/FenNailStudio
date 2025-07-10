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
    public class TechnicianService : ITechnicianService
    {
        private readonly ITechnicianRepository _technicianRepository;
        private readonly IMapper _mapper;

        public TechnicianService(ITechnicianRepository technicianRepository, IMapper mapper)
        {
            _technicianRepository = technicianRepository;
            _mapper = mapper;
        }

        public async Task<TechnicianDto> GetByIdAsync(int id)
        {
            var technician = await _technicianRepository.GetByIdAsync(id);
            return _mapper.Map<TechnicianDto>(technician);
        }

        public async Task<IEnumerable<TechnicianDto>> GetAllAsync()
        {
            var technicians = await _technicianRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TechnicianDto>>(technicians);
        }

        public async Task<int> CreateAsync(CreateTechnicianDto technicianDto)
        {
            var technician = _mapper.Map<Technician>(technicianDto);
            await _technicianRepository.AddAsync(technician);
            return technician.Id;
        }

        public async Task UpdateAsync(UpdateTechnicianDto technicianDto)
        {
            var technician = await _technicianRepository.GetByIdAsync(technicianDto.Id);
            _mapper.Map(technicianDto, technician);
            await _technicianRepository.UpdateAsync(technician);
        }

        public async Task DeleteAsync(int id)
        {
            await _technicianRepository.DeleteAsync(id);
        }
    }
}
