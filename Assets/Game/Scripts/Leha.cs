using UnityEngine;
public class Leha : FieldUnit
{
  public float live_time = 0f;
  private float time_passed = 0f;

  void LehaTick()
  {
    time_passed += Time.deltaTime;
    if(time_passed >= live_time)
    {
      Die();
    }
  }

  public override void Die(bool destroy = true)
  {
    RaiseDeath();
    Reset();
    GlobalEventSystem.OnTurnTick -= FieldUpdate;
    if( tile != null)
      tile.unit = null;
    tile = null;

  }
  private void FieldUpdate()
  {
    tile.RunLehaFromHere();
  }
  public override void Initialize()
  {
    base.Initialize();
    time_passed = 0;
    health = max_health;
    OnRealTimeTick += LehaTick;
    GlobalEventSystem.OnTurnTick += FieldUpdate;
    tile.RunLehaFromHere();
  }



}