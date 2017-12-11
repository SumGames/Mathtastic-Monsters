using UnityEngine;


//Class is set as a public variable on items, as it'll tell players what they need to win to unlock them.
public enum classType
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Calculi,
    None,
}

public class ItemPart : MonoBehaviour
{
    public Vector3 Scale = new Vector3(1, 1, 1);


    //Can we equip this item?
    public bool owned;

    //How many shards will it cost.
    public int cost;



    public abilityTypes ability;

    internal Renderer[] renderers;

    public Material textureMaterial;

    public int starRequired;

    private void Start()
    {        

        if (textureMaterial != null)
        {

            renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer item in renderers)
            {
                item.material = textureMaterial;
            }

        }
    }
}
