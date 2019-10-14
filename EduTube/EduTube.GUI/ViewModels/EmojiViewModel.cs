using EduTube.BLL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
	public class EmojiViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string OldImage { get; set; }
		public IFormFile Image { get; set; }

		public EmojiViewModel()
		{
		}

		public EmojiViewModel(EmojiModel model)
		{
			Id = model.Id;
			Name = model.Name;
			OldImage = model.ImagePath;
		}

		public static EmojiModel CopyToModel(EmojiViewModel viewModel)
		{
			EmojiModel model = new EmojiModel()
			{
				Id = viewModel.Id,
				Name = viewModel.Name,
				ImagePath = viewModel.OldImage,
				Deleted = false
			};
			return model;
		}
	}
}
