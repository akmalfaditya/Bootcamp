using AutoMapper;
using AutoMapperMVC.DTOs;
using AutoMapperMVC.Models;

namespace AutoMapperMVC.MappingProfiles
{
    /// <summary>
    /// AutoMapper profile for Student-related mappings
    /// This is where we define how our entities map to DTOs and vice versa
    /// Profiles keep our mapping logic organized and separated by domain
    /// </summary>
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            // Mapping from Student entity to StudentDTO for client consumption
            // Notice how we're mapping some properties with different names
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentID))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Branch))
                .ForMember(dest => dest.Grades, opt => opt.MapFrom(src => src.Grades));

            // Mapping from StudentCreateDTO to Student entity for creating new students
            // AutoMapper is smart enough to map properties with the same name automatically
            // We only need to specify mappings for properties with different names
            CreateMap<StudentCreateDTO, Student>()
                .ForMember(dest => dest.StudentID, opt => opt.Ignore()) // ID is auto-generated
                .ForMember(dest => dest.Grades, opt => opt.Ignore()); // Grades are added separately

            // Reverse mapping for editing scenarios - from StudentDTO back to Student
            // This is useful when we need to update an existing student
            CreateMap<StudentDTO, Student>()
                .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Grades, opt => opt.Ignore()); // Don't overwrite grades collection
        }
    }
}
