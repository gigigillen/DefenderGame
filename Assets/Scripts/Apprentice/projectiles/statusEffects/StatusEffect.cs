using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    protected float duration;
    protected float tickRate;
    protected float nextTickTime;
    protected bool isActive;

    protected GameObject activeVfx;

    protected virtual void Update() {

        if (!isActive) return;

        if (Time.time >= nextTickTime) {
            ApplyEffectTick();
            nextTickTime = Time.time + tickRate;
        }

        duration -= Time.deltaTime;
        if (duration <=0) {
            RemoveEffect();
        }
    }

    public virtual void ApplyEffect(float duration) {

        this.duration = duration;
        isActive = true;
        nextTickTime = Time.time + tickRate;
    }

    public void RefreshDuration(float newDuration) {
        duration = newDuration;
        isActive = true;
        nextTickTime = Time.time + tickRate;
    }

    protected virtual void ApplyEffectTick() { }

    protected virtual void RemoveEffect() {

        if (activeVfx != null) {
            Destroy(activeVfx);
        }

        Destroy(this);
        isActive = false;
    }
}
