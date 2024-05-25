﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.Contracts;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController: Controller
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor  _context;
        
    public AuthController(IUserService userService, IHttpContextAccessor  context)
    {
        _userService = userService;
        _context = context;
    }
    
    [HttpPost("/auth/login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        try
        {
            var token = await _userService.Login(request.Email, request.Password, request.RepeatPassword);
            _context.HttpContext.Response.Cookies.Append("token", token);
                
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { message = ex.Message });
        }
    }
}