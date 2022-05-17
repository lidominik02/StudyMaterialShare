using StudyMaterialShare.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Database.Repositories
{
    public class StudyMaterialRepository : RepositoryBase<StudyMaterial>
    {
        public StudyMaterialRepository(StudyMaterialShareDbContext context) : base(context) { }

        public override StudyMaterial? Create(StudyMaterial studyMaterial)
        {
            try
            {
                _context.Add(studyMaterial);
                _context.SaveChanges();
                return studyMaterial;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override StudyMaterial? Delete(int id)
        {
            var studyMaterial = new StudyMaterial()
            {
                Id = id
            };

            return Delete(studyMaterial);
        }

        public override StudyMaterial? Delete(StudyMaterial studyMaterial)
        {
            try
            {
                _context.Remove(studyMaterial);
                _context.SaveChanges();

                return studyMaterial;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override IEnumerable<StudyMaterial> Get(Expression<Func<StudyMaterial, bool>>? filter = null, Func<IQueryable<StudyMaterial>, IOrderedQueryable<StudyMaterial>>? orderBy = null)
        {
            IQueryable<StudyMaterial> studyMaterials = _context.StudyMaterials;

            if (filter != null)
            {
                studyMaterials = studyMaterials.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(studyMaterials).AsEnumerable();
            }
            else
            {
                return studyMaterials.AsEnumerable();
            }
        }

        public override StudyMaterial? Get(int id)
        {
            return Get(s => s.Id == id);
        }

        public override StudyMaterial? Get(Expression<Func<StudyMaterial, bool>> predicate)
        {
            return _context.StudyMaterials.FirstOrDefault(predicate);
        }

        public override StudyMaterial? Update(StudyMaterial studyMaterial)
        {
            try
            {
                _context.Update(studyMaterial);
                _context.SaveChanges();

                return studyMaterial;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeleteStudyMaterialRatings(StudyMaterial studyMaterial)
        {
            try
            {
                _context.RemoveRange(studyMaterial.Ratings);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteStudyMaterialRatings(int id)
        {
            var studyMaterial = new StudyMaterial()
            {
                Id = id,
            };

            return DeleteStudyMaterialRatings(studyMaterial);
        }
    }
}
