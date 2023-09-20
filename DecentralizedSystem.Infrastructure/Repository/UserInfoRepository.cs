using DecentralizedSystem.Core.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace DecentralizedSystem.Infrastructure.Repository
{
    public interface IUserInfoRepository
    {
        Task<IReadOnlyList<UserInfo>> GetListAsync(string roleId, string branchId, int? activeFlag);
        Task<IReadOnlyList<UserInfo>> GetListByFCCAsync(string roleFcc, string branchId, int? activeFlag);
        Task<UserInfo> GetUserInfoByID(string userID);
        Task<UserInfo> GetUserInfoByName(string userName);
    }
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;

        public UserInfoRepository(IConnectionContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IReadOnlyList<UserInfo>> GetListAsync( string roleId, string branchId, int? activeFlag)
        {
            var sql = @"SELECT T.*, h.CHUCDANHGOC AS TITLE
                        FROM ORG_USER T LEFT join huv_profile h on t.id = h.msnv
                        WHERE (:V_ROLE_ID IS NULL OR T.ROLE_ID = :V_ROLE_ID)
                           AND (:V_BRANCH_ID IS NULL OR T.BRANCH_ID = :V_BRANCH_ID)
                           AND ((:V_ACTIVE_FLAG IS NULL AND T.ACTIVE_FLAG = 1) OR
                               T.ACTIVE_FLAG = :V_ACTIVE_FLAG)
                           AND T.ROLE_FCC IS NOT NULL";

            var result = (await _context.Connection.QueryAsync<UserInfo>(sql, new
            {
                V_ROLE_ID = roleId,
                V_BRANCH_ID = branchId,
                V_ACTIVE_FLAG = activeFlag
            })).ToList();

            return result;
        }

        public async Task<IReadOnlyList<UserInfo>> GetListByFCCAsync(string roleFcc, string branchId, int? activeFlag)
        {
            var sql = @"SELECT T.*, h.CHUCDANHGOC AS TITLE
                        FROM ORG_USER T LEFT join huv_profile h on t.id = h.msnv
                        WHERE (:V_ROLE_FCC IS NULL OR T.ROLE_FCC = :V_ROLE_FCC)
                           AND (:V_BRANCH_ID IS NULL OR T.BRANCH_ID = :V_BRANCH_ID)
                           AND ((:V_ACTIVE_FLAG IS NULL AND T.ACTIVE_FLAG = 1) OR
                               T.ACTIVE_FLAG = :V_ACTIVE_FLAG)
                           AND T.ROLE_FCC IS NOT NULL";

            var result = (await _context.Connection.QueryAsync<UserInfo>(sql, new
            {
                V_ROLE_FCC = roleFcc,
                V_BRANCH_ID = branchId,
                V_ACTIVE_FLAG = activeFlag
            })).ToList();

            return result;
        }

        public async Task<UserInfo> GetUserInfoByID(string userID)
        {
            var sql = @"SELECT T.*, h.CHUCDANHGOC AS TITLE
                        FROM ORG_USER T LEFT join huv_profile h on t.id = h.msnv
                        WHERE (T.ID = :V_USER_ID) AND T.ACTIVE_FLAG = 1";

            var result = (await _context.Connection.QueryAsync<UserInfo>(sql, new { V_USER_ID = userID })).FirstOrDefault();
            return result;
        }

        public async Task<UserInfo> GetUserInfoByName(string userName)
        {
            var sql = @"SELECT T.*,
                               H.CHUCDANHGOC AS TITLE,
                               B.NAME AS BRANCH_NAME,
                               H.DIACHIDV_GOC AS ADDRESS,
                               PB.CODE AS PARENT_BRANCH_CODE,
                               PB.NAME AS PARENT_BRANCH_NAME,
                               (SELECT CASE
                                         WHEN COUNT(1) > 1 THEN
                                          1
                                         ELSE
                                          0
                                       END
                                  FROM ORG_BRANCH TT
                                 WHERE TT.PARENT_CODE = B.CODE) AS IS_PARENT
                          FROM ORG_USER T
                          LEFT JOIN HUV_PROFILE H
                            ON T.USER_NAME = UPPER(H.USER_NAME)
                          LEFT JOIN ORG_BRANCH B
                            ON T.BRANCH_ID = B.CODE
                          LEFT JOIN ORG_BRANCH PB
                            ON B.PARENT_ID = PB.ID
                         WHERE (T.USER_NAME = :V_USER_NAME)
                           AND T.ACTIVE_FLAG = 1
                        ";

            var result = (await _context.Connection.QueryAsync<UserInfo>(sql, new { V_USER_NAME = userName })).FirstOrDefault();
            return result;
        }
    }
}
