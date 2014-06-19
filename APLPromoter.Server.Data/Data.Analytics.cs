using System;
using System.Collections.Generic;
using APLPromoter.Server.Entity;

namespace APLPromoter.Server.Data {

    public interface IAnalyticData {
        void Dispose();
        Session<List<Server.Entity.Analytic.Identity>> LoadList(Session<Server.Entity.NullT> session);
        Session<Server.Entity.Analytic.Identity> SaveIdentity(Session<Server.Entity.Analytic.Identity> session);
        Session<List<Server.Entity.Filter>> LoadFilters(Session<Server.Entity.Analytic.Identity> session);
        Session<List<Server.Entity.Filter>> SaveFilters(Session<Server.Entity.Analytic> session);
        Session<List<Server.Entity.Analytic.Driver>> LoadDrivers(Session<Server.Entity.Analytic.Identity> session);
        Session<List<Server.Entity.Analytic.Driver>> SaveDrivers(Session<Server.Entity.Analytic> session);
        Session<List<Server.Entity.PriceList>> LoadPriceLists(Session<Server.Entity.Analytic.Identity> session);
        Session<List<Server.Entity.PriceList>> SavePriceLists(Session<Server.Entity.Analytic> session);
    }

    public class AnalyticData : IAnalyticData {

        #region Constants...
        const String invalid = "Invalid:";
        const String connectionName = "defaultConnectionString";
        const String aplServiceEventLog = "APLPromoterServerData";
        #endregion

        #region Variables...
        private System.Diagnostics.EventLog localServiceLog;
        private APLPromoter.Server.Data.AnalyticMap sqlMapper;
        private APLPromoter.Server.Data.SqlService sqlService;
        #endregion

        private String sqlConnection {
            get {
                return System.Configuration.ConfigurationManager.AppSettings[connectionName];
            }
        }

        public AnalyticData() {

            sqlMapper = new AnalyticMap();            
            sqlService = new SqlService(this.sqlConnection);
            localServiceLog = new System.Diagnostics.EventLog();
            //if (!System.Diagnostics.EventLog.SourceExists(cseServiceEventLog)) EventLog.CreateEventSource(cseServiceEventLog, "Application");
            //Setup <cseServiceEventLog> event source manually through registry key: HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Eventlog\Application
            //To resolve message IDs create a RG_EXPAND_SZ attribute, named "EventMessageFile" to: "C:\WINDOWS\Microsoft.NET\Framework\<current version>\EventLogMessages.dll"
            localServiceLog.Source = aplServiceEventLog;

        }

        ~AnalyticData() {
            if (sqlService != null) sqlService.ExecuteCloseConnection();
        }

        public Session<Server.Entity.NullT> LoadWorkflow(Session<Server.Entity.NullT> sessionIn)
        {
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.NullT> sessionOut = sessionIn.Clone<NullT>(new NullT());

            try
            {
                sqlMapper.LoadWorkflowMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk)
                {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse)
                    {
                        sessionOut.Workflow = sqlMapper.LoadWorkflowMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex)
            {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally
            {
                //SQL Service error...
                if (!sqlService.SqlStatusOk)
                {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse)
                {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;

        }
        
        public Session<List<Server.Entity.Analytic.Identity>> LoadList(Session<Server.Entity.NullT> sessionIn) {
            
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.Analytic.Identity>> sessionOut = Session<NullT>.Clone<List<Analytic.Identity>>(sessionIn);

            try {
                sqlMapper.LoadListMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadListMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<Server.Entity.Analytic.Identity> SaveIdentity(Session<Server.Entity.Analytic.Identity> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.Analytic.Identity> sessionOut = sessionIn.Clone<Analytic.Identity>(new Analytic.Identity());

            try {
                sqlMapper.SaveIdentityMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data.Id = (sqlMapper.SaveIdentityMapData(dataTable, sqlService)).Id;
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<List<Server.Entity.Filter>> LoadFilters(Session<Server.Entity.Analytic.Identity> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.Filter>> sessionOut = Session<Analytic.Identity>.Clone<List<Filter>>(sessionIn);

            try {
                sqlMapper.LoadFiltersMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadFiltersMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<List<Server.Entity.Filter>> SaveFilters(Session<Server.Entity.Analytic> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.Filter>> sessionOut = Session<Analytic>.Clone<List<Filter>>(sessionIn);

            try {
                sqlMapper.SaveFiltersMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadFiltersMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<List<Server.Entity.Analytic.Driver>> LoadDrivers(Session<Server.Entity.Analytic.Identity> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.Analytic.Driver>> sessionOut = Session<Analytic.Identity>.Clone<List<Analytic.Driver>>(sessionIn);

            try {
                sqlMapper.LoadDriversMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadTypesMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<List<Server.Entity.Analytic.Driver>> SaveDrivers(Session<Server.Entity.Analytic> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.Analytic.Driver>> sessionOut = Session<Analytic>.Clone<List<Analytic.Driver>>(sessionIn);

            try {
                sqlMapper.SaveDriversMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadTypesMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<List<Server.Entity.PriceList>> LoadPriceLists(Session<Server.Entity.Analytic.Identity> sessionIn) {
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.PriceList>> sessionOut = Session<Analytic.Identity>.Clone<List<PriceList>>(sessionIn);

            try {
                sqlMapper.LoadPricelistsMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadPricelistsMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<List<Server.Entity.PriceList>> SavePriceLists(Session<Server.Entity.Analytic> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.PriceList>> sessionOut = Session<Analytic>.Clone<List<PriceList>>(sessionIn);

            try {
                sqlMapper.SavePricelistsMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadPricelistsMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public void Dispose() {
            if (sqlService != null)
                if (!sqlService.ExecuteCloseConnection())
                    this.localServiceLog.WriteEntry(sqlService.SqlStatusMessage);
        }
    }
}
