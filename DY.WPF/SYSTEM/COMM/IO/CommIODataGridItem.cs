﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using DY.NET;

namespace DY.WPF.SYSTEM.IO
{
    /// <summary>
    /// PLC IO 입출력을 위한 컬렉션 아이템
    /// </summary>
    public class CommIODataGridItem : INotifyPropertyChanged, ICommIOData
    {
#if false
        public CommIODataGridItem(CommIODataGridItem that)
        {
            m_Type = that.m_Type;
            m_Address = that.m_Address;
            m_Input = that.m_Input;
            m_Output = that.m_Output;
            m_Comment = that.m_Comment;
        }
#endif
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private DataType m_Type;
        private string m_Address;
        private object m_Input;
        private object m_Output;
        private string m_Comment;

        public DataType Type { get { return m_Type; } set { m_Type = value; OnPropertyChanged("Type"); } }
        public string Address { get { return m_Address; } set { m_Address = value; OnPropertyChanged("Address"); } }
        public object Input { get { return m_Input; } set { m_Input = value; OnPropertyChanged("Input"); } }
        public object Output { get { return m_Output; } set { m_Output = value; OnPropertyChanged("Output"); } }
        public string Comment { get { return m_Comment; } set { m_Comment = value; OnPropertyChanged("Comment"); } }

        public void SetValue(object value)
        {
            Output = value;
        }

        public string GetAddress()
        {
            return Address;
        }

        public DataType GetDataType()
        {
            return Type;
        }
    }
}