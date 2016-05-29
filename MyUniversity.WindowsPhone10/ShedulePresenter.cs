using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MyUniversity.Core.ScheduleModel;

namespace MyUniversity.WindowsPhone10
{
    class ShedulePresenter
    {
        public readonly IShedulePage _view;
        public readonly IScheduleModel _model;

      

        public ShedulePresenter(IShedulePage view, IScheduleModel model)
        {
            this._view = view;
            this._model = model;

            _model.NoNetwork += _model_NoNetwork;
            _model.ScheduleItemsUpdated += _model_ScheduleItemsUpdated;
            _model.WeeksListCreated += _model_WeeksListCreated;
            _model.AccountIncorrect += _model_AccountIncorrect;
          
        }

        private void _model_AccountIncorrect(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewErrorAccountIncorrect();
        }

        private void _model_NoNetwork(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewErrorNoNetwork();
        }

        private void _model_WeeksListCreated(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.SetSchedule(e.Item as List<WeekData>);
            _view.ViewSchedule();

        }



        private void _model_ScheduleItemsUpdated(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _model.CreateListWeeks();
        }

        

        public void GetWeeks()
        {
            _model.GetSchedule();
        }

      
   
    }
}
