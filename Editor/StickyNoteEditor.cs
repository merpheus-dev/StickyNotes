using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Subtegral.StickyNotes
{
    [CustomEditor(typeof(StickyNote))]
    public class StickyNoteEditor : Editor
    {
        private StickyNote _note;
        public override VisualElement CreateInspectorGUI()
        {
            _note = target as StickyNote;
            var database = StickNoteManagementUtils.LoadOrCreateDatabase();
            var root = new VisualElement();
            var noteStyleSheet = Resources.Load<StyleSheet>("StickyNotesStyle");
            root.styleSheets.Add(noteStyleSheet);
            root.name = "row";

            var headerLabel = new TextField()
            {
                name = "headerTextField"
            };
            headerLabel.SetValueWithoutNotify(_note.noteHeader);
            headerLabel.RegisterValueChangedCallback(evt =>
            {
                _note.noteHeader = evt.newValue;
                EditorUtility.SetDirty(_note);
            });
            var headerVisualElement = new VisualElement()
            {
                name = "header"
            };
            headerVisualElement.Add(headerLabel);
            root.Add(headerVisualElement);

            var noteLabel = new TextField()
            {
                name = "contentTextField"
            };
            noteLabel.SetValueWithoutNotify(_note.noteContext);
            noteLabel.RegisterValueChangedCallback(evt =>
            {
                _note.noteContext = evt.newValue;
                EditorUtility.SetDirty(_note);
            });
            root.Add(noteLabel);
            
//            foreach (var stickyNote in database.GameObjectHashMap)
//            {
//                var row = new VisualElement();
//                row.style.flexDirection = FlexDirection.Row;
//                var labelKey = new Label(stickyNote.Key);
//                var labelValue = new Label(stickyNote.Value.attachment.ToString());
//                row.Add(labelKey);
//                row.Add(labelValue);
//                root.Add(row);
//            }

            return root;
        }

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            if (StickNoteManagementUtils.Icon == null)
                return null;
            var texture = new Texture2D(width,height);
            EditorUtility.CopySerialized (StickNoteManagementUtils.Icon, texture);
            return texture;
        }
    }
}