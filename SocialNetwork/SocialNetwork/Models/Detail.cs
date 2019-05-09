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

    public class PollDetail : Detail
    {
        public class pollOption
        {
            private string option;
            private long votes;

            public pollOption(string opt)
            {
                option = opt;
                votes = 0;
            }

            public void vote()
            {
                votes++;
            }

            public void unVote()
            {
                votes--;
            }
        }

        public List<pollOption> Poll { get; set; }
    }
}
