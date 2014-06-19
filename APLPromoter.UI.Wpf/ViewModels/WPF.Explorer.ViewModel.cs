
using APLPromoter.Client.Entity;
using ReactiveUI;
using System.Collections.Generic;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class ExplorerViewModel :  ReactiveObject
    {
        public ExplorerViewModel(Session<NullT> session)
        {

            Explorers = new List<User.Role.Explorer>();
            var planning = session.UserIdentity.Role.Planning;
            planning.IsExpanded = true;
            Explorers.Add(planning);
            Explorers.Add(new User.Role.Explorer
            {
                Title = "Tracking",
                IsExpanded = false,
                Name = "TrackingTree",
                Navigators = new List<Navigator> 
                    { 
                        //new Navigator {NodeTitle = "Node1"},
                        //new Navigator {NodeTitle = "Node2"}
                    }
            });
            Explorers.Add(new User.Role.Explorer
            {
                Title = "Reporting",
                IsExpanded = false,
                Name = "PlanningTree",
                Navigators = new List<Navigator> 
                    { 
                        //new Navigator {NodeTitle = "Node1"},
                        //new Navigator {NodeTitle = "Node2"}
                    }
            });
            if (session.UserIdentity.Role.Tracking != null)
            {
                Explorers.Add(session.UserIdentity.Role.Tracking);
            }
            if (session.UserIdentity.Role.Reporting != null)
            {
                Explorers.Add(session.UserIdentity.Role.Planning);
            }
        }

        private List<APLPromoter.Client.Entity.User.Role.Explorer> _explorers;
        public List<APLPromoter.Client.Entity.User.Role.Explorer> Explorers
        {
            get
            { return _explorers; }
            set { this.RaiseAndSetIfChanged(ref _explorers, value); }
        }


        private List<Client.Entity.Navigator> _rootNodes;
        public List<Client.Entity.Navigator> RootNodes
        {
            get
            { return _rootNodes; }
            set { this.RaiseAndSetIfChanged(ref _rootNodes, value); }
        }
        
    }


}
