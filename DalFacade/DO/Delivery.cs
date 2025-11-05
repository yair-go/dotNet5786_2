
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO;
/// <summary>
/// Linking entity between Order and Courier: represents a courier handling the order (a delivery).
/// Validation and sequencing of Ids performed by the logical/configuration layer.
/// </summary>
internal record Delivery(
    int Id,                         // מזהה ישות המשלוח הייחודי (לא ניתן לעדכון)
    int OrderId,                    // מזהה ההזמנה שאליה משתייך המשלוח
    int CourierId,                  // ת"ז של השליח (0 עבור "משלוח מדומה" שנוצר בעקבות ביטול)
    DeliveryType DeliveryType,      // סוג השילוח בעת יצירת המשלוח (נשמר כערך היסטורי)
    DateTime StartTime,             // זמן תחילת המשלוח (נוצר ע"י מערכת, לא ניתן לעדכון)
    double? ActualDistance = null,  // מרחק בפועל (ק"מ) - מחושב בשכבה הלוגית; null עד לסיום חישוב
    DeliveryEndType? EndType = null,// סוג סיום המשלוח (null אם המשלוח עדיין בעשייה)
    DateTime? EndTime = null        // זמן סיום המשלוח (null אם לא הסתיים)
);
