using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
{
   if (SceneData.shouldTeleportBack)
    {
        GameObject returnTarget = GameObject.Find(SceneData.returnTargetObjectName);
        if (returnTarget != null)
        {
            transform.position = returnTarget.transform.position;
            SceneData.shouldTeleportBack = false;
            SceneData.returnTargetObjectName = "TriviaReturnPoint";
        }
    }
}



}
