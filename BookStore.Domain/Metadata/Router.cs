using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Metadata
{
    public static class Router
    {
        public const string root = "api/";
        public const string version = "v1/";
        public const string rule = root + version;

        public static class StudentRouting
        {
            public const string prefix = rule + "student/";
            public const string list = prefix + "list";
            public const string Paginated = prefix + "paginated";
            public const string GetById = prefix + "{id}";
            public const string Create = prefix + "create";
            public const string Edit = prefix + "edit";
            public const string Delete = prefix + "{id}";



        }
        public static class DepartmentRouting
        {
            public const string prefix = rule + "department/";
            public const string list = prefix + "list";
            public const string Paginated = prefix + "paginated";
            public const string GetById = prefix + "{id}";
            public const string Create = prefix + "create";
            public const string Edit = prefix + "edit";
            public const string Delete = prefix + "{id}";



        }

        public static class ApplicationUserRouting
        {
            public const string prefix = rule + "User";
            public const string Create = prefix + "/Create";
            public const string Paginated = prefix + "/Paginated";
            public const string GetById = prefix + "{id}";
            public const string Edit = prefix + "/Edit";
            public const string Delete = prefix + "/{id}";
            public const string ChangePassword = prefix + "/Change-Password";
        }
        public static class Authentication
        {
            public const string Prefix = rule + "Authentication";
            public const string SignIn = Prefix + "/SignIn";
            public const string RefreshToken = Prefix + "/Refresh-Token";
            public const string ValidateToken = Prefix + "/Validate-Token";
            public const string ConfirmEmail = "/Api/Authentication/ConfirmEmail";
            public const string SendResetPasswordCode = Prefix + "/SendResetPasswordCode";
            public const string ConfirmResetPasswordCode = Prefix + "/ConfirmResetPasswordCode";
            public const string ResetPassword = Prefix + "/ResetPassword";

        }
        public static class AuthorizationRouting
        {
            public const string Prefix = rule + "AuthorizationRouting";
            public const string Roles = Prefix + "/Roles";
            public const string Claims = Prefix + "/Claims";
            public const string Create = Roles + "/Create";
            public const string Edit = Roles + "/Edit";
            public const string Delete = Roles + "/Delete/{id}";
            public const string RoleList = Roles + "/Role-List";
            public const string GetRoleById = Roles + "/Role-By-Id/{id}";
            public const string ManageUserRoles = Roles + "/Manage-User-Roles/{userId}";
            public const string ManageUserClaims = Claims + "/Manage-User-Claims/{userId}";
            public const string UpdateUserRoles = Roles + "/Update-User-Roles";
            public const string UpdateUserClaims = Claims + "/Update-User-Claims";
        }
        public static class EmailsRoute
        {
            public const string Prefix = rule + "EmailsRoute";
            public const string SendEmail = Prefix + "/SendEmail";
        }
        public static class InstructorRouting
        {
            public const string Prefix = rule + "InstructorRouting";
            public const string GetSalarySummationOfInstructor = Prefix + "/Salary-Summation-Of-Instructor";
            public const string AddInstructor = Prefix + "/Create";
        }
    }
}
