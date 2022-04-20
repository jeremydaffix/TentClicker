using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // serializable pour avoir la liste des champs dans l'Inspector
public class DecorationModel
{
    public enum DecorationType
    {
        Pine1,
        Pine2,
        Pine3
    }

    [SerializeField] DecorationType _type;
    [SerializeField] int _row;
    [SerializeField] int _col;


    public DecorationModel(DecorationType type = DecorationType.Pine1, int row = 0, int col = 0)
    {
        _type = type;
        _row = row;
        _col = col;
    }


    public DecorationType Type { get => _type; set => _type = value; }
    public int Row { get => _row; set => _row = value; }
    public int Col { get => _col; set => _col = value; }
}
