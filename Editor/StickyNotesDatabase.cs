using System.Collections.Generic;
using UnityEngine;

namespace Subtegral.StickyNotes
{
    public class StickyNotesDatabase : ScriptableObject
    {
        public List<StickyNoteBinding> Bindings = new List<StickyNoteBinding>();

        public void AddGameObjectToHashList(GameObject gameObject, StickyNote noteInstance)
        {
            if (Bindings.Exists(x => x.Key == gameObject))
                return;
            Bindings.Add(new StickyNoteBinding
            {
                Key = gameObject, Note = noteInstance
            });
        }

        public void RemoveNoteWithHash(GameObject gameObject)
        {
            Bindings.RemoveAll(x=>x.Key==gameObject);
        }

        public StickyNote GetStickyNoteFromHash(GameObject gameObject)
        {
            return Bindings.Find(x => x.Key == gameObject)?.Note;
            
        }
    }
}