
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField]
    private Image[] splashImages;
    public float fadeDuration;
    [SerializeField]
    private int splashCounter;

    void Awake()
    {
     splashImages = GetComponentsInChildren<Image>();
     splashCounter = 0; 
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //makes splash screens invisible
        foreach (var image in splashImages) {
            image.color = new Color(255f, 255f, 255f, 0f);
        }

        StartCoroutine(displaySplashImage());
    }

    private IEnumerator displaySplashImage() {
        Image image = splashImages[splashCounter];
        float alpha = 0f;
        float elapsedTime = 0f;

        //fade in
        while(elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            alpha = Mathf.Clamp01(elapsedTime/fadeDuration);
            image.color = new Color(255f, 255f, 255f, alpha);
            yield return null;
        }
        image.color = new Color(255f, 255f, 255f, 1f);

        //wait a bit
        yield return new WaitForSeconds(fadeDuration);

        //fade out
        while(elapsedTime > 0f) {
            elapsedTime -= Time.deltaTime;
            alpha = Mathf.Clamp01(elapsedTime/fadeDuration);
            image.color = new Color(255f, 255f, 255f, alpha);
            yield return null;
        }

        //display next splash
        if(++splashCounter < splashImages.Length) {
            StartCoroutine(displaySplashImage());
        }
        else {
            // Sets it so the game always loads the first page of the level select, on startup
            PlayerPrefs.SetInt("LevelSelectPageToLoad", 0);
            PlayerPrefs.Save();

            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().loadMainMenuScene();
        }
        yield return null;

    }
}
