using Behemoth.Data;
using Behemoth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Behemoth.Games.Controllers
{
    public class GamesApiController : ApiController
    {
        protected IUowData Data { get; set; }

        public GamesApiController()
            :this(new UowData())
        {

        }

        [ActionName("register")]
        public string Get()
        {
            //return Guid.NewGuid().ToString();
            return User.Identity.Name;
        }

        public GamesApiController(IUowData data)
        {
            this.Data = data;
        }

        [ActionName("Favourite")]
        public HttpResponseMessage GetFavouriteGame(int id)
        {
            var game = this.Data.Games.GetById(id);
            var user = this.Data.Users.All().FirstOrDefault(usr => usr.UserName == User.Identity.Name);

            if (user != null)
            {
                user.FavouriteGames.Add(game);
                this.Data.SaveChanges();
            }

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [ActionName("VoteForGame")]
        [HttpGet]
        public HttpResponseMessage PostVoteForGame(int id, int vote)
        {
            var game = this.Data.Games.GetById(id);
            var user = this.Data.Users.All().FirstOrDefault(usr => usr.UserName == User.Identity.Name);

            if (user != null)
            {
                var voteEntity = this.Data.Votes.All().FirstOrDefault(v => v.Game.Id == game.Id && v.User.Id == user.Id);
                if (voteEntity == null)
                {
                    voteEntity = new Vote
                    {
                        Stars = vote,
                        User = user,
                        Game = game
                    };

                    this.Data.Votes.Add(voteEntity);
                    this.Data.SaveChanges();
                }
                else
                {
                    voteEntity.Stars = vote;
                    this.Data.SaveChanges();
                }
            }

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        [ActionName("GameVotes")]
        public HttpResponseMessage GetGameVotes(int id)
        {
            var game = this.Data.Games.GetById(id);
            var votes = this.Data.Votes.All().Where(v => v.Game.Id == game.Id);
            var user = this.Data.Users.All().FirstOrDefault(usr => usr.UserName == User.Identity.Name);
            var userVote = votes.FirstOrDefault(v => v.User.Id == user.Id);
            double stars = 0;
            if (userVote != null)
            {
                stars = userVote.Stars;
            }

            double votesSum = 0;
            foreach (var vote in votes)
            {
                votesSum += vote.Stars;
            }
            double averageVote = 0;
            averageVote = votesSum / votes.Count();

            var response = this.Request.CreateResponse(HttpStatusCode.OK, new { averageVote = averageVote, stars = stars });
            return response;
        }
    }
}
