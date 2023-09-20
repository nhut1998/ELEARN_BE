using Dapper;
using DecentralizedSystem.Core.Entities;
using DecentralizedSystem.Helpers;
using DecentralizedSystem.Infrastructure.Repository;
using DecentralizedSystem.Models.CourseManagemet;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DecentralizedSystem.Services.CourseManagement
{
    public interface ICourseLikeService
    {
        /// <summary>
        /// Like khóa học
        /// </summary>
        /// <param name="likes">data input</param>
        /// <returns></returns>
        public Task<string> CourseLike(LikeModel likes);

        public Task<string> DisCourseLike(LikeModel likes);

        public Task Likes(string CourseId);

    }
    public class CourseLikeService : ICourseLikeService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly ILikeRepository _likeRepository;

        public CourseLikeService(IUnitOfWorkContext unitOfWork, ILikeRepository likeRepository, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _likeRepository = likeRepository;
            _config = config;
        }

        public async Task Likes(string CourseId)
        {
            await this._likeRepository.LikeAsync(CourseId);
        }

        public async Task<string> DisCourseLike(LikeModel likes)
        {
            var isLike = await this._likeRepository.GetLikeAsync(likes.CourseId, likes.UserId);
            if (isLike == null)
            {
                throw new ArgumentException("User đã unlike khóa học");
            }
            await this._likeRepository.DisLikeAsync(likes.CourseId);
                await this._likeRepository.DeleteLikeAsync(likes.CourseId, likes.UserId);
                return likes.CourseId;
        }
        public async Task<string> CourseLike(LikeModel likes)
        {
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    var isLike = await this._likeRepository.GetLikeAsync(likes.CourseId, likes.UserId);
                    if(isLike != null)
                    {
                        throw new ArgumentException("User đã like khóa học");
                    }
                    await this._likeRepository.AddLikeAsync(likes.MapProp<LikeModel, Like>());
                    await this.Likes(likes.CourseId);
                    await uow.CommitAsync();
                   return likes.CourseId;

                }
                catch (Exception)
                {
                    await uow.RollBackAsync();
                    throw;
                }
           }

        }


    }
}
