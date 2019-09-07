using System.IO;
using Subtegral.StickyNotes;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Directory = UnityEngine.Windows.Directory;

public static class StickNoteManagementUtils
{
    private static Texture2D _icon;
    public static Texture2D Icon
    {
        get
        {
            if(_icon==null)
                _icon = Resources.Load<Texture2D>("StickyNoteIcon");
            return _icon;
        }
    }

    public static StickyNotesDatabase LoadOrCreateDatabase()
    {
        var dbInstance = Resources.Load<StickyNotesDatabase>("StickyNotes/Database");
        if (dbInstance != null)
            return dbInstance;

        var soInstance = ScriptableObject.CreateInstance<StickyNotesDatabase>();
        if (!Directory.Exists("Assets/Resources"))
            Directory.CreateDirectory("Assets/Resources");

        if (!Directory.Exists("Assets/Resources/StickyNotes"))
            Directory.CreateDirectory("Assets/Resources/StickyNotes");

        AssetDatabase.CreateAsset(soInstance, "Assets/Resources/StickyNotes/Database.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return soInstance;
    }

    [MenuItem("Subtegral/Sticky Notes/Setup")]
    public static void StickNotesSetup()
    {
        StickNoteManagementUtils.LoadOrCreateDatabase();
    }

    [MenuItem("Assets/Create/Sticky Note", false, 100)]
    public static void StickyNoteCreateCommand()
    {
        var noteActionInstance = ScriptableObject.CreateInstance<StickyNoteAction>();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, noteActionInstance, "NewStickyNote",
            StickNoteManagementUtils.Icon, null);
    }

    class StickyNoteAction : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var noteInstance = ScriptableObject.CreateInstance<StickyNote>();
            noteInstance.name = Path.GetFileName(pathName);
            noteInstance.attachment = Attachment.SelfContained;
            AssetDatabase.CreateAsset(noteInstance, pathName + ".asset");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            ProjectWindowUtil.ShowCreatedAsset(noteInstance);

            var db = LoadOrCreateDatabase();
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(noteInstance, out string guid, out long localId))
                db.AddGameObjectToHashList(guid, noteInstance);
        }
    }
}