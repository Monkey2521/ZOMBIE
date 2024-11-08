using ZombieSurvival.General;

namespace ZombieSurvival.Spawners
{
    public abstract class Factory<TObject> : ZSMonoBehaviour where TObject : ZSMonoBehaviour
    {


        protected abstract void Create(TObject instance);
    }
}