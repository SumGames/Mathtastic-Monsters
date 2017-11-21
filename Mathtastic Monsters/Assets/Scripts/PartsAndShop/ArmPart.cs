using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPart : ItemPart
{
    public GameObject foreArm;
    public GameObject hand;

    public void EquipArm(GameObject upperArmSpot, GameObject foreArmSpot,  GameObject handSpot)
    {
        upperArmSpot.transform.localScale = Scale;

        transform.SetParent(upperArmSpot.transform, false);
        foreArm.transform.SetParent(foreArm.transform, false);
        hand.transform.SetParent(hand.transform, false);
    }

    public void deleteArm()
    {
        Destroy(foreArm);
        Destroy(hand);
        Destroy(gameObject);

    }

}
