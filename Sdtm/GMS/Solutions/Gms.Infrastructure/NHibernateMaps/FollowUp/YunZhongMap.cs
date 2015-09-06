using FluentNHibernate.Automapping.Alterations;
using Gms.Domain.FollowUp;
using Gms.Domain.Health;

namespace Gms.Infrastructure.NHibernateMaps.FollowUp
{
    public class YunZhongMap : IAutoMappingOverride<YunZhong>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<YunZhong> mapping)
        {
            mapping.Table("FollowUp_YunZhongs");
        }
    }
}
