using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;

    [SerializeField] int _autoGatherTime = 5;
    [SerializeField] int _upgradePrice = 20;
    [SerializeField] int _maxUpgradeLevel = 10;

    [SerializeField] int _resources = 0;
    [SerializeField] int _clickUpgradeLevel = 0;
    [SerializeField] int _autoGatherUpgradeLevel = 0;


    float _lastTimeGathered = 0f;


    void OnEnable()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }


    void Start()
    {
        
    }

    void Update()
    {
        if(Time.time >= (_lastTimeGathered + AutoGatherTime))
        {
            _lastTimeGathered = Time.time;
            AutoGather();
        }
    }

    void FixedUpdate()
    {
        
    }


    public void ClickGather()
    {
        //Debug.Log("Click gather: " + (int)Mathf.Pow(2, ClickUpgradeLevel));

        AddResources(CalculateClickGather());
    }

    void AutoGather()
    {
        //Debug.Log("AutoGather gather: " + (Mathf.Pow(2, AutoGathererUpgradeLevel - 1)));

        if (AutoGathererUpgradeLevel > 0)
        {
            AddResources(CalculateAutoGather());
        }
    }


    public int CalculateClickGather()
    {
        return (int)Mathf.Pow(2, ClickUpgradeLevel);
    }

    public int CalculateAutoGather()
    {
        int rsrc = 0;

        if (AutoGathererUpgradeLevel > 0)
        {
           rsrc = (int)Mathf.Pow(2, AutoGathererUpgradeLevel - 1);
        }

        return rsrc;
    }

    public void BuyClickUpgrade()
    {
        if(ClickUpgradeLevel < MaxUpgradeLevel && TakeResources(UpgradePrice))
        {
            ++ClickUpgradeLevel;

            UIManager.Instance.UpdateUpgradesItems();
        }
    }

    public void BuyAutoGatherUpgrade()
    {
        if (AutoGathererUpgradeLevel < MaxUpgradeLevel && TakeResources(UpgradePrice))
        {
            ++AutoGathererUpgradeLevel;

            UIManager.Instance.UpdateUpgradesItems();
        }
    }


    public bool TakeResources(int price)
    {
        bool enoughResources = (Resources >= price);

        if (enoughResources)
        {
            _resources -= price;
            UIManager.Instance.UpdateResources();
        }

        return enoughResources;
    }

    public void AddResources(int nbr)
    {
        _resources += nbr;
        UIManager.Instance.UpdateResources();
    }




    public static GameManager Instance { get => _instance; set => _instance = value; }

    public int Resources { get => _resources; set => _resources = value; }
    public int ClickUpgradeLevel { get => _clickUpgradeLevel; set => _clickUpgradeLevel = value; }
    public int AutoGathererUpgradeLevel { get => _autoGatherUpgradeLevel; set => _autoGatherUpgradeLevel = value; }
    public int AutoGatherTime { get => _autoGatherTime; set => _autoGatherTime = value; }
    public int UpgradePrice { get => _upgradePrice; set => _upgradePrice = value; }
    public int MaxUpgradeLevel { get => _maxUpgradeLevel; set => _maxUpgradeLevel = value; }
}
