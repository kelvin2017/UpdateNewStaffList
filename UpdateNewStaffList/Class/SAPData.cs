using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using SAP.Middleware.Connector;

namespace UpdateNewStaffList.Class
{    
    public class SAPData
    {
        private string strTblName;
        private string strFuncName;

        public DataTable Connect(string strFunc, string strTbl)
        {
            DataTable tbl = new DataTable();
            strTblName = strTbl;
            strFuncName = strFunc;
            
            tbl = GetSAPData();

            return tbl;
        }

        public RfcConfigParameters GetParameters()
        {
            RfcConfigParameters parms = new RfcConfigParameters();
            //parms.Add(RfcConfigParameters.Name, "leave");
            //parms.Add(RfcConfigParameters.AppServerHost, "192.168.2.191");
            //parms.Add(RfcConfigParameters.SystemNumber, "00");
            //parms.Add(RfcConfigParameters.SystemID, "CWD");
            //parms.Add(RfcConfigParameters.User, "cwitdev");
            //parms.Add(RfcConfigParameters.Password, "cwit2948");
            //parms.Add(RfcConfigParameters.Client, "400");
            //parms.Add(RfcConfigParameters.Language, "EN");
            //parms.Add(RfcConfigParameters.IdleTimeout, "1000600");

            //Production
            parms.Add(RfcConfigParameters.Name, "leave");
            parms.Add(RfcConfigParameters.AppServerHost, "192.168.2.190");
            parms.Add(RfcConfigParameters.SystemNumber, "00");
            parms.Add(RfcConfigParameters.SystemID, "CWD");
            parms.Add(RfcConfigParameters.User, "cwitpro");
            parms.Add(RfcConfigParameters.Password, "cwit2948");
            parms.Add(RfcConfigParameters.Client, "800");
            parms.Add(RfcConfigParameters.Language, "EN");
            parms.Add(RfcConfigParameters.IdleTimeout, "1000");

            return parms;
        }      

        private DataTable GetSAPData()
        {
            DataTable tb = new DataTable();

            try
            {
                RfcDestination sapDestin = RfcDestinationManager.GetDestination(GetParameters());
                RfcRepository sapRepository = sapDestin.Repository;
                IRfcFunction sapFunc = sapRepository.CreateFunction(this.strFuncName);

                sapFunc.Invoke(sapDestin);
 
                //Create DataTable Records
                IRfcTable tblReturn = sapFunc.GetTable(strTblName);

                tb = CreateDataTable(tblReturn);

            }
            catch(Exception ex)
            {
                SystemLog.filePath = Directory.GetCurrentDirectory() + "\\sys.log";
                SystemLog.log(ex.ToString());
            }

            return tb;
        }

        /// <summary>
        /// Create DataTable to Store SAP Records
        /// </summary>
        /// <param name="rfcTable"></param>
        /// <returns></returns>
        public static DataTable CreateDataTable(IRfcTable rfcTable)
        {
            var dataTable = new DataTable();

            for (int element = 0; element < rfcTable.ElementCount; element++)
            {
                RfcElementMetadata metadata = rfcTable.GetElementMetadata(element);
                dataTable.Columns.Add(metadata.Name);
            }

            foreach (IRfcStructure row in rfcTable)
            {
                DataRow newRow = dataTable.NewRow();
                for (int element = 0; element < rfcTable.ElementCount; element++)
                {
                    RfcElementMetadata metadata = rfcTable.GetElementMetadata(element);
                    newRow[metadata.Name] = row.GetString(metadata.Name);
                }
                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }

    }
}