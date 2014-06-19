﻿using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;

namespace APLPromoter.UI.Wpf.Views
{
    public class ViewRouter : ReactiveObject, IScreen
    {
        public ViewRouter()
        {
            Router = new RoutingState();
            var resolver = RxApp.MutableResolver;
            resolver.RegisterConstant(this, typeof(IScreen));
            resolver.Register(() => new LoginView(), typeof(IViewFor<LoginViewModel>));
            resolver.Register(() => new MainView(), typeof(IViewFor<MainViewModel>));
            resolver.Register(() => new HomeView(), typeof(IViewFor<HomeViewModel>));
            resolver.Register(() => new AnalyticFrame(), typeof(IViewFor<AnalyticViewModel>));
            resolver.Register(() => new PricingFrame(), typeof(IViewFor<PriceRoutineViewModel>));
            resolver.Register(() => new AdministrationFrame(), typeof(IViewFor<AdminViewModel>));
            //resolver.Register(() => new PriceRoutineEditView(), typeof(IViewFor<PriceRoutineViewModel>));
            resolver.Register(() => new AboutUsView(), typeof(IViewFor<AboutUsViewModel>));

            var app = App.Current.Resources["Locator"] as ViewModelLocator;
            //Router.Navigate.Execute(new LoginViewModel(this
            //    , new MainViewModel(this),
            //    new Promoter.Services.UserService()));


            //Router.Navigate.Execute(new LoginViewModel(this
            //    , new MainViewModel(this),
            //    app.UserService));

            Router.Navigate.Execute(app.MainViewModel);
        }
        public IRoutingState Router { get; set; }
    }
}
