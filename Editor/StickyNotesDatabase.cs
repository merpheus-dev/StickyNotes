using System.Collections.Generic;
using UnityEngine;

namespace Subtegral.StickyNotes
{
    public class StickyNotesDatabase : ScriptableObject
    {
        public Dictionary<string, StickyNote> GameObjectHashMap = new Dictionary<string, StickyNote>();
        public void AddGameObjectToHashList(string guid, StickyNote noteInstance)
        {
            if (GameObjectHashMap.ContainsKey(guid))
                return;
            GameObjectHashMap.Add(guid, noteInstance);
        }

        public void RemoveNoteWithHash(string guid)
        {
            GameObjectHashMap.Remove(guid);
        }

        public StickyNote GetStickyNoteFromHash(string guid)
        {
            return GameObjectHashMap.TryGetValue(guid, out StickyNote stickyNote) ? stickyNote : null;
        }
    }
}