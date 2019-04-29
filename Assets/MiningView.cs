using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiningView : MonoBehaviour
{
    public enum MiningModeEnum { Minerals, Gas };
    private MiningModeEnum _miningMode;
    public MiningModeEnum MiningMode {
        get => _miningMode;
        set {
            _miningMode = value;
            

        }
    }

    private int _minedTimes = 0;
    [SerializeField]
    private Vector2Int _resourcePerMineRange = new Vector2Int(5, 7);

    private Star.PlanetInformation _planetInformation;

    [SerializeField]
    private CanvasGroup _canvasGroupMineralsMined;
    [SerializeField]
    private CanvasGroup _canvasGroupGasMined;

    [SerializeField]
    private TextMeshProUGUI _mineralsMinedText;
    [SerializeField]
    private TextMeshProUGUI _gasMinedText;

    public void Init(MiningModeEnum miningMode, Star.PlanetInformation planetInformation) {
        MiningMode = miningMode;
        _planetInformation = planetInformation;
        _minedTimes = 0;
    }

    private bool _uiBusy = false;

    [SerializeField]
    private Button _miningButton;

    public void Update()
    {
        _miningButton.interactable = !_uiBusy;
    }

    public void Mine() {
        if (_uiBusy) return;
        _uiBusy = true;

        GameManager.Instance.SpendFuel(1, 0f);
        GameManager.Instance.SpendProvisioning(1, 0f);

        int resourcesMined = (int) Mathf.Max(_planetInformation.ResourcesRich - _minedTimes, 0) * Random.Range(_resourcePerMineRange.x, _resourcePerMineRange.y);
        _planetInformation.ResourcesRich = (int)Mathf.Max(_planetInformation.ResourcesRich - 1, 0);

        if (_miningMode == MiningModeEnum.Minerals) {
            GameManager.Instance.Minerals += resourcesMined + GameManager.Instance.ExtraMineralsMined;
            _mineralsMinedText.text = "+" + (resourcesMined + GameManager.Instance.ExtraMineralsMined);

            _canvasGroupMineralsMined.DOComplete();
            _canvasGroupMineralsMined.DOFade(1, 0.2f).OnComplete(() => _canvasGroupMineralsMined.DOFade(0, 0.1f));

            _canvasGroupMineralsMined.transform.DOComplete();
            _canvasGroupMineralsMined.transform.DOPunchScale(Vector3.one * 0.1f, 0.4f, 0, 0).OnComplete(() => _uiBusy = false);

        }

        if (_miningMode == MiningModeEnum.Gas)
        {
            GameManager.Instance.Gas += resourcesMined + GameManager.Instance.ExtraGasMined;
            _gasMinedText.text = "+" + (resourcesMined + GameManager.Instance.ExtraGasMined);

            _canvasGroupGasMined.DOComplete();
            _canvasGroupGasMined.DOFade(1, 0.2f).OnComplete(() => _canvasGroupGasMined.DOFade(0, 0.1f));

            _canvasGroupGasMined.transform.DOComplete();
            _canvasGroupGasMined.transform.DOPunchScale(Vector3.one * 0.1f, 0.4f, 0, 0).OnComplete(() => _uiBusy = false);
        }

        GetComponentInChildren<InventoryView>().UpdateView();
    }

    public void Back() {
        GameManager.Instance.Fader.DOFade(1, 0.2f).OnComplete(() => {
            gameObject.SetActive(false);
            GameManager.Instance.Fader.DOFade(0, 0.2f);
        });

        StarSystemController.Instance.ShowSelector(false, 0);
    }
}
