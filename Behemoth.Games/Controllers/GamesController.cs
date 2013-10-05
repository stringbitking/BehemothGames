using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Behemoth.Games.Models;
using Behemoth.Models;

namespace Behemoth.Games.Controllers
{
    [Authorize]
    public class GamesController : BaseController
    {
        //
        // GET: /Games/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetFavouriteGames(int gameId)
        {
            var user = this.Data.Users.All().FirstOrDefault(usr => usr.UserName == User.Identity.Name);
            var game = this.Data.Games.GetById(gameId);
            user.FavouriteGames.Remove(game);
            this.Data.SaveChanges();

            return PartialView("_FavouriteGames", user.FavouriteGames);
        }

        public JsonResult GetAutocompleteData(string text)
        {
            var selectedBooks = this.Data.Games.All()
                .Where(x => x.Name.ToLower().Contains(text.ToLower()))
                .Select(GameViewModel.FromGame);

            return Json(selectedBooks, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            var game = this.Data.Games.GetById(id);
            var user = this.Data.Users.All().FirstOrDefault(usr => usr.UserName == User.Identity.Name);

            var gameModel = new GameViewModel()
            {
                Id = game.Id,
                Name = game.Name,
                ImageUrl = game.ImageUrl,
                ScriptUrl = game.ScriptUrl,
                Description = game.Description,
                CategoryName = game.Category.Name,
                Votes = game.Votes
            };

            if (user.FavouriteGames.Contains(game))
            {
                gameModel.IsFavourite = true;
            }
            else
            {
                gameModel.IsFavourite = false;
            }

            return View(gameModel);
        }

        public ActionResult Play(int id, int? roomId)
        {
            var game = this.Data.Games.GetById(id);

            if (roomId.HasValue)
            {
                ViewBag.RoomId = roomId.Value;
            }

            return View(game);
        }

        public ActionResult PlayMultiplayer(int roomId, int gameId)
        {
            var room = this.Data.GameRooms.GetById(roomId);
            ViewBag.RoomId = roomId;

            if (room.Status == RoomStatus.Empty)
            {
                room.Player1Id = User.Identity.Name;
                room.Status = RoomStatus.Waiting;
            }
            else
            {
                if (room.Status == RoomStatus.Waiting)
                {
                    room.Player2Id = User.Identity.Name;
                    room.Status = RoomStatus.Active;
                }
                else
                {
                    return HttpNotFound();
                }
            }

            this.Data.SaveChanges();

            return RedirectToAction("Play", new { id = gameId, roomId = roomId });
        }

        public ActionResult Rooms(int id)
        {
            ViewBag.GameId = id;
            var game = this.Data.Games.GetById(id);

            if (!game.Rooms.Any(r => r.Status == RoomStatus.Empty))
            {
                game.Rooms.Add(new GameRoom()
                {
                    Name = "Default",
                    Status = RoomStatus.Empty
                });

                this.Data.SaveChanges();
            }

            var roomEntities = game.Rooms.Where(r => r.Status == RoomStatus.Empty ||
                                                        r.Status == RoomStatus.Waiting);
            List<RoomViewModel> rooms = new List<RoomViewModel>();

            foreach (var room in roomEntities)
            {
                rooms.Add(new RoomViewModel()
                {
                    Id = room.Id,
                    Status = room.Status,
                    Name = room.Name,
                    Password = room.Password,
                    Player1Id = room.Player1Id,
                    Player2Id = room.Player2Id
                });
            }

            return View(rooms);
        }
	}
}