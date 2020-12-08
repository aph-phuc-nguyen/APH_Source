using KZ_EPMAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_EPMAPI.BLL
{
    public class MachineImportBLL
    {
        public DataTable GetMachineNO(DataBase DB, string CompanyCode)
        {
            MachineImportDAL Dal = new MachineImportDAL();
            return Dal.GetMachineNO(DB, CompanyCode);
        }
        public DataTable MachineUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string userId, string CompanyCode, string dateFormat)
        {
            MachineImportDAL Dal = new MachineImportDAL();
            DataTable tab = Dal.ValiMachineUpLoad(DB, dt, CompanyCode);
            if (tab.Rows.Count == 0)
            {
                Dal.MachineUpLoad(DB, dt, userId, CompanyCode);
            }
            return tab;
        }
        public DataTable MaintainUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string userId, string CompanyCode, string dateFormat)
        {
            MachineImportDAL Dal = new MachineImportDAL();
            DataTable tab = Dal.ValiMaintainUpLoad(DB, dt, CompanyCode);
            if (tab.Rows.Count == 0)
            {
                Dal.MaintainUpLoad(DB, dt, userId, CompanyCode);
            }
            return tab;
        }
        public DataTable CorrectionUpLoad(SJeMES_Framework_NETCore.DBHelper.DataBase DB, DataTable dt, string userId, string CompanyCode, string dateFormat)
        {
            MachineImportDAL Dal = new MachineImportDAL();
            DataTable tab = Dal.ValiCorrectionUpLoad(DB, dt, CompanyCode);
            if (tab.Rows.Count == 0)
            {
                Dal.CorrectionUpLoad(DB, dt, userId, CompanyCode);
            }
            return tab;
        }
    }
}
