using FluentValidation;
using Hospital.Application.DTOs.Specialization;
using Hospital.Application.Services.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Repositories.Interfaces;

namespace Hospital.Application.Services.Implementations
{
    public class SpecializationService : ISpecializationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateSpecializationDto> _createSpecializationValidator;
        private readonly IValidator<UpdateSpecializationDto> _updateSpecializationValidator;


        public SpecializationService(IUnitOfWork unitOfWork, IValidator<CreateSpecializationDto> createSpecializationValidator, IValidator<UpdateSpecializationDto> updateSpecializationValidator)
        {
            _unitOfWork = unitOfWork;
            _createSpecializationValidator = createSpecializationValidator;
            _updateSpecializationValidator = updateSpecializationValidator;
        }

        public async Task CreateSpecialization(CreateSpecializationDto model)
        {
            var result = _createSpecializationValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var nameExists = await _unitOfWork.Specializations
                .GetAsync(
                filter: s => s.Name == model.Name
                );

            var descriptionExists = await _unitOfWork.Specializations
                .GetAsync(
                filter: s => s.Description == model.Description
                );

            if (nameExists != null)
                throw new Exception("A specialization with the same name already exists");

            if (descriptionExists != null)
                throw new Exception("A specialization with the same description already exists");

            var specialization = new Specialization
            {
                Name = model.Name,
                Description = model.Description
            };

            await _unitOfWork.Specializations.AddAsync(specialization);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteSpecialization(int id)
        {
            var specialization = await _unitOfWork.Specializations
                .GetAsync(
                filter: s => s.SpecializationId == id
                );

            if (specialization == null)
                throw new Exception("Specialization not found");

            await _unitOfWork.Specializations.Delete(specialization);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<GetSpecializationDto>> GetAllSpecializations()
        {
            var specializations = await _unitOfWork.Specializations
                .GetAllAsync(
                filter: s => true,
                selector: s => new GetSpecializationDto
                {
                    Id = s.SpecializationId,
                    Name = s.Name,
                    Description = s.Description
                });

            return specializations;
        }

        public async Task UpdateSpecialization(int id, UpdateSpecializationDto model)
        {
            var result = _updateSpecializationValidator.Validate(model);

            if (!result.IsValid)
                throw new Exception(result.ToString(","));

            var specialization = await _unitOfWork.Specializations
                .GetAsync(
                filter: s => s.SpecializationId == id
                );

            if (specialization.Name == model.Name)
                throw new Exception("The new name is the same as the old name");

            if (specialization.Description == model.Description)
                throw new Exception("The new description is the same as the old description");

            var nameExists = await _unitOfWork.Specializations
                .GetAsync(
                filter: s => s.Name == model.Name
                );

            var descriptionExists = await _unitOfWork.Specializations
                .GetAsync(
                filter: s => s.Description == model.Description
                );

            if (nameExists != null)
                throw new Exception("A specialization with the same name already exists");

            if (descriptionExists != null)
                throw new Exception("A specialization with the same description already exists");

            specialization.Name = model.Name;
            specialization.Description = model.Description;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
