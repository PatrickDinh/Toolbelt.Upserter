using System;

namespace Toolbelt.Upserter
{
    public class Upserter<T> : Upserter<T, int>
    {
        public Upserter(Func<T, int> getIdentifier,
                        Func<T, T, bool> ifRowNeedUpdate,
                        Func<T[], int> adder,
                        Func<T[], int> updater,
                        Func<T[], int> deleter)
            : base(getIdentifier, ifRowNeedUpdate, adder, updater, deleter)
        {
            
        }
    }
}