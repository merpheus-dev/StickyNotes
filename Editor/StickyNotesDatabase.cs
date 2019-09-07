using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Subtegral.StickyNotes
{
    public class StickyNotesDatabase : ScriptableObject
    {
        public List<SceneNoteBinding> SceneBindings = new List<SceneNoteBinding>();
        public List<AssetDatabaseBinding> AssetDatabaseBindings = new List<AssetDatabaseBinding>();
        
        public void NewStickySceneNote(long localId,StickyNote noteInstance)
        {
            noteInstance.attachment = Attachment.GameObject;
            SceneBindings.Add(new SceneNoteBinding(localId,noteInstance));
            
        }

        public void NewStickyAssetNote(string guid,StickyNote noteInstance)
        {
            noteInstance.attachment = Attachment.Asset;
            AssetDatabaseBindings.Add(new AssetDatabaseBinding(guid,noteInstance));
        }

        public void RemoveStickyNote(StickyNote note)
        {
            AssetDatabaseBindings.RemoveAll(x => x.Note == note);
            SceneBindings.RemoveAll(x => x.Note == note);
        }

        public StickyNote[] GetStickySceneNotes(long localId)
        {
            return SceneBindings.FindAll(x => x.LocalIdentifier == localId).Select(x=>x.Note).ToArray();
        }

        public StickyNote[] GetStickyAssetNotes(string guid)
        {
            return AssetDatabaseBindings.FindAll(x => x.GUID == guid).Select(x => x.Note).ToArray();
        }
    }
}