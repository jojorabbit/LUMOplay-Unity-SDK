using System;
using System.Collections;
using System.Collections.Generic;
using LUMOPlay.UIToolkit.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace LUMOPlay.UIToolkit {
    [DisallowMultipleComponent]
    public class VisualElementProvider : MonoBehaviour {

        public event Action OnAttachAction;
        public event Action OnDetachAction;

        private bool queryInitialized;
        private IQuery query;

        private List<VisualElement> elements;
        private bool ignoreFurtherDetachEvents;
        private Coroutine reattachCoroutine;
        
        public IQuery Query {
            get {
                // skip unless the game object has become null (when deleted/destroyed)
                if (queryInitialized && (query == null || query.LinkedGameObject != null)) {
                    return query;
                }

                queryInitialized = true;

                if (query == null || query.LinkedGameObject == null) {
                    query = GetComponent<IQuery>();
                }

                return query;
            }
        }

        public IList<VisualElement> QueriedElements => Query?.GetElements();

        public bool HasElements => Elements != null && Elements.Count > 0;

        public List<VisualElement> Elements {
            get {
                if (elements != null) {
                    return elements;
                }

                if (Query == null || QueriedElements == null || QueriedElements.Count <= 0) {
                    return elements;
                }

                elements ??= new List<VisualElement>();

                foreach (var queriedElement in QueriedElements) {
                    if (queriedElement == null) {
                        continue;
                    }
                        
                    elements.Add(queriedElement);
                }

                return elements;
            }
        }

        private void ClearElementCache() {
            queryInitialized = false;
            query = null;
            elements = null;
        }

        private void OnEnable() {
            OnAttach();
        }

        private void OnAttach() {
            ClearElementCache();

            Query?.Execute();

            RegisterDetachEvent();
            ignoreFurtherDetachEvents = false;
            
            OnAttachAction?.Invoke();
        }

        private void RegisterDetachEvent() {
            if (Elements == null || Elements.Count <= 0) {
                return;
            }

            foreach (var visualElement in Elements) {
                visualElement.UnregisterCallback<DetachFromPanelEvent>(OnDetachedFromPanel);
                visualElement.RegisterCallback<DetachFromPanelEvent>(OnDetachedFromPanel);
            }
        }

        private void UnregisterDetachEvent() {
            if (Elements == null || Elements.Count <= 0) {
                return;
            }

            foreach (var visualElement in Elements) {
                visualElement.UnregisterCallback<DetachFromPanelEvent>(OnDetachedFromPanel);
            }
        }

        private void OnDetachedFromPanel(DetachFromPanelEvent evt) {
            if (!ignoreFurtherDetachEvents && this != null && isActiveAndEnabled) {
                // make sure only one detach call is propagated
                ignoreFurtherDetachEvents = true;
                UnregisterDetachEvent();
                
                // try to reattach (required when UI is reloaded in editor)
                if (reattachCoroutine != null) {
                    StopCoroutine(reattachCoroutine);
                    reattachCoroutine = null;
                }

                reattachCoroutine = StartCoroutine(ReattachRoutine());
                
                OnDetachAction?.Invoke();
            }
        }

        private IEnumerator ReattachRoutine() {
            yield return null;

            ignoreFurtherDetachEvents = false;
            
            OnAttach();
        }

    }
}