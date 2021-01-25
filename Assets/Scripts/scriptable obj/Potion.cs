using UnityEngine;

[CreateAssetMenu(fileName = "new Potion", menuName = "Items/Potion")]

public class Potion : Item
{
    public int healthAmount;


    public override void Equip()
    {

        base.Equip();

        CharacterController2D player = FindObjectOfType<CharacterController2D>();
        if (player)
        {
            Health health = player.GetComponent<Health>();
            if (health)
            {

                if (health.IsAlive() && health.currentHealth < health.maxHealth)
                {
                    health.ChangeHealth(healthAmount);
                    Inventory.Instance.RemoveItem(this);

                }
            }
        }
    }

}
