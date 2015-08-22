﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

using MahApps.Metro.Controls;
using OxyPlot;
using OxyPlot.Wpf;
using NLog;

namespace DY.WPF
{
    public partial class COxyRectangleGraphType01 : UserControl, ICOxyPlotRange, ICOxyRectangleRange
    {
        private COxyBottonAxisType m_BottonAxisType = COxyBottonAxisType.LINEAR_AXIS;

        public static readonly DependencyProperty PlotMaximumXProperty =
                    DependencyProperty.Register("PlotMaximumX",
                    typeof(double),
                    typeof(COxyRectangleGraphType01),
                    new PropertyMetadata(100.0));
        public static readonly DependencyProperty PlotMaximumYProperty =
                    DependencyProperty.Register("PlotMaximumY",
                    typeof(double),
                    typeof(COxyRectangleGraphType01),
                    new PropertyMetadata(100.0));
        public static readonly DependencyProperty PlotMinimumXProperty =
                    DependencyProperty.Register("PlotMinimumX",
                    typeof(double),
                    typeof(COxyRectangleGraphType01),
                    new PropertyMetadata(0.0));
        public static readonly DependencyProperty PlotMinimumYProperty =
                    DependencyProperty.Register("PlotMinimumY",
                    typeof(double),
                    typeof(COxyRectangleGraphType01),
                    new PropertyMetadata(0.0));

        public static readonly DependencyProperty RectMinimunXProperty =
                    DependencyProperty.Register("RectMinimunX",
                    typeof(double),
                    typeof(COxyRectangleGraphType01),
                    new PropertyMetadata(20.0));
        public static readonly DependencyProperty RectMinimunYProperty =
                    DependencyProperty.Register("RectMinimunY",
                    typeof(double),
                    typeof(COxyRectangleGraphType01),
                    new PropertyMetadata(20.0));
        public static readonly DependencyProperty RectMaximunXProperty =
                    DependencyProperty.Register("RectMaximunX",
                    typeof(double),
                    typeof(COxyRectangleGraphType01),
                    new PropertyMetadata(80.0));
        public static readonly DependencyProperty RectMaximunYProperty =
                    DependencyProperty.Register("RectMaximunY",
                    typeof(double),
                    typeof(COxyRectangleGraphType01),
                    new PropertyMetadata(80.0));

        public double PlotMaximumX
        {
            get { return (double)GetValue(PlotMaximumXProperty); }
            set { SetValue(PlotMaximumXProperty, value); }
        }

        public double PlotMaximumY
        {
            get { return (double)GetValue(PlotMaximumYProperty); }
            set { SetValue(PlotMaximumYProperty, value); }
        }

        public double PlotMinimumX
        {
            get { return (double)GetValue(PlotMinimumXProperty); }
            set { SetValue(PlotMinimumXProperty, value); }
        }

        public double PlotMinimumY
        {
            get { return (double)GetValue(PlotMinimumYProperty); }
            set { SetValue(PlotMinimumYProperty, value); }
        }

        public double RectMinimunX
        {
            get { return (double)GetValue(RectMinimunXProperty); }
            set { SetValue(RectMinimunXProperty, value); }
        }

        public double RectMinimunY
        {
            get { return (double)GetValue(RectMinimunYProperty); }
            set { SetValue(RectMinimunYProperty, value); }
        }

        public double RectMaximunX
        {
            get { return (double)GetValue(RectMaximunXProperty); }
            set { SetValue(RectMaximunXProperty, value); }
        }

        public double RectMaximunY
        {
            get { return (double)GetValue(RectMaximunYProperty); }
            set { SetValue(RectMaximunYProperty, value); }
        }

        public COxyBottonAxisType BottonAxisType
        {
            get
            {
                return m_BottonAxisType;
            }
            set
            {
                m_BottonAxisType = value;
                RemoveBottonAxis();
                AddBottonAxis(value);
            }
        }

        public Plot Plot
        {
            get
            {
                return NPlot;
            }
        }

        public Axis AxisBotton
        {
            get;
            private set;
        }

        public Axis AxisLeft
        {
            get
            {
                return NLinearAxisY;
            }
        }

        public Annotation Annotation
        {
            get
            {
                return NRectangle;
            }
        }

        public RangeSlider SliderX
        {
            get
            {
                return NRangeX;
            }
        }

        public RangeSlider SliderY
        {
            get
            {
                return NRangeY;
            }
        }

        public Collection<Series> Series
        {
            get
            {
                return NPlot.Series;
            }
        }

        public COxyRectangleGraphType01()
        {
            InitializeComponent();
            AddBottonAxis(COxyBottonAxisType.LINEAR_AXIS);
        }

        public COxyRectangleGraphType01(COxyBottonAxisType bottonAxesType)
        {
            InitializeComponent();
            BottonAxisType = bottonAxesType;
            AddBottonAxis(bottonAxesType);
        }

        /// <summary>
        /// BottonAxisType 타입에 맞추어 LineSeries을 생성하여 반환한다.
        /// </summary>
        /// <returns>LineSeries</returns>
        public LineSeries CreateLineSeries()
        {
            LineSeries series = null;
            switch (BottonAxisType)
            {
                case COxyBottonAxisType.LINEAR_AXIS:
                    series = new LineSeries()
                    {
                        ItemsSource = new Collection<COxyDateValue>(),
                        DataFieldX = "Date",
                        DataFieldY = "Value"
                    };
                    break;
                case COxyBottonAxisType.TIMESPAN_AXIS:
                    series = new LineSeries()
                    {
                        ItemsSource = new Collection<DataPoint>(),
                        DataFieldX = "X",
                        DataFieldY = "Y"
                    };
                    break;
            }
            return series;
        }

        /// <summary>
        /// Plot의 Series 콜렉션에 새로운 series를 추가한다.
        /// </summary>
        /// <param name="series"></param>
        public void AddSeries(Series series)
        {
            if (series != null)
                Series.Add(series);
        }

        /// <summary>
        /// Plot에 그려진 LineSeries를 삭제한다.
        /// </summary>
        public void ClearPlot()
        {
            foreach(var s in Series)
                s.Items.Clear();
        }

        private void RemoveBottonAxis()
        {
            Collection<Axis> items = NPlot.Axes;
            Axis target = null;
            foreach (var i in items)
            {
                if (i.Position == OxyPlot.Axes.AxisPosition.Bottom)
                {
                    target = i;
                    break;
                }
            }
            if (target != null)
                items.Remove(target);
        }

        private void AddBottonAxis(COxyBottonAxisType type)
        {
            Axis axis = null;
            switch (type)
            {
                case COxyBottonAxisType.LINEAR_AXIS:
                    axis = new LinearAxis()
                    {
                        Position = OxyPlot.Axes.AxisPosition.Bottom,
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot
                    };
                    break;
                case COxyBottonAxisType.TIMESPAN_AXIS:
                    axis = new TimeSpanAxis()
                    {
                        Position = OxyPlot.Axes.AxisPosition.Bottom,
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot
                    };
                    break;
            }
            axis.SetBinding(Axis.MaximumProperty, new Binding("PlotMaximumX") { Source = this, Mode = BindingMode.TwoWay });
            axis.SetBinding(Axis.MinimumProperty, new Binding("PlotMinimumX") { Source = this, Mode = BindingMode.TwoWay });
            AxisBotton = axis;
            NPlot.Axes.Add(axis);
        }

        private void NRangeX_UpperValueChanged(object sender, RangeParameterChangedEventArgs e)
        {
            RectMinimunX = e.NewValue;
        }

        private void NRangeX_LowerValueChanged(object sender, RangeParameterChangedEventArgs e)
        {
            RectMaximunX = e.NewValue;
        }

        private void NRangeY_UpperValueChanged(object sender, RangeParameterChangedEventArgs e)
        {
            RectMaximunY = e.NewValue;
        }

        private void NRangeY_LowerValueChanged(object sender, RangeParameterChangedEventArgs e)
        {
            RectMinimunY = e.NewValue;
        }
    }
}