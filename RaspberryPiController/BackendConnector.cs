using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RaspberryPiController
{
    public enum MovementType
    {
        Forward,
        Backward
    }

    public enum TurnType
    {
        Left,
        Right
    }

    public class Position
    {
        public Position(double xPos, double yPos, double rotationDegrees)
        {
            XPos = xPos;
            YPos = yPos;
            RotationDegrees = rotationDegrees;
        }

        public double XPos { get; set; }
        public double YPos { get; set; }
        public double RotationDegrees { get; set; }
    }

    public static class BackendConnector
    {
        private const string LocalUrl = "http://localhost:9876";
        private const string RemoteUrl = "";
        private const string BackendUrl = LocalUrl;
        private static readonly HttpClient Client = new HttpClient();
        private static Position current { get; } = new Position(0, 0, 0);
        private static bool blinking { get; set; } = false;
        
        public static void Move(double amount, MovementType type)
        {
            amount = Math.Round(amount, 2);
            if (type == MovementType.Forward)
            {
                current.XPos += Math.Sin((Math.PI / 180) * current.RotationDegrees) * amount;
                current.YPos += Math.Cos((Math.PI / 180) * current.RotationDegrees) * amount;
            }
            else
            {
                current.XPos -= Math.Sin((Math.PI / 180) * current.RotationDegrees) * amount;
                current.YPos -= Math.Cos((Math.PI / 180) * current.RotationDegrees) * amount;
            }
        }

        public static void Turn(double amount, TurnType type)
        {
            amount = Math.Round(amount, 2);
            if (type == TurnType.Right) {
                current.RotationDegrees += amount;
                if (current.RotationDegrees >= 360) current.RotationDegrees -= 360;
            } else {
                current.RotationDegrees -= amount;
                if (current.RotationDegrees <= -360) current.RotationDegrees += 360;
            }
        }

        public static void Blink()
        {
            blinking = true;
            new Thread(() =>
            {
                Thread.Sleep(1500);
                blinking = false;
            }).Start();
        }

        public static bool IsBlinking()
        {
            return blinking;
        }

        public static Position GetPosition()
        {
            return current;
        }
    }
}