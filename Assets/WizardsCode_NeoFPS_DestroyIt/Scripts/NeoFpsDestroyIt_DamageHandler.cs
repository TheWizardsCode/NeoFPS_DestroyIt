#if NEOFPS
using DestroyIt;
using NeoFPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.NeoFPS.DestroyIt
{
    public class NeoFpsDestroyIt_DamageHandler : MonoBehaviour, IDamageHandler
    {
        [SerializeField, Tooltip("The value to multiply any incoming damage by. Use to reduce damage to areas like feet, or raise it for areas like the head.")]
        private float m_Multiplier = 1f;

        [SerializeField, Tooltip("Does the damage count as critical. Used to change the feedback for the damage taker and dealer.")]
        private bool m_Critical = false;

        Destructible m_Destructible;

        public DamageFilter inDamageFilter
        {
            get { return DamageFilter.AllDamageAllTeams; ; }
            set { }
        }

        private void Awake()
        {
            m_Destructible = GetComponentInParent<Destructible>();
            Debug.Assert(m_Destructible != null, gameObject.name + " has a NeoFPS DestroyIt Damage Handler but it is not a destructible. Disabling the damage handler.");
            if (m_Destructible == null) DestroyImmediate(this);
        }

        public DamageResult AddDamage(float damage, RaycastHit hit)
        {
            return AddDamage(damage);
        }

        public DamageResult AddDamage(float damage, RaycastHit hit, IDamageSource source)
        {
            return AddDamage(damage, source);
        }

        public DamageResult AddDamage(float damage, IDamageSource source)
        {
            return AddDamage(damage);
        }

        public DamageResult AddDamage(float damage)
        {
            if (m_Multiplier > 0f)
            {
                int scaledDamage = Mathf.CeilToInt(damage * m_Multiplier);

                m_Destructible.ApplyDamage(scaledDamage);

                return m_Critical ? DamageResult.Critical : DamageResult.Standard;
            }
            else
            {
                return DamageResult.Ignored;
            }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (m_Multiplier < 0f)
                m_Multiplier = 0f;
        }
#endif
    }
}
#endif