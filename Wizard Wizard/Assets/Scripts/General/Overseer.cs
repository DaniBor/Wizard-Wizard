using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        input.gameObject.SetActive(false);
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
        Instantiate(wizard, t.position, t.rotation);
    }

    #region Getters
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

    public void OnAllyWizardKill(Wizard wiz)
    {
        playerWizards.Remove(wiz);
    }

    public void OnEnemyWizardKill(Wizard wiz)
    {
        enemyWizards.Remove(wiz);
        if(enemyWizards.Count == 0)
        {
            foreach(Wizard w in playerWizards)
            {
                w.DamageMe(new Damage(99999, Damage.DamageType.MAGIC));
            }
        }
        stairs.SetActive(true);
    }

    public void StartNextFloor()
    {
        stairs.SetActive(false);
        SceneManager.LoadScene("End Screen");
    }

    
}
