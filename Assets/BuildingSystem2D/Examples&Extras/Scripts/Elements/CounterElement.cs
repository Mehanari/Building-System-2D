using System;
using BuildingSystem2D.Elements;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace BuildingSystem2D.Examples_Extras.Scripts.Elements
{
    [RequireComponent(typeof(Collider2D))]
    public class CounterElement : Element
    {
        [Serializable]
        public class State
        {
            [JsonProperty("count")]
            public int Count { get; set; }
        }

        [SerializeField] private TextMeshPro countMesh;
    
        private State _state = new();

        private void Awake()
        {
            UpdateView();
        }

        private void OnMouseDown()
        {
            _state.Count++;
            InvokeStateUpdated();
            UpdateView();
        }

        public override void SetState(string state)
        {
            _state = JsonConvert.DeserializeObject<State>(state);
            UpdateView();
        }
        
        private void UpdateView()
        {
            countMesh.text = _state.Count.ToString();
        }

        public override string GetState()
        {
            return JsonConvert.SerializeObject(_state);
        }
    }
}
