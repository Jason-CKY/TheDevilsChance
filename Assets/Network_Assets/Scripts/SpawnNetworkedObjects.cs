using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class SpawnNetworkedObjects : NetworkBehaviour
{
    public GameObject[] diamond_Prefabs, monsters_Prefabs;
    //public GameObject Door_Prefab;
    GameObject[] diamonds, monsters;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Multiplayer_Canvas"))
        {
            GameObject.Find("Multiplayer_Canvas").SetActive(false);
        }
        if (isLocalPlayer)
        {
            if (isServer)
            {
                diamonds = new GameObject[diamond_Prefabs.Length];
                for (int i = 0; i < diamond_Prefabs.Length; i++)
                {
                    diamonds[i] = Instantiate(diamond_Prefabs[i]);
                    NetworkServer.Spawn(diamonds[i]);
                }

                monsters = new GameObject[monsters_Prefabs.Length];
                for(int i=0; i<monsters_Prefabs.Length; i++)
                {
                    monsters[i] = Instantiate(monsters_Prefabs[i]);
                    NetworkServer.Spawn(monsters[i]);
                }

                //NetworkServer.Spawn(Instantiate(Door_Prefab));
            }

        }

    }
}
