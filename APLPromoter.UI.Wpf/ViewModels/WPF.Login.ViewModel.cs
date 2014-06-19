using APLPromoter.Client.Contracts;
using APLPromoter.Client.Entity;
using APLPromoter.Core.UI;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Threading;
using FluentValidation;
using APLPromoter.Core;
using APLPromoter.Core.Reactive;
using APLPromoter.Core.Reactive.Events;


namespace APLPromoter.UI.Wpf.ViewModel
{
    public class LoginViewModel : ReactiveValidatedEntity , IRoutableViewModel
    {

        public LoginViewModel(IUserService userService, ViewModelLocator locator)
        {

            CurrentSynchContext = SynchronizationContext.Current ?? new SynchronizationContext(); 
            Locator = locator;
            Publisher = locator.EventPublisher;
            UserService = userService;
            var canLogin = this.WhenAny(x => x.LoginName, x => x.Password, x => x.Session,
                    (l, p, m) => !String.IsNullOrWhiteSpace(l.Value) && !String.IsNullOrWhiteSpace(p.Value) && (m.Value != null));

            var canChangePassword = this.WhenAny(x => x.LoginName, x => x.Password, x => x.NewPassword, x => x.ConfirmPassword, (l, p, np, cp) =>
                !String.IsNullOrWhiteSpace(l.Value) && !String.IsNullOrWhiteSpace(p.Value) && !String.IsNullOrWhiteSpace(np.Value) && !String.IsNullOrWhiteSpace(cp.Value));

            LoginCommand = new ReactiveCommand(canLogin);
            ChangePasswordCommand = new ReactiveCommand(canChangePassword);
            ClearCommand = new ReactiveCommand(null);
            //ToggleChangePasswordCommand = new ReactiveCommand(null);

            var w = new Workflow();
            w.Title = "User Login & Authentication";
            w.Steps = new List<Workflow.Step> {
                new Workflow.Step{ Name="Initialization"
                }
            };

            w.Steps[0].IsActive = true;
            w.Steps[0].IsWorking = true;
            w.Steps[0].Advisors = new List<Workflow.Advisor>()
            {
                new Workflow.Advisor{

                    Message="Initializing....",
                    Errors = new List<Workflow.Error>(){
                        new Workflow.Error{
                            Message = "this an advisor error."
                        }
                    }
                }
            };

            Workflow = w; //intial workflow data on initializing...
            HasMessageAlert = true;


            TaskScheduler _currentScheduler = TaskScheduler.Default;

            Task<Session<NullT>> initialized = null;
            try
            {
                initialized = Initialize();

            }
            catch (Exception)
            {

                ToggleStepStatus(StepType.Initialization, false, "Failed to initialize.");
            }
            


            initialized.ContinueWith(t =>
            {

                ToggleInitErrorStatus(StepType.Initialization, false, "Failed to initialize.");
                ToggleActiveStep(StepType.Initialization);
                HasMessageAlert = false;
                

            }, TaskContinuationOptions.OnlyOnFaulted);
                

            //ToggleWorkingStep(StepType.Initialization, false);
                initialized
                .ContinueWith(t =>
            {
                try
                {
                    Session = t.Result;
                    if (Session == null || Session.AppOnline == false) //invalid session prompt to reconnect
                    {
                        ToggleStepStatus(StepType.Initialization, false, "Failed to connect.");
                    }
                    else
                    {
                        Workflow = Session.Workflow;

                        ToggleStepStatus(StepType.Initialization, true, "Initialization complete.");
                        ToggleActiveStep(StepType.Authentication);
                        HasMessageAlert = false;
                    }
                }
                catch (Exception ex)
                {
                    ToggleStepStatus(StepType.Initialization, false, "Failed to connect");
                }

            },TaskContinuationOptions.NotOnFaulted);


            var loggedIn = LoginCommand.RegisterAsyncTask(async _ =>
            {
                ClearErrors();
                ToggleActiveStep(StepType.Authentication);
                Validator = new LoginValidator();
                Validate();
                if (IsValid)
                {
                    ToggleWorkingStep(StepType.Authentication, true);
                    
                    var session = await Authenticate(LoginName, Password);
                    ToggleWorkingStep(StepType.Authentication, false);
                    if (session.Authenticated)
                    {
                        var mainVM = Locator.MainViewModel;
                        mainVM.Session = session;
                        
                        mainVM.ExplorerViewModel = new ExplorerViewModel(session);

                        AuthenticatedMenu =
                                new MenuItemsCollection{
                                    new MenuItem
                                    {
                                        Text = "Promoter",
                                        Items = new MenuItemsCollection()
                                        {
                                            new MenuItem{Text="Logout", ImageUrl="/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Menu-PowerButton.png"},
                                            new MenuItem{Text="Exit", ImageUrl="/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Menu-Logout.png"}
                                        },
                                        FontSize = 16
                                    }
                        };

                        var mi = new MenuItem { Text = session.UserIdentity.Role.Planning.Title, FontSize = 16 };
                        mi.Items = new MenuItemsCollection();
                        foreach(var node in session.UserIdentity.Role.Planning.Navigators)
                        {
                            string nodeImageUri = string.Empty;
                            string nodeHeader = string.Empty;
                            switch (node.NodeTitle)
                            {
                                case "Logout":
                                    nodeImageUri = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Menu-Logout.png";                                    
                                    break;
                                case "Home":
                                    nodeImageUri = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Menu-Home.png";
                                    break;
                                case "Analytics":
                                    nodeImageUri = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Menu-EveryDay.png";
                                    break;
                                case "Pricing":
                                    nodeImageUri = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Dollar-Tag.png";
                                    break;
                                case "Administration":
                                    
                                    nodeImageUri = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Menu-Admin.png";
                                    break;
                            };
                            mi.Items.Add(new MenuItem{
                                Text=node.NodeTitle, 
                                NodeHeader = node.NodeHeader,
                                ViewType=node.WorkflowStep,
                                EntityId=node.EntityId,
                                ImageUrl = nodeImageUri,
                                FontSize = 13
                            });
                        }
                        AuthenticatedMenu.Add(mi);
                        AuthenticatedMenu.Add(new MenuItem
                                {
                                    Text = "About",
                                    Items = new MenuItemsCollection()
                                    {
                                        new MenuItem{Text="APL", ImageUrl = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Menu-EveryDay.png", FontSize = 13},
                                        new MenuItem{Text="Promoter", ImageUrl = "/APLPromoter.UI.Wpf;component/Resources/Images/Logos/AplPNG-ICO.png", FontSize = 13}
                                    },
                                    FontSize = 16
                                });
                        AuthenticatedMenu.Add(new MenuItem
                        {
                            Text = "Themes",
                            Items = new MenuItemsCollection()
                                    {
                                        new MenuItem{Text="ArcticWhite", ImageUrl = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/ArcticWhiteSwatch.jpg", FontSize = 13},
										new MenuItem{Text="BlackIce" , ImageUrl = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/BlackIceSwatch.jpg", FontSize = 13},
                                        new MenuItem{Text="Windows7", ImageUrl = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Windows7Swatch.jpg", FontSize = 13},							
                                        new MenuItem{Text="Office2003", ImageUrl = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Office2003Swatch.jpg", FontSize = 13}								

                                    },
                                    FontSize = 16
                                  });

                        mainVM.MenuViewModel.Items = AuthenticatedMenu;
                        //mainVM.MenuViewModel.RaisePropertyChanged("Items");
                        
                        //mainVM.RaisePropertyChanged("ExplorerViewModel");
                        

                        mainVM.SelectedViewModel = new HomeViewModel();
                        //mainVM.RaisePropertyChanged("SelectedViewModel");
                        Workflow = null;
                        
                        

                        mainVM.RaisePropertyChanged("ExplorerViewModel");
                    }
                    else
                    {

                        if(string.IsNullOrEmpty(session.ClientMessage))
                        {
                            ToggleStepStatus(StepType.Authentication, false, "Login failed.");
                        }
                        else
                        {

                        ToggleStepStatus(StepType.Authentication, false, session.ClientMessage);
                        }

                        Password = string.Empty;
                        LoginName = LoginName;
                        Publisher.Publish(new ErrorExistsEvent());
                    }
                }
            });

            LoginCommand.ThrownExceptions.ObserveOn(CurrentSynchContext).Subscribe(ex =>
            {
                ToggleStepStatus(StepType.Authentication, false, "Failed To Authenticate.");
                //HasMessageAlert = false;
                HasMessageAlert = true;
                Publisher.Publish(HasMessageAlert);
                ToggleWorkingStep(StepType.Authentication, false);
                //TODO: logout and reset steps


            });




            var changingPassword = ChangePasswordCommand.RegisterAsyncTask(async _ =>
            {
                ClearErrors();
                ToggleActiveStep(StepType.PasswordChange);
                Validator = new ChangePasswordValidator();
                Validate();
                if (IsValid)
                {
                    //if (NewPassword != ConfirmPassword) { return; }
                    ToggleWorkingStep(StepType.PasswordChange, true);
                    var authResponse = await Authenticate(LoginName, Password);
                    ToggleWorkingStep(StepType.PasswordChange, false);
                    Session = authResponse;

                    if (authResponse.Authenticated)
                    {
                        ToggleStepStatus(StepType.Authentication, true, "Login Successful.");
                        var changeResponse = await ChangePassword(LoginName, Password,
                                         NewPassword);
                        if (changeResponse.SessionOk)
                        {
                            ToggleStepStatus(StepType.Authentication, true, "Password Successfully Changed.");
                            HostScreen.Router.Navigate.Execute(Application);  
                            HasMessageAlert = true;
                        }
                    }
                }

            }).ObserveOn(CurrentSynchContext);

            ChangePasswordCommand.ThrownExceptions.Subscribe(ex =>
            {
                ToggleStepStatus(StepType.PasswordChange, false, "Failed To Change Password.");
                HasMessageAlert = true;
                //TODO: logout and reset steps


            });

            var clearing = ClearCommand.RegisterAsyncAction(_ => { })
                .ObserveOn(CurrentSynchContext)
                .Subscribe(x =>
                {
                    LoginName = string.Empty;
                    Password = string.Empty;
                    NewPassword = string.Empty;
                    ConfirmPassword = string.Empty;
                    //TestErrors.ToList().Clear();
                });


            ToggleChangePasswordCommand = new DelegateCommand<Visibility>(ToggleStepAndClear);

        }
        public SynchronizationContext CurrentSynchContext { get; set; }

        MenuItemsCollection _authenticatedMenu;
        public MenuItemsCollection AuthenticatedMenu
        {
            get
            { return _authenticatedMenu; }
            set { this.RaiseAndSetIfChanged(ref _authenticatedMenu, value); }
        }


        public void ToggleStepAndClear(Visibility x)
        {

            if (x == System.Windows.Visibility.Visible)
            {
                ToggleActiveStep(StepType.PasswordChange);
                ValidationErrors = Enumerable.Empty<FluentValidation.Results.ValidationFailure>();
            }
            else
            {
                ToggleActiveStep(StepType.Authentication);
                ValidationErrors = Enumerable.Empty<FluentValidation.Results.ValidationFailure>();
            }

        }



        Boolean _HasMessageAlert;
        public Boolean HasMessageAlert
        {
            get
            {
                return _HasMessageAlert;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _HasMessageAlert, value);
            }
        }

        public void Validate()
        {
            base.Validate();
            ValidationErrors = base.ValidationErrors;
            if (ValidationErrors.Count() > 0) Publisher.Publish(new ErrorExistsEvent());
        }


        public void ClearErrors()
        {
            if( Workflow !=null && Workflow.Steps != null)
            foreach (var i in this.Workflow.Steps)
            {
                if (i != null && i.Errors != null) { 
                    i.Errors.Clear();
                    i.RaisePropertyChanged("Errors");
                    
                }
            }

        }

        public void ToggleStepStatus(StepType stepType, bool isValid, string message = null)
        {
            var stepNo = (int) stepType;
            var step = Workflow.Steps[stepNo];

            if (step != null)
            {
                //step.IsWorking = false;
                //step.Advisors[0].Message = message;
                step.IsValid = isValid;
                ClearErrors();
                step.Errors = new List<Workflow.Error> { new Workflow.Error { Message = message } };
            }
        }

        public void ToggleInitErrorStatus(StepType stepType, bool isValid, string message = null)
        {
            var stepNo = (int)stepType;
            var step = Workflow.Steps[stepNo];

            if (step != null)
            {
                step.IsWorking = false;
                step.Advisors[0].Message = string.Empty;
                step.IsValid = isValid;
                ClearErrors();
                step.Errors = new List<Workflow.Error> { new Workflow.Error { Message = message } };
            }
        }

        public void ToggleWorkingStep(StepType activeStep, Boolean IsOn)
        {
            var stepNo = (int)activeStep;
            for (Int32 i = 0; i < Workflow.Steps.Count; i++)
            {
                var step = Workflow.Steps[i];
                if (step != null)
                {
                    bool isWorking = (i == stepNo) ? step.IsWorking = IsOn : step.IsWorking = false;
                }
            }
        }



        public void ToggleActiveStep(StepType activeStep)
        {
            var stepNo = (int) activeStep;
                for (Int32 i = 0; i < Workflow.Steps.Count; i++)
                {
                    var step = Workflow.Steps[i];
                    if (step != null)
                    {
                         bool isActive = (i == stepNo) ? step.IsActive = true : step.IsActive = false;
                    }
                }
        }

        public async Task<Session<NullT>> Initialize()
        {
            return await Task.Run(() =>
            {
                Session<NullT> key = new Session<NullT>()
                {
                    SqlKey = ConfigurationManager.AppSettings["sharedKey"].ToString()
                };
                //throw new Exception();
                //Thread.Sleep(3000); //simulate initialize delay
                return UserService.Initialize(key); 
            });
        }

        public async Task<Session<NullT>> Authenticate(string loginName, string password)
        {
            return await Task.Run(() =>
            {
                Session.UserIdentity = new User.Identity
                {
                    Login = loginName,
                    Password = new User.Password { Old = Password }
                };
                //Thread.Sleep(3000); //simulate initialize delay
                //throw new Exception();
                return UserService.Authenticate(Session); //TODO:Using and Dispose of proxy.

            });
        }

        public async Task<Session<NullT>> ChangePassword(string loginName, string password, string newPassword)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Session.UserIdentity = new User.Identity
                    {
                        Login = loginName,
                        Password = new User.Password { Old = Password, New = newPassword }
                    };
                    Thread.Sleep(3000); //simulate initialize delay
                    return UserService.SavePassword(Session);
                }
                catch (Exception)
                {
                    throw;
                }

            });
        }

        public ReactiveCommand LoginCommand { get; set; }
        public ReactiveCommand ChangePasswordCommand { get; set; }
        public ReactiveCommand ClearCommand { get; set; }
        public DelegateCommand<Visibility> ToggleChangePasswordCommand { get; set; }

        public IEventAggregator Publisher { get; set; }
        public ViewModelLocator Locator { get; set; }
        protected override IValidator GetValidator()
        {

            return new LoginValidator();
            
        }

        IEnumerable<FluentValidation.Results.ValidationFailure> _validationErrors = 
            new List<FluentValidation.Results.ValidationFailure>();
        public IEnumerable<FluentValidation.Results.ValidationFailure> ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _validationErrors, value);
            }
        }

        public class err
        {
            public string ErrorMessage { get; set; }
        }



        string _version;
        public string Version
        {
            get
            {
                return "Version 4.0";
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _version, value);
            }
        }

        Session<NullT> _session; //TODO: Need to store in application cache
        public Session<NullT> Session
        {
            get
            { return _session; }
            set { this.RaiseAndSetIfChanged(ref _session, value); }
        }
        
        IUserService _userService;
        public IUserService UserService
        {
            get
            { return _userService; }
            set { this.RaiseAndSetIfChanged(ref _userService, value); }
        }

        MainViewModel _application;
        public MainViewModel Application
        {
            get
            { return _application; }
            set { this.RaiseAndSetIfChanged(ref _application, value); }
        }

        string _loginName;
        public string LoginName
        {
            get
            { return _loginName; }
            set { this.RaiseAndSetIfChanged(ref _loginName, value); }
        }

        string _password;
        public string Password
        {
            get { return _password;}
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { this.RaiseAndSetIfChanged(ref _newPassword, value); }
        }

        string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { this.RaiseAndSetIfChanged(ref _confirmPassword, value); }
        }

        public IScreen HostScreen { get; private set; }



        public string UrlPathSegment
        {
            get { return "login"; }
        }
        

        }

    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(o => o.LoginName)
                .NotEmpty()
                .NotNull()
                .Length(10,50);

            RuleFor(o => o.Password)
                .NotEmpty()
                .NotNull()
                .Length(5, 50);


        }
        }


    public class ChangePasswordValidator : AbstractValidator<LoginViewModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(o => o.LoginName)
                .NotEmpty()
                .NotNull()
                .Length(2, 50);

            RuleFor(o => o.Password)
                .NotEmpty()
                .NotNull()
                .Length(5, 50);

            RuleFor(o => o.NewPassword)
            .NotEmpty()
            .NotNull()
            .Length(5, 50);

            RuleFor(o => o.NewPassword).Equal(o => o.ConfirmPassword).WithMessage("New password and Confirm Password must be the same.");

    }
}
}
