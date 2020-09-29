using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField]
        Text damageText = null;

        public void SetValue(float value)
        {
           this.damageText.text = string.Format("{0:0}", value); 
        }
    }
}