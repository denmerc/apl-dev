using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class MenuViewModel : ReactiveObject
    {
        private MenuItemsCollection _items;

        public MenuViewModel() {
            var promoterMenu =
                new MenuItemsCollection{
                    new MenuItem
                    {
                        Text = "Promoter",
                        Items = new MenuItemsCollection()
                        {
                            new MenuItem{Text="Login"},
                            new MenuItem{Text="Exit"}
                        }
                    },
                    new MenuItem
                    {
                        Text = "About",
                        Items = new MenuItemsCollection()
                        {
                            new MenuItem{Text="Us"},
                            new MenuItem{Text="Promoter"}
                        }
                    }
                };


            Items = promoterMenu;
        
        
        }
        public MenuViewModel(List<Client.Entity.Navigator> nodes)
        {
            //PlanningNodes = nodes;




            var promoterMenu =
                new MenuItemsCollection{
                    new MenuItem
                    {
                        Text = "Promoter",
                        Items = new MenuItemsCollection()
                        {
                            new MenuItem{Text="Login"},
                            new MenuItem{Text="Exit"}
                        }
                    },
                    new MenuItem
                    {
                        Text = "About",
                        Items = new MenuItemsCollection()
                        {
                            new MenuItem{Text="Us"},
                            new MenuItem{Text="Promoter"}
                        }
                    }
                };


            Items = promoterMenu;
        }




        private List<Client.Entity.Navigator> _rootNodes;
        public List<Client.Entity.Navigator> PlanningNodes
        {
            get
            { return _rootNodes; }
            set { this.RaiseAndSetIfChanged(ref _rootNodes, value); }
        }

        private MenuItemsCollection _Items;
        public MenuItemsCollection Items
        {
            get
            {
                return _Items;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _Items, value);
            }

        }
    }

    public class MenuItemsCollection : ObservableCollection<MenuItem>
    {
        private MenuItem parent;

        public MenuItemsCollection()
            : this(null)
        {
        }

        public MenuItemsCollection(MenuItem parent)
        {
            this.parent = parent;
        }

        public MenuItem Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        public void InsertItem(int index, MenuItem item)
        {
            item.Parent = this.Parent;
            base.InsertItem(index, item);
        }

    }


    [ContentProperty("Items")]
    public class MenuItem : ReactiveObject
    {
        private bool isChecked;
        private bool isEnabled = true;
        private string text;
        private string groupName;
        private bool isCheckable;
        private bool isSeparator;
        private string imageUrl;
        private bool staysOpenOnClick;
        private MenuItemsCollection items;
        private MenuItem parent;
        private int _FontSize;
        private string _NodeHeader;

        public MenuItem()
        {
            this.items = new MenuItemsCollection(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;



        public APLPromoter.Client.Entity.WorkflowStepType ViewType { get; set; }
        public Int32 EntityId { get; set; }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                
                this.RaiseAndSetIfChanged(ref text, value);
            }
        }

        
        public string NodeHeader
        {
            get
            {
                return this._NodeHeader;
            }
            set
            {

                this.RaiseAndSetIfChanged(ref _NodeHeader, value);
            }
        }

        public int FontSize
        {
            get
            {
                return this._FontSize;
            }
            set
            {

                this.RaiseAndSetIfChanged(ref _FontSize, value);
            }
        }

        public string GroupName
        {
            get
            {
                return this.groupName;
            }
            set
            {
                this.groupName = value;
            }
        }

        public bool IsCheckable
        {
            get
            {
                return this.isCheckable;
            }
            set
            {
                this.isCheckable = value;
            }
        }

        public bool IsSeparator
        {
            get
            {
                return this.isSeparator;
            }
            set
            {
                this.isSeparator = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                return this.imageUrl;
            }
            set
            {
                this.imageUrl = value;
            }
        }

        public bool StaysOpenOnClick
        {
            get
            {
                return this.staysOpenOnClick;
            }
            set
            {
                this.staysOpenOnClick = value;
            }
        }


        private MenuItemsCollection _Items;
        public MenuItemsCollection Items
        {
            get
            {
                return _Items;
            }
            set
            {
               _Items = value;
            }

        }

        public MenuItem Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                if (value != this.isEnabled)
                {
                    this.isEnabled = value;
                    this.OnPropertyChanged("IsEnabled");
                }
            }
        }

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                if (value != this.isChecked)
                {
                    this.isChecked = value;
                    this.OnPropertyChanged("IsChecked");

                    if (!string.IsNullOrEmpty(this.GroupName))
                    {
                        if (this.IsChecked)
                        {
                            this.UncheckOtherItemsInGroup();
                        }
                        else
                        {
                            this.IsChecked = true;
                        }
                    }
                }
            }
        }

        public Image Image
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImageUrl)) return null;

                return new Image()
                {
                    Source = new BitmapImage(new Uri(this.ImageUrl, UriKind.RelativeOrAbsolute)),
                    Stretch = Stretch.None
                };
            }
        }

        private void UncheckOtherItemsInGroup()
        {
            IEnumerable<MenuItem> groupItems = this.Parent.Items.Where((MenuItem item) => item.GroupName == this.GroupName);
            foreach (MenuItem item in groupItems)
            {
                if (item != this)
                {
                    item.isChecked = false;
                    item.OnPropertyChanged("IsChecked");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (null != this.PropertyChanged)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
