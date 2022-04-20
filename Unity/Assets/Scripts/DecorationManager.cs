using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    #region Champs static
    static DecorationManager _instance; // instance du singleton
    #endregion


    // structure pour pouvoir faire les associations type de décoration -> prefab directement dans l'Inspector
    // (Unity ne gérant pas directement l'édition de dictionnaires)
    [Serializable]
    public struct DecorationPrefab
    {
        public DecorationModel.DecorationType Type;
        public GameObject Prefab;
    }

    [SerializeField] Transform _gridRoot;
    [SerializeField] List<DecorationPrefab> _prefabs;

    int _nbrRows;
    int _nbrCols;

    Dictionary<DecorationModel.DecorationType, GameObject> _prefabsDictionary = new Dictionary<DecorationModel.DecorationType, GameObject>();


    #region Evénements MonoBehaviour

    void OnEnable()
    {
        // une seule instance en même temps
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }


    void Start()
    {
        if(_gridRoot != null)
        {
            _nbrRows = _gridRoot.childCount;

            if(_nbrRows > 0)
            {
                _nbrCols = _gridRoot.GetChild(0).childCount;
            }
        }

        // on remplit un dictionnaire liant un type de décoration et un prefab
        foreach(DecorationPrefab p in _prefabs)
        {
            _prefabsDictionary[p.Type] = p.Prefab;
        }
    }

    void Update()
    {
    }

    void FixedUpdate()
    {

    }

    #endregion


    public void BuyDecoration(int typeDecoration)
    {
        DecorationModel.DecorationType type = (DecorationModel.DecorationType)typeDecoration;

        if (GameManager.Instance.SaveGame.TakeResources(GameManager.Instance.UpgradePrice))
        {
            //Debug.Log("BUY " + type);

            UIManager.Instance.UpdateResources();

            int row, col;
            do
            {
                row = UnityEngine.Random.Range(0, _nbrRows);
                col = UnityEngine.Random.Range(0, _nbrCols);

            } while (!IsCellFree(row, col)); // TODO cas où toutes les cases sont occupées = blocage

            AddDecoration(type, row, col);
        }
    }


    void AddDecoration(DecorationModel.DecorationType type, int row, int col)
    {
        if (_gridRoot != null && _prefabsDictionary.ContainsKey(type))
        {
            Transform parent = _gridRoot.GetChild(row).GetChild(col);
            GameObject prefab = _prefabsDictionary[type];

            GameObject deco = GameObject.Instantiate(prefab);

            if (parent != null && deco != null)
            {
                deco.transform.SetParent(parent);
                deco.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                deco.transform.localRotation = Quaternion.identity;

                GameManager.Instance.SaveGame.Decorations.Add(new DecorationModel(type, row, col));
            }
        }
    }

    public bool IsCellFree(int row, int col)
    {
        return _gridRoot.GetChild(row).GetChild(col).childCount == 0;
    }

    public void ClearGrid()
    {
        if(_gridRoot != null)
        {
            foreach(Transform row in _gridRoot)
            {
                foreach(Transform col in row)
                {
                    foreach(Transform tree in col)
                    {
                        Destroy(tree.gameObject);
                    }
                }
            }
        }
    }
}
