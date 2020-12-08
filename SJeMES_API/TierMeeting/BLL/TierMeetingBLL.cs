using SJeMES_Framework_NETCore.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TierMeeting.DAL;

namespace TierMeeting.BLL
{
    public class TierMeetingBLL
    {
        #region Tier4Form
        public DataTable GetDepartmentByUser(DataBase DB, string userCode)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetDepartmentByUser(DB, userCode);
        }
        public DataTable GetKZAPDataTable(DataBase DB, string dept, int type, int process,string companyCode)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            if (companyCode.Equals("200"))
            {
                return DAL.GetAPEKZAPDataTable(DB, dept, type, process);
            }
            else
            {
                return DAL.GetKZAPDataTable(DB, dept, type, process);
            }
        }

        #region DetailKaizenForm
        public DataTable GetKaizenData(DataBase DB, DateTime date, string dept, int type, string companyCode)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            if (companyCode.Equals("200"))
            {
                //return DAL.GetAPEKaizenData(DB, date, dept, type);
                return DAL.GetAPEKaizenData(DB, date, dept, type);
            }
            else
            {
                return DAL.GetKaizenData(DB, date, dept, type);
            }

        }

        #endregion

        #region Kaizen Action
        public DataTable GetKaizenAction(DataBase DB, string from, string to, int type, string dept, int process, string companyCode)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            if (companyCode.Equals("200"))
            {
                return DAL.GetAPEKaizenAction(DB, from, to, type, dept, process);
            }
            else
            {
                return DAL.GetKaizenAction(DB, from, to, type, dept, process);
            }
        }

        public DataTable GetKZAPDataPieChart(DataBase DB, string dept, int type, int process,string companyCode)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            if (companyCode.Equals("200"))
            {
                return DAL.GetAPEKZAPDataPieChart(DB, dept, type, process);
            }
            else
            {
                return DAL.GetKZAPDataPieChart(DB, dept, type, process);
            }
        }
        public DataTable GetKZAPDataBarChart(DataBase DB, string dept, int type,int process)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetKZAPDataBarChart(DB, dept, type, process);
        }
        public DataTable GetSafetyDataByDate(DataBase DB, DateTime date, string dept, int type)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetSafetyDataByDate(DB, date, dept, type);
        }
        public DataTable GetSafetyDataUntilDate(DataBase DB, DateTime date, DateTime firstDateOfYear, string dept, int type)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetSafetyDataUntilDate(DB, date, firstDateOfYear, dept, type) ;
        }
        public DataTable GetSafetyDays(DataBase DB, DateTime date, DateTime firstDateOfYear, string dept, int type)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetSafetyDays(DB, date, firstDateOfYear, dept, type);
        }
        public DataTable GetKaizenUntilDate(DataBase DB, DateTime firstDate, DateTime date, string dept, int type)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetKaizenUntilDate(DB, firstDate, date, dept, type);
        }
        public DataTable GetManpower(DataBase DB, string dept, int type, DateTime date)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetManpower(DB, dept, type, date);
        }
        public DataTable GetMainKaizenChart(DataBase DB, DateTime date,string dept, int type)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetMainKaizenChart(DB, date, dept,  type);
        }
        #endregion
        #region DetailSafetyForm
        public DataTable GetSafetyTable(DataBase DB, DateTime firstDateOfMonth, string dept, int type)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetSafetyTable(DB, firstDateOfMonth, dept, type);
        }
        public DataTable GetSafetyChart(DataBase DB, string firstDateOfMonth, string dept, int type)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetSafetyChart(DB, firstDateOfMonth, dept, type);
        }
        #endregion
      
        public void EditKaizenAction(DataBase DB, string id, string department, string finder, string problem, string measure, string principal, string remark, string createdDate, string planDate, string finishDate)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                TierMeetingDAL DAL = new TierMeetingDAL();
                DAL.EditKaizenAction(DB, id, department, finder, problem, measure, principal, remark, createdDate, planDate, finishDate);
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }
        }
        public void CreateKaizenAction(DataBase DB, string department, string finder, string problem, string measure, string principal, string remark, string createdDate, string planDate, string finishDate, string T1, string T2, string T3, string T4)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                TierMeetingDAL DAL = new TierMeetingDAL();
                DAL.CreateKaizenAction(DB, department, finder, problem, measure, principal, remark, createdDate, planDate, finishDate, T1, T2, T3, T4);
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }
        }
        public void UpdateStatus(DataBase DB, int id, string T1, string T2, string T3, string T4)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                TierMeetingDAL DAL = new TierMeetingDAL();
                DAL.UpdateStatus(DB, id, T1, T2, T3, T4);
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }
        }
        #endregion
        #region Maturity Assessment
        public DataTable GetMaturityList(DataBase DB)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetMaturityList(DB);
        }
        public DataTable GetMaturityAssessmentList(DataBase DB, string deptCode, DateTime date)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetMaturityAssessmentList(DB, deptCode, date);
        }
        public void SaveMaturityAssessment(DataBase DB, string deptCode, DateTime date, string[] listCode, string[] listStatus, string[] listNote)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                TierMeetingDAL DAL = new TierMeetingDAL();
                DAL.DeleteMaturityAssessment(DB, deptCode, date);
                DAL.SaveMaturityAssessment(DB, deptCode, date, listCode, listStatus, listNote);
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }
        }
        public void DeleteMaturityAssessment(DataBase DB, string deptCode, DateTime date)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                TierMeetingDAL DAL = new TierMeetingDAL();
                DAL.DeleteMaturityAssessment(DB, deptCode, date);
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }
        }
        #endregion

        public DataTable GetDeptList(DataBase DB, string plant, string section)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetDeptList(DB, plant, section);
        }
        #region Tier 1
        public DataTable GetTier1(DataBase DB, string dept, DateTime date)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetTier1(DB, dept, date);
        }
        public DataTable GetTier1Standard(DataBase DB, string dept, DateTime date)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetTier1Standard(DB, dept, date);
        }
        public void SaveTier1(DataBase DB, string G_DEPTCODE, DateTime G_DATE, string G_1, string G_2, string G_3, string G_4,
                        string G_5, string G_6, string G_7, string G_8, string G_RESULT, string G_AUDITOR,
                         string username)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                TierMeetingDAL DAL = new TierMeetingDAL();
                if (DAL.GetTier1(DB, G_DEPTCODE, G_DATE).Rows.Count > 0)
                {
                    DAL.UpdateTier1(DB, G_DEPTCODE, G_DATE, G_1, G_2, G_3, G_4, G_5, G_6, G_7, G_8, G_RESULT, G_AUDITOR,
                             username);
                }
                else
                {
                    DAL.InsertTier1(DB, G_DEPTCODE, G_DATE, G_1, G_2, G_3, G_4, G_5, G_6, G_7, G_8, G_RESULT, G_AUDITOR,
                                 username);
                }
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }

        }
        public void SaveTier1Standard(DataBase DB, string G_DEPTCODE, DateTime G_DATE,
            string G_SUPERVISOR_1, string G_SUPERVISOR_2, string G_SUPERVISOR_3, string G_SUPERVISOR_4,
            string G_SUPERVISOR_5, string G_SUPERVISOR_6, string G_SUPERVISOR_7, string G_SUPERVISOR_8, string G_SUPERVISOR_AUDITOR,
            string G_VSM_1, string G_VSM_2, string G_VSM_3, string G_VSM_4,
            string G_VSM_5, string G_VSM_6, string G_VSM_7, string G_VSM_8, string G_VSM_AUDITOR,
            string G_THIRD_PARTY_1, string G_THIRD_PARTY_2, string G_THIRD_PARTY_3, string G_THIRD_PARTY_4,
            string G_THIRD_PARTY_5, string G_THIRD_PARTY_6, string G_THIRD_PARTY_7, string G_THIRD_PARTY_8, string G_THIRD_PARTY_AUDITOR,
            string username)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                TierMeetingDAL DAL = new TierMeetingDAL();
                if (DAL.GetTier1Standard(DB, G_DEPTCODE, G_DATE).Rows.Count > 0)
                {
                    DAL.UpdateTier1Standard(DB, G_DEPTCODE, G_DATE,
                        G_SUPERVISOR_1, G_SUPERVISOR_2, G_SUPERVISOR_3, G_SUPERVISOR_4, G_SUPERVISOR_5, G_SUPERVISOR_6, G_SUPERVISOR_7, G_SUPERVISOR_8, G_SUPERVISOR_AUDITOR,
                        G_VSM_1, G_VSM_2, G_VSM_3, G_VSM_4, G_VSM_5, G_VSM_6, G_VSM_7, G_VSM_8, G_VSM_AUDITOR,
                        G_THIRD_PARTY_1, G_THIRD_PARTY_2, G_THIRD_PARTY_3, G_THIRD_PARTY_4, G_THIRD_PARTY_5, G_THIRD_PARTY_6, G_THIRD_PARTY_7, G_THIRD_PARTY_8, G_THIRD_PARTY_AUDITOR,
                        username);
                }
                else
                {
                    DAL.InsertTier1Standard(DB, G_DEPTCODE, G_DATE,
                        G_SUPERVISOR_1, G_SUPERVISOR_2, G_SUPERVISOR_3, G_SUPERVISOR_4, G_SUPERVISOR_5, G_SUPERVISOR_6, G_SUPERVISOR_7, G_SUPERVISOR_8, G_SUPERVISOR_AUDITOR,
                        G_VSM_1, G_VSM_2, G_VSM_3, G_VSM_4, G_VSM_5, G_VSM_6, G_VSM_7, G_VSM_8, G_VSM_AUDITOR,
                        G_THIRD_PARTY_1, G_THIRD_PARTY_2, G_THIRD_PARTY_3, G_THIRD_PARTY_4, G_THIRD_PARTY_5, G_THIRD_PARTY_6, G_THIRD_PARTY_7, G_THIRD_PARTY_8, G_THIRD_PARTY_AUDITOR,
                        username);
                }
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }

        }
        public DataTable GetTHTByART(DataBase DB, string ART)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetTHTByART(DB, ART);
        }
        public void SaveTHT(DataBase DB, string ART, string THT)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                TierMeetingDAL DAL = new TierMeetingDAL();
                if (DAL.GetTHTByART(DB, ART).Rows.Count > 0)
                {
                    DAL.UpdateTHT(DB, ART,THT);
                }
                else
                {
                    DAL.InsertTHT(DB, ART, THT);
                }
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }

        }

        public DataTable Tier1_WeekSafety(DataBase DB, string line, string firstDate, string seventhDate)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_WeekSafety(DB, line, firstDate, seventhDate);
        }
        public DataTable GetDeptType(DataBase DB, string dept)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.GetDeptType(DB, dept);
        }
        public DataTable Tier1_WeekOutput(DataBase DB, string line, string firstDate, string seventhDate)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_WeekOutput(DB, line, firstDate, seventhDate);
        }
        public DataTable Tier1_Kaizen(DataBase DB, string dept, DateTime date)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_Kaizen(DB, dept, date);
        }
        public DataTable Tier1_WeekPPHTarget(DataBase DB, string line, string firstDate, string seventhDate)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_WeekPPHTarget(DB, line, firstDate, seventhDate);
        }
        public DataTable Tier1_WeekPPH(DataBase DB, string line, string firstDate, string seventhDate)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_WeekPPH(DB, line, firstDate, seventhDate);
        }
        public DataTable Tier1_WeekLLER(DataBase DB, string line, string firstDate, string seventhDate)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_WeekLLER(DB, line, firstDate, seventhDate);
        }
        public DataTable Tier1_WeekMulti(DataBase DB, string line, string firstDate, string seventhDate)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_WeekMulti(DB, line, firstDate, seventhDate);
        }
        public void SaveTier1_Downtime(DataBase DB, string dept, string downtime, DateTime date)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                TierMeetingDAL DAL = new TierMeetingDAL();
                if (DAL.GetTier1_Downtime(DB, dept, date).Rows.Count > 0)
                {
                    DAL.UpdateTier1_Downtime(DB, dept, downtime, date);
                }
                else
                {
                    DAL.InsertTier1_Downtime(DB, dept, downtime, date);
                }
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }
        }
        public DataTable Tier1_WeekDowntime(DataBase DB, string vLine, string FirstDay, string SeventhDay)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.Tier1_WeekDowntime(DB, vLine, FirstDay, SeventhDay);
        }
        public void SaveTier1_COT(DataBase DB, string dept, DateTime date, string target, string actual)
        {
            try
            {
                DB.Open();
                DB.BeginTransaction();
                TierMeetingDAL DAL = new TierMeetingDAL();
                if (DAL.GetTier1_COT(DB, dept, date).Rows.Count > 0)
                {
                    DAL.UpdateTier1_COT(DB, dept, date, target, actual);
                }
                else
                {
                    DAL.InsertTier1_COT(DB, dept, date, target, actual);
                }
                DB.Commit();
            }
            catch (Exception)
            {
                DB.Rollback();
                throw;
            }
            finally
            {
                DB.Close();
            }
        }
        public DataTable Tier1_WeekCOT(DataBase DB, string vLine, string FirstDay, string SeventhDay)
        {
            TierMeetingDAL DAL = new TierMeetingDAL();
            return DAL.Tier1_WeekCOT(DB, vLine, FirstDay, SeventhDay);
        }
        public DataTable Tier1_WeekHourlyOutput(DataBase DB, string line, string firstDate, string seventhDate)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_WeekHourlyOutput(DB, line, firstDate, seventhDate);
        }
        public DataTable Tier1_WeekRFT(DataBase DB, string vDept,  string FirstDay, string SeventhDay)
        {
            TierMeetingDAL Dal = new TierMeetingDAL();
            return Dal.Tier1_WeekRFT(DB, vDept, FirstDay, SeventhDay);
        }
        #endregion

    }
}
