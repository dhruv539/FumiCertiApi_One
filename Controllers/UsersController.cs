﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FumicertiApi.Data;
using FumicertiApi.Models;
using FumicertiApi.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Sieve.Services;
using Sieve.Models;

namespace FumicertiApi.Controllers
{
    [Authorize]
    [ApiController]

    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        public UsersController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        // ✅ GET: api/users
        [HttpGet]
        public async Task<ActionResult> GetUsers([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var usersQuery = _context.Users
                .Include(u => u.Role)
                .AsNoTracking();

            var filteredUsers = _sieveProcessor.Apply(sieveModel, usersQuery, applyPagination: false);
            var totalRecords = await filteredUsers.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedUsers = filteredUsers
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            var users = await pagedUsers.ToListAsync();

            var result = users.Select(u => new UserReadDTo
            {
                UserId = u.UserId,
                UserFirstName = u.UserFirstName,
                UserLastName = u.UserLastName,
                UserEmail = u.UserEmail,
                UserName = u.UserName,
                UserPhone = u.UserPhone,
                UserCountryCode = u.UserCountryCode,
                UserMobile = u.UserMobile,
                UserStatus = (byte)u.UserStatus,
                UserRoleId = u.UserRoleId,
                UserRoleName = u.Role?.RoleName ?? "-",
                UserAddress = u.UserAddress,
                UserCompanyId = u.UserCompanyId
            });

            return Ok(new
            {
             
                pagination = new
                {
                    page = currentPage,
                    pageSize = pageSize,
                    totalRecords = totalRecords,
                    totalPages = totalPages
                },
                data = result
            });
        }



        // ✅ GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTo>> GetUserbyId(string id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            var result = new UserReadDTo
            {
                UserId = user.UserId,
                UserFirstName = user.UserFirstName,
                UserLastName = user.UserLastName,
                UserEmail = user.UserEmail,
                UserName = user.UserName,
                UserPhone = user.UserPhone,
                UserCountryCode = user.UserCountryCode,
                UserMobile = user.UserMobile,
                UserStatus = (byte)user.UserStatus,
                UserRoleId = user.UserRoleId,
                UserRoleName = user.Role?.RoleName ?? "",
                UserAddress = user.UserAddress,
                UserBranchId = user.UserBranchId,
                UserCompanyId = user.UserCompanyId

            };

            return Ok(result);
        }

        // ✅ POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(UserAddDto dto)
        {
            var user = new User
            {
               UserId = Guid.NewGuid().ToString(),
                UserRoleId = dto.UserRoleId,
                UserParentId = dto.UserParentId,
                UserFirstName = dto.UserFirstName,
                UserLastName = dto.UserLastName,
                UserEmail = dto.UserEmail,               
                UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword),
                UserZipcode = dto.UserZipcode,
                UserPhone = dto.UserPhone,
                UserCountryCode = dto.UserCountryCode,
                UserMobile = dto.UserMobile,
                UserImage = dto.UserImage,
                UserAddress = dto.UserAddress,
                UserBranchId= GetBranchId(),
                UserCompanyId = GetCompanyId(), // Set if available
                UserName = dto.UserName,
                UserStatus = 1, // default active
                UserCreated = DateTime.UtcNow,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserbyId), new { id = user.UserId }, user);
        }

        // ✅ PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserUpdateDto dto)
        {
            if (id != dto.UserId)
                return BadRequest("ID mismatch");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.UserRoleId = dto.UserRoleId;
            user.UserId = dto.UserId;
            user.UserFirstName = dto.UserFirstName;
            user.UserLastName = dto.UserLastName;
            user.UserEmail = dto.UserEmail;
            user.UserPhone = dto.UserPhone;
            user.UserCountryCode = dto.UserCountryCode;
            user.UserMobile = dto.UserMobile;
            user.UserAddress = dto.UserAddress;
           
            user.UserStatus = dto.UserStatus;
     
            user.UserName = dto.UserName;
            user.UserCompanyId =GetCompanyId();

            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // ✅ DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
