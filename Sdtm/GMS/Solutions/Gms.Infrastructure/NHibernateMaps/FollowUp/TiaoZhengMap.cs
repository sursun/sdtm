using FluentNHibernate.Automapping.Alterations;
using Gms.Domain.FollowUp;

namespace Gms.Infrastructure.NHibernateMaps.FollowUp
{
    public class TiaoZhengMap : IAutoMappingOverride<TiaoZheng>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<TiaoZheng> mapping)
        {
            mapping.Table("FollowUp_TiaoZhengs");
        }
    }
}
