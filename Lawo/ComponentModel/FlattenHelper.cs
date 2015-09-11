﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// <copyright>Copyright 2012-2015 Lawo AG (http://www.lawo.com). All rights reserved.</copyright>
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Lawo.ComponentModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>Provides methods to flatten a <see cref="ReadOnlyObservableCollection{T}"/> containing
    /// <see cref="ReadOnlyObservableCollection{T}"/> instances into one <see cref="ReadOnlyObservableCollection{T}"/>.
    /// </summary>
    /// <threadsafety static="true" instance="false"/>
    public static class FlattenHelper
    {
        /// <summary>Returns a collection containing all the elements in the inner collection.</summary>
        /// <typeparam name="T">The type of the elements in the inner collection.</typeparam>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposable object is returned to the caller.")]
        public static DisposableReadOnlyObservableCollection<T> Flatten<T>(
            this ReadOnlyObservableCollection<ReadOnlyObservableCollection<T>> original)
        {
            return new DisposableReadOnlyObservableCollection<T>(new FlattenCollection<T>(original));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private sealed class FlattenCollection<T> : SubscribedObservableCollection<T>
        {
            private readonly ReadOnlyObservableCollection<ReadOnlyObservableCollection<T>> original;
            private readonly List<Action> unsubscribeCallbacks = new List<Action>();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            internal FlattenCollection(ReadOnlyObservableCollection<ReadOnlyObservableCollection<T>> original)
            {
                if (original == null)
                {
                    throw new ArgumentNullException("original");
                }

                this.original = original;
                var handler = this.original.AddChangeHandlers<ReadOnlyObservableCollection<ReadOnlyObservableCollection<T>>, ReadOnlyObservableCollection<T>>(
                    this.AddedToOuter, this.RemovedFromOuter, this.ClearedOuter);
                this.RegisterForRemoval(original, handler);
            }

            internal sealed override void Dispose(bool disposing)
            {
                try
                {
                    foreach (var unsubscribeCallback in this.unsubscribeCallbacks)
                    {
                        unsubscribeCallback();
                    }
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            private void AddedToOuter(int outerIndex, ReadOnlyObservableCollection<T> inner)
            {
                var handler = inner == null ? (NotifyCollectionChangedEventHandler)null :
                    inner.AddChangeHandlers<ReadOnlyObservableCollection<T>, T>(
                        (index, item) => this.AddedToInner(inner, index, item),
                        (index, item) => this.RemovedFromInner(inner, index),
                        () => this.ClearedInner(inner));
                this.unsubscribeCallbacks.Insert(outerIndex, () => Unsubscribe(inner, handler));
            }

            private void RemovedFromOuter(int outerIndex, ReadOnlyObservableCollection<T> inner)
            {
                var unsubscribeCallback = this.unsubscribeCallbacks[outerIndex];
                this.unsubscribeCallbacks.RemoveAt(outerIndex);
                unsubscribeCallback();

                var startIndex = GetTotalCount(this.original.Take(outerIndex));
                var pastEndIndex = startIndex + (inner == null ? 0 : inner.Count);

                for (var index = startIndex; index < pastEndIndex; ++index)
                {
                    this.RemoveAt(startIndex);
                }
            }

            private void ClearedOuter()
            {
                try
                {
                    foreach (var unsubscribeCallback in this.unsubscribeCallbacks)
                    {
                        unsubscribeCallback();
                    }
                }
                finally
                {
                    this.unsubscribeCallbacks.Clear();
                    this.Clear();
                }
            }

            private void AddedToInner(ReadOnlyObservableCollection<T> inner, int index, T item)
            {
                this.Insert(GetBeforeCount(inner) + index, item);
            }

            private void RemovedFromInner(ReadOnlyObservableCollection<T> inner, int index)
            {
                this.RemoveAt(GetBeforeCount(inner) + index);
            }

            private void ClearedInner(ReadOnlyObservableCollection<T> inner)
            {
                var startIndex = GetBeforeCount(inner);
                var pastEndIndex = this.Count - GetAfterCount(inner);

                for (var index = startIndex; index < pastEndIndex; ++index)
                {
                    this.RemoveAt(startIndex);
                }
            }

            private int GetBeforeCount(ReadOnlyObservableCollection<T> inner)
            {
                return GetTotalCount(this.original.Take(this.original.IndexOf(inner)));
            }

            private int GetAfterCount(ReadOnlyObservableCollection<T> inner)
            {
                return GetTotalCount(this.original.Skip(this.original.IndexOf(inner) + 1));
            }

            private static int GetTotalCount(IEnumerable<ReadOnlyObservableCollection<T>> inners)
            {
                return inners.Aggregate(0, (c, i) => c + (i == null ? 0 : i.Count));
            }

            private static void Unsubscribe(INotifyCollectionChanged inner, NotifyCollectionChangedEventHandler handler)
            {
                if (inner != null)
                {
                    inner.CollectionChanged -= handler;
                }
            }
        }
    }
}
