using APLPromoter.Client.Contracts;
using ReactiveUI;
using System;
using System.Collections.Generic;
using APLPromoter.Client.Entity;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Linq;
using APLPromoter.Core.Reactive;
using System.ComponentModel.Composition;
using System.Threading;


namespace APLPromoter.UI.Wpf.ViewModel
{
    [Export]
    public class AnalyticViewModel : ReactiveValidatedEntity, IRoutableViewModel
    {
        
        public AnalyticViewModel(Session<NullT> session, IAnalyticService analyticService,  IEventAggregator publisher, Int32 modelId = 0)
        {
            if (modelId == 0) { ActiveStep = 0; }
            Publisher = publisher;
            Session = session;
            Workflow = session.UserIdentity.Role.Planning.Workflows.SingleOrDefault(x => x.ThisWorkflowType == WorkflowType.PlanningAnalytics);
            AnalyticService = analyticService;

            /*  onNavigatorClicked 
             * 
             *  IF modelId is 0 (change of step)
             *      -> set ListModel to list
             *      -> selectedStepVM to listVM
             *  -ELSE- (change of id)
             *      -> set IdentityModel to identity 
             *      -> selectedStepVM = identityVM(id) ->filters -> priceLists 
             *      -> then loadlist
             *  
             *  onSelectedIdentity(identity from list)
             *      -> set IdentityModel to selected from list
             *      -> selectedStepVM = reload IdentityVM(identity) -> filters etc;
             *      
             *  onTabClicked (only step changes)
             *      -> SelectedStepVM(step)
             *              -> show cached viewmodel based on stepType list
             *      
             *
             */

            //onTabClicked
            ChangeStepCommand = new ReactiveCommand(null);
            ChangeStepCommand.Subscribe(step =>
            {
                ChangeStep(((Workflow.Step)step).ThisStepType);
                this.raisePropertyChanged("SelectedStepViewModel");
            });

            //onSelectedIdentity (from list)
            IdentitySelectedCommand = new ReactiveCommand(null);
            IdentitySelectedCommand.Subscribe(analytic =>
                {
                    Model = (Analytic.Identity)analytic;
                    ChangeStep(WorkflowStepType.PlanningAnalyticsIdentity);
                });

            var ModelListCommand = new ReactiveCommand(null);
            var pushList = ModelListCommand.RegisterAsyncTask(_ => LoadList())
                .Subscribe(response =>
                {

                    ModelList.Clear();
                     var modelList = response.Data.CreateDerivedCollection(a => a);
                     ModelList.AddRange(modelList);

                    //onNavigatorClicked 
                    if (modelId == 0)
                    {
                        //check cache and refresh or loadlist
                        //show list view
                        ChangeStep(WorkflowStepType.PlanningAnalyticsMyAnalytics);
                    }
                    else
                    {
                        Model = ModelList.Where(a => a.Id == modelId).FirstOrDefault();
                        ChangeStep(WorkflowStepType.PlanningAnalyticsIdentity);
                    }
                });

            var canNavigateForward = this.WhenAny(x => x.ActiveStep, s =>
                s.Value != Workflow.Steps.Count - 1
            );

            NextCommand = new ReactiveCommand(canNavigateForward);
            NextCommand.Subscribe( _ =>
                {
                    var nextStepIndex = Workflow.Steps.FindIndex(st => st.IsActive == true) + 1;
                    if (nextStepIndex < Workflow.Steps.Count)
                    {
                        var nextStep = Workflow.Steps[nextStepIndex];
                        ChangeStep(nextStep.ThisStepType);
                    }
                });

                    //var step = Workflow.Steps.SkipWhile(st => st.IsActive == true).Skip(1).FirstOrDefault();
                    //if(step != null)
                    //{
                    //    ChangeStep(step.ThisStepType);
                    //}


            var canNavigatePrevious = this.WhenAny(x => x.ActiveStep, s =>
                s.Value >= 1
            );

            PreviousCommand = new ReactiveCommand(canNavigatePrevious);
            PreviousCommand.Subscribe( _ =>
            {
                var prevStepIndex = Workflow.Steps.FindIndex(st => st.IsActive == true) - 1;
                if (prevStepIndex >= 0)
                {
                    var prevStep = Workflow.Steps[prevStepIndex];
                    ChangeStep(prevStep.ThisStepType);

                }
            });

            ModelListCommand.Execute(null);
            this.raisePropertyChanged("SelectedStepViewModel");
        }

        public void ChangeStep(WorkflowStepType stepType)
        {
            var step = Workflow.Steps
                .FirstOrDefault(s => s.ThisStepType == stepType);
            if (step != null)
            {
                //Todo : dispose of susbscriptions
                if (step.StepViewModel != null)
                {
                    step.StepViewModel.Dispose();
                    step.StepViewModel = null;
                }
                //Refresh here -OR- replace if necessary & unregister observables and event subscriptions
                //eg. if new identity is selected refresh identity, filters, pricelists
                switch (stepType)
                {
                    case WorkflowStepType.PlanningAnalyticsMyAnalytics:

                        step.StepViewModel = new AnalyticListViewModel(ModelList);
                        break;
                    case WorkflowStepType.PlanningAnalyticsIdentity:
                        step.StepViewModel = new AnalyticEditIdentityViewModel(Model, Session, AnalyticService);
                        break;
                    case WorkflowStepType.PlanningAnalyticsFilters:
                        step.StepViewModel = new AnalyticEditFiltersViewModel(new Analytic(), Session, AnalyticService);
                        break;
                    case WorkflowStepType.PlanningAnalyticsPriceLists:
                        step.StepViewModel = new AnalyticEditPriceListsViewModel();
                        break;
                    case WorkflowStepType.PlanningAnalyticsValueDrivers:
                        step.StepViewModel = new AnalyticEditValueDriversViewModel();
                        break;
                    case WorkflowStepType.PlanningAnalyticsResults:
                        step.StepViewModel = new AnalyticEditResultsViewModel();
                        break;
                }
                StepHeader = step.Name;
                ActiveStep = step.Index -1;
                this.raisePropertyChanged("ActiveStep");
            }
            SelectedStepViewModel = Workflow.Steps.FirstOrDefault(st => st.ThisStepType == stepType).StepViewModel;
            Publisher.Publish<WorkflowStepType>(stepType);
            this.raisePropertyChanged("SelectedStepViewModel");
            
        }

        private int _ActiveStep = 1;
        public int ActiveStep
        {
            get
            {
                return _ActiveStep;
            }
            set
            {

                _ActiveStep = value;
                this.RaiseAndSetIfChanged(ref _ActiveStep, value);

            }
        }

        private ReactiveValidatedEntity _SelectedStepViewModel;
        public ReactiveValidatedEntity SelectedStepViewModel
        {
            get
            { 
                return _SelectedStepViewModel; 
            }
            set
            {

                _SelectedStepViewModel = value;
                this.RaiseAndSetIfChanged(ref _SelectedStepViewModel, value);

            }
        }

        Session<NullT> _session;
        public Session<NullT> Session
        {
            get
            { return _session; }
            set { this.RaiseAndSetIfChanged(ref _session, value); }
        }

        public ReactiveCommand ChangeStepCommand { get; set; }
        public ReactiveCommand IdentitySelectedCommand { get; set; }
        private ReactiveList<Analytic.Identity> _ModelList = new ReactiveList<Analytic.Identity>();
        public ReactiveList<Analytic.Identity> ModelList 
        {
            get
            {
                return _ModelList;
            }
            set
            {
                if (_ModelList != value)
                {
                    _ModelList = value;
                    this.RaiseAndSetIfChanged(ref _ModelList, value);
                }
            }
        }

        private Analytic.Identity _Model;
        public Analytic.Identity Model 
        {
            get
            {
                return _Model;
            }
            set
            {
                if (_Model != value)
                {
                    _Model = value;
                    this.RaiseAndSetIfChanged(ref _Model, value);
                }
            }
        }

        private ReactiveList<APLPromoter.Client.Entity.Analytic.Driver> _Drivers = new ReactiveList<APLPromoter.Client.Entity.Analytic.Driver>();
        public ReactiveList<APLPromoter.Client.Entity.Analytic.Driver> Drivers
        {
            get
            {
                return this._Drivers;
            }
            set
            {
                if (_Drivers != value)
                {
                    _Drivers = value;
                    this.RaiseAndSetIfChanged(ref _Drivers, value);
                }
            }
        }

        private ReactiveList<Filter> _Filters = new ReactiveList<Filter>();
        public ReactiveList<Filter> Filters
        {
            get
            {
                return this._Filters;
            }
            set
            {
                if (_Filters != value)
                {
                    _Filters = value;
                    this.RaiseAndSetIfChanged(ref _Filters, value);
                }
            }
        }
        public IEventAggregator Publisher { get; set; }
        public IAnalyticService AnalyticService { get; set; }
        public IScreen HostScreen { get; set; }
        public IRoutingState Router { get; set; }
        public string UrlPathSegment { get { return "AnalyticEdit"; } }

        public async Task<Session<List<Analytic.Identity>>> LoadList()
        {
            return await Task.Run(() =>
            {
                return AnalyticService.LoadList(Session);
            });
        }

        public async Task<Session<List<Filter>>> LoadFilters()
        {
            return await Task.Run(() =>
            {
                return AnalyticService.LoadFilters(Session.Clone<Analytic.Identity>(Model));
            });
        }

        public async Task<Session<List<Analytic.Driver>>> LoadDrivers()
        {
            return await Task.Run(() =>
            {
                return AnalyticService.LoadDrivers(Session.Clone<Analytic.Identity>(Model));
            });
        }
    }
        public class AnalyticListViewModel : ReactiveValidatedEntity
        {
            public AnalyticListViewModel(ReactiveList<Analytic.Identity> list) 
            {
                Identities = list;
                //NextCommand = new ReactiveCommand(null);
                //var nextScreen = NextCommand.RegisterAsyncAction(_ => { })
                //    .ObserveOn(SynchronizationContext.Current)
                //    .Subscribe(_ => { });
            }

            public ReactiveList<Analytic.Identity> Identities { get; set; }

        }
        public class AnalyticEditIdentityViewModel: ReactiveValidatedEntity
        {
            public AnalyticEditIdentityViewModel() {}
            public AnalyticEditIdentityViewModel(Analytic.Identity model, Session<NullT> session, IAnalyticService service) 
            {
                Model = model;
                AnalyticService = service;
                SaveCommand= new ReactiveCommand(null);
                var saving = SaveCommand.RegisterAsyncTask( _ => SaveIdentity(model, session))
                    .Subscribe(x =>
                    {
                        //publish event SaveCompleted
                    });

                

            }

            private Analytic.Identity _Model;
            public Analytic.Identity Model
            {
                get
                {
                    return _Model;
                }
                set
                {
                    if (_Model != value)
                    {
                        _Model = value;
                        this.RaiseAndSetIfChanged(ref _Model, value);
                    }
                }
            }
            
            public async Task<Session<Analytic.Identity>> SaveIdentity(Analytic.Identity model, Session<NullT> session)
            {
                return await Task.Run(() =>
                {
                    var request = Session<NullT>.Clone(session, model);
                    return AnalyticService.SaveIdentity(request);
                });
            }

            Int32 _id;
            public Int32 Id
            {
                get
                { return _id; }
                set { this.RaiseAndSetIfChanged(ref _id, value); }
            }

            string _name;
            public string Name
            {
                get
                { return _name; }
                set { this.RaiseAndSetIfChanged(ref _name, value); }
            }

            string _description;
            public string Description
            {
                get
                { return _description; }
                set { this.RaiseAndSetIfChanged(ref _description, value); }
            }

            public IAnalyticService AnalyticService { get; set; }
        }
        public class AnalyticEditFiltersViewModel : ReactiveValidatedEntity
        {
            public AnalyticEditFiltersViewModel(Analytic analytic, Session<NullT> session, IAnalyticService service) 
            {
             

            }

            public async Task<Session<List<Filter>>> SaveFilters(Analytic model, Session<NullT> session)
            {
                return await Task.Run(() =>
                {
                    var request = Session<NullT>.Clone(session, model);
                    return AnalyticService.SaveFilters(request);
                });
            }
            
            public IAnalyticService AnalyticService { get; set; }

        }
        public class AnalyticEditPriceListsViewModel : ReactiveValidatedEntity
        {
            
        }
        public class AnalyticEditValueDriversViewModel : ReactiveValidatedEntity
        {

        }
        public class AnalyticEditResultsViewModel : ReactiveValidatedEntity
        {

        }

}
