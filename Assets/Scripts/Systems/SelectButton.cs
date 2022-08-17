using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    public Button defaultButton;

    // Start is called before the first frame update
    void Start()
    {
        defaultButton.Select();
    }
}
