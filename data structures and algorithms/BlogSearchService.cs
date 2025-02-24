﻿using Microsoft.EntityFrameworkCore;
using RochaBlogs.Data;
using RochaBlogs.Models;

namespace RochaBlogs.Services
{
    public class BlogSearchService
    {

        private readonly ApplicationDbContext _context;

        public BlogSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Search(string searchTerm)
        {
            var posts = _context.Posts.Include(p => p.BlogUser).Where(p => p.ReadyStatus == Enums.ReadyStatus.ProductionReady).AsQueryable();

            if (searchTerm is not null)
            {
                searchTerm = searchTerm.ToLower();
                posts = posts.Where(
                    p => p.Title.ToLower().Contains(searchTerm) ||
                    p.Abstract.ToLower().Contains(searchTerm) ||
                    p.Content.ToLower().Contains(searchTerm) ||
                    p.Comments.Any(
                        c => c.Body.ToLower().Contains(searchTerm) ||
                        c.ModeratedBody.ToLower().Contains(searchTerm) ||
                        c.BlogUser.FirstName.ToLower().Contains(searchTerm) ||
                        c.BlogUser.LastName.ToLower().Contains(searchTerm) ||
                        c.BlogUser.Email.ToLower().Contains(searchTerm)));
            }

            return posts.OrderByDescending(p => p.Created);
        }
    }
}
