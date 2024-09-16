using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FootballLeague.Helpers
{
    public interface IBlobHelper
    {
        Task<Guid> UploadBlobAsync(IFormFile file, string containerName);
    }
}
