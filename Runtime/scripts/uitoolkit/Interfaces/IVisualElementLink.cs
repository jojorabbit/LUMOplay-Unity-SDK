using UnityEngine;
using UnityEngine.UIElements;

namespace LUMOPlay.UIToolkit.Interfaces {
    public interface IVisualElementLink {

        VisualElement GetElement();
        void RefreshElement();
        GameObject LinkedGameObject { get; }

    }
}