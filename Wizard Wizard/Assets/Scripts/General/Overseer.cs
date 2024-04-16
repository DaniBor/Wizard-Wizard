using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Overseer : MonoBehaviour
{
    public static Overseer Instance { get; private set; }

    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button forceDeselectButton;
    [SerializeField] private Player player;
    [SerializeField] private GameObject stairs;

    [SerializeField] private GameObject[] wizardPrefabs;
    [SerializeField] private GameObject finalBoss;
    [SerializeField] private List<Wizard> playerWizards;
    [SerializeField] private List<Wizard> enemyWizards;
    private List<int> enemyWizardSupply = new List<int>();
    //private Dictionary<string, int> playerSpawnStatistic = new Dictionary<string, int>();

    private int floorCount;
    private float spawnTimer;
    private float timeTilSpawn;
    private int spawnsPerWave;
    private List<Wizard> deadWizards = new List<Wizard>();

    [SerializeField] private AudioSource music;

    bool flooractive;

    [SerializeField] private List<Transform> spawnpoints;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        input.gameObject.SetActive(false);

        floorCount = 1;
        spawnsPerWave = 5;
        spawnTimer = 5;
        timeTilSpawn = spawnTimer;

        stairs.SetActive(false);

        enemyWizardSupply.Add(0); //Magic
        enemyWizardSupply.Add(0); //Fire
        enemyWizardSupply.Add(0); //Lightning
        enemyWizardSupply.Add(0); //Earth
        enemyWizardSupply.Add(0); //Strength
        enemyWizardSupply.Add(0); //Air
        enemyWizardSupply.Add(0); //Bulwark


        music.Play();

        //It is assumed that the game starts over on Awake
        StartNextFloor();
    }

    public void StartNextFloor()
    {
        stairs.SetActive(false);
        timeTilSpawn = spawnTimer;
        player.refillHealth();

        switch (floorCount)
        {
            case 1:
                enemyWizardSupply[0] = 5;
                enemyWizardSupply[1] = 5;

                break;
            case 2:
                enemyWizardSupply[0] = 5;
                enemyWizardSupply[1] = 5;
                enemyWizardSupply[2] = 2;
                break;
            case 3:
                spawnsPerWave++;
                enemyWizardSupply[0] = 7;
                enemyWizardSupply[1] = 5;
                enemyWizardSupply[2] = 2;
                enemyWizardSupply[3] = 2;

                break;
            case 4:
                enemyWizardSupply[0] = 8;
                enemyWizardSupply[1] = 8;
                enemyWizardSupply[2] = 5;
                enemyWizardSupply[3] = 3;
                enemyWizardSupply[4] = 1;
                break;
            case 5:
                enemyWizardSupply[0] = 10;
                enemyWizardSupply[1] = 10;
                enemyWizardSupply[2] = 8;
                enemyWizardSupply[3] = 6;
                enemyWizardSupply[4] = 2;
                enemyWizardSupply[5] = 3;
                break;
            case 6:
                spawnsPerWave++;
                enemyWizardSupply[0] = 12;
                enemyWizardSupply[1] = 10;
                enemyWizardSupply[2] = 4;
                enemyWizardSupply[3] = 8;
                enemyWizardSupply[4] = 2;
                enemyWizardSupply[5] = 4;
                enemyWizardSupply[6] = 2;
                break;
            case 7:
                enemyWizardSupply[0] = 13;
                enemyWizardSupply[1] = 11;
                enemyWizardSupply[2] = 5;
                enemyWizardSupply[3] = 9;
                enemyWizardSupply[4] = 3;
                enemyWizardSupply[5] = 5;
                enemyWizardSupply[6] = 3;
                break;
            case 8:
                enemyWizardSupply[0] = 14;
                enemyWizardSupply[1] = 12;
                enemyWizardSupply[2] = 6;
                enemyWizardSupply[3] = 10;
                enemyWizardSupply[4] = 4;
                enemyWizardSupply[5] = 6;
                enemyWizardSupply[6] = 4;
                break;
            case 9:
                spawnsPerWave++;
                enemyWizardSupply[0] = 15;
                enemyWizardSupply[1] = 15;
                enemyWizardSupply[2] = 8;
                enemyWizardSupply[3] = 11;
                enemyWizardSupply[4] = 5;
                enemyWizardSupply[5] = 7;
                enemyWizardSupply[6] = 5;
                break;
            case 10:
                StartFinalBoss();
                break;
            default:
                Debug.LogError("Invalid Floor!");
                break;
        }

        floorCount++;
    }


    private void Start()
    {

    }

    private void Update()
    {
        timeTilSpawn -= Time.deltaTime;
        if(timeTilSpawn <= 0f)
        {
            timeTilSpawn += spawnTimer;
            if(checkIfWizardsLeft())
                SpawnWave();
        }
    }

    private void LateUpdate()
    {
        if(deadWizards.Count > 0)
        {
            foreach(Wizard wiz in deadWizards)
            {
                playerWizards.Remove(wiz);
                enemyWizards.Remove(wiz);

                if(wiz.gameObject.name == "Player")
                {
                    GameOver(false);
                }

                if (wiz.gameObject.name == "FinalBoss")
                {
                    Debug.Log("Final Boss Killed");
                    GameOver(true);
                }

                if (!wiz.IsDestroyed())
                {
                    Destroy(wiz.gameObject);
                }
                    
            }
            deadWizards.Clear();

            if(enemyWizards.Count == 0 && !checkIfWizardsLeft())
            {
                stairs.SetActive(true);
            }
                
        }
    }

    public void startInput()
    {
        input.gameObject.SetActive(true);
        input.Select();
    }

    public void handleInput()
    {
        string str = input.text.ToLower();

        switch (str)
        {
            case "magic":
                SpawnWizard(player.transform, wizardPrefabs[0], true);
                break;
            case "fire":
                SpawnWizard(player.transform, wizardPrefabs[1], true);
                break;
            case "lightning":
                SpawnWizard(player.transform, wizardPrefabs[2], true);
                break;
            case "earth":
                SpawnWizard(player.transform, wizardPrefabs[3], true);
                break;
            case "strength":
                SpawnWizard(player.transform, wizardPrefabs[4], true);
                break;
            case "air":
                SpawnWizard(player.transform, wizardPrefabs[5], true);
                break;
            case "bulwark":
                SpawnWizard(player.transform, wizardPrefabs[6], true);
                break;
            default:
                break;
        }

        input.text = string.Empty;
        input.gameObject.SetActive(false);
        forceDeselectButton.Select();
    }

    private void SpawnWizard(Transform t, GameObject wizard, bool isAlly)
    {
        Wizard wiz;
        
        if (isAlly)
        {
            if (player.hasMana(wizard.GetComponent<Wizard>().getCost()))
            {
                wiz = Instantiate(wizard, t.position, t.rotation).GetComponent<Wizard>();
                playerWizards.Add(wiz);
                wiz.isAlly = isAlly;
                //playerSpawnStatistic[input.text] += 1;
                player.spendMana(wizard.GetComponent<Wizard>().getCost());
            }
            
        }
        else
        {
            wiz = Instantiate(wizard, t.position, t.rotation).GetComponent<Wizard>();
            enemyWizards.Add(wiz);
            wiz.isAlly = isAlly;
        }
            
    }

    public void SpawnRandomEnemyWizard(Transform t)
    {
        int randWiz = Random.Range(0, wizardPrefabs.Length);
        SpawnWizard(t, wizardPrefabs[randWiz], false);
    }

    #region Getters
    public List<Wizard> getWizards(bool isAlly)
    {
        return isAlly == true ? enemyWizards : playerWizards;
    }

    public List<Wizard> getWizardsInRange(Transform t, int range, bool isAlly)
    {
        List<Wizard> list = new List<Wizard>();

        foreach (Wizard wiz in getWizards(isAlly))
        {
            if(Vector3.Distance(t.position, wiz.transform.position) < range)
            {
                list.Add(wiz);
            }
        }

        return list;
    }

    public List<Wizard> getWizardsInRange(Transform t, float range, bool isAlly)
    {
        List<Wizard> list = new List<Wizard>();

        foreach (Wizard wiz in getWizards(isAlly))
        {
            if (Vector3.Distance(t.position, wiz.transform.position) < range)
            {
                list.Add(wiz);
            }
        }

        return list;
    }

    public List<Wizard> getPlayerWizards()
    {
        return playerWizards;
    }

    public List<Wizard> getEnemyWizards()
    {
        return enemyWizards;
    }

    public Wizard getClosestEnemyWizard(Transform t)
    {
        Wizard result = null;
        foreach(Wizard wiz in  enemyWizards)
        {
            if(result == null)
                result = wiz;
            if(Vector3.Distance(transform.position, result.transform.position) >
                Vector3.Distance(transform.position, wiz.transform.position))
            {
                result = wiz;
            }
        }
        return result;
    }

    public Wizard getClosestPlayerWizard(Transform t)
    {
        Wizard result = null;
        foreach (Wizard wiz in playerWizards)
        {
            if (result == null)
                result = wiz;
            if (Vector3.Distance(transform.position, result.transform.position) >
                Vector3.Distance(transform.position, wiz.transform.position))
            {
                result = wiz;
            }
        }
        return result;
    }


    #endregion

    public void OnWizardKill(Wizard wiz)
    {
        deadWizards.Add(wiz);
    }

    private void StartFinalBoss()
    {
        Wizard finalboss = Instantiate(finalBoss, Vector3.zero, Quaternion.identity).GetComponent<Wizard>();
        finalboss.name = "FinalBoss";
           
        enemyWizards.Add(finalboss);
    }

    private void SpawnWave()
    {
        if(enemyWizardSupply.Count > 0)
        {
            for(int i = 0; i < spawnsPerWave; i++)
            {
                bool found = false;
                int wizIndex = 0;
                while (!found)
                {
                    wizIndex = Random.Range(0, enemyWizardSupply.Count);
                    if (enemyWizardSupply[wizIndex] > 0)
                        found = true;
                }
                
                int spawnIndex = Random.Range(0, spawnpoints.Count);
                SpawnWizard(spawnpoints[spawnIndex], wizardPrefabs[wizIndex], false);
                enemyWizardSupply[wizIndex]--;
                if (!checkIfWizardsLeft())
                    break;
            }
        }
    }

    private bool checkIfWizardsLeft()
    {
        int result = 0;
        foreach(int i in enemyWizardSupply)
        {
            result += i;
        }
        return result > 0;
    }

    private void GameOver(bool hasWon)
    {
        if(hasWon)
        {
            SceneManager.LoadScene("End Screen");
        }
        else
        {
            SceneManager.LoadScene("Game Over");
        }
    }
}
