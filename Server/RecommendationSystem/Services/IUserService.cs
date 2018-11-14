using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSystem
{
    public interface IUserService
    {
        List<User> GetUsers();
        List<Rating> GetRatings(String userID);
    }

   public class User
    {
        public String name;
        public String userID;
        public float similarity;
    }

    public class Rating
    {
        public String userID;
        public String filmName;
        public float score;

        public override String ToString()
        {
            return String.Format($"User ID:{userID}\tFilm Name:{filmName}\tScore:{score}");
        }
    }

}
