using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{

    // Config parameters
    [SerializeField] float backgroundScrollSpeed = 0.5f;

    // Cached references
    Material backgroundMaterial;
    Vector2 backgroundMaterialOffset;

	// Use this for initialization
	void Start ()
    {
        backgroundMaterial = GetComponent<Renderer>().material;

        backgroundMaterialOffset = new Vector2(0, backgroundScrollSpeed);
    }
	
	// Update is called once per frame
	void Update ()
    {
        backgroundMaterial.mainTextureOffset += backgroundMaterialOffset * Time.deltaTime;
	}
}
