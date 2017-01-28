using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivUWP.Data
{
    class WorkEqualityComparer : IEqualityComparer<Work>
    {
        public bool Equals(Work x, Work y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Work obj)
        {
            return obj.Id.GetHashCode();
        }
        public static WorkEqualityComparer Default { get; } = new WorkEqualityComparer();
    }
    class UsersFavoriteWorkEqualityComparer : IEqualityComparer<UsersFavoriteWork>
    {
        public bool Equals(UsersFavoriteWork x, UsersFavoriteWork y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(UsersFavoriteWork obj)
        {
            return obj.Id.GetHashCode();
        }
        public static UsersFavoriteWorkEqualityComparer Default { get; } = new UsersFavoriteWorkEqualityComparer();
    }
}
