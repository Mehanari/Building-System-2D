using System.Collections.Generic;
using BuildingSystem2D.Core;
using BuildingSystem2D.Core.Algorithms;
using BuildingSystem2D.Core.Restrictions.AttachRestrictions;
using BuildingSystem2D.Core.Restrictions.RemoveRestrictions;
using UnityEngine;

namespace BuildingSystem2D.Examples_Extras.Scripts
{
    public class ConstructionGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2 coordinatesScale; //Multiplier for blocks coordinates.
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private Transform pivot;

        private readonly List<GameObject> _blocks = new();
        private Camera _camera;
        private Construction _construction;
        private Builder _builder;

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

        private void OnDestroy()
        {
            ConstructionIO.Save(_construction);
        }

        private void Start()
        {
            UpdateView();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pos = ToConstructionCoordinates(Input.mousePosition);
                var posInt = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
                var block = new Block { Position = posInt };
                if (_builder.TryAttach(block))
                {
                    UpdateView();
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
                        UpdateView();
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

        private void UpdateView()
        {
            foreach (var block in _blocks)
            {
                Destroy(block);
            }
            _blocks.Clear();
            
            foreach (var block in _construction.Blocks)
            {
                var localPos = new Vector2(block.Position.x * coordinatesScale.x, block.Position.y * coordinatesScale.y);
                var go = Instantiate(blockPrefab, Vector2.zero, Quaternion.identity);
                go.transform.SetParent(pivot, worldPositionStays: false);
                go.transform.localPosition = localPos;
                _blocks.Add(go);
            }
        }
    }
}
