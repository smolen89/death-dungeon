using RD.GameEngine.ECS;
using UnityEngine;

namespace RD
{
    public class TestClass : MonoBehaviour
    {
        public World entityWorld;
        
        
        
        public void Start()
        {
            entityWorld = new World(  );
            entityWorld.InitializeAll( true );
            
        }

        public void Update()
        {
            entityWorld.Update();
        }

        public void OnDestroy()
        {            
            entityWorld.UnloadContent();
        }
    }
}