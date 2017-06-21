using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(WebNotify.App_Start.Startup))]
namespace WebNotify
{
    [HubName("notifyHub")]
    public class NotificationHub : Hub
    {

        public string Activate()
        {
            return "Monitor Activated";
        }


        /// <summary>
        /// 連線動作
        /// </summary>
        /// <param uid="uid"></param>
        [HubMethodName("userConnected")]
        public void userConnected(string uid)
        {
            var contId = Context.ConnectionId;
            UserHandler.ConnectedIds.Add(contId, uid);
            ShowNotify(uid, $"{uid}，您已連線，您的ID:{contId}");
        }


        /// <summary>
        /// 離線動作
        /// </summary>
        /// <param name="clientName">clent名稱</param>
        /// <param name="hostName">host名稱</param>
        [HubMethodName("userOffLine")]
        public void OffLine(string clientName)
        {

            var clientId = UserHandler.ConnectedIds.Where(p => p.Value == clientName).FirstOrDefault().Key;

            //斷client
            Clients.Client(clientId).closeConnection();

        }

        //[HubMethodName("SetStatus")]
        //public void SetStatus()
        //{
        //    JobRepository job = new JobRepository();
        //    job.GetData();
        //    ShowData();
        //}

        /// <summary>
        /// 顯示資料狀態到前端
        /// </summary>
        public static void ShowData()
        {
            GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.All.displayStatus();
        }

        /// <summary>
        /// 顯示通知到前端
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="message"></param>
        public static void ShowNotify(string uid, string message)
        {
            var contId = UserHandler.ConnectedIds.Where(x => x.Value == uid).FirstOrDefault().Key;
            GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients.Client(contId).showNotify(message);
        }

        /// <summary>
        /// 顯示已經工作完成的訊息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="reportName"></param>
        /// <param name="dataID"></param>
        public static void ShowCompleteNotify(string uid,string reportName,string dataID)
        {
            //JobRepository.SetNotified(dataID);
            ShowNotify(uid, $"親愛的{uid}，您的報表「{reportName}」已完成");
        }
    }




    /// <summary>
    /// 連線使用者類別
    /// </summary>
    public static class UserHandler
    {
        public static Dictionary<string, string> ConnectedIds = new Dictionary<string, string>();
    }
}