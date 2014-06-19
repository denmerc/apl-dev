using System;
using System.Runtime.Serialization;

namespace APLPromoter.Server.Entity
{
    public class NullT { }

    [DataContract]
    public class Session<T> where T : class
    {
        [DataMember]
        public User.Identity UserIdentity { get; set; }
        [DataMember]
        public T Data { get; set; }
        [DataMember]
        public Boolean AppOnline { get; set; }
        [DataMember]
        public Boolean Authenticated { get; set; }
        [DataMember]
        public Boolean SqlAuthorization { get; set; }
        [DataMember]
        public Boolean WinAuthorization { get; set; }
        [DataMember]
        public Boolean SessionOk { get; set; }
        [DataMember]
        public String ClientMessage { get; set; }
        [DataMember]
        public String ServerMessage { get; set; }
        [DataMember]
        public String SqlKey { get; set; }
        [DataMember]
        public APLPromoter.Server.Entity.Workflow Workflow { get; set; }

        public Session<Tdata> Clone<Tdata>(Tdata Data) where Tdata : class {

            return InitCommon<Tdata>(this, Data);
        }

        public static Session<Tdata> Clone<Tdata>(Session<T> session, Tdata Data = null) where Tdata : class {

            return session.InitCommon<Tdata>(session, Data);
        }

        private Session<Tdata> InitCommon<Tdata>(Session<T> session, Tdata Data) where Tdata : class {

            return new Session<Tdata> 
            {
                SessionOk = false,
                ClientMessage = String.Empty,
                ServerMessage = String.Empty,

                SqlKey = session.SqlKey,
                AppOnline = session.AppOnline,
                UserIdentity = session.UserIdentity,
                Authenticated = session.Authenticated,
                SqlAuthorization = session.SqlAuthorization,
                WinAuthorization = session.WinAuthorization,
                Data = Data
            };
        }
    }

}
