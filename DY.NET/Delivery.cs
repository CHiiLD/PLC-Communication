﻿using System.Diagnostics;

namespace DY.NET
{
    public class Delivery
    {
        public DeliveryError Error { get; set; }
        public object Package { get; set; }
        public Stopwatch DelivaryTime { get; private set; }

        public Delivery()
        {
            Error = DeliveryError.SUCCESS;
            DelivaryTime = Stopwatch.StartNew();
        }

        public Delivery Packing()
        {
            DelivaryTime.Stop();
            return this;
        }
    }
}
