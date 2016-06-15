using MyUniversity.Core.RatingModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyUniversity.WindowsPhone10
{
    class LessonsModelPresenter
    {
       
        public readonly ILessonsPage _view;
        public readonly IRatingModel _model;

        public LessonsModelPresenter(ILessonsPage view, IRatingModel model)
        {
            this._view = view;
            this._model = model;

            _model.AccountIncorrect += _model_AccountIncorrect;
            _model.UpdatingRating += _model_UpdatingRating;
            _model.NoNetwork += _model_NoNetwork;
        }


        private void _model_NoNetwork(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewErrorNoNetwork();
        }

        private void _model_UpdatingRating(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewProfile(e.Item as List<Lesson>);
        }

        private void _model_AccountIncorrect(object sender, Core.Сommon_Code.MessageEvent e)
        {
            _view.ViewErrorAccountIncorrect();
        }

        public void GetNotifications()
        {
            _model.GetRating();
        }


      
     
     
    }
}
