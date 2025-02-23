using FirstRestAPI.Services.Minio.Interfaces;

namespace FirstRestAPI.Services.Minio;

public class MinioSettings:IMinioSettings
{
    public MinioSettings(IConfiguration configuration)
    {
        Endpoint = configuration["ConnectionStrings:Minio:Endpoint"];
        Secure = configuration["ConnectionStrings:Minio:Secure"] == "true";
        AccessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY");
        SecretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY");
        BucketName = Environment.GetEnvironmentVariable("MINIO_BUCKET");
    }
}