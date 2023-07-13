using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.Enemies.Abstraction
{
    public interface IDestructable
    {
        bool IsDestructable { get; }
        void TakeDamage(float damage);
    }
}
