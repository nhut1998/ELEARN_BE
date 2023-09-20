using DecentralizedSystem.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DecentralizedSystem.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionDict = new Dictionary<DatabaseConnectionName, string>
            {
                { DatabaseConnectionName.Connection1, configuration.GetConnectionString("DefaultConnection") },
                { DatabaseConnectionName.Connection2, configuration.GetConnectionString("DefaultConnection") }
            };
            // Inject this dict
            services.AddSingleton<IDictionary<DatabaseConnectionName, string>>(connectionDict);

            // Inject the factory
            services.AddTransient<IDbConnectionFactory, DapperDbConnectionFactory>();
            services.AddTransient<IUnitOfWorkContext, UnitOfWorkContext>();
            services.AddTransient<IConnectionContext, UnitOfWorkContext>();

            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IPermitRepository, PermitRepository>();
            services.AddTransient<IAccountTranFccRepository, AccountTranFccRepository>();
            services.AddTransient<IAccountClassFccRepository, AccountClassFccRepository>();
            services.AddTransient<IAccountClassFccRepository, AccountClassFccRepository>();
            services.AddTransient<IUserInfoRepository, UserInfoRepository>();
            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IUserManagementRepository, UserManagementRepository>();
            services.AddTransient<ICourseManagementRepository, CourseManagementRepository>();
            services.AddTransient<ILikeRepository, CourseLikeRepository>();

            services.AddTransient<IMasterPackageRepository, MasterPackageRepository>();
        }
    }
}
