using Cars.Core.Domain;
using Cars.Core.Dto;

namespace AutoServices.Core.ServiceInterface
{
    public interface ICarsServices
    {
        Task<Car> DetailAsync(Guid id);
        Task<Car> Update(CarDto dto);
        Task<Car> Delete(Guid id);
        Task<Car> Create(CarDto dto);
    }
}
