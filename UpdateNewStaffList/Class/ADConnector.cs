using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Data;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using CWAppsCore.Helpers;

namespace UpdateNewStaffList.Class
{
    class ADConnector
    {
        private DataTable MakeADUserTable()
        {
            // Create a new DataTable.
            DataTable ADUsertbl = new DataTable("ADUserTable");

            DataColumn EmployeeId = new DataColumn("EmployeeId");
            EmployeeId.DataType = typeof(string);
            EmployeeId.AllowDBNull = true;
            EmployeeId.MaxLength = 20;
            ADUsertbl.Columns.Add(EmployeeId);

            DataColumn Name = new DataColumn("Name");
            Name.DataType = typeof(string);
            Name.AllowDBNull = true;
            Name.MaxLength = 50;
            ADUsertbl.Columns.Add(Name);

            DataColumn EmailAddr = new DataColumn("EmailAddr");
            EmailAddr.DataType = typeof(string);
            EmailAddr.AllowDBNull = true;
            EmailAddr.MaxLength = 50;
            ADUsertbl.Columns.Add(EmailAddr);

            DataColumn DisplayName = new DataColumn("DisplayName");
            DisplayName.DataType = typeof(string);
            DisplayName.AllowDBNull = true;
            DisplayName.MaxLength = 100;
            ADUsertbl.Columns.Add(DisplayName);

            DataColumn Enabled = new DataColumn("ADEnabled");
            Enabled.DataType = typeof(Int16);
            Enabled.DefaultValue = false;
            ADUsertbl.Columns.Add(Enabled);

            return ADUsertbl;
        }

        public DataTable GenADTable()
        {
            DataTable tbl = MakeADUserTable();

            List<UserPrincipal> lstUser = GetAllUser();

            for (int i = 0; i < lstUser.Count; i++)
            {
                DataRow dr = tbl.NewRow();

                dr["EmployeeId"] = lstUser[i].EmployeeId == "" ? "" : lstUser[i].EmployeeId.NullToString();
                dr["Name"] = lstUser[i].Name;
                dr["EmailAddr"] = lstUser[i].EmailAddress == "" ? "" : lstUser[i].EmailAddress.NullToString();
                dr["DisplayName"] = lstUser[i].DisplayName;
                dr["ADEnabled"] = lstUser[i].Enabled == true ? 1 : 0;
                tbl.Rows.Add(dr);
            }

            return tbl;
        }


        public List<UserPrincipal> GetAllUser()
        {
            try
            {
                List<UserPrincipal> lst = new List<UserPrincipal>();
                List<string> logMess = new List<string>();
                //string path = ConfigurationSettings.AppSettings["SysDir"];

                //string adCon = "";

                //var path = Path.Combine(Directory.GetCurrentDirectory(), "\\CWISConfig.xml");
                string path = Directory.GetCurrentDirectory() + "\\CWISConfig.xml";

                //ADAuthenicator AD = new ADAuthenicator("C:\\Project\\ProjectClass\\ProjectClass\\CWISConfig.xml");
                ADAuthenicator AD = new ADAuthenicator(path);

                //ADAuthenicator AD = new ADAuthenicator(path);
                lst = AD.ListAllUser(ref logMess);

                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
