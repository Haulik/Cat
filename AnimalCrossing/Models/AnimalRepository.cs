using System;
using System.Collections.Generic;
using System.Linq;
using AnimalCrossing.Data;

namespace AnimalCrossing.Models
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly AnimalCrossingContext _context;
        public AnimalRepository(AnimalCrossingContext _context)
        {
            this._context = _context;
        }

        public List<Cat> Mates(Cat c) 
        {
            using (_context)
            {
                var cats = from m in _context.Cats
                           select m;

                cats = cats.Where(cat => cat.Gender != c.Gender);

                return cats.ToList();
            }
        }

        public void Delete(int catId)
        {
            _context.Cats.Remove(this.Get(catId));
        }

        public List<Cat> Find(string search1)
        {
            var cats = from m in _context.Cats
                       select m;
            
            if (!String.IsNullOrEmpty(search1))
            {
                string search = search1.ToLower();
                cats = cats.Where(cat => cat.Name.ToLower().Contains(search) || cat.Description.ToLower().Contains(search)) ;
            }

            return cats.ToList();

        }

        public List<Cat> Get()
        {
            return _context.Cats.ToList();
        }

        public Cat Get(int catId)
        {
            return _context.Cats.Find(catId);
        }

        public void Save(Cat c)
        {
            if (c.CatId == 0)
            {
                _context.Cats.Add(c);
            }
            else
            {
                _context.Cats.Update(c);
            }

            _context.SaveChanges();
        }
    }
}
