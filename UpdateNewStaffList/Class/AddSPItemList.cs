using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using CWAppsCore.Helpers;
using System.Configuration;


namespace UpdateNewStaffList.Class
{
    public class AddSPItemList
    {
        public void DeleteNewStafflist()
        {
            

        }

        public void AddNewStafflist(List<string> lstStaff)
        {
            try
            {
                //Get App.config
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                AppSettingsSection appSettings = configuration.AppSettings;

                com.chunwo.dms.Lists lstService = new com.chunwo.dms.Lists();
                lstService.Credentials = CredentialCache.DefaultCredentials;
                lstService.Url = appSettings.Settings["PhonebookService"].Value;

                string strLogin = appSettings.Settings["PhonebookServiceID"].Value;
                string strPw = appSettings.Settings["PhonebookServicePW"].Value;
                string strDomain = appSettings.Settings["PhonebookServiceDomain"].Value;

                //lstService.Url = "http://dms.chunwo.com/sites/teamsupport/phonebook/_vti_bin/Lists.asmx";

                //lstService.Credentials = new NetworkCredential("spadmin", "P@ssw0rd", "CHUNWO");
                //lstService.Credentials = new NetworkCredential("wong.wing", "97476228@Mm", "CHUNWO");
                lstService.Credentials = new NetworkCredential(strLogin, strPw, strDomain);


                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                System.Xml.XmlElement batchElement = doc.CreateElement("Batch");

                string strBatch = "<Method ID='1' Cmd='New'>" +
                    "<Field Name ='Title'>" + lstStaff[0] + "</Field>" +
                    "<Field Name ='Staff_x0020_Number'>" + lstStaff[1] + "</Field>" +
                    "<Field Name ='Full_x0020_Name'>" + lstStaff[2] + "</Field>" +
                    "<Field Name ='First_x0020_Name'>" + lstStaff[3] + "</Field>" +
                    "<Field Name ='Last_x0020_Name'>" + lstStaff[4] + "</Field>" +
                    "<Field Name ='Nick_x0020_Name'>" + lstStaff[5] + "</Field>" +
                    "<Field Name ='Department'>" + lstStaff[6] + "</Field>" +
                    "<Field Name ='DepartmentDesc'>" + lstStaff[7].StringToURLEncode() + "</Field>" +
                    "<Field Name ='Company_x0020_Co'>" + lstStaff[8] + "</Field>" +
                    "<Field Name ='PositionCode'>" + lstStaff[9] + "</Field>" +
                    "<Field Name ='PositionDesc'>" + lstStaff[10].StringToURLEncode() + "</Field>" +
                    "<Field Name ='Email'>" + lstStaff[11] + "</Field>" +
                    "<Field Name ='ADEnable'>" + lstStaff[12] + "</Field>" +
                    "<Field Name ='Division'>" + lstStaff[13] + "</Field>" +
                    "<Field Name ='ChineseName'>" + lstStaff[14] + "</Field>" +
                    "</Method>";

                batchElement.SetAttribute("OnError", "Continue");
                batchElement.SetAttribute("ListVersion", "1");
                batchElement.InnerXml = strBatch.ToString();

                //Add To Server
                lstService.UpdateListItems("New Staff List", batchElement);
                lstService.Dispose();

            }
            catch (Exception ex)
            {
                SystemLog.filePath = Directory.GetCurrentDirectory() + "\\sys.log";
                SystemLog.log(ex.ToString());
            }

        }

    }
}