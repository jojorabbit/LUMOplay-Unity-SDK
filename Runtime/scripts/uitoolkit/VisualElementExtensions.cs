using UnityEngine;
using UnityEngine.UIElements;

namespace LUMOPlay.UIToolkit {
    public static class VisualElementExtensions {

        public static void Click(this VisualElement self) {
            if (self == null) {
                Debug.LogWarning($"{nameof(VisualElementExtensions)}::{nameof(Click)} " +
                                 $"Cannot click on null visual element!"
                );

                return;
            }

            // for RegisterCallback<ClickEvent>
            using var clickEvent = ClickEvent.GetPooled();

            clickEvent.target = self;
            self.panel.visualTree.SendEvent(clickEvent);
            
            // for clicked += event registration
            // ReSharper disable once InvertIf
            if (self is Button button) {
                using var submitEvent = NavigationSubmitEvent.GetPooled();
                submitEvent.target = button;
                button.SendEvent(submitEvent);
            }
        }

    }
}