﻿using EduTube.BLL.Models;
using EduTube.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
    public class VideoCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name= "Youtube embaded url")]
        public string YoutubeUrl { get; set; }
        public IFormFile Video { get; set; }
        public bool AllowComments { get; set; }
        public string IvniteCode { get; set; }
        public int VideoVisibility { get; set; }
        public List<String> Hashtags { get; set; }

        public VideoCreateViewModel(){}

        public static VideoModel ConvertToModel(VideoCreateViewModel viewModel)
        {
            VideoModel model = new VideoModel();
            model.Name = viewModel.Name;
            model.Description = viewModel.Description;
            model.YoutubeUrl = viewModel.YoutubeUrl;
            model.AllowComments = viewModel.AllowComments;
            model.IvniteCode = viewModel.IvniteCode;
            model.VideoVisibility = (VideoVisibilityModel)viewModel.VideoVisibility;
            model.Deleted = false;
            model.Blocked = false;
            model.DateCreatedOn = DateTime.Now;

            return model;
        }
    }
}
