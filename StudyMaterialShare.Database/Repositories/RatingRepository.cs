using StudyMaterialShare.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Database.Repositories
{
    public class RatingRepository : RepositoryBase<Rating>
    {
        public RatingRepository(StudyMaterialShareDbContext context) : base(context) { }

        public Rating? Create(StudyMaterial studyMaterial, ApplicationUser user, int rateValue)
        {
            try
            {
                if (user == null) return null;
                if (studyMaterial == null) return null;
                if (rateValue < 1 || rateValue > 5) return null;

                var rating = _context.Ratings
                .Where(rating =>
                    rating.User.Id == user.Id && rating.StudyMaterial.Id == studyMaterial.Id)
                .FirstOrDefault();

                if (rating == null)
                {
                    rating = new Rating()
                    {
                        RateValue = rateValue,
                        StudyMaterial = studyMaterial,
                        User = user
                    };
                    _context.Ratings.Add(rating);
                }
                else
                {
                    rating.RateValue = rateValue;
                }

                _context.SaveChanges();
                return rating;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override Rating? Create(Rating rating)
        {
            return Create(rating.StudyMaterial,rating.User,rating.RateValue);
        }

        public override Rating? Delete(int id)
        {
            var rating = new Rating()
            {
                Id = id
            };

            return Delete(rating);
        }

        public override Rating? Delete(Rating rating)
        {
            try
            {
                _context.Remove(rating);
                _context.SaveChanges();

                return rating;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override IEnumerable<Rating> Get(Expression<Func<Rating, bool>>? filter = null, Func<IQueryable<Rating>, IOrderedQueryable<Rating>>? orderBy = null)
        {
            IQueryable<Rating> ratings = _context.Ratings;

            if (filter != null)
            {
                ratings = ratings.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(ratings).AsEnumerable();
            }
            else
            {
                return ratings.AsEnumerable();
            }
        }

        public override Rating? Get(int id)
        {
            return Get(s => s.Id == id);
        }

        public override Rating? Get(Expression<Func<Rating, bool>> predicate)
        {
            return _context.Ratings.FirstOrDefault(predicate);
        }

        public override Rating? Update(Rating rating)
        {
            try
            {
                _context.Update(rating);
                _context.SaveChanges();

                return rating;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
