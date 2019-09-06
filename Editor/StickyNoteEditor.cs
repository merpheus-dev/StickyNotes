using UnityEditor;
using UnityEngine.UIElements;

public class StickyNoteEditor : Editor
{
    [MenuItem("Subtegral/Sticky Notes/Setup")]
    public static void StickNotesSetup()
    {
        StickNoteManagementUtils.LoadOrCreateDatabase();
    }

    public override VisualElement CreateInspectorGUI()
    {
        var database = StickNoteManagementUtils.LoadOrCreateDatabase();
        var root = new VisualElement();
        foreach (var stickyNote in database.GameObjectHashMap)
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            var labelKey = new Label(stickyNote.Key);
            var labelValue = new Label(stickyNote.Value.attachment.ToString());
            row.Add(labelKey);
            row.Add(labelValue);
            root.Add(row);
        }

        return root;
    }

}