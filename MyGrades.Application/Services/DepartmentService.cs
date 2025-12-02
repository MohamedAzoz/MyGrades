using AutoMapper;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.Department;
using MyGrades.Application.Contracts.Repositories;
using MyGrades.Application.Contracts.Services;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DepartmentService(IUnitOfWork _unitOfWork,IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        public async Task<Result> Create(DepartmentDto departmentDto)
        {
            var department = mapper.Map<Department>(departmentDto);
            if (department == null)
            {
                return Result.Failure("Invalid department data.",404);
            }
            await unitOfWork.Departments.AddAsync(department);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> CreateRang(List<DepartmentDto> departments)
        {
            var departmentEntities = mapper.Map<List<Department>>(departments);
            await unitOfWork.Departments.AddRangeAsync(departmentEntities);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> Delete(int id)
        {
            var department = await unitOfWork.Departments.GetByIdAsync(id);
            if (department == null || department.Data==null)
            {
                return Result.Failure("Department not found.", 404);
            }

            await unitOfWork.Departments.DeleteAsync(department.Data);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        

        public async Task<Result<Department>> GetById(int id)
        {
            var department = await unitOfWork.Departments.GetByIdAsync(id);
            if (department == null || department.Data==null)
            {
                return Result<Department>.Failure("Department not found.", 404);
            }

            var departmentDto = mapper.Map<Department>(department.Data);
            return Result<Department>.Success(departmentDto);
        }
        

        public async Task<Result<List<Department>>> GetAll()
        {
            var departments = await unitOfWork.Departments.GetAllAsync();
            if (departments == null || departments.Data==null || departments.Data.Count==0)
            {
                return Result<List<Department>>.Failure("No departments found.", 404);
            }
            //var departmentDtos = mapper.Map<List<DepartmentDto>>(departments);
            return Result<List<Department>>.Success(departments.Data.ToList());
        }
        

        public async Task<Result<Department>> Update(DepartmentDto departmentDto)
        {
            //var department = await unitOfWork.Departments.GetByIdAsync(departmentDto);
            //if (department == null)
            //{
            //    return Result<>.Failure("Department not found.", 404);
            //}

            //mapper.Map(departmentDto, department);
            //await unitOfWork.SaveChangesAsync();
            //return Result.Success(department);
            throw new NotImplementedException();
        }
        
}
}
