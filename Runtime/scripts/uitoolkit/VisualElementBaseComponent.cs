using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

// ReSharper disable InconsistentNaming

namespace LUMOPlay.UIToolkit {
    [RequireComponent(typeof(VisualElementProvider))]
    public abstract class VisualElementBaseComponent<T> : MonoBehaviour where T : VisualElement {

        protected VisualElementProvider provider;
        protected List<T> elements;

        protected VisualElementProvider Provider {
            get {
                if (provider) {
                    return provider;
                }

                provider = GetComponent<VisualElementProvider>();
                provider.OnAttachAction += OnAttach;
                provider.OnDetachAction += OnDetach;

                return provider;
            }
        }

        protected List<T> Elements {
            get {
                if (elements != null) {
                    return elements;
                }

                Debug.Log($"{nameof(GetType)}::{nameof(Elements)} provider= {Provider} " +
                          $"provider.elements= {Provider?.Elements}"
                );

                if (Provider != null && Provider.Elements != null) {
                    elements = Provider.Elements.Select(element => element as T)
                        .Where(element => element != null)
                        .ToList();
                }

                return elements;
            }
        }

        /// <summary>
        /// Gets the first element in case it has elements, null otherwise.
        /// </summary>
        protected T Element => HasElements ? Elements[0] : null;

        protected bool HasElements => Elements != null && Elements.Count > 0;

        protected virtual void OnEnable() {
            OnAttach();
        }

        protected virtual void OnDestroy() {
            if (provider == null) {
                return;
            }

            provider.OnAttachAction -= OnAttach;
            provider.OnDetachAction -= OnDetach;
        }

        protected void ExecuteOnEveryElement(Action<T> action) {
            if (!HasElements) {
                return;
            }

            foreach (var visualElement in Elements) {
                if (visualElement == null) {
                    continue;
                }

                action?.Invoke(visualElement);
            }
        }


        protected virtual void OnAttach() { }
        protected virtual void OnDetach() { }

    }
}