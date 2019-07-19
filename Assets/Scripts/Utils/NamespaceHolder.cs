// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi

using System.Collections;
using System.Collections.Generic;
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

namespace DRL { }
namespace DRL.Engine { }
namespace DRL.Engine.Systems { }
namespace DRL.Engine.Utils { }



namespace DRL.Engine.ECS.Entities { }
namespace DRL.Engine.ECS.Components { }
namespace DRL.Engine.ECS.Systems { }
namespace DRL.Engine.ECS.Pathfinding { }
namespace DRL.Engine.Jobs { }

namespace DRL.Utils { }

[System.Serializable]
public struct MoveSpeed : IComponentData
{
	public void OnUpdate() => throw new System.NotImplementedException();
}

public interface IComponentData
{
	void OnUpdate();
}