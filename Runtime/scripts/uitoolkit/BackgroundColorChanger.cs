using UnityEngine;
using UnityEngine.UIElements;

namespace LUMOPlay.UIToolkit {
    public class BackgroundColorChanger : VisualElementBaseComponent<VisualElement> {

        private const float Saturation = 1f;
        private const float Value = 0.5f;

        private float hue;
        
        private void SetColor() {
            var color = Color.HSVToRGB(hue, Saturation, Value);

            if (!HasElements) {
                return;
            }

            foreach (var element in Elements) {
                if (element == null) {
                    continue;
                }

                element.style.backgroundColor = color;
            }
        }

        public void PickRandomColor() {
            hue = Random.value;
            SetColor();
        }

    }
}