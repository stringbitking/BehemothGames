using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Behemoth.Data;
using System.Threading.Tasks;
using Behemoth.Games.SignalRModels;
using Behemoth.Models;

namespace Behemoth.Games
{
    public class SpaceHub : Hub
    {
        //public async Task JoinRoom(string roomName)
        //{
        //    Clients.All.addMessage("Joining room");
        //    await Groups.Add(Context.ConnectionId, roomName);
        //    Clients.Group(roomName).addMessage(Context.User.Identity.Name + " joined.");
        //}

        //public void JoinGameRoom(int roomId)
        //{
        //    var taskResult = JoinRoom("room" + roomId);
        //    taskResult.Wait();
        //}

        public void JoinRoom(string roomName)
        {

            Clients.All.addMessage("Joining...");
            Groups.Add(Context.ConnectionId, roomName).Wait();
            Clients.Group(roomName).addMessage(Context.User.Identity.Name + " joined.");
        }

        public void JoinGameRoom(int roomId)
        {
            JoinRoom("room" + roomId);

        }

        public void SendShip(PlayerShip obj, int roomId)
        {
            DataContext ctx = new DataContext();
            var room = ctx.GameRooms.FirstOrDefault(r => r.Id == roomId);
            bool shouldStartGame = false;
            if (room.Status == RoomStatus.Active)
            {
                shouldStartGame = true;
            }

            //if (room != null)
            //{
            //    if (room.Player1Id == null || room.Player1Id.Length == 0)
            //    {
            //        room.Player1Id = obj.id;
            //        room.Status = RoomStatus.Waiting;
            //    }
            //    else
            //    {
            //        room.Player2Id = obj.id;
            //        shouldStartGame = true;
            //    }

            //    ctx.SaveChanges();
            //}

            if (shouldStartGame)
            {
                // Start game
                room.Status = RoomStatus.Active;
                ctx.SaveChanges();
                Clients.Group("room" + roomId).startGame();
            }

            // Call the broadcastMessage method to update clients.
            //Clients.Others.toAllRegisterShip(obj);
            Clients.Group("room" + roomId, Context.ConnectionId).toAllRegisterShip(obj);

            if (!shouldStartGame)
            {
                Clients.Caller.makeMainPlayer();
            }
        }

        public void RefreshShip(object obj, int roomId)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Group("room" + roomId, Context.ConnectionId).refreshShipPosition(obj);
        }

        public void RefreshBall(object obj, int roomId)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Group("room" + roomId, Context.ConnectionId).refreshBallPosition(obj);
        }

        public void RefreshInvaders(object obj, int roomId, string playerId)
        {
            DataContext ctx = new DataContext();
            var room = ctx.GameRooms.FirstOrDefault(r => r.Id == roomId);

            if (room != null)
            {
                if (room.Player1Id == playerId)
                {
                    // Call the broadcastMessage method to update clients.
                    Clients.Group("room" + roomId, Context.ConnectionId).refreshInvadersPosition(obj);
                }
            }

        }

        public void CheckWin(int roomId)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Group("room" + roomId).displayWinner();
        }

        public void FireBullet(object obj, int roomId)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Group("room" + roomId, Context.ConnectionId).fireBulletAll(obj);
        }

        public void FireProjectile(object obj, int roomId)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Group("room" + roomId, Context.ConnectionId).fireProjectileAll(obj);
        }
    }
}