using System;
using System.Linq;

namespace Toolbelt.Upserter
{
    public class Upserter<T, TIdentifier> 
    {
        private readonly Func<T, T, bool> _defaultIfRowNeedsUpdate = (arg1, arg2) => true;
        private readonly Func<T[], int> _defaultFunc = arg => 0;
        private readonly Func<T, T, bool> _ifRowNeedsUpdate;
        private readonly Func<T, TIdentifier> _getIdentifier;
        private readonly Func<T[], int> _adder;
        private readonly Func<T[], int> _updater;
        private readonly Func<T[], int> _deleter;
        
        public Upserter(Func<T, TIdentifier> getIdentifier,
                        Func<T, T, bool> ifRowNeedUpdate,
                        Func<T[], int> adder,
                        Func<T[], int> updater,
                        Func<T[], int> deleter)
        {
            _getIdentifier = getIdentifier;
            _ifRowNeedsUpdate = ifRowNeedUpdate ?? _defaultIfRowNeedsUpdate;
            _adder = adder ?? _defaultFunc;
            _updater = updater ?? _defaultFunc;
            _deleter = deleter ?? _defaultFunc;
        }

        public UpsertResult Run(T[] existingRows, T[] insertingRows)
        {
            var existingRowsDictionary = existingRows.ToDictionary(r => _getIdentifier(r));
            var insertingRowsDictionary = insertingRows.ToDictionary(r => _getIdentifier(r));

            var newRows = insertingRows.Where(ir => !existingRowsDictionary.ContainsKey(_getIdentifier(ir))).ToArray();
            var deletedRows = existingRows.Where(er => !insertingRowsDictionary.ContainsKey(_getIdentifier(er))).ToArray();
            var updateRows = insertingRows.Where(ir =>
                                                 {
                                                     if (!existingRowsDictionary.ContainsKey(_getIdentifier(ir)))
                                                         return false;

                                                     var existingRow = existingRowsDictionary[_getIdentifier(ir)];
                                                     return _ifRowNeedsUpdate(existingRow, ir);
                                                 }).ToArray();

            var added = _adder(newRows);
            var updated = _updater(updateRows);
            var deleted = _deleter(deletedRows);

            return new UpsertResult(added, updated, deleted);
        }
    }
}