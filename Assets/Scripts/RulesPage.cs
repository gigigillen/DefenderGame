using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesPage : MonoBehaviour
{

    [SerializeField] private GameObject menuUI;

    [SerializeField] private GameObject rulesUI;

    [SerializeField] private GameObject rulesText;

    public void RulesOpen()
    {
        menuUI.SetActive(false);
        rulesUI.SetActive(true);
    }

    public void RulesClose()
    {
        menuUI.SetActive(true);
        rulesUI.SetActive(false);
    }
}
