﻿

#pragma checksum "D:\Aly\Downloads\SampleCode\MonAssoce\Template-MonAsso\Template MonAsso\MonAssoce\Views\SearchPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E177846A68EC2EE564381867626D0F6F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonAssoce.Views
{
    partial class SearchPage : global::MonAssoce.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 149 "..\..\Views\SearchPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.Filter_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 123 "..\..\Views\SearchPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 105 "..\..\Views\SearchPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.Filter_Checked;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 70 "..\..\Views\SearchPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


