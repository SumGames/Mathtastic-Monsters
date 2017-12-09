//Torsos need to link up the head, arms, and leg. This class does so.

using UnityEngine;

public enum Animations
{
    Idle,
    Attack,
    Hurt,
    Dead

}

public class TorsoPart : ItemPart
{

    public GameObject neckForHead;

    public GameObject LeftArmUpper;
    public GameObject LeftArmFore;
    public GameObject LeftArmHand;


    public GameObject RightArmUpper;
    public GameObject RightArmFore;
    public GameObject RightArmHand;



    public GameObject RightUpperThigh;
    public GameObject Rightshin;
    public GameObject RightAnkle;
    public GameObject RightFoot;

    public GameObject LeftUpperThigh;
    public GameObject Leftshin;
    public GameObject LefttAnkle;
    public GameObject LeftFoot;


    public Animator bodyAnimator;


    // Use this for initialization
    void Start ()
    {

        if (textureMaterial != null)
        {

            renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer item in renderers)
            {
                item.material = textureMaterial;
            }

        }


        bodyAnimator = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void Animate(Animations a_anim)
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = new Quaternion(0, 0, 0, 0);

        string anim = "";

        switch (a_anim)
        {
            case Animations.Idle:
                anim = "Idle";
                break;
            case Animations.Attack:
                anim = "Attack_Anim";
                break;
            case Animations.Hurt:
                anim = "Hit_Anim";
                break;
            case Animations.Dead:
                anim = "Death_Anim";
                break;
            default:
                return;
        }
        bodyAnimator.Play(anim, -1, 0);

    }
}
