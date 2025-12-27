using UnityEngine;
using UnityEditor;

public class ObstacleAdder : MonoBehaviour
{
    [MenuItem("Tools/Add Obstacle to Chunk 5")]
    public static void AddObstacle()
    {
        // 1. Find the Chunk 5 Object
        GameObject chunk5 = GameObject.Find("Chunk 5");
        if (chunk5 == null)
        {
            Debug.LogError("Could not find 'Chunk 5' in the scene. Make sure the scene is open.");
            return;
        }

        // 2. Find or Create the "Obstacles" container
        Transform obstaclesParent = chunk5.transform.Find("Obstacles");
        if (obstaclesParent == null)
        {
            GameObject obsObj = new GameObject("Obstacles");
            obstaclesParent = obsObj.transform;
            obstaclesParent.SetParent(chunk5.transform, false);
        }

        // 3. Create the New Obstacle (SpinningPatroller)
        GameObject newObstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newObstacle.name = "SpinningPatroller";
        newObstacle.transform.SetParent(obstaclesParent, false);
        
        // Position it nicely
        newObstacle.transform.localPosition = new Vector3(0, 1, 0); // Center, slightly up
        newObstacle.transform.localScale = new Vector3(2, 1, 1);    // Make it look like a block

        // 4. Add the Script
        // Note: This requires the SpinningPatroller script to exist in your project first.
        // We use Undo.AddComponent to ensure you can CTRL+Z this action.
        var patrolScript = Undo.AddComponent(newObstacle, typeof(SpinningPatroller)) as SpinningPatroller;
        
        if (patrolScript != null)
        {
            patrolScript.leftBound = -10f;
            patrolScript.rightBound = 10f;
            patrolScript.moveSpeed = 6f;
        }

        // 5. Assign a Material (Optional - tries to find one of your uploaded mats)
        string[] matGuids = AssetDatabase.FindAssets("Red t:Material"); // Looking for "Red"
        if (matGuids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(matGuids[0]);
            Material redMat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (redMat != null)
            {
                newObstacle.GetComponent<Renderer>().material = redMat;
            }
        }

        // Register the creation for Undo so you can revert it easily
        Undo.RegisterCreatedObjectUndo(newObstacle, "Create Spinning Patroller");
        Selection.activeGameObject = newObstacle;
        
        Debug.Log("Successfully added SpinningPatroller to Chunk 5!");
    }
}