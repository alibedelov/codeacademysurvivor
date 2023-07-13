using Code.Scripts.Collectables.Abstraction;

namespace Code.Scripts.Collectables.CollectablesMono
{
    public class Xp : CollectableBaseMono
    {
        private string _xpName;

        private float _xpValue;

        public float XpValue => _xpValue;
        public string XpName => _xpName;


        public void Initialize(float xpValue, string xpName)
        {
            _xpName = xpName;
            _xpValue = xpValue;
        }
    }
}