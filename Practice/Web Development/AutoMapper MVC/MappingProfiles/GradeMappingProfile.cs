using AutoMapper;
using AutoMapperMVC.DTOs;
using AutoMapperMVC.Models;

namespace AutoMapperMVC.MappingProfiles
{
    /// <summary>
    /// AutoMapper profile for Grade-related mappings
    /// This handles the conversion between Grade entities and their corresponding DTOs
    /// Notice the more complex mapping for GradeDTO which includes student information
    /// </summary>
    public class GradeMappingProfile : Profile
    {
        public GradeMappingProfile()
        {
            // Mapping from Grade entity to GradeDTO for display purposes
            // This includes student information for convenience in the UI
            CreateMap<Grade, GradeDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GradeID))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.GradeValue))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : "Unknown"));

            // Mapping from GradeCreateDTO to Grade entity for creating new grades
            // We let Entity Framework handle the auto-generated ID
            CreateMap<GradeCreateDTO, Grade>()
                .ForMember(dest => dest.GradeID, opt => opt.Ignore()) // Auto-generated
                .ForMember(dest => dest.Student, opt => opt.Ignore()) // Navigation property handled by EF
                .AfterMap((src, dest) =>
                {
                    // Calculate letter grade if not provided
                    // This shows how we can add custom logic during mapping
                    if (string.IsNullOrEmpty(dest.LetterGrade))
                    {
                        dest.LetterGrade = dest.CalculateLetterGrade();
                    }
                });

            // Reverse mapping for editing scenarios
            CreateMap<GradeDTO, Grade>()
                .ForMember(dest => dest.GradeID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.GradeValue, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.Student, opt => opt.Ignore()); // Don't overwrite navigation property
        }
    }
}
