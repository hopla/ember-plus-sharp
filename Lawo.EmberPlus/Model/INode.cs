﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// <copyright>Copyright 2012-2015 Lawo AG (http://www.lawo.com). All rights reserved.</copyright>
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Lawo.EmberPlus.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>Represents a node in the object tree accessible through
    /// <see cref="Consumer{T}.Root">Consumer&lt;TRoot&gt;.Root</see>.</summary>
    public interface INode : IElementWithSchemas
    {
        /// <summary>Gets a value indicating whether this is a root node.</summary>
        bool IsRoot { get; }

        /// <summary>Gets the children of this node.</summary>
        ReadOnlyObservableCollection<IElement> Children { get; }

        /// <summary>Gets the child where <see cref="IElement.Number"/> equals <paramref name="number"/>.</summary>
        /// <exception cref="KeyNotFoundException">No child exists where <see cref="IElement.Number"/> equals
        /// <paramref name="number"/>.</exception>
        IElement this[int number] { get; }

        /// <summary>Gets the element with the path <paramref name="path"/> relative to this node.</summary>
        /// <param name="path">The relative path to the desired element.</param>
        /// <returns>The element with the given path, if such an element exists; otherwise, <c>null</c>.</returns>
        IElement GetElement(string path);
    }
}