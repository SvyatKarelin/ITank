using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour 
{
    private int PrevLifes;

    private void Update()
    {
        int Lifes = FindAnyObjectByType<GameManager>().PlayerLifesCount;
        for(int Indicator = 0; Indicator < transform.childCount; Indicator++) transform.GetChild(Indicator).gameObject.SetActive(Indicator < Lifes);
        PrevLifes = Lifes;
    }
}
