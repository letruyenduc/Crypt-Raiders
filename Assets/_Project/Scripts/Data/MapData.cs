using UnityEngine;

[CreateAssetMenu(fileName = "New_Map", menuName = "RPG/Map Data")]
public class MapData : ScriptableObject
{
    public string mapName;
    public string sceneName;
    public Sprite mapThumbnail;
    [TextArea(3, 5)]
    public string mapDescription;
    public int recommendedLevel = 1;
}
