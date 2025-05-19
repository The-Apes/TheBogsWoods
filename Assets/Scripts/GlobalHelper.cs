using UnityEngine;

public static class GlobalHelper
{
    public static string GenerateUniqueID(GameObject obj)
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}"; //Chest_3_4
    }
}
//Ignore this file, it is just a helper class to generate unique IDs for objects in the scene I was using for testing purposes
// and will be removed in the final version of the game.