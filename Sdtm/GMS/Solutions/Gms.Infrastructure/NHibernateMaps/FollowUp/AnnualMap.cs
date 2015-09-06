using FluentNHibernate.Automapping.Alterations;
using Gms.Domain.FollowUp;
using Gms.Domain.Health;

namespace Gms.Infrastructure.NHibernateMaps.FollowUp
{
    public class AnnualMap : IAutoMappingOverride<Annual>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<Annual> mapping)
        {
            mapping.Table("FollowUp_Annuals");
        }
    }
}
