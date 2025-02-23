using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstRestAPI.Services.Minio.Interfaces;

public interface IMinioService
{
    Task UploadFile(string objectName, Stream fileStream);
    Task DeleteFile(string objectName);
}