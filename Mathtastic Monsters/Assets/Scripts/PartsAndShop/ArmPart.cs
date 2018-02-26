using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmType
{
    Biped,
    FourLeg,
    None
}


public class ArmPart : ItemPart
{
    public GameObject foreArm;
    public GameObject hand;

    public ArmType armType;

    public GameObject foot;


    public GameObject FourFoot;

    public void EquipArm(TorsoPart torso, GameObject upperArmSpot, GameObject foreArmSpot,  GameObject handSpot)
    {
        upperArmSpot.transform.localScale = Scale;

        transform.SetParent(upperArmSpot.transform, false);
        if (torso.bodyType != BodyType.FourArm)
        {
            foreArm.transform.SetParent(foreArmSpot.transform, false);
            hand.transform.SetParent(handSpot.transform, false);

            foreArm.transform.localPosition = Vector3.zero;
            foreArm.transform.localRotation = Quaternion.identity;

            hand.transform.localPosition = Vector3.zero;
            hand.transform.localRotation = Quaternion.identity;
        }
        if (FourFoot)
        {
            foot.SetActive(true);

            FourFoot.SetActive(false);
        }

        if (torso.bodyType == BodyType.FourLeg)
        {
            transform.localScale += new Vector3(0, .6f, 0);
            foreArm.transform.localScale += new Vector3(0, .6f, 0);

            hand.transform.rotation = new Quaternion(0, 20, -70, 0);
            
            foreArm.transform.localPosition += new Vector3(0, .2f, 0);

            hand.transform.localPosition += new Vector3(0, -.15f, 0);

            if (FourFoot)
            {
                FourFoot.SetActive(true);
                foot.SetActive(false);
            }

            if (armType == ArmType.FourLeg)
            {
                foreArm.transform.localPosition += new Vector3(-0.036f, -0.07f, 0.045f);
            }

        }
        if (textureMaterial)
        {
            foreArm.GetComponentInChildren<Renderer>().material = textureMaterial;
            if (FourFoot && FourFoot.gameObject.activeSelf)
            {
                foreach (Renderer item in FourFoot.GetComponentsInChildren<Renderer>())
                {
                    item.material = textureMaterial;
                }
            }
            else
            {
                hand.GetComponentInChildren<Renderer>().material = textureMaterial;
            }
        }
    }

    public void deleteArm()
    {
        Destroy(foreArm);
        Destroy(hand);
        Destroy(foot);
        if (FourFoot)
        {
            Destroy(FourFoot);
        }

        Destroy(gameObject);

    }

}
