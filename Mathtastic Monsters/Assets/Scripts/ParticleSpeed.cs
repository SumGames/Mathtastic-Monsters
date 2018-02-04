using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    public Vector3 speed;

	void Start ()
    {
		
	}

	void Update ()
    {
        transform.Rotate(speed * Time.deltaTime);
	}
}
