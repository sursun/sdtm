using FluentNHibernate.Automapping.Alterations;
using Gms.Domain.FollowUp;
using Gms.Domain.Health;

namespace Gms.Infrastructure.NHibernateMaps.FollowUp
{
    public class FuZhenMap : IAutoMappingOverride<FuZhen>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<FuZhen> mapping)
        {
            mapping.Table("FollowUp_FuZhens");
        }
    }
}
