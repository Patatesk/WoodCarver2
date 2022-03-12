using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Doors : MonoBehaviour
{
    //public UnityEvent<string> UpgradeModel;
    //public UnityEvent<int> IncreaseScore;
    [SerializeField] int doorNumber;
    [SerializeField] Material material;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(Layers.collectWood))
        {
            WoodScript wood = other.GetComponent<WoodScript>();

            if((wood.gameObject.tag == Tags.taglar[0]) && doorNumber == 1)
            {
                wood.UpGrade(transform.gameObject.name);

            }
            else if ((wood.gameObject.tag == Tags.taglar[1]) && doorNumber == 2)
            {
                wood.ChangeMaterial(material);
            }
            else if((wood.gameObject.tag == Tags.taglar[2]) && doorNumber == 3)
            {
                wood.ChangeMaterial(material);
            }
        }
    }
}
