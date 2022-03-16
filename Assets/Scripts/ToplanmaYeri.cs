using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class ToplanmaYeri : MonoBehaviour
{
    [SerializeField] WoodScript spawnwood;
    [SerializeField] GameObject portal;
    [SerializeField] Transform spawnWoodT;
    [SerializeField] Transform woodM1;
    [SerializeField] Transform woodM2;
    [SerializeField] Transform woodM3;
    [SerializeField] Transform scoreTransform;
    public Transform positionToGo;
    bool start = false;

    List<GameObject> woodsM1 = new List<GameObject>();
    List<GameObject> woodsM2 = new List<GameObject>();
    List<GameObject> woodsM3 = new List<GameObject>();
    public List<GameObject> objectsToBuild = new List<GameObject>();
    public List<GameObject> objectsToBuildsToGo = new List<GameObject>();


    private GameManager manager;
    [SerializeField] private PlayerSettings settings;
    private WoodStack woodStack;
    private bool allowCorutine = false;
    public GameObject[] ayaklar;
    private GameObject currentObject;
    int hit;
    public int toplamAcilanObje;


    //private int InstantieModelIndex;
    //private Models modeller;
    
    void Start()
    {
        woodStack = FindObjectOfType<WoodStack>();
        manager = FindObjectOfType<GameManager>();
        hit = 0;

        //ObjectControl();

        if (settings.howManyObjectsOpend < objectsToBuild.Count)
        {
            switch (settings.howManyObjectsOpend)
            {
                case 1:
                    objectsToBuild[0].SetActive(false);
                    objectsToBuildsToGo[0].gameObject.SetActive(true);
                    objectsToBuildsToGo[0].transform.localScale = objectsToBuild[0].transform.localScale;
                    objectsToBuildsToGo[0].GetComponent<BoxCollider>().enabled = false;
                    break;
                case 2:
                    objectsToBuild[1].SetActive(false);
                    objectsToBuild[0].SetActive(false);
                    objectsToBuildsToGo[0].gameObject.SetActive(true);
                    objectsToBuildsToGo[0].transform.localScale = objectsToBuild[0].transform.localScale;
                    objectsToBuildsToGo[0].GetComponent<BoxCollider>().enabled = false;
                    objectsToBuildsToGo[1].gameObject.SetActive(true);
                    objectsToBuildsToGo[1].transform.localScale = objectsToBuild[1].transform.localScale;
                    objectsToBuildsToGo[1].GetComponent<BoxCollider>().enabled = false;
                    break;
                case 3:
                    settings.index++;
                    break;
            }
        }

        currentObject = objectsToBuildsToGo[toplamAcilanObje];

        toplamAcilanObje = settings.howManyObjectsOpend;
        positionToGo = objectsToBuildsToGo[toplamAcilanObje].transform;

        //else if (settings.howManyObjectsOpend >= objectsToBuild.Count)
        //{
        //    settings.howManyObjectsOpend = 0;
        //}
    }

    private void Update()
    {
        if (!settings.isPlaying && allowCorutine)
        {
            //objectsToBuild[0].SetActive(true);
            StartCoroutine(ObjectCreate());
           
        }
        toplamAcilanObje = settings.howManyObjectsOpend;
        Debug.Log(objectsToBuildsToGo[toplamAcilanObje].transform.name);
        if (start)
        {
            positionToGo = scoreTransform;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //if (!activePortal)
        //{
        //    InstantieModelIndex = manager.InstantieWood();
        //    PortalActive(true);
        //}

        if (other.gameObject.layer == LayerMask.NameToLayer(Layers.collectWood))
        {
            WoodScript x = other.gameObject.GetComponent<WoodScript>();
            switch (x.tagIndex)
            {
                case 0:
                    x.transporter.woods.Remove(x);
                    other.gameObject.transform.parent = null;
                    other.gameObject.transform.DOLocalMove(new Vector3(transform.position.x + 10, transform.position.y, transform.position.z), 0.5f);
                    Destroy(other.gameObject, 2);
                    //other.gameObject.transform.DORotate(new Vector3(0, 0, -90), 0.5f);
                    //woodsM1.Add(other.gameObject);
                    break;
                case 1:
                    EventManager.Event_OnLastScore(x.WoodPuan);
                    x.transporter.woods.Remove(x);
                    other.gameObject.transform.parent = null;
                    other.gameObject.transform.DOLocalMove(new Vector3(woodM2.position.x, woodM2.position.y + woodsM2.Count / 2f, woodM2.position.z), 0.5f);
                    other.gameObject.transform.DORotate(new Vector3(0, 0, -90), 0.5f);
                    woodsM2.Add(other.gameObject);
                    break;
                case 2:
                    EventManager.Event_OnLastScore(x.WoodPuan);
                    x.transporter.woods.Remove(x);
                    other.gameObject.transform.parent = null;
                    other.gameObject.transform.DOMove(new Vector3(woodM3.position.x, woodM3.position.y + woodsM3.Count / 3.33f, woodM3.position.z), 0.5f);
                    other.gameObject.transform.DORotate(new Vector3(0, 0, -90), 0.5f);
                    woodsM3.Add(other.gameObject);
                    break;
            }

            //woodStack.DropWood(wood);

            //return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            woodStack.EnableIsPlay(false);
            allowCorutine = true;
            EventManager.Event_OnCharacterAnimControl(false);  //Karakter Aniamsyon kapanmas�
        }

        //woodStack.EnableIsPlay(false);

        //InstantieteModel();


    }
    private void ChangeAyaklar(GameObject model)
    {
        for (int i = 0; i < 2; i++)
        {
            Vector3 pos = ayaklar[i].transform.position;
            Destroy(ayaklar[i].gameObject);
            ayaklar[i] = Instantiate(model, pos, Quaternion.identity);
        }
    }

    //public void InstantieteModel()
    //{
    //    modeller = FindObjectOfType<Models>();

    //    if (InstantieModelIndex >= modeller.Modeller.Length)
    //    {
    //        InstantieModelIndex = modeller.Modeller.Length - 1;
    //    }
    //    print(InstantieModelIndex);
    //    GameObject _wood = Instantiate(spawnwood.gameObject, spawnWoodT.position, Quaternion.identity);
    //    _wood.SetActive(false);
    //    WoodScript wood = _wood.GetComponent<WoodScript>();
    //    wood.SpawnModel(InstantieModelIndex);
    //    _wood.SetActive(true);

    //    ChangeAyaklar(_wood);
    //}

    bool tamamlanmadi = false;

    IEnumerator ObjectCreate()
    {
        allowCorutine = false;
        yield return new WaitForSeconds(1);
        objectsToBuildsToGo[toplamAcilanObje].SetActive(true);
        while (woodsM1.Count != 0 || woodsM2.Count != 0 || woodsM3.Count != 0)
        {
            
            if (hit < 5)
            {

                if (woodsM1.Count != 0)
                {
                    woodsM1.Last().transform.DOMove(positionToGo.position, 0.2f);
                    woodsM1.Last().transform.DORotate(new Vector3(0, 0, 90), 0.5f);
                    woodsM1.Remove(woodsM1.Last().gameObject);
                }

                if (woodsM2.Count != 0)
                {
                    woodsM2.Last().transform.DOMove(positionToGo.position, 0.2f);
                    woodsM2.Last().transform.DORotate(new Vector3(0, 0, 90), 0.5f);
                    woodsM2.Remove(woodsM2.Last().gameObject);
                }

                if (woodsM3.Count != 0)
                {
                    woodsM3.Last().transform.DOMove(positionToGo.position, 0.2f);
                    woodsM3.Last().transform.DORotate(new Vector3(0, 0, 90), 0.5f);
                    woodsM3.Remove(woodsM3.Last().gameObject);
                }
            }
            else if (hit >=5)
            {
                if (woodsM1.Count != 0)
                {
                    woodsM1.Last().transform.DOMove(scoreTransform.position, 0.2f);
                    woodsM1.Last().transform.DORotate(new Vector3(0, 0, 90), 0.5f);
                    woodsM1.Remove(woodsM1.Last().gameObject);
                }

                if (woodsM2.Count != 0)
                {
                    woodsM2.Last().transform.DOMove(scoreTransform.position, 0.2f);
                    woodsM2.Last().transform.DORotate(new Vector3(0, 0, 90), 0.5f);
                    woodsM2.Remove(woodsM2.Last().gameObject);
                }

                if (woodsM3.Count != 0)
                {
                    woodsM3.Last().transform.DOMove(scoreTransform.position, 0.2f);
                    woodsM3.Last().transform.DORotate(new Vector3(0, 0, 90), 0.5f);
                    woodsM3.Remove(woodsM3.Last().gameObject);
                }
            }
            
            yield return new WaitForSeconds(0.2f);
        }
        EventManager.Event_OnLevelFinish();
        allowCorutine = true;
    }

    public void ObjectControl()
    {
        hit += 1;
        if (hit == 5)
        {
            start = true;
            objectsToBuild[toplamAcilanObje].SetActive(false);
            positionToGo = GameObject.FindGameObjectWithTag("x").transform;
            //positionToGo.transform.GetComponent<BoxCollider>().enabled = false;
            settings.howManyObjectsOpend++;
        }
        //else if (hit < 5)
        //{
        //}
    }

}
