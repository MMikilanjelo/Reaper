
using Game.Components;

public interface IVisitor
{
	void Visit(HealthComponent _healthComponent);
	void Visit(HurtBoxComponent _hurtBoxComponent);
	void Visit(VelocityComponent _velocityComponent);
	void Visit (WeaponRootComponent _weaponRootComponent);
	void Visit(StatusRecivierComponent _statusRecivierComponent);	
}
