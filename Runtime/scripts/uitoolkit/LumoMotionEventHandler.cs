#if UNITY_EDITOR
#define MOTION_EVENT_HANDLER_LOGS
#endif
using LUMOplay;
using UnityEngine;
using UnityEngine.UIElements;

namespace LUMOPlay.UIToolkit {
    // TODO: make delayed event handler
    public class LumoMotionEventHandler : VisualElementBaseComponent<VisualElement> {

        [Header("Call public methods in your own scripts (Callback passes a LumoMotionEvent)")]
        [SerializeField] private LumoMotionEventUnityEvent onMotionEventCallbacks;


        protected override void OnAttach() {
            base.OnAttach();

            Debug.Log($"{nameof(LumoMotionEventHandler)}::{nameof(OnAttach)} elementCount= {Elements.Count}");
        }

        // using FixedUpdate as MotionListener only has new data during FixedUpdate
        private void FixedUpdate() {
            if (!HasElements) {
                // no elements so do nothing
                return;
            }
            
            foreach (var visualElement in Elements) {
                if (visualElement == null) {
                    // should not have nulls but in case it does skip them
                    continue;
                }
                
                CheckElementBounds(visualElement);
            }
        }

        /// <summary>
        /// Checks the bounds for specified <see cref="VisualElement"/>.
        /// </summary>
        /// <param name="element">The element to check.</param>
        private void CheckElementBounds(VisualElement element) {
            if (element == null) {
                return;
            }

            var boundsRect = element.worldBound;
            // need to invert the coordinate system 0,0 in UI Toolkit is at top left
            // TODO: check if this works for other elements 
            boundsRect.y = Screen.height - boundsRect.y - boundsRect.height;
            
            Debug.DrawLine(boundsRect.min, boundsRect.max, Color.blue, 2f);
            // Debug.Log($"{GetType()}::{nameof(CheckElementBounds)} name= {element.name} bounds= {boundsRect}");
            
            var events = MotionListener.CheckRectOverlapsBounds(boundsRect);

            foreach (var evt in events) {
                // invoke all defined callbacks
                Invoke(evt);
            }
        }

        /// <summary>
        /// Invokes the <see cref="onMotionEventCallbacks"/> event.
        /// </summary>
        /// <param name="motionEvent">The <see cref="LumoMotionEvent"/> instance.</param>
        private void Invoke(LumoMotionEvent motionEvent) {
            // Debug.Log($"{nameof(GetType)}::{nameof(Invoke)} evt= {motionEvent}");
            onMotionEventCallbacks.Invoke(motionEvent);
        }

    }
}