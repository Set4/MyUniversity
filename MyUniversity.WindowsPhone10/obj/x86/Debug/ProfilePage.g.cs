﻿#pragma checksum "C:\Users\badpi\Documents\GitHub\MyUniversity\MyUniversity.WindowsPhone10\ProfilePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AE849632011D9385BA0EB4031C3AF7A1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyUniversity.WindowsPhone10
{
    partial class ViewProfil : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.btnflt = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 2:
                {
                    this.imgProfileImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 3:
                {
                    this.btnLogOut = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 258 "..\..\..\ProfilePage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnLogOut).Tapped += this.btnLogOut_Tapped;
                    #line default
                }
                break;
            case 4:
                {
                    this.txblockModeofStudy = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.txblockTrainingProfile = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6:
                {
                    this.txblockSpecialty = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 7:
                {
                    this.txblockChair = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.txblockDepartment = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9:
                {
                    this.txblockGroup = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.txblock_EMail = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11:
                {
                    this.fltEdit = (global::Windows.UI.Xaml.Controls.Flyout)(target);
                    #line 39 "..\..\..\ProfilePage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Flyout)this.fltEdit).Closed += this.fltEdit_Closed;
                    #line default
                }
                break;
            case 12:
                {
                    this.txbLasName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 13:
                {
                    this.txbName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 14:
                {
                    this.txblck_Error = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 15:
                {
                    this.btn_CancelChangeName = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 78 "..\..\..\ProfilePage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_CancelChangeName).Tapped += this.btnCancel_Tapped;
                    #line default
                }
                break;
            case 16:
                {
                    this.btn_SaveChangeName = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 87 "..\..\..\ProfilePage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_SaveChangeName).Tapped += this.btnsave_Tapped;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

