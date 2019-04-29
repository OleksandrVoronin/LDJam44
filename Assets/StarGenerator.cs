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
    public List<GameObject> GeneratedStars {
        get => _generatedStars;
    }

    [SerializeField]
    private float _noiseScale = 0.5f;
    [SerializeField]
    private int _noiseOffset = 999;
    [SerializeField]
    private Vector2 _starGenValueRange = new Vector2(0.69f, 0.7f);

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

    private bool[,] _generatedWorld;

    [Button]
    private void GenerateStars()
    {
        WipeStars();
        _noiseOffset = Random.Range(1000, 10000);
        _generatedWorld = new bool[(int)_gridResolution.x, (int)_gridResolution.y];

        float stepX = (_gameBounds.z - _gameBounds.x) / (_gridResolution.x);
        float stepY = (_gameBounds.w - _gameBounds.y) / (_gridResolution.y);

        for (int x = 0; x < _gridResolution.x; x++)
        {
            for (int y = 0; y < _gridResolution.y; y++)
            {
                float noise = Mathf.PerlinNoise((x + _noiseOffset) * _noiseScale, (y + _noiseOffset) * _noiseScale);

                _generatedWorld[x, y] = false;

                if (!(x == 0 && y == 0) && !(x == _gridResolution.x - 1 && y == _gridResolution.y - 1))
                    if (noise < _starGenValueRange.x || noise > _starGenValueRange.y) continue;

                _generatedWorld[x, y] = true;

                GameObject newStar = Instantiate(_starPrefab, new Vector3(_gameBounds.x + x * stepX + (true ? Random.Range(_everyOtherOffset.x, _everyOtherOffset.y) : 0),
                                                                            _gameBounds.y + y * stepY + (true ? Random.Range(_everyOtherOffset.x, _everyOtherOffset.y) : 0)), new Quaternion(), transform);
                _generatedStars.Add(newStar);

                newStar.GetComponent<Star>().Init();

                if (_generatedStars.Count == 1) // first sytem
                {
                    newStar.GetComponent<Star>().Planets[0].Inhabited = true;
                    newStar.GetComponent<Star>().Planets[0].PlanetType = Star.PlanetInformation.PlanetTypeEnum.Inhabited;
                    newStar.GetComponent<Star>().Planets[0].ResourcesRich = 0;
                    newStar.GetComponent<Star>().Planets[0].Scanned = true;
                    newStar.GetComponent<Star>().Planets[0].Variant = 0;
                }
            }
        }

        if (_generatedStars.Count > 0)
        {
            _player.transform.position = _generatedStars[0].transform.position;
            _player.GetComponent<PlayerShipMover>().CurrentStar = _generatedStars[0];
        }

        CheckPath();
    }

    private void CheckPath()
    {
        Node[,] nodes = new Node[(int)_gridResolution.x, (int)_gridResolution.y];

        for (int x = 0; x < _gridResolution.x; x++)
        {
            for (int y = 0; y < _gridResolution.y; y++)
            {
                if (_generatedWorld[x, y])
                {
                    nodes[x, y] = new Node(x, y);
                }
            }
        }

        Vector2Int[] directionNeighbors = {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1)
        };

        for (int x = 0; x < _gridResolution.x; x++)
        {
            for (int y = 0; y < _gridResolution.y; y++)
            {
                if (_generatedWorld[x, y])
                {
                    for (int i = 0; i < directionNeighbors.Length; i++)
                    {
                        if (x + directionNeighbors[i].x < 0 || y + directionNeighbors[i].y < 0
                            || x + directionNeighbors[i].x >= (int)_gridResolution.x || y + directionNeighbors[i].y >= (int)_gridResolution.y)
                        {
                            continue;
                        }

                        if (nodes[x + directionNeighbors[i].x, y + directionNeighbors[i].y] != null)
                        {
                            nodes[x, y].neighbors.Add(nodes[x + directionNeighbors[i].x, y + directionNeighbors[i].y]);
                        }
                    }
                }
            }
        }

        List<Node> toVisit = new List<Node>();
        toVisit.Add(nodes[0, 0]);

        bool found = false;

        while (toVisit.Count > 0 && !found)
        {
            Node visiting = toVisit[0];
            toVisit.RemoveAt(0);

            if (visiting != null && !visiting.visited)
            {
                visiting.visited = true;
                toVisit.AddRange(visiting.neighbors);
            }

            if (visiting == nodes[(int)_gridResolution.x - 1, (int)_gridResolution.y - 1])
            {
                found = true;
            }
        }

        if (found)
        {
            Debug.Log("This map contains a valid path");
        }
        else
        {
            Debug.Log("This map does not contain a valid path.. Regenerating");
            GenerateStars();
        }

    }

    private class Node
    {
        public int x;
        public int y;

        public bool visited;

        public List<Node> neighbors = new List<Node>();

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
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
