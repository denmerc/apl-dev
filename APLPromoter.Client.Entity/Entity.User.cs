using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ReactiveUI;
using APLPromoter.Core;
namespace APLPromoter.Client.Entity
{
    [DataContract]
    public class User : ReactiveValidatedEntity
    {
        [DataContract]
        public class Identity : ReactiveValidatedEntity
        {

            #region Initialize...
            public Identity() { }
            public Identity(
                Int32 Id,
                String sqlKey,
                String Login,
                Boolean Active,
                String Email,
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
                )
            {
                this.Id = Id;
                this.sqlKey = sqlKey;
                this.Active = Active;
                this.Login = Login;
                this.Email = Email;
                this.FirstName = FirstName;
                this.LastName = LastName;
                this.Greeting = Greeting;
                this.LastLoginText = LastLoginText;
                this.CreatedText = CreatedText;
                this.EditedText = EditedText;
                this.LastLogin = LastLogin;
                this.Created = Created;
                this.Edited = Edited;
                this.Editor = Editor;
                this.Role = Role;
                this.Name = Name;
            }
            #endregion

            private Int32 _Id;
            [DataMember]
            public Int32 Id
            {
                get{return _Id;}
                set
                {
                 _Id = value;
                 this.RaiseAndSetIfChanged(ref _Id, value);
                }
            }
            
            [DataMember]
            public String sqlKey { get; set; }
            [DataMember]
            public Boolean Active { get; set; }
            [DataMember]
            public Role Role { get; set; }
            [DataMember]
            public String Login { get; set; }
            [DataMember]
            public Password Password { get; set; }
            [DataMember]
            public String Email { get; set; }
            [DataMember]
            public String Name { get; set; }
            [DataMember]
            public String FirstName { get; set; }
            [DataMember]
            public String LastName { get; set; }
            [DataMember]
            public String Greeting { get; set; }
            [DataMember]
            public DateTime LastLogin { get; set; }
            [DataMember]
            public String LastLoginText { get; set; }
            [DataMember]
            public DateTime Created { get; set; }
            [DataMember]
            public String CreatedText { get; set; }
            [DataMember]
            public DateTime Edited { get; set; }
            [DataMember]
            public String EditedText { get; set; }
            [DataMember]
            public String Editor { get; set; }
        }

        [DataContract]
        public class Role
        {

            #region Initialize...
            public Role() { }
            public Role(
                Int32 Id,
                String Name,
                String Description
            )
            {
                this.Id = Id;
                this.Name = Name;
                this.Description = Description;
            }
            #endregion

            [DataMember]
            public Int32 Id { get; set; }
            [DataMember]
            public String Name { get; set; }
            [DataMember]
            public String Description { get; set; }
            [DataMember]
            public Entity.User.Role.Explorer Planning { get; set; }
            [DataMember]
            public Entity.User.Role.Explorer Tracking { get; private set; }
            [DataMember]
            public Entity.User.Role.Explorer Reporting { get; private set; }
            [DataMember]
            public List<Entity.SQLEnumeration> Enumerations { get; private set; }
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
                public String Name{ get;  set; }
                [DataMember]
                public String Title{ get;  set; }
                [DataMember]
                public List<Entity.Navigator> Navigators{ get;  set; } 
                [DataMember]
                public List<Entity.Workflow> Workflows{ get;  set; }
                [DataMember]
                public WorkflowGroupType WorkflowGroup{ get; private set; }
                [IgnoreDataMember]
                public Boolean IsExpanded{ get;  set; }
            }
        }

        [DataContract]
        public class Password
        {

            #region Initialize...
            public Password() { }
            public Password(
                String Old,
                String New
            )
            {
                this.Old = Old;
                this.New = New;
            }
            #endregion

            [DataMember]
            public String Old { get; set; }
            [DataMember]
            public String New { get; set; }
        }

    }
}
