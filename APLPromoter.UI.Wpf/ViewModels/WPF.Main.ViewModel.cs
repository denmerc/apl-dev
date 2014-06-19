using System.Windows.Media.Imaging;
using APLPromoter.Client.Contracts;
using APLPromoter.Client.Entity;
using APLPromoter.Core.Reactive;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using System.Windows;


namespace APLPromoter.UI.Wpf.ViewModel
{
    //[Export]
    public class MainViewModel : ReactiveValidatedEntity, IRoutableViewModel
    {
        private DelegateCommand _notifyOnClickCommand;
        
        [ImportingConstructor]
        public MainViewModel(IAnalyticService analyticService, IUserService userService, IEventAggregator eventPublisher, 
                                ViewModelLocator locator = null, System.Windows.Application app = null)
        {
            Locator = locator ?? App.Current.Resources["Locator"] as ViewModelLocator;
            UserService = userService;
            CurrentAppContext = App.Current ?? app;
            CurrentSynchContext = SynchronizationContext.Current ?? new SynchronizationContext(); 
            Publisher = eventPublisher;


            _notifyOnClickCommand = new DelegateCommand(NotifyOnClickExecuted);


            Publisher.GetEvent<WorkflowStepType>()
                .Subscribe(stepType =>
                {
                    if (this.SelectedViewModel != null)
                    {
                        this.SelectedViewModel.Workflow.Steps.ForEach(a => a.IsActive = false);
                        var activeStep = this.SelectedViewModel.Workflow.Steps.FirstOrDefault(s => s.ThisStepType == stepType);
                        if (activeStep != null) { activeStep.IsActive = true; }
                    }

                });

            Publisher.GetEvent<Navigator>()
                .Subscribe( x => 
                    {
                        ReactiveValidatedEntity vm = null; 
                        switch (x.WorkflowStep)
                        {
                            case WorkflowStepType.PlanningHomeMyHomePage:
                                this.SelectedViewModel = new HomeViewModel();
                                break;    
                            case WorkflowStepType.PlanningAnalyticsMyAnalytics:
                            case WorkflowStepType.PlanningAnalyticsIdentity:
                            case WorkflowStepType.PlanningAnalyticsFilters:
                            case WorkflowStepType.PlanningAnalyticsPriceLists:
                            case WorkflowStepType.PlanningAnalyticsResults:
                                
                                if (CurrentModules.ContainsKey(WorkflowType.PlanningAnalytics))
                                {
                                    CurrentModules[WorkflowType.PlanningAnalytics].Dispose();
                                    CurrentModules[WorkflowType.PlanningAnalytics] = null;
                                    CurrentModules.Remove(WorkflowType.PlanningAnalytics);
                                }
                                vm = new AnalyticViewModel(Session, Locator.AnalyticService, Publisher, x.EntityId);
                                CurrentModules.Add(WorkflowType.PlanningAnalytics, vm);
                                break;


                            case WorkflowStepType.PlanningPricingMyPricing:
                            case WorkflowStepType.PlanningPricingIdentity:
                            case WorkflowStepType.PlanningPricingApproval:
                            case WorkflowStepType.PlanningPricingFilters:
                            case WorkflowStepType.PlanningPricingForecast:
                            case WorkflowStepType.PlanningPricingRounding:
                            case WorkflowStepType.PlanningPricingStrategy:
                            case WorkflowStepType.PlanningPricingResults:
                                vm = new PriceRoutineViewModel(x.EntityId, Session);
                                break;

                            //case WorkflowStepType.PlanningAdministrationFilters:
                            case WorkflowStepType.PlanningAdministrationOptimization:
                            case WorkflowStepType.PlanningAdministrationPricelists:
                            case WorkflowStepType.PlanningAdministrationProcesses:
                            case WorkflowStepType.PlanningAdministrationRollback:
                            //case WorkflowStepType.PlanningAdministrationRounding:
                            case WorkflowStepType.PlanningAdministrationUserMaintenance:
                                if (CurrentModules.ContainsKey(WorkflowType.PlanningAdministration))
                                {
                                    CurrentModules[WorkflowType.PlanningAdministration].Dispose();
                                    CurrentModules[WorkflowType.PlanningAdministration] = null;
                                    CurrentModules.Remove(WorkflowType.PlanningAdministration);
                                }
                                vm = new AdminViewModel(navigator: x,userService: UserService, locator: Locator, session: Session, publisher: Publisher);
                                CurrentModules.Add(WorkflowType.PlanningAdministration, vm);
                                break;
                        }
                        if (vm != null)
                        {
                            vm.Workflow.Steps.ForEach(a => a.IsActive = false);
                            var activeStep = vm.Workflow.Steps.FirstOrDefault(y => y.ThisStepType == x.WorkflowStep);
                            if (activeStep != null) { activeStep.IsActive = true; }

                            this.SelectedViewModel = vm;
                            this.SelectedViewModel.HeaderTitle = x.NodeHeader;
                            //TODO: add to instance hash mapper navigator to viewmodel
                            Publisher.Publish<WorkflowStepType>(x.WorkflowStep);
                        }

                        
                    });

            SelectedViewModel = new LoginViewModel(userService: userService, locator: Locator);
            ExplorerViewModel = null;

            ChangeThemeCommand = new ReactiveCommand(null);


            var changingTheme = ChangeThemeCommand.RegisterAsyncFunction(x => {return x; })
                .ObserveOn(CurrentSynchContext)
                .Subscribe( x =>
                    {
                        switch (x.ToString())
                        {
                                                        case "ArcticWhite": 
                                UserSettings.HeaderBrush = Brushes.ArticWhiteHeaderBrush;
                                UserSettings.BackgroundBrush = Brushes.ArticWhiteBackgroundBrush;
								UserSettings.BorderBrush = Brushes.ArticWhiteBorderBrush;
								UserSettings.TextBrush = Brushes.ArticWhiteTextBrush;
                                break;
                            case "BlackIce":
                                UserSettings.HeaderBrush = Brushes.BlackIceHeaderBrush;
                                UserSettings.BackgroundBrush = Brushes.BlackIceBackgroundBrush;
								UserSettings.BorderBrush = Brushes.BlackIceBorderBrush;
								UserSettings.TextBrush = Brushes.BlackIceTextBrush;
                                break;
                            case "Office2003":
                                UserSettings.HeaderBrush = Brushes.Office2003HeaderBrush;
                                UserSettings.BackgroundBrush = Brushes.Office2003BackgroundBrush;
								UserSettings.BorderBrush = Brushes.Office2003BorderBrush;
								UserSettings.TextBrush = Brushes.Office2003TextBrush;
                                break;
                            case "Windows7" :
                                UserSettings.HeaderBrush = Brushes.Windows7HeaderBrush;
                                UserSettings.BackgroundBrush = Brushes.Windows7BackgroundBrush;
								UserSettings.BorderBrush = Brushes.Windows7BorderBrush;
								UserSettings.TextBrush = Brushes.Windows7TextBrush;
                                break;
                        }
                    });


            CloseCommand = new ReactiveCommand(null);
            var closing = CloseCommand.RegisterAsyncAction(_ => { })
                .ObserveOn(CurrentSynchContext)
                .Subscribe(_ =>  CurrentAppContext.Shutdown());


            AboutCommand = new ReactiveCommand(null);


            AboutCommand.Select(x => Observable.Start(() => { },
                RxApp.TaskpoolScheduler))
                .Switch()
                .ObserveOn(CurrentSynchContext)
                .Subscribe(_ =>
                {
                    this.SelectedViewModel = new AboutUsViewModel();
                });

            LogoutCommand = new ReactiveCommand(null);
            LogoutCommand.Select(x => Observable.Start(() => {
                    }, RxApp.TaskpoolScheduler))
                    .Switch()
                .ObserveOn(CurrentSynchContext)
                .Subscribe(_ =>
                {
                    
                    this.ExplorerViewModel = null;
                    this.Session = null;
                    this.SelectedViewModel = null;
                    this.SelectedViewModel = new LoginViewModel(userService: userService, locator: Locator);
                    
                });

            DefaultMenu =
                new MenuItemsCollection{
                    new MenuItem
                    {
                        Text = "Promoter",
                        Items = new MenuItemsCollection()
                        {
                            new MenuItem{Text="Login" , ImageUrl = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Menu-Login.png", FontSize = 13},
                            new MenuItem{Text="Exit", ImageUrl = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Menu-Logout.png", FontSize = 13 }
                        }                 ,
                        FontSize = 16
                    },
					
					
                    new MenuItem
                    {
                        Text = "About",
                        Items = new MenuItemsCollection()
                        {
                            new MenuItem{Text="APL", ImageUrl = "/APLPromoter.UI.Wpf;component/Resources/Images/ContextMenu/Menu-EveryDay.png", FontSize = 13},
                            new MenuItem{Text="Promoter", ImageUrl="/APLPromoter.UI.Wpf;component/Resources/Images/Logos/AplPNG-ICO.png", FontSize = 13}
                        },
                        FontSize = 16

                    },
					new MenuItem
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
                    }
                };



            MenuViewModel.Items = DefaultMenu;
        }

        private void NotifyOnClickExecuted(object obj)
        {
            var menuItem = (MenuItem)obj;

            if (menuItem != null)
            {
                if(menuItem.ViewType == 0) // not logged in
                {
                    switch (menuItem.Text)
                    {
                        case "APL":
                            this.SelectedViewModel = new AboutUsViewModel();
                            break;
                        case "Exit":
                            CloseCommand.Execute(null);
                            break;
                        case "Login" :
                            LogoutCommand.Execute(null);
                            break;
                        case "Logout":
                            LogoutCommand.Execute(null);
                            this.MenuViewModel.Items = DefaultMenu;
                            
                            break;
                        case "ArcticWhite":
                        case "BlackIce":
                        case "Office2003":
                        case "Windows7":
                            ChangeThemeCommand.Execute(menuItem.Text);
                            break;

                    }

                }
                else //logged in
                {
                        Publisher.Publish<Navigator>(new Navigator{WorkflowStep = menuItem.ViewType, NodeHeader = menuItem.NodeHeader});
                }
            }

        }

        MenuItemsCollection _defaultMenu;
        public MenuItemsCollection DefaultMenu
        {
            get
            { return _defaultMenu; }
            set { this.RaiseAndSetIfChanged(ref _defaultMenu, value); }
        }

        public System.Windows.Application CurrentAppContext { get; set; }
        public SynchronizationContext CurrentSynchContext { get; set; }
        
        public IEventAggregator Publisher { get; set; }
        public IUserService UserService { get; set; }
        Session<NullT> _session; 
        public Session<NullT> Session
        {
            get
            { return _session; }
            set { this.RaiseAndSetIfChanged(ref _session, value); }
        }


        public  ViewModelLocator Locator { get; set; }


        private ExplorerViewModel _explorerViewModel = null;
        public ExplorerViewModel ExplorerViewModel 
        {
            get { return _explorerViewModel; }
            set
            {
                this.RaiseAndSetIfChanged(ref _explorerViewModel, value);
            }
        }

        private MenuViewModel _menuViewModel = new MenuViewModel();
        public MenuViewModel MenuViewModel
        {
            get { return _menuViewModel; }
            set
            {
                this.RaiseAndSetIfChanged(ref _menuViewModel, value);
            }
        }

        private Dictionary<WorkflowType, ReactiveValidatedEntity> _CurrentModules = new Dictionary<WorkflowType,ReactiveValidatedEntity>();
        public Dictionary<WorkflowType, ReactiveValidatedEntity> CurrentModules
        {
            get
            { return _CurrentModules; }

            set { this.RaiseAndSetIfChanged(ref _CurrentModules, value);  }
        }

        public IScreen HostScreen { get; set; }
        public IRoutingState Router { get; set; }
        public string UrlPathSegment {get { return "main";}}

        //private List<ReactiveObject> _navigatedViewModels;
        //public List<ReactiveObject> NavigatedViewModels
        //{
        //    get
        //    {
        //        if (_navigatedViewModels == null)
        //            _navigatedViewModels = new List<ReactiveObject>();
        //        return _navigatedViewModels;
        //    }
        //}

        public string UserGreeting { get { return "Welcome:" + Session.UserIdentity.FirstName + " " + Session.UserIdentity.LastName; } }
        public string MessageCenterTitle { get {
            string t = SelectedViewModel == null || (SelectedViewModel.Workflow == null) ? string.Empty : SelectedViewModel.Workflow.Title;
            return t;
             } 
        }
        private ReactiveValidatedEntity _selectedViewModel;
        public ReactiveValidatedEntity SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedViewModel, value);
                this.RaisePropertyChanged("MessageCenterTitle");
            }
        }
        public ReactiveCommand LogoutCommand { get; set; }
        public ReactiveCommand AboutCommand { get; set; }
        public ReactiveCommand CloseCommand { get; set; }
        public ReactiveCommand Navigate { get; set; }
        public ReactiveCommand Refresh { get; set; }
        //private Dictionary<WorkflowType, Dictionary<WorkflowStepType, ReactiveValidatedEntity>> _StepViewModels = new Dictionary<WorkflowType, Dictionary<WorkflowStepType, ReactiveValidatedEntity>>();
        //public Dictionary<WorkflowType, Dictionary<WorkflowStepType, ReactiveValidatedEntity>> StepViewModels
        //{
        //    get { return _StepViewModels; }
        //    set
        //    {
        //        this.RaiseAndSetIfChanged(ref _StepViewModels, value);
        //    }
        //}
        public ReactiveCommand ChangeThemeCommand { get; set; }



        private Settings _UserSettings = new Settings()
                {
                    HeaderBrush = Brushes.ArticWhiteHeaderBrush,
                    BackgroundBrush = Brushes.ArticWhiteBackgroundBrush,
                    TextBrush = Brushes.ArticWhiteTextBrush,
                    LogoBrush = Brushes.LogoDarkImageBrush

                };
        public Settings UserSettings 
        {
            get 
            {
                return _UserSettings;
            }
            set 
            {
                this.RaiseAndSetIfChanged(ref _UserSettings, value);
            }
        
        }

        public ICommand NotifyOnClickCommand
        {
            get
            {
                return this._notifyOnClickCommand;
            }
        }
        


        [Serializable]
        public class Settings : ReactiveObject
        {
            Brush _HeaderBrush = Brushes.BlackIceHeaderBrush;
            public Brush HeaderBrush
            {
                get
                {
                    return _HeaderBrush;
                }
                set
                {

                    this.RaiseAndSetIfChanged(ref _HeaderBrush, value);
                }
            }

            Brush _TextBrush = Brushes.BlackIceHeaderBrush;
            public Brush TextBrush
            {
                get
                {
                    return _TextBrush;
                }
                set
                {

                    this.RaiseAndSetIfChanged(ref _TextBrush, value);
                }
            }

            Brush _BorderBrush = Brushes.BlackIceBorderBrush;
            public Brush BorderBrush
            {
                get
                {
                    return _BorderBrush;
                }
                set
                {

                    this.RaiseAndSetIfChanged(ref _BorderBrush, value);
                }
            }

            Brush _BackgroundBrush = Brushes.BlackIceHeaderBrush;
            public Brush BackgroundBrush
            {
                get
                {
                    return _BackgroundBrush;
                }
                set
                {

                    this.RaiseAndSetIfChanged(ref _BackgroundBrush, value);
                }
            }

            Brush _LogoBrush = Brushes.LogoDarkImageBrush;
            public Brush LogoBrush
            {
                get
                {
                    return _LogoBrush;
                }
                set
                {

                    this.RaiseAndSetIfChanged(ref _LogoBrush, value);
                }
            }

        }

    }


    public static class Brushes
    {
        
        //LogoImage Brush

        //black ice header
        static readonly Color BlackIce1 = (Color)ColorConverter.ConvertFromString("#FF7E7E7E");
        static readonly Color BlackIce2 = (Color)ColorConverter.ConvertFromString("Black");
        static readonly Color BlackIce3 = (Color)ColorConverter.ConvertFromString("#FF444343");

        //black ice background
        static readonly Color BlackIceBg1 = (Color)ColorConverter.ConvertFromString("#FF080808");
        static readonly Color BlackIceBg2 = (Color)ColorConverter.ConvertFromString("#FF232222");
        static readonly Color BlackIceBg3 = (Color)ColorConverter.ConvertFromString("#FF6C6C6C");

		//black ice text
		static readonly Color BlackIceText1 = (Color)ColorConverter.ConvertFromString("White");

		//black ice border
		static readonly Color BlackIceBorder1 = (Color)ColorConverter.ConvertFromString("#FF080808");
		
        //artic white header
        static readonly Color ArticWhite1 = (Color)ColorConverter.ConvertFromString("White");
        static readonly Color ArticWhite2 = (Color)ColorConverter.ConvertFromString("#FFE2E2E2");
        static readonly Color ArticWhite3 = (Color)ColorConverter.ConvertFromString("#FFE2E2E2");

        //artic white background
        static readonly Color ArticWhiteBg1 = (Color)ColorConverter.ConvertFromString("#FFC7C7C7");
        static readonly Color ArticWhiteBg2 = (Color)ColorConverter.ConvertFromString("#FFF6F6F6");
        static readonly Color ArticWhiteBg3 = (Color)ColorConverter.ConvertFromString("#FFC7C7C7");

		//acrtic white text
		static readonly Color ArcticWhiteText1 = (Color)ColorConverter.ConvertFromString("Black");

		//arctic white border
		static readonly Color ArcticWhiteBorder1 = (Color)ColorConverter.ConvertFromString("Black");

        //Office2003 header
        static readonly Color Office1 = (Color)ColorConverter.ConvertFromString("#FF9BD9A4");
        static readonly Color Office2 = (Color)ColorConverter.ConvertFromString("#FF286337");

        //Office2003 background
        static readonly Color OfficeBg1 = (Color)ColorConverter.ConvertFromString("#FF286337");
        static readonly Color OfficeBg2 = (Color)ColorConverter.ConvertFromString("#FF286337");
        static readonly Color OfficeBg3 = (Color)ColorConverter.ConvertFromString("#FF286337");
        
		//Office2003 text
		static readonly Color Office2003Text1 = (Color)ColorConverter.ConvertFromString("Black");
		
		//Office2003 border
		static readonly Color Office2003Border1 = (Color)ColorConverter.ConvertFromString("Black");
        
        //Windows7 header
        static readonly Color Windows1 = (Color)ColorConverter.ConvertFromString("#FF7CA6BE");
        static readonly Color Windows2 = (Color)ColorConverter.ConvertFromString("#FF468EA6");
        static readonly Color Windows3 = (Color)ColorConverter.ConvertFromString("#FF407D9F");
        static readonly Color Windows4 = (Color)ColorConverter.ConvertFromString("#FF0E5478");

        //Windows7 background
        static readonly Color WindowsBg1 = (Color)ColorConverter.ConvertFromString("#FF8CBBD6");
        static readonly Color WindowsBg2 = (Color)ColorConverter.ConvertFromString("#FF468EA6");
        
		//Windows7 text
		static readonly Color Windows7Text1 = (Color)ColorConverter.ConvertFromString("Black");

		//Windows7 border
		static readonly Color Windows7Border1 = (Color)ColorConverter.ConvertFromString("Black");


        public static ImageBrush LogoLightImageBrush
        {
            get
            {
                var logoBrush = new ImageBrush
                {
                    ImageSource = new BitmapImage(
                        new Uri(
                            @"pack://application:,,,/APLPromoter.UI.Wpf;component/Resources/Images/Logos/PromoterPNGLogo.png",
                            UriKind.Absolute)
                        )
                };
                return logoBrush;
            }
        }
        public static ImageBrush LogoDarkImageBrush
        {
            get
            {
                var logoBrush = new ImageBrush
                {
                    ImageSource = new BitmapImage(
                        new Uri(
                            @"pack://application:,,,/APLPromoter.UI.Wpf;component/Resources/Images/Logos/PromoterMessageCenterHeader.png",
                            UriKind.Absolute)
                        )
                };
                return logoBrush;
            }
        }
        public static Brush BlackIceHeaderBrush
        {
            get
            {
                return new LinearGradientBrush(
                        endPoint: new Point(.5D, 1D),
                        startPoint: new Point(.5D, 0D),
                        gradientStopCollection:
                            new GradientStopCollection(
                                    new GradientStop[]{ 
                                        new GradientStop(BlackIce1, 0D), 
                                        new GradientStop(BlackIce2, 0.583D),
                                        new GradientStop(BlackIce3, 0.483D)
                                    }
                                )
                    );
            }
        }

        public static Brush BlackIceTextBrush
        {
            get { return new SolidColorBrush(Colors.White); }
        }

        public static Brush BlackIceBorderBrush
        {
            get { return new SolidColorBrush(Colors.White); }
        }

        public static Brush BlackIceBackgroundBrush
        {
            get
            {
                return new LinearGradientBrush(
                        endPoint: new Point(.5D, 1D),
                        startPoint: new Point(.5D, 0D),
                        gradientStopCollection:
                            new GradientStopCollection(
                                    new GradientStop[]{ 
                                        new GradientStop(BlackIceBg1, 1D), 
                                        new GradientStop(BlackIceBg2, 0.756D),
                                        new GradientStop(BlackIceBg3, 0D)
                                    }
                                )
                    );
            }
        }

        public static Brush ArticWhiteHeaderBrush
        {
            get
            {
                return new LinearGradientBrush(
                        endPoint: new Point(0D, 1D),
                        startPoint: new Point(0D, 0D),
                        gradientStopCollection:
                            new GradientStopCollection(
                                    new GradientStop[]{ 
                                        new GradientStop(ArticWhite1, 0.522D), 
                                        new GradientStop(ArticWhite2, 1D),
                                        new GradientStop(ArticWhite3, 0D)
                                    }
                                )
                    );
            }
        }

        public static Brush ArticWhiteBackgroundBrush
        {
            get
            {
                return new LinearGradientBrush(
                        endPoint: new Point(0D, 1D),
                        startPoint: new Point(0D, 0D),
                        gradientStopCollection:
                            new GradientStopCollection(
                                    new GradientStop[]{ 
                                        new GradientStop(ArticWhiteBg1, 0.004D), 
                                        new GradientStop(ArticWhiteBg2, 0.366D),
                                        new GradientStop(ArticWhiteBg3, 1D)
                                    }
                                )
                    );
            }
        }

        public static Brush ArticWhiteTextBrush
        {
            get { return new SolidColorBrush(Colors.Black); }
        }

        public static Brush ArticWhiteBorderBrush
        {
            get { return new SolidColorBrush(Colors.Black); }
        }

        public static Brush Office2003HeaderBrush
        {
            get
            {

                return new LinearGradientBrush(
                        endPoint: new Point(.5, 1),
                        startPoint: new Point(.5, 0),
                        gradientStopCollection:
                            new GradientStopCollection(
                                    new GradientStop[]{ 
                                        new GradientStop(Office1, 0), 
                                        new GradientStop(Office2, 1),
                                    }
                                )
                    );
            }
        }

        public static Brush Office2003BackgroundBrush
        {
            get
            {

                return new LinearGradientBrush(
                        endPoint: new Point(.5, 1),
                        startPoint: new Point(.5, 0),
                        gradientStopCollection:
                            new GradientStopCollection(
                                    new GradientStop[]{ 
                                        new GradientStop(OfficeBg1, .519D), 
                                        new GradientStop(OfficeBg2, 1D), 
                                        new GradientStop(OfficeBg3, 0D),
                                    }
                                )
                    );
            }
        }

        public static Brush Office2003TextBrush
        {
            get { return new SolidColorBrush(Colors.White); }
        }

        public static Brush Office2003BorderBrush
        {
            get { return new SolidColorBrush(Colors.White); }
        }

        public static Brush Windows7HeaderBrush
        {
            get
            {
                
                return new LinearGradientBrush(
                        endPoint: new Point(.5D, 1D),
                        startPoint: new Point(.5D, 0D),
                        gradientStopCollection:
                            new GradientStopCollection(
                                    new GradientStop[]{ 
                                        new GradientStop(Windows1, 0D), 
                                        new GradientStop(Windows2, 1D),
                                        new GradientStop(Windows3, 0.474D),
                                        new GradientStop(Windows4, 0.517D)
                                    }
                                )
                    );
            }
        }

        public static Brush Windows7TextBrush
        {
            get { return new SolidColorBrush(Colors.White); }
        }

        public static Brush Windows7BorderBrush
        {
            get { return new SolidColorBrush(Colors.White); }
        }

        public static Brush Windows7BackgroundBrush
        {
            get
            {

                return new LinearGradientBrush(
                        endPoint: new Point(.5D, 1D),
                        startPoint: new Point(.5D, 0D),
                        gradientStopCollection:
                            new GradientStopCollection(
                                    new GradientStop[]{ 
                                        new GradientStop(Windows1, 0D), 
                                        new GradientStop(Windows2, 1D),
                                        new GradientStop(Windows2, 0.544D)
                                    }
                                )
                    );
            }
        }

        
    }

}
