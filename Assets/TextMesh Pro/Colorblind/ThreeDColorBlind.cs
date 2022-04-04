using UnityEngine;

public class ThreeDColorBlind : MonoBehaviour
{
    public Renderer render;
    public Material colorblindMaterial;
    public Material colorblindMaterialHat;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("ToggleBool2") == 1)
        {
            render.material = colorblindMaterial;
            var materials = render.materials;
            materials[4] = colorblindMaterialHat;
            render.materials = materials;
        }
        
    }
}
