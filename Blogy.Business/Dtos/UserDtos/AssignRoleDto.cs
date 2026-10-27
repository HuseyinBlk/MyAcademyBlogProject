using Blogy.Business.Dtos.Common;

namespace Blogy.Business.Dtos.UserDtos;

public class AssignRoleDto : BaseDto
{
    public string RoleName { get; set; }
    public bool RoleExist { get; set; }
}
