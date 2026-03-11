using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public enum FeatherIconEnum
    {
        // General notification
        [Display(Name = "Bell")]
        Bell,
        [Display(Name = "Bell-Off")]
        BellOff,
        [Display(Name = "Information")]
        Info,

        // Success
        [Display(Name = "Check")]
        Check,
        [Display(Name = "Check-Circle")]
        CheckCircle,
        [Display(Name = "Check-Square")]
        CheckSquare,

        // Error / failure
        [Display(Name = "X")]
        X,
        [Display(Name = "X-Circle")]
        XCircle,
        [Display(Name = "Alert-Circle")]
        AlertCircle,
        [Display(Name = "Alert-Triangle")]
        AlertTriangle,

        // Warning
        [Display(Name = "Alert-Octagon")]
        AlertOctagon,

        // Status / activity
        [Display(Name = "Clock")]
        Clock,
        [Display(Name = "Loader")]
        Loader,
        [Display(Name = "Refresh")]
        RefreshCw,

        // Message / communication
        [Display(Name = "Message-Circle")]
        MessageCircle,
        [Display(Name = "Message-Square")]
        MessageSquare,
        [Display(Name = "Mail")]
        Mail,

        // Attention
        [Display(Name = "Flag")]
        Flag,
        [Display(Name = "Eye")]
        Eye,
        [Display(Name = "Eye-Off")]
        EyeOff
    }
}
