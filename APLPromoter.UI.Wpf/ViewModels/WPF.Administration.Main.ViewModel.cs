using APLPromoter.Client.Contracts;
using APLPromoter.Client.Entity;
using ReactiveUI;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APLPromoter.Core.Reactive;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class AdminViewModel : ReactiveValidatedEntity
    {
        public AdminViewModel(
                                Navigator navigator,
                                IUserService userService,
                                ViewModelLocator locator,
                                Session<NullT> session,
                                IEventAggregator publisher
                                )
        {
            Session = session;
            UserService = userService;
            Publisher = publisher;
            Workflow = session.UserIdentity.Role.Planning.Workflows.SingleOrDefault(x => x.ThisWorkflowType == WorkflowType.PlanningAdministration);

            if (Workflow != null)
            {
                Workflow.Step first = Workflow.Steps.FirstOrDefault();
                if (first != null) { first.IsActive = true; }
            }

            //sync selected step to property when any child in list changes
            StepList = this.Workflow.Steps
                .CreateDerivedCollection(x => x);
            //set default 
            SelectedStep = StepList.FirstOrDefault(x => x.IsActive == true);

            if (navigator != null) { ChangeStep(navigator.WorkflowStep); }

            var selectedStepChanged = StepList.Changed.Select(_ => StepList.SingleOrDefault(x => x.IsActive == true))
                            .ToProperty(this, x => x.SelectedStep, null)
                            .Subscribe(x =>
                            {

                                if (x.Index == 0) //add entity mode -> default all to false and init +/-
                                {
                                    Workflow.Steps.ForEach(y =>
                                    {
                                        if (y.Index == SelectedStep.Index || y.Index == SelectedStep.Index - 1 || y.Index == SelectedStep.Index + 1)
                                            y.IsEnabled = true;
                                        else
                                            y.IsEnabled = false;

                                    });
                                }
                                else //mark all enable if existing 
                                {
                                    Workflow.Steps.ForEach(z => z.IsEnabled = true);
                                }

                            });



            //onTabClicked
            ChangeStepCommand = new ReactiveCommand(null);
            ChangeStepCommand.Subscribe(step =>
            {
                var stepType = ((Workflow.Step)step).ThisStepType;
                ChangeStep(stepType);
                Publisher.Publish<WorkflowStepType>(stepType);
                this.raisePropertyChanged("SelectedStepViewModel");
            });



            var canNavigatePrevious = this.WhenAny(x => x.SelectedIdentity.IsDirty, x => x.SelectedStep.Index, x => x.SelectedIdentity.Id,
                (di, ix, id) => di.Value == false && ix.Value >= 1 && id.Value == 0);

            canNavigatePrevious.Subscribe(x =>
            {
                CanNavigateFrom = x;
                Workflow.Steps.ForEach(y =>
            {
                if (y.Index == SelectedStep.Index || y.Index == SelectedStep.Index - 1 || y.Index == SelectedStep.Index + 1)
                    y.IsEnabled = true;
                else
                    y.IsEnabled = false;

            });
            });

            var canNavigateNext = this.WhenAny(x => x.SelectedIdentity.IsDirty, x => x.SelectedStep.Index, x => x.SelectedIdentity.Id,
                (di, ix, id) => di.Value == false && ix.Value != Workflow.Steps.Count && id.Value == 0);

            canNavigatePrevious.Subscribe(x =>
            {
                CanNavigateFrom = x;
                Workflow.Steps.ForEach(y =>
                {
                    if (y.Index == SelectedStep.Index || y.Index == SelectedStep.Index - 1 || y.Index == SelectedStep.Index + 1)
                        y.IsEnabled = true;
                    else
                        y.IsEnabled = false;

                });
            });


            var isEditingExisting = this.WhenAny(x => x.SelectedStep.IsDirty, x => x.SelectedIdentity.Id,
                    (di, id) => di.Value == true && id.Value != 0)
                    .ToProperty(this, s => s.CanNavigateFrom)
                    .Subscribe(x =>
                    {
                        Workflow.Steps.ForEach(y =>
                        {
                            y.IsEnabled = true;
                        });

                    });


            var isEditingNew = this.WhenAny(x => x.SelectedStep.IsDirty, x => x.SelectedIdentity.Id,
                    (di, id) => di.Value == true && id.Value == 0)
                    .Subscribe(x =>
                    {
                        if (x == true)
                        {
                            Workflow.Steps.ForEach(y =>
                                {

                                    if (y.Index == SelectedStep.Index)
                                        y.IsEnabled = true;
                                    else
                                        y.IsEnabled = false;
                                }

                                );
                        }

                    });


            PreviousCommand = new ReactiveCommand(canNavigatePrevious);
            PreviousCommand.Subscribe(_ =>
            {
                var prevStepIndex = Workflow.Steps.FindIndex(st => st.IsActive == true) - 1;
                if (prevStepIndex >= 0)
                {
                    var prevStep = Workflow.Steps[prevStepIndex];
                    ChangeStep(prevStep.ThisStepType);
                    Publisher.Publish<WorkflowStepType>(prevStep.ThisStepType);
                    this.raisePropertyChanged("SelectedStepViewModel");
                }
            });



            NextCommand = new ReactiveCommand(canNavigateNext);

            NextCommand.Subscribe(_ =>
                {
                    var nextStepIndex = Workflow.Steps.FindIndex(st => st.IsActive == true) + 1;
                    if (nextStepIndex < Workflow.Steps.Count)
                    {
                        var nextStep = Workflow.Steps[nextStepIndex];
                        ChangeStep(nextStep.ThisStepType);
                        Publisher.Publish<WorkflowStepType>(nextStep.ThisStepType);
                        this.raisePropertyChanged("SelectedStepViewModel");
                    }
                });

            SaveCommand = new ReactiveCommand(null);
            var editingUsers = SaveCommand.RegisterAsyncTask<Session<User.Identity>>(async identity =>
                {
                    var id = (User.Identity)identity;

                    return await Task.Run(() =>
                        {
                            return UserService.SaveIdentity(new Session<User.Identity>
                            {
                                SqlKey = session.SqlKey,
                                Data = id
                            });

                        });


                })
                .Subscribe(x =>
                {

                    SelectedIdentity = x.Data;
                    //if (Users != null || Users.Count > 0) 
                    //{ 
                    //    LoadUserCommand.Execute(
                    //        new User.Identity
                    //        {
                    //            Id = Users[0].Id
                    //        }
                    //    );
                    //}
                });






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
                    case WorkflowStepType.PlanningAdministrationUserMaintenance:

                        step.StepViewModel = new AdminUserListViewModel(UserService, Session);
                        break;
                    case WorkflowStepType.PlanningAdministrationPricelists:
                        step.StepViewModel = new AdminEditPriceListsViewModel();
                        break;
                    case WorkflowStepType.PlanningAdministrationOptimization:
                        step.StepViewModel = new AdminEditOptimizationsViewModel();
                        break;
                    case WorkflowStepType.PlanningAdministrationRoundingrules:
                        step.StepViewModel = new AdminEditRoundingViewModel();
                        break;
                    case WorkflowStepType.PlanningAdministrationRollback:
                        step.StepViewModel = new AdminEditRoundingViewModel();
                        break;
                    case WorkflowStepType.PlanningAdministrationProcesses:
                        step.StepViewModel = new AdminEditProcessesViewModel();
                        break;
                }
                StepHeader = step.Name;
                ActiveStep = step.Index - 1;
                this.raisePropertyChanged("ActiveStep");
            }
            SelectedStepViewModel = Workflow.Steps.FirstOrDefault(st => st.ThisStepType == stepType).StepViewModel;


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


        private User.Identity _selectedIdentity;
        public User.Identity SelectedIdentity
        {
            get
            { return _selectedIdentity; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedIdentity, value);
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
        public IEventAggregator Publisher { get; set; }
        public ReactiveCommand ChangeStepCommand { get; set; }


        Session<NullT> _session;
        public Session<NullT> Session
        {
            get
            { return _session; }
            set { this.RaiseAndSetIfChanged(ref _session, value); }
        }


        private Workflow.Step _SelectedStep;
        public Workflow.Step SelectedStep
        {
            get
            { return _SelectedStep; }
            set
            {
                this.RaiseAndSetIfChanged(ref _SelectedStep, value);
            }
        }

        private IReactiveDerivedList<Workflow.Step> _stepList;
        public IReactiveDerivedList<Workflow.Step> StepList
        {
            get
            {
                return _stepList;
            }
            set
            {
                if (_stepList != value)
                {
                    _stepList = value;
                    this.RaiseAndSetIfChanged(ref _stepList, value);
                }
            }
        }


        public IUserService UserService { get; set; }


        private Int32 _id;
        public Int32 Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.RaiseAndSetIfChanged(ref _id, value);
                }
            }
        }

        private Boolean _CanNavigateFrom;
        public Boolean CanNavigateFrom
        {
            get
            {
                return _CanNavigateFrom;
            }
            set
            {
                if (_CanNavigateFrom != value)
                {
                    _CanNavigateFrom = value;
                    this.RaiseAndSetIfChanged(ref _CanNavigateFrom, value);
                }
            }
        }


        private List<APLPromoter.Client.Entity.User.Role> _RoleChoices;
        public List<APLPromoter.Client.Entity.User.Role> RoleChoices
        {
            get
            {
                return new List<APLPromoter.Client.Entity.User.Role>{new APLPromoter.Client.Entity.User.Role{Description="Administrator", Name = "Admin", Id = 0},     
                                                                            new APLPromoter.Client.Entity.User.Role{Description="Pricing Analyst", Id=1}, 
                                                                            new APLPromoter.Client.Entity.User.Role{Description="Reviewer", Id=2} };
            }
            set { this.RaiseAndSetIfChanged(ref _RoleChoices, value); }
        }

        public enum UserStatus
        {
            Active = 0,
            Inactive = 1
        }

        //private UserStatus _SelectedUserStatus;
        //public UserStatus SelectedUserStatus
        //{
        //    get { return _SelectedUserStatus; }
        //    set { this.RaiseAndSetIfChanged(ref _SelectedUserStatus, value); }
        //}

        public IEnumerable<UserStatus> UserStatusChoices
        {
            get { return Enum.GetValues(typeof(UserStatus)).Cast<UserStatus>(); }
        }



        public AdminViewModel(Int32 id)
        {
            Id = id;
        }


        public class AdminUserListViewModel : ReactiveValidatedEntity
        {
            public AdminUserListViewModel(IUserService UserService, Session<NullT> session)
            {

                LoadUserCommand = new ReactiveCommand(null);
                var loadingUser = LoadUserCommand.RegisterAsyncTask<Session<User.Identity>>(async identity =>
                {
                    var id = (User.Identity)identity;

                    return await Task.Run(() =>
                    {
                        return new Session<User.Identity>()
                        {
                            Data = id
                        };
                    });


                }).Subscribe(x =>
                        { SelectedIdentity = x.Data; }
                );



                var loadListCommand = new ReactiveCommand(null);
                var usersLoading = loadListCommand.RegisterAsyncTask(async x =>
                {
                    return await Task.Factory.StartNew(() =>
                   {

                       return UserService.LoadList(session);
                   });

                })//.ObserveOn(context)
                .Subscribe(x =>
                {
                    Users = x.Data;
                    //if (Users != null || Users.Count > 0) 
                    //{ 
                    //    LoadUserCommand.Execute(
                    //        new User.Identity
                    //        {
                    //            Id = Users[0].Id
                    //        }
                    //    );
                    //}
                });

                //load on init
                loadListCommand.Execute(null);
            }

            private User.Identity _selectedIdentity;
            public User.Identity SelectedIdentity
            {
                get
                { return _selectedIdentity; }
                set
                {
                    this.RaiseAndSetIfChanged(ref _selectedIdentity, value);
                }
            }

            private List<User.Identity> _Users;
            public List<User.Identity> Users
            {
                get
                {
                    return _Users;
                }

                set
                {
                    this.RaiseAndSetIfChanged(ref _Users, value);
                }
            }
            public ReactiveCommand LoadUserCommand { get; set; }
            public ReactiveCommand EditUserCommand { get; set; }
            public ReactiveList<User.Identity> Identities { get; set; }

        }

        public class AdminEditPriceListsViewModel : ReactiveValidatedEntity
        {

        }

        public class AdminEditOptimizationsViewModel : ReactiveValidatedEntity
        {

        }

        public class AdminEditRoundingViewModel : ReactiveValidatedEntity
        {

        }

        public class AdminEditRollbackViewModel : ReactiveValidatedEntity
        {

        }

        public class AdminEditProcessesViewModel : ReactiveValidatedEntity
        {

        }

        public class AdminEditAlertsViewModel : ReactiveValidatedEntity
        {

        }
    }
}
