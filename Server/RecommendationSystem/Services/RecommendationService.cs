using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSystem
{
    public class RecommendationService : IRecommendationService
    {
        private List<User> users;
        private List<Rating> ratings;

        private List<Rating> weightedRatings = new List<Rating>();
        private List<WeightedSum> weightedSums = new List<WeightedSum>();

        public RecommendationService()
        {
            users = new List<User>();
            ratings = new List<Rating>();
        }

        public ICollection<WeightedSum> GetRecommendations(string userID, IUserService userService, SimilarityScore similarityScoreType)
        {
            this.users = userService.GetUsers();
            this.ratings = userService.GetRatings(null);

            User userA = users.Find(
                   delegate (User r)
                   {
                       return r.userID == userID;
                   }
                  );

            //Which movies has userA already seen?
            List<Rating> userARatings = ratings.FindAll(
                delegate (Rating r)
                {
                    return r.userID == userID;
                }
                );

        

            //Must remove all userA's ratings from ratings
            foreach (User u in users)
            {
                float answer;
                if(similarityScoreType==SimilarityScore.Pearson)
                {
                    answer = Pearson(userA, u);
                }
                else { answer = Euclidian(userA, u); }
                
                u.similarity = answer;
            }

            ratings.RemoveAll(
                 delegate (Rating r)
                 {
                     return r.userID == userID;
                 });

            CalculateWeightedScores();
            CalculateWeightedSums();
            CalculateSimilaritySums();
            CalculateTotal();

            //Then remove all these films from the ratings - we don't want to recommned a film already seen

            foreach (Rating userARating in userARatings)
            {
                weightedSums.RemoveAll(
                        delegate (WeightedSum wS)
                        {
                            return wS.filmName == userARating.filmName;
                        }
                        );
            }

            return weightedSums.OrderByDescending(rating => rating.totalSum_sim).ToList<WeightedSum>();
        }

        public float Euclidian(User A, User B)
        {
            //Init variables
            float sim = 0;
            //Counter for number of matching products
            int n = 0;
            //Iterate over all rating combinations


            List<Rating> userARatings = ratings.FindAll(
                delegate (Rating r)
                {
                    return r.userID == A.userID;
                }
            );

            List<Rating> userBRatings = ratings.FindAll(
               delegate (Rating r)
               {
                   return r.userID == B.userID;
               }
           );

            foreach (Rating rA in userARatings)
                foreach (Rating rB in userBRatings)
                    if (rA.filmName == rB.filmName)
                        sim += (rA.score - rB.score) * (rA.score - rB.score); //a*a
            n += 1;
            //No ratings in common – return 0
            if (n == 0) return 0;
            //Calculate inverted score
            float inv = 1 / (1 + sim);
            return inv;
        }

        public float Pearson(User A, User B)
        {
            //Init variables
            float sum1 = 0, sum2 = 0, sum1sq = 0, sum2sq = 0, pSum = 0;

            //Counter for number of matching products
            int n = 0;
            //Iterate over all rating combinations

            List<Rating> userARatings = ratings.FindAll(
                delegate (Rating r)
                {
                    return r.userID == A.userID;
                }
            );

            List<Rating> userBRatings = ratings.FindAll(
               delegate (Rating r)
               {
                   return r.userID == B.userID;
               }
           );

            foreach (Rating rA in userARatings)
                foreach (Rating rB in userBRatings)
                    if (rA.filmName == rB.filmName)
                    {
                        sum1 += rA.score;
                        sum2 += rB.score;
                        sum1sq += rA.score * rA.score; //rA*rA
                        sum2sq += rB.score * rB.score; //rB*rB
                        pSum += rA.score * rB.score;
                        n += 1;
                    }

            //No ratings in common – return 0
            if (n == 0) return 0;
            //Calculate Pearson
            float num = pSum - ((sum1 * sum2) / n);
            float den = (float)Math.Sqrt((sum1sq - (sum1 * sum1 / n)) * (sum2sq - (sum2 * sum2 / n)));
            return num / den;
        }

        private void CalculateWeightedScores()
        {
            foreach (User u in users)
            {
                foreach (Rating r in ratings)
                {
                    if (r.userID == u.userID)
                    {
                        Rating newRating = new Rating();
                        newRating.userID = r.userID;
                        newRating.filmName = r.filmName;
                        newRating.score = r.score * u.similarity;
                        weightedRatings.Add(newRating);
                    }
                }

            }


        }

        private void CalculateWeightedSums()
        {
            foreach (Rating r in weightedRatings)
            {
                WeightedSum w = weightedSums.Find(
                    delegate (WeightedSum wS)
                    {
                        return wS.filmName == r.filmName;
                    }
                    );
                if (w == null)
                {
                    w = new WeightedSum();
                    w.filmName = r.filmName;
                    w.sum = r.score;
                    weightedSums.Add(w);
                }
                else
                {
                    w.sum += r.score;
                }
            }
        }

        private void CalculateSimilaritySums()
        {
            foreach (WeightedSum wS in weightedSums)
            {
                //Get list of film ratings for this particular film
                List<Rating> rList = ratings.FindAll(
                     delegate (Rating rating)
                     {
                         return rating.filmName == wS.filmName;
                     }
                    );

                foreach (Rating r in rList)
                {
                    if (r.score != 0)
                    {
                        User user = users.Find(
                            delegate (User u)
                            {
                                return u.userID == r.userID;
                            }
                        );
                        wS.similaritySum += user.similarity;
                    }
                }

            }
        }

        private void CalculateTotal()
        {
            foreach (WeightedSum ws in weightedSums)
            {
                ws.totalSum_sim = ws.sum / ws.similaritySum;
            }
        }

    }

}


public enum SimilarityScore { Euclidean, Pearson }

public class WeightedSum
{
    public String filmName;
    public float sum;
    public float similaritySum;
    public float totalSum_sim;

    public override String ToString()
    {
        return String.Format($"Film Name:{filmName}\tSum:{sum}\t Similarity Score:{similaritySum}\tTotal:{totalSum_sim}");
    }
}
