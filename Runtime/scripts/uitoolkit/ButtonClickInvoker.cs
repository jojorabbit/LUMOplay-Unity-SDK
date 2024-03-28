using UnityEngine;
using UnityEngine.UIElements;

namespace LUMOPlay.UIToolkit {
    public class ButtonClickInvoker : VisualElementBaseComponent<Button> {

        public void Click() {
            if (!HasElements) {
                return;
            }

            foreach (var button in Elements) {
                button.Click();
            }
        }


        public void LogTest() {
            Debug.Log($"{nameof(ButtonClickInvoker)}::{nameof(LogTest)} Element= {Element}");
        }

    }
}