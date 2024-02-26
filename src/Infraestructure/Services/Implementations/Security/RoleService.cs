using AutoMapper;
using Domain.Entities.Security;
using Domain.Repositories.Security;
using Infraestructure.Data;
using Infraestructure.Dtos.Common;
using Infraestructure.Dtos.Security;
using Infraestructure.Providers;
using Infraestructure.Services.Contracts.Security;
using System;
using System.Linq.Dynamic.Core;

namespace Infraestructure.Services.Implementations.Security
{
    public class RoleService : Service<RoleDto, Role, int, IRoleRepository, ISecurityUnitOfWork>, IRoleService
    {
        private readonly IContextProvider _contextProvider;

#pragma warning disable IDE0290 // Usar constructor principal
        public RoleService(ISecurityUnitOfWork unitOfWork, IMapper mapper, IContextProvider contextProvider)
#pragma warning restore IDE0290 // Usar constructor principal
            : base(unitOfWork, mapper)
        {
            _contextProvider = contextProvider;
        }


        protected override IRoleRepository GetRepositoryFrom()
        {
            return _unitOfWork.Roles;
        }

        public async Task<RoleAddEditDto> Add(RoleAddEditDto dto)
        {
            if (dto is null) throw new Exception("There is not entity");
            if (dto.Id.HasValue) throw new Exception("Invalid Parameter Id");
            if (!_contextProvider.PermissionsId.Any(id => id == (int)Domain.Enums.Permissions.RoleAdd))
                throw new Exception("Do Not Have Permission To Add Role");

            if (string.IsNullOrWhiteSpace(dto.Name)) throw new Exception("RoleName Is Required");
            if ((await _unitOfWork.Roles.Count(x => x.Name.Equals(dto.Name, StringComparison.CurrentCultureIgnoreCase))) > 0)
                throw new Exception("RoleName Already Exists");
            if (dto.PermissionIds.Count < 1) throw new Exception("Role Must Contain At Least One Permission");

            try
            {
                var role = await _unitOfWork.TransactionallyDo(async () =>
                {
                    var newRole = new Role
                    {
                        Name = dto.Name,
                        Description = dto.Description ?? string.Empty,
                    };

                    foreach (var permissionId in dto.PermissionIds)
                    {
                        if (await _unitOfWork.Permissions.Count(x => x.Id == permissionId) == 0)
                            throw new Exception("Permission Not Found");

                        newRole.RolePermissions.Add(new RolePermission
                        {
                            PermissionId = permissionId
                        });
                    }

                    await _unitOfWork.Roles.Add(newRole);
                    return newRole;
                }, System.Data.IsolationLevel.Serializable);

                dto.Id = role.Id;

                return dto;
            }

            catch (Exception)
            {
                throw new Exception("Error Adding Role");
            }
        }

        public async Task<PagedDataDto<RoleDto>> GetPaged(RoleFilterDto filter)
        {
            if (filter is null) throw new Exception("Invalid Parameter Dto");
            if (!_contextProvider.PermissionsId.Any(id => id == (int)Domain.Enums.Permissions.RoleList))
                throw new Exception("Do Not Have Permission ToList Roles");
            filter.PageSize = filter.PageSize > 0 ? filter.PageSize : DefaultPageSize;
            filter.SortExpression = string.IsNullOrEmpty(filter.SortExpression) ?
                "NAME asc" :
                filter.SortExpression;

            var sortExpressionParts = filter.SortExpression.Split(" ");
            sortExpressionParts[0] = sortExpressionParts.First() switch
            {
                "DESCRIPTION" => "Description",
                _ => "Name",
            };
            filter.SortExpression = string.Join(" ", sortExpressionParts);

            var data = await _unitOfWork.Roles.GetPaged(
                filter.PageIndex,
                filter.PageSize,
                filter.SortExpression,
                x => string.IsNullOrWhiteSpace(filter.Name) || x.Name.Contains(filter.Name)
                ,
                x => new Role
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                });

            return _serviceMapper.Map<PagedDataDto<RoleDto>>(data);
        }

        public async Task<int> Update(RoleAddEditDto dto)
        {
            if (dto is null || !dto.Id.HasValue) throw new Exception("Invalid Parameter Dto");
            if (!_contextProvider.PermissionsId.Any(id => id == (int)Domain.Enums.Permissions.RoleEdit))
                throw new Exception("Do Not Have Permission To Edit Role");

            if (string.IsNullOrWhiteSpace(dto.Name)) throw new Exception("RoleName Is Required");
            if ((await _unitOfWork.Roles.Count(x => x.Name.Equals(dto.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != dto.Id)) > 0)
                throw new Exception("RoleName Already Exists");
            if (dto.PermissionIds.Count < 1) throw new Exception("Role Must Contain At Least One Permission");

            try
            {
                var roleId = await _unitOfWork.TransactionallyDo(async () =>
                {
                    var roleToUpdate = await _unitOfWork.Roles.GetWithPermissions(x => x.Id == dto.Id) ?? throw new Exception("Role Not Found");
                    roleToUpdate.Name = dto.Name;
                    roleToUpdate.Description = dto.Description ?? string.Empty;

                    var rolePermissionIds = roleToUpdate.RolePermissions.Select(x => x.PermissionId);
                    var permissionIdsToRemove = rolePermissionIds.Except(dto.PermissionIds);

                    // Removing permissions
                    roleToUpdate.RolePermissions = roleToUpdate.RolePermissions
                        .Where(x => !permissionIdsToRemove.Contains(x.PermissionId))
                        .ToList();

                    // Adding Permissions                    
                    foreach (var permissionId in dto.PermissionIds)
                    {
                        if (await _unitOfWork.Permissions.Count(x => x.Id == permissionId) == 0)
                            throw new Exception("Permission Not Found");

                        if (!rolePermissionIds.Contains(permissionId))
                        {
                            roleToUpdate.RolePermissions.Add(new RolePermission
                            {
                                PermissionId = permissionId
                            });
                        }
                    }

                    _unitOfWork.Roles.Update(roleToUpdate);
                    return roleToUpdate.Id;
                }, true, System.Data.IsolationLevel.Serializable); // Usando estrategia de ejecución y nivel de aislamiento Serializable

                return roleId;
            }
            catch (Exception)
            {
                throw new Exception("Error Updating Role");
            }
        }
    }
}
