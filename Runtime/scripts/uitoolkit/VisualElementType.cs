using System;
using UnityEngine.UIElements;

namespace LUMOPlay.UIToolkit {
    public enum VisualElementType {

        Button,
        Toggle,
        Image,
        VisualElement,
        // TODO: add slider, text element etc

    }

    public static class VisualElementTypeExtensions {
        
        public static bool IsButton(this VisualElementType self) => self == VisualElementType.Button;
        public static bool IsImage(this VisualElementType self) => self == VisualElementType.Image;
        public static bool IsToggle(this VisualElementType self) => self == VisualElementType.Toggle;
        public static bool IsVisualElement(this VisualElementType self) => self == VisualElementType.VisualElement;


        public static Type GetElementSystemType(this VisualElementType self) {
            switch (self) {
                case VisualElementType.Button:
                    return typeof(Button);
                case VisualElementType.Toggle:
                    return typeof(Toggle);
                case VisualElementType.Image:
                    return typeof(Image);
                case VisualElementType.VisualElement:
                    return typeof(VisualElement);
                default:
                    return null;
            }
        }

    }
}