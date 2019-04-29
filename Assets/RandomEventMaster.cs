using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventMaster : MonoBehaviour
{

    public static RandomEventMaster Instance;

    [SerializeField]
    private GameObject[] _commonEvents;

    [SerializeField]
    private GameObject[] _uncommonEvents;

    [SerializeField]
    private GameObject[] _rareEvents;



    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RollEvent()
    {
        Random.InitState((int) (Time.time * 100));
        int roll = Random.Range(0, 100);

        if (roll < 25)
        {
            _commonEvents[Random.Range(0, _commonEvents.Length)].SetActive(true);
            return;
        }

        roll = Random.Range(0, 100);

        if (roll < 15)
        {
            _uncommonEvents[Random.Range(0, _uncommonEvents.Length)].SetActive(true);
            return;
        }

        roll = Random.Range(0, 100);

        if (roll < 5)
        {
            _rareEvents[Random.Range(0, _rareEvents.Length)].SetActive(true);
            return;
        }

    }

}
