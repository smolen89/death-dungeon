// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

//public class NamespaceHolder : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}

namespace RDE { }
namespace RDE.Engine { }
namespace RDE.Engine.Systems { }
namespace RDE.Engine.Utils { }

namespace RDE.Engine.ECS.Entities { }
namespace RDE.Engine.ECS.Components { }
namespace RDE.Engine.ECS.Systems { }
namespace RDE.Engine.ECS.Pathfinding { }
namespace RDE.Engine.Jobs { }

namespace RDE.Utils { }

[System.Serializable]
public struct MoveComponent : IComponentData
{
	public Direction direction;
}


