using UnityEngine;

[CreateAssetMenu(fileName = "new Timer Add-On", menuName = "Items/Timer_Add-On")]

public class TimerAddon : Item
{
    public int time;


    public override void Equip()
    {

        base.Equip();
        TimerController levelTimer = FindObjectOfType<TimerController>();
        if(levelTimer){
            
            if(!levelTimer.IsTimeOut()){
                levelTimer.AddTime(time);
                Inventory.Instance.RemoveItem(this);

            }
        } 
    }
    
}
