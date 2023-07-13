using UnityEngine;

namespace Code.Scripts.Managers
{
    //Use to Call Coroutine from non-Mono classes (Turan yapiwtir gitsin)
    public class CoroutineStarter : SingletoneBase<CoroutineStarter>
    {
        private void Start()
        {
            Application.targetFrameRate = -1;
        }
    }
}
