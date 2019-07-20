﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models.API
{
    public class BuyItem
    {
        public string WalletAddress { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public double[] Times
        {
            get => new double[] { 0, 60 };
        }
        public double[] Values
        {
            get => new double[] { Price / 2, Price / 2 };
        }
    }
}
