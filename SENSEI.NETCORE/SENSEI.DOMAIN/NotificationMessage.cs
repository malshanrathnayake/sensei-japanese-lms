using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace SENSEI.DOMAIN
{
    public class NotificationMessage
    {
        [DisplayName("Type")]
        public string Type { get; set; } = "success"; // success, error, warning, info

        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Duration")]
        public int Duration { get; set; } = 3000;

        [DisplayName("Dismissible")]
        public bool Dismissible { get; set; } = true;

        [DisplayName("Position X")]
        public string PositionX { get; set; } = "right";

        [DisplayName("Position Y")]
        public string PositionY { get; set; } = "top";
    }
}
