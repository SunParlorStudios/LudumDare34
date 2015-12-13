using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour
{
    public Vector2 worldSize;

    public GameObject[] asteroidsFirstLayer;
    public int firstLayerMinimum;
    public int firstLayerMaximum;

    public GameObject[] asteroidsSecondLayer;
    public int secondLayerMinimum;
    public int secondLayerMaximum;

    public GameObject[] asteroidsThirdLayer;
    public int thirdLayerMinimum;
    public int thirdLayerMaximum;

    public GameObject[] asteroidsFourthLayer;
    public int fourthLayerMinimum;
    public int fourthLayerMaximum;

    public void Awake()
    {
        int numAsteroids = Random.Range(firstLayerMinimum, firstLayerMaximum);

        for (int i = 0; i < numAsteroids; i++)
        {
            GameObject curr = asteroidsFirstLayer[Random.Range(0, asteroidsFirstLayer.Length)];

            Instantiate(
                curr,
                new Vector3(Random.Range(-(worldSize.x / 2), worldSize.x / 2), Random.Range(-(worldSize.y / 2), worldSize.y / 2), curr.transform.position.z),
                Quaternion.Euler(0, 0, Random.Range(0, 360))
            );
        }

        numAsteroids = Random.Range(secondLayerMinimum, secondLayerMaximum);

        for (int i = 0; i < numAsteroids; i++)
        {
            GameObject curr = asteroidsSecondLayer[Random.Range(0, asteroidsSecondLayer.Length)];

            GameObject obj = (GameObject)Instantiate(
                curr,
                new Vector3(Random.Range(-(worldSize.x / 2), worldSize.x / 2), Random.Range(-(worldSize.y / 2), worldSize.y / 2), curr.transform.position.z),
                Quaternion.Euler(0, 0, Random.Range(0, 360))
            );

            float scaleFactor = (Random.value * 0.1f + 0.05f);

            obj.transform.localScale = new Vector3(
                obj.transform.localScale.x * scaleFactor,
                obj.transform.localScale.y * scaleFactor,
                obj.transform.localScale.z * scaleFactor
            );

            obj.GetComponent<SpriteRenderer>().material.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        }

        numAsteroids = Random.Range(thirdLayerMinimum, thirdLayerMaximum);

        for (int i = 0; i < numAsteroids; i++)
        {
            GameObject curr = asteroidsThirdLayer[Random.Range(0, asteroidsThirdLayer.Length)];

            GameObject obj = (GameObject)Instantiate(
                curr,
                new Vector3(Random.Range(-(worldSize.x / 2), worldSize.x / 2), Random.Range(-(worldSize.y / 2), worldSize.y / 2), curr.transform.position.z),
                Quaternion.Euler(0, 0, Random.Range(0, 360))
            );

            obj.GetComponent<SpriteRenderer>().material.color = new Color(0.4f, 0.4f, 0.4f, 1.0f);
        }

        numAsteroids = Random.Range(fourthLayerMinimum, fourthLayerMaximum);

        for (int i = 0; i < numAsteroids; i++)
        {
            GameObject curr = asteroidsFourthLayer[Random.Range(0, asteroidsFourthLayer.Length)];

            GameObject obj = (GameObject)Instantiate(
                curr,
                new Vector3(Random.Range(-(worldSize.x / 2), worldSize.x / 2), Random.Range(-(worldSize.y / 2), worldSize.y / 2), curr.transform.position.z),
                Quaternion.Euler(0, 0, Random.Range(0, 360))
            );

            float scaleFactor = 1 + (Random.value * 0.1f + 0.05f);

            obj.transform.localScale = new Vector3(
                obj.transform.localScale.x * scaleFactor,
                obj.transform.localScale.y * scaleFactor,
                obj.transform.localScale.z * scaleFactor
            );

            obj.GetComponent<SpriteRenderer>().material.color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
        }
    }
}
