using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace APLPromoter.Server.Entity
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
        public String Name; //CLIENT { get; private set; }
        [DataMember]
        public List<Value> Values; //CLIENT { get; private set; }

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
            public Int32 Id; //CLIENT { get; private set; }
            [DataMember]
            public Int32 Key; //CLIENT { get; private set; }
            [DataMember]
            public String Code; //CLIENT { get; private set; }
            [DataMember]
            public String Name; //CLIENT { get; private set; }
            [DataMember]
            public Boolean Included; //CLIENT { get; set; }
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
        public String Name; //CLIENT { get; private set; }
        [DataMember]
        public List<Value> Values; //CLIENT { get; private set; }

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
            public Int32 Id; //CLIENT { get; private set; }
            [DataMember]
            public Int32 Key; //CLIENT { get; private set; }
            [DataMember]
            public String Code; //CLIENT { get; private set; }
            [DataMember]
            public String Name; //CLIENT { get; private set; }
            [DataMember]
            public Boolean Included; //CLIENT { get; set; }
        }
    }

    [DataContract]
    public class Workflow
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

        [DataMember]
        public String Title; //CLIENT { get; private set; }
        [DataMember]
        public List<Step> Steps; //CLIENT { get; private set; }
        [DataMember]
        public WorkflowType ThisWorkflowType;

        [DataContract]
        public class Step
        {
            #region Initialize...
            public Step() { }
            public Step(
                Int16 Index,
                String Name,
                String Caption,
                Boolean IsValid,
                Boolean IsActive,
                Boolean IsEnabledPrevious,
                Boolean IsEnabledNext,
                List<Error> Errors,
                List<Advisor> Advisors,
                WorkflowStepType ThisStepType
                ) {
                    this.Index = Index;
                    this.Name = Name;
                    this.Caption = Caption;
                    this.IsValid = IsValid;
                    this.IsActive = IsActive;
                    this.IsEnabledPrevious = IsEnabledPrevious;
                    this.IsEnabledNext = IsEnabledNext;
                    this.Errors = Errors;
                    this.Advisors = Advisors;
                    this.ThisStepType = ThisStepType;
            }
            #endregion

            [DataMember]
            public Int16 Index; //CLIENT { get; private set; }
            [DataMember]
            public String Name; //CLIENT { get; private set; }
            [DataMember]
            public String Caption; //CLIENT { get; set; }
            [DataMember]
            public Boolean IsValid; //CLIENT { get; set; }
            [DataMember]
            public Boolean IsActive; //CLIENT { get; set; }
            [DataMember]
            public Boolean IsEnabledPrevious; //CLIENT { get; set; }
            [DataMember]
            public Boolean IsEnabledNext; //CLIENT { get; set; }
            [DataMember]
            public List<Advisor> Advisors; //CLIENT { get; private set; }
            [DataMember]
            public List<Error> Errors; //CLIENT { get; private set; }
            [DataMember]
            public WorkflowStepType ThisStepType;
        }

        [DataContract]
        public class Advisor
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
            public Int32 SortId; //CLIENT { get; private set; }
            [DataMember]
            public String Message; //CLIENT { get; private set; }
        }

        [DataContract]
        public class Error
        {
            #region Initialize...
            public Error() { }
            public Error(
                String Message
                ) {
                    this.Message = Message;
            }
            #endregion

            [DataMember]
            public String Message; //CLIENT { get; set; }
        }
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
            this.NodeHeader = NodeHeader;
            this.NodeCaption = NodeCaption;
            this.Workflow = Workflow;
            this.WorkflowStep = WorkflowStep;
            this.WorkflowGroup = WorkflowGroup;
            this.WorkflowReadonly = WorkflowReadonly;
            this.Nodes = Nodes;
        }
        #endregion

        [DataMember]
        public Int32 EntityId; // CLIENT { get; private set; }
        [DataMember]
        public String NodeHeader; // CLIENT { get; private set; }
        [DataMember]
        public String NodeTitle; // CLIENT { get; private set; }
        [DataMember]
        public String NodeCaption; // CLIENT { get; private set; }
        [DataMember]
        public WorkflowType Workflow; //CLIENT { get; private set; }
        [DataMember]
        public WorkflowStepType WorkflowStep; //CLIENT { get; private set; }
        [DataMember]
        public WorkflowGroupType WorkflowGroup; //CLIENT { get; private set; }
        [DataMember]
        public Boolean WorkflowReadonly; // CLIENT { get; private set; }
        [DataMember]
        public List<Server.Entity.Navigator> Nodes; //CLIENT { get; private set; }
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
            ) {
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
