using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class StarGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject _starPrefab;

    [SerializeField]
    private Vector4 _gameBounds;

    [SerializeField]
    private Vector2 _gridResolution;

    [SerializeField]
    private Vector2 _everyOtherOffset;

    [SerializeField]
    private GameObject _player;

    private List<GameObject> _generatedStars = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        WipeStars();
        GenerateStars();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button]
    private void GenerateStars()
    {
        float stepX = (_gameBounds.z - _gameBounds.x) / (_gridResolution.x);
        float stepY = (_gameBounds.w - _gameBounds.y) / (_gridResolution.y);

        for (int x = 0; x < _gridResolution.x; x++)
        {
            for (int y = 0; y < _gridResolution.y; y++)
            {
                GameObject newStar = Instantiate(_starPrefab, new Vector3(_gameBounds.x + x * stepX + (true ? Random.Range(_everyOtherOffset.x, _everyOtherOffset.y) : 0), 
                                                                            _gameBounds.y + y * stepY + (true ? Random.Range(_everyOtherOffset.x, _everyOtherOffset.y) : 0)), new Quaternion(), transform);
                _generatedStars.Add(newStar);
            }
        }

        _player.transform.position = _generatedStars[0].transform.position;
        _player.GetComponent<PlayerShipMover>().CurrentStar = _generatedStars[0];
    }

    [Button]
    private void WipeStars()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
            i--;
        }

        _generatedStars.Clear();
    }
}
