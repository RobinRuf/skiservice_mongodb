using skiservice.Dto;
using skiservice.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace skiservice.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Authenticate a user and return a JWT token
        /// </summary>
        /// <param name="userService">The user login Data containing the username and password</param>
        /// <param name="tokenService">A JSON result with the username and token</param>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginUserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            try
            {
                var user = _userService.GetUserByUsername(model.UserName);
                if (user == null || !_userService.VerifyPassword(model.UserName, model.Password))
                {
                    return Unauthorized("Invalid Credentials");
                }

                var token = _tokenService.CreateToken(model.UserName, user.Role.ToString());
                return Ok(new JsonResult(new
                {
                    Username = model.UserName,
                    Token = token
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new user account with the given username and password
        /// </summary>
        /// <param name="model">The user login Data containing the username and password</param>
        /// <returns>A 200 OK if the user was created successfully</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userService.CreateUser(model.UserName, model.Password, model.Role);
                return Ok($"Benutzer {model.UserName} mit der Rolle {model.Role} wurde erfolgreich erstellt.");
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("unlock")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "ADMIN,USER")]
        public IActionResult UnlockUser([FromBody] UnlockUserDto model)
        {
            try
            {
                _userService.UnlockUser(model.UserName);
                return Ok($"Benutzerkonto {model.UserName} wurde erfolgreich entsperrt.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
