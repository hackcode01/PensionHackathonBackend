using PensionHackathonBackend.Application.Interfaces;
using PensionHackathonBackend.Core.Abstractions;
using PensionHackathonBackend.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PensionHackathonBackend.Modules.Functionality;

namespace PensionHackathonBackend.Application.Services
{
    /* Класс сервиса файла CSV по реализации репозитория файла CSV */
    public class FileService(IConfiguration config, IWebHostEnvironment environment, IFileServiceRepository fileServiceRepository) : IFileService
    {
        private readonly IConfiguration _config = config;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IFileServiceRepository _fileServiceRepository = fileServiceRepository;
    
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) throw new ArgumentException("File is invalid.");

            string zipUploadFile = Path.Combine(_environment.WebRootPath, "files");

            // Создаем директории, если их нет
            if (!Directory.Exists(zipUploadFile))
            {
                Directory.CreateDirectory(zipUploadFile);
            }

            // Генерация имени файла
            var fileRecord = FileRecord.Create(file.FileName, DateTime.Today);
            var fileName = $"{fileRecord.fileRecord.Id}_{fileRecord.fileRecord.FileName}";
            
            
            string filePath = Path.Combine(zipUploadFile, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _fileServiceRepository.AddFileRecordAsync(fileRecord.fileRecord);
        
           var pythonScriptPath = Path.Combine(_environment.WebRootPath, "python");
            // Выполнение Python-скрипта
            var aiResult =  PythonAIFunctionality.ExecuteScript(_config["PythonExeFilePath"], pythonScriptPath, filePath);
            
            return aiResult;
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var fileRecord = await _fileServiceRepository.GetFileRecordAsync(fileId);
            if (fileRecord == null) throw new FileNotFoundException("File not found in the database.");

            string filePath = Path.Combine(_environment.WebRootPath, "images", $"{fileRecord.Id}_{fileRecord.FileName}");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await _fileServiceRepository.DeleteFileRecordAsync(fileId);
        }

        public async Task<List<FileRecord>> GetFilesAsync()
        {
            return await _fileServiceRepository.GetFilesAsync();
        }

        public async Task<FileRecord> GetFileByIdAsync(int fileId)
        {
            return await _fileServiceRepository.GetFileRecordAsync(fileId);
        }
    }
}
