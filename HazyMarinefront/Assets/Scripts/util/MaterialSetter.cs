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

    public static void ChangeAlpha(Material mat, float alphaValue)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
        mat.SetColor("_Color", newColor);
    }

    public static void ChangeColor(Material mat, Color newColor)
    {
        mat.SetColor("_Color", newColor);
    }
}
