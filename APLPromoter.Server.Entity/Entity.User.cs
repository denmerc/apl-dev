using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace APLPromoter.Server.Entity
{
    [DataContract]
    public class User
    {
        [DataContract]
        public class Identity {

            #region Initialize...
            public Identity() { }
            public Identity(
                Int32 Id,
                String sqlKey,
                Boolean Active,
                String Login,
                String Email,
                String Greeting,
                String Name,
                String FirstName, 
                String LastName,
                DateTime LastLogin,
                String LastLoginText,
                DateTime Created,
                String CreatedText, 
                DateTime Edited,
                String EditedText, 
                String Editor,
                Role Role
                ) {
                    this.Id = Id;
                    this.sqlKey = sqlKey;
                    this.Active = Active;
                    this.Login = Login;
                    this.Email = Email;
                    this.Greeting = Greeting;
                    this.Name = Name;
                    this.FirstName = FirstName;
                    this.LastName = LastName;
                    this.LastLoginText = LastLoginText;
                    this.CreatedText = CreatedText;
                    this.EditedText = EditedText;
                    this.LastLogin = LastLogin;
                    this.Created = Created;
                    this.Edited = Edited;
                    this.Editor = Editor;
                    this.Role = Role;
            }
            #endregion

            [DataMember]
            public Int32 Id; //CLIENT { get; private set; }
            [DataMember]
            public String sqlKey; //CLIENT { get; private set; }
            [DataMember]
            public Boolean Active; //CLIENT { get; set; }
            [DataMember]
            public Role Role; //CLIENT { get; set; }
            [DataMember]
            public String Login; //CLIENT { get; set; }
            [DataMember]
            public Password Password; //CLIENT { get; set; }
            [DataMember]
            public String Email; //CLIENT { get; set; }
            [DataMember]
            public String Name; //CLIENT { get; set; }
            [DataMember]
            public String FirstName; //CLIENT { get; set; }
            [DataMember]
            public String LastName; //CLIENT { get; set; }
            [DataMember]
            public String Greeting; //CLIENT { get; set; }
            [DataMember]
            public DateTime LastLogin; //CLIENT { get; private set; }
            [DataMember]
            public String LastLoginText; //CLIENT { get; private set; }
            [DataMember]
            public DateTime Created; //CLIENT { get; private set; }
            [DataMember]
            public String CreatedText; //CLIENT { get; private set; }
            [DataMember]
            public DateTime Edited; //CLIENT { get; private set; }
            [DataMember]
            public String EditedText; //CLIENT { get; private set; }
            [DataMember]
            public String Editor; //CLIENT { get; private set; }
        }

        [DataContract]
        public class Role {

            #region Initialize...
            public Role() {}
            public Role(
                Int32 Id,
                String Name,
                String Description
            ) {
                this.Id = Id;
                this.Name = Name;
                this.Description = Description;
            }
            #endregion

            [DataMember]
            public Int32 Id; //CLIENT { get; private set; }
            [DataMember]
            public String Name; //CLIENT { get; private set; }
            [DataMember]
            public String Description; //CLIENT { get; private set; }
            [DataMember]
            public Entity.User.Role.Explorer Planning; //CLIENT { get; private set; }
            [DataMember]
            public Entity.User.Role.Explorer Tracking; //CLIENT { get; private set; }
            [DataMember]
            public Entity.User.Role.Explorer Reporting; //CLIENT { get; private set; }
            [DataMember]
            public List<Entity.SQLEnumeration> Enumerations; //CLIENT { get; private set; }

            [DataContract]
            public class Explorer
            {
                #region Initialize...
                public Explorer() { }
                public Explorer(
                    String Name,
                    String Title,
                    List<Navigator> Navigators,
                    List<Workflow> Workflows,
                    WorkflowGroupType WorkflowGroup
                ) {
                    this.Name = Name;
                    this.Title = Title;
                    this.Navigators = Navigators;
                    this.Workflows = Workflows;
                    this.WorkflowGroup = WorkflowGroup;
                }
                #endregion

                [DataMember]
                public String Name;
                [DataMember]
                public String Title;
                [DataMember]
                public List<Entity.Navigator> Navigators;
                [DataMember]
                public List<Entity.Workflow> Workflows;
                [DataMember]
                public WorkflowGroupType WorkflowGroup;
            }

        }

        [DataContract]
        public class Password {

            #region Initialize...
            public Password() { }
            public Password(
                String Old,
                String New
            ) {
                this.Old = Old;
                this.New = New;
            }
            #endregion

            [DataMember]
            public String Old; //CLIENT { get; set; }
            [DataMember]
            public String New; //CLIENT { get; set; }
        }

    }
}
