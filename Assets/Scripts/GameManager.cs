using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private GameObject moneyBag;

    [SerializeField]
    private Text coinTxt;

    private int collectedCoins;

    private int elfCoins;

    //private int coins;

    public GameObject CoinPrefab
    {
        get
        {
            return coinPrefab;
        }
    }

    public GameObject MoneyBag
    {
        get
        {
            return moneyBag;            
        }
    }

    public int CollectedCoins
    {
        get
        {
            return collectedCoins;
        }

        set
        {
            coinTxt.text = value.ToString();
            collectedCoins = value;
        }
    }

    public int ElfCoins
    {
        get
        {
            return elfCoins;
        }

        set
        {
            elfCoins = value;
        }
    }

    //public int Coins
    //{
    //    get
    //    {
    //        return coins;
    //    }

    //    set
    //    {
    //        coins = CollectedCoins + ElfCoins;
    //        Debug.Log(coins);
    //    }
    //}
}
