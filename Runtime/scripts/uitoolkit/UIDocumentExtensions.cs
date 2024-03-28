using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace LUMOPlay.UIToolkit {
    public static class UIDocumentExtensions {

        public static VisualElement QueryType(this UIDocument self,
            VisualElementType elementType = VisualElementType.VisualElement, string elementName = null,
            string className = null) {
            if (self == null || self.rootVisualElement == null) {
                return null;
            }

            var systemType = elementType.GetElementSystemType();

            if (systemType == null) {
                return null;
            }

            return self.QueryType(systemType, elementName, className);
        }

        public static VisualElement QueryType(this UIDocument self, Type systemType, string elementName = null,
            string className = null) {
            if (self == null || self.rootVisualElement == null) {
                return null;
            }

            var queryState = self.rootVisualElement.Query<VisualElement>(elementName, className).Build();

            foreach (var visualElement in queryState) {
                var elementType = visualElement.GetType();

                if (elementType == systemType || elementType.IsSubclassOf(systemType)) {
                    return visualElement;
                }
            }

            return null;
        }

        public static List<VisualElement> QueryTypes(this UIDocument self,
            VisualElementType elementType = VisualElementType.VisualElement, string elementName = null,
            string className = null) {
            if (self == null || self.rootVisualElement == null) {
                return null;
            }

            var toReturn = new List<VisualElement>();
            var systemType = elementType.GetElementSystemType();

            if (systemType == null) {
                return toReturn;
            }

            return self.QueryTypes(systemType, elementName, className);
        }

        public static List<VisualElement> QueryTypes(this UIDocument self, Type systemType,
            string elementName = null, string className = null) {
            
            var toReturn = new List<VisualElement>();
            
            if (self == null || self.rootVisualElement == null) {
                return toReturn;
            }

            var queryState = self.rootVisualElement.Query<VisualElement>(elementName, className).Build();

            foreach (var visualElement in queryState) {
                var elementType = visualElement.GetType();

                if (elementType == systemType || elementType.IsSubclassOf(systemType)) {
                    toReturn.Add(visualElement);
                }
            }

            return toReturn;
        }

    }
}