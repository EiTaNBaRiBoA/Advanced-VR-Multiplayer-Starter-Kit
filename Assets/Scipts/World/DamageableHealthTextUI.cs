using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageableHealthTextUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Damageable damageable;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Health: " + damageable.health;
    }
}
