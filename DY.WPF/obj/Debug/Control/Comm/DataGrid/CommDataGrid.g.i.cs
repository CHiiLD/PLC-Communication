﻿#pragma checksum "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "81F4144FE550F692EA93C99002ED9994"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.34209
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


namespace DY.WPF {
    
    
    /// <summary>
    /// CommDataGrid
    /// </summary>
    public partial class CommDataGrid : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 8 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DY.WPF.CommDataGrid @this;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid NDataGrid;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem NMI_Add;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem NMI_Connect;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem NMI_Disconnect;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem NMI_Remove;
        
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
            System.Uri resourceLocater = new System.Uri("/DY.WPF;component/control/comm/datagrid/commdatagrid.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
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
            this.@this = ((DY.WPF.CommDataGrid)(target));
            return;
            case 2:
            this.NDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 26 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
            this.NDataGrid.SelectedCellsChanged += new System.Windows.Controls.SelectedCellsChangedEventHandler(this.NDataGrid_SelectedCellsChanged);
            
            #line default
            #line hidden
            
            #line 27 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
            this.NDataGrid.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.NDataGrid_MouseRightButtonUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.NMI_Add = ((System.Windows.Controls.MenuItem)(target));
            
            #line 31 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
            this.NMI_Add.Click += new System.Windows.RoutedEventHandler(this.NMI_Add_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.NMI_Connect = ((System.Windows.Controls.MenuItem)(target));
            
            #line 32 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
            this.NMI_Connect.Click += new System.Windows.RoutedEventHandler(this.NMI_Connect_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.NMI_Disconnect = ((System.Windows.Controls.MenuItem)(target));
            
            #line 33 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
            this.NMI_Disconnect.Click += new System.Windows.RoutedEventHandler(this.NMI_Disconnect_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.NMI_Remove = ((System.Windows.Controls.MenuItem)(target));
            
            #line 34 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
            this.NMI_Remove.Click += new System.Windows.RoutedEventHandler(this.NMI_Remove_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 7:
            
            #line 64 "..\..\..\..\..\Control\Comm\DataGrid\CommDataGrid.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.NCB_CheckBox_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

