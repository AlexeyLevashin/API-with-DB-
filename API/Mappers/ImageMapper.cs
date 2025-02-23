using Applications.DTO.Images.Requests;
using FirstRestAPI.Domain;
using Infrastructure.Models;
using FirstRestAPI.Mappers;

namespace FirstRestAPI.Mappers;

public static class ImageMapper
{
    public static DomImage MapToDomain(this Image Image)
    {
        return new DomImage
        {
            ImageId = Image.ImageId,
            ImageName = Image.ImageName,
            CreatedAt = Image.CreatedAt
        };
    }

    public static ImageResponseDTO MapToDto(this DomImage image)
    {
        return new ImageResponseDTO
        {
            Id = image.ImageId, ImageUrl = $"http://localhost:9000/bucket/{image.ImageName}", CreatedAt = image.CreatedAt
        };
    }
 
    public static List<ImageResponseDTO> MapToDto(List<DomImage> images)
    {
        return images.Select(image => image.MapToDto()).ToList();
        
    }

    public static List<DomImage> MapToDomain(this List<Image> Images)
    {
        return Images.Select(i => i.MapToDomain()).ToList();
    }
    


}