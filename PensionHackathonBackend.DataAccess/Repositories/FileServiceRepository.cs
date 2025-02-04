using Microsoft.EntityFrameworkCore;
using PensionHackathonBackend.Core.Abstractions;
using PensionHackathonBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace PensionHackathonBackend.DataAccess.Repositories
{
    /* Репозиторий файла CSV для дальнейшей реализации CRUD запросов */
    public class FileServiceRepository(PensionHackathonDbContext context, IMemoryCache memoryCache) : IFileServiceRepository
    {
        private readonly PensionHackathonDbContext _context = context;
        private readonly IMemoryCache _cache = memoryCache;
        
        private const string GetFilesCacheKey = "GetFilesCache";
        
        public async Task<int> AddFileRecordAsync(FileRecord fileRecord)
        {
            await _context.FileRecords.AddAsync(fileRecord);
            await _context.SaveChangesAsync();
            
            _cache.Remove(GetFilesCacheKey);

            
            return fileRecord.Id;
        }

        public async Task<FileRecord?> GetFileRecordAsync(int fileId)
        {
            return await _context.FileRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == fileId);
        }

        public async Task<List<FileRecord>> GetFilesAsync()
        {
            if (!_cache.TryGetValue(GetFilesCacheKey, out List<FileRecord> files))
            {
                files = await _context.FileRecords
                    .AsNoTracking()
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                
                _cache.Set(GetFilesCacheKey, files, cacheOptions);
            }

            return files;
        }

        public async Task DeleteFileRecordAsync(int fileId)
        {
            var fileRecord = await _context.FileRecords.FindAsync(fileId);
            if (fileRecord != null)
            {
                _context.FileRecords.Remove(fileRecord);
                await _context.SaveChangesAsync();
        
                _cache.Remove(GetFilesCacheKey);
            }
        }
    }
}
