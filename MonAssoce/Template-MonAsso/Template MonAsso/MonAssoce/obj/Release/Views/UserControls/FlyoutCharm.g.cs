﻿

#pragma checksum "D:\Aly\Downloads\SampleCode\MonAssoce\Template-MonAsso\Template MonAsso\MonAssoce\Views\UserControls\FlyoutCharm.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "836F53E378862862AC44659EF26239DF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonAssoce.Views.UserControls
{
    partial class FlyoutCharm : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 111 "..\..\..\Views\UserControls\FlyoutCharm.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Popup)(target)).Closed += this.OnPopupClosed;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 132 "..\..\..\Views\UserControls\FlyoutCharm.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.BackButton_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


