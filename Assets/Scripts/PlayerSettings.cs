using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public bool isPlaying;
    public bool isDeath;
    public float ForwardSpeed;
    public float sensitivity;
    public int score;
    public int finishScore;
    public int level;
    public int howManyObjectsOpend;
    public int index = 0;
    public int levelcount = 0;
    public int TotalScore;

    public List<List<Material>> oyunSonuMats;


}
