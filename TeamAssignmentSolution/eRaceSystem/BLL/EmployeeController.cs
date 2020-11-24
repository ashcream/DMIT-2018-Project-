using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using eRaceSystem.DAL;
using eRaceSystem.Data.POCOs;
using eRaceSystem.Data.Entities;

namespace eRaceSystem.BLL
{
    [DataObject]
    public class EmployeeController
    {
        public List<string> Employees_GetTitles()
        {
            using (var context = new eRaceContext())
            {
                var results = (from x in context.Employees
                               select x.Position.Description).Distinct();
                return results.ToList();
            }
        }

        public Employee Get_Employee(int id)
        {
            using (var context = new eRaceContext())
            {
                return context.Employees.Find(id);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> Employee_ListNames()
        {
            using (var context = new eRaceContext())
            {
                var employeeList = from x in context.Employees
                                   orderby x.LastName, x.FirstName
                                   select new SelectionList
                                   {
                                       IDValueField = x.EmployeeID,
                                       DisplayText = x.FirstName + " " + x.LastName
                                   };
                return employeeList.ToList();
            }
        }

        public List<EmployeesAccountPOCO> Employee_Accounts()
        {
            using (var context = new eRaceContext())
            {
                var employeeAccounts = from x in context.Employees
                                       orderby x.FirstName, x.LastName
                                       select new EmployeesAccountPOCO
                                       {
                                           EmployeeID = x.EmployeeID,
                                           EmployeeRole = x.Position.Description,
                                           Email = x.Position.Description.Replace(" ", "") + x.FirstName + "@somewhere.com",
                                           UserName = x.Position.Description.Replace(" ", "") + x.FirstName + x.EmployeeID
                                       };
                return employeeAccounts.ToList();
            }
        }
    }

    
}
