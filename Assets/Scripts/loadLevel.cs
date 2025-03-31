using UnityEngine;

public class loadLevel : MonoBehaviour
{
    [SerializeField]
    private int levelID;

    public void loadLevelID() {
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().loadSceneAt(levelID);
    }
}
