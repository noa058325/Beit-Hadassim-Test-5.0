import pandas as pd
from collections import defaultdict
import os


# 1.
def read_and_validate_data(file_path):
    # קריאה והמרה של נתונים
    df = pd.read_csv(file_path, parse_dates=['timestamp'], dayfirst=True)

    # המרה של עמודת value למספרים חובה כדי לא להיתקע בהשוואות
    df['value'] = pd.to_numeric(df['value'], errors='coerce')

    # סינון כפילויות וערכים חסרים
    df.drop_duplicates(inplace=True)
    df.dropna(inplace=True)

    # בדיקה לערכים שליליים
    if (df['value'] < 0).any():
        print("אזהרה: נמצאו ערכים שליליים בטור value")

    return df


# 2.
def compute_hourly_averages(df):
    df['hour'] = df['timestamp'].dt.floor('h')
    hourly_avg = df.groupby('hour')['value'].mean().reset_index()
    hourly_avg.rename(columns={'hour': 'timestamp', 'value': 'mean_value'}, inplace=True)
    return hourly_avg


def split_by_day_and_process(df):
    #  חיבור הנתונים לאחר חיתוך לפי ימים
    df['date'] = df['timestamp'].dt.date
    results = []

    for date_val, daily_df in df.groupby('date'):  # חלוקה לפי ימים
        daily_df['hour'] = daily_df['timestamp'].dt.floor('h')
        avg = daily_df.groupby('hour')['value'].mean().reset_index()
        avg.rename(columns={'hour': 'timestamp', 'value': 'mean_value'}, inplace=True)
        results.append(avg)

        # הוספת שורת סיכום עם ממוצע כללי של היום
        mean_val = daily_df['value'].mean()
        summary_row = pd.DataFrame([{'timestamp': f'{date_val} total', 'value': mean_val}])
        daily_df_with_total = pd.concat([daily_df, summary_row], ignore_index=True)

        # שמירה של הקובץ עם השורה הנוספת
        daily_df_with_total.to_csv(f'time_series_{date_val}.csv', index=False)

    return pd.concat(results)


# 3.
def simulate_streaming_processing(df):
    # נעדכן ממוצע תוך כדי זרימה של שורות
    sums = defaultdict(float)
    counts = defaultdict(int)
    streaming_results = []

    for _, row in df.iterrows():
        ts = row['timestamp']
        val = row['value']
        hour = ts.replace(minute=0, second=0, microsecond=0)
        sums[hour] += val
        counts[hour] += 1
        avg = sums[hour] / counts[hour]
        streaming_results.append({'timestamp': hour, 'mean_value': avg})

    return pd.DataFrame(streaming_results)


# 4.
# Parquet נותן ביצועים טובים יותר מ-CSV כשעובדים עם הרבה נתונים.
# הוא שומר את המידע לפי עמודות, ככה שאפשר לקרוא רק את מה שצריך – וזה חוסך זמן.
# הקבצים עצמם הרבה יותר דחוסים, אז גם טעינה וגם אחסון עובדים מהר יותר.
# בנוסף הוא שומר טיפוסים בצורה מדויקת, אז פחות מתעסקים בתיקוני המרות או שגיאות.
def process_parquet_file(parquet_path):
    df = pd.read_parquet(parquet_path)
    return compute_hourly_averages(df)


def main():
    input_csv = 'time_series.csv'
    input_parquet = 'time_series.parquet'

    print("טוען את הקובץ המקורי...")
    df = read_and_validate_data(input_csv)
    print("הקובץ נטען ונבדק בהצלחה (בלי כפילויות/שגיאות פורמט)")

    print("מחשב ממוצעים לפי שעה...")
    hourly_avg = compute_hourly_averages(df)
    hourly_avg.to_csv('hourly_avg_from_full_data.csv', index=False)
    print("נשמר hourly_avg_from_full_data.csv")

    print("מחלק את הנתונים לפי ימים...")
    combined_hourly = split_by_day_and_process(df)
    combined_hourly.to_csv('hourly_means_combined.csv', index=False)
    print("נשמר hourly_means_combined.csv - ממוצעים לפי ימים")

    if os.path.exists(input_parquet):
        print("קיים קובץ Parquet – מעבד גם אותו...")
        parquet_avg = process_parquet_file(input_parquet)
        parquet_avg.to_csv('parquet_hourly_avg.csv', index=False)
        print("נשמר parquet_hourly_avg.csv")

    print("סיימנו! כל הקבצים נוצרו בהצלחה.")


if __name__ == '__main__':
    main()
