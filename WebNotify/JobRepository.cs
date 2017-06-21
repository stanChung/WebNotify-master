using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebNotify
{
    public class StatusEntity
    {
        public string DataID { get; set; }

        public string ReportName { get; set; }

        public string ReportStatus { get; set; }

        public string ReportOwner { get; set; }

        public bool IsComplete { get; set; }

        public bool IsNotified { get; set; }
    }


    public class JobRepository
    {

        /// <summary>
        /// 取得顯示資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StatusEntity> GetData()
        {
            var lst = new List<StatusEntity>();
            var strSQL = @"SELECT DataID,ReportName,ReportStatus,ReportOwner,IsComplete FROM dbo.TestMonitor ";
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["THICOM_SQL"].ConnectionString))
            using (var cmd = new SqlCommand(strSQL, conn))
            {

                cmd.Notification = null;
                var dependency = new SqlDependency(cmd);
                dependency.OnChange += new OnChangeEventHandler(Dependency_OnChange);

                if (conn.State != System.Data.ConnectionState.Open) conn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    lst = dr.Cast<IDataRecord>().Select(x => new StatusEntity()
                    {
                        DataID = x.GetString(0),
                        ReportName = x.GetString(1),
                        ReportStatus = x.GetString(2),
                        ReportOwner = x.GetString(3),
                        IsComplete = x.GetBoolean(4)
                    }).ToList();
                }
            }

            return lst;
        }

        /// <summary>
        /// 更新通知狀態
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        //public static int SetNotified(string dataId)
        //{
        //    var strSQL = @"Update dbo.TestMonitor SET IsNotified=1 WHERE DataId=@dataId ";
        //    var result = 0;

        //    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["THICOM_SQL"].ConnectionString))
        //    using (var cmd = new SqlCommand(strSQL, conn))
        //    {
        //        cmd.Connection.Open();
        //        cmd.Parameters.AddWithValue("@dataId", dataId);
        //        result = cmd.ExecuteNonQuery();
        //    }

        //    return result;
        //}

        /// <summary>
        /// 資料狀態改變時的事件處理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
                NotificationHub.ShowData();

        }



    }
}