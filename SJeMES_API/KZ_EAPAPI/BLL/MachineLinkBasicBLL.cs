using KZ_EAPAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_EAPAPI.BLL
{
    public class MachineLinkBasicBLL
    {
        public DataTable QueryMachineList(DataBase DB, string vMachineID, string vFactory, string vDept, string vType, string vWhetherLink)
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            return Dal.QueryMachineList(DB, vMachineID, vFactory, vDept, vType, vWhetherLink);
        }

        public DataTable QueryDeptList(DataBase DB , string vFactory, string vDept )
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            return Dal.QueryDeptList(DB, vFactory, vDept);
        }

        public DataTable QueryMachineIdByDept(DataBase DB, string vDept)
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            return Dal.QueryMachineIdByDept(DB, vDept);
        }

        public DataTable GetAllMachineType(DataBase DB)
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            return Dal.GetAllMachineType(DB);
        }

        public DataTable GetFactory(DataBase DB)
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            return Dal.GetFactory(DB);
        }

        public string AddMachineContrast(DataBase DB, string vMachineID, string vMachineNO, string userId)
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            return Dal.AddMachineContrast(DB, vMachineID, vMachineNO, userId);
        }

        public void DelMachineContrastById(DataBase DB, string vMachineID)
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            Dal.DelMachineContrastById(DB, vMachineID);
        }

        public DataTable GetMachineContrastList(DataBase DB, string vFactory, string vType, string vMachineID, string vMachineNO)
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            return Dal.GetMachineContrastList(DB, vFactory, vType, vMachineID, vMachineNO);
        }

        public DataTable GetEapContrastListByType(DataBase DB, string vType)
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            return Dal.GetEapContrastListByType(DB, vType);
        }

        public void UpdateLinkedStatusById(DataBase DB, string vMachineID, string vWhetherLink)
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            Dal.UpdateLinkedStatusById(DB, vMachineID, vWhetherLink);
        }

        public void updateEapRftAndRate(DataBase DB, DataTable dt, string userId)
        {
            MachineLinkBasicDAL Dal = new MachineLinkBasicDAL();
            Dal.updateEapRftAndRate(DB, dt, userId);
        }
    }
}




