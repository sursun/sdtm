using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gms.Domain;

namespace Gms.Infrastructure
{
    public class MedicineRepository : RepositoryBase<Medicine>, IMedicineRepository
    {
        protected override IQueryable<Medicine> LoadQuery<TQ>(TQ query)
        {
            IQueryable<Medicine> q = base.LoadQuery(query);
            var medicineQuery = query as MedicineQuery;
            if (medicineQuery == null) return q;

            if (medicineQuery.MedicineTypeId.HasValue)
            {
                q = q.Where(c => c.MedicineType.Id == medicineQuery.MedicineTypeId);
            }

            if (!string.IsNullOrEmpty(medicineQuery.NormalName))
            {
                q = q.Where(c => c.NormalName.Contains(medicineQuery.NormalName));
            }

            if (!string.IsNullOrEmpty(medicineQuery.ChemicalName))
            {
                q = q.Where(c => c.ChemicalName.Contains(medicineQuery.ChemicalName));
            }

            if (!string.IsNullOrEmpty(medicineQuery.PinYin))
            {
                q = q.Where(c => c.PinYin.Contains(medicineQuery.PinYin));
            }

            if (medicineQuery.Recommend.HasValue)
            {
                q = q.Where(c => c.Recommend == medicineQuery.Recommend);
            }

            if (!string.IsNullOrEmpty(medicineQuery.Note))
            {
                q = q.Where(c => c.Note.Contains(medicineQuery.Note));
            }

            if (medicineQuery.Enabled.HasValue)
            {
                q = q.Where(c => c.Enabled == medicineQuery.Enabled);
            }

            return q;
        }
        
        public Medicine GetBy(string chemicalName)
        {
            return Query.FirstOrDefault(c => c.ChemicalName.Equals(chemicalName.Trim()));
        }
    }
}
