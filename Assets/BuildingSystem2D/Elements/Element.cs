using System;
using UnityEngine;

namespace BuildingSystem2D.Elements
{
    /// <summary>
    /// Use this class to create building blocks with different states and behaviours to be used in your construction.
    /// Encode and decode state of your elements as strings (as Jsons, for example).
    /// 
    /// You can use it differently, of course, but the one mentioned above is an intended usage. 
    /// </summary>
    public abstract class Element : MonoBehaviour
    {
        [SerializeField] private string prefabPrefabId;

        public string PrefabId => prefabPrefabId;

        public event Action<string> StateUpdated;

        protected void InvokeStateUpdated()
        {
            StateUpdated?.Invoke(GetState());
        }
        
        public abstract void SetState(string state);
        public abstract string GetState();
    }
}