using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    
    [SerializeField] GameObject moneyPrefab;

    [SerializeField] public int noOfMoneyObjects; 
    Transform moneyDropPos;
    [SerializeField] public float offset = 2f ;


    private void Start()
    {
        moneyDropPos = this.transform; // Assign the current object's transform

        for (int i = 0; i < noOfMoneyObjects; i++)
        {
            Vector2 instantiateOffeset = new Vector2(Random.Range(0, 2) * offset, 0);
            moneyDropPos.position = new Vector3(this.gameObject.transform.position.x + instantiateOffeset.x, this.gameObject.transform.position.y + instantiateOffeset.y, 0);
            GameObject moneyObject = Instantiate(moneyPrefab, moneyDropPos.position, Quaternion.identity);
        }
    }

}
