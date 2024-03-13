using System;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.ActionFilters;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
[ApiController]
[Produces("application/json")]
[LogActionFilter]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;


    [HttpGet]
    public ViewResult List(int opt)
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        });

        if (opt == 1)
        {
            items = items.Where(x => x.IsActive == true).ToList();
        }
        else if (opt == 2)
        {
            items = items.Where(x => x.IsActive == false).ToList();
        }

        var model = new UserListViewModel
        {
            Items = items.OrderBy(x => x.Id).ToList()
        };

        return View(model);
    }
    [HttpPost]
    [Route("CreateUser")]
    public bool CreateUser(User user)
    {
        long id = _userService.GetAll().OrderBy(x => x.Id).Last().Id + 1;

        user.Id = id;

        _userService.Create(user);

        return true;
    }

    [HttpGet]
    [Route("GetUser")]
    public UserListItemViewModel? GetUser(long Vid)
    {
        var items = _userService.GetAll().Where(p => p.Id == Vid).Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive,
            UserActivity = p.UserActivity
        }).FirstOrDefault();

        return items;
    }

    [HttpGet]
    [Route("EditUserView")]
    public UserListItemViewModel? EditUserView(long Eid)
    {
        var items = _userService.GetAll().Where(p => p.Id == Eid).Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        }).FirstOrDefault();

        return items;
    }

    [HttpDelete]
    [Route("DeleteUser")]
    public bool DeleteUser(long Did)
    {
        bool result = false;
        var user = _userService.GetAll().Where(p => p.Id == Did).FirstOrDefault();
        if (user?.Email != null)
        {
            _userService.Delete(Did);
            result = true;
        }
        return result;
    }

    [HttpPut]
    [Route("EditUser")]
    public bool EditUser(User user)
    {
        bool result = false;
        var userId = _userService.GetAll().Where(p => p.Id == user.Id);
        var UserOldActivity = _userService.GetAll().Where(p => p.Id == user.Id).Select(p => p.UserActivity);
        Console.WriteLine(UserOldActivity);
        //Console.WriteLine(UserOldActivity.FirstOrDefault()); 
        //Console.WriteLine(string.Join(";", UserOldActivity));
        if (userId != null)
        {
            if (UserOldActivity != null)
            {
                user.UserActivity = UserOldActivity + " Data Edited:" + DateTime.Now;
            }
            else
            {
                user.UserActivity = "Data Edited:" + DateTime.Now;
            }
            _userService.Update(user);
            result = true;
        }
        return result;
    }

    [HttpPost]
    [Route("SendActionLog")]
    public ActionResult SendActionLog(string type, long id)
    {
        string text = "";
        switch (type)
        {
            case "ShowAll":
                text = "Show All Button Clicked";
                break;
            case "NonActive":
                text = "Non Active Button Clicked";
                break;
            case "ActiveOnly":
                text = "Active Only Button Clicked";
                break;
            case "View":
                text = "Viewed User(ID:" + id + ")'s Data";
                break;
            case "EditUserView":
                text = "Attempt to Edit User(ID:" + id + ")'s Data";
                break;
            case "Edit":
                text = "Edited User(ID:" + id + ")'s Data";
                break;
            case "EditCancel":
                text = "Edit User(ID:" + id + ")'s Data Action Canceled";
                break;
            case "Delete":
                text = "User(ID:" + id + ")'s Data Deleted";
                break;
            case "DeleteCancel":
                text = "Delect User(ID:" + id + ")'s Data Action Canceled";
                break;
            case "Create":
                text = "Created New User data";
                break;
            case "CreateCancel":
                text = "Create User Action Canceled";
                Console.WriteLine("1");
                break;
            default:
                text = "Invaild Type";
                break;
        }
        Console.WriteLine("2: " + text);
        return RedirectToRoute(new { Controller = "logs", Action = "CreateLogs", message = text });
    }
}
