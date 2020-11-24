using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region Additional Namespaces
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data.Entity;
using TeamAssignment.Models;
using TeamAssignment;
using eRaceSystem.BLL;
using eRaceSystem.Data.POCOs;

#endregion

namespace TeamAssignment.Security
{
    public class SecurityDbContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            #region Seed the roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var startupRoles = ConfigurationManager.AppSettings["startupRoles"];            
            roleManager.Create(new IdentityRole { Name = startupRoles });

            
            PositionController sysmgrPosition = new PositionController();
            List<string> roleList = sysmgrPosition.Positions_GetDescriptions();
            foreach (var role in roleList)
                roleManager.Create(new IdentityRole { Name = role });
            
            #endregion

            #region Seed the users
            string adminUser = ConfigurationManager.AppSettings["adminUserName"];
            string adminRole = ConfigurationManager.AppSettings["adminRole"];
            string adminEmail = ConfigurationManager.AppSettings["adminEmail"];
            string adminPassword = ConfigurationManager.AppSettings["adminPassword"];
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var result = userManager.Create(new ApplicationUser
            {
                UserName = adminUser,
                Email = adminEmail
            }, adminPassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(adminUser).Id, adminRole);

            //string customerUser = ConfigurationManager.AppSettings["customerUserName"];
            //string customerRole = ConfigurationManager.AppSettings["customerRole"];
            //string customerEmail = ConfigurationManager.AppSettings["customerEmail"];
            string employeePassword = "Pa$$w0rd";
            EmployeeController sysmgrEmployee = new EmployeeController();
            List<EmployeesAccountPOCO> employeeAccounts = sysmgrEmployee.Employee_Accounts();

            foreach (EmployeesAccountPOCO employee in employeeAccounts)
            {
                result = userManager.Create(new ApplicationUser
                {
                    UserName = employee.UserName,
                    Email = employee.Email,
                    EmployeeId = employee.EmployeeID
                }, employeePassword);
                if (result.Succeeded)
                    userManager.AddToRole(userManager.FindByName(employee.UserName).Id, employee.EmployeeRole);
            }
            
            #endregion

            // ... etc. ...

            base.Seed(context);
        }
    }
}