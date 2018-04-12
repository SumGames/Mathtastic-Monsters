using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinShard : MonoBehaviour
{
    RectTransform rect;

    RectTransform end;

    public bool fired;

    public float distance;

    float speedGiven;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (fired)
        {

            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, end.anchoredPosition, (Time.deltaTime * 100 * speedGiven));

            distance = Vector3.Distance(rect.anchoredPosition, end.anchoredPosition);

            if (distance < 0.01f)
            {
                FindObjectOfType<HeaderGUI>().incrementShards();
                Destroy(gameObject);
            }
        }
    }

    public void FireShard(RectTransform a_end, float a_speed)
    {
        speedGiven = a_speed;

        rect = GetComponent<RectTransform>();

        fired = true;

        end = a_end;

        rect.localScale = new Vector3(1, 1, 1);
    }

}
