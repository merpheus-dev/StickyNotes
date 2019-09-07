using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Subtegral.StickyNotes
{
    [CustomEditor(typeof(StickyNotesDatabase))]
    public class StickyNoteDatabaseEditor : Editor
    {
        private StickyNotesDatabase _database;

        public override void OnInspectorGUI()
        {
            if (_database == null)
                _database = target as StickyNotesDatabase;

            foreach (var perItem in _database.SceneBindings)
            {
                EditorGUILayout.LabelField(perItem.LocalIdentifier.ToString());
            }
        }
    }
}