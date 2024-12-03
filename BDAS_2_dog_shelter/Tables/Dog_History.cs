﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS_2_dog_shelter.Tables
{
    public class Dog_History : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string date_of_event;
        public string DateOfEvent
        {
            get => date_of_event;
            set
            {
                if (date_of_event != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DateOfEvent)));
                    date_of_event = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfEvent)));
                }
            }
        }
        private string event_description;
        public string EventDescription
        {
            get => event_description;
            set
            {
                if (event_description != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(EventDescription)));
                    event_description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EventDescription)));
                }
            }
        }
        public Dog_History() { date_of_event = ""; event_description = ""; }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}