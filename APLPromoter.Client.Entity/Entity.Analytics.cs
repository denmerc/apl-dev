using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace APLPromoter.Client.Entity
{
    [DataContract]
    public class Analytic
    {
        [DataMember]
        public List<Filter> Filters { get; private set; }
        [DataMember]
        public List<Driver> Drivers { get; private set; }
        [DataMember]
        public List<PriceList> PriceLists { get; private set; }
        [DataMember]
        public List<Workflow> Workflow { get; private set; }
        [DataMember]
        public Identity Self { get;  set; }

        [DataContract]
        public class Identity
        {

            #region Initialize...
            public Identity() { }
            public Identity(
                Int32 Id,
                String Name,
                String Description,
                String RefreshedText,
                String CreatedText,
                String EditedText,
                DateTime Refreshed,
                DateTime Created,
                DateTime Edited,
                String Author,
                String Editor,
                String Owner,
                Boolean Active
                ) {
                    this.Id = Id;
                    this.Name = Name;
                    this.Description = Description;
                    this.Refreshed = Refreshed;
                    this.RefreshedText = RefreshedText;
                    this.Created = Created;
                    this.CreatedText = CreatedText;
                    this.Edited = Edited;
                    this.EditedText = EditedText;
                    this.Author = Author;
                    this.Editor = Editor;
                    this.Owner = Owner;
                    this.Active = Active;
            }
            #endregion

            [DataMember]
            public Int32 Id { get;  set; }
            [DataMember]
            public String Name { get;  set; }
            [DataMember]
            public String Description { get; set; }
            [DataMember]
            public DateTime Refreshed;
            [DataMember]
            public String RefreshedText;
            [DataMember]
            public DateTime Created;
            [DataMember]
            public String CreatedText;
            [DataMember]
            public DateTime Edited;
            [DataMember]
            public String EditedText;
            [DataMember]
            public String Author;
            [DataMember]
            public String Editor;
            [DataMember]
            public String Owner;
            [DataMember]
            public Boolean Active;
        }

        [DataContract]
        public class Driver
        {
            #region Initialize...
            public Driver() { }
            public Driver(
                Int32 Id,
                Int32 Key,
                String Name,
                String Tooltip,
                Boolean Selected,
                List<Mode> Modes
                ) {
                    this.Id=Id;
                    this.Key = Key;
                    this.Name = Name;
                    this.Tooltip = Tooltip;
                    this.Selected = Selected;
                    this.Modes = Modes;
            }
            #endregion

            [DataMember]
            public Int32 Id { get; private set; }
            [DataMember]
            public Int32 Key { get; private set; }
            [DataMember]
            public String Name { get; private set; }
            [DataMember]
            public String Tooltip { get; private set; }
            [DataMember]
            public Boolean Selected { get; set; }
            [DataMember]
            public List<Mode> Modes { get; private set; }

            [DataContract]
            public class Mode {

                #region Initialize...
                public Mode() { }
                public Mode(
                    Int32 Key,
                    String Name,
                    String Tooltip,
                    Boolean Selected,
                    List<Group> Groups
                    ) {
                    this.Key = Key;
                    this.Name = Name;
                    this.Tooltip = Tooltip;
                    this.Selected = Selected;
                    this.Groups = Groups;
                }
                #endregion

                [DataMember]
                public Int32 Key { get; private set; }
                [DataMember]
                public String Name { get; private set; }
                [DataMember]
                public String Tooltip { get; private set; }
                [DataMember]
                public Boolean Selected { get; private set; }
                [DataMember]
                public List<Group> Groups { get; private set; }

                [DataContract]
                public class Group {

                    #region Initialize...
                    public Group() { }
                    public Group(
                        Int32 Id,
                        Int32 Value,
                        Decimal MinOutlier,
                        Decimal MaxOutlier
                        ) {
                        this.Id = Id;
                        this.Value = Value;
                        this.MinOutlier = MinOutlier;
                        this.MaxOutlier = MaxOutlier;
                    }
                    #endregion

                    [DataMember]
                    public Int32 Id { get; private set; }
                    [DataMember]
                    public Int32 Value { get; private set; }
                    [DataMember]
                    public Decimal MinOutlier { get; private set; }
                    [DataMember]
                    public Decimal MaxOutlier { get; private set; }
                }
            }

            public Mode this[String index] {
                get {
                    Mode mode = new Mode();
                    foreach (Mode item in this.Modes) {
                        if (item.Name == index) {
                            mode = item;
                            break;
                        }
                    }
                    return mode;
                }
            }
        }
    }
}




