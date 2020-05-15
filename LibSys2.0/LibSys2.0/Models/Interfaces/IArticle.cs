using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    /// <summary>
    /// Used to wrap stuff into the same <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>
    /// </summary>
    public abstract class IArticle
    #region empty
    {
        public bool IsEbook { get; set; }
    } 
    #endregion
}
