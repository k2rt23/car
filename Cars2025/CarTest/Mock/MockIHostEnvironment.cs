using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Cars.CarTest.Mock
{
    public class MockIHostEnvironment : IHostEnvironment
    {
        public string ApplicationName { get; set; } = "Test Application";
        public IFileProvider ContentRootFileProvider { get; set; } = new PhysicalFileProvider(System.IO.Directory.GetCurrentDirectory());
        public string ContentRootPath { get; set; } = System.IO.Directory.GetCurrentDirectory();
        public string EnvironmentName { get; set; } = "Development";
    }
}
