﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAP.Utilities.ConcurrentObservableCollections
{
  public class ExtendedPropertyChangedEventArgs : PropertyChangedEventArgs
  {
    public ExtendedPropertyChangedEventArgs(string propertyName, object oldValue, object newValue) : base(propertyName)
    {
      OldValue = oldValue;
      NewValue = newValue;
    }

    public object OldValue { get; }
    public object NewValue { get; }
  }
}
