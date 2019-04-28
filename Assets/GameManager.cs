using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField]
    private GameObject[] _globalPlaneActives;
    [SerializeField]
    private GameObject[] _starSystemActives;

    [SerializeField]
    private Image _fader;
    public Image Fader
    {
        get => _fader;
    }

    private bool _fading = false;


    [SerializeField]
    private int _fuel;
    [SerializeField]
    private TextMeshProUGUI _fuelText;
    public int Fuel
    {
        get => _fuel;
        set
        {
            _fuel = value;
            _fuelText.text = "" + _fuel;
        }
    }

    [SerializeField]
    private int _provisioning;
    [SerializeField]
    private TextMeshProUGUI _provisioningText;
    public int Provisioning
    {
        get => _provisioning;
        set
        {
            _provisioning = value;
            _provisioningText.text = "" + _provisioning;
        }
    }

    [SerializeField]
    private int _catLives;
    [SerializeField]
    private TextMeshProUGUI _catLivesText;
    public int CatLives
    {
        get => _catLives;
        set
        {
            _catLives = value;
            _catLivesText.text = "" + _catLives;
        }
    }

    [SerializeField]
    private int _minerals;
    public int Minerals
    {
        get => _minerals;
        set
        {
            _minerals = value;
        }
    }

    [SerializeField]
    private int _gas;
    public int Gas
    {
        get => _gas;
        set
        {
            _gas = value;
        }
    }

    public float FuelWeight
    {
        get => 0.6f;
    }

    public float ProvisioningWeight
    {
        get => 0.4f;
    }

    public float MineralsWeight
    {
        get => 1.5f;
    }

    public float GasWeight
    {
        get => 0.7f;
    }

    [SerializeField]
    private float _shipMaxCapacity;
    public float ShipMaxCapacity {
        get => _shipMaxCapacity;
        set => _shipMaxCapacity = value;
    }

    public float ShipCapacity {
        get => Mathf.Round(FuelWeight * Fuel) + Mathf.Round(ProvisioningWeight * Provisioning) + Mathf.Round(MineralsWeight * Minerals) + Mathf.Round(GasWeight * Gas);
    }


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SwitchToGlobalView();

            CatLives = 9;
            Provisioning = 30;
            Fuel = 35;
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }

    }

    [Button]
    public void SwitchToGlobalView()
    {
        if (_fading) return;
        _fading = true;

        _fader.DOFade(1, 0.2f).OnComplete(() =>
        {
            foreach (var go in _globalPlaneActives)
            {
                go.SetActive(true);
            }

            foreach (var go in _starSystemActives)
            {
                go.SetActive(false);
            }

            _fader.DOFade(0, 0.2f).OnComplete(() => _fading = false);
        });

        StarSystemController.Instance.Reset();
    }

    [Button]
    public void SwitchToStarSystemView()
    {
        if (_fading) return;
        _fading = true;

        _fader.DOFade(1, 0.2f).OnComplete(() =>
        {
            foreach (var go in _globalPlaneActives)
            {
                go.SetActive(false);
            }

            foreach (var go in _starSystemActives)
            {
                go.SetActive(true);
            }

            _fader.DOFade(0, 0.2f).OnComplete(() => _fading = false);
        });
    }

    public void SpendFuel(int amount, float time)
    {
        StartCoroutine(SpendFuelCoroutine(amount, time));
    }

    private IEnumerator SpendFuelCoroutine(int amount, float time)
    {
        float timeTick = time / amount;

        for (int i = 0; i < amount; i++)
        {
            Fuel--;

            if (Fuel < 0)
            {
                Debug.Log("Fuel ran out!");
            }

            yield return new WaitForSeconds(timeTick);
        }

        yield return null;
    }

    public void SpendProvisioning(int amount, float time)
    {
        StartCoroutine(SpendProvisioningCoroutine(amount, time));
    }

    private IEnumerator SpendProvisioningCoroutine(int amount, float time)
    {
        float timeTick = time / amount;

        for (int i = 0; i < amount; i++)
        {
            Provisioning--;

            if (Fuel < 0)
            {
                Debug.Log("Provisioning ran out!");
            }

            yield return new WaitForSeconds(timeTick);
        }

        yield return null;
    }


}
