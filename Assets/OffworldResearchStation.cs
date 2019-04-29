using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public class OffworldResearchStation : MonoBehaviour
{
    [SerializeField]
    private Button option1;
    [SerializeField]
    private Button option2;

    [SerializeField]
    private TextMeshProUGUI phase2Text;

    [SerializeField]
    private GameObject phase2;

    private void OnEnable()
    {
        GetComponent<CanvasGroup>().DOFade(1, 0.3f);

        phase2.SetActive(false);
        phase2.GetComponent<CanvasGroup>().alpha = 0;
        option1.interactable = GameManager.Instance.Minerals >= 10;
    }

    public void Option1()
    {
        GameManager.Instance.Minerals -= 10;

        phase2.SetActive(true);
        phase2.GetComponent<CanvasGroup>().DOFade(1, 0.3f);

        int roll = Random.Range(0, 100);

        if (roll < 25 + GameManager.Instance.Luck)
        {
            phase2Text.text = "A few engineers entered your ship and threw out a bunch of fur balls. Ship capacity increased +5!";
            GameManager.Instance.ShipMaxCapacity += 5;
        }
        else if (roll < 50 + GameManager.Instance.Luck * 2)
        {
            phase2Text.text = "Scientists install a new experimental mining tech on your ship! Mining efficiency improved!";
            GameManager.Instance.ExtraMineralsMined++;
            GameManager.Instance.ExtraGasMined++;
        }
        else
        {
            phase2Text.text = "After a few hours chief scientist returned apologizing for a long wait. Looks like they are in no rush to help you but offered some fuel in return (+15 fuel)";
            GameManager.Instance.Fuel += 15;

        }
    }

    public void Close()
    {
        GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }
}
