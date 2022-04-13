﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F88BD299B3CBDE07BAEDEABF48328E4114301ADC5A03776A1A5DF85AC6801EA0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AeroUI;
using Microsoft.Maps.MapControl.WPF;
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


namespace AeroUI {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 201 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid BG;
        
        #line default
        #line hidden
        
        
        #line 203 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ImageBrush img_bg;
        
        #line default
        #line hidden
        
        
        #line 209 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel PnTakeOff;
        
        #line default
        #line hidden
        
        
        #line 214 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_Lat;
        
        #line default
        #line hidden
        
        
        #line 215 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_Long;
        
        #line default
        #line hidden
        
        
        #line 216 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_Speed;
        
        #line default
        #line hidden
        
        
        #line 217 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblLat;
        
        #line default
        #line hidden
        
        
        #line 218 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblLong;
        
        #line default
        #line hidden
        
        
        #line 219 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblSpeed;
        
        #line default
        #line hidden
        
        
        #line 220 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_Alt;
        
        #line default
        #line hidden
        
        
        #line 221 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblAlt;
        
        #line default
        #line hidden
        
        
        #line 225 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblDistanceToTarget;
        
        #line default
        #line hidden
        
        
        #line 230 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Maps.MapControl.WPF.Map AeroMap;
        
        #line default
        #line hidden
        
        
        #line 231 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Maps.MapControl.WPF.Pushpin aircraft_pin;
        
        #line default
        #line hidden
        
        
        #line 232 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Maps.MapControl.WPF.MapPolyline lineFromAircraftToTarget;
        
        #line default
        #line hidden
        
        
        #line 233 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Maps.MapControl.WPF.MapPolyline aircraftRoute;
        
        #line default
        #line hidden
        
        
        #line 242 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel PnFlightData;
        
        #line default
        #line hidden
        
        
        #line 243 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PanelFlightData;
        
        #line default
        #line hidden
        
        
        #line 246 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button searchFlightFileButton;
        
        #line default
        #line hidden
        
        
        #line 249 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider PlayBackSlider;
        
        #line default
        #line hidden
        
        
        #line 258 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel PnSettings;
        
        #line default
        #line hidden
        
        
        #line 260 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridSettings;
        
        #line default
        #line hidden
        
        
        #line 311 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LatitudeTextBox;
        
        #line default
        #line hidden
        
        
        #line 333 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LongitudeTextBox;
        
        #line default
        #line hidden
        
        
        #line 345 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSetTarget;
        
        #line default
        #line hidden
        
        
        #line 371 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CmbPorts;
        
        #line default
        #line hidden
        
        
        #line 383 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnBuscar;
        
        #line default
        #line hidden
        
        
        #line 399 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnConexion;
        
        #line default
        #line hidden
        
        
        #line 421 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Header;
        
        #line default
        #line hidden
        
        
        #line 447 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtConnection;
        
        #line default
        #line hidden
        
        
        #line 455 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgConnectionStatus;
        
        #line default
        #line hidden
        
        
        #line 459 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRecord;
        
        #line default
        #line hidden
        
        
        #line 476 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblRecTime;
        
        #line default
        #line hidden
        
        
        #line 485 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnMinimizar;
        
        #line default
        #line hidden
        
        
        #line 505 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid nav_pnl;
        
        #line default
        #line hidden
        
        
        #line 511 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel st_pnl;
        
        #line default
        #line hidden
        
        
        #line 541 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton Tg_Btn;
        
        #line default
        #line hidden
        
        
        #line 560 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Animation.Storyboard HideStackPanel;
        
        #line default
        #line hidden
        
        
        #line 574 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Animation.Storyboard ShowStackPanel;
        
        #line default
        #line hidden
        
        
        #line 593 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView LV;
        
        #line default
        #line hidden
        
        
        #line 598 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListViewItem LvTakeOff;
        
        #line default
        #line hidden
        
        
        #line 614 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ToolTip tt_take_off;
        
        #line default
        #line hidden
        
        
        #line 622 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListViewItem LvFlightData;
        
        #line default
        #line hidden
        
        
        #line 638 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ToolTip tt_Flight_Data;
        
        #line default
        #line hidden
        
        
        #line 646 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListViewItem LvSettings;
        
        #line default
        #line hidden
        
        
        #line 662 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ToolTip tt_settings;
        
        #line default
        #line hidden
        
        
        #line 670 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListViewItem LvSettings1;
        
        #line default
        #line hidden
        
        
        #line 686 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ToolTip tt_exit;
        
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
            System.Uri resourceLocater = new System.Uri("/AeroUI;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
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
            this.BG = ((System.Windows.Controls.Grid)(target));
            
            #line 201 "..\..\MainWindow.xaml"
            this.BG.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BG_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.img_bg = ((System.Windows.Media.ImageBrush)(target));
            return;
            case 3:
            this.PnTakeOff = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.lbl_Lat = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.lbl_Long = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.lbl_Speed = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.lblLat = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.lblLong = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.lblSpeed = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.lbl_Alt = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.lblAlt = ((System.Windows.Controls.Label)(target));
            return;
            case 12:
            this.lblDistanceToTarget = ((System.Windows.Controls.Label)(target));
            return;
            case 13:
            this.AeroMap = ((Microsoft.Maps.MapControl.WPF.Map)(target));
            return;
            case 14:
            this.aircraft_pin = ((Microsoft.Maps.MapControl.WPF.Pushpin)(target));
            return;
            case 15:
            this.lineFromAircraftToTarget = ((Microsoft.Maps.MapControl.WPF.MapPolyline)(target));
            return;
            case 16:
            this.aircraftRoute = ((Microsoft.Maps.MapControl.WPF.MapPolyline)(target));
            return;
            case 17:
            this.PnFlightData = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 18:
            this.PanelFlightData = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 19:
            this.searchFlightFileButton = ((System.Windows.Controls.Button)(target));
            
            #line 246 "..\..\MainWindow.xaml"
            this.searchFlightFileButton.Click += new System.Windows.RoutedEventHandler(this.searchFlightFile);
            
            #line default
            #line hidden
            return;
            case 20:
            this.PlayBackSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 251 "..\..\MainWindow.xaml"
            this.PlayBackSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.showPlayBackData);
            
            #line default
            #line hidden
            return;
            case 21:
            this.PnSettings = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 22:
            this.GridSettings = ((System.Windows.Controls.Grid)(target));
            return;
            case 23:
            this.LatitudeTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 24:
            this.LongitudeTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 25:
            this.btnSetTarget = ((System.Windows.Controls.Button)(target));
            
            #line 352 "..\..\MainWindow.xaml"
            this.btnSetTarget.Click += new System.Windows.RoutedEventHandler(this.btnSetTarget_Click);
            
            #line default
            #line hidden
            return;
            case 26:
            this.CmbPorts = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 27:
            this.BtnBuscar = ((System.Windows.Controls.Button)(target));
            
            #line 390 "..\..\MainWindow.xaml"
            this.BtnBuscar.Click += new System.Windows.RoutedEventHandler(this.BtnBuscar_Click);
            
            #line default
            #line hidden
            return;
            case 28:
            this.BtnConexion = ((System.Windows.Controls.Button)(target));
            
            #line 407 "..\..\MainWindow.xaml"
            this.BtnConexion.Click += new System.Windows.RoutedEventHandler(this.BtnConexion_Click);
            
            #line default
            #line hidden
            return;
            case 29:
            this.Header = ((System.Windows.Controls.Grid)(target));
            return;
            case 30:
            this.txtConnection = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 31:
            this.imgConnectionStatus = ((System.Windows.Controls.Image)(target));
            return;
            case 32:
            this.btnRecord = ((System.Windows.Controls.Button)(target));
            
            #line 464 "..\..\MainWindow.xaml"
            this.btnRecord.Click += new System.Windows.RoutedEventHandler(this.startRecording);
            
            #line default
            #line hidden
            return;
            case 33:
            this.lblRecTime = ((System.Windows.Controls.Label)(target));
            return;
            case 34:
            this.BtnMinimizar = ((System.Windows.Controls.Button)(target));
            
            #line 492 "..\..\MainWindow.xaml"
            this.BtnMinimizar.Click += new System.Windows.RoutedEventHandler(this.BtnMinimizar_Click);
            
            #line default
            #line hidden
            return;
            case 35:
            this.nav_pnl = ((System.Windows.Controls.Grid)(target));
            return;
            case 36:
            this.st_pnl = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 37:
            this.Tg_Btn = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 549 "..\..\MainWindow.xaml"
            this.Tg_Btn.Unchecked += new System.Windows.RoutedEventHandler(this.Tg_Btn_Unchecked);
            
            #line default
            #line hidden
            
            #line 549 "..\..\MainWindow.xaml"
            this.Tg_Btn.Checked += new System.Windows.RoutedEventHandler(this.Tg_Btn_Checked);
            
            #line default
            #line hidden
            return;
            case 38:
            this.HideStackPanel = ((System.Windows.Media.Animation.Storyboard)(target));
            return;
            case 39:
            this.ShowStackPanel = ((System.Windows.Media.Animation.Storyboard)(target));
            return;
            case 40:
            this.LV = ((System.Windows.Controls.ListView)(target));
            return;
            case 41:
            this.LvTakeOff = ((System.Windows.Controls.ListViewItem)(target));
            
            #line 599 "..\..\MainWindow.xaml"
            this.LvTakeOff.MouseEnter += new System.Windows.Input.MouseEventHandler(this.ListViewItem_MouseEnter);
            
            #line default
            #line hidden
            
            #line 599 "..\..\MainWindow.xaml"
            this.LvTakeOff.Selected += new System.Windows.RoutedEventHandler(this.LvTakeOff_Selected);
            
            #line default
            #line hidden
            return;
            case 42:
            this.tt_take_off = ((System.Windows.Controls.ToolTip)(target));
            return;
            case 43:
            this.LvFlightData = ((System.Windows.Controls.ListViewItem)(target));
            
            #line 623 "..\..\MainWindow.xaml"
            this.LvFlightData.MouseEnter += new System.Windows.Input.MouseEventHandler(this.ListViewItem_MouseEnter);
            
            #line default
            #line hidden
            
            #line 623 "..\..\MainWindow.xaml"
            this.LvFlightData.Selected += new System.Windows.RoutedEventHandler(this.LvFlightData_Selected);
            
            #line default
            #line hidden
            return;
            case 44:
            this.tt_Flight_Data = ((System.Windows.Controls.ToolTip)(target));
            return;
            case 45:
            this.LvSettings = ((System.Windows.Controls.ListViewItem)(target));
            
            #line 647 "..\..\MainWindow.xaml"
            this.LvSettings.MouseEnter += new System.Windows.Input.MouseEventHandler(this.ListViewItem_MouseEnter);
            
            #line default
            #line hidden
            
            #line 647 "..\..\MainWindow.xaml"
            this.LvSettings.Selected += new System.Windows.RoutedEventHandler(this.LvSettings_Selected);
            
            #line default
            #line hidden
            return;
            case 46:
            this.tt_settings = ((System.Windows.Controls.ToolTip)(target));
            return;
            case 47:
            this.LvSettings1 = ((System.Windows.Controls.ListViewItem)(target));
            
            #line 671 "..\..\MainWindow.xaml"
            this.LvSettings1.MouseEnter += new System.Windows.Input.MouseEventHandler(this.ListViewItem_MouseEnter);
            
            #line default
            #line hidden
            
            #line 671 "..\..\MainWindow.xaml"
            this.LvSettings1.Selected += new System.Windows.RoutedEventHandler(this.LvSettings1_Selected);
            
            #line default
            #line hidden
            return;
            case 48:
            this.tt_exit = ((System.Windows.Controls.ToolTip)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

