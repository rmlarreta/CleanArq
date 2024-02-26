using Domain.Entities.Security;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Role = Domain.Entities.Security.Role;

namespace Infraestructure.Seeds
{
    public class SeedData
    {
        #region Contructor
        public static void Seed(ModelBuilder builder)
        {

            SeedPermissions(builder);
            SeedRoles(builder);
            SeedRolePermissions(builder);
            SeedUsers(builder);
            SeedUsersRoles(builder);
        }
        #endregion

        #region Permissions
        private static void SeedPermissions(ModelBuilder builder)
        {
            builder.Entity<Permission>().HasData(new List<Permission>()
            {
                // ----------------------------------------------------
                // Roles
                // ----------------------------------------------------
                new() { Id = (int)Permissions.RoleList, Name = Permissions.RoleList.ToString(), ModuleId = (int)Modules.Roles },
                new() { Id = (int)Permissions.RoleAdd, Name = Permissions.RoleAdd.ToString(), ModuleId = (int)Modules.Roles },
                new() { Id = (int)Permissions.RoleEdit, Name = Permissions.RoleEdit.ToString(), ModuleId = (int)Modules.Roles },
                new() { Id = (int)Permissions.RoleDelete, Name = Permissions.RoleDelete.ToString(), ModuleId = (int)Modules.Roles },
                new() { Id = (int)Permissions.RoleViewDetail, Name = Permissions.RoleViewDetail.ToString(), ModuleId = (int)Modules.Roles },
                // ----------------------------------------------------
                //Users
                // ----------------------------------------------------
                new() { Id = (int)Permissions.UserChangeRole, Name = Permissions.UserChangeRole.ToString(), ModuleId = (int)Modules.Users },
                });
        }
        #endregion

        #region Roles
        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(new List<Role>()
            {
                new() { Id = (int)Domain.Enums.Role.Administrator, Name = Domain.Enums.Role.Administrator.ToString(), Description = Domain.Enums.Role.Administrator.ToString()},
                new() { Id = (int)Domain.Enums.Role.Operator, Name = Domain.Enums.Role.Operator.ToString(), Description = Domain.Enums.Role.Operator.ToString()},

            });
        }
        #endregion

        #region RolePermissions
        private static void SeedRolePermissions(ModelBuilder builder)
        {
            builder.Entity<RolePermission>().HasData(new List<RolePermission>()
            {
                // ----------------------------------------------------
                // General Administrator
                // ----------------------------------------------------
                new() { RoleId = (int)Domain.Enums.Role.Administrator, PermissionId = (int)Permissions.RoleList },
                new() { RoleId = (int)Domain.Enums.Role.Administrator, PermissionId = (int)Permissions.RoleAdd },
                new() { RoleId = (int)Domain.Enums.Role.Administrator, PermissionId = (int)Permissions.RoleEdit },
                new() { RoleId = (int)Domain.Enums.Role.Administrator, PermissionId = (int)Permissions.RoleViewDetail },
                new() { RoleId = (int)Domain.Enums.Role.Administrator, PermissionId = (int)Permissions.RoleDelete },
                new() { RoleId = (int)Domain.Enums.Role.Administrator, PermissionId = (int)Permissions.UserChangeRole },


            });
        }
        #endregion

        #region Users
        private static void SeedUsers(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(new List<User>()
            {
                new() { Id = 1, FullName = "Ricardo Larreta", Email = "rmlarreta@gmail.com", MinutesFromUtc = -180, CreatedDate = new DateTime(2020, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), CreatedBy = "System" },
                new() { Id = 2, FullName = "Ricardo Larreta Op", Email = "rmlarretaop@gmail.com", MinutesFromUtc = -180, CreatedDate = new DateTime(2020, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), CreatedBy = "System"},
            });
        }
        #endregion

        #region UsersRoles
        private static void SeedUsersRoles(ModelBuilder builder)
        {
            builder.Entity<UserRole>().HasData(new List<UserRole>()
            {
                new() { UserId = 1, RoleId = (int)Domain.Enums.Role.Administrator },
                new() { UserId = 2, RoleId = (int)Domain.Enums.Role.Operator },
            });
        }
        #endregion
    }
}
