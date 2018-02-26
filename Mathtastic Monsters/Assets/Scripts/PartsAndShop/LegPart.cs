using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegPart : ItemPart
{
    public GameObject Shin;
    public GameObject Ankle;
    public GameObject Foot;


    public void EquipLeg(GameObject upperThighSpot, GameObject ShinSpot, GameObject AnkleSpot,GameObject FootSpot)
    {

        upperThighSpot.transform.localScale = Scale;

        transform.SetParent(upperThighSpot.transform, false);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        Shin.transform.SetParent(ShinSpot.transform, false);
        Shin.transform.localRotation = Quaternion.identity;
        Shin.transform.localPosition = Vector3.zero;
        Shin.transform.localScale = Vector3.one;
        if (Ankle != null)
        {
            Ankle.transform.SetParent(AnkleSpot.transform, false);
            Ankle.transform.localRotation = Quaternion.identity;
            Ankle.transform.localPosition = Vector3.zero;
            Ankle.transform.localScale = Vector3.one;
        }
        Foot.transform.SetParent(FootSpot.transform, false);
        Foot.transform.localRotation = Quaternion.identity;
        Foot.transform.localPosition = Vector3.zero;
        Foot.transform.localScale = Vector3.one;

        if (textureMaterial)
        {
            Shin.GetComponentInChildren<Renderer>().material = textureMaterial;
            Foot.GetComponentInChildren<Renderer>().material = textureMaterial;

            if(Ankle)
            {
                Ankle.GetComponentInChildren<Renderer>().material = textureMaterial;
            }
        }
    }

    public void DeleteLeg()
    {
        Destroy(Shin);
        if (Ankle != null)
            Destroy(Ankle);
        Destroy(Foot);
        Destroy(gameObject);

    }
}
