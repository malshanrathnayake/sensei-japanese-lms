using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public enum FeatherIconEnum
    {
        // General notification
        [Display(Name = "bell")]
        Bell,
        [Display(Name = "bell-off")]
        BellOff,
        [Display(Name = "information")]
        Info,

        // Success
        [Display(Name = "check")]
        Check,
        [Display(Name = "check-circle")]
        CheckCircle,
        [Display(Name = "check-square")]
        CheckSquare,

        // Error / failure
        [Display(Name = "x")]
        X,
        [Display(Name = "x-circle")]
        XCircle,
        [Display(Name = "alert-circle")]
        AlertCircle,
        [Display(Name = "alert-triangle")]
        AlertTriangle,

        // Warning
        [Display(Name = "alert-octagon")]
        AlertOctagon,

        // Status / activity
        [Display(Name = "clock")]
        Clock,
        [Display(Name = "loader")]
        Loader,
        [Display(Name = "refresh-cw")]
        RefreshCw,

        // Message / communication
        [Display(Name = "message-circle")]
        MessageCircle,
        [Display(Name = "message-square")]
        MessageSquare,
        [Display(Name = "mail")]
        Mail,

        // Attention
        [Display(Name = "flag")]
        Flag,
        [Display(Name = "eye")]
        Eye,
        [Display(Name = "eye-off")]
        EyeOff
    }
}
