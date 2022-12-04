using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_FORMATION : MonoBehaviour
{
    public GameObject Prefab;

    public float xOffset = 1.5f;
    public float yOffset = 3.75f;
    public int numCheerleaders = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            numCheerleaders++;
            SetFormation();
        }
    }

    public void SetFormation()
    {
        GameObject gO = GetCheerleader();
        Vector3 Origin = Vector3.zero;
        int currentCheerleader = 1;
        int currentColumnNumber = 1;
        int cheerleadersInCurrentColumn;
        while (currentCheerleader <= numCheerleaders)
        {
            cheerleadersInCurrentColumn = 1;
            Vector3 position = new Vector3(Origin.x + (currentColumnNumber - 1) * xOffset, Origin.y);
            while (cheerleadersInCurrentColumn <= currentColumnNumber && currentCheerleader <= numCheerleaders)
            {
                gO.transform.SetParent(transform);
                gO.transform.position = position;
                position += new Vector3(-xOffset / 2, yOffset, 0);
                cheerleadersInCurrentColumn++;
                currentCheerleader++;
            }
            currentColumnNumber++;
        }
    }

    public GameObject GetCheerleader()
    {
        GameObject gO = Instantiate(Prefab);
        return gO;
    }
}
