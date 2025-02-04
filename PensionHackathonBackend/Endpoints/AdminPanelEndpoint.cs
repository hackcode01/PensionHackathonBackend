using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PensionHackathonBackend.Application.Services;
using PensionHackathonBackend.Contracts.UserResponse;

namespace PensionHackathonBackend.Endpoints;

public static class AdminPanelEndpoint
{
    public static IEndpointRouteBuilder AddAdminPanel(
        this IEndpointRouteBuilder app)
    {
        app.MapGet("GetUsers", GetUsers);
        app.MapGet("Details/{id:int}", Details);
        app.MapPost("CreateUser", CreateUser);
        app.MapPut("UpdateUser/{id:int}", UpdateUser);
        app.MapDelete("DeleteUser/{id:int}", DeleteUser);

        return app;
    }

    private static async Task<IResult> UserExists(int id, UserService userService)
    {
        try
        {
            var users = await userService.GetAllUsers();

            return Results.Ok(users.Any(u => u.Id == id));
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }

    private static async Task<IResult> GetUsers(string? sortOrder,
        string? searchString, UserService userService)
    {
        try
        {
            var users = await userService.GetAllUsers();

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Login.Contains(searchString) || u.Role.Contains(searchString)).ToList();
            }

            users = sortOrder switch
            {
                "login_desc" => users.OrderByDescending(u => u.Login).ToList(),
                "login_asc" => users.OrderBy(u => u.Login).ToList(),
                "role_desc" => users.OrderByDescending(u => u.Role).ToList(),
                "role_asc" => users.OrderBy(u => u.Role).ToList(),
                _ => users.OrderBy(u => u.Login).ToList(),
            };

            return Results.Ok(users);

        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }

    private static async Task<IResult> Details(int id, UserService userService)
    {
        try
        {
            if (string.IsNullOrEmpty(Convert.ToString(id)))
            {
                return Results.BadRequest();
            }

            var users = await userService.GetAllUsers();

            var response = users.FirstOrDefault(user => user.Id == id);
            if (response == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }

    private static async Task<IResult> CreateUser([FromBody] UserRequest request, UserService userService)
    {
        try
        {
            var (user, error) = Core.Models.User.Create(
                request.Login,
                request.Password,
                request.Role);

            if (!string.IsNullOrEmpty(error))
            {
                return Results.BadRequest(error);
            }

            var userId = await userService.CreateUser(user);

            return Results.Ok(userId);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }

    private static async Task<IResult> UpdateUser([FromBody] UserRequest request,
        int id, UserService userService)
    {
        try
        {
            var response = await userService.UpdateUser
                (id, request.Login, request.Password, request.Role);

            return Results.Ok(response);

        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }

    private static async Task<IResult> DeleteUser(int id, UserService userService)
    {
        try
        {
            var response = await userService.DeleteUser(id);

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }
}