using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Toggle))]
public class ManagedToggle : MonoBehaviour
{

    private Toggle button;
    void Start()
    {
        button = this.GetComponent<Toggle>();
        button.onValueChanged.AddListener(this.onChange);
    }

    private void onChange(bool value)
    {
        UIManager.Main.Set(this.name, value);
    }

    // Update is called once per frame
    void Update()
    {
       // this.button.interactable = !UIManager.Main.isDisabled(this.name);
        this.button.SetIsOnWithoutNotify(UIManager.Main.GetBool(this.name));
    }
}
