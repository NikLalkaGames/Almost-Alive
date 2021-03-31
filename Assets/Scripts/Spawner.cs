using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    private IEnumerator spawn;

    private int myCount;
    private int myCheck;
    private Vector3 ranpos;
    private int letters;
    public bool getKilled = false;
    // Start is called before the first frame update


    private
    void Start()
    {

/*         Generator("Trash/building_1", 2, 2);
        Generator("Trash/building_2", 2, 2);
        Generator("Trash/building_3", 2, 2);
        Generator("Trash/building_4", 2, 2);
        Generator("Trash/grass_1", 5, 5);
        Generator("Trash/grass_2", 5, 5);
        Generator("Trash/grass_3", 5, 5);
        Generator("Trash/grass_4", 5, 5);
        Generator("HumanBlue", 5, 5);
        Generator("HumanYellow", 5, 5);
        Generator("HumanPurple", 5, 5);
        Generator("HumanPink", 5, 5);
        Generator("HumanGreen", 5, 5); */



        Generator("Trash/bus_stop_1", 3);
        Generator("Trash/bus_stop_2", 3);
        Generator("Trash/lantern1", 5);
        Generator("Trash/lantern2", 5);
        Generator("Trash/grass_1", 10);
        Generator("Trash/grass_2", 10);
        Generator("Trash/grass_3", 10);
        Generator("Trash/grass_4", 10);
        Generator("Trash/rocks_1", 8);
        Generator("Trash/rocks_2", 8);
        Generator("Trash/tree1", 2);
        Generator("Trash/tree2", 2);
        Generator("Trash/tree3", 2);
        Generator("Trash/tree4", 2);
        Generator("Trash/tree5", 2);
        Generator("Trash/tree6", 2);
        Generator("Trash/bush1", 6);
        Generator("Trash/bush2", 6);
        Generator("Trash/bush3", 6);

        Generator("HumanBlue", 6);
        Generator("HumanYellow", 6);
        Generator("HumanPurple", 6);
        Generator("HumanPink", 6);
        Generator("HumanGreen", 6);

    }

    public void GenerateMatchingHuman(EmotionColor killedColor)
    {
        switch (killedColor)
        {
            case EmotionColor.pink    :    Generator("HumanPink", 1); break;
            case EmotionColor.blue    :    Generator("HumanBlue", 1); break;
            case EmotionColor.yellow  :    Generator("HumanYellow", 1); break;
            case EmotionColor.purple  :    Generator("HumanPurple", 1); break;
            case EmotionColor.green   :    Generator("HumanGreen", 1); break;
        }
        Debug.Log("Revival of human is successful (ReInstantiation)");
    }

    private void Generator(string obj, int myCount)
    {
        for(int i = 0; i < myCount; i++) 
        {
            do 
            {
                myCheck = 0;
                ranpos = new Vector3( Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), 0 );
                Collider[] hitColliders = Physics.OverlapSphere(ranpos, 1f);
                for(int j = 0; j < hitColliders.Length; j++) 
                {
                    myCheck++;
                }
            }
            while (myCheck > 0);

            Instantiate(Resources.Load(obj), ranpos, Quaternion.identity);  
        }
    }

    Vector3 GetFreespawnPosition()
    {
        Vector3 spawnPosition;
        Collider[] collisions = new Collider[100];
        do
        {
            spawnPosition = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20f, 20f), 0);
            
        }
        while(Physics.OverlapSphereNonAlloc(spawnPosition, 1f, collisions) > 0);

        return spawnPosition;
    }


    private void Generator(string Object, int min, int max)
    {
        letters = Random.Range(min, max);
        while (letters > 0)
        {
            var human  = Instantiate(Resources.Load(Object), GetFreespawnPosition(), Quaternion.identity);
            letters--;
        }
    }
}