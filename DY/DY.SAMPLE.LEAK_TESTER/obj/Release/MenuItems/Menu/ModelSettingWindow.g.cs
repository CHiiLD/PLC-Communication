﻿#pragma checksum "..\..\..\..\MenuItems\Menu\ModelSettingWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3EF77373D3587BFD18E5D72C98A5A53A"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.34014
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace DY.SAMPLE.LEAK_TESTER {
    
    
    /// <summary>
    /// ModelSettingWindow
    /// </summary>
    public partial class ModelSettingWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\MenuItems\Menu\ModelSettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox NModelNum;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\MenuItems\Menu\ModelSettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NModel;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\MenuItems\Menu\ModelSettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NCustomer;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\MenuItems\Menu\ModelSettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NProdectInfo;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\MenuItems\Menu\ModelSettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NProductNum;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\MenuItems\Menu\ModelSettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NLabelID;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\MenuItems\Menu\ModelSettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NLHSerialNum;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\MenuItems\Menu\ModelSettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NRHSerialNum;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DY.SAMPLE.LEAK_TESTER;component/menuitems/menu/modelsettingwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\MenuItems\Menu\ModelSettingWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.NModelNum = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.NModel = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.NCustomer = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.NProdectInfo = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.NProductNum = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.NLabelID = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.NLHSerialNum = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.NRHSerialNum = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
