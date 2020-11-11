using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Instance
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion


    public bool IsGameStarted { get; set; }
    public Player player;
    public Crosshair crosshair;
    
  
    void Start()
    {
        Player newPlayer = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity) as Player;
        //Crosshair newCrosshair = Instantiate(crosshair, new Vector3(0, 0, 0), Quaternion.Euler(90,0,0)) as Crosshair;
        Crosshair newCrosshair = Instantiate(crosshair, new Vector3(0, 0, 0), Quaternion.identity) as Crosshair;
        newCrosshair.transform.SetParent(GameObject.Find("Canvas").transform, false);
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
