using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Torpedo : MonoBehaviour
{
    BossMonster boss;

    public string Answer;

    RectTransform rect;

    RectTransform start;

    RectTransform end;


    SubtractionContainer container;

    CombatStateManager stateManager;

    float velocity;

    bool bounce;

    // Use this for initialization
    void Start()
    {
        stateManager = FindObjectOfType<CombatStateManager>();

    }

    internal void CreateTorpedo(RectTransform a_start, RectTransform a_end, BossMonster a_boss, string a_answer, SubtractionContainer subtractionContainer, float a_velocity)
    {
        start = a_start;

        container = subtractionContainer;

        Answer = a_answer;

        GetComponentInChildren<Text>().text = a_answer.ToString();

        rect = GetComponent<RectTransform>();
        end = a_end;
        boss = a_boss;

        velocity = a_velocity;
    }


    // Update is called once per frame
    void Update()
    {
        if (stateManager && stateManager.gameState != playStatus.playing)
        {
            Destroy(gameObject);
        }
        if (end && rect)
        {

            if (!bounce)
            {
                rect.localPosition = Vector2.MoveTowards(rect.localPosition, end.localPosition, (Time.deltaTime * velocity));


                if (Vector3.Distance(transform.localPosition, end.localPosition) < 1)
                {
                    playerHit();
                    return;
                }
            }
            else
            {

                rect.localPosition = Vector2.MoveTowards(rect.localPosition, start.localPosition, (Time.deltaTime * 2));

                if (Vector3.Distance(transform.localPosition, start.localPosition) < 1)
                {
                    boss.MonsterHurt();
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Answer" && !bounce)
        {
            if (other.GetComponent<SubtractionDragger>().AnswerNeeded == Answer)
            {
                other.gameObject.SetActive(false);

                bounce = true;
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
        boss.EnemyAttack();
        Destroy(gameObject);
        return;
    }
}