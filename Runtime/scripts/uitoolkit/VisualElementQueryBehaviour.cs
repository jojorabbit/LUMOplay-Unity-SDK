using System;
using System.Collections.Generic;
using LUMOPlay.UIToolkit.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace LUMOPlay.UIToolkit {
    public class VisualElementQueryBehaviour : MonoBehaviour, IQuery {

        [Tooltip("The UIDocument")]
        [SerializeField] private UIDocument document;

        [Tooltip("Only elements of specified type are used in query.")]
        [SerializeField] private VisualElementType elementType = VisualElementType.Button;

        [Tooltip("When true selects all elements matching the criteria, otherwise only first match is selected.")]
        [SerializeField] private bool multiSelect;

        [Tooltip("The uss class name of the element.")]
        [SerializeField] private string className;

        [Tooltip("The element name in UXML")]
        [SerializeField] private string elementName;


        private bool executed;
        private readonly List<VisualElement> elements = new List<VisualElement>();


        private UIDocument Document {
            get {
                if (!document) {
                    document = FindDocument();
                }

                if (!document) {
                    Debug.LogWarning($"{nameof(VisualElementQueryBehaviour)}::{nameof(Document)} " +
                                     $"No {nameof(UIDocument)} found!"
                    );
                }

                return document;
            }
        }


        private List<VisualElement> Elements {
            get {
                if (executed) {
                    return elements;
                }

                Execute();
                executed = true;

                return elements;
            }
        }

        public GameObject LinkedGameObject => gameObject;

        public IList<VisualElement> GetElements() => Elements;

        public virtual void OnEnable() {
            Execute();
        }

        public virtual void OnDisable() {
            executed = false;
        }

        /// <summary>
        /// Runs the query and saves the result in Elements property.
        /// </summary>
        public virtual void Execute() {
            if (Document == null || Document.rootVisualElement == null) {
                return;
            }

            elements.Clear();
            
            if (multiSelect) {
                var foundElements = Document.QueryTypes(
                    elementType,
                    elementName: string.IsNullOrEmpty(elementName) ? null : elementName,
                    className: string.IsNullOrEmpty(className) ? null : className
                );
                
                elements.AddRange(foundElements);

                // debug only
                foreach (var element in elements) {
                    Debug.Log($"{nameof(VisualElementQueryBehaviour)}::{nameof(Execute)} " +
                              $"element= {element} contentRect= {element.contentRect}"
                    );
                    // element.style.backgroundColor = new StyleColor { value = Color.green };
                }
                // end debug only
            } else {
                var element = Document.QueryType(
                    elementType,
                    elementName: string.IsNullOrEmpty(elementName) ? null : elementName,
                    className: string.IsNullOrEmpty(className) ? null : className
                );

                if (element != null) {
                    // debug only
                    Debug.Log($"{nameof(VisualElementQueryBehaviour)}::{nameof(Execute)} " +
                              $"element= {element} contentRect= {element.contentRect}"
                    );
                    // element.style.backgroundColor = new StyleColor { value = Color.green };
                    // end debug only
                    
                    elements.Add(element);
                }
            }
        }

        private UIDocument FindDocument() {
            if (!document) {
                document = GetComponentInParent<UIDocument>(includeInactive: true);
            }

            return document;
        }


        private void OnValidate() {
            if (!document) {
                document = FindDocument();
            }
        }

    }
}