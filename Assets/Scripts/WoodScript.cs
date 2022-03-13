using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Linq;

public class WoodScript : MonoBehaviour
{
    [SerializeField] Transform ChildTransform;
    private List<string> DoorsName;
    public int modelindex;
    public int WoodPuan;
    public WoodStack transporter;
    private Collider MyCollider;
    private string scaleAnimName;
    private Models modeller;
    private GameObject Model;
    private Animation Anim;
    public ParticleSystem explosionEffect;
    private void Start()
    {
        WoodPuan = modelindex + 1;
        ChildTransform = transform.GetChild(0).transform;
        Anim = ChildTransform.GetComponent<Animation>();
        DoorsName = new List<string>();
        SpawnModel(modelindex);
        modeller = FindObjectOfType<Models>();
        scaleAnimName = "ScaleWood";
        AnimPlay(true);
        MyCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(Layers.wood))
        {
            EventManager.Event_OnWoodAdded(other.GetComponent<WoodScript>());
        }
        if((other.gameObject.layer == LayerMask.NameToLayer(Layers.obstacle)) && transporter != null)
        {
            transporter.DropWood(this);
        }
    }

    public void SpawnModel(int indexModel = 0)
    {
        if (modeller == null) modeller = FindObjectOfType<Models>();
        modelindex = indexModel;
        gameObject.tag = Tags.taglar[indexModel];
        WoodPuan = modelindex + 1;
        Model = modeller.Modeller[modelindex];
        if (!GameManager.levelFinish)
        {
            if (ChildTransform.childCount != 0) Destroy(ChildTransform.GetChild(0)?.gameObject);
        }
        GameObject model = Instantiate(Model, ChildTransform);
        model.transform.localPosition = Vector3.zero;
    }

    public void AnimPlay(bool value)
    {
        if (value)
        {
            Anim.Play("DefaultWood");
        }
        else
        {
            Anim.Stop("DefaultWood");
            ChildTransform.localRotation = Quaternion.Euler(Vector3.zero);
            ChildTransform.localPosition = Vector3.zero;
        }
    }
    public void DestRoyWood()
    {
        Instantiate(explosionEffect,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void DropStackList()
    {
        MyCollider.enabled = false;
        Invoke("EnableCollider",1f);
        Vector3 paunch = new Vector3(transform.position.x + Random.Range(2f, -2f), transform.position.y, transform.position.z + Random.Range(1f, 3f));
        transform.DOJump(paunch, 2f, 1, 1f);
    }

    private void EnableCollider()
    {
        MyCollider.enabled = true;
    }

    public void UpGrade(string name)
    {
        if (modeller == null) modeller = FindObjectOfType<Models>();

        if (!DoorsName.Contains(name))
        {
            DoorsName.Add(name);// arda arda etkile�imi engellemek i�in
            if (modelindex < modeller.Modeller.Length)
            {
                modelindex++;
                gameObject.tag = Tags.taglar[modelindex];
                WoodPuan = modelindex + 1;
                Model = modeller.Modeller[modelindex];
                if (transform.childCount != 0) Destroy(ChildTransform.GetChild(0)?.gameObject);
                GameObject model = Instantiate(Model, ChildTransform);
                model.transform.localPosition = Vector3.zero;
                AnimationScaleWood();
                EventManager.Event_OnIncreaseScore(1);
            }
        }
    }

    public void AnimationScaleWood()
    {
        Anim.Play(scaleAnimName);
    }
    

    public void ChangeMaterial(Material color)
    {
        WoodPuan++;
        Material[] mats= ChildTransform.GetChild(0).GetChild(0).GetComponent<Renderer>().materials;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = color;
        }
        ChildTransform.GetChild(0).GetChild(0).GetComponent<Renderer>().materials = mats;
        AnimationScaleWood();
        EventManager.Event_OnIncreaseScore(1);
    }

    public void Polish(Material toPolish)
    {
        WoodPuan++;
        toPolish.color = ChildTransform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color;
        ChildTransform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = new Material(toPolish);
        ChildTransform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
        AnimationScaleWood();
        EventManager.Event_OnIncreaseScore(1);
    }
}
