using KZ_JMSAPI.DAL;
using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KZ_JMSAPI.BLL
{
    public class JMS_QRCode_Print_BLL
    {
        public DataTable GetUnitList(DataBase DB )
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.GetUnitList(DB);
        }

        public DataTable GetRoutList(DataBase DB)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.GetRoutList(DB);
        }

        public DataTable GetSeidList(DataBase DB,string CompanyCode)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.GetSeidList(DB, CompanyCode);
        }

        public DataTable GetSizeListBySeID(DataBase DB, string vSeId, string vRoutNo)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.GetSizeListBySeID(DB, vSeId, vRoutNo);
        }

        public DataTable GetMaxPrintVer(DataBase DB, string vSeId, string vRoutNo)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.GetMaxPrintVer(DB, vSeId, vRoutNo);
        }

        public bool InsertQrCodeData(DataBase DB, string CompanyCode, DataTable vDt, string userId,string vUnit,string vSeId, string vRoutNo,string vStartDate, string vFinishDate,int vPrintVer,int vEndTieno, int vPrintNum)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.InsertQrCodeData(DB, CompanyCode, vDt, userId, vUnit, vSeId, vRoutNo, vStartDate, vFinishDate, vPrintVer, vEndTieno, vPrintNum);
        }

        public DataTable GetQrCodeBySeidAndVer(DataBase DB, string vSeId, string vPrintVer, string vRoutNo)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.GetQrCodeBySeidAndVer(DB, vSeId, vPrintVer, vRoutNo);
        }

        public DataTable GetMListBySeid(DataBase DB, string vSeId, string vRoutNo)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.GetMListBySeid(DB, vSeId, vRoutNo);
        }

        public DataTable QueryDListBySeidAndVer(DataBase DB, string vSeId, string vPrintVer, string vRoutNo)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.QueryDListBySeidAndVer(DB, vSeId, vPrintVer, vRoutNo);
        }

        public DataTable QueryDDetialBySize(DataBase DB, string vSeId, string vPrintVer, string vSizeNo, string vRoutNo)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.QueryDDetialBySize(DB, vSeId, vPrintVer, vSizeNo, vRoutNo);
        }

        public DataTable GetQrCodeByTieNo(DataBase DB, string vSeId, string vTieNo, string vRoutNo)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.GetQrCodeByTieNo(DB, vSeId, vTieNo, vRoutNo);
        }

        public DataTable GetQrCodeBySizeAndVer(DataBase DB, string vSeId, string vPrintVer, string vSizeNo, string vRoutNo)
        {
            JMS_QRCode_Print_DAL Dal = new JMS_QRCode_Print_DAL();
            return Dal.GetQrCodeBySizeAndVer(DB, vSeId, vPrintVer, vSizeNo, vRoutNo);
        }
    }
}
