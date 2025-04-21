using UnityEngine;

public interface IDamageable 
    //this is not a class it's an interface, it's a contract that any class that implements it must follow
    // this means that any class that implements this interface must have a method called ReceiveDamage
{
    void ReceiveDamage(int damageTaken, GameObject source);
}
