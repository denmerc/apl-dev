using System;
using System.Data;
using System.Collections.Generic;
using APLPromoter.Server.Entity;

namespace APLPromoter.Server.Data {

    class UserMap {

        #region Initialize...
        public void InitializeMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //Shared client key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.initializeMessage)
            }; service.sqlParameters.List = parameters;

        }

        public Server.Entity.Session<NullT> InitializeMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.Session<NullT> init = new Session<NullT>();
            //Single record...
            if (reader.Read()) {
                init.SessionOk = false;
                init.ClientMessage = String.Empty;
                init.ServerMessage = String.Empty;
                init.SqlKey = reader[UserMap.Names.privateKey].ToString();
                init.AppOnline = Boolean.Parse(reader[UserMap.Names.appOnline].ToString());
                init.SqlAuthorization = Boolean.Parse(reader[UserMap.Names.sqlAuthorization].ToString());
                init.WinAuthorization = Boolean.Parse(reader[UserMap.Names.winAuthorization].ToString());
            }

            if (reader != null) reader.Dispose();
            return init;
        }

        public Server.Entity.Workflow InitializeMapWorkflow(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            Boolean reading = true;
            Boolean IsValid = true;
            Boolean IsActive = false;
            Int32 rows = data.Rows.Count;
            String stepNow = String.Empty;
            String stepLast = String.Empty;
            APLPromoter.Server.Entity.Workflow workflow = null;
            List<Server.Entity.Workflow.Step> listSteps = new List<Entity.Workflow.Step>();
            List<Server.Entity.Workflow.Error> listErrors = new List<Entity.Workflow.Error>();
            List<Server.Entity.Workflow.Advisor> listAdvisor = new List<Entity.Workflow.Advisor>();
            System.Data.DataTableReader reader = data.CreateDataReader();

            //From record set...
            while (reading) {
                reading = reader.Read();
                workflow = new Workflow { Title = reader[UserMap.Names.workflowViewTitle].ToString(), Steps = listSteps };
                stepNow = (reading) ? reader[UserMap.Names.workflowStepTitle].ToString() : String.Empty;
                if (reading) {
                    listAdvisor.Add(new Entity.Workflow.Advisor(
                        Int32.Parse(reader[UserMap.Names.workflowMessageSort].ToString()),
                        reader[UserMap.Names.workflowMessageTitle].ToString()
                        ));
                    if (stepLast != stepNow) {
                        listSteps.Add(new Entity.Workflow.Step(
                            Int16.Parse(reader[UserMap.Names.workflowStepSort].ToString()),
                            reader[UserMap.Names.workflowStepName].ToString(),
                            reader[UserMap.Names.workflowStepTitle].ToString(),
                            IsValid,
                            IsActive,
                            Boolean.Parse(reader[UserMap.Names.workflowStepEnablePrevious].ToString()),
                            Boolean.Parse(reader[UserMap.Names.workflowStepEnableNext].ToString()),
                            listErrors,
                            listAdvisor,
                            (Server.Entity.WorkflowStepType)Int32.Parse(reader[UserMap.Names.workflowStepKey].ToString())
                            ));
                    }
                }
                if (!(stepLast.Equals(String.Empty) || stepLast == stepNow)) {
                    if (stepNow.Equals(String.Empty)) {
                        listSteps[listSteps.Count - 1].Advisors = listAdvisor.GetRange(0, listAdvisor.Count);
                    }
                    else {
                        listSteps[listSteps.Count - 2].Advisors = listAdvisor.GetRange(0, listAdvisor.Count - 1);
                        listAdvisor.RemoveRange(0, listAdvisor.Count - 1);
                    }
                }
                stepLast = stepNow;
            }

            if (reader != null) reader.Dispose();
            return workflow;
        }
        #endregion

        #region Authenticate...
        public void AuthenticateMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;
            String authenticateMessage = (session.SqlAuthorization) ? UserMap.Names.authenticateSqlUserMessage : ( (session.WinAuthorization) ? UserMap.Names.authenticateWinUserMessage : String.Empty );

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.login, SqlDbType.VarChar, 100, ParameterDirection.Input, session.UserIdentity.Login),
                new SqlServiceParameter(UserMap.Names.password, SqlDbType.VarChar, 100, ParameterDirection.Input, session.UserIdentity.Password.Old),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //Tenant private client key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, authenticateMessage)
            }; service.sqlParameters.List = parameters;

        }

        public Server.Entity.User.Identity AuthenticateMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.User.Identity user = null;
            //Record set...
            if (reader.Read()) {
                user = new User.Identity(
                    Int32.Parse(reader[UserMap.Names.id].ToString()),
                    reader[UserMap.Names.sqlSession].ToString(),
                    Boolean.Parse(reader[UserMap.Names.active].ToString()),
                    reader[UserMap.Names.login].ToString(),
                    reader[UserMap.Names.email].ToString(),
                    reader[UserMap.Names.userGreeting].ToString(),
                    reader[UserMap.Names.userName].ToString(),
                    reader[UserMap.Names.firstName].ToString(),
                    reader[UserMap.Names.lastName].ToString(),
                    DateTime.Parse(reader[UserMap.Names.lastLogin].ToString()),
                    reader[UserMap.Names.lastLoginText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.created].ToString()),
                    reader[UserMap.Names.createdText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.edited].ToString()),
                    reader[UserMap.Names.editedText].ToString(),
                    reader[UserMap.Names.editor].ToString(),
                    new User.Role(
                        Int32.Parse(reader[UserMap.Names.roleId].ToString()),
                        reader[UserMap.Names.roleName].ToString(),
                        reader[UserMap.Names.roleDescription].ToString()
                    )
                );
            }

            if (reader != null) reader.Dispose();
            return user;
        }
        #endregion

        #region Load Identity...
        public void LoadIdentityMapParameters(Session<Server.Entity.User.Identity> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.loadIdentityMessage)
            }; service.sqlParameters.List = parameters;
        }

        public Server.Entity.User.Identity LoadIdentityMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.User.Identity identity = null;
            //Single record...
            if (reader.Read()) {
                identity = new User.Identity(
                    Int32.Parse(reader[UserMap.Names.id].ToString()),
                    UserMap.Names.sharedKey.ToString(),
                    Boolean.Parse(reader[UserMap.Names.active].ToString()),
                    reader[UserMap.Names.login].ToString(),
                    reader[UserMap.Names.email].ToString(),
                    reader[UserMap.Names.userGreeting].ToString(),
                    reader[UserMap.Names.userName].ToString(),
                    reader[UserMap.Names.firstName].ToString(),
                    reader[UserMap.Names.lastName].ToString(),
                    DateTime.Parse(reader[UserMap.Names.lastLogin].ToString()),
                    reader[UserMap.Names.lastLoginText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.created].ToString()),
                    reader[UserMap.Names.createdText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.edited].ToString()),
                    reader[UserMap.Names.editedText].ToString(),
                    reader[UserMap.Names.editor].ToString(),
                    new User.Role(
                        Int32.Parse(reader[UserMap.Names.roleId].ToString()),
                        reader[UserMap.Names.roleName].ToString(),
                        reader[UserMap.Names.roleDescription].ToString()
                    )
                );
            }

            if (reader != null) reader.Dispose();
            return identity;
        }
        #endregion

        #region Load List...
        public void LoadListMapParameters(Session<List<Server.Entity.User.Identity>> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.loadIdentitiesMessage)
            }; service.sqlParameters.List = parameters;
        }

        public List<Server.Entity.User.Identity> LoadListMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            List<Server.Entity.User.Identity> list = new List<User.Identity>(data.Rows.Count);
            //Record set...
            while (reader.Read()) {
                list.Add(new User.Identity(
                    Int32.Parse(reader[UserMap.Names.id].ToString()),
                    UserMap.Names.sharedKey.ToString(),
                    Boolean.Parse(reader[UserMap.Names.active].ToString()),
                    reader[UserMap.Names.login].ToString(),
                    reader[UserMap.Names.email].ToString(),
                    reader[UserMap.Names.userGreeting].ToString(),
                    reader[UserMap.Names.userName].ToString(),
                    reader[UserMap.Names.firstName].ToString(),
                    reader[UserMap.Names.lastName].ToString(),
                    DateTime.Parse(reader[UserMap.Names.lastLogin].ToString()),
                    reader[UserMap.Names.lastLoginText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.created].ToString()),
                    reader[UserMap.Names.createdText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.edited].ToString()),
                    reader[UserMap.Names.editedText].ToString(),
                    reader[UserMap.Names.editor].ToString(),
                    new User.Role(
                        Int32.Parse(reader[UserMap.Names.roleId].ToString()),
                        reader[UserMap.Names.roleName].ToString(),
                        reader[UserMap.Names.roleDescription].ToString()
                    )
                ));
            }

            if (reader != null) reader.Dispose();
            return list;
        }
       #endregion

        #region Load Explorer Planning...
        public void LoadExplorerPlanningMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.UserIdentity.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.loadExplorerPlanningMessage)
            }; service.sqlParameters.List = parameters;
        }

        public Server.Entity.User.Identity LoadExplorerPlanningMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            Boolean reading = true;
            Int32 entityId = 0;
            Int32 rows = data.Rows.Count;
            String viewNow = String.Empty;
            String viewLast = String.Empty;
            List<Server.Entity.Navigator> listViews = new List<Entity.Navigator>();
            List<Server.Entity.Navigator> listNodes = new List<Entity.Navigator>();
            Server.Entity.User.Identity identity = new User.Identity { Role = new User.Role { Planning = new User.Role.Explorer() } };
            System.Data.DataTableReader reader = data.CreateDataReader();

            //From record set...
            while (reading) {
                reading = reader.Read();
                viewNow = (reading) ? reader[UserMap.Names.viewName].ToString() : String.Empty;
                entityId = (reading) ? Int32.Parse(reader[UserMap.Names.entityId].ToString()) : UserMap.Names.nodeEntityZero;
                if (reading) {
                    identity.Role.Planning.Title = reader[UserMap.Names.groupText].ToString();
                    identity.Role.Planning.Name = reader[UserMap.Names.groupName].ToString();
                    identity.Role.Planning.WorkflowGroup = (Server.Entity.WorkflowGroupType)Int32.Parse(reader[UserMap.Names.groupId].ToString());

                    if (entityId > UserMap.Names.nodeEntityZero)
                        listNodes.Add(new Entity.Navigator(
                            //EntityId, NodeHeader, NodeTitle, NodeCaption, WorkflowType, WorkflowStep, WorkflowGroup, WorkflowReadonly, Nodes
                            Int32.Parse(reader[UserMap.Names.entityId].ToString()),
                            reader[UserMap.Names.nodeHeader].ToString(),
                            reader[UserMap.Names.nodeName].ToString(),
                            reader[UserMap.Names.nodeText].ToString(),
                            (Server.Entity.WorkflowType)Int32.Parse(reader[UserMap.Names.viewId].ToString()),
                            (Server.Entity.WorkflowStepType)Int32.Parse(reader[UserMap.Names.nodeId].ToString()),
                            (Server.Entity.WorkflowGroupType)Int32.Parse(reader[UserMap.Names.groupId].ToString()),
                            Boolean.Parse(reader[UserMap.Names.viewIsReadOnly].ToString()),
                            new List<Navigator>()
                            ));
                    if (viewLast != viewNow) {
                        listViews.Add(new Entity.Navigator(
                            UserMap.Names.nodeEntityZero,
                            reader[UserMap.Names.viewName].ToString(),
                            reader[UserMap.Names.viewName].ToString(),
                            reader[UserMap.Names.viewText].ToString(),
                            (Server.Entity.WorkflowType)Int32.Parse(reader[UserMap.Names.viewId].ToString()),
                            (Server.Entity.WorkflowStepType)Int32.Parse(reader[UserMap.Names.viewDefault].ToString()),
                            (Server.Entity.WorkflowGroupType)Int32.Parse(reader[UserMap.Names.groupId].ToString()),
                            Boolean.Parse(reader[UserMap.Names.viewIsReadOnly].ToString()),
                            new List<Navigator>()
                            ));
                    }
                }
                if (!(viewLast.Equals(String.Empty) || viewLast == viewNow || listNodes.Count == 0)) {
                    if (viewNow.Equals(String.Empty)) {
                        listViews[listViews.Count - 1].Nodes = listNodes.GetRange(0, listNodes.Count);
                    }
                    else {
                        listViews[listViews.Count - 2].Nodes = listNodes.GetRange(0, (entityId > UserMap.Names.nodeEntityZero) ? listNodes.Count - 1 : listNodes.Count);
                        listNodes.RemoveRange(0, (entityId > UserMap.Names.nodeEntityZero) ? listNodes.Count - 1 : listNodes.Count);
                    }
                }
                viewLast = viewNow;
            }

            identity.Role.Planning.Navigators = listViews;

            if (reader != null) reader.Dispose();
            return identity;
        }

        public List<Server.Entity.Workflow> LoadExplorerPlanningMapWorkflow(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            Boolean reading = true;
            Boolean IsValid = true;
            Boolean IsActive = false;
            String workflowNow = String.Empty;
            String workflowLast = String.Empty;
            String stepNow = String.Empty;
            String stepLast = String.Empty;
            List<Server.Entity.Workflow> listWorkflows = new List<Entity.Workflow>();
            List<Server.Entity.Workflow.Step> listSteps = new List<Entity.Workflow.Step>();
            List<Server.Entity.Workflow.Error> listErrors = new List<Entity.Workflow.Error>();
            List<Server.Entity.Workflow.Advisor> listAdvisor = new List<Entity.Workflow.Advisor>();
            System.Data.DataTableReader reader = data.CreateDataReader();

            //From record set...
            while (reading) {
                reading = reader.Read();
                workflowNow = (reading) ? reader[UserMap.Names.workflowViewName].ToString() : String.Empty;
                stepNow = (reading) ? reader[UserMap.Names.workflowStepName].ToString() : String.Empty;
                if (reading) {
                    listAdvisor.Add(new Entity.Workflow.Advisor(
                        Int32.Parse(reader[UserMap.Names.workflowMessageSort].ToString()),
                        reader[UserMap.Names.workflowMessageTitle].ToString()
                        ));
                    if (stepLast != stepNow) {
                        listSteps.Add(new Entity.Workflow.Step(
                            Int16.Parse(reader[UserMap.Names.workflowStepSort].ToString()),
                            reader[UserMap.Names.workflowStepName].ToString(),
                            reader[UserMap.Names.workflowStepTitle].ToString(),
                            IsValid,
                            IsActive,
                            Boolean.Parse(reader[UserMap.Names.workflowStepEnablePrevious].ToString()),
                            Boolean.Parse(reader[UserMap.Names.workflowStepEnableNext].ToString()),
                            listErrors,
                            listAdvisor,
                            (Server.Entity.WorkflowStepType)Int32.Parse(reader[UserMap.Names.workflowStepKey].ToString())
                            ));
                    }
                    if (workflowLast != workflowNow) {
                        listWorkflows.Add(new Entity.Workflow(
                            reader[UserMap.Names.workflowViewName].ToString(),
                            listSteps,
                            (Server.Entity.WorkflowType)Int32.Parse(reader[UserMap.Names.workflowViewKey].ToString())
                            ));
                    }
                }
                if (!(stepLast.Equals(String.Empty) || stepLast == stepNow)) {
                    if (stepNow.Equals(String.Empty)) {
                        listSteps[listSteps.Count - 1].Advisors = listAdvisor.GetRange(0, listAdvisor.Count);
                    }
                    else {
                        listSteps[listSteps.Count - 2].Advisors = listAdvisor.GetRange(0, listAdvisor.Count - 1);
                        listAdvisor.RemoveRange(0, listAdvisor.Count - 1);
                    }
                }
                if (!(workflowLast.Equals(String.Empty) || workflowLast == workflowNow)) {
                    if (workflowNow.Equals(String.Empty)) {
                        listWorkflows[listWorkflows.Count - 1].Steps = listSteps.GetRange(0, listSteps.Count);
                    }
                    else {
                        listWorkflows[listWorkflows.Count - 2].Steps = listSteps.GetRange(0, listSteps.Count - 1);
                        listSteps.RemoveRange(0, listSteps.Count - 1);
                    }
                }
                stepLast = stepNow;
                workflowLast = workflowNow;
            }

            if (reader != null) reader.Dispose();
            return listWorkflows;
        }
        #endregion

        #region Load Explorer Tracking...
        public void LoadExplorerTrackingMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.UserIdentity.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.loadExplorerTrackingMessage)
            }; service.sqlParameters.List = parameters;
        }

        public Server.Entity.User.Identity LoadExplorerTrackingMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.User.Identity identity = new User.Identity();
            //Single record...
            //if (reader.Read()) {
            //    identity.Role.Tracking = reader[UserMap.Names.xmlDataColumn].ToString();
            //}
            if (reader != null) reader.Dispose();
            return identity;
        }
        #endregion

        #region Load Explorer Reporting...
        public void LoadExplorerReportingMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.UserIdentity.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.loadExplorerReportingMessage)
            }; service.sqlParameters.List = parameters;
        }

        public Server.Entity.User.Identity LoadExplorerReportingMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.User.Identity identity = new User.Identity();
            //Single record...
            //if (reader.Read()) {
            //    identity.Role.Reporting = reader[UserMap.Names.xmlDataColumn].ToString();
            //}
            if (reader != null) reader.Dispose();
            return identity;
        }
        #endregion

        #region Save Password...
        public void SavePasswordMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.updateCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.login, SqlDbType.VarChar, 100, ParameterDirection.Input, session.UserIdentity.Login),
                new SqlServiceParameter(UserMap.Names.password, SqlDbType.VarChar, 100, ParameterDirection.Input, session.UserIdentity.Password.New),
                new SqlServiceParameter(UserMap.Names.oldPassword, SqlDbType.VarChar, 100, ParameterDirection.Input, session.UserIdentity.Password.Old),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //user session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.savePasswordMessage)
            }; service.sqlParameters.List = parameters;
        }
        #endregion

        #region Save Identity...
        public void SaveIdentityMapParameters(Session<Server.Entity.User.Identity> session, ref Server.Data.SqlService service) {

            //Map the command...
            Int16 insertId = 0;
            service.SqlProcedure = UserMap.Names.updateCommand;
            String updateCommandMessage = (session.Data.Id == insertId) ? UserMap.Names.saveIdentityInsertMessage : UserMap.Names.saveIdentityUpdateMessage;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.roleId, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Role.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.login, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.Login),
                new SqlServiceParameter(UserMap.Names.firstName, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.FirstName),
                new SqlServiceParameter(UserMap.Names.lastName, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.LastName),
                new SqlServiceParameter(UserMap.Names.email, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.Email),
                new SqlServiceParameter(UserMap.Names.password, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.Password.New),
                new SqlServiceParameter(UserMap.Names.oldPassword, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.Password.Old),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //user session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, updateCommandMessage)
            }; service.sqlParameters.List = parameters;
        }
        
        public Server.Entity.User.Identity SaveIdentityMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.User.Identity identity = null;
            //Single record...
            if (reader.Read()) {
                identity = new User.Identity(
                    Int32.Parse(reader[UserMap.Names.id].ToString()),
                    UserMap.Names.sharedKey.ToString(),
                    Boolean.Parse(reader[UserMap.Names.active].ToString()),
                    reader[UserMap.Names.login].ToString(),
                    reader[UserMap.Names.email].ToString(),
                    reader[UserMap.Names.userGreeting].ToString(),
                    reader[UserMap.Names.userName].ToString(),
                    reader[UserMap.Names.firstName].ToString(),
                    reader[UserMap.Names.lastName].ToString(),
                    DateTime.Parse(reader[UserMap.Names.lastLogin].ToString()),
                    reader[UserMap.Names.lastLoginText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.created].ToString()),
                    reader[UserMap.Names.createdText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.edited].ToString()),
                    reader[UserMap.Names.editedText].ToString(),
                    reader[UserMap.Names.editor].ToString(),
                    new User.Role(
                        Int32.Parse(reader[UserMap.Names.roleId].ToString()),
                        reader[UserMap.Names.roleName].ToString(),
                        reader[UserMap.Names.roleDescription].ToString()
                    ));
            }

            if (reader != null) reader.Dispose();
            return identity;
        }
        #endregion

        #region Load Enumeration...
        public List<Server.Entity.SQLEnumeration> EnumerationMapData(System.Data.DataTable data) {
            System.Data.DataTableReader reader = data.CreateDataReader();
            List<Server.Entity.SQLEnumeration> list = new List<SQLEnumeration>(data.Rows.Count);

            while (reader.Read()) {
                list.Add(new SQLEnumeration(
                    Int16.Parse(reader[UserMap.Names.enumSort].ToString()),
                    Int16.Parse(reader[UserMap.Names.enumValue].ToString()),
                    reader[UserMap.Names.enumName].ToString(),
                    reader[UserMap.Names.enumDescription].ToString()
                ));
            }

            if (reader != null) reader.Dispose();
            return list;

        } 
        #endregion

        #region Entity map...
        //Database names...
        public class Names {
            #region Select commands...
            public const String selectCommand = "dbo.aplUserSelect";
            public const String initializeMessage = "selectInitialize";
            public const String loadIdentityMessage = "selectIdentity";
            public const String loadIdentitiesMessage = "selectIdentities";
            public const String loadAuthenticatedUserMessage = "selectUser";
            public const String loadExplorerPlanningMessage = "selectExplorerPlanning";
            public const String loadExplorerTrackingMessage = "selectExplorerTracking";
            public const String loadExplorerReportingMessage = "selectExplorerReporting";
            public const String authenticateSqlUserMessage = "selectSqlUser";
            public const String authenticateWinUserMessage = "selectWinUser";
            #endregion

            #region Update commands...
            public const String updateCommand = "dbo.aplUserUpdate";
            public const String savePasswordMessage = "updatePassword";
            public const String saveIdentityInsertMessage = "insertIdentity";
            public const String saveIdentityUpdateMessage = "updateIdentity";
            #endregion

            #region Default parameters...
            public const String id = "id";
            public const String sqlSession = "session";
            public const String sqlMessage = "message";
            #endregion

            #region Fields Identity...
            public const String userId = "id";
            public const String login = "login";
            public const String active = "active";
            public const String password = "password";
            public const String oldPassword = "passwordold";
            public const String roleId = "role";
            public const String roleName = "roleName";
            public const String roleDescription = "roleText";
            public const String email = "email";
            public const String userName = "username";
            public const String userGreeting = "usergreeting";
            public const String firstName = "firstname";
            public const String lastName = "lastname";
            public const String lastLoginText = "lastLoginText";
            public const String createdText = "createdText";
            public const String editedText = "editedText";
            public const String lastLogin = "lastLogin";
            public const String created = "created";
            public const String edited = "edited";
            public const String editor = "lastEditor";
            public const String groupId= "groupId";
            public const String groupName = "groupName";
            public const String groupText = "groupTitle";
            public const String viewId = "viewId";
            public const String viewDefault = "viewDefault";
            public const String viewName = "viewName";
            public const String viewText = "viewTitle";
            public const String viewIsReadOnly = "viewReadonly";
            public const String viewDescription = "viewDescription";
            public const String mapId = "mapId";
            public const String entityId = "entityId";
            public const String nodeId = "nodeId";
            public const String nodeHeader = "nodeHeader";
            public const String nodeName = "nodeName";
            public const String nodeText = "nodeTitle";
            public const Int32 nodeEntityZero = 0;
            #endregion

            #region Fields Session...
            //public const String sqlKey = "sqlKey";
            public const String appOnline = "appOnline";
            public const String privateKey = "privateKey";
            public const String sqlAuthorization = "sqlAuthorization";
            public const String winAuthorization = "winAuthorization";
            public const String sharedKey = "72B9ED08-5D12-48FD-9CF7-56A3CA30E660";
            #endregion

            #region Fields Workflow...
            public const String workflowGroupKey= "workflowGroupKey";
            public const String workflowGroupName = "workflowGroupName";
            public const String workflowGroupTitle = "workflowGroupTitle";
            public const String workflowViewKey = "workflowViewKey";
            public const String workflowViewName = "workflowViewName";
            public const String workflowViewTitle = "workflowViewTitle";
            public const String workflowStepKey = "workflowStepKey";
            public const String workflowStepName = "workflowStepName";
            public const String workflowStepTitle = "workflowStepTitle";
            public const String workflowMessageTitle = "workflowMessageTitle";
            public const String workflowStepSort = "workflowStepSort";
            public const String workflowMessageSort = "workflowMessageSort";
            public const String workflowStepEnablePrevious = "workflowStepEnablePrevious";
            public const String workflowStepEnableNext = "workflowStepEnableNext";
            #endregion

            #region Fields Enumeration...
            public const String enumSort = "sort";
            public const String enumValue = "value";
            public const String enumName = "name";
            public const String enumDescription = "description";
            #endregion

        }
        //Database enumerations...
        public enum DataSets { entitydata=0, workflow=1, enumeration=1 };
        #endregion

        #region Message map result sets...
        //selectInitialize - set 1: sqlAuthorization,winAuthorization,appOnline,privateKey 
        //                        set 2: workflowGroupKey,workflowGroupName,workflowGroupTitle,workflowViewKey,workflowViewName,workflowViewTitle,
        //		                                workflowStepKey,workflowStepName,workflowStepTitle,workflowMessageTitle,workflowStepSort,workflowMessageSort,workflowStepEnablePrevious,workflowStepEnableNext
        //selectSqlUser -  set 1: id,session,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        //                        set 2 user role enumerations: value,name,description,sort
        //selectWinUser - id,session,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        //selectAuthenticatedUser - id,session,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        //selectIdentity - set 1: id,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        //                        set 2 user role enumerations: value,name,description,sort
        //selectIdentities - id,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        //selectExplorerPalnning - userId,userLogin,userSession,userRole,userRoleName,groupId,groupName,groupTitle,viewId,viewName,viewTitle,viewReadonly,nodeId,nodeHeader,nodeName,nodeTitle,entityId,mapId

        //updateIdentity - id,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        #endregion

    }
}
