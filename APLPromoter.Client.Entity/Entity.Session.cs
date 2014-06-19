using System;
using System.Runtime.Serialization;

namespace APLPromoter.Client.Entity
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
        public APLPromoter.Client.Entity.Workflow Workflow { get; set; }



        public static Session<Tdata> Clone<Tdata>(Session<T> session, Tdata data)
            where Tdata : class
        {
            return new Session<Tdata>
            {
                Data = data,
                AppOnline = session.AppOnline,
                Authenticated = session.Authenticated,
                SqlKey = session.SqlKey,
                SqlAuthorization = session.SqlAuthorization,
                WinAuthorization = session.WinAuthorization
            };
        }

        public Session<Tdata> Clone<Tdata>(Tdata data)
            where Tdata : class
        {
            var session = new Session<Tdata>
            {
                Data = data,
                AppOnline = this.AppOnline,
                Authenticated = this.Authenticated,
                SqlKey = this.SqlKey,
                SqlAuthorization = this.SqlAuthorization,
                WinAuthorization = this.WinAuthorization
            };
            return session;
        }
    }
}
