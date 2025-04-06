using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BuildingSystem2D.Elements
{
    [CreateAssetMenu(menuName = "Building System/Elements Database", fileName = "Elements Database")]
    public class ElementsDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        [Serializable]
        private class IdToPrefab
        {
            public string Id;
            public Element Prefab;
        }
        
        #if UNITY_EDITOR
        [SerializeField] private List<Element> prefabs = new List<Element>();
#else
        [System.NonSerialized] private List<Element> prefabs = new List<Element>();
#endif
        
        [SerializeField, HideInInspector] private List<IdToPrefab> data = new();
        
        private Dictionary<string, Element> _dict = new();

        public void OnBeforeSerialize()
        {
            data.Clear();
            var ids = new HashSet<string>();
            for (int i = 0; i < prefabs.Count; i++)
            {
                var prefab = prefabs[i];
                if (prefab is null)
                {
                    continue;
                }
                if (ids.Contains(prefab.PrefabId))
                {
                    continue;
                }

                ids.Add(prefab.PrefabId);
                data.Add(new IdToPrefab{Id = prefab.PrefabId, Prefab = prefab});
            }
        }

        public void OnAfterDeserialize()
        {
            ResetDictionary();
        }

        private void OnValidate()
        {
            FillData();
        }

        private void FillData()
        {
            data.Clear();
            var ids = new HashSet<string>();
            for (int i = 0; i < prefabs.Count; i++)
            {
                var prefab = prefabs[i];
                if (prefab is null)
                {
                    continue;
                }
                if (ids.Contains(prefab.PrefabId))
                {
                    Debug.LogWarning("Element prefab with id \"" + prefab.PrefabId +
                                     "\" is already added to the database. Prefabs with repeating ids will be ignored.\nPrefab number in list: " +
                                     (i + 1) + ".");
                    continue;
                }

                ids.Add(prefab.PrefabId);
                data.Add(new IdToPrefab{Id = prefab.PrefabId, Prefab = prefab});
            }
        }

        private void ResetDictionary()
        {
            _dict.Clear();
            foreach (var entry in data)      
            {
                if (entry is null)
                {
                    Debug.LogError("Null entry encountered in the data list while resetting data dictionary.");
                    continue;
                }
                if (string.IsNullOrEmpty(entry.Id))
                {
                    Debug.LogError("Entry with empty id encountered while resetting data dictionary.");
                    continue;
                }
                if (_dict.ContainsKey(entry.Id))
                {
                    Debug.LogError("Entry with repeating id encountered while resetting data dictionary. Entry id: " + entry.Id + ".");
                    continue;
                }
                _dict.Add(entry.Id, entry.Prefab);
            }
        }

        public string[] GetElementsIds()
        {
            return _dict.Keys.ToArray();
        }
        
        public Element GetElement(string id)
        {
            return _dict[id];
        }
        
        public bool ContainsElement(string id)
        {
            return _dict.ContainsKey(id);
        }

        public bool TryGetElement(string id, out Element itemFile)
        {
            itemFile = default(Element);
            if (_dict.TryGetValue(id, out var value))
            {
                itemFile = value;
                return true;
            }

            return false;
        }
    }
}