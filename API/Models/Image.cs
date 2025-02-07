namespace Infrastructure.Models;

public class Image
{
    public int ImageId { get; set; }
    public int PostIdIm { get; set; }
    public string ImageName { get; set; }
    public DateTime CreatedAt { get; set; }
}