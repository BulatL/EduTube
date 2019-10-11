﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface ITagManager
   {
      Task<List<TagModel>> GetAll();
      Task<List<TagModel>> GetByNames(string names);
      Task<TagModel> GetById(int id);
      Task<List<int?>> Get2MostPopularTagsIdByVideoId(List<int> videosId);
   }
}
