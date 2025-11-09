using Fastkart.Models.Entities;
using System.Collections.Generic;

namespace Fastkart.Services
{
    public interface IPageService
    {
        List<Pages> GetAllPages();

        Pages GetPageById(int uid);
        void CreatePage(Pages page);
        void UpdatePage(Pages page);
        void DeletePage(int uid);
    }
}