using System;
using AssemblyReloader.Properties;
using UnityEngine;

namespace AssemblyReloader.DataObjects
{
    public class WindowAppearanceInfo
    {
        public GUISkin Skin { get; set; }
        public Rect InitialSize { get; set; }
        public Vector2 MinDimensions { get; set; }
        public Texture2D DragCursorTexture { get; set; }
        public Vector2 DragHotzoneSize { get; set; }

        public WindowAppearanceInfo(
            [NotNull] GUISkin skin, 
            Rect initialSize, 
            Vector2 dragHotzoneSize, 
            Vector2 minDimensions, [NotNull] Texture2D dragCursorTexture)
        {
            if (skin == null) throw new ArgumentNullException("skin");
            if (dragCursorTexture == null) throw new ArgumentNullException("dragCursorTexture");

            Skin = skin;
            InitialSize = initialSize;
            MinDimensions = minDimensions;
            DragCursorTexture = dragCursorTexture;
            DragHotzoneSize = dragHotzoneSize;
        }
    }
}
