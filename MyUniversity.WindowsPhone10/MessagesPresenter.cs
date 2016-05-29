using MyUniversity.Core.NotificationModel;
using MyUniversity.Core.Сommon_Code;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyUniversity.WindowsPhone10
{
    class MessagesModelPresenter
    {
        public readonly IMessagesCollectionPage _view;
        public readonly INotificationModel _model;
      

        public event EventHandler<MessageEvent> SelectedMessage = delegate { };

        public MessagesModelPresenter(IMessagesCollectionPage view, INotificationModel model)
        {
            this._view = view;
            this._model = model;


            _model.AccountIncorrect += _model_AccountIncorrect;
            _model.UpdatingNotification += _model_UpdatingNotification;
            _model.NoNetwork += _model_NoNetwork;
        }

        private void _model_NoNetwork(object sender, MessageEvent e)
        {
            _view.ViewErrorNoNetwork();
        }

        private void _model_UpdatingNotification(object sender, MessageEvent e)
        {
            _view.ViewNotifications(e.Item as List<Notification>);
        }

        private void _model_AccountIncorrect(object sender, MessageEvent e)
        {
            _view.ViewErrorAccountIncorrect();
        }

        public void GetNotifications()
        {
            _model.GetNotifications();
        }
        public void Readet(Notification item)
        {
            _model.ReadNotifications(item);
        }
    }

 
}
