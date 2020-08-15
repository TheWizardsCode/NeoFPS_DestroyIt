#if NEOFPS
using DestroyIt;
using NeoFPS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.NeoFPS.DestroyIt
{
    public class NeoFpsDestroyIt_ExplosiveObject : ExplosiveObject
    {
        Destructible m_Destructible;

        private void Start()
        {
            m_Destructible = gameObject.GetComponent<Destructible>();
            if (m_Destructible != null)
                m_Destructible.DestroyedEvent += OnDestroyed;

            if (m_Destructible.totalHitPoints != damageTaken)
            {
                Debug.LogWarning(gameObject.name + ": the damage set in the NeoFpsDestroyIt_ExplosiveObject component is different from that set in the Destructible component. This can cause unexpected results. Forcing the value from `ExplodingObject` to be the one true setting.");
                m_Destructible.totalHitPoints = damageTaken;
                m_Destructible.currentHitPoints = damageTaken;
            }
        }

        public override DamageResult AddDamage(float damage, IDamageSource source)
        {
            if (source != null && !inDamageFilter.CollidesWith(source.outDamageFilter, FpsGameMode.friendlyFire))
                return DamageResult.Ignored;

            if (base.AddDamage(damage, source) == DamageResult.Ignored) return DamageResult.Ignored; ;

            m_Destructible.ApplyDamage(damage);
            return DamageResult.Standard;
        }

        private void OnDestroyed()
        {
            OnKilled(null);
        }
    }
}
#endif