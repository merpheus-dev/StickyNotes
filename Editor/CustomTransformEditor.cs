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
            //transformContainer.Remove(noteField);
           var db = StickyNoteManagementUtils.LoadOrCreateDatabase();
           if (db.GetStickyNoteFromHash(_transform.gameObject) != null)
           {
               Debug.Log("RECORD EXISTS");
           }
           else
           {
               var noteInstance = ScriptableObject.CreateInstance<StickyNote>();
               AssetDatabase.AddObjectToAsset(noteInstance, db);
               AssetDatabase.SaveAssets();
               AssetDatabase.Refresh();
               db.AddGameObjectToHashList(_transform.gameObject, noteInstance);
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