using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MinionObject", order = 1)]
public class MinionScriptableObject : ScriptableObject
{
    [Header("Minion Stats")]
    [Tooltip("How long the minions are alive after spawned")]
    public float timeAlive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
