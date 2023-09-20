using DecentralizedSystem.Core.Entities;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Infrastructure.Repository;
using DecentralizedSystem.Middlewares;
using DecentralizedSystem.Models.Globally;
using DecentralizedSystem.Models.MasterPackage.Request;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DecentralizedSystem.Services.MasterPackages
{
    public interface IMasterPackageServices
    {
        Task<ErrorModel> AddAsync(MasterPackageRequestModel masterPackage);

        Task<ErrorModel> UpdateAsync(MasterPackageRequestModel masterPackage);

        Task<ErrorModel> DeleteAsync(MasterPackageRequestModel masterPackage);
    }

    public class MasterPackageServices : IMasterPackageServices
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IMasterPackageRepository _masterPackageRepository;

        public MasterPackageServices(IConfiguration config, IUnitOfWorkContext unitOfWork, IMasterPackageRepository masterPackageRepository)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _masterPackageRepository = masterPackageRepository;
        }

        public async Task<ErrorModel> AddAsync(MasterPackageRequestModel masterPackage)
        {
            var entityMapping = masterPackage.MapProp<MasterPackageRequestModel, MasterPackage>();
            var result = await this._masterPackageRepository.ExecuteMasterPackageAsync(entityMapping);

            if (result.ErrCode != "0000")
            {
                throw new CustomException(result.ErrCode, result.ErrDesc);
            }

            return new ErrorModel
            {
                Code = result.ErrCode,
                Message = result.ErrDesc
            };
        }

        public async Task<ErrorModel> UpdateAsync(MasterPackageRequestModel masterPackage)
        {
            var entityMapping = masterPackage.MapProp<MasterPackageRequestModel, MasterPackage>();
            var result = await this._masterPackageRepository.ExecuteMasterPackageAsync(entityMapping);

            if (result.ErrCode != "0000")
            {
                throw new CustomException(result.ErrCode, result.ErrDesc);
            }

            return new ErrorModel
            {
                Code = result.ErrCode,
                Message = result.ErrDesc
            };
        }

        public async Task<ErrorModel> DeleteAsync(MasterPackageRequestModel masterPackage)
        {
            var entityMapping = masterPackage.MapProp<MasterPackageRequestModel, MasterPackage>();
            var result = await this._masterPackageRepository.ExecuteMasterPackageAsync(entityMapping);

            if (result.ErrCode != "0000")
            {
                throw new CustomException(result.ErrCode, result.ErrDesc);
            }

            return new ErrorModel
            {
                Code = result.ErrCode,
                Message = result.ErrDesc
            };
        }
    }
}
