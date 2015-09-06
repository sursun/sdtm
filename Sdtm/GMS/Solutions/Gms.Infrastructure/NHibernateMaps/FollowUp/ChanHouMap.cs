using FluentNHibernate.Automapping.Alterations;
using Gms.Domain.FollowUp;
using Gms.Domain.Health;

namespace Gms.Infrastructure.NHibernateMaps.FollowUp
{
    public class ChanHouMap : IAutoMappingOverride<ChanHou>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<ChanHou> mapping)
        {
            mapping.Table("FollowUp_ChanHous");
        }
    }
}
