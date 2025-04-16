using UnityEngine;
using UnityEngine.SceneManagement;
using Enums;
using System.Collections;
using System.IO;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;
using UnityEngine.UI;
using System.Data.Common;

public class GameManager : MonoBehaviour
{
    [Header("Pause Screen")]
    [SerializeField]
    private InputActionReference pauseGameAction;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject settingsMenu;

    [Header("Pickup Screens")]
    [SerializeField]
    private GameObject headSelectionUI;
    [SerializeField]
    private GameObject torsoSelectionUI;
    [SerializeField]
    private GameObject legsSelectionUI;
    [field: SerializeField]
    public Sprite currentHead {get; set;}
    [field: SerializeField]
    public Sprite currentTorso {get; set;}
    [field: SerializeField]
    public Sprite currentLegs {get; set;}
    private Sprite currentPlayerHead {get; set;}
    private Sprite currentPlayerTorso {get; set;}
    private Sprite currentPlayerLegs {get; set;}

    [Header("Mission UIs")]
    [SerializeField]
    private GameObject headMissionUI;
    [SerializeField]
    private GameObject torsoMissionUI;
    [SerializeField]
    private GameObject legsMissionUI;

    [Header("Monster Complete Screen")]
    [SerializeField]
    private GameObject monsterCompleteUI;
    [SerializeField]
    private GameObject monsterImageSavedUI;

    [Header("Monster Screenshot Requirements")]
    [SerializeField]
    private RectTransform screenshotArea;

    [Header("Interactibles")]

    [SerializeField]
    private GameObject[] pickups;
    

    [Header("Android Support")]
    
    [SerializeField]
    private GameObject androidUI;


    [Header("Misc")]
    private PlayerController player;
    [SerializeField]
    private PlayerInput playerInput;
    private RuntimePlatform gamePlatform;
    public bool isTutorial;
    public bool showScreenshotScreen;
    public bool isMenuScene;
    public string playerPartsFilePath;
    private FlatDoorController door;
    [SerializeField]
    private GameObject fadeScreenEffect;
    [SerializeField]
    public static bool wasPreviousSceneMenu;
    private string sceneName;
    [SerializeField]
    private string[] sceneList;

    void Awake()
    {
        if(!isMenuScene) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            fadeScreenEffect = GameObject.FindGameObjectWithTag("FadeEffect");
            door = GameObject.FindGameObjectWithTag("Door").GetComponent<FlatDoorController>();
        }
        sceneName = SceneManager.GetActiveScene().name;
        androidUI = GameObject.FindGameObjectWithTag("AndroidUI");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamePlatform = Application.platform;

        if(gamePlatform == RuntimePlatform.Android && androidUI != null) {
            androidUI.SetActive(true);
        }

        //Load sprites from memory
        currentPlayerHead = Resources.Load<Sprite>("PlayerParts/" + PlayerPrefs.GetString("playerHead"));
        currentPlayerTorso = Resources.Load<Sprite>("PlayerParts/" + PlayerPrefs.GetString("playerTorso"));
        currentPlayerLegs = Resources.Load<Sprite>("PlayerParts/" + PlayerPrefs.GetString("playerLegs"));

        if(!isTutorial && !isMenuScene && currentPlayerHead != null && currentPlayerTorso != null && currentPlayerLegs != null) {
            player.updatePlayerSprite(PickupType.head, currentPlayerHead);
            player.updatePlayerSprite(PickupType.torso, currentPlayerTorso);
            player.updatePlayerSprite(PickupType.legs, currentPlayerLegs);
            Debug.Log("Loaded character sprites.");
        }
        Debug.Log("Previous Scene: " + doFadeInOnLoad.previousSceneName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable() {
        if(pauseGameAction != null) {
            pauseGameAction.action.performed += pauseUnpauseGame;
        }  
    }
    private void OnDisable() {
        if(pauseGameAction != null) {
            pauseGameAction.action.performed -= pauseUnpauseGame;
        }  
    }

    private void pauseUnpauseGame(InputAction.CallbackContext obj) {
        if(!isMenuScene) {
            switchPauseState();
        }
    }

    public void switchPauseState() {

        if(settingsMenu.activeSelf) {
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        else {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            switchPlayerInputMap();         
        }
    }

    public void switchFromSettingsMenuToGameplay() {
        settingsMenu.SetActive(false);
        switchPlayerInputMap();
    }

    public void loadMainMenuScene() {
        updateWasPreviousSceneMenu();
        SceneManager.LoadScene("Main Menu");
    }

    public void loadLevelSelectMenuScene() {
        updateWasPreviousSceneMenu();
        PlayerPrefs.SetInt(sceneName, 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Level Select Menu");
    }

    public void reloadCurrentScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void loadNextScene() {

        //if not at the final scene
        updateWasPreviousSceneMenu();
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1) {
            
            //Save level as completed
            PlayerPrefs.SetInt(sceneName, 1);
            PlayerPrefs.Save();

            //Load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else {
            reloadCurrentScene();
        }

       /*  if (Application.CanStreamedLevelBeLoaded(nextScene)) {
            Debug.Log("got here");
            SceneManager.LoadScene(nextScene);
        }
        else {
            reloadCurrentScene();
        } */

    }

    public void loadSceneAt(int sceneID) {
        updateWasPreviousSceneMenu();
        SceneManager.LoadScene(sceneID);
    }

    public void loadSceneByName(string sceneName) {
        updateWasPreviousSceneMenu();
        SceneManager.LoadScene(sceneName);
    }

    private void updateWasPreviousSceneMenu() {
        doFadeInOnLoad.previousSceneName = SceneManager.GetActiveScene().name;
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

    public void updatePlayerSprite(PickupType pickupType, Sprite partSpriteUI, Sprite partSpritePlayer) {
        switch (pickupType) {
            case PickupType.head:
                currentHead = partSpriteUI;
                currentPlayerHead = partSpritePlayer;
                PlayerPrefs.SetString("playerHead", partSpritePlayer.name);
                break;
            case PickupType.torso:
                currentTorso = partSpriteUI;
                currentPlayerTorso = partSpritePlayer;
                PlayerPrefs.SetString("playerTorso", partSpritePlayer.name);
                break;
            case PickupType.legs:
                currentLegs = partSpriteUI;
                currentPlayerLegs = partSpritePlayer;
                PlayerPrefs.SetString("playerLegs", partSpritePlayer.name);                
                break;
        }

        PlayerPrefs.Save();

        player.updatePlayerSprite(pickupType, partSpritePlayer);

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
        switchPlayerInputMap();        
        door.decreasePickupsLeft();
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
        switchPlayerInputMap();
    }

    public void displayEndOfLevelUI() {
        fadeScreenEffect.SetActive(true);
        switchPlayerInputMap();
    }

    public void saveMonsterImage() {
        StartCoroutine(saveMonsterCoroutine());
    }

    private IEnumerator saveMonsterCoroutine() {
        yield return new WaitForEndOfFrame();

        //Step 1: Define the screenshot area
        UnityEngine.Vector3[] screenshotCorners = new UnityEngine.Vector3[4];
        screenshotArea.GetWorldCorners(screenshotCorners);

        //Step 2: Define height and width
        UnityEngine.Vector2 bottomLeft = screenshotCorners[0];
        UnityEngine.Vector2 topLeft = screenshotCorners[1];
        UnityEngine.Vector2 topRight = screenshotCorners[2];

        Debug.Log("bottomLeft: " + bottomLeft);
        Debug.Log("topLeft: " + topLeft);
        Debug.Log("topRight: " + topRight);
        
        float height = topLeft.y - bottomLeft.y;
        float width = topRight.x - bottomLeft.x;

        Debug.Log("height: " + height);
        Debug.Log("width: " + width);

        //Step 3: Create a texture and rectangle area with the new measurements
        Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
        Rect rex = new Rect(bottomLeft.x,bottomLeft.y,width,height);
        
        //Step 4: Save all the pixels in the rectangle area into the texture
        tex.ReadPixels(rex, 0, 0);
        tex.Apply();

        //Step 5: Encode the texture's contents into a byte array in the png format
        byte[] bytes = tex.EncodeToPNG();

        //Step 6: Texture no longer needed, can be destroyed to avoid memory leaks
        Destroy(tex);

        //Step 7: Save bytes at the provided destination
        if(gamePlatform == RuntimePlatform.Android) {
            NativeGallery.SaveImageToGallery(bytes, "MonsterLab", "MyMonster" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png", ( success, path ) => Debug.Log( "Media save result: " + success + " " + path ));
        }
        else {
            string folderPath = Path.Combine(Application.dataPath + "/MonsterLabPics/");
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, "MyMonster" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
            File.WriteAllBytes(filePath, bytes);
        }
        
        //Step 8: Pop up message indicating the operation's success
        monsterImageSavedUI.SetActive(true);
        
    }

    public void switchPlayerInputMap()
    {
        if(playerInput.currentActionMap.name == "Player") {
            playerInput.SwitchCurrentActionMap("UI");
        }
        else {
            playerInput.SwitchCurrentActionMap("Player");
        }

        Debug.Log("Switch action map to: " + playerInput.currentActionMap.name);
    }

    public void resetPlayerProgress() {
        foreach(string scene in sceneList) {
            PlayerPrefs.SetInt(scene, 0);
        }
        PlayerPrefs.Save();
    }
}
