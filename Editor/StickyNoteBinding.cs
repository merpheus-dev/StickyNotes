using System;
using UnityEngine;

namespace Subtegral.StickyNotes
{
    [Serializable]
    public abstract class StickyNoteBinding
    {
        public StickyNote Note;
    }

    [Serializable]
    public class SceneNoteBinding : StickyNoteBinding
    {
        public long LocalIdentifier;

        public SceneNoteBinding(long localIdentifier,StickyNote note)
        {
            LocalIdentifier = localIdentifier;
            Note = note;
        }
    }

    [Serializable]
    public class AssetDatabaseBinding : StickyNoteBinding
    {
        public string GUID;

        public AssetDatabaseBinding(string guid,StickyNote note)
        {
            GUID = guid;
            Note = note;
        }
    }
}