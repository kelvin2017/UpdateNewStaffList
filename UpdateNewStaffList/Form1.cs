using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateNewStaffList
{
    public partial class Form1 : Form
    {
        DataTable dtSAP;
        DataTable dtAD;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {               
                //GET SAP & AD data
                DataStore();

                AddNewStaffList();

                this.Close();
            }
            catch (Exception ex)
            {
                Class.SystemLog.filePath = Directory.GetCurrentDirectory() + "\\sys.log";
                Class.SystemLog.log(ex.ToString());
            }
        }

        private void DataStore()
        {
            //Get SAP Staff Full List
            UpdateNewStaffList.Class.SAPData sap = new Class.SAPData();
            dtSAP = sap.Connect("ZHR_STAFF_LIST_FULL", "IT_TAB");
             

            //Get AD All Account
            UpdateNewStaffList.Class.ADConnector ad = new Class.ADConnector();
            dtAD = ad.GenADTable();

            //if(dtAD.Rows.Count > 0)
            //{
            //    MessageBox.Show("AD Count" + dtAD.Rows.Count.ToString().Trim());
            //}
            //else
            //{ 
            //    MessageBox.Show("AD Count : 0 ");
            //}
        }

        /// <summary>
        /// Insert phone book New Staff List from SAP & AD
        /// http://dms.chunwo.com/sites/teamsupport/phonebook/_layouts/15/start.aspx#/SitePages/NewStaffList.aspx
        /// </summary>
        private void AddNewStaffList()
        {
            try
            {
                int nRecCount = 0;

                foreach (DataRow drSAP in dtSAP.Rows)
                {
                    List<string> lstStafflist = new List<string>();
                    lstStafflist.Add(drSAP["NACHN"].ToString().Trim()); // Title
                    lstStafflist.Add(drSAP["PERNR"].ToString().Trim()); // Staff Number
                    lstStafflist.Add(drSAP["ZFNAME"].ToString().Trim()); // Full Name
                    lstStafflist.Add(drSAP["NACHN"].ToString().Trim()); // First Name
                    lstStafflist.Add(drSAP["VORNA"].ToString().Trim()); // Last Name
                    lstStafflist.Add(drSAP["RUFNM"].ToString().Trim()); // Nick Name
                    lstStafflist.Add(drSAP["SHORT"].ToString().Trim()); // Department
                    lstStafflist.Add(drSAP["STEXT"].ToString().Trim()); // Department Desc
                    lstStafflist.Add(drSAP["ZZ_COMPANY"].ToString().Trim()); // Company Code
                    lstStafflist.Add(drSAP["POSITION_SHORT"].ToString().Trim()); // Position Code 
                    lstStafflist.Add(drSAP["POSITION_DESC"].ToString().Trim()); // Position Desc
                    lstStafflist.Add("");                                      // Email
                    lstStafflist.Add("0");                                     // AD Enable
                    lstStafflist.Add(drSAP["EESUBGROUP"].ToString().Trim()); // Division
                    lstStafflist.Add(drSAP["CHINESE_NAME"].ToString().Trim()); // Chinese Name

                    foreach (DataRow drAD in dtAD.Rows)
                    {
                        if (drAD["EmployeeId"].ToString().Trim() == drSAP["PERNR"].ToString().Trim())
                        {
                            lstStafflist[11] = drAD["EmailAddr"].ToString().Trim(); // Email
                            lstStafflist[12] = drAD["ADEnabled"].ToString().Trim(); // AD Enabled
                        }
                    }

                    //Add List item
                    UpdateNewStaffList.Class.AddSPItemList sp = new Class.AddSPItemList();
                    sp.AddNewStafflist(lstStafflist);
                    lstStafflist.Clear();

                    nRecCount = nRecCount + 1;                    
                }

                //Update Log
                Class.SystemLog.filePath = Directory.GetCurrentDirectory() + "\\Log\\Update.log";
                //Class.SystemLog.log(DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
                Class.SystemLog.log("New Staff Records : " + nRecCount.ToString());
                Class.SystemLog.log("Success");
                Class.SystemLog.log("-------------------------------------------------");

            }
            catch (Exception ex)
            {
                Class.SystemLog.filePath = Directory.GetCurrentDirectory() + "\\sys.log";
                Class.SystemLog.log(ex.ToString());
            }
        }



    }
}
