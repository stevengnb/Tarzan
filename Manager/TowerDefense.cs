using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NormalMode,
    TowerDefense
}

public class TowerDefense : MonoBehaviour
{
    public static TowerDefense instance { get; private set; }
    private GameState currState;
    private int wave;
    public bool isChanging = false;
    [SerializeField] private GameObject td;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            wave = 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }
        
    private void Start()
    {
        currState = GameState.NormalMode;
    }

    public void SwitchDefense()
    {
        currState = GameState.TowerDefense;
        td.SetActive(true);
        isChanging = true;
    }

    public void SwitchNormal()
    {
        wave++;
        currState = GameState.NormalMode;
        td.SetActive(false);
        isChanging = true;
    }

    public GameState CurrState
    {
        get { return currState; }
    }

    public int GetWave()
    {
        return wave;
    }
}
