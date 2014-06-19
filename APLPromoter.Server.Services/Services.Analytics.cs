using System.ServiceModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using APLPromoter.Server.Services.Contracts;
using APLPromoter.Server.Data;
using APLPromoter.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace APLPromoter.Server.Services
{
    public class AnalyticService : IAnalyticService
    {
        private IAnalyticData _analyticData;

        public AnalyticService() : this(new AnalyticData()) { }
        public AnalyticService(IAnalyticData analyticRepository)
        { 
            this._analyticData = analyticRepository;
        }

        public Session<Server.Entity.Analytic.Identity> SaveIdentity(Session<Server.Entity.Analytic.Identity> sessionIn)
        {
            Session<Server.Entity.Analytic.Identity> sessionOut = _analyticData.SaveIdentity(sessionIn);
            _analyticData.Dispose();

            return sessionOut;
        }

        public Session<List<Server.Entity.Analytic.Identity>> LoadList(Session<Server.Entity.NullT> sessionIn)
        {
            Session<List<Server.Entity.Analytic.Identity>> sessionOut = _analyticData.LoadList(sessionIn);
            _analyticData.Dispose();

            return sessionOut;
        }

        public Session<List<Server.Entity.Filter>> LoadFilters(Session<Server.Entity.Analytic.Identity> sessionIn)
        {
            Session<List<Server.Entity.Filter>> sessionOut = _analyticData.LoadFilters(sessionIn);
            _analyticData.Dispose();

            return sessionOut;
        }

        public Session<List<Server.Entity.Filter>> SaveFilters(Session<Server.Entity.Analytic> sessionIn)
        {
            Session<List<Server.Entity.Filter>> sessionOut = _analyticData.SaveFilters(sessionIn);
            _analyticData.Dispose();

            return sessionOut;
        }

        public Session<List<Server.Entity.Analytic.Driver>> SaveDrivers(Session<Server.Entity.Analytic> sessionIn)
        {
            Session<List<Server.Entity.Analytic.Driver>> sessionOut = _analyticData.SaveDrivers(sessionIn);
            _analyticData.Dispose();

            return sessionOut;
        }

        public Session<List<Server.Entity.Analytic.Driver>> LoadDrivers(Session<Server.Entity.Analytic.Identity> sessionIn)
        {
            Session<List<Server.Entity.Analytic.Driver>> sessionOut = _analyticData.LoadDrivers(sessionIn);
            _analyticData.Dispose();

            return sessionOut;
        }

    }
}
