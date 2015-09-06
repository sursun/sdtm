using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain;

namespace Gms.Infrastructure
{
    public class DiseaseRepository : RepositoryBase<Disease>, IDiseaseRepository
    {
        protected override IQueryable<Disease> LoadQuery<TQ>(TQ query)
        {
            IQueryable<Disease> q = base.LoadQuery(query);
            var diseaseQuery = query as DiseaseQuery;
            if (diseaseQuery == null) return q;

            if (diseaseQuery.TypeId.HasValue)
            {
                q = q.Where(c => c.Type.Id == diseaseQuery.TypeId);
            }

            if (!string.IsNullOrEmpty(diseaseQuery.Name))
            {
                q = q.Where(c => c.Name.Contains(diseaseQuery.Name));
            }

            if (!string.IsNullOrEmpty(diseaseQuery.CodeNo))
            {
                q = q.Where(c => c.CodeNo.Contains(diseaseQuery.CodeNo));
            }

            if (!string.IsNullOrEmpty(diseaseQuery.PinYin))
            {
                q = q.Where(c => c.PinYin.Contains(diseaseQuery.PinYin));
            }

            return q;
        }
    }
}
