using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eScene { mainMenu, level001, nateTestScene }
public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public PlayerBehavior playerRef;
    public soDish[] recipeList;
    public CanvasManager canvasManager;
    public OrderManager orderManager;
    public eScene curScene;

    public bool isPaused;

    private void Awake()
    {
        canvasManager = GetComponent<CanvasManager>();
        orderManager = GetComponent<OrderManager>();
        Init();
    }


    private void Init()
    {
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        PauseToggle();
    }

    #region SceneLoading
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        curScene = (eScene)_scene.buildIndex;
        switch (curScene)
        {
            case eScene.mainMenu:
                canvasManager.ShowCanvasFE();
                break;
            case eScene.level001:
                playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
                // Function for loading level001
                break;
            case eScene.nateTestScene:
                playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
                break;
        }
    }
    #endregion
    private void PauseToggle()
    {
        if (curScene != eScene.mainMenu)
        {
            if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = true;
                canvasManager.ShowPauseMenu();
            } else
            if (isPaused && Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject.Find("Widget_PauseMenu(Clone)").GetComponent<WidgetPauseMenu>().OnResumePressed();
                isPaused = false;
            }
        }
    }

    public soDish SolveForRecipe(List<ItemSO> _presentIngredients)
    {
        for (int i = 0; i < recipeList.Length; i++)
        {
            for (int j = 0; j < recipeList[i].soItem.Count; j++)
            {
                if (!_presentIngredients.Contains(recipeList[i].soItem[j]))
                {
                    i++;
                    j = 0;
                }else if(j == recipeList[i].soItem.Count)
                {
                    return recipeList[i];
                }
            }
        }
        Debug.Log("FailedToUpdateMesh");
        return null; //Return burnt food dish.

    }
}
