using APLPromoter.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace APLPromoter.Client.Entity
{
    [DataContract]
    public class PriceList
    {
        #region Initialize...
        public PriceList() { }
        public PriceList(
            String Name,
            List<Value> Values
            ) {
            this.Name = Name;
            this.Values = Values;
        }
        #endregion

        [DataMember]
        public String Name { get; private set; }
        [DataMember]
        public List<Value> Values { get; private set; }

        [DataContract]
        public class Value
        {
            #region Initialize...
            public Value() { }
            public Value(
                Int32 Id,
                Int32 Key,
                String Code,
                String Name,
                Boolean Included
                ) {
                this.Id = Id;
                this.Key = Key;
                this.Code = Code;
                this.Name = Name;
                this.Included = Included;
            }
            #endregion

            [DataMember]
            public Int32 Id { get; private set; }
            [DataMember]
            public Int32 Key { get; private set; }
            [DataMember]
            public String Code { get; private set; }
            [DataMember]
            public String Name { get; private set; }
            [DataMember]
            public Boolean Included { get; private set; }
        }
    }

    [DataContract]
    public class Filter
    {
        #region Initialize...
        public Filter() { }
        public Filter(
            String Name,
            List<Value> Values
            ) {
            this.Name = Name;
            this.Values = Values;
        }
        #endregion

        [DataMember]
        public String Name { get; private set; }
        [DataMember]
        public List<Value> Values { get; private set; }

        [DataContract]
        public class Value
        {
            #region Initialize...
            public Value() { }
            public Value(
                Int32 Id,
                Int32 Key,
                String Code,
                String Name,
                Boolean Included
                ) {
                this.Id = Id;
                this.Key = Key;
                this.Code = Code;
                this.Name = Name;
                this.Included = Included;
            }
            #endregion

            [DataMember]
            public Int32 Id { get;  set; }
            [DataMember]
            public Int32 Key { get;  set; }
            [DataMember]
            public String Code { get;  set; }
            [DataMember]
            public String Name { get;  set; }
            [DataMember]
            public Boolean Included { get;  set; }
        }
    }

    [DataContract]
    public class Workflow : ReactiveObject
    {

        #region Initialize...
        public Workflow() { }
        public Workflow(
            String Title,
            List<Step> Steps,
            WorkflowType WorkflowType
            ) {
            this.Title = Title;
            this.Steps = Steps;
            this.ThisWorkflowType = WorkflowType;
        }
        #endregion
        string _title;
        [DataMember]
        public String Title 
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        
        }
        
        
        List<Step> _steps;
        [DataMember]
        public List<Step> Steps{
            get { return _steps; }
            set { this.RaiseAndSetIfChanged(ref _steps, value); }
        }

        [DataMember]
        public WorkflowType ThisWorkflowType;


        [DataContract]
        public class Step : ReactiveValidatedEntity
        {
            #region Initialize...
            public Step() { }
            public Step(
                Int16 Index,
                String Name,
                String Caption,
                Boolean IsValid,
                Boolean IsActive,
                List<Error> Errors,
                List<Advisor> Advisors,
                WorkflowStepType WorkflowStepType
                ){
                this.Index = Index;
                this.Name = Name;
                this.Caption = Caption;
                this.IsValid = IsValid;
                this.IsActive = IsActive;
                this.IsEnabledPrevious = IsEnabledPrevious;
                this.IsEnabledNext = IsEnabledNext;
                this.Errors = Errors;
                this.Advisors = Advisors;
                this.ThisStepType = WorkflowStepType;
            }
            #endregion
            [IgnoreDataMember]
            ReactiveValidatedEntity _StepViewModel;
            public ReactiveValidatedEntity StepViewModel
            {
                get { return _StepViewModel;}
                set { this.RaiseAndSetIfChanged(ref _StepViewModel, value); }
            }

            Int32 _Index;
            [DataMember]
            public Int32 Index
            {
                get { return _Index; }
                set { this.RaiseAndSetIfChanged(ref _Index, value); }
            }
            
            Boolean _isEnabled;
            [DataMember]
            public Boolean IsEnabled
            {
                get { return _isDirty; }
                set { this.RaiseAndSetIfChanged(ref _isDirty, value); }
            }

            Boolean _isDirty;
            [DataMember]
            public Boolean IsDirty
            {
                get { return _isDirty; }
                set { this.RaiseAndSetIfChanged(ref _isDirty, value); }
            }

            [DataMember]
            public Boolean IsEnabledPrevious { get; set; }
            [DataMember]
            public Boolean IsEnabledNext { get; set; }


            string _name;
            [DataMember]
            public string Name {
                get { return _name; }
                set { this.RaiseAndSetIfChanged(ref _name, value); }
            }
            [DataMember]
            string _caption;
            public String Caption {
                get { return _caption; }
                set { this.RaiseAndSetIfChanged(ref _caption, value); }
            }


            Boolean _isValid;
            [DataMember]
            public Boolean IsValid {
                get { return _isValid; }
                set { this.RaiseAndSetIfChanged(ref _isValid, value);}
            }

            Boolean _IsActive;
            [DataMember]
            public Boolean IsActive {
                get { return _IsActive; }
                set { this.RaiseAndSetIfChanged(ref _IsActive, value);}
            }

            Boolean _IsWorking;
            [IgnoreDataMember]
            public Boolean IsWorking
            {
                get { return _IsWorking; }
                set { this.RaiseAndSetIfChanged(ref _IsWorking, value); }
            }

            List<Advisor> _advisors;
            [DataMember]
            public List<Advisor> Advisors {
                get { return _advisors; }
                set { this.RaiseAndSetIfChanged(ref _advisors, value); }
            }

            List<Error> _Errors;
            [DataMember]
            public List<Error> Errors {
                get { return _Errors; }
                set { this.RaiseAndSetIfChanged(ref _Errors, value); }
            }
            [DataMember]
            public WorkflowStepType ThisStepType;
        }

        [DataContract]
        public class Advisor : ReactiveObject
        {
            #region Initialize...
            public Advisor() { }
            public Advisor(
                Int32 SortId,
                String Message
                ) {
                this.SortId = SortId;
                this.Message = Message;
            }
            #endregion

            [DataMember]
            public Int32 SortId { get;  private set; }
            [DataMember]
            string _Message;
            [DataMember]
            public String Message
            {
                get { return _Message; }
                set { this.RaiseAndSetIfChanged(ref _Message, value); }

            }
            [IgnoreDataMember]
            public List<Error> Errors { get; set; }
        }

        [DataContract]
        public class Error : ReactiveObject
        {
            #region Initialize...
            public Error() { }
            public Error(
                String Message
                )
            {
                this.Message = Message;
            }
            #endregion

            string _Message;
            [DataMember]
            public String Message
            {
                get{ return _Message;}
                set { this.RaiseAndSetIfChanged(ref _Message, value);}
                
            }
        }

    }

    //TODO: change to WorkflowStepType
    public enum StepType
    {
        Initialization = 0,
        Authentication,
        PasswordChange
    }



    [DataContract]
    public class Navigator
    {
        #region Initialize...
        public Navigator() { }
        public Navigator(
            Int32 EntityId,
            String NodeHeader,
            String NodeTitle,
            String NodeCaption,
            WorkflowType Workflow,
            WorkflowStepType WorkflowStep,
            WorkflowGroupType WorkflowGroup,
            Boolean WorkflowReadonly,
            List<Navigator> Nodes
            ) {
            this.EntityId = EntityId;
            this.NodeHeader = NodeHeader;
            this.NodeTitle = NodeTitle;
            this.NodeCaption = NodeCaption;
            this.Workflow = Workflow;
            this.WorkflowStep = WorkflowStep;
            this.WorkflowGroup = WorkflowGroup;
            this.WorkflowReadonly = WorkflowReadonly;
            this.Nodes = Nodes;
        }
        #endregion

        [DataMember]
        public Int32 EntityId{ get;  set; } 
        [DataMember]
        public String NodeTitle { get;  private set; }
        [DataMember]
        public String NodeHeader { get;  set; }
        [DataMember]
        public String NodeCaption { get; private set; }
        [DataMember]
        public WorkflowType Workflow { get; set; }
        [DataMember]
        public WorkflowStepType WorkflowStep { get;  set; }
        [DataMember]
        public WorkflowGroupType WorkflowGroup { get;   set; }
        [DataMember]
        public Boolean WorkflowReadonly { get;  private set; }
        [DataMember]
        public List<Client.Entity.Navigator> Nodes{ get; private set; }
    }

    [DataContract]
    public class SQLEnumeration
    {
        #region Initialize...
        public SQLEnumeration() { }
        public SQLEnumeration(
            Int16 Sort,
            Int16 Value,
            String Name,
            String Description
            )
    {
            this.Sort = Sort;
            this.Value = Value;
            this.Name = Name;
            this.Description = Description;
        }
        #endregion

        [DataMember]
        public Int16 Value;
        [DataMember]
        public String Name;
        [DataMember]
        public String Description;
        [DataMember]
        public Int16 Sort;
    }
}




