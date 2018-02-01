using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialScript : MonoBehaviour
{

    public Material newMaterial;

	// Use this for initialization
	void Start ()
    {
        if (newMaterial != null)
        {
            foreach (Renderer item in GetComponentsInChildren<Renderer>())
            {
                item.material = newMaterial;

            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
