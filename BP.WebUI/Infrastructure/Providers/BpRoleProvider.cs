using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;
using BP.BLL.Interface.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace BP.WebUI.Infrastructure.Providers
{
    public class BpRoleProvider : RoleProvider
    {
        public IRoleService roleService
        {
            get
            {
                return System.Web.Mvc.DependencyResolver.Current.GetService<IRoleService>();
            }
            set { }
        }
        public IUserService userService
        {
            get
            {
                return System.Web.Mvc.DependencyResolver.Current.GetService<IUserService>();
            }
            set { }
        }

        public override void CreateRole(string roleName)
        {
            if (roleName != null)
                roleService.Create(roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return roleService.Remove(roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            return new string[] { userService.Find(username).Role.Name };
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            BllUser user = userService.Find(username);
            if (user != null && user.Role.Name == roleName)
                return true;
            return false;
        }

        public override string[] GetAllRoles()
        {
            return roleService.GetAll().Select(x => x.Name).ToArray();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
    }
}