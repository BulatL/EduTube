using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Enums
{
   public enum LoginResult
   {
      Success,
      UserBlocked,
      UserNotFound,
      WrongCredentials,
      ClaimsFailed
   }
}
