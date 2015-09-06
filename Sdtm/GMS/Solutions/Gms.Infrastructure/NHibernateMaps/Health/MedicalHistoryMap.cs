using FluentNHibernate.Automapping.Alterations;
using Gms.Domain.Health;

namespace Gms.Infrastructure.NHibernateMaps.Health
{
    public class MedicalHistoryMap : IAutoMappingOverride<MedicalHistory>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<MedicalHistory> mapping)
        {

        }
    }
}
