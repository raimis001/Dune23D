using System;
using UnityEngine;
using UnityEngine.UI;

public class UiConstruction : MonoBehaviour
{

    [SerializeField]
    Image progress;
    [SerializeField]
    GameObject buildButton;
    [SerializeField]
    GameObject placeButton;

    [SerializeField]
    BuildingConstruction building;

    private void OnEnable()
    {
        building.OnBuildingReady += BuildingReady;
        Build.OnBuild += OnBuildingBuild;
    }

    private void OnDisable()
    {
        building.OnBuildingReady -= BuildingReady;
        Build.OnBuild -= OnBuildingBuild;
    }

    private void BuildingReady()
    {
        buildButton.SetActive(false);
        placeButton.SetActive(true);
    }

    private void OnBuildingBuild()
    {
        buildButton.SetActive(true);
        placeButton.SetActive(false);

        building.Reset();
    }

    private void Update()
    {
        progress.fillAmount = building.progress;
    }

    public void OnBuildButton()
    {
        building.StartBuild();
        buildButton.SetActive(false);
        placeButton.SetActive(false);
    }

    public void OnPlaceButton()
    {
        if (!building.ready)
            return;

        Build.StartPlace(building.currentBuild);
    }
}
