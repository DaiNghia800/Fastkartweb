using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Fastkart.Services
{
    public class PageService : IPageService
    {
        private readonly ApplicationDbContext _context;

        public PageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Pages> GetAllPages()
        {
            try
            {
                return _context.Pages
                               .Include(p => p.Author)
                               .Where(p => p.Deleted == false)
                               .AsNoTracking()
                               .ToList();
            }
            catch (Exception ex)
            {
                return new List<Pages>();
            }
        }

        public Pages GetPageById(int uid)
        {
            try
            {
                return _context.Pages
                               .Include(p => p.Author)
                               .FirstOrDefault(p => p.Uid == uid && p.Deleted == false);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void CreatePage(Pages page)
        {
            _context.Pages.Add(page);
            _context.SaveChanges();
        }

        public void UpdatePage(Pages page)
        {
            _context.Entry(page).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeletePage(int uid)
        {
            var page = _context.Pages.Find(uid);
            if (page != null)
            {
                page.Deleted = true;
                _context.SaveChanges();
            }
        }
    }
}