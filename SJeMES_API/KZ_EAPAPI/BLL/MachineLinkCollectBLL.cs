using KZ_EAPAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_EAPAPI.BLL
{
    public class MachineLinkCollectBLL
    {
        public DataTable QuerySeCutPart(DataBase DB, string vSeId)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QuerySeCutPart(DB, vSeId);
        }

        public DataTable QueryEapStatusById(DataBase DB, string vMachineID)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryEapStatusById(DB, vMachineID);
        }

        public DataTable QueryQty(DataBase DB, string vDept, string vOvenId, string vPressId, string vFreezerId)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryQty(DB, vDept, vOvenId, vPressId, vFreezerId);
        }

        public DataTable QueryEqpStatusByDept(DataBase DB, string vDept)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryEqpStatusByDept(DB, vDept);
        }

        public DataTable QueryOvenParmById(DataBase DB, string vMachineID)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryOvenParmById(DB, vMachineID);
        }

        public DataTable QueryPressParmById(DataBase DB, string vMachineID)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryPressParmById(DB, vMachineID);
        }

        public DataTable QueryFreezerParmById(DataBase DB, string vMachineID)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryFreezerParmById(DB, vMachineID);
        }

        public DataTable QueryEqpAlarmByDept(DataBase DB, string vDept)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryEqpAlarmByDept(DB, vDept);
        }

        public DataTable QueryOvenHourOutput(DataBase DB, string vMachineID, string vDate)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryOvenHourOutput(DB, vMachineID, vDate);
        }

        public DataTable QueryFreezerHourOutput(DataBase DB, string vMachineID, string vDate)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryFreezerHourOutput(DB, vMachineID, vDate);
        }

        public DataTable QueryPressHourOutput(DataBase DB, string vMachineID, string vDate)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryPressHourOutput(DB, vMachineID, vDate);
        }

        public DataTable QueryCutHourOutput(DataBase DB, string vMachineID, string vDate)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCutHourOutput(DB, vMachineID, vDate);
        }

        public DataTable QueryEqpHourOutput(DataBase DB, string vDate, string vDept)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryEqpHourOutput(DB, vDate, vDept);
        }

        public DataTable QueryOvenHourParams(DataBase DB, string vMachineID, string vMaxTime, string vMinTime)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryOvenHourParams(DB, vMachineID, vMaxTime, vMinTime);
        }

        public DataTable QueryFreezerHourParams(DataBase DB, string vMachineID, string vMaxTime, string vMinTime)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryFreezerHourParams(DB, vMachineID, vMaxTime, vMinTime);
        }

        public DataTable Query7DaysRunTime(DataBase DB, string vMachineID)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.Query7DaysRunTime(DB, vMachineID);
        }

        public DataTable QueryCut7DaysOutput(DataBase DB, string vMachineID)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCut7DaysOutput(DB, vMachineID);
        }

        public DataTable QueryCut7DayOee(DataBase DB, string vMachineID)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCut7DayOee(DB, vMachineID);
        }

        public DataTable QueryCutDayStatusTime(DataBase DB, string vMachineID, string vDate)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCutDayStatusTime(DB, vMachineID, vDate);
        }

        public DataTable QueryCutRunTimeByDay(DataBase DB, string vMachineID, string vDate)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCutRunTimeByDay(DB, vMachineID, vDate);
        }

        public DataTable QueryCutOutPutByDay(DataBase DB, string vMachineID, string vDate, string vShift)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCutOutPutByDay(DB, vMachineID, vDate, vShift);
        }

        public DataTable QueryCutRunTimeByDetail(DataBase DB, string vMachineID, string vDate)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCutRunTimeByDetail(DB, vMachineID, vDate);
        }

        public DataTable QueryCutOutPutByDetail(DataBase DB, string vMachineID, string vDate, string vShift)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCutOutPutByDetail(DB, vMachineID, vDate, vShift);
        }

        public DataTable QueryOeeByDeptShift(DataBase DB, string vDept, string vDate)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryOeeByDeptShift(DB, vDept, vDate);
        }

        public DataTable QueryDayOutByDept(DataBase DB, string vDept, string vDate)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryDayOutByDept(DB, vDept, vDate);
        }

        public DataTable QueryCutRateByDay(DataBase DB, string vMachineID, string vDate, string vShift)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCutRateByDay(DB, vMachineID, vDate, vShift);
        }

        public DataTable QueryCutRftByDay(DataBase DB, string vMachineID, string vDate, string vShift)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCutRftByDay(DB, vMachineID, vDate, vShift);
        }

        public DataTable QueryCutOeeByDay(DataBase DB, string vMachineID, string vDate, string vShift)
        {
            MachineLinkCollectDAL Dal = new MachineLinkCollectDAL();
            return Dal.QueryCutOeeByDay(DB, vMachineID, vDate, vShift);
        }
    }
}

