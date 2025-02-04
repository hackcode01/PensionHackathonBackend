using PensionHackathonBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PensionHackathonBackend.Core.Abstractions
{
    /* Интерфейс пользователя для облегчения добавления новых методов */
    public interface IFileServiceRepository
    {
        Task<int> AddFileRecordAsync(FileRecord fileRecord);
        Task<FileRecord> GetFileRecordAsync(int fileId);
        Task<List<FileRecord>> GetFilesAsync();
        Task DeleteFileRecordAsync(int fileId);
    }
}