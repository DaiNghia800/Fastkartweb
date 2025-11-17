using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastkart.Services
{
    public class WishlistService
    {
        private readonly ApplicationDbContext _context;

        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetUserWishlist(int userId)
        {
            return await _context.Wishlist
                .Where(w => w.UserUid == userId)
                .Include(w => w.Product) 
                .Select(w => w.Product)
                .ToListAsync();
        }
        public async Task<bool> ToggleWishlist(int userId, int productId)
        {
            var existingItem = await _context.Wishlist
                .FirstOrDefaultAsync(w => w.UserUid == userId && w.ProductUid == productId);

            if (existingItem != null)
            {
                _context.Wishlist.Remove(existingItem);
                await _context.SaveChangesAsync();
                return false;
            }
            else
            {
                var newItem = new Wishlist
                {
                    UserUid = userId,
                    ProductUid = productId
                };
                _context.Wishlist.Add(newItem);
                await _context.SaveChangesAsync();
                return true; 
            }
        }
        public async Task<int> GetCount(int userId)
        {
            return await _context.Wishlist.CountAsync(w => w.UserUid == userId);
        }
        public async Task<List<int>> GetUserWishlistProductIds(int userId)
        {
            return await _context.Wishlist
                .Where(w => w.UserUid == userId)
                .Select(w => w.ProductUid)
                .ToListAsync();
        }
    }
}