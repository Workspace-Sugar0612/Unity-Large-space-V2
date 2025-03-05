using UnityEngine;
using UnityEngine.SceneManagement;

public class VRCanvasMenu : MonoBehaviour
{
    private string mapName;

    public void ButtonMap(int _map)
    {
        if (_map == 1)
        {
            mapName = "001-Basic";
        }
        else if (_map == 2)
        {
            mapName = "002-Simple";
            //mapName = "SceneVR-Common";
        }
        else if (_map == 3)
        {
            mapName = "Showcase";
        }

        //Debug.Log(name + " loading map: " + mapName);

        SceneManager.LoadScene(mapName);

    }
}

