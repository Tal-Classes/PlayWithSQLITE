# 🔌 שלב 2: התחברות למסד הנתונים ומימוש פעולות CRUD בסיסיות

בשלב זה נלמד כיצד לפתוח חיבור אסינכרוני למסד הנתונים של SQLite ב-.NET MAUI, כיצד לבנות את הטבלאות, וכיצד לממש את פעולות הבסיס: **C**reate, **R**ead, **U**pdate, **D**elete.

---

## 1. היכן נשמר קובץ מסד הנתונים במכשיר?

במכשיר נייד (Android/iOS) לכל אפליקציה יש תיקייה מבודדת (Sandbox) השמורה עבורה.  
ב-.NET MAUI ניגשים לתיקייה זו בעזרת:
```csharp
string dbPath = Path.Combine(FileSystem.AppDataDirectory, "SchoolLms.db3");
```
> ⚠️ **זהירות:** לעולם אין להשתמש בנתיבים קשיחים (כגון `C:\mydb.db3` או `/data/mydb.db3`), מכיוון שנתיבים אלו משתנים לחלוטין בין מכשירי Android שונים, אמולטורים ומכשירי iOS.

---

## 2. יצירת החיבור ויצירת הטבלאות

במחלקה `DatabaseService` נגדיר אובייקט מסוג `SQLiteAsyncConnection`:
```csharp
_database = new SQLiteAsyncConnection(_dbPath, 
    SQLiteOpenFlags.ReadWrite | 
    SQLiteOpenFlags.Create | 
    SQLiteOpenFlags.SharedCache);
```

### פקודת `CreateTableAsync<T>()`:
פקודה זו היא **אידמפוטנטית** (`Idempotent`):
- אם הטבלה עדיין אינה קיימת — SQLite מייצר אותה לפי הגדרות המודל והקישוטים.
- אם הטבלה כבר קיימת — הפקודה אינה מוחקת נתונים ואינה זורקת שגיאה (אך בודקת האם נוספו עמודות חדשות).

```csharp
public async Task InitializeAsync()
{
    if (_database != null) return;

    _database = new SQLiteAsyncConnection(_dbPath, ...);

    await _database.CreateTableAsync<Teacher>();
    await _database.CreateTableAsync<Course>();
    await _database.CreateTableAsync<Student>();
    await _database.CreateTableAsync<Enrollment>();
}
```

---

## 3. ארבע פעולות ה-CRUD ב-SQLite

### 1. Create (יצירה / הוספה) — `InsertAsync`
הוספת רשומה חדשה לטבלה. אם השדה `Id` מסומן ב-`[AutoIncrement]`, מסד הנתונים יקצה עבורו מזהה ייחודי חדש ומעדכן את האובייקט בזיכרון:
```csharp
public async Task<int> AddTeacherAsync(Teacher teacher)
{
    await EnsureInitializedAsync();
    // הפונקציה מחזירה את מספר השורות שהושפעו (בדרך כלל 1)
    return await _database.InsertAsync(teacher);
}
```

### 2. Read (קריאה / שליפה) — `Table<T>()` ו-LINQ
שליפת נתונים מתבצעת בעזרת שיטת `Table<T>()` ומאפשרת שימוש מלא בפקודות LINQ:
```csharp
// שליפת כל המורים ממוינים לפי שם
public async Task<List<Teacher>> GetTeachersAsync()
{
    await EnsureInitializedAsync();
    return await _database.Table<Teacher>()
                          .OrderBy(t => t.FullName)
                          .ToListAsync();
}

// שליפת רשומה בודדת לפי מזהה (Id)
public async Task<Teacher?> GetTeacherByIdAsync(int id)
{
    await EnsureInitializedAsync();
    return await _database.Table<Teacher>()
                          .FirstOrDefaultAsync(t => t.Id == id);
}
```

### 3. Update (עדכון) — `UpdateAsync`
עדכון רשומה קיימת. SQLite מוצא את הרשומה לפי ה-`PrimaryKey` שלה (`Id`) ומעדכן את שאר העמודות:
```csharp
public async Task<int> UpdateTeacherAsync(Teacher teacher)
{
    await EnsureInitializedAsync();
    return await _database.UpdateAsync(teacher);
}
```

> **דפוס עבודה נפוץ (Upsert):**  
> נהוג לאחד את `Add` ו-`Update` למתודה אחת `SaveTeacherAsync`:
> ```csharp
> public async Task<int> SaveTeacherAsync(Teacher teacher)
> {
>     await EnsureInitializedAsync();
>     if (teacher.Id != 0)
>         return await _database.UpdateAsync(teacher);
>     return await _database.InsertAsync(teacher);
> }
> ```

### 4. Delete (מחיקה) — `DeleteAsync`
מחיקת רשומה מהטבלה לפי האובייקט או לפי ה-`Id`:
```csharp
public async Task<int> DeleteTeacherAsync(Teacher teacher)
{
    await EnsureInitializedAsync();
    return await _database.DeleteAsync(teacher);
}
```

---

## 4. כלל הברזל של ביצועים ב-UI: שימוש ב-Async
בכל פנייה למסד הנתונים:
- ✅ **חובה** להשתמש ב-`await` ובשיטות אסינכרוניות (`ToListAsync`, `InsertAsync`, `UpdateAsync`).
- ❌ **אסור בהחלט** לחסום את תהליכון המשתמש (UI Thread) בעזרת `.Result` או `.Wait()`, דבר שגורם לקפיאת המסך (ANR - Application Not Responding) במכשיר.

עברו לשלב הבא: [03-Table-Relationships.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/03-Table-Relationships.md).
