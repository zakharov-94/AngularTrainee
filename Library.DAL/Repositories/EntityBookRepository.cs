﻿using Library.DAL.Context;
using Library.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.DAL.Repositories
{
    public class EntityBookRepository<T> : EntityRepository<T> where T : Book
    {
        private ApplicationContext _context;
        private DbSet<T> _dbSet;

        public EntityBookRepository(ApplicationContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public override IEnumerable<T> GetAll()
        {

            try
            {
                IEnumerable<T> item = (IEnumerable<T>)_context.Books.Include(e => e.PublicationHouseBooks).ThenInclude(e => e.PublicationHouse).ToList();
                return item;
            }
            catch (Exception ex)
            {
                LogRegistrator.Write(ex);
                return null;
            }
        }
        public override T FindById(int id)
        {
            try
            {
                return (T)_context.Books.Include(e => e.PublicationHouseBooks).ThenInclude(e => e.PublicationHouse)
                    .Where(e => e.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogRegistrator.Write(ex);
                return null;
            }
        }
        public override void Add(T item)
        {
            try
            {
                Book entity = new Book
                {
                    Id = item.Id,
                    Name = item.Name,
                    Author = item.Author,
                    YearOfPublishing = item.YearOfPublishing,
                    //PublicationHouses = new List<PublicationHouse>()
                };
                _context.Books.Add(entity);
                _context.SaveChanges();
                //if (item.PublicationHouses.Count != 0)
                //{
                //    List<PublicationHouse> list = _context.PublicationHouses.ToList();
                //    foreach (var iterator in item.PublicationHouses)
                //    {
                //        entity.PublicationHouses.Add(list.Find(x => x.Id == iterator.Id));
                //    }

                //    _context.Entry(entity).State = EntityState.Modified;
                //    _context.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                LogRegistrator.Write(ex);
            }
        }

        public override void Update(T item)
        {
            try
            {
                Book entity = (Book)item;
                Book book = _context.Books.Find(entity.Id);
                foreach (var prop in item.GetType().GetProperties())
                {
                    if (prop.Name != "PublicationHouses")
                    {
                        book.GetType().GetProperty(prop.Name).SetValue(book, prop.GetValue(item));
                    }
                }
                //book.PublicationHouses.Clear();

                //List<PublicationHouse> list = _context.PublicationHouses.ToList();
                //foreach (var iterator in entity.PublicationHouses)
                //{
                //    book.PublicationHouses.Add(list.Find(x => x.Id == iterator.Id));
                //}
                _context.Entry(book).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogRegistrator.Write(ex);
            }
        }
    }
}
