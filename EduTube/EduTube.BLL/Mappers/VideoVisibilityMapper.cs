using EduTube.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Mappers
{
    public class VideoVisibilityMapper
    {
        public static VideoVisibility ModelToEntity (VideoVisibilityModel model)
        {
            if (model == VideoVisibilityModel.Invitation)
                return VideoVisibility.Invitation;

            else if (model == VideoVisibilityModel.Private)
                return VideoVisibility.Private;

            else 
                return VideoVisibility.Public;
        }
        public static VideoVisibilityModel EntityToModel (VideoVisibility entity)
        {
            if (entity == VideoVisibility.Invitation)
                return VideoVisibilityModel.Invitation;

            else if (entity == VideoVisibility.Private)
                return VideoVisibilityModel.Private;

            else
                return VideoVisibilityModel.Public;
        }
    }
}
