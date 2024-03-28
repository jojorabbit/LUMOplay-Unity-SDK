using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LUMOPlay.UIToolkit.Interfaces {
    public interface IQuery {

        IList<VisualElement> GetElements();
        void Execute();
        GameObject LinkedGameObject { get; }

    }
}