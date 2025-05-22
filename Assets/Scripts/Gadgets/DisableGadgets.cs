using UnityEngine;

public class DisableGadgets : MonoBehaviour
{
    public GameObject throwingGadget;

    public GameObject toolGadget;

    public GameObject supportGadget;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        if (!GameConnect.isDaterRegim && !GameConnect.GadgetModes(GameConnect.gameMode))
        {
            throwingGadget.GetComponent<GadgetTouchButton>().enabled = false;
            throwingGadget.SetActive(false);
            toolGadget.GetComponent<GadgetTouchButton>().enabled = false;
            toolGadget.SetActive(false);
            supportGadget.GetComponent<GadgetTouchButton>().enabled = false;
            supportGadget.SetActive(false);
            
        }
        Gadget value = null;
        if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetInfo.GadgetCategory.Throwing, out value))
        {
            if (value == null)
            {
                throwingGadget.SetActive(false);
                throwingGadget.GetComponent<GadgetTouchButton>().enabled = false;
            }
        }
        if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetInfo.GadgetCategory.Tools, out value))
        {
            if (value == null)
            {
                toolGadget.SetActive(false);
                toolGadget.GetComponent<GadgetTouchButton>().enabled = false;
            }
        }
        if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetInfo.GadgetCategory.Support, out value))
        {
            if (value == null)
            {
                supportGadget.SetActive(false);
                supportGadget.GetComponent<GadgetTouchButton>().enabled = false;
            }
        }
        if (GameConnect.isDaterRegim)
        {
            toolGadget.GetComponent<GadgetTouchButton>().enabled = false;
            toolGadget.SetActive(false);
            supportGadget.GetComponent<GadgetTouchButton>().enabled = false;
            supportGadget.SetActive(false);
        }
    }
}
