using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Cars.CarTest.Mock
{
    public class MockIHostEnvironment : IHostEnvironment
    {
        public string ApplicationName { get; set; } = "Test Application";

        public IFileProvider ContentRootFileProvider { get; set; } =
            new PhysicalFileProvider(Directory.GetCurrentDirectory()); // ✅ Kasutab `System.IO.Directory`

        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
        public string EnvironmentName { get; set; } = "Development";
    }
}
