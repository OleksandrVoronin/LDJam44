using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhabitedView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Back()
    {
        GameManager.Instance.Fader.DOFade(1, 0.2f).OnComplete(() => {
            gameObject.SetActive(false);
            GameManager.Instance.Fader.DOFade(0, 0.2f);
        });
    }
}
