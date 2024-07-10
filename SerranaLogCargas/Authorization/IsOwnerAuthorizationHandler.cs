﻿using LogCargas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

//https://learn.microsoft.com/pt-br/aspnet/core/security/authorization/secure-data?view=aspnetcore-6.0#create-owner-manager-and-administrator-authorization-handlers

namespace LogCargas.Authorization
{
    public class IsOwnerAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, LoadScheduling>
    {
        UserManager<IdentityUser> _userManager;

        public IsOwnerAuthorizationHandler(UserManager<IdentityUser>
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   LoadScheduling resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // If not asking for CRUD permission, return.

            //if (requirement.Name != Constants.CreateOperationName &&
            //    requirement.Name != Constants.ReadOperationName &&
            //    requirement.Name != Constants.UpdateOperationName &&
            //    requirement.Name != Constants.DeleteOperationName)
            //{
            //    return Task.CompletedTask;
            //}

            if (resource.OwnerID == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}