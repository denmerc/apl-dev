using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace APLPromoter.Server.Entity
{
    [DataContract]
    public class Analytic
    {
        [DataMember]
        public List<Filter> Filters; //CLIENT { get; private set; }
        [DataMember]
        public List<Driver> Drivers; //CLIENT { get; private set; }
        [DataMember]
        public List<PriceList> PriceLists; //CLIENT { get; private set; }
        [DataMember]
        public List<Workflow> Workflow; //CLIENT { get; private set; }
        [DataMember]
        public Identity Self; //CLIENT { get; private set; }

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
            public Int32 Id; //CLIENT { get; set; }
            [DataMember]
            public String Name; //CLIENT { get; set; }
            [DataMember]
            public String Description; //CLIENT { get; set; }
            [DataMember]
            public DateTime Refreshed; //CLIENT { get; private set; }
            [DataMember]
            public String RefreshedText; //CLIENT { get; private set; }
            [DataMember]
            public DateTime Created; //CLIENT { get; private set; }
            [DataMember]
            public String CreatedText; //CLIENT { get; private set; }
            [DataMember]
            public DateTime Edited; //CLIENT { get; private set; }
            [DataMember]
            public String EditedText; //CLIENT { get; private set; }
            [DataMember]
            public String Author; //CLIENT { get; private set; }
            [DataMember]
            public String Editor; //CLIENT { get; private set; }
            [DataMember]
            public String Owner; //CLIENT { get; private set; }
            [DataMember]
            public Boolean Active; //CLIENT { get; private set; }
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
            public Int32 Id; //CLIENT { get; private set; }
            [DataMember]
            public Int32 Key; //CLIENT { get; private set; }
            [DataMember]
            public String Name; //CLIENT { get; private set; }
            [DataMember]
            public String Tooltip; //CLIENT { get; private set; }
            [DataMember]
            public Boolean Selected; //CLIENT { get; set; }
            [DataMember]
            public List<Mode> Modes; //CLIENT { get; private set; }

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
                public Int32 Key; //CLIENT { get; private set; }
                [DataMember]
                public String Name; //CLIENT { get; private set; }
                [DataMember]
                public String Tooltip; //CLIENT { get; private set; }
                [DataMember]
                public Boolean Selected; //CLIENT { get; set; }
                [DataMember]
                public List<Group> Groups; //CLIENT { get; private set; }

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
                    public Int32 Id; //CLIENT { get; private set; }
                    [DataMember]
                    public Int32 Value; //CLIENT { get; set; }
                    [DataMember]
                    public Decimal MinOutlier; //CLIENT { get; set; }
                    [DataMember]
                    public Decimal MaxOutlier; //CLIENT { get; set; }
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




