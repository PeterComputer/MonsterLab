using UnityEngine;
using UnityEngine.SceneManagement;
using Enums;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections;
using System.Numerics;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [Header("Pickup Screens")]
    [SerializeField]
    private GameObject headSelectionUI;
    [SerializeField]
    private GameObject torsoSelectionUI;
    [SerializeField]
    private GameObject legsSelectionUI;
    [SerializeField]

    [Header("Mission UIs")]
    private GameObject headMissionUI;
    [SerializeField]
    private GameObject torsoMissionUI;
    [SerializeField]
    private GameObject legsMissionUI;

    [Header("Monster Complete Screen")]
    [SerializeField]
    private GameObject monsterCompleteUI;

    [Header("Monster Screenshot Requirements")]
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    private RectTransform screenshotArea;



    [Header("Interactibles")]

    [SerializeField]
    private GameObject[] pickups; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    public void loadMainMenuScene() {
        SceneManager.LoadScene("Main Menu");
    }

    public void reloadCurrentScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void loadNextScene() {
        //if not at the final scene
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else {
            reloadCurrentScene();
        }
    }

    public void openSelectionUI(PickupType type) {
        switch (type) {
            case PickupType.head:
            headSelectionUI.SetActive(true);
            break;
            case PickupType.torso:
            torsoSelectionUI.SetActive(true);
            break;
            case PickupType.legs:
            legsSelectionUI.SetActive(true);
            break;                        
        }
    }

    public void advanceToNextMission(PickupType currentType) {

        //This probably shouldn't be hardcoded, but it'll work
        switch(currentType) {
            case PickupType.head:
                headMissionUI.SetActive(false);
                torsoMissionUI.SetActive(true);
                setActiveTypeOfPickup(PickupType.torso);
            break;
            case PickupType.torso:
                torsoMissionUI.SetActive(false);
                legsMissionUI.SetActive(true);
                setActiveTypeOfPickup(PickupType.legs);
            break;
            case PickupType.legs:
                legsMissionUI.SetActive(false);
            break;
        }
        
        destroyTypeOfPickup(currentType);
        
        GameObject.FindWithTag("Door").GetComponent<DoorController>().decreasePickupsLeft();
    }


    private void destroyTypeOfPickup(PickupType type) {
        foreach (GameObject pickup in pickups) {
            if (pickup != null && pickup.GetComponent<PickupController>().getPickupType() == type) {
                Destroy(pickup);
            }
        }
    }

    private void setActiveTypeOfPickup(PickupType type) {
        foreach (GameObject pickup in pickups) {
            if (pickup != null && pickup.GetComponent<PickupController>().getPickupType() == type) {
                pickup.SetActive(true);
            }
        }        
    }

    public void closeApplication() {
        Application.Quit();
    }

    public void displayMonsterCompleteUI() {
        legsMissionUI.SetActive(false);
        monsterCompleteUI.SetActive(true);
    }

    public void saveMonsterImage() {
        saveMonsterCoroutine();
    }

    private IEnumerator saveMonsterCoroutine() {
        yield return new WaitForEndOfFrame();

        //Step 1: Define the screenshot area
        UnityEngine.Vector3[] screenshotCorners = new UnityEngine.Vector3[4];
        screenshotArea.GetWorldCorners(screenshotCorners);

        //Step 2: Convert world coordinates into camera coordinates
        UnityEngine.Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(sceneCamera, screenshotCorners[0]);
        UnityEngine.Vector2 topLeft = RectTransformUtility.WorldToScreenPoint(sceneCamera, screenshotCorners[1]);
        UnityEngine.Vector2 topRight = RectTransformUtility.WorldToScreenPoint(sceneCamera, screenshotCorners[2]);

        //Step 3: Define height and width
        float height = topLeft.y - bottomLeft.y;
        float width = topRight.x - topLeft.x;

        //Step 4: Create a texture and rectangle area with the new measurements
        Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
        Rect rex = new Rect(bottomLeft.x,bottomLeft.y,width,height);
        
        //Step 5: Save all the pixels in the rectangle area into the texture
        tex.ReadPixels(rex, 0, 0);
        tex.Apply();

        //Step 6: Encode the texture's contents into a byte array in the png format
        byte[] bytes = tex.EncodeToPNG();

        //Step 7: Texture no longer needed, can be destroyed
        Destroy(tex);

        //Step 8: Save bytes at the provided destination
        //File.WriteAllBytes(Application.dataPath + fileName, bytes);
    }
}
