using QPhysics.Dynamics;
using QPhysics.Extensions.PhysicsLogics.PhysicsLogicBase;

namespace QPhysics.Extensions.Controllers.ControllerBase
{
    public abstract class Controller : FilterData
    {
        private ControllerType _type;
        public bool Enabled;
        public World World;

        public Controller(ControllerType controllerType)
        {
            _type = controllerType;
        }

        public override bool IsActiveOn(Body body)
        {
            if (body.ControllerFilter.IsControllerIgnored(_type))
                return false;

            return base.IsActiveOn(body);
        }

        public abstract void Update(float dt);
    }
}