using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;
    //public BaseGold SelectedHero;

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

    public void SpawnGold()
    {
        GoldCount = 100;

        for (int i = 0; i <= GoldCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseGold>(Faction.Gold);
            var spawnedGold = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetSpawnTile();

            //see Tile script 
            randomSpawnTile.SetUnit(spawnedGold);
        }

        //GameManager.Instance.ChangeState(GameState.SpawnEnergyPot);
    }

    public void SpawnEnergyPot()
    {
        EnergyPotCount = 100;

        for (int i = 0; i <= EnergyPotCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnergyPot>(Faction.EnergyPot);
            var spawnedEnergyPot = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetSpawnTile();

            randomSpawnTile.SetUnit(spawnedEnergyPot);
        }

        //GameManager.Instance.ChangeState(GameState.HeroesTurn);
        //GameManager.Instance.ChangeState(GameState.Null);
    }

    //generic function for returning a random prefab (gold or energy pot)
    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedHero(BaseGold hero)
    {
        //SelectedHero = hero;
        //MenuManager.Instance.ShowSelectedHero(hero);
    }
   
}
