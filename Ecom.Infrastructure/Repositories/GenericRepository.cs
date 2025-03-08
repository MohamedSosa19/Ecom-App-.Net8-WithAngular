﻿using Ecom.Core.interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity=await GetAsyncById(id);
             _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _context.Set<T>().AsNoTracking().ToListAsync();
        

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsyncById(int id)
        {
            var entity=await _context.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<T> GetAsyncById(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            var entity = await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "id") == id);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State=EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
