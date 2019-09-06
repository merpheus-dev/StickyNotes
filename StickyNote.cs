using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Subtegral.StickyNotes
{
    public class StickyNote : ScriptableObject
    {
        public Attachment attachment;
        public Color paperColor;
        public string noteHeader;
        public string noteContext;
    }
}