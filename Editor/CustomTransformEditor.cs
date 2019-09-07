using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Subtegral.StickyNotes
{
    [CustomEditor(typeof(Transform), true)]
    public class CustomTransformEditor : Editor
    {
        private Editor defaultEditor;
        private VisualElement transformContainer;
        private Transform _transform;

        public override VisualElement CreateInspectorGUI()
        {
            _transform = target as Transform;
            transformContainer = new VisualElement();
            var noteField = new VisualElement()
            {
                name = "noteField"
            };
            var button = new Button(OnNoteTake)
            {
                text = "POST-IT!"
            };
            noteField.Add(button);

            transformContainer.Add(noteField);
            var imguiContainer = new IMGUIContainer(() => { defaultEditor.OnInspectorGUI(); });
            transformContainer.Add(imguiContainer);
            return transformContainer;
        }

        private void OnNoteTake()
        {
            var noteField = transformContainer.Q("noteField");
            transformContainer.Remove(noteField);
            var db = StickyNoteManagementUtils.LoadOrCreateDatabase();
            var localId = StickyNoteManagementUtils.GetLocalIdentifierFromSceneObject(_transform.gameObject);
            if (localId!=0 && db.GetStickySceneNotes(localId).Length!=0)
            {
                noteField = new VisualElement();
                noteField.style.flexDirection = FlexDirection.Row;
                foreach (var stickySceneNote in db.GetStickySceneNotes(localId))
                {
                    noteField.Add(new Label(stickySceneNote.paperColor.ToString()));
                }
                transformContainer.Add(noteField);
            }
            else
            {
                var noteInstance = ScriptableObject.CreateInstance<StickyNote>();
                AssetDatabase.AddObjectToAsset(noteInstance, db);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                db.NewStickySceneNote(
                    StickyNoteManagementUtils.GetLocalIdentifierFromSceneObject(_transform.gameObject), noteInstance);
                EditorUtility.SetDirty(db);
                
            }
        }


        private void OnEnable()
        {
            //When this inspector is created, also create the built-in inspector
            defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
        }

        private void OnDisable()
        {
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable
            MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (disableMethod != null)
                disableMethod.Invoke(defaultEditor, null);
            DestroyImmediate(defaultEditor);
        }
    }
}