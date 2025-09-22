using AutoMapper;
using MyWebAPI.DTOs;
using MyWebAPI.Models;

namespace MyWebAPI.MapProfile;

public class MapProfile : Profile
{
	public MapProfile() 
	{
		CreateMap<Category, CategoryDTO>().ReverseMap();
	}
}
