using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace krisnart.ControllerTut.Example
{
    public class ExecuteTut : MonoBehaviour
    {
        public Dropdown ButtonDropDown;
        public Dropdown HandDropDown;

        List<string> OculusButtons = new List<string> { "Trigger", "ThumbStick", "Grip", "Menu", "A_X", "B_Y" };
        List<string> ViveButtons = new List<string> { "Trigger", "Trackpad", "Grip", "Menu", "SystemMenu" };
        List<string> MRButtons = new List<string> { "Trigger", "ThumbStick", "Grip", "Menu", "Trackpad", "SystemMenu" };

        private void Start()
        {
            SetButtonDropDown();            
        }

        void SetButtonDropDown()
        {
            switch (ControllerButton.instance.HMDmodel)
            {
                case "htc":
                    SetOptions(ViveButtons);
                    break;
                case "mr":
                    SetOptions(MRButtons);
                    break;
                default:
                    SetOptions(OculusButtons);
                    break;
            }
        }

        void SetOptions(List<string> list)
        {
            ButtonDropDown.options.Clear();
            foreach (string option in list)
            {
                ButtonDropDown.options.Add(new Dropdown.OptionData(option));
            }
        }

        public void EnableControllerTut()
        {
            ControllerButton.ShowButtonHint(HandDropDown.options[HandDropDown.value].text, ButtonDropDown.options[ButtonDropDown.value].text);            
        }

        public void DisableControllerTut()
        {
            ControllerButton.HideButtonHint(HandDropDown.options[HandDropDown.value].text, ButtonDropDown.options[ButtonDropDown.value].text);
            
        }

        public void ButtonHintcall()
        {
            ControllerButton.ShowButtonHint(ControllerButton.Hand.Left, ControllerButton.Buttons.Grip, "Click this button to grab the object");
        }

        public void ShowAllHint()
        {
            ControllerButton.ShowAllHint(HandDropDown.options[HandDropDown.value].text);
        }

        public void HidelAllHint()
        {
            ControllerButton.HideAllHint(HandDropDown.options[HandDropDown.value].text);
        }
       
    }    
}
