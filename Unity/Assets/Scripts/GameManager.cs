using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;

    [SerializeField] float _autoGatherTime = 5.0f;
    [SerializeField] int _upgradePrice = 20;

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

        Resources += (int)Mathf.Pow(2, ClickUpgradeLevel);

        UIManager.Instance.UpdateResources();
    }

    void AutoGather()
    {
        //Debug.Log("AutoGather gather: " + (Mathf.Pow(2, AutoGathererUpgradeLevel - 1)));

        if (AutoGathererUpgradeLevel > 0)
        {
            Resources += (int)Mathf.Pow(2, AutoGathererUpgradeLevel - 1);
            UIManager.Instance.UpdateResources();
        }
    }



    public static GameManager Instance { get => _instance; set => _instance = value; }

    public int Resources { get => _resources; set => _resources = value; }
    public int ClickUpgradeLevel { get => _clickUpgradeLevel; set => _clickUpgradeLevel = value; }
    public int AutoGathererUpgradeLevel { get => _autoGatherUpgradeLevel; set => _autoGatherUpgradeLevel = value; }
    public float AutoGatherTime { get => _autoGatherTime; set => _autoGatherTime = value; }
    public int UpgradePrice { get => _upgradePrice; set => _upgradePrice = value; }
}
