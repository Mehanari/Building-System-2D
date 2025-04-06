using System;
using BuildingSystem2D.Elements;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem2D.Examples_Extras.Scripts.Elements
{
    public class SliderElement : Element
    {
        [Serializable]
        public class State
        {
            [JsonProperty("sliderValue")]
            public float SliderValue;
        }

        [SerializeField] private Slider slider;

        private State _state = new();

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            _state.SliderValue = value;
            InvokeStateUpdated();
        }

        public override void SetState(string state)
        {
            _state = JsonConvert.DeserializeObject<State>(state);
            slider.value = _state.SliderValue;
        }

        public override string GetState()
        {
            return JsonConvert.SerializeObject(_state);
        }
    }
}