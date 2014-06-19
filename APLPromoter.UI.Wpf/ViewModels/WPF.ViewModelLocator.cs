using APLPromoter.Client;
using APLPromoter.Client.Contracts;
using APLPromoter.Core.Reactive;
using APLPromoter.UI.Wpf.ViewModel;
using APLPromoter.UI.Wpf.Views;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Reflection;
using System.IO;
using System;

namespace APLPromoter.UI.Wpf.ViewModel
{
    [Export]
    public class ViewModelLocator
    {
        public static CompositionContainer Container { get; set; }
        
        public ViewModelLocator()
        {
            var conventions = new RegistrationBuilder();
            conventions.ForType<MainViewModel>().Export<MainViewModel>().SetCreationPolicy(CreationPolicy.Shared);
            conventions.ForType<ViewRouter>().Export<ViewRouter>().SetCreationPolicy(CreationPolicy.Shared);
            conventions.ForType<IUserService>().Export<UserClient>().SetCreationPolicy(CreationPolicy.Shared);
            conventions.ForType<IAnalyticService>().Export<AnalyticClient>().SetCreationPolicy(CreationPolicy.Shared);
            conventions.ForType<IEventAggregator>().Export<EventAggregator>().SetCreationPolicy(CreationPolicy.Shared);

            var appCatalog = new AssemblyCatalog(typeof(App).Assembly, conventions);
            var proxyCatalog = new AssemblyCatalog(typeof(APLPromoter.Client.AnalyticClient).Assembly, conventions);
            var coreCatalog = new AssemblyCatalog(typeof(APLPromoter.Core.Reactive.EventAggregator).Assembly, conventions);
            var aggregate = new AggregateCatalog(appCatalog, proxyCatalog, coreCatalog);
            Container = new CompositionContainer(aggregate);
           
            var batch = new CompositionBatch();
            batch.AddExportedValue<IEventAggregator>(EventPublisher);
            batch.AddExportedValue<IUserService>(UserService);
            batch.AddExportedValue<IAnalyticService>(AnalyticService);
            
            var mvm = new MainViewModel(AnalyticService, UserService, EventPublisher, this, null);
            batch.AddExportedValue<MainViewModel>(mvm);
            //Container.ComposeExportedValue(mvm);

            Container.Compose(batch);
            //App.Current.Resources.Add("Locator", this);



            Container.ComposeParts(this, conventions);
           
        }

        public IEventAggregator EventPublisher 
        {
            get
            {
                return Container.GetExportedValue<EventAggregator>();
            }
        }

        public string Version
        {
            get
            {
                return Container.GetExportedValue<string>("Version");
            }
        }
        private MainViewModel _mainViewModel;
        public MainViewModel MainViewModel
        {
            get
            {
                return Container.GetExportedValue<MainViewModel>();
            }
        }

        public ViewRouter ViewRouter
        {
            get
            {
                return Container.GetExportedValue<ViewRouter>();
            }
        }

        public ViewModelLocator VMLocator
        {
            get
            {
                return Container.GetExportedValue<ViewModelLocator>();
            }
        }

        public IUserService UserService
        {
            get
            {   
                return Container.GetExportedValue<UserClient>();
            }
        }

        public IAnalyticService AnalyticService
        {
            get
            {   
                return Container.GetExportedValue<AnalyticClient>();
            }
        }

        public static void Cleanup()
        {
            //TODO: Must dispose of unmanaged resources - i.e wcf client proxies!
            
            var analyticResources = Container.GetExportedValues<IAnalyticService>();

            foreach (var proxy in analyticResources)
            {
              if (proxy != null && proxy is IDisposable)
                (proxy as IDisposable).Dispose();
                
            }

            var userResources = Container.GetExportedValues<IUserService>();

            foreach (var proxy in userResources)
            {
                if (proxy != null && proxy is IDisposable)
                    (proxy as IDisposable).Dispose();

            }
            Container.Dispose();
        }
    }
}




