namespace CheeseMods.CheesesDroneBase
{
    public class AnyUnitFilter : IUnitFilter
    {
        public bool PassesFilter(UnitSpawner uSpawner)
        {
            return true;
        }
    }
}
