using FluentNHibernate.Automapping.Alterations;
using Gms.Domain.Health;

namespace Gms.Infrastructure.NHibernateMaps.Health
{
    public class DiagnosesMap : IAutoMappingOverride<Diagnoses>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<Diagnoses> mapping)
        {
            mapping.HasManyToMany(c => c.Diseases).Table("Health_Diagnoses_Diseases");
        }
    }
}
