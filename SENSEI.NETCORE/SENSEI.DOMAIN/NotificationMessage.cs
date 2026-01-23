using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SENSEI.DOMAIN
{
    public class NotificationMessage
    {
        public string Type { get; set; } = "success"; // success, error, warning, info

        public string Message { get; set; }

        public int Duration { get; set; } = 3000;

        public bool Dismissible { get; set; } = true;

        public string PositionX { get; set; } = "right";

        public string PositionY { get; set; } = "top";
    }
}
