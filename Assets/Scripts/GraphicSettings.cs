using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
public class GraphicSettings : MonoBehaviour
{
    [SerializeField] private RenderPipelineAsset[] qualityLevels;
    [SerializeField] private TMP_Dropdown graphicDropdown;

    private void Start()
    {
        if(PlayerPrefs.HasKey("Quality"))
        {
            graphicDropdown.value = PlayerPrefs.GetInt("Quality");
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
            QualitySettings.renderPipeline = qualityLevels[PlayerPrefs.GetInt("Quality")];
        }
        else
        {
            QualitySettings.SetQualityLevel(graphicDropdown.value);
            QualitySettings.renderPipeline = qualityLevels[graphicDropdown.value];
            PlayerPrefs.SetInt("Quality", graphicDropdown.value);
        }
    }

    public void ChangeLevel(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevels[value];
        PlayerPrefs.SetInt("Quality", value);
    }
}
