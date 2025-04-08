using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{

    private Image image;
    private Color imageColor;
    private bool fadingOut;
    private bool isStopped;
    public float fadeSpeed;
    //[SerializeField]
    //private GameObject endOfLevelMenu;
    private GameManager gameManager;

    void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1f);
        imageColor = image.color;
        fadingOut = true;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (doFadeInOnLoad.previousSceneName == "Main Menu") {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if(!isStopped) {
            //Fading out
            if(fadingOut) {
                image.color = new Color(imageColor.r, imageColor.g, imageColor.b, imageColor.a -= fadeSpeed * Time.deltaTime);

                if(image.color.a <= 0) {
                    fadingOut = false;
                    gameObject.SetActive(false);
                }
            }
            //Fading in
            else {
                image.color = new Color(imageColor.r, imageColor.g, imageColor.b, imageColor.a += fadeSpeed * Time.deltaTime);

                if(image.color.a >= 1) {
                    isStopped = true;
                    //endOfLevelMenu.SetActive(true);
                    gameManager.loadLevelSelectMenuScene();
                }
            }
        }
    }
}
