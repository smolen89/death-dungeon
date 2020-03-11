﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RD.Util
{
    internal static class DisposableExtension
    {
        #region Types

        private sealed class DisposableGroup : IDisposable
        {
            #region Fields

            private readonly IDisposable[] disposables;

            #endregion

            #region Initialisation

            public DisposableGroup(IEnumerable<IDisposable> disposables)
            {
                this.disposables = GetDisposables(disposables).ToArray();
            }

            #endregion

            #region Methods

            private IEnumerable<IDisposable> GetDisposables(IEnumerable<IDisposable> disposables)
            {
                foreach (IDisposable disposable in disposables)
                {
                    if (disposable is DisposableGroup group)
                    {
                        foreach (IDisposable child in group.disposables)
                        {
                            yield return child;
                        }
                    }
                    else
                    {
                        yield return disposable;
                    }
                }
            }

            #endregion

            #region IDisposable

            void IDisposable.Dispose()
            {
                for (int i = disposables.Length - 1; i >= 0; --i)
                {
                    disposables[i]?.Dispose();
                }

                // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
                GC.SuppressFinalize(this);
            }

            #endregion
        }

        #endregion

        #region Methods

        public static IDisposable Merge(this IEnumerable<IDisposable> disposables) => new DisposableGroup(disposables ?? Enumerable.Empty<IDisposable>());

        #endregion
    }
}
