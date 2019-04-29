using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField]
    private GameObject[] _globalPlaneActives;
    [SerializeField]
    private GameObject[] _starSystemActives;

    [SerializeField]
    private GameObject _inventoryView;

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
    public int Provision
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

            if(_catLives < 0)
            {
                _catLives = 0;
                EndGame(GameOverReasonEnum.Lives);
            }

            _catLivesText.text = "" + _catLives;
        }
    }

    [SerializeField]
    private int _minerals;
    [SerializeField]
    private TextMeshProUGUI _mineralsText;
    public int Minerals
    {
        get => _minerals;
        set
        {
            _minerals = value;
            _mineralsText.text = "" + _minerals;
        }
    }

    [SerializeField]
    private int _gas;
    [SerializeField]
    private TextMeshProUGUI _gasText;
    public int Gas
    {
        get => _gas;
        set
        {
            _gas = value;
            _gasText.text = "" + _gas;
        }
    }

    public float FuelWeight
    {
        get => 0.5f;
    }

    public float ProvisioningWeight
    {
        get => 0.3f;
    }

    public float MineralsWeight
    {
        get => 1f;
    }

    public float GasWeight
    {
        get => 0.6f;
    }

    [SerializeField]
    private float _shipMaxCapacity;
    public float ShipMaxCapacity
    {
        get => _shipMaxCapacity;
        set => _shipMaxCapacity = value;
    }

    public float ShipCapacity
    {
        get => Mathf.Round(FuelWeight * Fuel) + Mathf.Round(ProvisioningWeight * Provision) + Mathf.Round(MineralsWeight * Minerals) + Mathf.Round(GasWeight * Gas);
    }


    [SerializeField]
    private Image[] _shipAppearances;
    [SerializeField]
    private Sprite[] _shipVariations;

    private int _shipVariant = 0;
    public int ShipVariant
    {
        get => _shipVariant;
        set
        {
            _shipVariant = value;
            foreach (var ship in _shipAppearances)
            {
                ship.sprite = _shipVariations[_shipVariant];
            }
        }
    }

    private int _combatAbility;
    public int CombatAbility {
        get => _combatAbility;
        set => _combatAbility = value;
    }

    private int _evasion;
    public int Evasion
    {
        get => _evasion;
        set => _evasion = value;
    }

    private float _fuelConsumption;
    public float FuelConsumption
    {
        get => _fuelConsumption;
        set => _fuelConsumption = value;
    }

    private int _luck;
    public int Luck {
        get => _luck;
        set => _luck = value;
    }

    public bool LongRangeScan = false;
    public int ExtraMineralsMined = 0;
    public int ExtraGasMined = 0;
    public int ExtraLowerFuelConsumption = 0;

    public PlayerShipMover PlayerShipMover;

    [SerializeField]
    private GameObject _gameOver;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;

    [SerializeField]
    private GameObject _gameWon;

    [SerializeField]
    private GameObject _shipLost;

    public enum GameOverReasonEnum { Fuel, Provision, Lives };

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SwitchToGlobalView();

            CatLives = 8;
            Provision = 0;
            Fuel = 0;
            Minerals = 0;
            Gas = 0;

            PlayerShipMover = FindObjectOfType<PlayerShipMover>();
            Time.timeScale = 1;
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

    public void OpenInventory()
    {
        _inventoryView.SetActive(true);
    }

    public void CloseInventory()
    {
        _inventoryView.SetActive(false);
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
                Fuel = 0;
                EndGame(GameOverReasonEnum.Fuel);
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
            Provision--;

            if (Provision < 0)
            {
                Provision = 0;
                EndGame(GameOverReasonEnum.Provision);
            }

            yield return new WaitForSeconds(timeTick);
        }

        yield return null;
    }

    public void EndGame(GameOverReasonEnum gameOverReasonEnum) {
        _gameOver.SetActive(true);

        Time.timeScale = 0;

        if (gameOverReasonEnum == GameOverReasonEnum.Fuel)
            _gameOverText.text = "You ran out of fuel.. Drifting through cold and empty space you find your doom.";

        if (gameOverReasonEnum == GameOverReasonEnum.Provision)
            _gameOverText.text = "You ran out of provision.. Drifting through cold and empty space you find your doom.";

        if (gameOverReasonEnum == GameOverReasonEnum.Lives)
            _gameOverText.text = "You ran out of lives.. It is over now.";
    }

    public void GameWon() {
        _gameWon.SetActive(true);

        Time.timeScale = 0;
    }

    public void RestartGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ShipLost() {
        _shipLost.SetActive(true);
    }
}
