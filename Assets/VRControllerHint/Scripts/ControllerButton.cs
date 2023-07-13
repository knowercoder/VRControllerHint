using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace krisnart.ControllerTut
{
    public class ControllerButton : MonoBehaviour
    {
        public static ControllerButton instance;
        
        [Tooltip("Change Material color of the button")]
        public bool isColor = true;
        [Tooltip("Highlight the button")]
        public bool isHighlight;
        [Tooltip("Animate the highlight of the button")]
        public bool isHighlightAnimate;
        [Tooltip("Give haptic vibration to the controller")]
        public bool isHaptics;

        LabelLine RightlabelLine;
        LabelLine LeftlabelLine;
        LabelLine labelLine;  
        
        [HideInInspector]
        public string HMDmodel;

        List<InputDevice> inputDevices = new List<UnityEngine.XR.InputDevice>();

        public enum Buttons
        {
            Trigger, ThumbStick, Trackpad, Grip, Menu, SystemMenu, A_X, B_Y
        }
                
        public enum Hand
        {
            Left, Right
        }

        List<string> OculusButtons = new List<string> { "Trigger", "ThumbStick", "Grip", "Menu", "A_X", "B_Y" };
        List<string> ViveButtons = new List<string> { "Trigger", "Trackpad", "Grip", "Menu", "SystemMenu" };
        List<string> MRButtons = new List<string> { "Trigger", "ThumbStick", "Grip", "Menu", "Trackpad", "SystemMenu" };

        public static List<string> ControllerButtons = new List<string>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //GetControllers();
            StartCoroutine(GetControllersCOR());
        }

        IEnumerator GetControllersCOR()
        {
            yield return new WaitForSeconds(1.5f);
            GetControllers();
        }

        void GetControllers()
        {
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, inputDevices);
            var model = inputDevices[0].name;
            Debug.Log("The input device is: " + model);
            if (model.ToLower().Contains("rift"))
            {
                HMDmodel = "rift";
                ControllerButtons = OculusButtons;                
                AssignControllers("rift");
            }
            else if (model == "Miramar" || model.ToLower().Contains("quest"))
            {
                HMDmodel = "quest";
                ControllerButtons = OculusButtons;                
                AssignControllers("quest");                
            }
            else if (model.ToLower().Contains("vive") || model.ToLower().Contains("htc"))
            {
                HMDmodel = "htc";
                ControllerButtons = ViveButtons;                
                AssignControllers("vive");
            }
            else
            {
                HMDmodel = "mr";
                ControllerButtons = MRButtons;                
                AssignControllers("MR");
            }            
        }

        void AssignControllers(string ControllerName)
        {
            var Controllers = FindObjectsOfType<krisnart.ControllerTut.Controllers>();            
            for (int i = 0; i < Controllers.Length; i++)
            {
                for (int j = 0; j < Controllers[i].ControllerList.Length; j++)
                {
                    if (Controllers[i].ControllerList[j].gameObject.name == ControllerName + "_R")
                    {
                        RightlabelLine = Controllers[i].ControllerList[j];
                    }
                    if (Controllers[i].ControllerList[j].gameObject.name == ControllerName + "_L")
                    {
                        LeftlabelLine = Controllers[i].ControllerList[j];
                    }
                } 
            }
        }
        

        public static void ShowButtonHint(string hand, string ButtonName, string label = "Default")
        {
            SetButton(hand, ButtonName, label);
        }
        
        public static void ShowButtonHint(Hand hand, Buttons ButtonName, string label = "Default")
        {
            SetButton(hand.ToString(), ButtonName.ToString(), label);
        }      
       

        static void SetButton(string hand, string ButtonName, string label)
        {
            if (label == "Default")
                label = ButtonName;

            if (hand == "Right")                            
                instance.labelLine = instance.RightlabelLine;                
            else            
                instance.labelLine = instance.LeftlabelLine;                
            
            Debug.Log("controller model assigned .. ");

            instance.labelLine.gameObject.SetActive(true);
            if(instance.isHighlight)
                SetButtonHighlight(ControllerButtons.IndexOf(ButtonName), true, instance.isHighlightAnimate);           
            instance.labelLine.Setlabel(ButtonName, label, true);
            instance.labelLine.labelStates[ControllerButtons.IndexOf(ButtonName)] = true;

            if (instance.isHaptics)
                instance.labelLine.SetHaptic(hand, true);
        }

        public static void HideButtonHint(string hand, string ButtonName, string label = "Default")
        {
            HideButton(hand, ButtonName, label);
        }
        
        public static void HideButtonHint(Hand hand, Buttons ButtonName, string label = "Default")
        {
            HideButton(hand.ToString(), ButtonName.ToString(), label);
        }
       
        static void HideButton(string hand, string ButtonName, string label)
        {
            if (hand == "Right")
                instance.labelLine = instance.RightlabelLine;               
            else
                instance.labelLine = instance.LeftlabelLine;               
                     
            SetButtonHighlight(ControllerButtons.IndexOf(ButtonName), false, instance.isHighlightAnimate);
            instance.labelLine.Setlabel(ButtonName, label, false);
            instance.labelLine.labelStates[ControllerButtons.IndexOf(ButtonName)] = false;

            if (!GetLabelStates()) //disable controller and haptics ony if all label states are false;
            { 
                instance.labelLine.gameObject.SetActive(false);
                instance.labelLine.SetHaptic(hand, false);
            }
        }

        public static void ShowAllHint(string hand)
        {
            instance.ShowAll(hand);
        }
        public static void ShowAllHint(Hand hand)
        {
            instance.ShowAll(hand.ToString());
        }

        void ShowAll(string hand)
        {
            if (hand == "Right")
                labelLine = instance.RightlabelLine;
            else
                labelLine = instance.LeftlabelLine;
            labelLine.gameObject.SetActive(true);
            labelLine.Setlabel(true);
            if (instance.isHaptics)
                labelLine.SetHaptic(hand, true);
        }

        public static void HideAllHint(string hand)
        {
            instance.HideAll(hand);
        }
        public static void HideAllHint(Hand hand)
        {
            instance.HideAll(hand.ToString());
        }

        void HideAll(string hand)
        {
            if (hand == "Right")
                labelLine = instance.RightlabelLine;
            else
                labelLine = instance.LeftlabelLine;            
            labelLine.Setlabel(false);
            labelLine.SetHaptic(hand, false);
            labelLine.gameObject.SetActive(false);
        }

        static bool GetLabelStates()
        {
            for(int i = 0; i < instance.labelLine.labelStates.Count; i++)
            {
                if (instance.labelLine.labelStates[i])
                    return true;
            }

            return false;
        }

        static void SetButtonHighlight(int BtnInt, bool state, bool HighlightAnimate)
        {
            var highlightController = instance.labelLine.transform.GetChild(0).GetChild(BtnInt).GetComponent<HighlightController>();
            highlightController.enabled = state;
            highlightController.HighlightAnimate = HighlightAnimate;
        } 
    }    
}
