using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using System;
using Random = UnityEngine.Random;

public class Star : MonoBehaviour
{
    private string _name;
    public string Name
    {
        get => _name;
    }

    private int _numberOfPlanets;
    public int NumberOfPlanets
    {
        get => _numberOfPlanets;
    }

    private List<PlanetInformation> _planets = new List<PlanetInformation>();
    public List<PlanetInformation> Planets
    {
        get => _planets;
    }

    // Start is called before the first frame update
    void Start()
    {
        _name = NameManager.Instance.GetStarSystemName();
        gameObject.name = _name;
        _numberOfPlanets = Random.Range(1, 4);

        for (int i = 0; i < _numberOfPlanets; i++)
        {
            _planets.Add(new PlanetInformation());
        }
    }

    public class PlanetInformation
    {
        public string Name;

        public enum PlanetTypeEnum { Gas, Minerals, Inhabited };
        public PlanetTypeEnum PlanetType;

        public int Variant;
        public bool Scanned;
        public bool Visited;
        public bool Inhabited;
        public int ResourcesRich = 0; //0 - none, 1 - low, 2 - medium, 3 - high

        public int Mined;

        public PlanetInformation()
        {
            PlanetType = (PlanetTypeEnum)Random.Range(0, 3);
            ResourcesRich = PlanetType == PlanetTypeEnum.Inhabited ? 0 : Random.Range(0, 4);
            Variant = Random.Range(0, 100);

            Name = NameManager.Instance.GetPlanetName();

            Inhabited = (PlanetType == PlanetTypeEnum.Inhabited);
        }

        public string PlanetTypeString()
        {
            switch (PlanetType) {
                case PlanetTypeEnum.Gas:
                    return "Gas Giant";

                case PlanetTypeEnum.Minerals:
                    return "Ice Giant";

                case PlanetTypeEnum.Inhabited:
                    return "Terrestrial";

                default:
                    return "Unknown Type";
            }
        }

        public string ResourcesRichString() {
            switch (ResourcesRich)
            {
                case 0:
                    return "None";

                case 1:
                    return "Low";

                case 2:
                    return "Medium";

                case 3:
                    return "High";

                default:
                    return "Unknown";
            }
        }
    }
}
