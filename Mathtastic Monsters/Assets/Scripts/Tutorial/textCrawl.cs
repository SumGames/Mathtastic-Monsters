using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textCrawl : MonoBehaviour
{
    int startingHeight = -700;
    int endHeight = 700;
    public float speed;

    float currentHeight;

    // Use this for initialization
    void Start()
    {
        transform.localPosition = new Vector3(0, startingHeight, 0);
        currentHeight = startingHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHeight >= endHeight)
        {
            FindObjectOfType<StepManager>().ProgressTutorial();
            Destroy(gameObject);
            return;
        }
        currentHeight = transform.localPosition.y;
        currentHeight += Time.deltaTime * speed;
        transform.localPosition = new Vector3(0, currentHeight, 0);
    }
}