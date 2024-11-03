using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    public static float Produce => GetProduce();
    public static float Using;

    public static bool HaveEnergy => Produce >= Using;

    [SerializeField]
    Image progressProduce;
    [SerializeField]
    Image progressUsing;

    static List<BuildingGenerator> producers = new List<BuildingGenerator>();

    public static void AddProducer(Building building)
    {
        producers.Add((BuildingGenerator)building);
    }

    static float GetProduce()
    {
        float result = 0;

        foreach (BuildingGenerator gen in producers)
            result += gen.currentEnergy;

        return result;
    }

    private void Update()
    {
        float prod = Produce;
        if (prod == 0)
        {
            progressProduce.fillAmount = 1;
            progressUsing.fillAmount = Using > 0 ? 0 : 1;
            return;
        }

        if (prod >= Using)
        {
            progressProduce.fillAmount = 0;
            progressUsing.fillAmount = 1f - Using / prod;

            return;
        }

        progressProduce.fillAmount = 1f - prod / Using;
        progressUsing.fillAmount = 0;



    }
}
