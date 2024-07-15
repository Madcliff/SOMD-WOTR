using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using SOMD.NewUnitParts;

namespace SOMD.NewComponents
{
    [TypeId("44440427d6194decb20524bc0d284a1a")]
    public class SecretsofMagicalDisciplineClericComponent : UnitFactComponentDelegate
    {
        public override void OnTurnOn()
        {
            var SecretsofMagicalDiscipline = Owner.Ensure<UnitPartSecretsofMagicalDiscipline>();
            if (m_Resource != null)
            {
                SecretsofMagicalDiscipline.SetSecretsofMagicalDisciplineResource(m_Resource);
            }
            SecretsofMagicalDiscipline.AddSecretsofMagicalDisciplineSpellbook(m_Spellbook, this.Fact);
            m_SpellLists.ForEach(list => {
                SecretsofMagicalDiscipline.AddSecretsofMagicalDisciplineSpellList(list, this.Fact);
            });
        }
        public override void OnTurnOff()
        {
            var SecretsofMagicalDiscipline = Owner.Get<UnitPartSecretsofMagicalDiscipline>();
            if (SecretsofMagicalDiscipline != null)
            {
                SecretsofMagicalDiscipline.RemoveEntry(this.Fact);
            }
        }

        public BlueprintSpellbookReference m_Spellbook;
        public BlueprintSpellListReference[] m_SpellLists;
        public BlueprintAbilityResourceReference m_Resource;
    }
}
