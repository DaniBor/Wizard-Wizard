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
    [SerializeField] private List<Wizard> playerWizards;
    [SerializeField] private List<Wizard> enemyWizards;
    [SerializeField] private Dictionary<string, int> enemyWizardSupply = new Dictionary<string, int>();

    private int floorCount;
    private int spawnTimer;
    private int timeTilSpawn;
    private List<Wizard> deadWizards = new List<Wizard>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        input.gameObject.SetActive(false);


        floorCount = 0;
        //It is assumed that the game starts over on Awake
        StartFloorOne();
    }

    private void LateUpdate()
    {
        if(deadWizards.Count > 0)
        {
            foreach(Wizard wiz in deadWizards)
            {
                playerWizards.Remove(wiz);
                enemyWizards.Remove(wiz);
                if(!wiz.IsDestroyed())
                    Destroy(wiz.gameObject);
            }
            if(enemyWizards.Count == 0)
                stairs.SetActive(true);
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
            default:
                break;
        }

        input.text = string.Empty;
        input.gameObject.SetActive(false);
        forceDeselectButton.Select();
    }

    private void SpawnWizard(Transform t, GameObject wizard, bool isAlly)
    {
        Wizard wiz = Instantiate(wizard, t.position, t.rotation).GetComponent<Wizard>();
        wiz.isAlly = isAlly;
        if (isAlly)
            playerWizards.Add(wiz);
        else
            enemyWizards.Add(wiz);
    }

    #region Getters
    public List<Wizard> getWizards(bool isAlly)
    {
        return isAlly == true ? enemyWizards : playerWizards;
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

    public void StartNextFloor()
    {
        stairs.SetActive(false);

        if(floorCount == 10)
        {

        }
        else
        {

        }

        SceneManager.LoadScene("End Screen");
    }

    private void StartFloorOne()
    {
        enemyWizardSupply.Add("fire", 5);
        enemyWizardSupply.Add("magic", 5);
    }

}
