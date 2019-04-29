using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarSystemController : MonoBehaviour
{
    public static StarSystemController Instance;

    private Star star;

    [SerializeField]
    private SpriteRenderer _star;

    [SerializeField]
    private SpriteRenderer _starGlow;

    [SerializeField]
    private GameObject[] _planets;
    [SerializeField]
    private GameObject[] _orbits;

    [SerializeField]
    private int _activePlanets = 3;
    public int ActivePlanets
    {
        get => _activePlanets;
        set
        {
            _activePlanets = value;
            for (int i = 0; i < _planets.Length; i++)
            {
                _planets[i].SetActive(i < _activePlanets);
                _orbits[i].SetActive(i < _activePlanets);
            }
        }
    }

    [SerializeField]
    private TextMeshProUGUI _systemNameText;

    [SerializeField]
    private SpriteRenderer _selectionIndicator;
    [SerializeField]
    private CanvasGroup _selectionUICanvasGroup;

    [SerializeField]
    private Vector2 _planetSizeRandomRange = new Vector2(0.8f, 1.2f);
    [SerializeField]
    private Vector2 _starSizeRandomRange = new Vector2(0.9f, 1.1f);

    private float _planetInitScale;
    private float _starInitScale;
    private float _starGlowInitScale;

    private float _selectionIndicatorInitScale;

    private GameObject _selectedPlanet;
    private bool _selectionBusy;

    [SerializeField]
    private LayerMask _selectorLayer;

    private float _animationSpeed = 0.15f;

    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _typeText;
    [SerializeField]
    private TextMeshProUGUI _inhabitedText;
    [SerializeField]
    private TextMeshProUGUI _resourcesText;

    [SerializeField]
    private CanvasGroup _scanButton;

    [SerializeField]
    private GameObject _MiningScreen;
    [SerializeField]
    private GameObject _InhabitedScreen;

    [SerializeField]
    private Image[] _planetBackgrounds;
    [SerializeField]
    private Image[] _planetAliens;

    [SerializeField]
    private Sprite[] _icePlanetBackgrounds;
    [SerializeField]
    private Sprite[] _gasPlanetBackgrounds;
    [SerializeField]
    private Sprite[] _terrestrialPlanetBackgrounds;
    [SerializeField]
    private Sprite[] _terrestrialPlanetAliens;

    [SerializeField]
    private Sprite[] _starSprites;
    [SerializeField]
    private Color[] _starColors;

    [SerializeField]
    private SpriteRenderer _starSpriteRenderer;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _planetInitScale = _planets[0].transform.localScale.x;
            _starInitScale = _star.transform.localScale.x;
            _starGlowInitScale = _starGlow.transform.localScale.x;

            _selectionIndicatorInitScale = _selectionIndicator.transform.localScale.x;
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject selectedPlanet = null;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 worldSpaceClickPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            RaycastHit2D raycastHit2D = Physics2D.Raycast(worldSpaceClickPosition, worldSpaceClickPosition + new Vector3(0, 0, 1), 999, _selectorLayer);

            if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.GetComponent<Planet>() != null)
            {
                selectedPlanet = raycastHit2D.collider.gameObject;
            }

            if (selectedPlanet != _selectedPlanet)
            {
                if (!_selectionBusy)
                {
                    if (selectedPlanet == null)
                    {
                        //_selectionBusy = true;
                        //_currentSelection = null;
                        //HideSelector(true, 0f);
                    }
                    else
                    {
                        _selectionBusy = true;

                        _selectedPlanet = selectedPlanet;

                        HideSelector(false, 0);
                        _selectionIndicator.transform.DOMove(_selectedPlanet.transform.position, _animationSpeed).SetDelay(_animationSpeed);
                        ShowSelector(true, _animationSpeed * 2);
                    }
                }
            }
        }
    }

    [Button]
    public void Randomize(Star star)
    {
        this.star = star;
        _systemNameText.text = star.Name;
        ActivePlanets = star.NumberOfPlanets;

        Random.InitState(star.Name.GetHashCode());

//_starSpriteRenderer

        float _starRandomScaleFactor = Random.Range(_starSizeRandomRange.x, _starSizeRandomRange.y);

        _star.transform.localScale = Vector3.one * _starInitScale * _starRandomScaleFactor;
        _starGlow.transform.localScale = Vector3.one * _starGlowInitScale * _starRandomScaleFactor;

        for (int i = 0; i < _planets.Length; i++)
        {
            _planets[i].transform.localScale = Vector3.one * _planetInitScale * Random.Range(_planetSizeRandomRange.x, _planetSizeRandomRange.y);

            float distanceToStar = (_planets[i].transform.position - _star.transform.position).magnitude;
            Vector3 randomVector = new Vector3(0, 0);

            while (randomVector.magnitude == 0)
            {
                randomVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * distanceToStar;
            }

            _planets[i].transform.position = _star.transform.position + randomVector;
        }
    }

    public void Reset()
    {
        HideSelector(true, 0);
        _selectedPlanet = null;
    }

    public void Scan()
    {
        HideSelector(false, 0);
        _selectionBusy = true;
        star.Planets[IndexOf(_planets, _selectedPlanet)].Scanned = true;
        ShowSelector(true, _animationSpeed);

        GameManager.Instance.SpendFuel(0, 0f);
        GameManager.Instance.SpendProvisioning(1, 0f);
    }

    public void Land()
    {
        Star.PlanetInformation.PlanetTypeEnum selectedPlanetType = star.Planets[IndexOf(_planets, _selectedPlanet)].PlanetType;

        switch (selectedPlanetType) {
            case Star.PlanetInformation.PlanetTypeEnum.Gas:
                GameManager.Instance.Fader.DOFade(1, 0.2f).OnComplete(() => {
                    _MiningScreen.SetActive(true);
                    _MiningScreen.GetComponent<MiningView>().Init(MiningView.MiningModeEnum.Gas, star.Planets[IndexOf(_planets, _selectedPlanet)]);
                    GameManager.Instance.Fader.DOFade(0, 0.2f);

                    foreach (var bg in _planetBackgrounds) {
                        bg.sprite = _gasPlanetBackgrounds[star.Planets[IndexOf(_planets, _selectedPlanet)].Variant % _gasPlanetBackgrounds.Length];
                    }
                });
                break;
            case Star.PlanetInformation.PlanetTypeEnum.Minerals:
                GameManager.Instance.Fader.DOFade(1, 0.2f).OnComplete(() => {
                    _MiningScreen.SetActive(true);
                    _MiningScreen.GetComponent<MiningView>().Init(MiningView.MiningModeEnum.Minerals, star.Planets[IndexOf(_planets, _selectedPlanet)]);
                    GameManager.Instance.Fader.DOFade(0, 0.2f);

                    foreach (var bg in _planetBackgrounds)
                    {
                        bg.sprite = _icePlanetBackgrounds[star.Planets[IndexOf(_planets, _selectedPlanet)].Variant % _icePlanetBackgrounds.Length];
                    }
                });
                break;
            case Star.PlanetInformation.PlanetTypeEnum.Inhabited:
                GameManager.Instance.Fader.DOFade(1, 0.2f).OnComplete(() => {
                    _InhabitedScreen.SetActive(true);
                    _InhabitedScreen.GetComponent<InhabitedView>().Init(star.Planets[IndexOf(_planets, _selectedPlanet)]);
                    GameManager.Instance.Fader.DOFade(0, 0.2f);

                    foreach (var bg in _planetBackgrounds)
                    {
                        bg.sprite = _terrestrialPlanetBackgrounds[star.Planets[IndexOf(_planets, _selectedPlanet)].Variant % _terrestrialPlanetBackgrounds.Length];
                    }
                    foreach (var bg in _planetAliens)
                    {
                        bg.sprite = _terrestrialPlanetAliens[star.Planets[IndexOf(_planets, _selectedPlanet)].Variant % _terrestrialPlanetAliens.Length];
                    }
                });
                break;
        }

        GameManager.Instance.SpendFuel(1, 0.1f);
        GameManager.Instance.SpendProvisioning(1, 0.1f);
    }

    [Button]
    private void HideSelector(bool resetBusy, float delay)
    {
        _scanButton.DOFade(0, _animationSpeed).SetDelay(delay);
        _selectionIndicator.DOFade(0, _animationSpeed).SetDelay(delay);
        _selectionUICanvasGroup.DOFade(0, _animationSpeed).SetDelay(delay).OnComplete(() =>
        {
            if (resetBusy)
            {
                _selectionBusy = false;
            }
        });
    }

    [Button]
    public void ShowSelector(bool resetBusy, float delay)
    {
        _selectionIndicator.DOFade(1, _animationSpeed).SetDelay(delay).OnStart(() =>
        {
            if (star.Planets[IndexOf(_planets, _selectedPlanet)].Scanned || GameManager.Instance.LongRangeScan)
            {
                _scanButton.DOFade(0, _animationSpeed);

                _nameText.text = "Planet: " + star.Planets[IndexOf(_planets, _selectedPlanet)].Name;
                _typeText.text = "Type: " + star.Planets[IndexOf(_planets, _selectedPlanet)].PlanetTypeString();
                _inhabitedText.text = "Inhabited: " + star.Planets[IndexOf(_planets, _selectedPlanet)].Inhabited;
                _resourcesText.text = "Resources: " + star.Planets[IndexOf(_planets, _selectedPlanet)].ResourcesRichString();
            }
            else
            {
                _scanButton.DOFade(1, _animationSpeed);

                _nameText.text = "Planet: ???";
                _typeText.text = "Type: ???";
                _inhabitedText.text = "Inhabited: ???";
                _resourcesText.text = "Resources: ???";
            }
        });
        _selectionUICanvasGroup.DOFade(1, _animationSpeed).SetDelay(delay).OnComplete(() =>
        {
            if (resetBusy)
            {
                _selectionBusy = false;
            }
        });
    }

    private int IndexOf(GameObject[] array, GameObject gameObject)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == gameObject)
                return i;
        }

        return -1;
    }
}
