using AutoMapper;
using MyGrades.Application.Contracts.DTOs;
using MyGrades.Application.Contracts.DTOs.AcademicYear;
using MyGrades.Application.Contracts.DTOs.Department;
using MyGrades.Application.Contracts.DTOs.Grade;
using MyGrades.Application.Contracts.DTOs.Subject;
using MyGrades.Application.Contracts.DTOs.User.Assistant;
using MyGrades.Application.Contracts.DTOs.User.Student;
using MyGrades.Application.Contracts.Projections_Models.Grades;
using MyGrades.Domain.Entities;

namespace MyGrades.Application.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {

            #region AcademicLevel Mappings

            CreateMap<AcademicLevelDto, AcademicLevel>()
                .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.LevelName))
                .ReverseMap();

            #endregion

            #region User Mappings

            CreateMap<UserExcelDto, AppUser>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.NationalId}@mygrades.edu"))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                .ReverseMap();

            #endregion

            #region Assistant Mappings

            CreateMap<AssistantExcelDto, AppUser>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.NationalId}@mygrades.edu"))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                .ReverseMap();

            CreateMap<Assistant, AssistantModelData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User!.FullName))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.User!.NationalId))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department!.Name));


            CreateMap<AssistantExcelDto, AssistantModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));

            #endregion

            #region Student Mappings

            CreateMap<Student, StudentModelData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User!.FullName))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.User!.NationalId))
                .ForMember(dest => dest.AcademicLevel, opt => opt.MapFrom(src => src.AcademicLevel.LevelName))
                .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.AppUserId))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<Student,StudentModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User!.FullName))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.User!.NationalId))
                .ForMember(dest => dest.AcademicLevelId, opt => opt.MapFrom(src => src.AcademicLevelId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));
            
            CreateMap<StudentCreateModel, Student>()
                .ForMember(dest => dest.AcademicLevelId, opt => opt.MapFrom(src => src.AcademicLevelId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));

            CreateMap<Student, UserExcelDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.User.NationalId));

            CreateMap<StudentCreateModel, AppUser>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => $"{src.NationalId}@mygrades.edu"))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true));
            
            //CreateMap< Student, GradeModel>()
            //    .ForMember(dest => dest.StudentFullName, opt => opt.MapFrom(src => src.User.FullName))
            //    .ForMember(dest => dest.StudentNationalId, opt => opt.MapFrom(src => src.User.NationalId));

            CreateMap<StudentSubject, CreateStudentSubjectDto>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                .ReverseMap();

            #endregion

            #region Department Mappings

            CreateMap<DepartmentDto, Department>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            #endregion

            #region Subject Mappings

            CreateMap<SubjectDto, Subject>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.AssistantId, opt => opt.MapFrom(src => src.AssistantId))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.AcademicLevelId, opt => opt.MapFrom(src => src.AcademicLevelId))
                .ReverseMap();

            CreateMap<UpdateSubjectDto, Subject>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.AssistantId, opt => opt.MapFrom(src => src.AssistantId))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.AcademicLevelId, opt => opt.MapFrom(src => src.AcademicLevelId))
                .ReverseMap();

            #endregion

            #region Grade Mappings
            //XXXXXX______XXXXXXXXXXXXXX_______XXXXXXXXX
            CreateMap<GradeExcelDto, Grade>()
                .ForMember(dest => dest.Practical, opt => opt.MapFrom(src => src.Practical))
                .ForMember(dest => dest.Attendance, opt => opt.MapFrom(src => src.Attendance))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks))
                //.ForMember(dest => dest.StudentSubject.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ReverseMap();

            //CreateMap<Grade , GradeModel>()
            //    .ForMember(dest => dest.Grade.Practical, opt => opt.MapFrom(src => src.Practical))
            //    .ForMember(dest => dest.Grade.Attendance, opt => opt.MapFrom(src => src.Attendance))
            //    .ForMember(dest => dest.Grade.Tasks, opt => opt.MapFrom(src => src.Tasks))
            //    .ForMember(dest => dest.Grade.StudentId, opt => opt.MapFrom(src => src.StudentId))
            //    .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));

            #endregion

        }
    }
}
