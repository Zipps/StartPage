using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using StartPage.Framework;
using StartPage.Models;

namespace StartPage.Services
{
    public interface IBookmarkService
    {
        Task<Bookmark> Create(Bookmark bookmark);
        Task<Bookmark> Get(Guid id);
        Task<IEnumerable<Bookmark>> GetAll();
        Task Update(Bookmark bookmark);
        Task Delete(Guid id);
    }

    public class BookmarkService : IBookmarkService
    {
        private readonly StartPageContext _context;

        public BookmarkService(StartPageContext context)
        {
            _context = context;
        }

        public async Task<Bookmark> Create(Bookmark bookmark)
        {
            if (bookmark.BookmarkId == Guid.Empty)
            {
                bookmark.BookmarkId = Guid.NewGuid();
            }
            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();

            return bookmark;
        }

        public async Task Update(Bookmark bookmark)
        {
            _context.Bookmarks.Update(bookmark);
            await _context.SaveChangesAsync();
        }

        public async Task<Bookmark> Get(Guid id)
        {
            var existingBookmark = await _context.Bookmarks
                        .SingleOrDefaultAsync(x => x.BookmarkId == id);
            if (existingBookmark == null) return null;

            return existingBookmark;
        }

        public async Task<IEnumerable<Bookmark>> GetAll()
        {
            var allBookmarks = new List<Bookmark>();
            await _context.Bookmarks.ForEachAsync(x => allBookmarks.Add(x));
            return allBookmarks;
        }

        public async Task Delete(Guid id)
        {
            var existingBookmark = await Get(id);
            if (existingBookmark == null) return;

            _context.Bookmarks.Remove(existingBookmark);
            await _context.SaveChangesAsync();
        }
    }
}