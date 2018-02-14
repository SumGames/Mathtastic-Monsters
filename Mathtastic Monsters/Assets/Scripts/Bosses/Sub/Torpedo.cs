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

    public GameObject bubbleSprite;

    // Use this for initialization
    void Start()
    {
        stateManager = FindObjectOfType<CombatStateManager>();
        bubbleSprite.SetActive(false);
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

                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, end.anchoredPosition, (Time.deltaTime * velocity));


                if (Vector3.Distance(rect.anchoredPosition, end.anchoredPosition) < 0.5f)
                {
                    playerHit();
                    return;
                }
            }
            else
            {
                Vector3 compassRotation = rect.transform.eulerAngles;
                compassRotation.z -= Time.deltaTime * 200;
                rect.transform.eulerAngles = compassRotation;

                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, start.anchoredPosition, (Time.deltaTime * velocity));

                if (Vector3.Distance(rect.anchoredPosition, start.anchoredPosition) < 0.5f)
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

                velocity = (Vector2.Distance(rect.anchoredPosition, start.anchoredPosition));
                bubbleSprite.SetActive(true);
            }
            else
            {
                boss.player.DamagePlayer(0.5f);
                container.ResetPosition(true);
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