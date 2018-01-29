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

    public void EquipArm(TorsoPart torso, GameObject upperArmSpot, GameObject foreArmSpot,  GameObject handSpot)
    {
        upperArmSpot.transform.localScale = Scale;

        transform.SetParent(upperArmSpot.transform, false);
        foreArm.transform.SetParent(foreArm.transform, false);
        hand.transform.SetParent(hand.transform, false);

        if (torso.bodyType == BodyType.FourLeg && armType == ArmType.Biped)
        {
            transform.localScale = new Vector3(1, 1.4f, 1);
            foreArm.transform.localScale = new Vector3(1, 1.4f, 1);
            hand.transform.rotation = new Quaternion(0, 20, -70, 0);
            hand.transform.localPosition += new Vector3(0, .1f, 0);
        }
    }

    public void deleteArm()
    {
        Destroy(foreArm);
        Destroy(hand);
        Destroy(gameObject);

    }

}
