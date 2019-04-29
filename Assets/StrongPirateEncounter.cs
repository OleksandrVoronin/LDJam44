using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public class StrongPirateEncounter : MonoBehaviour
{
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
    }

    public void Option1()
    {
        GameManager.Instance.Fuel -= GameManager.Instance.Fuel/2;
        GameManager.Instance.Provision -= GameManager.Instance.Provision / 2;
        GameManager.Instance.Minerals -= GameManager.Instance.Minerals / 2;
        GameManager.Instance.Gas -= GameManager.Instance.Gas / 2;

        Close();
    }

    public void Option2()
    {
        phase2.SetActive(true);
        phase2.GetComponent<CanvasGroup>().DOFade(1, 0.3f);

        int roll = Random.Range(0, 100);

        if (roll < (GameManager.Instance.Luck + GameManager.Instance.CombatAbility))
        {
            phase2Text.text = "You shot a volley with impressive precision and damaged pirate's ship engines and weapons before they could react! Pirates retreat leaving you some resources.";
            GameManager.Instance.Fuel += 20;
            GameManager.Instance.Minerals += 15;

            return;
        }
        roll -= (GameManager.Instance.Luck + GameManager.Instance.CombatAbility);

        if (roll < (GameManager.Instance.Luck + GameManager.Instance.CombatAbility))
        {
            phase2Text.text = "You shot a volley with impressive precision and snapped the enemy ship in two pieces. They somehow performed a warp jump with one half, but the other is yours to loot.";
            GameManager.Instance.Provision += 30;
            GameManager.Instance.Gas += 25;
            return;
        }

        phase2Text.text = "Your decision proved your stupidity. You had no chance to stand against force like this. You die yet again.";
        _shipLost = true;
    }

    public void Option3()
    {
        phase2.SetActive(true);
        phase2.GetComponent<CanvasGroup>().DOFade(1, 0.3f);

        int roll = Random.Range(0, 100);

        if (roll < 30 + (GameManager.Instance.Luck + GameManager.Instance.Evasion) * 2)
        {
            phase2Text.text = "You engaged afterburners to avoid the pirate! (-2 fuel)";
            GameManager.Instance.Fuel -= 2;

            return;
        }
        roll -= 30 + (GameManager.Instance.Luck + GameManager.Instance.Evasion) * 2;

        if (roll < 50 - (GameManager.Instance.Luck + GameManager.Instance.Evasion))
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
