using System;
using System.Collections.Generic;
using System.Linq;

namespace Toolbelt.Upserter
{
    public class Upserter<T, TIdentifier> 
    {
        private readonly Func<T, T, bool> _defaultIfRowNeedsUpdate = (arg1, arg2) => true;
        private readonly Func<T[], int> _defaultFunc = arg => 0;
        private readonly Func<UpdateRequest<T>[], int> _defaultUpdateFunc = arg => 0;

        private readonly Func<T, T, bool> _ifRowNeedsUpdate;
        private readonly Func<T, TIdentifier> _getIdentifier;
        private readonly Func<T[], int> _adder;
        private readonly Func<T[], int> _updater;
        private readonly Func<UpdateRequest<T>[], int> _diyUpdater;
        private readonly Func<T[], int> _deleter;

        private readonly RunningMode _runningMode;

        public Upserter(Func<T, TIdentifier> getIdentifier,
                        Func<T, T, bool> ifRowNeedUpdate,
                        Func<T[], int> adder,
                        Func<T[], int> updater,
                        Func<T[], int> deleter)
        {
            _runningMode = RunningMode.Legacy;
            _getIdentifier = getIdentifier;
            _ifRowNeedsUpdate = ifRowNeedUpdate ?? _defaultIfRowNeedsUpdate;
            _adder = adder ?? _defaultFunc;
            _updater = updater ?? _defaultFunc;
            _deleter = deleter ?? _defaultFunc;
        }

        public Upserter(Func<T, TIdentifier> getIdentifier,
                        Func<T[], int> adder,
                        Func<UpdateRequest<T>[], int> updater,
                        Func<T[], int> deleter)
        {
            _runningMode = RunningMode.DIY;
            _getIdentifier = getIdentifier;
            _adder = adder ?? _defaultFunc;
            _diyUpdater = updater ?? _defaultUpdateFunc;
            _deleter = deleter ?? _defaultFunc;
        }

        private int RunUpdateUsingComparer(IDictionary<TIdentifier, T> existingRowsDictionary, T[] insertingRows)
        {
            var updateRows = insertingRows.Where(ir =>
            {
                if (!existingRowsDictionary.ContainsKey(_getIdentifier(ir)))
                    return false;

                var existingRow = existingRowsDictionary[_getIdentifier(ir)];
                return _ifRowNeedsUpdate(existingRow, ir);
            }).ToArray();
            return _updater(updateRows);
        }

        private int RunUpdate(IDictionary<TIdentifier, T> existingRowsDictionary, T[] insertingRows)
        {
            var updateRequests = new List<UpdateRequest<T>>();
            foreach (var insertingRow in  insertingRows)
            {
                if (!existingRowsDictionary.ContainsKey(_getIdentifier(insertingRow)))
                    continue;

                var existingRow = existingRowsDictionary[_getIdentifier(insertingRow)];
                updateRequests.Add(new UpdateRequest<T>(existingRow, insertingRow));
            }
            return _diyUpdater(updateRequests.ToArray());
        }

        public UpsertResult Run(T[] existingRows, T[] insertingRows)
        {
            var existingRowsDictionary = existingRows.ToDictionary(r => _getIdentifier(r));
            var insertingRowsDictionary = insertingRows.ToDictionary(r => _getIdentifier(r));

            var newRows = insertingRows.Where(ir => !existingRowsDictionary.ContainsKey(_getIdentifier(ir))).ToArray();
            var deletedRows = existingRows.Where(er => !insertingRowsDictionary.ContainsKey(_getIdentifier(er))).ToArray();

            var added = _adder(newRows);
            var updated = _runningMode == RunningMode.Legacy 
                ? RunUpdateUsingComparer(existingRowsDictionary, insertingRows)
                : RunUpdate(existingRowsDictionary, insertingRows);
            var deleted = _deleter(deletedRows);

            return new UpsertResult(added, updated, deleted);
        }

        private enum RunningMode
        {
            Legacy,
            // ReSharper disable InconsistentNaming
            DIY
            // ReSharper restore InconsistentNaming
        }
    }
}