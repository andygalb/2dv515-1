using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSystem
{
    public interface IRecommendationService
    {
        ICollection<WeightedSum> GetRecommendations(String userID, IUserService userService, SimilarityScore similarityScoreType);
    }

}
