using System;

namespace DO;

/// <summary>
/// Data-layer representation of an order. Validation (address existence, coordinate calculation,
/// format checks, id sequencing, etc.) must be performed in the business/logical layer; the DAL
/// assumes values it receives are already valid.
/// </summary>
internal record Order(
    int Id,                         // מספר הזמנה ייחודי (לא ניתן לעדכון)
    OrderType Type,                 // סוג הזמנה (נקבע/מנוהל על ידי מנהל)
       string Address,                 // כתובת מלאה תקינה (DAL מכיל רק הזמנות עם כתובת תקינה)
    double Latitude,                // קו רוחב (נקבע ע"י השכבה הלוגית)
    double Longitude,               // קו אורך (נקבע ע"י השכבה הלוגית)
    string CustomerName,            // שם מלא של המזמין
    string CustomerPhone,           // טלפון סלולרי של המזמין (10 ספרות, מתחיל ב-'0')
        DateTime OpenedAt,             // זמן פתיחת ההזמנה (נוצר ע"י מערכת, לא ניתן לעדכון)

    string? Description = null,     // תיאור מילולי קצר (יכול להיות null; עריכה ע"י מנהל)
  double? Weight = null,          // משקל (ק"ג) - אופציונלי, לפי שיקול החברה
    double? Volume = null,          // נפח (מ"ק) - אופציונלי
    bool IsFragile = false         // נתון תוספתי לדוגמה
);


