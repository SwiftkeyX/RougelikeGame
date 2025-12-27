using UnityEngine;

/// <summary>
/// Due to this being like Hades, Enemy should always know where player is, IF PLAYER IS IN THEIR ROOM
/// </summary>
public class Room : MonoBehaviour
{
    private EnemyStateMachine[] enemies;

    void Awake()
    {
        // get all enemy in the room
        enemies = GetComponentsInChildren<EnemyStateMachine>();
    }

    /// <summary>
    /// when Player enter the room, make all enemy in the room alert
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Alert = true;
        }
    }


}