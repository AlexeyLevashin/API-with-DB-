using FirstRestAPI.Services.Minio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;

namespace FirstRestAPI.Services.Minio;

public class MinioService : IMinioService
{
    private readonly IMinioSettings _minioSettings;
    private readonly IMinioClient _minioClient;

    public MinioService(IMinioSettings minioSettings)
    {
        _minioSettings = minioSettings;
        _minioClient = new MinioClient()
            .WithEndpoint(minioSettings.Endpoint)
            .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
            .WithSSL(minioSettings.Secure)
            .Build();
    }

    public async Task UploadFile(string objectName, Stream fileStream)
    {
        await _minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(_minioSettings.BucketName)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType("application/octet-stream")
        );

        await _minioClient.StatObjectAsync(
            new StatObjectArgs().WithBucket(_minioSettings.BucketName).WithObject(objectName));
    }

    public async Task DeleteFile(string objectName)
    {
        await _minioClient.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_minioSettings.BucketName)
                .WithObject(objectName));
    }
}