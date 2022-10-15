using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;

    [SerializeField] private int GoldCount, EnergyPotCount;


    void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    void Start()
    {
        SpawnGold();
        SpawnEnergyPot();
    }

    //
    // Spawning gold randomnly into the Grid
    //
    public void SpawnGold()
    {
        GoldCount = 100;

        for (int i = 0; i <= GoldCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseGold>(Faction.Gold);
            var spawnedGold = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetSpawnTile();

            if (Buildings.Instance.buildingsTiles.Contains(randomSpawnTile))
            {
                randomSpawnTile.OccupiedUnit = spawnedGold;
                spawnedGold.OccupiedTile = randomSpawnTile;
            }
            //see Tile script 
            randomSpawnTile.SetUnit(spawnedGold);
        }
    }

    //
    // Spawning energy pots randomnly into the Grid
    //
    public void SpawnEnergyPot()
    {
        EnergyPotCount = 100;

        for (int i = 0; i <= EnergyPotCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnergyPot>(Faction.EnergyPot);
            var spawnedEnergyPot = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetSpawnTile();

            if (Buildings.Instance.buildingsTiles.Contains(randomSpawnTile))
            {
                randomSpawnTile.OccupiedUnit = spawnedEnergyPot;
                spawnedEnergyPot.OccupiedTile = randomSpawnTile;
            }
            //see Tile script 
            randomSpawnTile.SetUnit(spawnedEnergyPot);
        }
    }

    //
    // Generic function for returning a random prefab (gold or energy pot)
    //
    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }
}
