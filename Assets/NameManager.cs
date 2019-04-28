using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance;

    public List<string> StarSystemNames = new List<string>();
    public List<string> PlanetNames = new List<string>();

    private string[] _starSystemNamePrefix = { "Apollo", "Bacchus", "Ceres", "Coelus", "Cupid", "Diana", "Hercules", "Juno", "Jupitor", "Latona", "Mars", "Mercury", "Neptune", "Minerva", "Pluto", "Prosperpina", "Saturn", "Venus", "Vulcan" };
    private string[] _starSystemNameBody = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
    private string[] _starSystemNameSuffix = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta", "Iota", "Kappa", "Lambda", "Mu", "Nu", "Xi", "Omikron", "Pi", "Rho", "Sigma", "Tau", "Upsilon", "Phi", "Chi", "Psi", "Omega" };

    private string[] _planetNamePrefix = { "Dionysus", "Demeter", "Uranus", "Eros", "Artemis", "Heracles", "Hera", "Zeus", "Leto", "Ares", "Hermes", "Poseidon", "Athena", "Hades", "Persephone", "Cronus", "Aphrodite", "Hephaestus" };
    private string[] _planetNameBody = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
    private string[] _planetNameSuffix = { "Prime", "Sa", "Ri", "Ga", "Ma", "Pa", "Dha" };

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;


        GeneratePlanetNames();
        GenerateSystemNames();

    }

    private void GeneratePlanetNames() {
        for (int i = 0; i < _planetNamePrefix.Length; i++)
        {
            for (int j = 0; j < _planetNameBody.Length; j++)
            {
                for (int k = 0; k < _planetNameSuffix.Length; k++)
                {
                    string name = _planetNamePrefix[i] +
                        (Random.Range(0, 2) == 0 ? (" " + _planetNameBody[j]) : "") +
                        (Random.Range(0, 2) == 0 ? (" " + _planetNameSuffix[k]) : "");

                    if (!PlanetNames.Contains(name))
                        PlanetNames.Add(name);
                }
            }
        }
    }

    private void GenerateSystemNames() {
        for (int i = 0; i < _starSystemNamePrefix.Length; i++)
        {
            for (int j = 0; j < _starSystemNameBody.Length; j++)
            {
                for (int k = 0; k < _starSystemNameSuffix.Length; k++)
                {
                    string name = _starSystemNamePrefix[i] +
                        (Random.Range(0, 2) == 0 ? (" " + _starSystemNameBody[j]) : "") +
                        (Random.Range(0, 2) == 0 ? (" " + _starSystemNameSuffix[k]) : "") +
                        " System";

                    if (!StarSystemNames.Contains(name))
                        StarSystemNames.Add(name);
                }
            }
        }
    }

    public string GetPlanetName()
    {
        if (PlanetNames.Count <= 0)
        {
            Debug.Log("Ran out of planet names.. generating them again");
            GeneratePlanetNames();
        }

        string name = PlanetNames[Random.Range(0, PlanetNames.Count)];
        PlanetNames.Remove(name);

        return name;
    }

    public string GetStarSystemName()
    {
        if (StarSystemNames.Count <= 0)
        {
            Debug.Log("Ran out of planet names.. generating them again");
            GenerateSystemNames();
        }

        string name = StarSystemNames[Random.Range(0, StarSystemNames.Count)];
        StarSystemNames.Remove(name);

        return name;
    }
}
