using FluentNHibernate.Automapping.Alterations;
using Gms.Domain.Health;

namespace Gms.Infrastructure.NHibernateMaps.Health
{
    public class TreatmentMap : IAutoMappingOverride<Treatment>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<Treatment> mapping)
        {
            mapping.HasMany(c => c.Medicates);
        }
    }
}
