using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    public static float Produce;
    public static float Using;

    [SerializeField]
    Image progressProduce;
    [SerializeField]
    Image progressUsing;

    private void Update()
    {

        if (Produce == 0)
        {
            progressProduce.fillAmount = 1;
            progressUsing.fillAmount = Using > 0 ? 0 : 1;
            return;
        }

        if (Produce >= Using)
        {
            progressProduce.fillAmount = 0;
            progressUsing.fillAmount = 1f - Using / Produce;

            return;
        }

        progressProduce.fillAmount = 1f - Produce / Using;
        progressUsing.fillAmount = 0;



    }
}
