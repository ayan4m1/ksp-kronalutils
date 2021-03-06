﻿using System.Collections.Generic;

namespace VesselViewer
{
    //taken from: https://github.com/Alewx/unofficailUbioWeld/tree/Beta -> https://github.com/Alewx/unofficailUbioWeld/blob/Beta/UbioWeldingLtd/WeldingHelpers.cs
    public static class EditorLockManager
    {
        private static readonly List<EditorLock> _activeLocks = new List<EditorLock>();


        /// <summary>
        ///     locks the editor keys for the given key
        /// </summary>
        /// <param name="loadButton"></param>
        /// <param name="exitButton"></param>
        /// <param name="saveButton"></param>
        /// <param name="lockKey"></param>
        public static void lockEditor(bool loadButton, bool exitButton, bool saveButton, string lockKey)
        {
            if (!isLockKeyActive(lockKey))
            {
                EditorLogic.fetch.Lock(loadButton, exitButton, saveButton, lockKey);
                _activeLocks.Add(new EditorLock(loadButton, exitButton, loadButton, lockKey));
            }
        }


        /// <summary>
        ///     unlocks the editor for the entered key
        /// </summary>
        /// <param name="lockKey"></param>
        public static void unlockEditor(string lockKey)
        {
            if (isLockKeyActive(lockKey))
            {
                EditorLogic.fetch.Unlock(lockKey);

                for (var i = 0; i < _activeLocks.Count; i++)
                    if (_activeLocks[i].lockKey == lockKey)
                    {
                        _activeLocks.RemoveAt(i);
                        return;
                    }
            }
        }


        /// <summary>
        ///     returns the info about the current lockstatus
        /// </summary>
        /// <returns></returns>
        public static bool isEditorLocked()
        {
            return _activeLocks.Count > 0 ? true : false;
        }


        /// <summary>
        ///     provides all the keys that are currently in use
        /// </summary>
        /// <returns></returns>
        public static string[] getActiveLockKeys()
        {
            var locks = new string[_activeLocks.Count];
            for (var i = 0; i < locks.Length; i++)
                locks[i] = _activeLocks[i].lockKey;
            return locks;
        }


        /// <summary>
        ///     provides the binary information if the key is already in use
        /// </summary>
        /// <param name="lockKey"></param>
        /// <returns></returns>
        public static bool isLockKeyActive(string lockKey)
        {
            foreach (var l in _activeLocks)
                if (l.lockKey == lockKey)
                    return true;
            return false;
        }


        /// <summary>
        ///     provides the information if the main buttons of the editor are locked
        /// </summary>
        /// <returns></returns>
        public static bool isEditorSoftlocked()
        {
            foreach (var l in _activeLocks)
                if (l.LockSave && l.lockExit && l.lockLoad)
                    return true;
            return false;
        }


        /// <summary>
        ///     resets the editorlocks to a clean state
        /// </summary>
        public static void resetEditorLocks()
        {
            _activeLocks.Clear();
        }

        public class EditorLock
        {
            public EditorLock(bool save, bool exit, bool load, string key)
            {
                LockSave = save;
                lockExit = exit;
                lockLoad = load;
                lockKey = key;
            }

            public bool LockSave { get; set; }

            public bool lockExit { get; set; }

            public bool lockLoad { get; set; }

            public string lockKey { get; set; }
        }
    }
}