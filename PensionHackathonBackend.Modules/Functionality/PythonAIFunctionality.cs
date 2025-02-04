using System;
using System.Diagnostics;

namespace PensionHackathonBackend.Modules.Functionality;

public static class PythonAIFunctionality
{
    public static string ExecuteScript(string pythonExeFilePath, string pythonScript, string filePath)
    {
        ProcessStartInfo start = new ProcessStartInfo
        {
            FileName = pythonExeFilePath,
            Arguments = $"{pythonScript} \"{filePath}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(start))
        {
            string result = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            
            return result;
        }
    }
}