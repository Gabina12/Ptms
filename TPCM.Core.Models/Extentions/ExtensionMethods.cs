using System;
using System.Collections.Generic;
using System.Linq;
using TPCM.Core.Models;

namespace TPCM.Core.Extentions {
    public static class ExtensionMethods {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users) {
            return users.Select(x => x.WithoutPassword());
        }

        public static User WithoutPassword(this User user) {
            user.Password = null;
            return user;
        }

        public static long AsDate(this DateTime date) {
            return (long)date
               .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
               .TotalMilliseconds;
        }
    }
}
