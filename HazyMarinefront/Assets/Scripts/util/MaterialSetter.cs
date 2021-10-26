using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MeshRenderer meshRenderer
    {
        get
        {
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();
            return _meshRenderer;
        }
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}
