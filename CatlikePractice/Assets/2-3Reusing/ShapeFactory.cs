using UnityEngine;

namespace Reusing
{
    [CreateAssetMenu]
    public class ShapeFactory : ScriptableObject
    {
        [SerializeField] private Shape[] _prefabs;

        [SerializeField] private Material[] _materials;

        public Shape Get(int shapeId, int materialId)
        {
            var instance = Instantiate(_prefabs[shapeId]);
            instance.ShapeId = shapeId;
            instance.SetMaterial(_materials[materialId], materialId);
            instance.SetColor(Random.ColorHSV(0f, 1f, 0.4f, 0.6f, 0.7f, 0.9f, 1f, 1f));
            return instance;
        }

        public Shape GetRandom()
        {
            var instance = Get(Random.Range(0, _prefabs.Length), Random.Range(0, _materials.Length));
            return instance;
        }
    }
}