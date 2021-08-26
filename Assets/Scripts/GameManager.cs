using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class GameManager : MonoBehaviour
{

    public bool isConnectedGooglePlayServices = false;
    private bool watchedAd = false;
    public RewardedAdsButton myRW;
    public float ObjectSpeed = 5;
    public GameObject adPanel;
    public GameObject moneyBG;
    public ParticleSystem myParts;
    public int Money = 0;
    public GameObject ShopPanel;
    public GameObject PlayButton;
    public Text OldMoney;
    public Text CurrentMoney;
    public Text FinishScore;
    public Text FinishHighscore;
    public SoundManager mySM;

    private List<Mesh> parts;
    public float objectSpeed = 10f;
    public bool GameOver = false;
    public bool pause = true;
    public bool waiting = true;
    public int spawnLimite = 3;
    public float maxSpawn = 3;
    public float waitTime = 2.5f;
    public Rotator orbPlayer;
    public int Score = 0;
    private int level = 1;
    public GameObject SceneController;
    public Text scoreText;
    public Text highscoreText;
    public Text moneyText;
    public List<Transform> Spawnpoints;
    public List<GameObject> Objects;
    public Vector3 ScaleFact = new Vector3(1.125f, 1.125f, 1.125f);
    public int scoreThresh = 10;
    public int scoreCounter = 0;

    public GameObject OutOfBounds;
    public Transform OOBPoint;
    public GameObject Background;
    private SaveFile mySave;
    public List<ObjectPack> Packs;
    public ObjectPack selectedPack;

    public GameObject RetryPanel;
    public GameObject switchPackButton;

    public bool pack0 = true;
    public bool pack1 = false;
    public bool pack2 = false;
    public bool pack3 = false;
    public bool pack4 = false;
    public bool pack5 = false;
    public bool pack6 = false;
    public bool pack7 = false;
    public bool allPack = false;

    public List<bool> packsEnabled;

    public int highScore = 0;
    public int lastPack = 0;

    private int debugInt = 0;
    public Text debugPackName;

    // Start is called before the first frame update
    private void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        mySave = SaveSystem.loadGame();
        if (mySave == null)
        {
            //add packs
            packsEnabled.Add(pack0);
            packsEnabled.Add(pack1);
            packsEnabled.Add(pack2);
            packsEnabled.Add(pack3);
            packsEnabled.Add(pack4);
            packsEnabled.Add(pack5);
            packsEnabled.Add(pack6);
            packsEnabled.Add(pack7);

            mySave = new SaveFile(this);
           // this.SaveGame();
           // RestartScene();
        }
        else
        {
            pack0 = mySave.pack0Unlock;
            pack1 = mySave.pack1Unlock;
            pack2 = mySave.pack2Unlock;
            pack3 = mySave.pack3Unlock;
            pack4 = mySave.pack4Unlock;
            pack5 = mySave.pack5Unlock;
            pack6 = mySave.pack6Unlock;
            pack7 = mySave.pack7Unlock;

            //add packs
            packsEnabled.Add(pack0);
            packsEnabled.Add(pack1);
            packsEnabled.Add(pack2);
            packsEnabled.Add(pack3);
            packsEnabled.Add(pack4);
            packsEnabled.Add(pack5);
            packsEnabled.Add(pack6);
            packsEnabled.Add(pack7);

            Money = mySave.Money;

            lastPack = mySave.lastPack;
            highScore = mySave.highScore;
            allPack = mySave.allPack;


        }
        Time.timeScale = 0;
    }

    public void SignIntoGooglePlayServices()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
        {
            switch(result)
            {
                case SignInStatus.Success:

                    isConnectedGooglePlayServices = true;
                    break;

                default:
                    isConnectedGooglePlayServices = false;
                    break;
            }
        });
    }

    void Start()
    {
        SignIntoGooglePlayServices();
        //start with a scale up and color shift
        scoreCounter = scoreThresh;       

        orbPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rotator>();
        if (selectedPack == null)
        {
            selectedPack = Packs[0];
            SwitchPack();
        }
        else
        {
            SwitchPack(lastPack);
        }
        foreach (GameObject point in GameObject.FindGameObjectsWithTag("Point"))
        {
            Spawnpoints.Add(point.transform);
        }

        debugPackName.text = selectedPack.gameObject.name;
        moneyText.text = "$" + Money;
        //StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameOver && !waiting)
        {
            Invoke("SpawnObject", waitTime);
            waiting = true;
        }
        if(GameOver && !pause)
        {
            GameObject[] Killobj = GameObject.FindGameObjectsWithTag("Object");
            foreach(GameObject obj in Killobj)
            {
                Destroy(obj);
            }
            mySM.GameOverSound();
            mySM.musicFilter.cutoffFrequency = 1000;
            //Time.timeScale = 0.001f;
            if(!watchedAd)
            {
                Invoke("OpenAd", 1f);
            }
            else
            {
                Invoke("GameOverFunc", 0.5f);
            }
            

            pause = true;

        }


        highscoreText.text = highScore.ToString();
        UpdateScore();

        if (scoreCounter >= scoreThresh && !pause)
        {
                mySM.ExpandSound();

            Scale(1f, ScaleFact);
           
            Debug.Log("Biogger");
            waitTime = Mathf.Clamp(waitTime - .125f, 0.4f, 3f);
            spawnLimite = Mathf.Clamp(spawnLimite + 1, 1, scoreThresh);
            level++;
            scoreCounter = 0;
            ScaleFact += new Vector3(0.125f, 0.125f, 0.125f);

            //particle system

            doParticles();


            //Give a break after each resize
            maxSpawn = 0;
            waiting = true;
        }

    }

    private void doParticles()
    {           

            myParts.GetComponent<ParticleSystemRenderer>().mesh = selectedPack.Objects[0].GetComponent<MeshFilter>().sharedMesh;
            myParts.startSize = selectedPack.Objects[0].transform.localScale.x * 2;
            myParts.Play();
    }

    public void StartGame()
    {
        GameOver = false;
        pause = false;
        waiting = false;
        Time.timeScale = 1;
        PlayButton.SetActive(false);
    }

    public void Continue()
    {
        watchedAd = true;
        adPanel.SetActive(false);
        GameOver = false;
        pause = false;
        Time.timeScale = 1.0f;
        waitTime += 2f;
        ScaleFact += new Vector3(0.25f, 0.25f, 0.25f);
        Scale(1f, ScaleFact);
        ObjectSpeed = 4.5f;
    }

    public void GameOverFunc()
    {
        AddMoney(Score);
        //Set the end UI
        FinishScore.text += " " + Score.ToString();
        FinishHighscore.text += " " + highScore.ToString();
        CurrentMoney.text += " $" + (Money - Score).ToString() + " + $" + Score.ToString();
        if(Score >= highScore)
        {
            FinishScore.text += "+$5 bonus";
            CurrentMoney.text += " + $5";
            AddMoney(4);

            if(this.isConnectedGooglePlayServices)
            {
                Social.ReportScore(Money, GPGSIds.leaderboard_money, (success) =>
                {
                    if (!success) Debug.Log("Failed to upload money highscore");
                });

                Social.ReportScore(Score, GPGSIds.leaderboard_highscore, (success) =>
                {
                    if (!success) Debug.Log("Failed to upload highscore");
                });

                if(Score >= 10)
                {
                    Social.ReportProgress(GPGSIds.achievement_grabbed10, 100.0f, null);
                }
                if (Score >= 20)
                {
                    Social.ReportProgress(GPGSIds.achievement_grabbed20, 100.0f, null);
                }
                if (Score >= 30)
                {
                    Social.ReportProgress(GPGSIds.achievement_grabbed30, 100.0f, null);
                }
                if (Score >= 40)
                {
                    Social.ReportProgress(GPGSIds.achievement_grabbed40, 100.0f, null);
                }
                if (Score >= 50)
                {
                    Social.ReportProgress(GPGSIds.achievement_grabbed50, 100.0f, null);
                }
                if (Score >= 60)
                {
                    Social.ReportProgress(GPGSIds.achievement_grabbed60, 100.0f, null);
                }
                if (Score >= 70)
                {
                    Social.ReportProgress(GPGSIds.achievement_grabbed70, 100.0f, null);
                }
                if (Score >= 80)
                {
                    Social.ReportProgress(GPGSIds.achievement_grabbed80, 100.0f, null);
                }
                if (Score >= 90)
                {
                    Social.ReportProgress(GPGSIds.achievement_grabbed90, 100.0f, null);
                }
                if (Score >= 100)
                {
                    Social.ReportProgress(GPGSIds.achievement_mastergrabber, 100.0f, null);
                }
            }

        }
        else if(this.isConnectedGooglePlayServices)
        {
            Social.ReportScore(highScore, GPGSIds.leaderboard_highscore, (success) =>
            {
                if (!success) Debug.Log("Failed to upload highscore");
            });

            Social.ReportScore(Money, GPGSIds.leaderboard_money, (success) =>
            {
                if (!success) Debug.Log("Failed to upload money highscore");
            });
        }

        CurrentMoney.text += " = $" + Money.ToString();
        //
        RetryPanel.SetActive(true);
        switchPackButton.SetActive(true);        
        this.SaveGame();
        pause = true;
    }

    public void SecretButton()
    {
        if(mySM.scoreVolume == 0)
        {
            Continue();
        }
    }

    void AddMoney(int profit)
    {
        Money += profit + 1;
        moneyText.text = "$" + Money;
    }

    public void SwitchPack(int packID)
    {
        selectedPack = Packs[packID];
        lastPack = packID;
        Objects.Clear();
        foreach(GameObject obj in selectedPack.Objects)
        {
            Objects.Add(obj);
        }
        this.SaveGame();
    }

    public void SwitchPack()
    {
        Objects.Clear();
        foreach (GameObject obj in selectedPack.Objects)
        {
            Objects.Add(obj);
        }
        this.SaveGame();
    }

    void UpdateScore()
    {
        Score = orbPlayer.itemsCaught;
        scoreText.text = Score.ToString();
        if (Score >= highScore)
        {            
            highScore = Score;
            highscoreText.text = highScore.ToString();
        }
    }

    void SpawnObject()
    {        
        if(maxSpawn >= 1)
        {
            Transform spawnPoint = Spawnpoints[Random.Range(0, Spawnpoints.Count)];
            if (spawnPoint.GetComponent<Warning>().Colliders.Count <= 0)
            {
                GameObject newOb = Instantiate(Objects[Random.Range(0, Objects.Count)], spawnPoint.position, Quaternion.Euler(Vector3.zero), SceneController.transform);
                newOb.GetComponent<Grabity>().speed = ObjectSpeed;
            }
            else
            {
                SpawnObject();
                return;
            }
            maxSpawn--;
            waiting = false;
            
            
        }
        else
        {
            //Debug.Log("Ye");
            maxSpawn = spawnLimite;
            waiting = false;
        }
    }

    private void Scale(float Seconds, Vector3 scaleTo)
    {
        StartCoroutine(ScaleOverSeconds(SceneController, scaleTo, Seconds));
    }

    public IEnumerator ScaleOverSeconds(GameObject objectToScale, Vector3 scaleTo, float seconds)
    {
        if(!GameOver)
        {
            
            float elapsedTime = 0;
            Vector3 startingScale = objectToScale.transform.localScale;
            Color newBGCol = new Color(Random.Range(0.1f, .3f), Random.Range(0.1f, .3f), Random.Range(0.1f, .3f), 1);
            Color newOBCol = new Color(Random.Range(0.5f, .85f), Random.Range(0.5f, .85f), Random.Range(0.5f, .85f), 1);
            Material matOB = OutOfBounds.GetComponent<MeshRenderer>().material;
            Material matBG = Background.GetComponent<MeshRenderer>().material;
            matOB.EnableKeyword("_Color");
            matBG.EnableKeyword("_Color");
            myParts.GetComponent<ParticleSystemRenderer>().material.SetColor(Shader.PropertyToID("_Color"), newOBCol);
            while (elapsedTime < seconds)
            {
                if (mySM.musicFilter.cutoffFrequency <= 8000)
                {
                    mySM.musicFilter.cutoffFrequency = Mathf.Lerp(mySM.musicFilter.cutoffFrequency, 8000, Time.deltaTime * 2.2f);
                }
                matOB.SetColor(Shader.PropertyToID("_Color"), Color.Lerp(matOB.color, newOBCol, Time.deltaTime * 2.2f));
                matBG.SetColor(Shader.PropertyToID("_Color"), Color.Lerp(matBG.color, newBGCol, Time.deltaTime * 2.2f));
                objectToScale.transform.localScale = Vector3.Lerp(startingScale, scaleTo, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToScale.transform.localScale = scaleTo;
            matBG.SetColor(Shader.PropertyToID("_Color"), newBGCol);
            matOB.SetColor(Shader.PropertyToID("_Color"), newOBCol);            
            StopCoroutine("ScaleOverSeconds");
        }
        else
        {
            StopCoroutine("ScaleOverSeconds");
        }
        
    }

    //public IEnumerator ColorShift(GameObject objectToShift,  float seconds, float colorRange)
    //{
    //    float elapsedTime = 0;
    //    Material mat = objectToShift.GetComponent<MeshRenderer>().material;
        
    //    while (elapsedTime < seconds)
    //    {
           
    //        elapsedTime += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //    mat.SetColor(Shader.PropertyToID("_Color"), newCol);
    //    Debug.Log("Finish");
    //    StopCoroutine("ColorShift");
    //}

    public void OpenShop()
    {
        ShopPanel.SetActive(true);
        moneyBG.SetActive(true);
        mySM.OpenSound();
    }
    public void CloseShop()
    {
        ShopPanel.SetActive(false);
        moneyBG.SetActive(false);
        mySM.CloseSound();
    }

    public void OpenAd()
    {
        adPanel.SetActive(true);
        myRW.LoadAd();
    }

    public void CloseAd()
    {
        adPanel.SetActive(false);
        mySM.CloseSound();
        Invoke("GameOverFunc", 0.1f);
    }

    public void adFinished()
    {
        adPanel.SetActive(false);
        Invoke("Continue", 1.0f);
    }

    public void adCancel()
    {
        CloseAd();
    }

    public void ShowLeaderboard()
    {
        if(Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }

    //Was for debugging purposes.
    public void debugSwitchPack()
    {
        if(Packs.Count-1 > debugInt)
        {
            debugInt++;
            SwitchPack(debugInt);
            debugPackName.text = selectedPack.gameObject.name;
        }
        else
        {
            debugInt = 0;
            SwitchPack(debugInt);
            debugPackName.text = selectedPack.gameObject.name;
        }

    }
    public void RestartScene()
    {
        this.SaveGame();
        Invoke("ReloadScene", 0.1f);       
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void enablePack(int packIndex)
    {
        packsEnabled[packIndex] = true;       
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(gameObject.GetComponent<GameManager>());
        Debug.Log("Saved Game");
    }
}
