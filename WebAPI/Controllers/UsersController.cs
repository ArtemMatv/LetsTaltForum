using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using BLL.Interfaces;
using BLL.Models;
using BLL.Models.FormsToFill;
using BLL.Exceptions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, IDisposable
    {
        private readonly IUsersService _userService;
        private bool _disposed;

        public UsersController(IUsersService service)
        {
            _userService = service;
            _disposed = false;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var users = await _userService.GetAllAsync().ConfigureAwait(false);
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserModel>> GetUserByUsername(string username)
        {
            var user = await _userService.GetAsync(username).ConfigureAwait(false);

            if (int.TryParse(username, out int id)
                && user == null)
            {
                user = await _userService.GetAsync(id).ConfigureAwait(false);
            }

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            UserModel user;
            try
            {
                user = await _userService.Register(model).ConfigureAwait(false);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            UserModel user = null;
            try
            {
                user = await _userService.Authenticate(model).ConfigureAwait(false);
            }
            catch (AccessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
             

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpDelete("{username}")]
        [Authorize]
        public async Task<ActionResult<UserModel>> Delete(string username, UserModel user)
        {
            if (user == null
                || username != user.UserName)
                return BadRequest();

            try
            {
                await _userService.DeleteAccount(user.UserName).ConfigureAwait(false);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{username}")]
        [Authorize]
        public async Task<IActionResult> Update(string username, UserModel user)
        {
            if (user == null
                || username != user.UserName)
                return  BadRequest();

            await _userService.UpdateAccount(user).ConfigureAwait(false);
            
            return Ok();
        }

        [HttpPut("{username}/admin")]
        [Authorize]
        public async Task<IActionResult> UpdateByAdmin(string username, UserModel user)
        {

            try
            {
                await _userService.UpdateAccountByAdmin(user, username).ConfigureAwait(false);
            }
            catch (AccessException ex)
            {
                //return BadRequest(ex.Message);
            }

            return Ok();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _userService.Dispose();
                }
            }
            _disposed = true;
        }
    }
}