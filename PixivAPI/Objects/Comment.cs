using System;
using System.Collections.Generic;
using System.Text;

namespace Pixeez.Objects
{
    public class Comment
    {
        public int id { get; set; }
        public string comment { get; set; }
        public DateTime date { get; set; }
        public class User
        {
            public int id { get; set; }
            public string name { get; set; }
            public string account { get; set; }
            public class ProfileImageUrls
            {
                public string medium { get; set; }
            }
            public ProfileImageUrls profile_image_urls { get; set; }
        }
        public User user { get; set; }
        public Comment parent_comment { get; set; }
    }

    public class IllustCommentObject
    {
        public int total_comments { get; set; }
        public Comment[] comments { get; set; }
        public string next_url { get; set; }
    }
}
