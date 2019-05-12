﻿using System;

namespace SmartMarket.Controls.InputKit.Abstraction
{
    public interface IValidatable
    {
        bool IsRequired { get; set; }
        bool IsValidated { get; }
        string ValidationMessage { get; set; }
        void DisplayValidation();
        event EventHandler ValidationChanged;
    }
}
