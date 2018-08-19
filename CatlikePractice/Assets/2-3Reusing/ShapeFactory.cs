using System.Collections.Generic;
using UnityEngine;

namespace Reusing
{
    [CreateAssetMenu]
    public class ShapeFactory : ScriptableObject
    {
        [SerializeField] private Shape[] _prefabs;

        [SerializeField] private Material[] _materials;

        [SerializeField] private bool _recycle;

        private List<Shape>[] _pools;

        public Shape Get(int shapeId, int materialId)
        {
            Shape instance;
            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }

                var pool = _pools[shapeId];
                var lastIndex = pool.Count - 1;
                if (lastIndex >= 0)
                {
                    instance = pool[lastIndex];
                    instance.gameObject.SetActive(true);
                    pool.RemoveAt(lastIndex);
                }
                else
                {
                    instance = Instantiate(_prefabs[shapeId]);
                    instance.ShapeId = shapeId;
                }
            }
            else
            {
                instance = Instantiate(_prefabs[shapeId]);
                instance.ShapeId = shapeId;
            }

            instance.SetMaterial(_materials[materialId], materialId);
            instance.SetColor(Random.ColorHSV(0f, 1f, 0.4f, 0.6f, 0.7f, 0.9f, 1f, 1f));

            return instance;
        }

        public Shape GetRandom()
        {
            var instance = Get(Random.Range(0, _prefabs.Length), Random.Range(0, _materials.Length));
            return instance;
        }

        public void Reclaim(Shape shapeToRecycle)
        {
            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }

                _pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
                shapeToRecycle.gameObject.SetActive(false);
            }
            else
            {
                Destroy(shapeToRecycle.gameObject);
            }
        }

        private void CreatePools()
        {
            _pools = new List<Shape>[_prefabs.Length];
            for (int i = 0; i < _pools.Length; i++)
            {
                _pools[i] = new List<Shape>();
            }
        }
    }
}