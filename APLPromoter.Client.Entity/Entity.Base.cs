﻿using APLPromoter.Client.Entity;
using FluentValidation;
using FluentValidation.Results;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;


namespace APLPromoter.Client.Entity
{
    //[DataContract]
    public class ObjectBase : NotificationObject, IDataErrorInfo
    {
        protected IValidator _Validator;
        protected IEnumerable<ValidationFailure> _ValidationErrors = null;
        protected bool _IsDirty;


        public ObjectBase(){
            _Validator = GetValidator();
            Validate();
        }

        //[DataMember]
        public IEnumerable<ValidationFailure> ValidationErrors{
            get{ return _ValidationErrors;}
            set {}
        }
        
        public void Validate()
        {
            if(_Validator != null){
                ValidationResult results = _Validator.Validate(this);
                _ValidationErrors = results.Errors;
            }
        }
        //[DataMember]
        public virtual bool IsValid
        {
            get
            {
                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                    return false;
                else
                    return true;
            }
        }
        
        protected virtual IValidator GetValidator()
        {
 	        return null;
        }


        #region Property Change Notification
        protected override void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
        {
            string propertyName = Core.Utils.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName, makeDirty);

        }

        protected void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            base.OnPropertyChanged(propertyName);

            if (makeDirty)
                _IsDirty = true;

            Validate();
        }
        #endregion

        #region IDirty Members
        public virtual bool IsDirty{
            get { return _IsDirty; }
            
            set{ //TODO: removed protected here to access from tests to initialize 
                _IsDirty = value;
                OnPropertyChanged("IsDirty", false);
            }
        }
        #endregion

        #region IDataErrorInfo members
        [DataMember]
        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                StringBuilder errors = new StringBuilder();

                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                {
                    foreach (ValidationFailure validationError in _ValidationErrors)
                    {
                        if (validationError.PropertyName == columnName)
                            errors.AppendLine(validationError.ErrorMessage);
                    }
                }

                return errors.ToString();
            }
        }

        #endregion
    }


    public class NotificationObject : INotifyPropertyChanged
    {
        private event PropertyChangedEventHandler _PropertyChangedEvent;
        protected List<PropertyChangedEventHandler> _PropertyChangedSubscribers = new List<PropertyChangedEventHandler>();

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!_PropertyChangedSubscribers.Contains(value))
                {
                    _PropertyChangedEvent += value;
                    _PropertyChangedSubscribers.Add(value);
                }
            }
            remove
            {
                _PropertyChangedEvent -= value;
                _PropertyChangedSubscribers.Remove(value);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_PropertyChangedEvent != null)
                _PropertyChangedEvent(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = Core.Utils.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName);
        }
    }


    [DataContract]
    public class ReactiveValidatedEntity : ReactiveObject, IDataErrorInfo
    {
        protected IValidator _Validator;
        protected IValidator Validator
        {
            get { return _Validator; }
            set { _Validator = value; }
        }

        public ReactiveValidatedEntity()
        {
            _Validator = GetValidator();

        }

        public void Dispose()
        {

        }

        public void Validate()
        {
            if (_Validator != null)
            {
                ValidationResult results = _Validator.Validate(this);
                //_ValidationErrors = results.Errors.CreateDerivedCollection();
                _ValidationErrors = results.Errors;
            }
        }
        [DataMember]
        protected Workflow _Workflow = null;
        public Workflow Workflow
        {
            get { return _Workflow; }
            set { this.RaiseAndSetIfChanged(ref _Workflow, value); }
        }
        [DataMember]
        protected Boolean _IsDirty = false;
        public Boolean IsDirty
        {
            get { return _IsDirty; }
            set { this.RaiseAndSetIfChanged(ref _IsDirty, value); }
        }

        [IgnoreDataMember]
        private string _headerTitle;
        public string HeaderTitle
        {
            get
            {
                return _headerTitle;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _headerTitle, value);
            }
        }

        [IgnoreDataMember]
        private string _StepHeader;
        public string StepHeader
        {
            get
            {
                return _StepHeader;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _StepHeader, value);
            }
        }

        [DataMember]
        public Int32 EntityId { get; set; }
        [DataMember]
        protected Boolean IsEditing { get; set; }

        [IgnoreDataMember]
        public ReactiveCommand PreviousCommand { get; set; }
        [IgnoreDataMember]
        public ReactiveCommand SaveCommand { get; set; }
        [IgnoreDataMember]
        public ReactiveCommand CancelCommand { get; set; }
        [IgnoreDataMember]
        public ReactiveCommand ClearCommand { get; set; }
        [IgnoreDataMember]
        public ReactiveCommand NextCommand { get; set; }

        
        
        [DataMember]
        protected IEnumerable<ValidationFailure> _ValidationErrors = null;
        [DataMember]
        public IEnumerable<ValidationFailure> ValidationErrors
        {
            get { return _ValidationErrors; }
            set { this.RaiseAndSetIfChanged(ref _ValidationErrors, value); }
        }

        [DataMember]
        protected ReactiveList<Client.Entity.Workflow.Error> _ServerErrors = null;
        public ReactiveList<Client.Entity.Workflow.Error> ServerErrors
        {
            get { return _ServerErrors; }
            set { this.RaiseAndSetIfChanged(ref _ServerErrors, value); }
        }

        protected virtual IValidator GetValidator()
        {
            return null;
        }

        [DataMember]
        public virtual bool IsValid
        {
            get
            {
                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                    return false;
                else
                    return true;
            }
            set { }
        }


        [IgnoreDataMember]
        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
            
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                StringBuilder errors = new StringBuilder();

                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                {
                    foreach (ValidationFailure validationError in _ValidationErrors)
                    {
                        if (validationError.PropertyName == columnName)
                            errors.AppendLine(validationError.ErrorMessage);
                    }
                }

                return errors.ToString();
            }
        }


    }

    
}
