using DecentralizedSystem.Core.Entities;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Infrastructure.Repository;
using DecentralizedSystem.Middlewares;
using DecentralizedSystem.Models.Globally;
using DecentralizedSystem.Models.IdmProject;
using DecentralizedSystem.Models.MasterPackage.Request;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentralizedSystem.Services.IdmProjects
{
    public interface IIdmProjectService
    {
        Task<ErrorModel> GetListPagingAsync(MasterPackageRequestModel masterPackage);
    }

    public class IdmProjectService : IIdmProjectService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IMasterPackageRepository _masterPackageRepository;

        public IdmProjectService(IConfiguration config, IUnitOfWorkContext unitOfWork, IMasterPackageRepository masterPackageRepository)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _masterPackageRepository = masterPackageRepository;
        }

        public async Task<ErrorModel> GetListPagingAsync(MasterPackageRequestModel masterPackage)
        {
            var entityMapping = masterPackage.MapProp<MasterPackageRequestModel, MasterPackage>();
            var result = await _masterPackageRepository.ExecuteMasterPackageAsync(entityMapping);

            if (result.ErrCode != "0000")
            {
                throw new CustomException(result.ErrCode, result.ErrDesc);
            }

            return new ErrorModel
            {
                Code = result.ErrCode,
                Message = result.ErrDesc,
                Data = new PagedResultModel<IdmProjectModel>()
                {
                    Result = result.OutputDetail.DeserializeObject<List<IdmProjectModel>>(),
                    TotalCount = 0
                }
            };
        }
    }
}
