using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    QuestionsProvider qProvider;
    [SerializeField]
    PopupManager popUpManager;
    [SerializeField]
    ExplosionManager explosionManager;
    [SerializeField]
    List<Transform> spawnPoints;
    [SerializeField]
    GameObject dropPrefab;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI healthScore;    
    [SerializeField]
    TextMeshProUGUI Level;
    [SerializeField]
    UnityEngine.UI.Image image;
    public AudioManager audioManager;

    [SerializeField]
    float spawnDelay = 1f;
    [SerializeField]
    int pointsPerWrongDetected = 10;
    [SerializeField]
    int questionsThisRound = 10;
    [SerializeField]
    int health = 3;

    private List<GameObject> activeObjects = new List<GameObject>();
    private float currentTime;
    private int score = 0;
    private bool GameStarted = false;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            DestroyImmediate(this.gameObject);
        }

        scoreText.text = "Score : 0";
        healthScore.text = "Lives : " + health;
        UpdateLevel();
        explosionManager.ClearExplosions();
        popUpManager.ShowPopup(PopupManager.PopUpType.INTRO, OnIntroDone, "Continue", true);
    }

    //Game is started here
    void OnIntroDone()
    {
        image.CrossFadeAlpha(0, 1f, true);
        GameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStarted)
            return;

        if (questionsThisRound > 0)
        {
            if (currentTime > spawnDelay)
            {
                currentTime = 0;
                SpawnObject();
                questionsThisRound--;
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        } else
        {
            //game end check
            if(activeObjects.Count <= 0)
            {
                LevelWonContinue();
            }
        }
    }

    void SpawnObject()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Count);
        CreateObjectAt(spawnIndex);
    }

    public void ObjectReachedGround(GameObject obj)
    {
        Drops drop = GetDrops(obj);
        if (drop.isWrongStatement)
        {
            RemoveAnObject(obj);
            EndGameEnemyWin();
        } else
        {
            RemoveAnObject(obj, false);
        }
    }


    void CreateObjectAt(int spawnIndex)
    {
        Transform parent = spawnPoints[spawnIndex];
        GameObject newObject = GameObject.Instantiate(dropPrefab, parent.position, Quaternion.identity);
        activeObjects.Add(newObject);
        Drops drop = GetDrops(newObject);
        bool isWrongStatement = Random.Range(0, 2) == 0;
        drop.isWrongStatement = isWrongStatement;
        if (isWrongStatement)
        {
            string question = qProvider.GetIncorrectStatement();
            drop.SetText(question);
        } else
        {
            string question = qProvider.GetCorrectStatement();
            drop.SetText(question);
        }
    }

    public void OnObjectTouched(GameObject obj)
    {
        Drops controller = GetDrops(obj);
        if(controller.tapsToDestroy > 1)
        {
            controller.tapsToDestroy--;
            audioManager.PlayClick();
            return;
        }

        if(controller.isWrongStatement)
        {
            UpdateScore();
            RemoveAnObject(obj);
        } else
        {
            UpdateHealth();
            RemoveAnObject(obj);
            if(health <= 0)
            {
                EndGameNoHp();
            }
        }
    }

    //because unity dont let get component for interfaces , we have strong coupling in one place only
    private Drops GetDrops(GameObject obj)
    {
        return obj.GetComponentInChildren<DropController>() as Drops;
    }

    private void UpdateScore()
    {
        score += pointsPerWrongDetected;
        scoreText.text = "Socre : " + score;
    }

    private void UpdateHealth()
    {
        health--;
        healthScore.text = "Lives : " + health;
    }

    private void UpdateLevel()
    {
        level++;
        Level.text = "Level : " + level;
    }

    public void EndGameEnemyWin()
    {
        popUpManager.ShowPopup(PopupManager.PopUpType.LOST_BY_LANDING, ResetGame, "Restart", true);
        RemoveAllObjects();
        explosionManager.CreateGameOverEffect();
        GameStarted = false;
        audioManager.PlayFailed();
        audioManager.PauseMusic();
    }

    public void EndGameNoHp()
    {
        popUpManager.ShowPopup(PopupManager.PopUpType.LOST_BY_WRONGANSWER, ResetGame, "TryAgain", true);
        RemoveAllObjects();
        GameStarted = false;
        audioManager.PlayFailed();
        audioManager.PauseMusic();
    }

    public void LevelWonContinue()
    {
        popUpManager.ShowPopup(PopupManager.PopUpType.WON_LEVEL, IncreaseLevel, "Help ?", true);
        RemoveAllObjects();
        GameStarted = false;
    }

    private void IncreaseLevel()
    {
        health = 3;
        questionsThisRound = 10;
        UpdateLevel();
        GameStarted = true;
        health = 3;
        questionsThisRound = 10;
        explosionManager.ClearExplosions();

        //Increase difficulty each level
        spawnDelay = Mathf.Clamp(spawnDelay - spawnDelay*0.5f, 1.5f, 4f);
        audioManager.PlayMusic();
    }

    private void ResetGame()
    {
        GameStarted = true;
        health = 3;
        questionsThisRound = 10;
        level = 0;
        UpdateLevel();
        scoreText.text = "Score : 0";
        healthScore.text = "Lives : " + health;
        explosionManager.ClearExplosions();
        audioManager.PlayMusic();
    }

    private void RemoveAnObject(GameObject obj, bool explosion = true)
    {
        if(explosion)
            explosionManager.CreateExplosionAt(obj.transform.position);
        activeObjects.Remove(obj.transform.parent.gameObject);
        GameObject.Destroy(obj.transform.parent.gameObject);
    }

    private void RemoveAllObjects()
    {
        for(int i=0; i<activeObjects.Count; i++)
        {
            GameObject.Destroy(activeObjects[i]);
        }
        activeObjects.Clear();
    }
}

interface Drops
{
    bool isWrongStatement { get; set; }
    int tapsToDestroy { get; set; }
    void SetText(string s);
}
