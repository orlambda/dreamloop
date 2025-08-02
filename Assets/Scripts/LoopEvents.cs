using UnityEngine;
namespace Actions
{
    public class OrchestratedActions : MonoBehaviour
    {
        public void SpawnEnemies(object[] args)
        {
            Debug.Log("Spawn in enemy");
        }

        public void MovePlatforms(object[] args)
        {
            Debug.Log("Platforms are moving");
        }

        public void BeamLasers(object[] args)
        {
            Debug.Log("Lasers beaming");
        }

    }


}

