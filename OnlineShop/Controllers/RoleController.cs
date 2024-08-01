using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Contracts;
using OnlineShop.DataModels;
using OnlineShop.DTOs.Roles;

namespace OnlineShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;

        public RoleController(IMapper mapper, IRoleRepository roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        [HttpPost("Add Roles")]
        public async Task<ActionResult<Role>> CreateRole(CreateRoleDto createRoleDto)
        {
            var role = _mapper.Map<Role>(createRoleDto);
            await this._roleRepository.CreateAsync(role);
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }

        [HttpGet("Get Roles")]
        public async Task<ActionResult<GetRoleDto>> GetRole(int roleId)
        {
            var role = await this._roleRepository.GetAsync(roleId);
            if (role == null)
            {
                throw new Exception($"RoleId{roleId} is not Found");
            }
            var roleDetailsDto = _mapper.Map<GetRoleDto>(role);
            return Ok(roleDetailsDto);
        }

        [HttpGet("Get All Roles")]
        public async Task<ActionResult<List<GetRoleDto>>> GetAllRoles()
        {
            var roles = await this._roleRepository.GetAllAsync();
            var records = _mapper.Map<List<GetRoleDto>>(roles);
            return Ok(records);
        }

        [HttpPut("Update Roles")]
        public async Task<ActionResult> UpdateRoles(int roleId, UpdateRoleDto updateRoleDto)
        {
            if (roleId != updateRoleDto.Id)
            {
                return BadRequest("Invalid Category Id");
            }

            var role = await _roleRepository.GetAsync(roleId);
            if (role == null)
            {
                throw new Exception($"RoleID {roleId} is not found");
            }

            _mapper.Map(updateRoleDto, role);

            try
            {
                await _roleRepository.UpdateAsync(role);
            }
            catch (Exception)
            {
                throw new Exception($"Error occured while updating roleID {roleId}.");
            }

            return NoContent();
        }

        [HttpDelete("Delete Role")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            await _roleRepository.DeleteAsync(roleId);
            return NoContent();
        }

    }
}
