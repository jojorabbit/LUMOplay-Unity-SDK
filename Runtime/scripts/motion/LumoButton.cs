#if UNITY_EDITOR
#define LOGS_ENABLED
#endif

using System;
using System.Collections;
using LUMOplay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ReSharper disable UnusedMember.Global

namespace LUMOPlay {
    /// <summary>
    /// Note: instead of using <see cref="OnMotionUI.onMotionEventCallbacks"/> use
    /// <see cref="onClick"/>
    /// </summary>
    [AddComponentMenu("LUMOplay/LumoButton")]
    [RequireComponent(typeof(OnMotionUI))]
    public class LumoButton : Selectable, IPointerClickHandler, ISubmitHandler {

        [Serializable]
        public class LumoButtonClickedEvent : UnityEvent { }

        [Min(0)]
        [Tooltip("Delay after which button will submit.")]
        [SerializeField] private float delayDuration = 0.5f;

        [Tooltip("The required OnMotionUI behaviour")]
        [SerializeField] private OnMotionUI onMotionUI;

        private bool blockMotion;

        [SerializeField] private LumoButtonClickedEvent onClick = new();

        private SelectionState previousState;

        protected LumoButton() { }

        public float DelayDuration {
            get => delayDuration;
            set => delayDuration = value;
        }

        public LumoButtonClickedEvent OnClick {
            get => onClick;
            set => onClick = value;
        }

        protected override void Awake() {
            base.Awake();

            if (!onMotionUI) {
                onMotionUI = GetComponent<OnMotionUI>();
            }
        }

        protected override void OnEnable() {
            base.OnEnable();
            onMotionUI.onMotionEventCallbacks.AddListener(OnLumoMotionEvent);
        }

        protected override void OnDisable() {
            base.OnDisable();

            onMotionUI.onMotionEventCallbacks.RemoveListener(OnLumoMotionEvent);
        }

        private void Press() {
            if (!IsActive() || !IsInteractable()) {
                return;
            }

            UISystemProfilerApi.AddMarker("LumoButton.onClick", this);
            onClick.Invoke();
        }


        // required for mouse and touch
        public virtual void OnPointerClick(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) {
                return;
            }

            Debug.Log($"{nameof(LumoButton)}::{nameof(OnPointerClick)}");
            Press();
        }

        // required for mouse and touch
        public virtual void OnSubmit(BaseEventData eventData) {
            // if we get set disabled during the press
            // don't run the coroutine.
            if (!IsActive() || !IsInteractable()) {
                return;
            }

            if (blockMotion) {
                return;
            }

            Debug.Log($"{nameof(LumoButton)}::{nameof(OnSubmit)}");

            blockMotion = true;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmitRoutine());
        }

        private IEnumerator OnFinishSubmitRoutine() {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime) {
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }

            yield return StartCoroutine(OnPostFinishSubmitRoutine());

            DoStateTransition(currentSelectionState, false);

            blockMotion = false;
        }

        #region == LUMO related motion events ==

        private void OnLumoMotionEvent(LumoMotionEvent lumoMotionEvent) {
            if (!IsActive() || !IsInteractable()) {
                return;
            }

            if (blockMotion) {
                return;
            }

            blockMotion = true;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnLumoFinishSubmitRoutine());
        }

        protected override void DoStateTransition(SelectionState state, bool instant) {
            if (previousState == state) {
                return;
            }

            base.DoStateTransition(state, instant);
            previousState = state;
        }

        private IEnumerator OnLumoFinishSubmitRoutine() {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime) {
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }

            yield return StartCoroutine(OnPostFinishSubmitRoutine());

            DoStateTransition(currentSelectionState, false);

            blockMotion = false;
        }

        private IEnumerator OnPostFinishSubmitRoutine() {
            var elapsedTime = 0f;

            while (elapsedTime < delayDuration) {
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }

            Press();
        }

        #endregion

        #if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();

            if (!onMotionUI) {
                onMotionUI = GetComponent<OnMotionUI>();
            }
        }
        #endif

    }
}