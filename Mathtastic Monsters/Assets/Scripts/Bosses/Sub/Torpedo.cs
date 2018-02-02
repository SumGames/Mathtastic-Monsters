using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Torpedo : MonoBehaviour
{
    BossMonster boss;

    public string Answer;

    RectTransform rect;

    RectTransform end;

    SubtractionContainer container;

    CombatStateManager stateManager;

    float velocity;

	// Use this for initialization
	void Start ()
    {
        stateManager = FindObjectOfType<CombatStateManager>();

	}

    internal void CreateTorpedo(RectTransform a_end, BossMonster a_boss, string a_answer,SubtractionContainer subtractionContainer, float a_velocity)
    {
        container = subtractionContainer;

        Answer = a_answer;

        GetComponentInChildren<Text>().text = a_answer.ToString();

        rect = GetComponent<RectTransform>();
        end = a_end;
        boss = a_boss;

        velocity = a_velocity;
    }

	
	// Update is called once per frame
	void Update ()
    {
        if (stateManager && stateManager.gameState != playStatus.playing)
        {
            Destroy(gameObject);
        }


        if (end && rect)
        {
            rect.localPosition = Vector2.MoveTowards(rect.localPosition, end.localPosition, (Time.deltaTime * velocity));

            if (rect.localPosition.y > 2.7)
            {
                playerHit();
                return;
            }
        }
        
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Answer")
        {
            if (other.GetComponent<SubtractionDragger>().AnswerNeeded == Answer)
            {
                other.gameObject.SetActive(false);
                Destroy(gameObject);

                container.LaunchTorpedo();
            }
            else
            {
                boss.player.DamagePlayer(0.5f);
                container.ResetPosition();
            }
        }
    }


    void playerHit()
    {
        container.LaunchTorpedo();
        boss.EnemyAttack();
        Destroy(gameObject);
        return;
    }

}
