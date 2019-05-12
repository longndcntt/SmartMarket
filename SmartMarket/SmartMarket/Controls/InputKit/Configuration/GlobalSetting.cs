﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SmartMarket.Controls.InputKit.Configuration
{
    /// <summary>
    /// To be added
    /// </summary>
    public class GlobalSetting : INotifyPropertyChanged
    {
        ///------------------------------------------------------------------
        /// <summary>
        /// Main color of control
        /// </summary>
        public Color Color { get; set; }
        ///------------------------------------------------------------------
        /// <summary>
        /// Background color of control
        /// </summary>
        public Color BackgroundColor { get; set; }
        ///------------------------------------------------------------------
        /// <summary>
        /// Border color of control
        /// </summary>
        public Color BorderColor { get; set; }
        ///------------------------------------------------------------------
        /// <summary>
        /// If control has a corner radius, this is it.
        /// </summary>
        public float CornerRadius { get; set; }
        ///------------------------------------------------------------------
        /// <summary>
        /// If control has fontsize, this is it.
        /// </summary>
        public double FontSize { get; set; }
        ///------------------------------------------------------------------
        /// <summary>
        /// Size of control. ( Like HeightRequest and WidthRequest )
        /// </summary>
        public double Size { get; set; }
        ///------------------------------------------------------------------
        /// <summary>
        /// Text Color of control.
        /// </summary>
        public Color TextColor { get; set; }
        ///------------------------------------------------------------------
        /// <summary>
        /// Font family of control.
        /// </summary>
        public string FontFamily { get; set; }

        ///------------------------------------------------------------------
        /// <summary>
        /// INotifyPropertyChanged Implementation
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
