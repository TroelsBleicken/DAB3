using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Models
{
    public abstract class Detail
    {}

    public class VideoDetail : Detail
    {
        public string Video { get; set; }
    }

    public class PictureDetail : Detail
    {
        public string Picture { get; set; }
    }
}
