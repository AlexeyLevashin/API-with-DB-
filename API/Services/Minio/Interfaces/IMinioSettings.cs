namespace FirstRestAPI.Services.Minio.Interfaces;

public class IMinioSettings
{
    public string Endpoint { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string BucketName { get; set; }
    public bool Secure { get; set; } 
}