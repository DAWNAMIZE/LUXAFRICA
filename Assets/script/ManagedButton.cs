using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ManagedButton : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(this.onClick);
    }

    private void onClick()
    {
        UIManager.Main.OnClick(this.name);
    }

    // Update is called once per frame
    void Update()
    {
        this.button.interactable = !UIManager.Main.isDisabled(this.name);
    }
}
