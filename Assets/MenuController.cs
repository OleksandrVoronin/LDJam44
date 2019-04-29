using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class MenuController : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetUpdate(true);
        Time.timeScale = 0;
    }

    public void QuitGame() {
        Application.Quit();
    }    

    public void Close()
    {
        GetComponent<CanvasGroup>().DOFade(0, 0.3f).SetUpdate(true).OnComplete(() => {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        });
    }
}
