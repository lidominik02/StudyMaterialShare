using StudyMaterialShare.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Database.Repositories
{
    public class SubjectRepository : RepositoryBase<Subject>
    {
        public SubjectRepository(StudyMaterialShareDbContext context) : base(context) { }

        public override Subject? Create(Subject entity)
        {
            try
            {
                _context.Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override Subject? Delete(int id)
        {
            var entity = _context.Subjects.FirstOrDefault(x => x.Id == id);

            if(entity == null)
                return null;

            return Delete(entity);
        }

        public override Subject? Delete(Subject entity)
        {
            try
            {
                _context.Remove(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override IEnumerable<Subject> Get(Expression<Func<Subject, bool>>? filter = null, Func<IQueryable<Subject>, IOrderedQueryable<Subject>>? orderBy = null)
        {
            IQueryable<Subject> subjects = _context.Subjects;

            if(filter != null)
            {
                subjects = subjects.Where(filter);
            }

            if(orderBy != null)
            {
                return orderBy(subjects).AsEnumerable();
            }
            else
            {
                return subjects.AsEnumerable();
            }
        }

        public override Subject? Get(int id)
        {
            return Get(s => s.Id == id);
        }

        public override Subject? Get(Expression<Func<Subject, bool>> predicate)
        {
            return _context.Subjects.FirstOrDefault(predicate);
        }

        public override Subject? Update(Subject entity)
        {
            try
            {
                _context.Update(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<StudyMaterial> GetStudyMaterialsForSubject(Subject subject)
        {
            return _context.StudyMaterials.Where(sm => sm.Subject.Id == subject.Id).AsEnumerable();
        }

        public IEnumerable<StudyMaterial> GetStudyMaterialsForSubject(int id)
        {
            return GetStudyMaterialsForSubject(new Subject() { Id = id });
        }
    }
}
