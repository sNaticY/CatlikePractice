using UnityEngine;

namespace Variety
{
    public class Shape : PersistableObject
    {
        private int _shapeId = int.MinValue;

        public int ShapeId
        {
            get { return _shapeId; }
            set
            {
                if (_shapeId == int.MinValue)
                {
                    _shapeId = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change shapeId.");
                }
            }
        }

        public int MaterialId { get; private set; }

        public Color Color { get; private set; }
        
        private MeshRenderer _meshRenderer;
        
        private static int _colorPropertyId = Shader.PropertyToID("_Color");
        private static MaterialPropertyBlock _sharedPropertyBlock;
        
        
        void Awake () {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetMaterial(Material material, int materialId)
        {
            _meshRenderer.material = material;
            MaterialId = materialId;
        }

        public void SetColor(Color color)
        {
            if (_sharedPropertyBlock == null) {
                _sharedPropertyBlock = new MaterialPropertyBlock();
            }
            _sharedPropertyBlock.SetColor(_colorPropertyId, color);
            _meshRenderer.SetPropertyBlock(_sharedPropertyBlock);
            Color = color;
        }

        public override void Save(GameDataWriter writer)
        {
            base.Save(writer);
            writer.Write(Color);
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
            SetColor(reader.Version <= 0 ? Color.white : reader.ReadColor());
        }
    }
}