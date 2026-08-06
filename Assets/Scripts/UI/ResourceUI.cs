using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    public TMP_Text woodText;
    public TMP_Text goldText;
    //public TMP_Text populationText;

    private void Update()
    {
        woodText.text = ResourceManager.Instance.wood.ToString();
        goldText.text = ResourceManager.Instance.gold.ToString();

        // na razie przykładowe wartości
        //goldText.text = "0";
        //populationText.text = "3 / 20";
    }
}