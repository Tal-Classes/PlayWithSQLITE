namespace PlayWithSQLITE;

/// <summary>
/// מחלקה סטטית לריכוז קבועי ההגדרה של מסד הנתונים (מיקום הקובץ והרשאות הפתיחה).
/// ראו מדריך 02: מחלקת Constants, הבנת IDatabaseService ומימוש CRUD.
/// </summary>
public static class Constants
{
    // 💡 שלב 2 - שם קובץ מסד הנתונים:
    // הגדירו כאן קבוע (const string) בשם DatabaseFilename עבור שם קובץ ה-SQLite (למשל "SchoolLms.db3").

    // 💡 שלב 2 - הרשאות ודגלי פתיחה (SQLiteOpenFlags):
    // לאחר שהתקנתם את חבילת sqlite-net-pcl (בשלב 1), הוסיפו בראש הקובץ using למרחב השמות של SQLite.
    // הגדירו כאן קבוע בשם Flags (מטיפוס SQLiteOpenFlags) שמשלב בעזרת אופרטור Bitwise OR (|) שלושה דגלים:
    // 1. פתיחה לקריאה ולכתיבה (ReadWrite)
    // 2. יצירת הקובץ אוטומטית אם אינו קיים עדיין (Create)
    // 3. הפעלת מטמון משותף לגישה מרובת-תהליכונים בטוחה (SharedCache)

    // 💡 שלב 2 - הנתיב המלא לקובץ במכשיר:
    // הגדירו מאפיין סטטי DatabasePath שמחבר בצורה חוצת-פלטפורמות (Path.Combine)
    // בין תיקיית הנתונים המבודדת של האפליקציה ב-MAUI (איזו תכונה ב-FileSystem מספקת זאת?) לבין DatabaseFilename.
}
