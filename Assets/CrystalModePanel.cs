using UnityEngine;

public class CrystalModePanel:Panel
{
    public virtual void Init()
    {
        DeactivateOnInit();
    }

    public virtual void DeactivateOnInit()
    {

        //  ChangeCrystalInfoRectTransformScale(0);

        //HandleCrystalInfoPanel(false, true);


    }
    public virtual void HandleCrystalInfoPanel(GameObject[] objs, bool activate, bool isFirstInitilized = false,bool isSmooth=false)
    {
        if (activate || (!activate && isFirstInitilized))
        {
            foreach (var obj in objs)
            {
                obj.SetActive(activate);

            }
            //CrystalInfoTexts.SetActive(activate);
            //CrystalStartInfoBG.SetActive(activate);

        }

        else if (!activate && !isFirstInitilized)
        {
            if (isSmooth)
            {
            CloseSmoothly();

            }
            else
            {

                foreach (var obj in objs)
                {
                    DeActivate(obj);

                }

            }
        }



    }
}