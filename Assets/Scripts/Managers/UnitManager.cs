using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;

    [SerializeField] private int GoldCount, EnergyPotCount;

    public List<Tile> goldTiles = new();
    public List<Tile> energyPotTiles = new();


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
        GoldCount = 250;

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

            goldTiles.Add(randomSpawnTile);
        }
    }

    //
    // Spawning energy pots randomnly into the Grid
    //
    public void SpawnEnergyPot()
    {
        EnergyPotCount = 300;

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

            energyPotTiles.Add(randomSpawnTile);
        }
    }

    //
    // Generic function for returning a random prefab (gold or energy pot)
    //
    public T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }
}
