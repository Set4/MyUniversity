using MyUniversity.Core.AuthenticationModel;
using MyUniversity.Core.ProfileModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyUniversity.WindowsPhone10
{
    public class MainPagePresenter
    {
    



    

        public string Title { get; set; }


        // "Мой КГЭУ";
        //  Type page;

        public readonly IViewMainPage _view;

        public readonly IAuthentificationModel _model;

        public MainPagePresenter(IViewMainPage view, IAuthentificationModel model)
        {
            _view = view;
            _model = model;
        }

      
       
     
    

    


       
    }
}
