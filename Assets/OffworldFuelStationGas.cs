using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffworldFuelStationGas : MonoBehaviour
{

    [SerializeField]
    private Button option1;
    [SerializeField]
    private Button option2;
    [SerializeField]
    private Button option3;
    [SerializeField]
    private Button option4;

    private void OnEnable()
    {
        GetComponent<CanvasGroup>().DOFade(1, 0.3f);

        option1.interactable = GameManager.Instance.Gas >= 15;
        option2.interactable = GameManager.Instance.Gas >= 10;
        option3.interactable = GameManager.Instance.Gas >= 5;
    }

    public void Option1() {
        GameManager.Instance.Gas -= 15;
        GameManager.Instance.Fuel += 30;
        Option4();
    }

    public void Option2()
    {
        GameManager.Instance.Gas -= 10;
        GameManager.Instance.Fuel += 20;
        Option4();
    }

    public void Option3()
    {
        GameManager.Instance.Gas -= 5;
        GameManager.Instance.Fuel += 10;
        Option4();
    }

    public void Option4()
    {
        GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }
}
