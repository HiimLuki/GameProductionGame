using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    [Header("Minion entity values")]
    [Tooltip("Scriptable object reference to general minion values")]
    public MinionScriptableObject minionValues;

    // Start is called before the first frame update
    void Start()
    {
        // Start enumerator to kill minions after they were alive for their specified time
        StartCoroutine("KillMinion");
        // Add force to rigidbody of minion, slightly randomized so it flys away from the player
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        Vector2 forceVector = new Vector2(Random.Range(-1f, 1f), Random.Range(2f, 4f));
        rb.AddForce(forceVector, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator KillMinion()
    {
        yield return new WaitForSeconds(minionValues.timeAlive);
        Destroy(this.gameObject);
    }
}
