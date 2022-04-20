using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveSerializable
{
    public string id;
    public int score;
    public int click_level;
    public int autogather_level;

    public string ToString()
    {
        return $"id: {id}, score: {score}, click_level: {click_level}, autogather_level: {autogather_level}";
    }
}
