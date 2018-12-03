using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Common;
using WebApplication.Authorize;
using Helper.ExtendedMethods;

namespace SignalR
{
    public class BaseUserConnect
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarImg { get; set; }
    }
    public class UserConnect : BaseUserConnect
    {
        public UserConnect()
        {
            ConnectIds = new List<string>();
        }
        public List<string> ConnectIds { get; set; }
        public bool isOnline
        {
            get
            {
                return this.ConnectIds.Count() != 0;
            }
        }
        public string _ConnectIds
        {
            get
            {
                return string.Join(",", this.ConnectIds);
            }
        }

        public bool AddConnect(string ConnectionId)
        {
            if (this.ConnectIds.Where(e => e.Equals(ConnectionId)).Count() == 0)
            {
                lock (this.ConnectIds)
                {
                    this.ConnectIds.Add(ConnectionId);
                }
                return true;
            }
            return false;
        }

        public bool RemoveConnect(string ConnectionId)
        {
            if (this.ConnectIds.Where(e => e.Equals(ConnectionId)).Count() == 0)
            {
                lock (this.ConnectIds)
                {
                    this.ConnectIds.Remove(ConnectionId);
                }
                return true;
            }
            return false;
        }
    }

    public class MessageObj
    {
        public BaseUserConnect From { get; set; }
        public string Message { get; set; }
        public long Time { get; set; }
    }

    [UserAuthorize]
    [HubName("ConnectionApp")]
    public class AppticationHub : Hub
    {
        public static AppticationHub _this;
        public static Dictionary<int, UserConnect> AccountConnect = new Dictionary<int, UserConnect>();

        public AppticationHub()
        {
            _this = this;
        }
        #region connect
        public override Task OnConnected()
        {
            var User = Context.User as CustomPrincipal;
            var ConnectionId = Context.ConnectionId;

            UserConnect UserConnect;
            if (AccountConnect.TryGetValue(User.Id, out UserConnect))
            {
                if (UserConnect.AddConnect(ConnectionId))
                {
                    //ResponseAllStatus();
                };
            }
            else
            {
                UserConnect = new UserConnect()
                {
                    UserId = User.Id,
                    AvatarImg = User.AvatarImg,
                    UserName = User.UserName,
                    ConnectIds = new List<string>() { ConnectionId }
                };
                lock (AccountConnect)
                {
                    AccountConnect.Add(User.Id, UserConnect);
                }

                ResponseAllStatus();
            }

            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var User = Context.User as CustomPrincipal;
            var ConnectionId = Context.ConnectionId;

            UserConnect UserConnect;
            if (AccountConnect.TryGetValue(User.Id, out UserConnect))
            {
                if (UserConnect.RemoveConnect(ConnectionId))
                {
                    if (!UserConnect.isOnline)
                    {
                        ResponseAllStatus();
                    }
                };
            }

            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            var User = Context.User as CustomPrincipal;
            var ConnectionId = Context.ConnectionId;

            UserConnect UserConnect;
            if (AccountConnect.TryGetValue(User.Id, out UserConnect))
            {
                if (UserConnect.AddConnect(ConnectionId))
                {
                    ResponseAllStatus();
                };
            }
            return base.OnReconnected();
        }
        #endregion

        #region methods
        dynamic JsonSuccess(dynamic data)
        {
            return new
            {
                Success = true,
                Data = data
            };
        }
        dynamic JsonError(string mess)
        {
            return new
            {
                Success = false,
                Message = mess
            };
        }
        dynamic JsonError(Exception ex)
        {
            return JsonError(ex.Message);
        }

        public void ResponseAllStatus()
        {
            var User = Context.User as CustomPrincipal;
            var ConnectionId = Context.ConnectionId;

            Clients.All.Listener_GetAllConnect(AccountConnect.ToList().Select(e => e.Value));
        }

        public dynamic GetAllConnect()
        {
            var User = Context.User as CustomPrincipal;
            var ConnectionId = Context.ConnectionId;
            //Clients.Caller.Listener_GetAllConnect(AccountConnect.ToList().Select(e => e.Value));
            return JsonSuccess(AccountConnect.Where(e => e.Value.isOnline).Select(e => e.Value).ToList());
        }

        public dynamic SendMessageTo(int UserId, string message)
        {
            var User = Context.User as CustomPrincipal;
            var ConnectionId = Context.ConnectionId;

            UserConnect UserConnect;
            if (AccountConnect.TryGetValue(UserId, out UserConnect))
            {
                Clients.Clients(UserConnect.ConnectIds).Listener_Message(new MessageObj()
                {
                    Message = message,
                    From = new BaseUserConnect()
                    {
                        AvatarImg = User.AvatarImg,
                        UserId = User.Id,
                        UserName = User.UserName
                    },
                    Time = DateTime.Now.ToLong()
                });
                return JsonSuccess(1);
            }

            return JsonError("Cannot find User ID or offline");
        }
        #endregion
    }
}