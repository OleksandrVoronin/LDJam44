using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public class WeakPirateEncounter : MonoBehaviour
{
    [SerializeField]
    private Button option1;
    [SerializeField]
    private Button option2;
    [SerializeField]
    private Button option3;

    [SerializeField]
    private TextMeshProUGUI phase2Text;

    [SerializeField]
    private GameObject phase2;

    private bool _shipLost = false;

    private void OnEnable()
    {
        _shipLost = false;

        GetComponent<CanvasGroup>().DOFade(1, 0.3f);

        phase2.SetActive(false);
        phase2.GetComponent<CanvasGroup>().alpha = 0;

        option1.interactable = GameManager.Instance.Fuel >= 10;
        option2.interactable = GameManager.Instance.Minerals >= 8;
        option3.interactable = GameManager.Instance.Gas >= 14;
    }

    public void Option1()
    {
        GameManager.Instance.Fuel -= 10;
        Close();
    }

    public void Option2()
    {
        GameManager.Instance.Minerals -= 8;
        Close();
    }

    public void Option3()
    {
        GameManager.Instance.Gas -= 14;
        Close();
    }

    public void Option4()
    {
        phase2.SetActive(true);
        phase2.GetComponent<CanvasGroup>().DOFade(1, 0.3f);

        int roll = Random.Range(0, 100);

        if (roll < 45 + (GameManager.Instance.Luck + GameManager.Instance.CombatAbility) * 2)
        {
            phase2Text.text = "You shot first and damaged pirate's ship's life-support system. You are able to collect some goods from him!";
            GameManager.Instance.Fuel += 5;
            GameManager.Instance.Minerals += 3;

            return;
        }
        roll -= 45 + (GameManager.Instance.Luck + GameManager.Instance.CombatAbility) * 2;

        if (roll < 35 - (GameManager.Instance.Luck + GameManager.Instance.CombatAbility))
        {
            phase2Text.text = "The pirate shot first and damaged your ship! You manage to flee but some of your cargo was lost.";
            GameManager.Instance.Minerals -= GameManager.Instance.Minerals / 5;
            GameManager.Instance.Gas -= GameManager.Instance.Gas / 5;
            return;
        }

        phase2Text.text = "Your cockpit got a direct hit. A bright flash, then emptiness.. You die yet again.";
        _shipLost = true;
    }

    public void Option5()
    {
        phase2.SetActive(true);
        phase2.GetComponent<CanvasGroup>().DOFade(1, 0.3f);

        int roll = Random.Range(0, 100);

        if (roll < 45 + (GameManager.Instance.Luck + GameManager.Instance.Evasion) * 2)
        {
            phase2Text.text = "You engaged afterburners to avoid the pirate! (-1 fuel)";
            GameManager.Instance.Fuel -= 1;

            return;
        }
        roll -= 45 + (GameManager.Instance.Luck + GameManager.Instance.Evasion) * 2;

        if (roll < 35 - (GameManager.Instance.Luck + GameManager.Instance.Evasion))
        {
            phase2Text.text = "The pirate hit your engine but you managed to escape (warp jump cost increased).";
            GameManager.Instance.ExtraLowerFuelConsumption -= 1;
            return;
        }

        phase2Text.text = "Enemy ship damaged your reactor core, you die..";
        _shipLost = true;
    }

    public void Close()
    {
        GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
        {
            if (_shipLost)
            {
                GameManager.Instance.CatLives--;
                GameManager.Instance.ShipLost();
            }
            gameObject.SetActive(false);
        });
    }
}
