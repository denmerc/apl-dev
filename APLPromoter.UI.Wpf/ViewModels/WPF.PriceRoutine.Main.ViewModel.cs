using APLPromoter.Client.Entity;
using ReactiveUI;
using System;
using System.Linq;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class PriceRoutineViewModel : ReactiveValidatedEntity
    {
        public PriceRoutineViewModel()
        {
        }

        public PriceRoutineViewModel(Int32 id, Session<NullT> session)
        {
            Id = id;
            Workflow = session.UserIdentity.Role.Planning.Workflows.SingleOrDefault(x => x.ThisWorkflowType == WorkflowType.PlanningPricing);
            StepList = this.Workflow.Steps
                .CreateDerivedCollection(x => x);
            SelectedStep = StepList.FirstOrDefault(x => x.IsActive == true);        
        }

        private Int32 _id;
        public Int32 Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.RaiseAndSetIfChanged(ref _id, value);
                }
            }
        }

        private Workflow.Step _SelectedStep;
        public Workflow.Step SelectedStep
        {
            get
            { return _SelectedStep; }
            set
            {

                _SelectedStep = value;
                this.RaiseAndSetIfChanged(ref _SelectedStep, value);

            }
        }



        private IReactiveDerivedList<Workflow.Step> _stepList;
        public IReactiveDerivedList<Workflow.Step> StepList
        {
            get
            {
                return _stepList;
            }
            set
            {
                if (_stepList != value)
                {
                    _stepList = value;
                    this.RaiseAndSetIfChanged(ref _stepList, value);
                }
            }
        }
    }
}
