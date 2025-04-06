using System.Collections.Generic;
using BuildingSystem2D.Core;
using BuildingSystem2D.Core.Algorithms;
using BuildingSystem2D.Core.Restrictions.AttachRestrictions;
using BuildingSystem2D.Core.Restrictions.RemoveRestrictions;
using BuildingSystem2D.Elements;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystem2D.Examples_Extras.Scripts
{
    public class ConstructionGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2 coordinatesScale; //Multiplier for blocks coordinates.
        [SerializeField] private GameObject defaultBlockPrefab;
        [SerializeField] private Transform pivot;
        [SerializeField] private ElementsDatabase database;
        [SerializeField] private RectTransform elementSelectionButtons;
        [SerializeField] private Button buttonPrefab;

        private readonly List<GameObject> _blocks = new();
        private Camera _camera;
        private Construction _construction;
        private Builder _builder;

        private Element _selectedElement;

        private void Awake()
        {
            _camera = Camera.main;
            _construction = new Construction();
            var root = new Block { Position = Vector2.zero };
            if (ConstructionIO.Exists())
            {
                _construction = ConstructionIO.Load();
                _construction.TryGetBlock(Vector2.zero, out root);
            }
            else
            {
                _construction.AddBlock(root);
            }
            _builder = new Builder(_construction)
            {
                AttachRestrictions = new List<AttachRestriction>
                {
                    new NoIntersectionsRestriction(new Vector2(1, 1), _construction),
                    new CrossAdjacencyRestriction(_construction, new Vector2(1, 1), new Vector2(Vector2.kEpsilon, Vector2.kEpsilon), 
                        new PositionsAttachRestriction(Vector2.kEpsilon, new Vector2(0, 0)))
                },
                RemoveRestrictions = new List<RemoveRestriction>
                {
                    new ConnectivityRestriction(root, new ConstructionGraph(_construction, new Vector2(1, 1)))
                }
            };
        }
        
        private void Start()
        {
            UpdateConstruction();
            GenerateUI();
        }

        private void GenerateUI()
        {
            foreach (var id in database.GetElementsIds())
            {
                var button = Instantiate(buttonPrefab, elementSelectionButtons);
                var textMeshPro = button.GetComponentInChildren<TextMeshProUGUI>();
                textMeshPro.text = id;
                button.onClick.AddListener(() => OnElementSelected(id));
            }
        }

        private void OnElementSelected(string id)
        {
            _selectedElement = database.GetElement(id);
        }

        private void OnDestroy()
        {
            ConstructionIO.Save(_construction);
        }



        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pos = ToConstructionCoordinates(Input.mousePosition);
                var blockPos = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
                var block = new Block { Position = blockPos};
                if (_selectedElement is not null)
                {
                    block.Contents = new List<Content>
                    {
                        new Content
                        {
                            PrefabId = _selectedElement.PrefabId,
                            Data = _selectedElement.GetState()
                        }
                    };
                }
                if (_builder.TryAttach(block))
                {
                    UpdateConstruction();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                var pos = ToConstructionCoordinates(Input.mousePosition);
                var posInt = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
                if (_construction.TryGetBlock(posInt, out var block))
                {
                    if (_builder.TryRemove(block))
                    {
                        UpdateConstruction();
                    }
                }
            }
        }

        private Vector2 ToConstructionCoordinates(Vector3 screenPosition)
        {
            var worldCoordinates = _camera.ScreenToWorldPoint(screenPosition);
            var localCoordinates = pivot.worldToLocalMatrix.MultiplyPoint(worldCoordinates);
            var unscaledCoordinates = new Vector2(localCoordinates.x / coordinatesScale.x,
                localCoordinates.y / coordinatesScale.y);
            return unscaledCoordinates;
        }

        private void UpdateConstruction()
        {
            foreach (var block in _blocks)
            {
                Destroy(block);
            }
            _blocks.Clear();
            
            foreach (var block in _construction.Blocks)
            {
                var localPos = new Vector2(block.Position.x * coordinatesScale.x, block.Position.y * coordinatesScale.y);
                var blockRoot = Instantiate(defaultBlockPrefab, Vector2.zero, Quaternion.identity);
                blockRoot.transform.SetParent(pivot, worldPositionStays: false);
                blockRoot.transform.localPosition = localPos;
                foreach (var content in block.Contents)
                {
                    var prefab = database.GetElement(content.PrefabId);
                    var state = content.Data;
                    var element = Instantiate(prefab, Vector2.zero, quaternion.identity);
                    element.SetState(state);
                    element.StateUpdated += (s) =>
                    {
                        content.Data = s;
                    };
                    var elementTransform = element.transform;
                    elementTransform.SetParent(blockRoot.transform, worldPositionStays: false);
                    elementTransform.localPosition = Vector3.zero;
                }
                _blocks.Add(blockRoot);
            }
        }
    }
}
