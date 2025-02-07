namespace FirstRestAPI.Domain;

public class DomImage
{
    public int ImageId { get; set; }
    public int PostId { get; set; }
    public string ImageName { get; set; }
    public DateTime CreatedAt { get; set; }
}